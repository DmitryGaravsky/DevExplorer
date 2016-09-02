namespace DevExplorer.ViewModels {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using DevExplorer.DataModel;
    using DevExplorer.DataModel.Extensions;
    using DevExplorer.DataServices;
    using DevExpress.Mvvm;
    using DevExpress.Mvvm.POCO;
    using System.Collections.ObjectModel;

    public class NewFolderViewModel : IDocumentContent {
        protected IDocumentManagerService DocumentManagerService {
            get { return this.GetRequiredService<IDocumentManagerService>(); }
        }
        protected IFolderBrowserDialogService FolderBrowserDialogService {
            get { return this.GetRequiredService<IFolderBrowserDialogService>(); }
        }
        protected IExplorerDataModel DataModel {
            get { return this.GetParentViewModel<MainViewModel>().DataModel; }
        }
        //
        public virtual bool IsLoading {
            get;
            protected set;
        }
        public virtual IEnumerable<Folder> Folders {
            get;
            protected set;
        }
        public Task OnLoad() {
            Messenger.Default.Register<RequerySuggested<Folder>>(this, OnRequerySuggested);
            IsLoading = true;
            var dispatcherService = this.GetService<IDispatcherService>();
            return Task.Factory.StartNew((state) =>
            {
                var folders = BuildsFolders();
                ((IDispatcherService)state).BeginInvoke(() =>
                {   // Perform updates on UI thread
                    Folders = folders;
                    IsLoading = false;
                });
            }, dispatcherService);
        }
        ReadOnlyCollection<Folder> BuildsFolders() {
            var folders = DataModel.GetFolders();
            Parallel.ForEach(folders, f =>
            {
                f.Image = !f.IsNew() ? DataModel.Cache.GetLargeImage(f.Path) : DevExplorer.Properties.Resources.NewFolder;
            });
            return folders;
        }
        public bool CanOpenOrCreate(Folder folder) {
            return (folder != null);
        }
        public void OpenOrCreate(Folder folder) {
            if(folder.IsNew()) {
                if(FolderBrowserDialogService.ShowDialog())
                    DataModel.OnNewFolder(FolderBrowserDialogService.ResultPath);
                return;
            }
            IDocument document = DocumentManagerService.FindDocumentByIdOrCreate(folder.Path,
                svc => svc.CreateDocument("FolderView", folder, this.GetParentViewModel<MainViewModel>()));
            document.Id = folder.Path;
            document.Show();
            DataModel.OnFolderOpen(folder);
        }
        public bool CanRename(Folder folder) {
            return (folder != null) && !folder.IsNewOrSpecial();
        }
        public void Rename(Folder folder) {
            DataModel.OnFolderChanged(folder); // TODO
        }
        #region Requery
        public Folder RequeryFolder { // MANUAL NOTIFICATIONS
            get;
            private set;
        }
        void OnRequerySuggested(RequerySuggested<Folder> message) {
            var dispatcherService = this.GetService<IDispatcherService>();
            var requery = message.IsSingleEntry ?
                GetFolderRequery(message.Entry) : GetFoldersRequery();
            dispatcherService.BeginInvoke(requery);
        }
        Action GetFolderRequery(Folder folder) {
            return () =>
            {
                RequeryFolder = Folders.FirstOrDefault(f => f.Path == f.Path);
                if(RequeryFolder != null)
                    this.RaisePropertyChanged(x => x.RequeryFolder);
                RequeryFolder = null;
            };
        }
        Action GetFoldersRequery() {
            return () => Folders = BuildsFolders();
        }
        #endregion
        #region IDocumentContent
        IDocumentOwner IDocumentContent.DocumentOwner {
            get;
            set;
        }
        void IDocumentContent.OnClose(CancelEventArgs e) {
        }
        void IDocumentContent.OnDestroy() {
            Messenger.Default.Unregister(this);
        }
        object IDocumentContent.Title {
            get { return "New Folder"; }
        }
        #endregion
    }
}
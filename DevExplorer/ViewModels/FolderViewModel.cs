namespace DevExplorer.ViewModels {
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using DevExplorer.DataModel;
    using DevExplorer.DataModel.Extensions;
    using DevExpress.Mvvm;
    using DevExpress.Mvvm.POCO;

    public class FolderViewModel : IDocumentContent, ISupportParameter {
        protected IDocumentManagerService DocumentManagerService {
            get { return this.GetRequiredService<IDocumentManagerService>(); }
        }
        protected IExplorerDataModel DataModel {
            get { return this.GetParentViewModel<MainViewModel>().DataModel; }
        }
        //
        public virtual object Parameter {
            get;
            set;
        }
        protected void OnParameterChanged() {
            Folder folder = Parameter as Folder;
            title = folder.Name;
            Path = folder.Path;
        }
        public virtual string Path {
            get;
            protected set;
        }
        protected void OnPathChanged(string oldPath) {
            var files = DataModel.GetFolderItems(Path);
            foreach(var file in files)
                file.Image = GetFileImage(file);
            Files = files;
            SelectedItem = FindItem(oldPath);
        }
        FolderItem FindItem(string path) {
            return
                Files.FirstOrDefault(f => f.Path == path) ??
                Files.FirstOrDefault(f => f.IsParentFolder());
        }
        object GetFileImage(FolderItem file) {
            return !file.IsParentFolder() ? DataModel.Cache.GetImage(file.Path) : null;
        }
        public virtual FolderItem SelectedItem {
            get;
            set;
        }
        public virtual IEnumerable<FolderItem> Files {
            get;
            protected set;
        }
        protected virtual void OnFilesChanged() {
            this.RaiseCanExecuteChanged(x => x.Back());
        }
        public bool CanOpen(FolderItem item) {
            return (item != null) && item.IsDirectory;
        }
        public void Open(FolderItem item) {
            Path = item.Path;
        }
        public bool CanBack() {
            return (Files != null) && Files.Any(f => f.IsParentFolder());
        }
        public void Back() {
            var parent = Files.FirstOrDefault(f => f.IsParentFolder());
            if(parent != null) Path = parent.Path;
        }
        #region IDocumentContent
        IDocumentOwner IDocumentContent.DocumentOwner { get; set; }
        void IDocumentContent.OnClose(CancelEventArgs e) { }
        void IDocumentContent.OnDestroy() { }
        string title;
        object IDocumentContent.Title {
            get { return title; }
        }
        #endregion
    }
}
namespace DevExplorer {
    using DevExplorer.DataModel;
    using DevExplorer.DataServices;
    using DevExplorer.Services;
    using DevExpress.Mvvm;
    using DevExpress.Mvvm.POCO;

    public class MainViewModel {
        readonly IExplorerDataModel dataModel = new ExplorerSimpleDataModel(FileSystemImagesCache.Default);
        public IExplorerDataModel DataModel {
            get { return dataModel; }
        }
        protected IDocumentManagerService DocumentManagerService {
            get { return this.GetRequiredService<IDocumentManagerService>(); }
        }
        readonly static object NewFolderID = new object();
        public void NewFolder() {
            IDocument document = DocumentManagerService.CreateDocument("NewFolderView", dataModel, this);
            document.Id = NewFolderID;
            document.Show();
        }
        public void OnLoad() {
            DataModel.Requery += DataModel_Requery;
            if(DocumentManagerService.FindDocumentById(NewFolderID) == null)
                NewFolder();
        }
        public void OnClosed() {
            DataModel.Requery -= DataModel_Requery;
            dataModel.Cache.Reset();
        }
        void DataModel_Requery(object sender, System.EventArgs e) {
            Messenger.Default.Send(sender as RequerySuggested<Folder>);
        }
    }
}
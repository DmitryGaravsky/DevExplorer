namespace DevExplorer.DataServices {
    using System;
    using System.Collections.ObjectModel;
    using DevExplorer.DataModel;

    public class ExplorerCQRSDataModel : IExplorerDataModel {
        readonly DataModel.Services.IImagesCache cache;
        public ExplorerCQRSDataModel(DataModel.Services.IImagesCache cache) {
            this.cache = cache;
        }
        readonly static IQueryFactory<Folder> foldersQueryFactory = new FoldersQueryFactory();
        ReadOnlyCollection<Folder> IExplorerDataModel.GetFolders() {
            return foldersQueryFactory.GetRecentFolders();
        }
        readonly static IQueryFactory<FolderItem> folderItemsQueryFactory = new FolderItemsQueryFactory();
        ReadOnlyCollection<FolderItem> IExplorerDataModel.GetFolderItems(string path) {
            return folderItemsQueryFactory.GetFiles(path);
        }
        //
        readonly static ICommandFactory<Folder> foldersCommandFactory = new FoldersCommandFactory();
        void IExplorerDataModel.OnNewFolder(string path) {
            if(foldersCommandFactory.New(path))
                RaiseRequery(new RequerySuggested<Folder>());
        }
        void IExplorerDataModel.OnFolderOpen(Folder folder) {
            foldersCommandFactory.Recent(folder.Path);
        }
        void IExplorerDataModel.OnFolderChanged(Folder folder) {
            if(foldersCommandFactory.Update(folder.Path, folder.Name))
                RaiseRequery(new RequerySuggested<Folder>(folder));
        }
        public event EventHandler Requery;
        void RaiseRequery(object message) {
            EventHandler handler = Requery;
            if(handler != null) handler(message, EventArgs.Empty);
        }
        //
        DataModel.Services.IImagesCache IExplorerDataModel.Cache {
            get { return cache; }
        }
    }
}
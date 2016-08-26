namespace DevExplorer.DataModel {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using DevExplorer.DataModel.Services;

    public interface IExplorerDataModel {
        ReadOnlyCollection<Folder> GetFolders();
        ReadOnlyCollection<FolderItem> GetFolderItems(string path);
        //
        void OnNewFolder(string path);
        void OnFolderOpen(Folder folder);
        void OnFolderChanged(Folder folder);
        //
        IImagesCache Cache { get; }
        //
        event EventHandler Requery;
    }
}
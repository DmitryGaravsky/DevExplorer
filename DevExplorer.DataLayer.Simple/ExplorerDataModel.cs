namespace DevExplorer.DataServices {
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using DevExplorer.DataModel;
    using DevExplorer.DataModel.Extensions;

    public class ExplorerSimpleDataModel : IExplorerDataModel {
        readonly DataModel.Services.IImagesCache cache;
        public ExplorerSimpleDataModel(DataModel.Services.IImagesCache cache) {
            this.cache = cache;
        }
        List<string> paths = new List<string>();
        IDictionary<string, string> names = new Dictionary<string, string>();
        ReadOnlyCollection<Folder> IExplorerDataModel.GetFolders() {
            return new ReadOnlyCollection<Folder>(GetFolders().ToList());
        }
        readonly static Environment.SpecialFolder[] specialFolders = new Environment.SpecialFolder[] { 
            Environment.SpecialFolder.Desktop,
            Environment.SpecialFolder.ProgramFiles
        };
        IEnumerable<Folder> GetFolders() {
            yield return NewFolder.Instance;
            string startUpPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            Folder startFolder;
            if(TryGetFolder(startUpPath, out startFolder, "StartUp"))
                yield return startFolder;
            foreach(var path in paths) {
                Folder folder;
                if(path != startUpPath && TryGetFolder(path, out folder))
                    yield return folder;
            }
            foreach(var item in specialFolders) {
                Folder special;
                if(item.TryGetSpecialFolder(out special))
                    yield return special;
            }
        }
        bool TryGetFolder(string path, out Folder folder, string name = null) {
            if(FolderExtension.TryGetFolder(path, out folder)) {
                if(name != null)
                    folder.Name = name;
                string customName;
                if(names.TryGetValue(path, out customName))
                    folder.Name = customName;
            }
            return folder != null;
        }
        ReadOnlyCollection<FolderItem> IExplorerDataModel.GetFolderItems(string path) {
            var files = GetFiles(path)
                .OrderByDescending(f => f.IsDirectory)
                .ThenBy(f => f.Name);
            return new ReadOnlyCollection<FolderItem>(files.ToList());
        }
        IEnumerable<FolderItem> GetFiles(string path) {
            var directory = new System.IO.DirectoryInfo(path);
            if(!directory.Exists)
                yield break;
            if((directory.Parent != null) && directory.Parent.Exists)
                yield return new ParentFolder(directory.Parent.FullName);
            foreach(var file in directory.EnumerateFileSystemInfos())
                yield return new FolderItem(file.FullName, CalcIsDirectory(file));
        }
        static bool CalcIsDirectory(System.IO.FileSystemInfo file) {
            return (file.Attributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory;
        }
        //
        void IExplorerDataModel.OnNewFolder(string path) {
            if(!paths.Contains(path)) {
                paths.Add(path);
                RaiseRequery(new RequerySuggested<Folder>());
            }
        }
        void IExplorerDataModel.OnFolderOpen(Folder folder) { }
        void IExplorerDataModel.OnFolderChanged(Folder folder) {
            names.Remove(folder.Path);
            names.Add(folder.Path, folder.Name);
            RaiseRequery(new RequerySuggested<Folder>(folder));
        }
        DataModel.Services.IImagesCache IExplorerDataModel.Cache {
            get { return cache; }
        }
        public event EventHandler Requery;
        void RaiseRequery(object message) {
            EventHandler handler = Requery;
            if(handler != null) handler(message, EventArgs.Empty);
        }
    }
}
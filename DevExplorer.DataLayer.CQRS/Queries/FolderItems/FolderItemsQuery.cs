namespace DevExplorer.DataServices {
    using System.Linq;
    using System.Collections.Generic;
    using DevExplorer.DataModel;

    public interface IFolderItemsQueryCriteria : IQueryCriteria<FolderItem> {
        string Path { get; }
    }
    //
    sealed class FolderItemsQueryCriteria : IFolderItemsQueryCriteria {
        public string Path {
            get;
            set;
        }
    }
    sealed class FolderItemsQuery : IQuery<FolderItem> {
        readonly IFolderItemsQueryCriteria criteria;
        public FolderItemsQuery(IFolderItemsQueryCriteria criteria) {
            this.criteria = criteria;
        }
        public IEnumerable<FolderItem> Execute() {
            return GetFiles(criteria.Path)
                .OrderByDescending(f => f.IsDirectory)
                .ThenBy(f => f.Name);
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
    }
}
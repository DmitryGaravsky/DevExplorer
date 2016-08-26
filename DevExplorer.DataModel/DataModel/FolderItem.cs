namespace DevExplorer.DataModel {
    using System.ComponentModel.DataAnnotations;

    public class FolderItem {
        public FolderItem(string path, bool isDirectory) {
            this.Path = path;
            this.IsDirectory = isDirectory;
            string fileName = this.IsParentFolder() ? ".." : System.IO.Path.GetFileNameWithoutExtension(path);
            this.Name = IsDirectory ? "[" + fileName + "]" : fileName;
            this.Extension = IsDirectory ? "<DIR>" : System.IO.Path.GetExtension(path);
        }
        [Display(AutoGenerateField = false)]
        public bool IsDirectory {
            get;
            private set;
        }
        public string Name {
            get;
            private set;
        }
        public string Extension {
            get;
            private set;
        }
        [Display(AutoGenerateField = false)]
        public string Path {
            get;
            private set;
        }
        [Display(AutoGenerateField = false)]
        public object Image {
            get;
            set;
        }
    }
    public sealed class ParentFolder : FolderItem {
        public ParentFolder(string path)
            : base(path, true) {
        }
    }
    //
    public static class FolderItemExtension {
        public static bool IsParentFolder(this FolderItem item) {
            return item is ParentFolder;
        }
    }
}
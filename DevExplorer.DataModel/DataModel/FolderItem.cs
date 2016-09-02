namespace DevExplorer.DataModel {
    using System.ComponentModel.DataAnnotations;
    using DevExplorer.DataModel.Extensions;

    public class FolderItem {
        public FolderItem(string path, bool isDirectory) {
            this.Path = path;
            this.IsDirectory = isDirectory;
            string fileName = this.IsParentFolder() ? ".." : FolderItemExtension.GetName(path);
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
}
namespace DevExplorer.DataModel.Extensions {
    using System.ComponentModel;
    using System.IO;

    public static class FolderItemExtension {
        internal static string GetName(string path) {
            var dirPath = Path.GetDirectoryName(path);
            bool isRoot = string.IsNullOrEmpty(dirPath) || dirPath.EndsWith(Path.DirectorySeparatorChar.ToString());
            return !string.IsNullOrEmpty(dirPath) ? path.Substring(dirPath.Length + (isRoot ? 0 : 1)) : string.Empty;
        }
        public static bool IsParentFolder(this FolderItem item) {
            return item is ParentFolder;
        }
        public static FolderItem AcceptFolderItem(this CancelEventArgs args, string text) {
            string dir = GetDirectoryPath(text);
            args.Cancel = !string.IsNullOrEmpty(dir);
            return args.Cancel ? new FolderItem(dir, true) : null;
        }
        static string GetDirectoryPath(string text) {
            string dirPath = null;
            if(Directory.Exists(text))
                dirPath = text;
            if(File.Exists(text))
                dirPath = Path.GetDirectoryName(text);
            return dirPath;
        }
    }
}
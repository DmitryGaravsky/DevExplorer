namespace DevExplorer.DataModel {
    public class Folder {
        public Folder(string path, string name) {
            this.Path = path;
            this.Name = name;
        }
        public string Path {
            get;
            private set;
        }
        public string Name {
            get;
            set;
        }
        public object Image {
            get;
            set;
        }
    }
    public sealed class SpecialFolder : Folder {
        public SpecialFolder(string path, string name)
            : base(path, name) {
        }
    }
    public sealed class NewFolder : Folder {
        public static readonly Folder Instance = new NewFolder();
        NewFolder() : base(null, "New Folder") { }
    }
    //
    public static class FolderExtension {
        public static bool IsNew(this Folder folder) {
            return folder is NewFolder;
        }
        public static bool IsSpecial(this Folder folder) {
            return folder is SpecialFolder;
        }
        public static bool IsNewOrSpecial(this Folder folder) {
            return IsNew(folder) || IsSpecial(folder);
        }
        public static bool TryGetSpecialFolder(this System.Environment.SpecialFolder specialFolder, out Folder result) {
            return TryGetFolder(System.Environment.GetFolderPath(specialFolder), out result, true);
        }
        public static bool TryGetFolder(string path, out Folder result, bool special = false) {
            var info = new System.IO.DirectoryInfo(path);
            result = !info.Exists ? null : CreateFolder(special, info);
            return result != null;
        }
        static Folder CreateFolder(bool special, System.IO.DirectoryInfo info) {
            return special ? new SpecialFolder(info.FullName, info.Name) : new Folder(info.FullName, info.Name);
        }
    }
}
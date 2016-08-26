namespace DevExplorer.DataServices {
    using DevExplorer.DataModel;

    public sealed class FoldersCommandFactory : CommandFactory<DataModel.Folder> {
        public FoldersCommandFactory() {
            RegisterEntry<IRecentFolderCommandParameter>(parameter => new RecentFolderCommand(parameter));
            RegisterEntry<INewFolderCommandParameter>(parameter => new NewFolderCommand(parameter));
            RegisterEntry<IUpdateFolderCommandParameter>(parameter => new UpdateFolderCommand(parameter));
        }
    }
    //
    public static class FolderCommands {
        public static bool Recent(this ICommandFactory<Folder> commandFactory, string path) {
            return commandFactory
                .GetCommand(new RecentFolderCommandParameter() { Path = path })
                .Execute();
        }
        public static bool New(this ICommandFactory<Folder> commandFactory, string path) {
            return commandFactory
                .GetCommand(new NewFolderCommandParameter() { Path = path })
                .Execute();
        }
        public static bool Update(this ICommandFactory<Folder> commandFactory, string path, string name) {
            return commandFactory
                .GetCommand(new UpdateFolderCommandParameter() { Path = path, Name = name })
                .Execute();
        }
    }
}
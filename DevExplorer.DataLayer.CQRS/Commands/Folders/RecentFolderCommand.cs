namespace DevExplorer.DataServices {
    public interface IRecentFolderCommandParameter
        : IFolderCommandParameter {
    }
    //
    sealed class RecentFolderCommandParameter : FolderCommandParameter, IRecentFolderCommandParameter { }
    sealed class RecentFolderCommand : FolderCommand<IRecentFolderCommandParameter> {
        public RecentFolderCommand(IRecentFolderCommandParameter parameter)
            : base(parameter) {
        }
        protected override bool ExecuteCore(IRecentFolderCommandParameter parameter) {
            return true;
        }
    }
}
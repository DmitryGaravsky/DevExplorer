namespace DevExplorer.DataServices {
    public interface INewFolderCommandParameter
        : IFolderCommandParameter {
    }
    //
    sealed class NewFolderCommandParameter : FolderCommandParameter, INewFolderCommandParameter { }
    sealed class NewFolderCommand : FolderCommand<INewFolderCommandParameter> {
        public NewFolderCommand(INewFolderCommandParameter parameter)
            : base(parameter) {
        }
        protected override bool ExecuteCore(INewFolderCommandParameter parameter) {
            return true;
        }
    }
}
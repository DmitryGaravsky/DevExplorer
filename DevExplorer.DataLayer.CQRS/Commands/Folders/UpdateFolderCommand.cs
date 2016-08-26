namespace DevExplorer.DataServices {
    public interface IUpdateFolderCommandParameter
        : IFolderCommandParameter {
        string Name {get;}
    }
    //
    sealed class UpdateFolderCommandParameter : FolderCommandParameter, IUpdateFolderCommandParameter {
        public string Name {
            get;
            set;
        }
    }
    sealed class UpdateFolderCommand : FolderCommand<IUpdateFolderCommandParameter> {
        public UpdateFolderCommand(IUpdateFolderCommandParameter parameter)
            : base(parameter) {
        }
        protected override bool ExecuteCore(IUpdateFolderCommandParameter parameter) {
            return true;
        }
    }
}
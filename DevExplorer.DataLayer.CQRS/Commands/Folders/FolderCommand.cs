namespace DevExplorer.DataServices {
    using DevExplorer.DataModel;

    public interface IFolderCommandParameter : ICommandParameter<Folder> {
        string Path { get; }
    }
    //
    abstract class FolderCommandParameter : IFolderCommandParameter {
        public string Path {
            get;
            set;
        }
    }
    abstract class FolderCommand<TParameter> : ICommand<Folder>
        where TParameter : IFolderCommandParameter {
        readonly TParameter parameter;
        public FolderCommand(TParameter parameter) {
            this.parameter = parameter;
        }
        public bool Execute() {
            return ExecuteCore(parameter);
        }
        protected abstract bool ExecuteCore(TParameter parameter);
    }
}
namespace DevExplorer.DataServices {
    public interface ICommand<T> {
        bool Execute();
    }
    //
    public interface ICommandParameter<T> { }
    public interface ICommandFactory<T> {
        ICommand<T> GetCommand(ICommandParameter<T> parameter);
    }
}
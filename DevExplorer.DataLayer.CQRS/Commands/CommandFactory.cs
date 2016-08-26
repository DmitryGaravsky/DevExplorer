namespace DevExplorer.DataServices {
    public abstract class CommandFactory<T> : BaseFactory<ICommandParameter<T>, ICommand<T>>, ICommandFactory<T> {
        ICommand<T> ICommandFactory<T>.GetCommand(ICommandParameter<T> parameter) {
            return GetEntry(parameter, Empty.Instance);
        }
        //
        sealed class Empty : ICommand<T> {
            internal readonly static ICommand<T> Instance = new Empty();
            Empty() { }
            bool ICommand<T>.Execute() {
                return true;
            }
        }
    }
}
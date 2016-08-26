namespace DevExplorer.Views {
    using DevExplorer.DataModel;
    using DevExplorer.ViewModels;
    using DevExpress.Utils.MVVM;
    using System.Windows.Forms;
    using DevExpress.XtraGrid.Views.Base;

    public class BackByBackspaceKey : EventToCommandBehavior<FolderViewModel, KeyEventArgs> {
        public BackByBackspaceKey()
            : base("KeyDown", x => x.Back()) {
        }
        protected override bool CanProcessEvent(KeyEventArgs args) {
            return args.KeyCode == Keys.Back;
        }
    }
}
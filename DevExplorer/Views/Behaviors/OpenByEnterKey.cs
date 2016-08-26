namespace DevExplorer.Views {
    using DevExplorer.DataModel;
    using DevExplorer.ViewModels;
    using DevExpress.Utils.MVVM;
    using System.Windows.Forms;
    using DevExpress.XtraGrid.Views.Base;

    public class OpenByEnterKey : EventToCommandBehavior<FolderViewModel, KeyEventArgs> {
        public OpenByEnterKey()
            : base("KeyDown", x => x.Open(null)) {
        }
        protected override object GetCommandParameter() {
            return ((ColumnView)Source).GetRow(((ColumnView)Source).FocusedRowHandle) as FolderItem;
        }
        protected override bool CanProcessEvent(KeyEventArgs args) {
            return args.KeyCode == Keys.Enter;
        }
    }
}
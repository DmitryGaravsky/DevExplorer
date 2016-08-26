namespace DevExplorer.Views {
    using DevExplorer.DataModel;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.Utils.MVVM;
    using System.Windows.Forms;
    using DevExplorer.ViewModels;
    using DevExpress.XtraGrid.Views.Base;

    public class OpenByRowDoubleClick : EventToCommandBehavior<FolderViewModel, RowClickEventArgs> {
        public OpenByRowDoubleClick()
            : base("RowClick", x => x.Open(null)) {
        }
        protected override object GetCommandParameter() {
            return ((ColumnView)Source).GetRow(Args.RowHandle) as FolderItem;
        }
        protected override bool CanProcessEvent(RowClickEventArgs args) {
            return args.Button == MouseButtons.Left && args.Clicks == 2;
        }
    }
}
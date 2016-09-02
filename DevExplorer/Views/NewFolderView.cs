namespace DevExplorer.Views {
    using System.ComponentModel;
    using System.Windows.Forms;
    using DevExplorer.DataModel;
    using DevExplorer.DataModel.Extensions;
    using DevExplorer.ViewModels;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraGrid.Views.Tile;

    public partial class NewFolderView : XtraUserControl {
        public NewFolderView() {
            InitializeComponent();
            if(!mvvmContext.IsDesignMode)
                InitializeBindings();
            searchControl.GotFocus += searchControl_GotFocus;
            tileView.KeyDown += tileView_KeyDown;
            tileView.KeyPress += tileView_KeyPress;
            tileView.ContextButtonCustomize += tileView_ContextButtonCustomize;
        }
        void searchControl_GotFocus(object sender, System.EventArgs e) {
            searchControl.GotFocus -= searchControl_GotFocus;
            gridControl.Focus();
        }
        void tileView_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Escape)
                searchControl.ClearFilter();
        }
        void tileView_KeyPress(object sender, KeyPressEventArgs e) {
            if(char.IsLetterOrDigit(e.KeyChar))
                searchControl.StartSearch(e.KeyChar);
        }
        void tileView_ContextButtonCustomize(object sender, TileViewContextButtonCustomizeEventArgs e) {
            var folder = GetFolder(e.RowHandle);
            if(folder.IsNewOrSpecial())
                e.Item.Visibility = ContextItemVisibility.Hidden;
        }
        void InitializeBindings() {
            var fluent = mvvmContext.OfType<NewFolderViewModel>();
            fluent.SetBinding(gridControl,
                gc => gc.DataSource, x => x.Folders);
            fluent.SetTrigger(x => x.IsLoading, ApplyLoading);
            //
            fluent.WithEvent(this, "Load")
                .EventToCommand(x => x.OnLoad());
            //
            fluent.WithEvent<TileViewItemClickEventArgs>(tileView, "ItemClick")
                .EventToCommand(x => x.OpenOrCreate(null), args => GetFolder(args.Item));
            fluent.WithEvent<ContextItemClickEventArgs>(tileView, "ContextButtonClick")
                .EventToCommand(x => x.Rename(null), args => GetFolder((TileViewItem)args.DataItem));
            //
            fluent.WithEvent<CancelEventArgs>(searchControl, "Accept")
                .EventToCommand(x => x.OpenOrCreate(null),
                    (CancelEventArgs args) => args.AcceptFolder(searchControl.Text));
            //
            fluent.WithCommand(x => x.OpenOrCreate(null))
                .After(() => searchControl.ResetSearch());
        }
        void ApplyLoading(bool loading) {
            itemForProgress.ContentVisible = loading;
            tileView.AnimateArrival = !loading;
        }
        Folder GetFolder(TileViewItem item) {
            return GetFolder(item.RowHandle);
        }
        Folder GetFolder(int rowHandle) {
            return tileView.GetRow(rowHandle) as Folder;
        }
    }
}
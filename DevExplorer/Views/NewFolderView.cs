namespace DevExplorer.Views {
    using DevExplorer.DataModel;
    using DevExplorer.ViewModels;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraGrid.Views.Tile;

    public partial class NewFolderView : XtraUserControl {
        public NewFolderView() {
            InitializeComponent();
            if(!mvvmContext.IsDesignMode)
                InitializeNavigation();
        }
        void InitializeNavigation() {
            var fluent = mvvmContext.OfType<NewFolderViewModel>();
            fluent.SetBinding(gridControl,
                gc => gc.DataSource, x => x.Folders);
            fluent.SetTrigger(x => x.IsLoading, loading =>
            {
                itemForProgress.ContentVisible = loading;
                tileView.AnimateArrival = !loading;
            });
            //
            fluent.WithEvent(this, "Load")
                .EventToCommand(x => x.OnLoad());
            fluent.WithEvent<TileViewItemClickEventArgs>(tileView, "ItemClick")
                .EventToCommand(x => x.OpenOrCreate(null), args => GetFolder(args.Item));
            fluent.WithEvent<ContextItemClickEventArgs>(tileView, "ContextButtonClick")
                .EventToCommand(x => x.Rename(null), args => GetFolder((TileViewItem)args.DataItem));
            //
            tileView.ContextButtonCustomize += tileView_ContextButtonCustomize;
        }
        void tileView_ContextButtonCustomize(object sender, TileViewContextButtonCustomizeEventArgs e) {
            var folder = GetFolder(e.RowHandle);
            if(folder.IsNewOrSpecial())
                e.Item.Visibility = ContextItemVisibility.Hidden;
        }
        Folder GetFolder(TileViewItem item) {
            return GetFolder(item.RowHandle);
        }
        Folder GetFolder(int rowHandle) {
            return tileView.GetRow(rowHandle) as Folder;
        }
    }
}
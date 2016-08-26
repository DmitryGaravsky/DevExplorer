namespace DevExplorer.Views {
    using DevExplorer.DataModel;
    using DevExplorer.ViewModels;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.ViewInfo;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;

    public partial class FolderView : XtraUserControl {
        public FolderView() {
            InitializeComponent();
            if(!mvvmContext.IsDesignMode)
                InitializeNavigation();
            gridView.CustomDrawCell += gridView1_CustomDrawCell;
        }
        void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e) {
            if(e.Column.FieldName == "Name" && gridView.IsDataRow(e.RowHandle)) {
                var folder = gridView.GetRow(e.RowHandle) as FolderItem;
                if(!folder.IsParentFolder()) {
                    var cell = (GridCellInfo)e.Cell;
                    ((TextEditViewInfo)cell.ViewInfo).ContextImage = (System.Drawing.Image)folder.Image;
                }
            }
        }
        void InitializeNavigation() {
            var fluent = mvvmContext.OfType<FolderViewModel>();
            fluent.SetBinding(gridControl,
                gc => gc.DataSource, x => x.Files);
            //
            fluent.BindCommand(backButton, x => x.Back());
            //
            mvvmContext.AttachBehavior<OpenByRowDoubleClick>(gridView);
            mvvmContext.AttachBehavior<OpenByEnterKey>(gridView);
            mvvmContext.AttachBehavior<BackByBackspaceKey>(gridView);
        }
    }
}
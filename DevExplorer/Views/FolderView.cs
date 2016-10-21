namespace DevExplorer.Views {
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using DevExplorer.DataModel;
    using DevExplorer.DataModel.Extensions;
    using DevExplorer.ViewModels;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.ViewInfo;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;

    public partial class FolderView : XtraUserControl {
        public FolderView() {
            InitializeComponent();
            if(!mvvmContext.IsDesignMode)
                InitializeBindings();
            gridView.CustomDrawCell += gridView_CustomDrawCell;
            gridView.KeyDown += gridView_KeyDown;
            gridView.KeyPress += gridView_KeyPress;
        }
        protected override void OnLoad(System.EventArgs e) {
            base.OnLoad(e);
            gridControl.Focus();
        }
        void gridView_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Escape)
                searchControl.ClearFilter();
        }
        void gridView_KeyPress(object sender, KeyPressEventArgs e) {
            if(char.IsLetterOrDigit(e.KeyChar))
                searchControl.StartSearch(e.KeyChar);
        }
        void gridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e) {
            if(e.Column.FieldName == "Name" && gridView.IsDataRow(e.RowHandle)) {
                var folder = gridView.GetRow(e.RowHandle) as FolderItem;
                if(!folder.IsParentFolder()) {
                    var cell = (GridCellInfo)e.Cell;
                    ((TextEditViewInfo)cell.ViewInfo).ContextImage = (Image)folder.Image;
                }
            }
        }
        void InitializeBindings() {
            var fluent = mvvmContext.OfType<FolderViewModel>();
            fluent.SetBinding(gridControl, gc => gc.DataSource, x => x.Files);
            //
            fluent.WithEvent<ColumnView, FocusedRowObjectChangedEventArgs>(gridView, "FocusedRowObjectChanged")
                .SetBinding(x => x.SelectedItem,
                    args => args.Row as FolderItem,
                        (gView, entity) => gView.SetFocusedRow(entity));
            //
            fluent.WithKeys(gridView, new Keys[] { Keys.Enter, Keys.Alt | Keys.Right })
                .KeysToCommand(x => x.Open(null),
                    (KeyEventArgs args) => gridView.GetFocusedRow() as FolderItem);
            fluent.WithKeys(gridView, new Keys[] { Keys.Back, Keys.Alt | Keys.Left })
                .KeysToCommand(x => x.Back());
            fluent.WithEvent<CancelEventArgs>(searchControl, "Accept")
                .EventToCommand(x => x.Open(null),
                    (CancelEventArgs args) => args.AcceptFolderItem(searchControl.Text));
            //
            mvvmContext.AttachBehavior<OpenByRowDoubleClick>(gridView);
            //
            fluent.WithCommand(x => x.Back())
                .Bind(btnBack);
            fluent.WithCommand(x => x.Open(null))
                .Bind(btnOpen, x => x.SelectedItem)
                .After(() => searchControl.ResetSearch());
        }
    }
}
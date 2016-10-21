namespace DevExplorer.Views {
    static class ColumnViewExtension {
        public static void SetFocusedRow<T>(this DevExpress.XtraGrid.Views.Base.ColumnView gView, T entity) {
            if(entity == null)
                gView.FocusInvalidRow();
            else
                gView.FocusedRowHandle = gView.FindRow(entity);
        }
    }
}
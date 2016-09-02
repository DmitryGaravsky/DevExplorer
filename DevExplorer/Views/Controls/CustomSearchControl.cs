namespace DevExplorer.Views {
    using System.ComponentModel;
    using System.Windows.Forms;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;

    public class CustomSearchControl : SearchControl {
        public CustomSearchControl() {
            AddingMRUItem += CustomSearchControl_AddingMRUItem;
        }
        protected override void Dispose(bool disposing) {
            AddingMRUItem -= CustomSearchControl_AddingMRUItem;
            base.Dispose(disposing);
        }
        public void ResetSearch() {
            ClearFilter();
            MoveFocusToClient();
        }
        public void StartSearch(char character) {
            Focus();
            SetFilter(character.ToString());
            Select(1, 0);
        }
        void MoveFocusToClient() {
            ((Control)Client).Focus();
        }
        //
        void CustomSearchControl_AddingMRUItem(object sender, AddingMRUItemEventArgs e) {
            e.Cancel = true;
        }
        protected override void OnKeyUp(KeyEventArgs e) {
            if(string.IsNullOrEmpty(Text)) {
                if(e.KeyCode == Keys.Escape)
                    MoveFocusToClient();
            }
            else {
                if(e.KeyCode == Keys.Down) {
                    MoveFocusToClient();
                    ClearFilter();
                }
            }
            base.OnKeyUp(e);
        }
        protected override void OnKeyDown(KeyEventArgs e) {
            if(e.KeyCode == Keys.Down)
                e.Handled = true;
            if(e.KeyCode == Keys.Enter)
                e.Handled = RaiseAccept();
            base.OnKeyDown(e);
        }
        //
        readonly static object accept = new object();
        public event CancelEventHandler Accept {
            add { Events.AddHandler(accept, value); }
            remove { Events.RemoveHandler(accept, value); }
        }
        protected bool RaiseAccept() {
            var handler = Events[accept] as CancelEventHandler;
            var args = new CancelEventArgs();
            if(handler != null)
                handler(this, args);
            return args.Cancel;
        }
    }
}
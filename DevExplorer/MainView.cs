using DevExpress.Utils.MVVM.Services;

namespace DevExplorer {
    public partial class MainView : DevExpress.XtraBars.TabForm {
        public MainView() {
            InitializeComponent();
            if(!mvvmContext.IsDesignMode)
                InitializeNavigation();
        }
        void InitializeNavigation() {
            mvvmContext.RegisterService(DocumentManagerService.Create(tabFormControl));
            mvvmContext.RegisterService(FolderBrowserDialogService.Create(this, new FolderBrowserDialogServiceOptions()
            {
                Description = "New Folder"
            }));
            //
            var fluent = mvvmContext.OfType<MainViewModel>();
            fluent.BindCommand(tabFormControl.AddPageButton, x => x.NewFolder());
            fluent.WithEvent(this, "Load")
                .EventToCommand(x => x.OnLoad());
            fluent.WithEvent(this, "Closed")
                .EventToCommand(x => x.OnClosed());
        }
    }
}
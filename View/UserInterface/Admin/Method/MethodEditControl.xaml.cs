using View.Utils;


namespace View.UserInterface.Admin.Method;

public partial class MethodEditControl
{
    private readonly MethodEditControlVM _viewModel;

    public MethodEditControl()
    {
        InitializeComponent();
        _viewModel = (MethodEditControlVM?)VmLocator.Resolve<MethodEditControl>();
        DataContext = _viewModel;
    }
}

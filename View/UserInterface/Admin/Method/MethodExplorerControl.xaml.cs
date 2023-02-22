using System.Windows.Controls;

using View.Utils;


namespace View.UserInterface.Admin.Method;

public partial class MethodExplorerControl : UserControl
{
    private readonly MethodExplorerControlVM _viewModel;

    public MethodExplorerControl()
    {
        InitializeComponent();
        _viewModel = (MethodExplorerControlVM?)VmLocator.Resolve<MethodExplorerControl>();
        DataContext = _viewModel;
    }
}


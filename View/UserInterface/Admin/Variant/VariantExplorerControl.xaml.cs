using System.Windows.Controls;

using View.Utils;


namespace View.UserInterface.Admin.Variant;

public partial class VariantExplorerControl : UserControl
{
    private readonly VariantExplorerControlVM _viewModel;

    public VariantExplorerControl()
    {
        InitializeComponent();
        _viewModel = (VariantExplorerControlVM?)VmLocator.Resolve<VariantExplorerControl>();
        DataContext = _viewModel;
    }
}
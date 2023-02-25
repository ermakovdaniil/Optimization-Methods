using View.Utils;


namespace View.UserInterface.Admin.Variant;

public partial class VariantEditControl
{
    private readonly VariantEditControlVM _viewModel;

    public VariantEditControl()
    {
        InitializeComponent();
        _viewModel = (VariantEditControlVM?)VmLocator.Resolve<VariantEditControl>();
        DataContext = _viewModel;
    }
}

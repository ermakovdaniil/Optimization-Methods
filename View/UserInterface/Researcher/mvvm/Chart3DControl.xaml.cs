using System.Windows.Controls;

using View.Utils;


namespace View.UserInterface.Researcher.Charts;

public partial class Chart3DControl : UserControl
{
    private readonly Chart3DControlVM _viewModel;

    public Chart3DControl()
    {
        _viewModel = (Chart3DControlVM?)VmLocator.Resolve<Chart3DControl>();
        DataContext = _viewModel;
    }
}
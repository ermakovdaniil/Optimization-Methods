using System.Windows.Controls;

using View.Utils;


namespace View.UserInterface.Researcher.Charts;

public partial class Chart2DControl : UserControl
{
    private readonly Chart2DControlVM _viewModel;

    public Chart2DControl()
    {
        InitializeComponent();
        _viewModel = (Chart2DControlVM?)VmLocator.Resolve<Chart2DControl>();
        DataContext = _viewModel;
    }
}
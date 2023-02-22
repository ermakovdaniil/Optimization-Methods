using System.Windows.Controls;

using View.Utils;


namespace View.UserInterface.Technologist;

public partial class TechnologistControl : UserControl
{
    private readonly TechnologistControlVM _viewModel;

    public TechnologistControl()
    {
        InitializeComponent();
        _viewModel = (TechnologistControlVM?)VmLocator.Resolve<TechnologistControl>();
        DataContext = _viewModel;
    }
}


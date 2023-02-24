using System.ComponentModel;
using System.Windows.Controls;

using View.Utils;


namespace View.UserInterface.Researcher;

public partial class ResearcherControl : UserControl
{
    private readonly ResearcherControlVM _viewModel;

    public ResearcherControl()
    {
        InitializeComponent();
        _viewModel = (ResearcherControlVM?)VmLocator.Resolve<ResearcherControl>();
        DataContext = _viewModel;
    }

    private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.PropertyDescriptor is PropertyDescriptor descriptor)
        {
            e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
        }
    }
}

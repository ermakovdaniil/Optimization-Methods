using System.ComponentModel;
using System.Windows;
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

    private void CreateCharts(object sender, RoutedEventArgs e)
    {
        //((ResearcherControlVM)DataContext).CalculateCommand.Execute(null);
        Chart2D.drawChart(((ResearcherControlVM)DataContext).Variant);
        Chart3D.Variant = ((ResearcherControlVM)DataContext).Variant;
    }
}

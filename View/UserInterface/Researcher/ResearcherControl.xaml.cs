using OptimizatonMethods.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace OptimizatonMethods
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // FontSize = (ActualHeight + ActualHeight / ActualWidth * ActualWidth) / 75;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((ResearcherControlVM)DataContext).CalculateCommand.Execute(null);
            Chart2D.drawChart(((ResearcherControlVM)DataContext).Task);
            //Chart3D.createChart( ((MainWindowViewModel)DataContext).Task);
            Chart3D.Task = ((ResearcherControlVM)DataContext).Task;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditWindow();
            editWindow.ShowDialog();
        }
    }
}

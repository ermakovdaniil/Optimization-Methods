using System.Windows.Controls;

using View.Utils;


namespace View.UserInterface.Admin.Task;

public partial class TaskExplorerControl : UserControl
{
    private readonly TaskExplorerControlVM _viewModel;

    public TaskExplorerControl()
    {
        InitializeComponent();
        _viewModel = (TaskExplorerControlVM?)VmLocator.Resolve<TaskExplorerControl>();
        DataContext = _viewModel;
    }
}
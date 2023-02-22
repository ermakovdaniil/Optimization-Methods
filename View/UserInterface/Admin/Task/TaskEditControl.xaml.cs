using View.Utils;


namespace View.UserInterface.Admin.Task;

public partial class TaskEditControl
{
    private readonly TaskEditControlVM _viewModel;

    public TaskEditControl()
    {
        InitializeComponent();
        _viewModel = (TaskEditControlVM?)VmLocator.Resolve<TaskEditControl>();
        DataContext = _viewModel;
    }
}

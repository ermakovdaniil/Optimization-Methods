using DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using View.UserInterface.Admin.Abstract;
using View.Utils;
using View.Utils.Dialog;
using View.Utils.MessageBoxService;


namespace View.UserInterface.Admin.Task;

public class TaskExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public TaskExplorerControlVM(MethodDBContext context, DialogService ds, IMessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
        _context = context;
        _ds = ds;
    }

    #endregion

    #endregion


    #region Properties

    private readonly DialogService _ds;
    private readonly MethodDBContext _context;
    private readonly IMessageBoxService _messageBoxService;
    public DataAccess.Models.Task SelectedTask { get; set; }

    public List<DataAccess.Models.Task> Tasks => _context.Tasks.ToList();

    #endregion


    #region Commands

    private RelayCommand _addTask;

    /// <summary>
    ///     Команда, открывающая окно создания нового задания
    /// </summary>
    public RelayCommand AddTask
    {
        get
        {
            return _addTask ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<TaskEditControl>(new WindowParameters
                {
                    Height = 210,
                    Width = 300,
                    Title = "Добавление варианта задания",
                },
                data: new DataAccess.Models.Task());

                OnPropertyChanged(nameof(Task));
            });
        }
    }

    private RelayCommand _editTask;

    /// <summary>
    ///     Команда, открывающая окно редактирования задания
    /// </summary>
    public RelayCommand EditTask
    {
        get
        {
            return _editTask ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<TaskEditControl>(new WindowParameters
                {
                    Height = 210,
                    Width = 300,
                    Title = "Редактирование варианта задания",
                },
                data: SelectedTask);

                OnPropertyChanged(nameof(Tasks));
            }, _ => SelectedTask is not null);
        }
    }

    private RelayCommand _deleteTask;

    /// <summary>
    ///     Команда, удаляющая задания
    /// </summary>
    public RelayCommand DeleteTask
    {
        get
        {
            return _deleteTask ??= new RelayCommand(o =>
            {
                if (_messageBoxService.ShowMessage($"Вы действительно хотите удалить: \"{SelectedTask.Name}\"?", "Удаление метода оптимизации", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.Tasks.Remove(SelectedTask);
                    _context.SaveChanges();
                }
                OnPropertyChanged(nameof(Tasks));
            }, c => SelectedTask is not null);
        }
    }

    #endregion
}


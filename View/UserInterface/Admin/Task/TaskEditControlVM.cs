using DataAccess.Data;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using View.UserInterface.Admin.Abstract;
using View.Utils;
using View.Utils.Dialog.Abstract;


namespace View.UserInterface.Admin.Task;

public class TaskEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private object _data;


    #region Functions

    #region Constructors

    public TaskEditControlVM(MethodDBContext context)
    {
        _context = context;
    }

    #endregion

    #endregion


    public object Data
    {
        get => _data;
        set
        {
            _data = value;

            TempTask = new DataAccess.Models.Task
            {
                Id = EditingTask.Id,
                Name = EditingTask.Name,
            };

            OnPropertyChanged(nameof(TempTask));
        }
    }

    public Action FinishInteraction { get; set; }

    public object? Result { get; }


    #region Properties

    public DataAccess.Models.Task TempTask { get; set; }

    public DataAccess.Models.Task EditingTask => (DataAccess.Models.Task)Data;

    private readonly MethodDBContext _context;

    public List<DataAccess.Models.Task> Tasks => _context.Tasks.ToList();

    #endregion


    #region Commands

    private RelayCommand _saveTask;

    /// <summary>
    ///     Команда сохраняющая изменение в базе данных
    /// </summary>
    public RelayCommand SaveTask
    {
        get
        {
            return _saveTask ??= new RelayCommand(o =>
            {
                EditingTask.Id = TempTask.Id;
                EditingTask.Name = TempTask.Name;

                if (!_context.Tasks.Contains(EditingTask))
                {
                    _context.Tasks.Add(EditingTask);
                }

                _context.SaveChanges();
                FinishInteraction();
            });
        }
    }

    private RelayCommand _closeCommand;

    public RelayCommand CloseCommand
    {
        get
        {
            return _closeCommand ??= new RelayCommand(o =>
            {
                FinishInteraction();
            });
        }
    }

    #endregion
}


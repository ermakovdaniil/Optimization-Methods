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
                Alpha = EditingTask.Alpha,
                Beta = EditingTask.Beta,
                Mu = EditingTask.Mu,
                MassConsumption = EditingTask.MassConsumption,
                Pressure = EditingTask.Pressure,
                Speed = EditingTask.Speed,
                Precision = EditingTask.Precision,
                Price = EditingTask.Price,
                T1max = EditingTask.T1max,
                T1min = EditingTask.T1min,
                T2max = EditingTask.T2max,
                T2min = EditingTask.T2min,
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
                EditingTask.Alpha = TempTask.Alpha;
                EditingTask.Beta = TempTask.Beta;
                EditingTask.Mu = TempTask.Mu;
                EditingTask.MassConsumption = TempTask.MassConsumption;
                EditingTask.Pressure = TempTask.Pressure;
                EditingTask.Speed = TempTask.Speed;
                EditingTask.Precision = TempTask.Precision;
                EditingTask.Price = TempTask.Price;
                EditingTask.T1max = TempTask.T1max;
                EditingTask.T1min = TempTask.T1min;
                EditingTask.T2max = TempTask.T2max;
                EditingTask.T2min = TempTask.T2min;
                EditingTask.TempCondition = TempTask.TempCondition;

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


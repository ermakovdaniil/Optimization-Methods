using DataAccess.Data;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using View.UserInterface.Admin.Abstract;
using View.Utils;
using View.Utils.Dialog.Abstract;


namespace View.UserInterface.Admin.Method;

public class MethodEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private object _data;


    #region Functions

    #region Constructors

    public MethodEditControlVM(MethodDBContext context)
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

            TempMethod = new DataAccess.Models.Method
            {
                Id = EditingMethod.Id,
                Name = EditingMethod.Name,
            };

            OnPropertyChanged(nameof(TempMethod));
        }
    }

    public Action FinishInteraction { get; set; }

    public object? Result { get; }


    #region Properties

    public DataAccess.Models.Method TempMethod { get; set; }

    public DataAccess.Models.Method EditingMethod => (DataAccess.Models.Method)Data;

    private readonly MethodDBContext _context;

    public List<DataAccess.Models.Method> Methods => _context.Methods.ToList();

    #endregion


    #region Commands

    private RelayCommand _saveMethod;

    /// <summary>
    ///     Команда сохраняющая изменение в базе данных
    /// </summary>
    public RelayCommand SaveMethod
    {
        get
        {
            return _saveMethod ??= new RelayCommand(o =>
            {
                EditingMethod.Id = TempMethod.Id;
                EditingMethod.Name = TempMethod.Name;

                if (!_context.Methods.Contains(EditingMethod))
                {
                    _context.Methods.Add(EditingMethod);
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


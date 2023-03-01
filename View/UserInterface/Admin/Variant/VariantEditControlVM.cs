using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using View.UserInterface.Admin.Abstract;
using View.Utils;
using View.Utils.Dialog.Abstract;


namespace View.UserInterface.Admin.Variant;

public class VariantEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private object _data;


    #region Functions

    #region Constructors

    public VariantEditControlVM(MethodDBContext context)
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

            TempVariant = new DataAccess.Models.Variant
            {
                Id = EditingVariant.Id,
                Name = EditingVariant.Name,
                Alpha = EditingVariant.Alpha,
                Beta = EditingVariant.Beta,
                Mu = EditingVariant.Mu,
                MassConsumption = EditingVariant.MassConsumption,
                Pressure = EditingVariant.Pressure,
                Speed = EditingVariant.Speed,
                Precision = EditingVariant.Precision,
                Price = EditingVariant.Price,
                T1max = EditingVariant.T1max,
                T1min = EditingVariant.T1min,
                T2max = EditingVariant.T2max,
                T2min = EditingVariant.T2min,
                TempCondition = EditingVariant.TempCondition,
            };

            OnPropertyChanged(nameof(TempVariant));
        }
    }

    public Action FinishInteraction { get; set; }

    public object? Result { get; }


    #region Properties

    public DataAccess.Models.Variant TempVariant { get; set; }

    public DataAccess.Models.Variant EditingVariant => (DataAccess.Models.Variant)Data;

    private readonly MethodDBContext _context;

    public List<DataAccess.Models.Variant> Variants => _context.Variants.ToList();

    #endregion


    #region Commands

    private RelayCommand _saveVariant;

    /// <summary>
    ///     Команда сохраняющая изменение в базе данных
    /// </summary>
    public RelayCommand SaveVariant
    {
        get
        {
            return _saveVariant ??= new RelayCommand(o =>
            {
                EditingVariant.Id = TempVariant.Id;
                EditingVariant.Name = TempVariant.Name;
                EditingVariant.Alpha = TempVariant.Alpha;
                EditingVariant.Beta = TempVariant.Beta;
                EditingVariant.Mu = TempVariant.Mu;
                EditingVariant.MassConsumption = TempVariant.MassConsumption;
                EditingVariant.Pressure = TempVariant.Pressure;
                EditingVariant.Speed = TempVariant.Speed;
                EditingVariant.Precision = TempVariant.Precision;
                EditingVariant.Price = TempVariant.Price;
                EditingVariant.T1max = TempVariant.T1max;
                EditingVariant.T1min = TempVariant.T1min;
                EditingVariant.T2max = TempVariant.T2max;
                EditingVariant.T2min = TempVariant.T2min;
                EditingVariant.TempCondition = TempVariant.TempCondition;

                if (!_context.Variants.Contains(EditingVariant))
                {
                    _context.Variants.Add(EditingVariant);
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


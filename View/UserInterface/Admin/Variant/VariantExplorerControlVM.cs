using DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using View.UserInterface.Admin.Abstract;
using View.Utils;
using View.Utils.Dialog;
using View.Utils.MessageBoxService;


namespace View.UserInterface.Admin.Variant;

public class VariantExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public VariantExplorerControlVM(MethodDBContext context, DialogService ds, IMessageBoxService messageBoxService)
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
    public DataAccess.Models.Variant SelectedVariant { get; set; }

    public List<DataAccess.Models.Variant> Variants => _context.Variants.ToList();

    #endregion


    #region Commands

    private RelayCommand _addVariant;

    /// <summary>
    ///     Команда, открывающая окно создания нового задания
    /// </summary>
    public RelayCommand AddVariant
    {
        get
        {
            return _addVariant ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<VariantEditControl>(new WindowParameters
                {
                    Height = 210,
                    Width = 300,
                    Title = "Добавление варианта задания",
                },
                data: new DataAccess.Models.Variant());

                OnPropertyChanged(nameof(Variant));
            });
        }
    }

    private RelayCommand _editVariant;

    /// <summary>
    ///     Команда, открывающая окно редактирования задания
    /// </summary>
    public RelayCommand EditVariant
    {
        get
        {
            return _editVariant ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<VariantEditControl>(new WindowParameters
                {
                    Height = 210,
                    Width = 300,
                    Title = "Редактирование варианта задания",
                },
                data: SelectedVariant);

                OnPropertyChanged(nameof(Variants));
            }, _ => SelectedVariant is not null);
        }
    }

    private RelayCommand _deleteVariant;

    /// <summary>
    ///     Команда, удаляющая задания
    /// </summary>
    public RelayCommand DeleteVariant
    {
        get
        {
            return _deleteVariant ??= new RelayCommand(o =>
            {
                if (_messageBoxService.ShowMessage($"Вы действительно хотите удалить: \"{SelectedVariant.Name}\"?", "Удаление варианта задания", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.Variants.Remove(SelectedVariant);
                    _context.SaveChanges();
                }
                OnPropertyChanged(nameof(Variants));
            }, c => SelectedVariant is not null);
        }
    }

    #endregion
}


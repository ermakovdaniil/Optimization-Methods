using DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using View.UserInterface.Admin.Abstract;
using View.Utils;
using View.Utils.Dialog;
using View.Utils.MessageBoxService;


namespace View.UserInterface.Admin.Method;

public class MethodExplorerControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public MethodExplorerControlVM(MethodDBContext context, DialogService ds, IMessageBoxService messageBoxService)
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
    public DataAccess.Models.Method SelectedMethod { get; set; }

    public List<DataAccess.Models.Method> Methods => _context.Methods.ToList();

    #endregion


    #region Commands

    private RelayCommand _addMethod;

    /// <summary>
    ///     Команда, открывающая окно создания нового метода
    /// </summary>
    public RelayCommand AddMethod
    {
        get
        {
            return _addMethod ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<MethodEditControl>(new WindowParameters
                {
                    Height = 210,
                    Width = 300,
                    Title = "Добавление метода оптимизациии",
                },
                data: new DataAccess.Models.Method());

                OnPropertyChanged(nameof(Method));
            });
        }
    }

    private RelayCommand _editMethod;

    /// <summary>
    ///     Команда, открывающая окно редактирования метода
    /// </summary>
    public RelayCommand EditMethod
    {
        get
        {
            return _editMethod ??= new RelayCommand(o =>
            {
                _ds.ShowDialog<MethodEditControl>(new WindowParameters
                {
                    Height = 210,
                    Width = 300,
                    Title = "Редактирование метода оптимизациии",
                },
                data: SelectedMethod);

                OnPropertyChanged(nameof(Methods));
            }, _ => SelectedMethod is not null);
        }
    }

    private RelayCommand _deleteMethod;

    /// <summary>
    ///     Команда, удаляющая метод
    /// </summary>
    public RelayCommand DeleteMethod
    {
        get
        {
            return _deleteMethod ??= new RelayCommand(o =>
            {
                if (_messageBoxService.ShowMessage($"Вы действительно хотите удалить: \"{SelectedMethod.Name}\"?", "Удаление метода оптимизации", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.Methods.Remove(SelectedMethod);
                    _context.SaveChanges();
                }
                OnPropertyChanged(nameof(Methods));
            }, c => SelectedMethod is not null);
        }
    }

    #endregion
}


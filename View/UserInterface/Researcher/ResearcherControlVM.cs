using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using OptimizationMethods.Methods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using View.UserInterface.Admin.Abstract;
using View.Utils;
using View.Utils.Dialog;
using View.Utils.IOService;
using View.Utils.MainWindowControlChanger;
using View.Utils.MessageBoxService;
using View.Utils.UserService;

namespace View.UserInterface.Researcher
{

    public class ResearcherControlVM : ViewModelBase
    {
        private object _data;
        private readonly IMathModel _mathModel;
        private readonly IFileDialogService _dialogService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly NavigationManager _navigationManager;
        private readonly IUserService _userService;

        #region Functions

        public ResearcherControlVM(UserDBContext userContext,
                                   MethodDBContext methodsContext,
                                   NavigationManager navigationManager,
                                   IMathModel mathModel,
                                   IFileDialogService dialogService,
                                   IMessageBoxService messageBoxService,
                                   IUserService userService)
        {
            Variant = new Variant { Alpha = 1, Beta = 1, Mu = 1, MassConsumption = 2, Pressure = 1, Speed = 2, Price = 100, Precision = 0.01, T1min = -3, T1max = 3, T2min = -2, T2max = 6, TempCondition = 1 };
            TabControlVisibility = Visibility.Hidden;
            _userContext = userContext;
            _methodsContext = methodsContext;
            _methodsContext.Methods.Load();
            _navigationManager = navigationManager;
            _mathModel = mathModel;
            _dialogService = dialogService;
            _messageBoxService = messageBoxService;
            _userService = userService;
        }


        #endregion


        #region Properties

        private readonly UserDBContext _userContext;
        private readonly MethodDBContext _methodsContext;
        public List<Method> Methods => _methodsContext.Methods.ToList();
        public Method SelectedMethod { get; set; }
        public List<Variant> Variants => _methodsContext.Variants.ToList();
        public Variant SelectedVariant { get; set; }
        public Variant Variant { get; set; }

        private bool _isCalculated;
        public bool IsCalculated
        {
            get
            {
                return _isCalculated;
            }
            set
            {
                _isCalculated = value;

                if (IsCalculated)
                {
                    TabControlVisibility = Visibility.Visible;
                }
                else
                {
                    TabControlVisibility = Visibility.Hidden;
                }
            }
        }

        private Visibility _tabControlVisibility;
        public Visibility TabControlVisibility
        {
            get
            {
                return _tabControlVisibility;
            }
            set
            {
                _tabControlVisibility = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable _dataList;
        public IEnumerable DataList
        {
            get
            {
                return _dataList;
            }
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        public CalculationResults Results { get; set; }

        #endregion


        #region Command

        private RelayCommand _calculateCommand;

        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(_ =>
                {
                    //if (SelectedMethod is null)
                    //{
                    //    _messageBoxService.ShowMessage("Не выбран метод оптимизации.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //if (Variant.T1min >= Variant.T1max && Variant.T2min >= Variant.T2max)
                    //{
                    //    _messageBoxService.ShowMessage("Минимальная температура не может быть больше максимальной.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //else
                    //{
                        try
                        {
                            IsCalculated = true;
                            //_mathModel = new MathModel(Variant);
                            CalculationResults result = new CalculationResults();
                            switch (SelectedMethod.Id)
                            {
                                case 1:
                                    ScanMethod.Calculate(Variant, out result);
                                    break;
                                case 2:
                                    //BoxMethod.Calculate(variant, out results);
                                    break;
                                default:
                                    break;
                            }
                            Results = result;
                            //results = _mathModel.Calculate(SelectedMethod, Variant);
                            Variant a = Variant;
                            OnPropertyChanged(nameof(Results));
                        }
                        catch (NullReferenceException)
                        {
                            _messageBoxService.ShowMessage(
                                "Невозможно произвести расчёт.\n" +
                                "\n" +
                                "Вероятные причины ошибки:\n" +
                                "   * Не выбран метод оптимизации.\n" +
                                "   * Неверно указаны температуры.\n" +
                                "      Минимальная температура не может быть больше максимальной.\n" +
                                "   * Неверно указана точность расчёта.", "Ошибка!",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    //}
                });
            }
        }

        private RelayCommand _createFile;
        public RelayCommand CreateFile
        {
            get
            {
                return _createFile ??= new RelayCommand(_ =>
                {
                    //if (AnalysisResult is null)
                    //{
                    //    _messageBoxService.ShowMessage("Недостаточно данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //else
                    //{
                    //    var filename = "АНАЛИЗ_" + DateTime.Now.ToString("dd.mm.yyyy_hh.mm.ss");
                    //    var filePath = _dialogService.SaveFileDialog(filename, ext: ".pdf");
                    //    if (!string.IsNullOrEmpty(filePath))
                    //    {
                    //        //FileSystem.ExportPdf(filePath, AnalysisResult);
                    //    }
                    //}
                });
            }
        }

        private RelayCommand _changeUser;
        public RelayCommand ChangeUser
        {
            get
            {
                return _changeUser ??= new RelayCommand(_ =>
                {
                    _navigationManager.Navigate<LoginControl>(new WindowParameters
                    {
                        Height = 300,
                        Width = 350,
                        Title = "Вход в систему",
                        StartupLocation = WindowStartupLocation.CenterScreen,
                    });
                });
            }
        }

        private RelayCommand _showInfo;
        public RelayCommand ShowInfo
        {
            get
            {
                return _showInfo ??= new RelayCommand(_ =>
                {
                    _messageBoxService.ShowMessage(
                        "Данный программный комплекс предназначен для расчёта\n" +
                        "целевой функции и поиска значений переменных целевой фукнции,\n" +
                        "при которых она принимает минимальное значение.\n" +
                        "\n" +
                        "Вам доступны следующие возможности:\n" +
                        "   * Расчёт целевой функции.\n" +
                        "   * Просморт результатов расчёта в виде таблицы.\n" +
                        "   * Просмотр 2D и 3D графиков функции.\n" +
                        "\n" +
                        "Автор:  Ермаков Даниил Игоревич\n" +
                        "Группа: 494\n" +
                        "Учебное заведение: СПбГТИ (ТУ)", "Справка о программе",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        private RelayCommand _watchInfo;
        public RelayCommand WatchInfo
        {
            get
            {
                return _watchInfo ??= new RelayCommand(c =>
                {
                    _messageBoxService.ShowMessage(
                        "Вариант 5\n" +
                        "Объектом оптимизации является химический реактор, в котором происходит образование целевого компонента.\n" +
                        "Реактор  оборудован  мешалкой и  двумя теплообменными устройствами: змеевиком и рубашкой.\n" +
                        "Необходимо определить температурные условия технологического процесса, обеспечивающие минимальную себестоимость целевого компонента.\n" +
                        "Согласно эмпирической математической модели, количество  получаемого целевого компонента  S(кг) связано с параметрами процесса следующим образом:\n" +
                        "S = α * (G * µ * ((T2 - T1) ^ N + (β * A - T1) ^ N)),\n" +
                        "где  α, β, µ, -нормирующие множители, равные 1;\n" +
                        "       G - расход реакционной массы ( 2 кг/ч );\n" +
                        "       А - давление в реакторе ( 1 Кпа );\n" +
                        "       N - скорость вращения мешалки ( 2 об/c );\n" +
                        "       Т1, Т2 - температуры в теплообменных устройствах (°C).\n" +
                        "Регламентом установлено, что температура в змеевике может изменяться в диапазоне от -3 до 3 °C,\n" +
                        "в рубашке -от - 2 до 6 °C. Кроме того, должно выполняться условие:\n" +
                        "       T1 + 0.5 * T2 ≤ 1.\n" +
                        "Себестоимость 1 кг целевого компонента \n" +
                        "составляет 100 у.е. Точность решения – 0,01 °C", "Условие",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        private RelayCommand _exit;
        public RelayCommand Exit
        {
            get
            {
                return _exit ??= new RelayCommand(_ =>
                {
                    Application.Current.Shutdown();
                });
            }
        }

        #endregion
    }
}

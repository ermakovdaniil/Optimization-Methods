using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using OptimizatonMethods.Models;

using WPF_MVVM_Classes;

using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;


namespace OptimizatonMethods.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {

        private ObservableCollection<Variant> _variants = new ObservableCollection<Variant>() { new Variant() { ID = 1, Name = "Вариант 1", IsEnabled = false },
                                                                                                new Variant() { ID = 2, Name = "Вариант 2", IsEnabled = false },
                                                                                                new Variant() { ID = 5, Name = "Вариант 5", IsEnabled = true } };

        public ObservableCollection<Variant> Variants
        {
            get { return _variants; }
            set { _variants = value; }
        }
        private Variant _selectedVariant;

        public Variant SelectedVariant
        {
            get { return _selectedVariant; }
            set { _selectedVariant = value; }
        }


        private ObservableCollection<CalcMethod> _calcMethods = new ObservableCollection<CalcMethod>() { new CalcMethod() { ID = 1, Name = "Метод сканирования", IsEnabled = true },
                                                                                                         new CalcMethod() { ID = 2, Name = "Градиентный метод", IsEnabled = false },
                                                                                                         new CalcMethod() { ID = 3, Name = "Метод наискорейшего спуска", IsEnabled = false } };

        public ObservableCollection<CalcMethod> CalcMethods
        {
            get { return _calcMethods; }
            set { _calcMethods = value; }
        }
        private CalcMethod _selectedCalcMethod;

        public CalcMethod SelectedCalcMethod
        {
            get { return _selectedCalcMethod; }
            set { _selectedCalcMethod = value; }
        }

        #region Constructors

        public MainWindowViewModel()
        {
            Task = new Task
            { Alpha = 1, Beta = 1, Mu = 1, G = 2, A = 1, N = 2, Price = 100, Precision = 0.01, T1min = -3, T1max = 3, T2min = -2, T2max = 6, TempCondition = 1 };
        }

        #endregion


        #region Variables

        private IEnumerable<Method> _allMethods;
        private IEnumerable<Task> _allTasks;
        private Task _selectedTask;
        private RelayCommand? _calculateCommand;
        private RelayCommand? _watchInfo;
        private IEnumerable _dataList;

        #endregion


        #region Properties

        public IEnumerable<Method> AllMethods
        {
            get
            {
                return _allMethods;
            }
            set
            {
                _allMethods = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Task> AllTasks
        {
            get
            {
                return _allTasks;
            }
            set
            {
                _allTasks = value;
                OnPropertyChanged();
            }
        }

        public Task Task { get; set; }

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

        private MathModel _mathModel;

        public MathModel MathModel
        {
            get
            {
                return _mathModel;
            }

            private set
            {
                _mathModel = value;
            }
        }

        #endregion

        #region Command

        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(c =>
                {
                    IsCalculated = true;

                    MathModel = new MathModel(Task);
                    MathModel.Calculate();
                    OnPropertyChanged(nameof(MathModel));
                });
            }
        }

        public RelayCommand WatchInfo
        {
            get
            {
                return _watchInfo ??= new RelayCommand(c =>
                {
                    HandyControl.Controls.MessageBox.Show("Вариант 5\n" +
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
                                    "составляет 100 у.е. Точность решения – 0,01 °C \n" +
                                    "Автор:  Ермаков Даниил Игоревич\n" +
                                    "Группа: 494\n" +
                                    "Учебное заведение: СПбГТИ (ТУ)", "Условие",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        #endregion
    }
}

using Autofac;
using DataAccess.Data;
using ImageAnalyzis;
using System.Globalization;
using System.Threading;
using System.Windows;
using View.UserInterface;
using View.UserInterface.Admin;
using View.UserInterface.Admin.Method;
using View.UserInterface.Admin.Task;
using View.UserInterface.Admin.User;
using View.UserInterface.Technologist;
using View.Utils;
using View.Utils.Dialog;
using View.Utils.FrameworkFactory;
using View.Utils.IOService;
using View.Utils.MainWindowControlChanger;
using View.Utils.MessageBoxService;
using View.Utils.UserService;
using FrameworkElementFactory = View.Utils.FrameworkFactory.FrameworkElementFactory;


namespace View;

public partial class App : Application
{
    private IContainer Container { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentUICulture;
        var builder = new ContainerBuilder();
        builder.RegisterType<MethodDBContext>().AsSelf();
        builder.RegisterType<UserDBContext>().AsSelf();

        builder.RegisterAssemblyTypes(typeof(App).Assembly)
               .Where(t => t.Name.EndsWith("VM"))
               .AsSelf();

        builder.RegisterAssemblyTypes(typeof(App).Assembly)
               .Where(t => t.Name.EndsWith("Control"))
               .AsSelf();

        builder.RegisterAssemblyTypes(typeof(App).Assembly)
               .Where(t => t.Name.EndsWith("Window"))
               .AsSelf();

        builder.RegisterType<MainWindow>().AsSelf().SingleInstance();
        builder.RegisterType<MainWindowVM>().AsSelf().SingleInstance();
        builder.RegisterType<FrameworkElementFactory>().As<IFrameworkElementFactory>();
        builder.RegisterType<NavigationManager>().AsSelf().SingleInstance();
        builder.RegisterType<UserControlFactory>().AsSelf();
        builder.RegisterType<DialogService>().AsSelf();
        //builder.RegisterType<ImageAnalyzis.ImageAnalyzer>().As<IImageAnalyzer>();
        builder.RegisterType<FileDialogService>().As<IFileDialogService>();
        builder.RegisterType<HandyMessageBoxService>().As<IMessageBoxService>();
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();

        Container = builder.Build();

        VmLocator.Container = Container;
        VmLocator.Register<MainWindow, MainWindowVM>();
        VmLocator.Register<LoginControl, LoginControlVM>();
        VmLocator.Register<TechnologistControl, TechnologistControlVM>();
        VmLocator.Register<MainAdminControl, MainAdminControlVM>();
        VmLocator.Register<UserEditControl, UserEditControlVM>();
        VmLocator.Register<UserExplorerControl, UserExplorerControlVM>();
        VmLocator.Register<MethodEditControl, MethodEditControlVM>();
        VmLocator.Register<MethodExplorerControl, MethodExplorerControlVM>();
        VmLocator.Register<TaskEditControl, TaskEditControlVM>();
        VmLocator.Register<TaskExplorerControl, TaskExplorerControlVM>();
        var mainWindow = Container.Resolve<MainWindow>();
        mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        mainWindow.Show();

        var navigator = Container.Resolve<NavigationManager>();

        navigator.Navigate<LoginControl>(new WindowParameters
        {
            Height = 300,
            Width = 350,
            Title = "Вход в систему",
            StartupLocation = WindowStartupLocation.CenterScreen,
        });
    }
}


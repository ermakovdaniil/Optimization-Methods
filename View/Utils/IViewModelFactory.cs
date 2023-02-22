using View.UserInterface.Admin.Abstract;


namespace View.Utils;

internal interface IViewModelFactory<TViewModel> where TViewModel : ViewModelBase
{
    TViewModel CreateVM();
}

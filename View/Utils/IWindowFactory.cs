using System.Windows;


namespace View.Utils;

internal interface IWindowFactory<T> where T : Window
{
    T CreateWindow();
}

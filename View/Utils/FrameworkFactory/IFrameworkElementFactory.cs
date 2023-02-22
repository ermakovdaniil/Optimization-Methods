using System.Windows;


namespace View.Utils.FrameworkFactory;

public interface IFrameworkElementFactory
{
    FrameworkElement CreateFrameworkElement<T>(object dataСontext) where T : FrameworkElement;
}

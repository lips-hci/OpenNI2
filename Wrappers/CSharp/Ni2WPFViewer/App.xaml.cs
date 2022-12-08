using System.Windows;

namespace Ni2WPFViewer
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Oops! Unhandled exception occurred!\n" + e.Exception.Message, "EXCEPTION", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}

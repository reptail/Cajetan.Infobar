using Cajetan.Infobar.Config;
using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Windows;

namespace Cajetan.Infobar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

        }

        private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            string msg = e.Exception.Message;
            string st = e.Exception.StackTrace;

            Debug.WriteLine("{0}\n{1}", msg, st);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();
            Current.MainWindow = mainWindow;

            AutofacConfig.Initialize(mainWindow);

            mainWindow.Show();
        }
    }
}

using Cajetan.Infobar.Config;
using Serilog;
using System;
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
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e?.Exception is null)
                Log.Error("Unhandled Exception, but Exception object was NULL!");
            else
                Log.Fatal(e.Exception, "Unhandled Exception! {ExceptionMessage:l}", e.Exception.Message);
        }

        private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            if (e?.Exception is null) return;

            Log.Debug(e.Exception, "First Change Exception: {ExceptionMessage:l}", e.Exception.Message);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();
            Current.MainWindow = mainWindow;

            AutofacConfig.Initialize(mainWindow);

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AutofacConfig.Dispose();

            base.OnExit(e);
        }
    }
}

using Cajetan.Infobar.Domain.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cajetan.Infobar.Services
{
    public class WindowService : IWindowService
    {

        private readonly List<Window> _openWindows;

        public WindowService()
        {
            _openWindows = new List<Window>();
        }

        public void OpenWindow(ObservableObject viewModel)
        {
            OpenWindow(viewModel, true);
        }

        public void OpenWindow(ObservableObject viewModel, bool allowResize)
        {
            OpenWindow(viewModel, allowResize, null, null);
        }

        public void OpenWindow(ObservableObject viewModel, bool allowResize, double? width, double? height)
        {
            Window window = CreateWindow(viewModel, allowResize, width, height);

            window.Show();
        }

        public bool? OpenDialog(ObservableObject viewModel)
        {
            return OpenDialog(viewModel, true);
        }

        public bool? OpenDialog(ObservableObject viewModel, bool allowResize)
        {
            return OpenDialog(viewModel, allowResize, null, null);
        }

        public bool? OpenDialog(ObservableObject viewModel, bool allowResize, double? width, double? height)
        {
            Window window = CreateWindow(viewModel, allowResize, width, height);

            // Do not show dialog in taskbar
            window.ShowInTaskbar = false;
            // Register KeyDown event
            window.PreviewKeyDown += window_PreviewKeyDown;
            // Show window
            return window.ShowDialog();
        }

        public bool Alert(string title, string message)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);

            return result == MessageBoxResult.OK;
        }

        public void CloseWindow(ObservableObject viewModel)
        {
            CloseWindow(viewModel, true);
        }

        public void CloseWindow(ObservableObject viewModel, bool result)
        {
            Window window = _openWindows.FirstOrDefault(w => w.DataContext == viewModel);
            if (window != null)
            {
                //window.Close();
                window.DialogResult = result;
                _openWindows.Remove(window);
            }
        }

        private Window CreateWindow(ObservableObject viewModel, bool allowResize, double? width, double? height)
        {
            Window window = new Window();

            try
            {
                window.Owner = Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            catch
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            window.ResizeMode = allowResize ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.ShowInTaskbar = true;
            if (width != null && height != null)
            {
                window.SizeToContent = SizeToContent.Manual;
                window.Width = width.Value;
                window.Height = height.Value;
            }
            else
            {
                window.SizeToContent = SizeToContent.WidthAndHeight;
            }

            window.Content = viewModel;
            window.DataContext = viewModel;

            window.ResizeMode = allowResize ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;            

            // Bind window title
            if (string.IsNullOrEmpty(window.Title))
                window.SetBinding(Window.TitleProperty, "DisplayName");

            // The WindowManager will always live longer than the window,
            // so we don't need to unsubscribe this event handler.
            window.Closed += window_Closed;
            
            // Add to open windows
            _openWindows.Add(window);

            return window;
        }

        private void window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Window w = sender as Window;
            // Set dialog result if sender is Window and Escape was pressed
            if (w != null && e.Key == Key.Escape)
                w.DialogResult = true;
            // Window will close
        }

        private void window_Closed(object sender, EventArgs e)
        {
            Window window = (Window)sender;

            // Unregister KeyDown event
            window.PreviewKeyDown -= window_PreviewKeyDown;

            // Notify ViewModel of event

            if (window.DataContext is ObservableObject vm)
            {
                //vm.Dispose();
            }

            // Remove from open windows
            _openWindows.Remove(window);
        }

    }
}

using Cajetan.Infobar.Domain.Services;
using Cajetan.Infobar.Domain.ViewModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Cajetan.Infobar.Services
{
    public class WindowService : IWindowService
    {
        private bool _isDisposed;
        private readonly List<Window> _openWindows;

        public WindowService()
        {
            _openWindows = new List<Window>();
        }

        public void OpenWindow(IWindowViewModel viewModel)
        {
            OpenWindow(viewModel, true);
        }

        public void OpenWindow(IWindowViewModel viewModel, bool allowResize)
        {
            OpenWindow(viewModel, allowResize, null, null);
        }

        public void OpenWindow(IWindowViewModel viewModel, bool allowResize, double? width, double? height)
        {
            Window window = CreateWindow(viewModel, allowResize, width, height);

            window.Show();
        }

        public bool? OpenDialog(IWindowViewModel viewModel)
        {
            return OpenDialog(viewModel, true);
        }

        public bool? OpenDialog(IWindowViewModel viewModel, bool allowResize)
        {
            return OpenDialog(viewModel, allowResize, null, null);
        }

        public bool? OpenDialog(IWindowViewModel viewModel, bool allowResize, double? width, double? height)
        {
            Window window = CreateWindow(viewModel, allowResize, width, height);

            // Do not show dialog in taskbar
            window.ShowInTaskbar = false;
            // Register KeyDown event
            window.PreviewKeyDown += Window_PreviewKeyDown;
            // Show window
            return window.ShowDialog();
        }

        public bool Alert(string title, string message)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);

            return result == MessageBoxResult.OK;
        }

        public string ShowColorDialog(string title, string currentColorHex)
        {
            BrushConverter brushConverter = new BrushConverter();

            TAlex.WPF.CommonDialogs.ColorPickerDialog diaglog = new TAlex.WPF.CommonDialogs.ColorPickerDialog
            {
                Title = title,
                Owner = Application.Current.MainWindow
            };

            if (!string.IsNullOrWhiteSpace(currentColorHex))
                if (brushConverter.ConvertFromString(currentColorHex) is SolidColorBrush solidColorBrush)
                    diaglog.SelectedColor = solidColorBrush.Color;

            if (diaglog.ShowDialog() == true)
            {
                SolidColorBrush selectedColor = new SolidColorBrush(diaglog.SelectedColor);
                string colorHex = brushConverter.ConvertToString(selectedColor);
                return colorHex;
            }

            return currentColorHex;
        }

        public void Invoke(Action act)
        {
            if (HasDispatcherAccess())
                act();
            else
                Application.Current.Dispatcher.Invoke(act);
        }
        public async Task InvokeAsync(Func<Task> asyncFunc)
        {
            if (HasDispatcherAccess())
            {
                await asyncFunc();
                return;
            }

            await Application.Current.Dispatcher.InvokeAsync(asyncFunc);
        }

        private static bool HasDispatcherAccess()
        {
            if (Application.Current?.Dispatcher is null)
                return true; // If dispatcher is null, assume we are running on UI thread.

            return Application.Current.Dispatcher.CheckAccess();
        }

        public void CloseWindow(IWindowViewModel viewModel)
        {
            CloseWindow(viewModel, true);
        }

        public void CloseWindow(IWindowViewModel viewModel, bool result)
        {
            Window window = _openWindows.FirstOrDefault(w => w.DataContext == viewModel);
            if (window != null)
            {
                //window.Close();
                window.DialogResult = result;
                _openWindows.Remove(window);
            }
        }

        private Window CreateWindow(IWindowViewModel viewModel, bool allowResize, double? width, double? height)
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
                window.SetBinding(Window.TitleProperty, nameof(IWindowViewModel.DisplayName));

            // The WindowManager will always live longer than the window,
            // so we don't need to unsubscribe this event handler.
            window.Closed += Window_Closed;

            // Add to open windows
            _openWindows.Add(window);

            return window;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Set dialog result if sender is Window and Escape was pressed
            if (sender is Window w && e.Key == Key.Escape)
                w.DialogResult = true;

            // Window will close
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (sender is not Window window) return;

            DisposeWindowAndDataContext(window);

            // Remove from open windows
            _openWindows.Remove(window);
        }

        private void DisposeWindowAndDataContext(Window window)
        {
            // Unregister events
            window.PreviewKeyDown -= Window_PreviewKeyDown;
            window.Closed -= Window_Closed;

            // Dispose DataContext
            if (window.DataContext is IDisposable disposable)
                disposable.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    foreach (Window w in _openWindows)
                        DisposeWindowAndDataContext(w);

                    _openWindows.Clear();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

using Cajetan.Infobar.Config;
using Cajetan.Infobar.Domain.AppBar;
using Cajetan.Infobar.ViewModels;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using appbar = WpfAppBar;

namespace Cajetan.Infobar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public partial class MainWindow : Window, IAppBarController
    {
        private MainViewModel _mainViewModel;

        private appbar.ABEdge _appbarEdge = appbar.ABEdge.None;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void DockBottom()
        {
            _appbarEdge = appbar.ABEdge.Bottom;
            appbar.AppBarFunctions.SetAppBar(this, _appbarEdge, topMost: false);
        }

        public void Undock()
        {
            _appbarEdge = appbar.ABEdge.None;
            appbar.AppBarFunctions.SetAppBar(this, _appbarEdge, topMost: false);
        }

        public void Reset()
        {
            if (_appbarEdge == appbar.ABEdge.None)
                return;

            appbar.AppBarFunctions.SetAppBar(this, appbar.ABEdge.None, topMost: false);
            Visibility = Visibility.Hidden;

            Thread.Sleep(50);

            Visibility = Visibility.Visible;
            appbar.AppBarFunctions.SetAppBar(this, _appbarEdge, topMost: false);

        }

        public void Shutdown() => Application.Current.Shutdown();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel = AutofacConfig.Resolve<MainViewModel>();
            _mainViewModel.Initialize();

            DataContext = _mainViewModel;

            // Remove from Alt+Tab Switcher
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);
            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);

#if DEBUG
            Top = 1150;
            Left = -1600;
            ShowInTaskbar = true;
            MouseDown += (s, e) =>
            {
                if (s is not Window w) return;
                if (e.ChangedButton != MouseButton.Left) return;

                w.DragMove();
            };
#else
            _mainViewModel.Dock();
#endif
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
#if DEBUG
            if (e.Key == Key.Escape)
                _mainViewModel.Close();
#endif
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            string wc = e.WidthChanged ? "T" : "F";
            string hc = e.HeightChanged ? "T" : "F";
            Debug.WriteLine($"Size Changed | W: {e.NewSize.Width,4:###0} ({wc}) | H: {e.NewSize.Height,4:###0} ({hc})");
        }


        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            int error;
            IntPtr result;
            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                int tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern int IntSetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        private static extern void SetLastError(int dwErrorCode);

    }
}

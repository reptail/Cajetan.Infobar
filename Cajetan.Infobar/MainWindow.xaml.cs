using Cajetan.Infobar.Config;
using Cajetan.Infobar.Domain.AppBar;
using Cajetan.Infobar.ViewModels;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using appBar = WpfAppBar;

namespace Cajetan.Infobar;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public partial class MainWindow : Window, IAppBarController
{
    private MainViewModel _mainViewModel;

    private appBar.ABEdge _appbarEdge = appBar.ABEdge.None;

    public MainWindow()
    {
        InitializeComponent();

        Loaded += OnLoaded;
#if DEBUG
        KeyDown += OnKeyDown;
#endif
    }

    public void DockBottom()
    {
        _appbarEdge = appBar.ABEdge.Bottom;
        appBar.AppBarFunctions.SetAppBar(this, _appbarEdge, topMost: false);
    }

    public void Undock()
    {
        _appbarEdge = appBar.ABEdge.None;
        appBar.AppBarFunctions.SetAppBar(this, _appbarEdge, topMost: false);
    }

    public void Reset()
    {
        if (_appbarEdge == appBar.ABEdge.None)
            return;

        appBar.AppBarFunctions.SetAppBar(this, appBar.ABEdge.None, topMost: false);

        Thread.Sleep(50);

        appBar.AppBarFunctions.SetAppBar(this, _appbarEdge, topMost: false);
    }

    public void Shutdown() => Application.Current.Shutdown();

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _mainViewModel = AutofacConfig.Resolve<MainViewModel>();
        _mainViewModel.Initialize();

        DataContext = _mainViewModel;

        // Remove from Alt+Tab Switcher
        WindowInteropHelper wndHelper = new(this);
        int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);
        exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
        SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);

#if DEBUG
        Top = 355;
        Left = -1250;
        ShowInTaskbar = true;
        MouseDown += (s, e) =>
        {
            if (s is not Window w) return;
            if (e.ChangedButton != MouseButton.Left) return;

            w.DragMove();
        };
#else
        Top = 10;
        Left = 10;
        Width = 0;
        _mainViewModel.Dock();
#endif
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            _mainViewModel.Close();
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

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
    private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
    private static extern int IntSetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
    private static extern void SetLastError(int dwErrorCode);

    private static int IntPtrToInt32(IntPtr intPtr)
    {
        return unchecked((int)intPtr.ToInt64());
    }
}

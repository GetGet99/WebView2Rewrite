using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIWindow = Microsoft.UI.Xaml.Window;
using Window = WinWrapper.Window;
using WinWrapper;
using WinRT.Interop;
using Windows.Win32;
using WebView2 = WebView2_Rewrite.WebView2;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WebView2_Rewrite_WinUI_3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WinUIWindow
    {
        readonly WebView2 WebView2;
        Window Window;
        //SubClassWindowMessageMonitor WndMsgMonitor;
        public MainWindow()
        {
            WebView2 = new(this);
            this.InitializeComponent();
            WebView2Place.Child = WebView2;
            //new Microsoft.UI.Xaml.Controls.WebView2();
            Window = Window.FromWindowHandle(WindowNative.GetWindowHandle(this));
            DoStuff();
            //WndMsgMonitor = new(Window);
            //WndMsgMonitor.WindowMessageReceived += WndMsgMonitor_WindowMessageReceived;
        }

        //private Windows.Win32.Foundation.LRESULT? WndMsgMonitor_WindowMessageReceived(Windows.Win32.Foundation.HWND hWnd, uint uMsg, Windows.Win32.Foundation.WPARAM wParam, Windows.Win32.Foundation.LPARAM lParam)
        //{
            //if (Keyboard.IsShiftDown)
            //    return PInvoke.SendMessage(PInvoke.GetFocus(), uMsg, wParam, lParam);
            //return null;
        //}

        async void DoStuff()
        {
            await WebView2.EnsureCoreWebView2Async();
            WebView2.CoreWebView2.Navigate("https://www.google.com/");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

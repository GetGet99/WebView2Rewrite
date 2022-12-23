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
using WebView2_Rewrite;
using Microsoft.Web.WebView2.Core;
using WebView2 = Microsoft.UI.Xaml.Controls.WebView2;
using Microsoft.UI.Composition.Interactions;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Win32.Foundation;
//using WebView2 = WebView2_Rewrite.WebView2;
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
        readonly Window Win32Window;
        //SubClassWindowMessageMonitor WndMsgMonitor;
        ElementInteractionTracker ElementInteractionTracker;
        public MainWindow()
        {
            WebView2 = new();
            this.InitializeComponent();
            WebView2Place.Child = WebView2;
            Win32Window = Window.FromWindowHandle(WindowNative.GetWindowHandle(this));
            DoStuff();
            ElementInteractionTracker = new ElementInteractionTracker(WebView2);
            ElementInteractionTracker.CustomAnimationStateEnteredEvent += ElementInteractionTracker_CustomAnimationStateEnteredEvent;
            ElementInteractionTracker.ValuesChangedEvent += ElementInteractionTracker_ValuesChanged;
            PrevPosition = ElementInteractionTracker.ScrollPresenterVisualInteractionSource.Position;
            ExtendsContentIntoTitleBar = true;
            new WinWrapper.SubClassWindowMessageMonitor().WindowMessageReceived += MainWindow_WindowMessageReceived;
        }

        private LRESULT? MainWindow_WindowMessageReceived(HWND hWnd, uint uMsg, WPARAM wParam, LPARAM lParam)
        {
            
        }

        //Point CursorPos;
        private void ElementInteractionTracker_CustomAnimationStateEnteredEvent(InteractionTrackerCustomAnimationStateEnteredArgs obj)
        {
            
        }

        Vector3 PrevPosition;
        async void ElementInteractionTracker_ValuesChanged(InteractionTrackerValuesChangedArgs obj)
        {
            PInvoke.GetCursorPos(out var pt);
            var winloc = Win32Window.ClientBounds.Location;
            var CoreWebView2 = WebView2.CoreWebView2;
            if (CoreWebView2 is null) return;
            var delta = obj.Position - PrevPosition;
            PrevPosition = obj.Position;
            //_ = CoreWebView2.ExecuteScriptAsync($"console.log('{pt.X - winloc.X}, {pt.Y - winloc.Y}')");
            await CoreWebView2.CallDevToolsProtocolMethodAsync("Input.dispatchMouseEvent", @$"
{{
    ""type"": ""mouseWheel"",
    ""x"": {pt.X - winloc.X},
    ""y"": {pt.Y - winloc.Y},
    ""deltaX"": {delta.X},
    ""deltaY"": {delta.Y}
}}");
            await CoreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setPageScaleFactor", @$"
{{
    ""pageScaleFactor"": {obj.Scale}
}}");
            
        }
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

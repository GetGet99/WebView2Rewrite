using Windows.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls;

namespace WebView2_Rewrite
{
    public sealed partial class MainPage : Page
    {
        readonly WebView2 WebView2 = new();
        public MainPage()
        {
            InitializeComponent();
            WebView2Place.Child = WebView2;
            InitializeWebView2();
        }

        async void InitializeWebView2()
        {
            await WebView2.EnsureCoreWebView2Async();
            WebView2.CoreWebView2.Navigate("https://www.google.com/");
            //WebView2.CoreWebView2CompositionController.SendMouseInput()
        }
    }
}

using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition.Interactions;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebView2_Rewrite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly WebView2 WebView2 = new();
        ElementInteractionTracker tracker;
        public MainPage()
        {
            this.InitializeComponent();
            WebView2Place.Child = WebView2;
            //new Microsoft.UI.Xaml.Controls.WebView2();
            DoStuff();

            //ScrollViewer.ScrollableWidth
            foreach (var i in Enumerable.Range(0, 100))
                StackPanel.Children.Add(new TextBlock { Text = "Hello"});
            tracker = new(Border);
            //InteractionTrackerInertiaRestingValue.Create().RestingValue
            PointerEventHandler pev = (o, e) => tracker.ScrollPresenterVisualInteractionSource.TryRedirectForManipulation(e.GetCurrentPoint(Border));
            //Border.PointerMoved += pev;
            //tracker.ScrollPresenterVisualInteractionSource.ConfigureDeltaScaleModifiers(CompositionConditionalValue.Create(compositor).)
            //Border.PointerPressed += pev;
            //Border.PointerWheelChanged += pev;
            tracker.ValuesChangedEvent += Tracker_ValuesChangedEvent;
            //InteractionTracker.CreateWithOwner()
            //ScrollViewer.ScrollToHorizontalOffset()
            PrevPosition = tracker.ScrollPresenterVisualInteractionSource.Position;


        }
        Vector3 PrevPosition;
        private async void Tracker_ValuesChangedEvent(InteractionTrackerValuesChangedArgs obj)
        {
            //_ = Dispatcher.TryRunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
            //{
            //    //tracker.ScrollPresenterVisualInteractionSource.
            //    //TB.Text = $"Position: {obj.Position} Scale: {obj.Scale} Velocity: {tracker.ScrollPresenterVisualInteractionSource.PositionVelocity} Scale Velocity: {tracker.ScrollPresenterVisualInteractionSource.ScaleVelocity}";
            //    Rectangle.Translation = obj.Position;
            //    Rectangle.Scale = new System.Numerics.Vector3(obj.Scale, obj.Scale, 1);
            //});
            var delta = obj.Position - PrevPosition;
            PrevPosition = obj.Position;
            var zoomfactor = WebView2.CoreWebView2CompositionController.ZoomFactor;
            //ScrollVertically((int)(-delta.Y / zoomfactor / 3));
            //ScrollHorizontally((int)(delta.X / zoomfactor / 3));
            await WebView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Input.dispatchMouseEvent", @$"
{{
    ""type"": ""mouseWheel"",
    ""x"": {100},
    ""y"": {100},
    ""deltaX"": {delta.X},
    ""deltaY"": {delta.Y}
}}");
            await WebView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Emulation.setPageScaleFactor", @$"
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
            //WebView2.CoreWebView2CompositionController.ZoomFactor = 2;
            
            //WebView2.CoreWebView2CompositionController.SetBoundsAndZoomFactor(
            //    new Rect(100, 100, 1610*2, 900*2),
            //    2
            //);
        }
        private void ScrollVertically(int Amount)
        {
            WebView2.CoreWebView2CompositionController.SendMouseInput(
                CoreWebView2MouseEventKind.Wheel, CoreWebView2MouseEventVirtualKeys.None, unchecked((uint)Amount), new Point(10, 10)
            );
        }
        private void ScrollHorizontally(int Amount)
        {
            WebView2.CoreWebView2CompositionController.SendMouseInput(
                CoreWebView2MouseEventKind.HorizontalWheel, CoreWebView2MouseEventVirtualKeys.None, unchecked((uint)Amount), new Point(100, 100)
            );
        }
    }
}

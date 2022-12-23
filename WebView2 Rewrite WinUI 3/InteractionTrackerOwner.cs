using System;
using System.Numerics;
using Microsoft.UI.Composition.Interactions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;

namespace WebView2_Rewrite
{
    class ElementInteractionTracker : IInteractionTrackerOwner
    {
        public InteractionTracker InteractionTracker { get; }
        public VisualInteractionSource ScrollPresenterVisualInteractionSource { get; }
        public ElementInteractionTracker(UIElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            InteractionTracker = InteractionTracker.CreateWithOwner(visual.Compositor, this);
            InteractionTracker.MinPosition = new Vector3(-1000, -1000, -1000);
            InteractionTracker.MaxPosition = new Vector3(1000, 1000, 1000);
            InteractionTracker.MinScale = 0.5f;
            InteractionTracker.MaxScale = 5f;

            InteractionTracker.InteractionSources.Add(
                ScrollPresenterVisualInteractionSource = VisualInteractionSource.Create(visual)
            );
            ScrollPresenterVisualInteractionSource.IsPositionXRailsEnabled =
                ScrollPresenterVisualInteractionSource.IsPositionYRailsEnabled = true;

            
            ScrollPresenterVisualInteractionSource.PointerWheelConfig.PositionXSourceMode =
                ScrollPresenterVisualInteractionSource.PointerWheelConfig.PositionYSourceMode
                = InteractionSourceRedirectionMode.Enabled;

            ScrollPresenterVisualInteractionSource.PositionXChainingMode =
                ScrollPresenterVisualInteractionSource.ScaleChainingMode =
                InteractionChainingMode.Auto;

            ScrollPresenterVisualInteractionSource.PositionXSourceMode =
                ScrollPresenterVisualInteractionSource.PositionYSourceMode =
                ScrollPresenterVisualInteractionSource.ScaleSourceMode =
                InteractionSourceMode.EnabledWithInertia;

        }
        public void CustomAnimationStateEntered(InteractionTracker sender, InteractionTrackerCustomAnimationStateEnteredArgs args)
        {
            CustomAnimationStateEnteredEvent?.Invoke(args);
        }

        public void IdleStateEntered(InteractionTracker sender, InteractionTrackerIdleStateEnteredArgs args)
        {

        }

        public void InertiaStateEntered(InteractionTracker sender, InteractionTrackerInertiaStateEnteredArgs args)
        {

        }

        public void InteractingStateEntered(InteractionTracker sender, InteractionTrackerInteractingStateEnteredArgs args)
        {

        }

        public void RequestIgnored(InteractionTracker sender, InteractionTrackerRequestIgnoredArgs args)
        {

        }
        Vector3 Vec = new Vector3(1000, 1000, 1000);
        public void ValuesChanged(InteractionTracker sender, InteractionTrackerValuesChangedArgs args)
        {
            InteractionTracker.MinPosition = args.Position - Vec;
            InteractionTracker.MaxPosition = args.Position + Vec;
            ValuesChangedEvent?.Invoke(args);
        }
        public event Action<InteractionTrackerCustomAnimationStateEnteredArgs>? CustomAnimationStateEnteredEvent;
        public event Action<InteractionTrackerValuesChangedArgs>? ValuesChangedEvent;
    }
}

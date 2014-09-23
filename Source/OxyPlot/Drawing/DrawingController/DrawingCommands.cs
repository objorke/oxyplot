// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingCommands.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines commands for the DrawingController.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Defines commands for the DrawingController.
    /// </summary>
    public static class DrawingCommands
    {
        /// <summary>
        /// Initializes static members of the <see cref="DrawingCommands" /> class.
        /// </summary>
        static DrawingCommands()
        {
            // commands that can be triggered from key events
            Reset = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandleReset(view));

            // commands that can be triggered from mouse down events
            ResetAt = new DelegateDrawingCommand<OxyMouseEventArgs>((view, controller, args) => HandleReset(view));
            PanAt = new DelegateDrawingCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new PanManipulator(view), args));
            ZoomRectangle = new DelegateDrawingCommand<OxyMouseDownEventArgs>((view, controller, args) => controller.AddMouseManipulator(view, new ZoomRectangleManipulator(view), args));
            ZoomWheel = new DelegateDrawingCommand<OxyMouseWheelEventArgs>((view, controller, args) => HandleZoomByWheel(view, args));
            ZoomWheelFine = new DelegateDrawingCommand<OxyMouseWheelEventArgs>((view, controller, args) => HandleZoomByWheel(view, args, 0.1));
            ZoomInAt = new DelegateDrawingCommand<OxyMouseEventArgs>((view, controller, args) => HandleZoomAt(view, args, 0.05));
            ZoomOutAt = new DelegateDrawingCommand<OxyMouseEventArgs>((view, controller, args) => HandleZoomAt(view, args, -0.05));

            PanZoomByTouch = new DelegateDrawingCommand<OxyTouchEventArgs>((view, controller, args) => controller.AddTouchManipulator(view, new TouchManipulator(view), args));

            // commands that can be triggered from key events
            PanLeft = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, -0.1, 0));
            PanRight = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0.1, 0));
            PanUp = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0, -0.1));
            PanDown = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0, 0.1));
            PanLeftFine = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, -0.01, 0));
            PanRightFine = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0.01, 0));
            PanUpFine = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0, -0.01));
            PanDownFine = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandlePan(view, 0, 0.01));

            ZoomIn = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandleZoomCenter(view, 1));
            ZoomOut = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandleZoomCenter(view, -1));
            ZoomInFine = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandleZoomCenter(view, 0.1));
            ZoomOutFine = new DelegateDrawingCommand<OxyKeyEventArgs>((view, controller, args) => HandleZoomCenter(view, -0.1));
        }

        /// <summary>
        /// Gets the reset axes command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> Reset { get; private set; }

        /// <summary>
        /// Gets the reset axes command (for mouse events).
        /// </summary>
        public static IViewCommand<OxyMouseEventArgs> ResetAt { get; private set; }

        /// <summary>
        /// Gets the pan/zoom touch command.
        /// </summary>
        public static IViewCommand<OxyTouchEventArgs> PanZoomByTouch { get; private set; }

        /// <summary>
        /// Gets the pan command.
        /// </summary>
        public static IViewCommand<OxyMouseDownEventArgs> PanAt { get; private set; }

        /// <summary>
        /// Gets the zoom rectangle command.
        /// </summary>
        public static IViewCommand<OxyMouseDownEventArgs> ZoomRectangle { get; private set; }

        /// <summary>
        /// Gets the zoom by mouse wheel command.
        /// </summary>
        public static IViewCommand<OxyMouseWheelEventArgs> ZoomWheel { get; private set; }

        /// <summary>
        /// Gets the fine-control zoom by mouse wheel command.
        /// </summary>
        public static IViewCommand<OxyMouseWheelEventArgs> ZoomWheelFine { get; private set; }

        /// <summary>
        /// Gets the pan left command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> PanLeft { get; private set; }

        /// <summary>
        /// Gets the pan right command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> PanRight { get; private set; }

        /// <summary>
        /// Gets the pan up command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> PanUp { get; private set; }

        /// <summary>
        /// Gets the pan down command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> PanDown { get; private set; }

        /// <summary>
        /// Gets the fine control pan left command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> PanLeftFine { get; private set; }

        /// <summary>
        /// Gets the fine control pan right command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> PanRightFine { get; private set; }

        /// <summary>
        /// Gets the fine control pan up command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> PanUpFine { get; private set; }

        /// <summary>
        /// Gets the fine control pan down command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> PanDownFine { get; private set; }

        /// <summary>
        /// Gets the zoom in command.
        /// </summary>
        public static IViewCommand<OxyMouseEventArgs> ZoomInAt { get; private set; }

        /// <summary>
        /// Gets the zoom out command.
        /// </summary>
        public static IViewCommand<OxyMouseEventArgs> ZoomOutAt { get; private set; }

        /// <summary>
        /// Gets the zoom in command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> ZoomIn { get; private set; }

        /// <summary>
        /// Gets the zoom out command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> ZoomOut { get; private set; }

        /// <summary>
        /// Gets the fine control zoom in command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> ZoomInFine { get; private set; }

        /// <summary>
        /// Gets the fine control zoom out command.
        /// </summary>
        public static IViewCommand<OxyKeyEventArgs> ZoomOutFine { get; private set; }

        /// <summary>
        /// Handles the reset events.
        /// </summary>
        /// <param name="view">The view to reset.</param>
        private static void HandleReset(IDrawingView view)
        {
            view.ActualViewModel.Reset();
        }

        /// <summary>
        /// Zooms the view by the specified factor at the position specified in the <see cref="OxyMouseEventArgs" />.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="args">The <see cref="OxyMouseWheelEventArgs" /> instance containing the event data.</param>
        /// <param name="delta">The zoom factor.</param>
        private static void HandleZoomAt(IDrawingView view, OxyMouseEventArgs args, double delta)
        {
            var m = new ZoomStepManipulator(view) { Step = delta, FineControl = args.IsControlDown };
            m.Started(args);
        }

        /// <summary>
        /// Zooms the view by the mouse wheel delta in the specified <see cref="OxyKeyEventArgs" />.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="args">The <see cref="OxyMouseWheelEventArgs" /> instance containing the event data.</param>
        /// <param name="factor">The zoom speed factor. Default value is 1.</param>
        private static void HandleZoomByWheel(IDrawingView view, OxyMouseWheelEventArgs args, double factor = 1)
        {
            var m = new ZoomStepManipulator(view) { Step = args.Delta * 0.001 * factor, FineControl = args.IsControlDown };
            m.Started(args);
        }

        /// <summary>
        /// Zooms the view by the key in the specified factor.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="delta">The zoom factor (positive zoom in, negative zoom out).</param>
        private static void HandleZoomCenter(IDrawingView view, double delta)
        {
            view.ActualViewModel.Zoom(1 + (delta * 0.12));
        }

        /// <summary>
        /// Pans the view by the key in the specified vector.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dx">The horizontal delta (percentage of Graphics area width).</param>
        /// <param name="dy">The vertical delta (percentage of Graphics area height).</param>
        private static void HandlePan(IDrawingView view, double dx, double dy)
        {
            dx *= view.ClientArea.Width;
            dy *= view.ClientArea.Height;
            view.ActualViewModel.Pan(new ScreenVector(dx, dy));
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingController.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a Graphics controller where the input command bindings can be modified.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a Graphics controller where the input command bindings can be modified.
    /// </summary>
    public class DrawingController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingController" /> class.
        /// </summary>
        public DrawingController()
        {
            // Zoom rectangle bindings: MMB / control RMB / control+alt LMB
            this.BindMouseDown(OxyMouseButton.Middle, DrawingCommands.ZoomRectangle);
            this.BindMouseDown(OxyMouseButton.Right, OxyModifierKeys.Control, DrawingCommands.ZoomRectangle);
            this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control | OxyModifierKeys.Alt, DrawingCommands.ZoomRectangle);

            // Reset bindings: Same as zoom rectangle, but double click / A key
            this.BindMouseDown(OxyMouseButton.Middle, OxyModifierKeys.None, 2, DrawingCommands.ResetAt);
            this.BindMouseDown(OxyMouseButton.Right, OxyModifierKeys.Control, 2, DrawingCommands.ResetAt);
            this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Control | OxyModifierKeys.Alt, 2, DrawingCommands.ResetAt);
            this.BindKeyDown(OxyKey.A, DrawingCommands.Reset);
            this.BindKeyDown(OxyKey.Home, DrawingCommands.Reset);
            this.BindCore(new OxyShakeGesture(), DrawingCommands.Reset);

            // Pan bindings: RMB / alt LMB / Up/down/left/right keys (panning direction on axis is opposite of key as it is more intuitive)
            this.BindMouseDown(OxyMouseButton.Right, DrawingCommands.PanAt);
            this.BindMouseDown(OxyMouseButton.Left, OxyModifierKeys.Alt, DrawingCommands.PanAt);
            this.BindKeyDown(OxyKey.Left, DrawingCommands.PanLeft);
            this.BindKeyDown(OxyKey.Right, DrawingCommands.PanRight);
            this.BindKeyDown(OxyKey.Up, DrawingCommands.PanUp);
            this.BindKeyDown(OxyKey.Down, DrawingCommands.PanDown);
            this.BindKeyDown(OxyKey.Left, OxyModifierKeys.Control, DrawingCommands.PanLeftFine);
            this.BindKeyDown(OxyKey.Right, OxyModifierKeys.Control, DrawingCommands.PanRightFine);
            this.BindKeyDown(OxyKey.Up, OxyModifierKeys.Control, DrawingCommands.PanUpFine);
            this.BindKeyDown(OxyKey.Down, OxyModifierKeys.Control, DrawingCommands.PanDownFine);

            this.BindTouchDown(DrawingCommands.PanZoomByTouch);

            // Zoom in/out binding: XB1 / XB2 / mouse wheels / +/- keys
            this.BindMouseDown(OxyMouseButton.XButton1, DrawingCommands.ZoomInAt);
            this.BindMouseDown(OxyMouseButton.XButton2, DrawingCommands.ZoomOutAt);
            this.BindMouseWheel(DrawingCommands.ZoomWheel);
            this.BindMouseWheel(OxyModifierKeys.Control, DrawingCommands.ZoomWheelFine);
            this.BindKeyDown(OxyKey.Add, DrawingCommands.ZoomIn);
            this.BindKeyDown(OxyKey.Subtract, DrawingCommands.ZoomOut);
            this.BindKeyDown(OxyKey.PageUp, DrawingCommands.ZoomIn);
            this.BindKeyDown(OxyKey.PageDown, DrawingCommands.ZoomOut);
            this.BindKeyDown(OxyKey.Add, OxyModifierKeys.Control, DrawingCommands.ZoomInFine);
            this.BindKeyDown(OxyKey.Subtract, OxyModifierKeys.Control, DrawingCommands.ZoomOutFine);
            this.BindKeyDown(OxyKey.PageUp, OxyModifierKeys.Control, DrawingCommands.ZoomInFine);
            this.BindKeyDown(OxyKey.PageDown, OxyModifierKeys.Control, DrawingCommands.ZoomOutFine);
        }

        /// <summary>
        /// Returns the elements that are hit at the specified position.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A sequence of hit results.
        /// </returns>
        public IEnumerable<HitTestResult> HitTest(IView view, HitTestArguments args)
        {
            var drawingView = (IDrawingView)view;
            return drawingView.ActualViewModel.HitTest(args);
        }
    }
}

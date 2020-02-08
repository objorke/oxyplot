// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomRectangleManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The zoom manipulator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Provides a plot control manipulator for zoom by rectangle functionality.
    /// </summary>
    public class ZoomRectangleManipulator : MouseManipulator
    {
        /// <summary>
        /// The zoom rectangle.
        /// </summary>
        private OxyRect zoomRectangle;

        /// <summary>
        /// The first position of the manipulation.
        /// </summary>
        private ScreenPoint startPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomRectangleManipulator" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ZoomRectangleManipulator(IDrawingView view)
            : base(view)
        {
        }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);

            this.View.HideZoomRectangle();
            this.View.SetCursorType(CursorType.Default);

            if (this.zoomRectangle.Width > 10 && this.zoomRectangle.Height > 10)
            {
                this.View.ActualViewModel.Zoom(this.zoomRectangle);
            }
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);
            this.zoomRectangle = new OxyRect(this.startPosition, e.Position);
            this.View.ShowZoomRectangle(this.zoomRectangle);
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>The cursor.</returns>
        public CursorType GetCursorType()
        {
            return CursorType.ZoomRectangle;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            this.startPosition = e.Position;
            this.zoomRectangle = new OxyRect(this.startPosition, this.startPosition);
            this.View.ShowZoomRectangle(this.zoomRectangle);
            this.View.SetCursorType(CursorType.ZoomRectangle);
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TouchManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a plot control manipulator for touch functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Provides a plot control manipulator for touch functionality.
    /// </summary>
    public class TouchManipulator : ManipulatorBase<OxyTouchEventArgs>
    {
        /// <summary>
        /// The previous position
        /// </summary>
        private ScreenPoint previousPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchManipulator" /> class.
        /// </summary>
        /// <param name="view">The plot control.</param>
        public TouchManipulator(IDrawingView view)
            : base(view)
        {
        }

        /// <summary>
        /// Gets the drawing view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public new IDrawingView View
        {
            get
            {
                return (IDrawingView)base.View;
            }
        }

        /// <summary>
        /// Occurs when a touch delta event is handled.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyTouchEventArgs e)
        {
            base.Delta(e);

            var newPosition = this.previousPosition + e.DeltaTranslation;

            this.View.ActualViewModel.Pan(this.previousPosition - newPosition, newPosition);

            this.View.ActualViewModel.ZoomAt(e.DeltaScale, newPosition);

            this.previousPosition = newPosition;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyTouchEventArgs e)
        {
            base.Started(e);
            this.previousPosition = e.Position;
        }
    }
}
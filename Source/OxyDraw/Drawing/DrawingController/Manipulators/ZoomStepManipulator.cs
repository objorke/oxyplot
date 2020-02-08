// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomStepManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The step manipulator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Provides a plot control manipulator for stepwise zoom functionality.
    /// </summary>
    public class ZoomStepManipulator : MouseManipulator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomStepManipulator" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ZoomStepManipulator(IDrawingView view)
            : base(view)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether FineControl.
        /// </summary>
        public bool FineControl { get; set; }

        /// <summary>
        /// Gets or sets Step.
        /// </summary>
        public double Step { get; set; }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);

            double scale = this.Step;
            if (this.FineControl)
            {
                scale *= 3;
            }

            scale = 1 + scale;

            // make sure the zoom factor is not negative
            if (scale < 0.1)
            {
                scale = 0.1;
            }

            this.View.ActualViewModel.ZoomAt(new ScreenVector(scale, scale), e.Position);
        }
    }
}
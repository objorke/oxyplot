// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PanManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a plot control manipulator for panning functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Provides a plot control manipulator for panning functionality.
    /// </summary>
    public class PanManipulator : MouseManipulator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PanManipulator" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public PanManipulator(IDrawingView view)
            : base(view)
        {
        }

        /// <summary>
        /// Gets or sets the previous position.
        /// </summary>
        private ScreenPoint PreviousPosition { get; set; }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);
            this.View.ActualViewModel.Pan(e.Position - this.PreviousPosition, e.Position);
            this.PreviousPosition = e.Position;
        }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);
            this.View.SetCursorType(CursorType.Default);
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>The cursor.</returns>
        public CursorType GetCursorType()
        {
            return CursorType.Pan;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            this.PreviousPosition = e.Position;
            this.View.SetCursorType(CursorType.Pan);
        }
    }
}

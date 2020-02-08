// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for mouse event manipulators.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Provides an abstract base class for mouse event manipulators.
    /// </summary>
    public abstract class MouseManipulator : ManipulatorBase<OxyMouseEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseManipulator"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected MouseManipulator(IDrawingView view)
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
    }
}
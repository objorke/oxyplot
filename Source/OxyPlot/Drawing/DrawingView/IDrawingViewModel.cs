// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDrawingViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies a presentation model for the drawing view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System.Collections.Generic;

    /// <summary>
    /// Specifies a presentation model for the drawing view.
    /// </summary>
    public interface IDrawingViewModel : IViewModel
    {
        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        /// <value>
        /// The client area rectangle.
        /// </value>
        OxyRect ClientArea { get; }

        /// <summary>
        /// Transforms the specified point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The transformed point.</returns>
        ScreenPoint Transform(DataPoint p);

        /// <summary>
        /// Transforms the specified point.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The transformed point.</returns>
        ScreenPoint Transform(double x, double y);

        /// <summary>
        /// Transforms the specified length.
        /// </summary>
        /// <param name="t">The length.</param>
        /// <returns>The transformed length.</returns>
        double Transform(double t);

        /// <summary>
        /// Inverse-transforms the specified length.
        /// </summary>
        /// <param name="t">The length.</param>
        /// <returns>The transformed length.</returns>
        double InverseTransform(double t);

        /// <summary>
        /// Inverse-transforms the specified point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The transformed point.</returns>
        DataPoint InverseTransform(ScreenPoint p);

        /// <summary>
        /// Zooms to extents.
        /// </summary>
        /// <param name="rc">The render context.</param>
        void ZoomExtents(IRenderContext rc);

        /// <summary>
        /// Gets the bounding box.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <returns>The bounding box.</returns>
        BoundingBox GetBounds(IRenderContext rc);

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="rc">The render context.</param>
        void Update(IRenderContext rc);

        /// <summary>
        /// Renders the view.
        /// </summary>
        /// <param name="rc">The render context.</param>
        void Render(IRenderContext rc);

        /// <summary>
        /// Invalidates the view.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Returns the elements that are hit at the specified position.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A sequence of hit results.
        /// </returns>
        IEnumerable<HitTestResult> HitTest(HitTestArguments args);
    }
}
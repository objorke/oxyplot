// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDrawingView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines a view that can display a DrawingModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Defines a view that can display a <see cref="DrawingModel" />.
    /// </summary>
    public interface IDrawingView : IView
    {
        /// <summary>
        /// Gets the drawing padding.
        /// </summary>
        /// <value>
        /// The padding.
        /// </value>
        double DrawingPadding { get; }

        /// <summary>
        /// Gets the actual presentation model.
        /// </summary>
        /// <value>
        /// The actual presentation model.
        /// </value>
        DrawingViewModel ActualViewModel { get; }
        
        /// <summary>
        /// Zooms the extents.
        /// </summary>
        void ZoomExtents();

        /// <summary>
        /// Invalidates this instance.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Gets the bounding box.
        /// </summary>
        /// <returns>The bounding box.</returns>
        BoundingBox GetBounds();
    }
}
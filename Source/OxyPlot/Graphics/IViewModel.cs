// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality for the view models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality for the view models.
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Pans the view by the specified vector.
        /// </summary>
        /// <param name="delta">The panning vector.</param>
        void Pan(ScreenVector delta);

        /// <summary>
        /// Pans the view by the specified vector.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <param name="point">The position.</param>
        void Pan(ScreenVector delta, ScreenPoint point);

        /// <summary>
        /// Zooms the view by the specified scale.
        /// </summary>
        /// <param name="deltaScale">The delta scale.</param>
        void Zoom(double deltaScale);

        /// <summary>
        /// Zooms to the specified rectangle.
        /// </summary>
        /// <param name="zoomRectangle">The zoom rectangle.</param>
        void Zoom(OxyRect zoomRectangle);

        /// <summary>
        /// Zooms at the specified point.
        /// </summary>
        /// <param name="deltaScale">The zoom factor.</param>
        /// <param name="current">The current.</param>
        void ZoomAt(ScreenVector deltaScale, ScreenPoint current);

        /// <summary>
        /// Resets the view.
        /// </summary>
        void Reset();
    }
}
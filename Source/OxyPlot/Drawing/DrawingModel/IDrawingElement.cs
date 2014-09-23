// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDrawingElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality for elements of a DrawingModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Specifies functionality for elements of a <see cref="DrawingModel" />.
    /// </summary>
    public interface IDrawingElement
    {
        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>The presentation model.</returns>
        DrawingElement.Presenter CreatePresentationModel(DrawingViewModel v);
    }
}
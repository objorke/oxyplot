// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for graphics models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality for the graphics models.
    /// </summary>
    public interface IModel {
        /// <summary>
        /// Gets the color of the background of the model.
        /// </summary>
        /// <value>The color.</value>
        OxyColor Background { get; }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="updateData">if set to <c>true</c> , all data collections will be updated.</param>
        void Update(bool updateData);

        /// <summary>
        /// Renders the model with the specified rendering context within the given rectangle.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="rect">The plot bounds.</param>
        void Render(IRenderContext rc, OxyRect rect);
    }
}

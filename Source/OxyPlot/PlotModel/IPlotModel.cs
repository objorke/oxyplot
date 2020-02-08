// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlotModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality for the plot model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality for the plot model.
    /// </summary>
    public interface IPlotModel : IModel
    {
        /// <summary>
        /// Attaches this model to the specified plot view.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        /// <remarks>Only one plot view can be attached to the plot model.
        /// The plot model contains data (e.g. axis scaling) that is only relevant to the current plot view.</remarks>
        void AttachPlotView(IPlotView plotView);
    }
}

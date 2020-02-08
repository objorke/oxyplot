// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for graphics models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    /// <summary>
    /// Provides an abstract base class for graphics models.
    /// </summary>
    public abstract partial class Model : IModel
    {
        /// <summary>
        /// The default selection color.
        /// </summary>
        internal static readonly OxyColor DefaultSelectionColor = OxyColors.Yellow;

        /// <summary>
        /// The synchronization root object.
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        protected Model()
        {
            this.SelectionColor = OxyColors.Yellow;
        }

        /// <summary>
        /// Gets or sets the color of the background of the plot.
        /// </summary>
        /// <value>The color. The default is <see cref="OxyColors.Undefined" />.</value>
        /// <remarks>If the background color is set to <see cref="OxyColors.Undefined" />, the default color of the plot view will be used.</remarks>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="Model" />.
        /// </summary>
        /// <value>A synchronization object.</value>
        /// <remarks>This property can be used when modifying the <see cref="Model" /> on a separate thread (not the thread updating or rendering the model).</remarks>
        public object SyncRoot
        {
            get { return this.syncRoot; }
        }

        /// <summary>
        /// Gets or sets the color of the selection.
        /// </summary>
        /// <value>The color of the selection.</value>
        public OxyColor SelectionColor { get; set; }

        /// <summary>
        /// Updates all axes and series.
        /// 0. Updates the owner PlotModel of all plot items (axes, series and annotations)
        /// 1. Updates the data of each Series (only if updateData==<c>true</c>).
        /// 2. Ensure that all series have axes assigned.
        /// 3. Updates the max and min of the axes.
        /// </summary>
        /// <param name="updateData">if set to <c>true</c> , all data collections will be updated.</param>
        void IModel.Update(bool updateData)
        {
            this.UpdateOverride(updateData);
        }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="updateData">if set to <c>true</c> , all data collections will be updated.</param>
        protected abstract void UpdateOverride(bool updateData);

        /// <summary>
        /// Renders the plot with the specified rendering context within the given rectangle.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="rect">The plot bounds.</param>
        void IModel.Render(IRenderContext rc, OxyRect rect)
        {
            this.RenderOverride(rc, rect);
        }

        /// <summary>
        /// Renders the plot with the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="rect">The plot bounds.</param>
        protected abstract void RenderOverride(IRenderContext rc, OxyRect rect);

        /// <summary>
        /// Returns the elements that are hit at the specified position.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A sequence of hit results.
        /// </returns>
        public IEnumerable<HitTestResult> HitTest(HitTestArguments args)
        {
            var hitTestElements = this.GetHitTestElements();

            foreach (var element in hitTestElements)
            {
                var result = element.HitTest(args);
                if (result != null)
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Gets all elements of the model, top-level elements first.
        /// </summary>
        /// <returns>An enumerator of the elements.</returns>
        protected abstract IEnumerable<UIElement> GetHitTestElements();
    }
}

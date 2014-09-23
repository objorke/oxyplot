// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointsElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for elements that contains points.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides an abstract base class for elements that contains points.
    /// </summary>
    public abstract class PointsElement : StrokedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointsElement"/> class.
        /// </summary>
        protected PointsElement()
        {
            this.Points = new List<DataPoint>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointsElement"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        protected PointsElement(IEnumerable<DataPoint> points)
        {
            this.Points = new List<DataPoint>(points);
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public List<DataPoint> Points { get; private set; }

        /// <summary>
        /// Represents the presentation model for the <see cref="PointsElement" />.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        protected abstract class PointsPresenter<T> : Presenter<T> where T : PointsElement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PointsPresenter{T}"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            protected PointsPresenter(T model, DrawingViewModel v)
                : base(model, v)
            {
            }

            /// <summary>
            /// Gets the transformed points.
            /// </summary>
            /// <value>
            /// The transformed points.
            /// </value>
            protected ScreenPoint[] TransformedPoints { get; private set; }

            /// <summary>
            /// Gets the bounding box of the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            /// <returns>
            /// The bounding box.
            /// </returns>
            public override BoundingBox GetBounds(IRenderContext rc)
            {
                var bbox = new BoundingBox();
                foreach (var p in this.Model.Points)
                {
                    bbox.Union(p);
                }

                return bbox;
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
                this.TransformedPoints = this.Model.Points.Select(this.Transform).ToArray();
                if (this.Model.MinimumSegmentLength > 0)
                {
                    this.TransformedPoints = ScreenPointHelper.ResamplePoints(this.TransformedPoints, this.Model.MinimumSegmentLength).ToArray();
                }
            }
        }
    }
}
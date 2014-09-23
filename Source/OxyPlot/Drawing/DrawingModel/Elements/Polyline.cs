// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polyline.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a polyline element.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a polyline element.
    /// </summary>
    public class Polyline : PointsElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Polyline"/> class.
        /// </summary>
        public Polyline()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polyline"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        public Polyline(IEnumerable<DataPoint> points)
            : base(points)
        {
        }

        /// <summary>
        /// Adds the specified point.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public void Add(double x, double y)
        {
            this.Points.Add(new DataPoint(x, y));
        }

        /// <summary>
        /// Adds a range of points.
        /// </summary>
        /// <param name="points">The points.</param>
        public void Add(params DataPoint[] points)
        {
            this.Points.AddRange(points);
        }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override DrawingElement.Presenter CreatePresenter(DrawingViewModel v)
        {
            return new PolylinePresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Polyline" />.
        /// </summary>
        protected class PolylinePresenter : PointsPresenter<Polyline>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PolylinePresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The v.</param>
            public PolylinePresenter(Polyline model, DrawingViewModel v)
                : base(model, v)
            {
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                rc.DrawLine(this.TransformedPoints, this.Model.Color, this.Transform(this.Model.Thickness), this.Model.LineStyle.GetDashArray(), this.Model.LineJoin, this.Model.Aliased);
            }
        }
    }
}
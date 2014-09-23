// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polygon.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a polygon element.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a polygon element.
    /// </summary>
    public class Polygon : ShapeElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        public Polygon()
        {
            this.Points = new List<DataPoint>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        public Polygon(IEnumerable<DataPoint> points)
        {
            this.Points = new List<DataPoint>(points);
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public IList<DataPoint> Points { get; private set; }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>
        /// The line join.
        /// </value>
        public LineJoin LineJoin { get; set; }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>
        /// The line style.
        /// </value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new PolygonPresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Polygon" />.
        /// </summary>
        private class PolygonPresenter : Presenter<Polygon>
        {
            /// <summary>
            /// The transformed points.
            /// </summary>
            private ScreenPoint[] transformedPoints;

            /// <summary>
            /// Initializes a new instance of the <see cref="PolygonPresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public PolygonPresenter(Polygon model, DrawingViewModel v)
                : base(model, v)
            {
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
                this.transformedPoints = this.Model.Points.Select(this.Transform).ToArray();
            }

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
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                rc.DrawPolygon(
                    this.transformedPoints,
                    this.Model.Fill,
                    this.Model.Stroke,
                    this.Transform(this.Model.Thickness),
                    this.Model.LineStyle.GetDashArray(),
                    this.Model.LineJoin);
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Lines.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a lines element.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Represents a lines element.
    /// </summary>
    public class Lines : PointsElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lines"/> class.
        /// </summary>
        public Lines()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lines"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        public Lines(IEnumerable<DataPoint> points)
            : base(points)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lines"/> class.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        public Lines(DataPoint p1, DataPoint p2)
            : this(new[] { p1, p2 })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lines"/> class.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public Lines(double x1, double y1, double x2, double y2)
            : this(new DataPoint(x1, y1), new DataPoint(x2, y2))
        {
        }

        /// <summary>
        /// Adds the specified p1.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        public void Add(DataPoint p1, DataPoint p2)
        {
            this.Points.Add(p1);
            this.Points.Add(p2);
        }

        /// <summary>
        /// Adds the specified x1.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public void Add(double x1, double y1, double x2, double y2)
        {
            this.Points.Add(new DataPoint(x1, y1));
            this.Points.Add(new DataPoint(x2, y2));
        }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new LinesPresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Lines" />.
        /// </summary>
        protected class LinesPresenter : PointsPresenter<Lines>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="LinesPresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public LinesPresenter(Lines model, DrawingViewModel v)
                : base(model, v)
            {
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                rc.DrawLineSegments(
                    this.TransformedPoints,
                    this.Model.Color,
                    this.Transform(this.Model.Thickness),
                    this.Model.LineStyle.GetDashArray(),
                    this.Model.LineJoin,
                    this.Model.Aliased);
            }
        }
    }
}
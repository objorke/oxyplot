// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Arrow.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an arrow.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Represents an arrow.
    /// </summary>
    public class Arrow : ShapeElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Arrow"/> class.
        /// </summary>
        public Arrow()
        {
            this.Thickness = 1;
            this.HeadLength = 6;
            this.HeadWidth = 3;
            this.Veeness = 1;
            this.Color = OxyColors.Black;
        }

        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        /// <value>
        /// The start point.
        /// </value>
        public DataPoint StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public DataPoint EndPoint { get; set; }

        /// <summary>
        /// Gets or sets the length of the head relative to the <see cref="ShapeElement.Thickness" />.
        /// </summary>
        /// <value>
        /// The length of the head.
        /// </value>
        public double HeadLength { get; set; }

        /// <summary>
        /// Gets or sets the width of the head relative to the <see cref="ShapeElement.Thickness" />.
        /// </summary>
        /// <value>
        /// The width of the head.
        /// </value>
        public double HeadWidth { get; set; }

        /// <summary>
        /// Gets or sets the veeness relative to the <see cref="ShapeElement.Thickness" />.
        /// </summary>
        /// <value>
        /// The veeness.
        /// </value>
        public double Veeness { get; set; }

        /// <summary>
        /// Gets or sets the color of the arrow.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new ArrowPresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Arrow" /> class.
        /// </summary>
        private class ArrowPresenter : Presenter<Arrow>
        {
            /// <summary>
            /// The points
            /// </summary>
            private readonly ScreenPoint[] points = new ScreenPoint[7];

            /// <summary>
            /// Initializes a new instance of the <see cref="ArrowPresenter" /> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public ArrowPresenter(Arrow model, DrawingViewModel v)
                : base(model, v)
            {
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
                var bb = new BoundingBox();
                bb.Union(this.Model.StartPoint);
                bb.Union(this.Model.EndPoint);
                return bb;
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
                var startPoint = this.Transform(this.Model.StartPoint);
                var endPoint = this.Transform(this.Model.EndPoint);
                var direction = endPoint - startPoint;
                direction.Normalize();
                var normal = new ScreenVector(-direction.Y, direction.X);
                var thickness = this.Transform(this.Model.Thickness);

                var p1 = endPoint - (direction * this.Model.HeadLength * thickness);
                var p2 = p1 + (direction * this.Model.Veeness * thickness);
                var n1 = normal * this.Model.HeadWidth * 0.5 * thickness;
                var n2 = normal * 0.5 * thickness;

                this.points[0] = endPoint;
                this.points[1] = p1 + n1;
                this.points[2] = p2 + n2;
                this.points[3] = startPoint + n2;
                this.points[4] = startPoint - n2;
                this.points[5] = p2 - n2;
                this.points[6] = p1 - n1;
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                rc.DrawPolygon(this.points, this.Model.Color, OxyColors.Undefined);
            }
        }
    }
}
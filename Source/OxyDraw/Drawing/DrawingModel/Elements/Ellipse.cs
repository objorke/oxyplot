// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ellipse.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an ellipse.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Represents an ellipse.
    /// </summary>
    public class Ellipse : ShapeElement
    {
        /// <summary>
        /// Gets or sets the center of the ellipse.
        /// </summary>
        /// <value>
        /// The center.
        /// </value>
        public DataPoint Center { get; set; }

        /// <summary>
        /// Gets or sets the x-axis radius of the ellipse.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double RadiusX { get; set; }

        /// <summary>
        /// Gets or sets the y-axis radius of the ellipse.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public double RadiusY { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new EllipsePresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Ellipse" />.
        /// </summary>
        private class EllipsePresenter : Presenter<Ellipse>
        {
            /// <summary>
            /// The rectangle defining the ellipse.
            /// </summary>
            private OxyRect rect;

            /// <summary>
            /// Initializes a new instance of the <see cref="EllipsePresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The v.</param>
            public EllipsePresenter(Ellipse model, DrawingViewModel v)
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
                var c = this.Model.Center;
                var dx = this.Model.RadiusX > 0 ? this.Model.RadiusX : 0;
                var dy = this.Model.RadiusY > 0 ? this.Model.RadiusY : 0;
                return new BoundingBox(c.X - dx, c.Y - dy, c.X + dx, c.Y + dy);
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
                var c = this.Transform(this.Model.Center);
                var dx = this.Model.RadiusX > 0 ? this.Transform(this.Model.RadiusX) : -this.Model.RadiusX;
                var dy = this.Model.RadiusY > 0 ? this.Transform(this.Model.RadiusY) : -this.Model.RadiusY;
                this.rect = new OxyRect(c.X - dx, c.Y - dy, dx * 2, dy * 2);
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                rc.DrawEllipse(this.rect, this.Model.Fill, this.Model.Stroke, this.Transform(this.Model.Thickness));
                if (this.Model.Text != null)
                {
                    rc.DrawText(this.rect.Center, this.Model.Text, this.Model.TextColor, this.Model.FontFamily, this.Model.FontSize, this.Model.FontWeight, 0, HorizontalAlignment.Center, VerticalAlignment.Middle);
                }
            }

            /// <summary>
            /// When overridden in a derived class, tests if the element is hit by the specified point.
            /// </summary>
            /// <param name="args">The hit test arguments.</param>
            /// <returns>
            /// The result of the hit test.
            /// </returns>
            protected override HitTestResult HitTestOverride(HitTestArguments args)
            {
                var dx = this.rect.Center.X - args.Point.X;
                var dy = this.rect.Center.Y - args.Point.Y;
                var rx = this.rect.Width / 2;
                var ry = this.rect.Height / 2;
                var q = (dx * dx / (rx * rx)) + (dy * dy / (ry * ry));
                if (q <= 1)
                {
                    return new HitTestResult(this.Model, args.Point);
                }

                return base.HitTestOverride(args);
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoundedRectangle.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a rounded rectangle.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a rounded rectangle.
    /// </summary>
    public class RoundedRectangle : Rectangle
    {
        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public double CornerRadius { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new RoundedRectanglePresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="RoundedRectangle" />.
        /// </summary>
        private class RoundedRectanglePresenter : Presenter<RoundedRectangle>
        {
            /// <summary>
            /// The rectangle
            /// </summary>
            private OxyRect rect;

            /// <summary>
            /// The points
            /// </summary>
            private List<ScreenPoint> points;

            /// <summary>
            /// Initializes a new instance of the <see cref="RoundedRectanglePresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public RoundedRectanglePresenter(RoundedRectangle model, DrawingViewModel v)
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
                return new BoundingBox(
                    this.Model.MinimumX,
                    this.Model.MinimumY,
                    this.Model.MaximumX,
                    this.Model.MaximumY);
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
                var p1 = this.Transform(this.Model.MinimumX, this.Model.MaximumY);
                var p2 = this.Transform(this.Model.MaximumX, this.Model.MinimumY);
                var cr = this.Transform(this.Model.CornerRadius);
                this.rect = new OxyRect(p1, p2);
                if (cr > 0)
                {
                    this.points = new List<ScreenPoint> { new ScreenPoint(p1.X + cr, p1.Y) };
                    this.points.AddRange(Interpolation.Arc(new ScreenPoint(p2.X - cr, p1.Y + cr), cr, cr, -90, 0));
                    this.points.AddRange(Interpolation.Arc(new ScreenPoint(p2.X - cr, p2.Y - cr), cr, cr, 0, 90));
                    this.points.AddRange(Interpolation.Arc(new ScreenPoint(p1.X + cr, p2.Y - cr), cr, cr, 90, 180));
                    this.points.AddRange(Interpolation.Arc(new ScreenPoint(p1.X + cr, p1.Y + cr), cr, cr, 180, 270));
                }
                else
                {
                    this.points = null;
                }
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                if (this.points != null)
                {
                    rc.DrawPolygon(
                        this.points,
                        this.Model.Fill,
                        this.Model.Stroke,
                        this.Transform(this.Model.Thickness));
                }
                else
                {
                    rc.DrawRectangleAsPolygon(
                        this.rect,
                        this.Model.Fill,
                        this.Model.Stroke,
                        this.Transform(this.Model.Thickness));
                }
            }
        }
    }
}
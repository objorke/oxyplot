// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rectangle.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a rectangle element.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Represents a rectangle element.
    /// </summary>
    public class Rectangle : ShapeElement
    {
        /// <summary>
        /// Gets or sets the minimum x coordinate.
        /// </summary>
        /// <value>
        /// The minimum x coordinate.
        /// </value>
        public double MinimumX { get; set; }

        /// <summary>
        /// Gets or sets the maximum x coordinate.
        /// </summary>
        /// <value>
        /// The maximum x coordinate.
        /// </value>
        public double MaximumX { get; set; }

        /// <summary>
        /// Gets or sets the minimum y coordinate.
        /// </summary>
        /// <value>
        /// The minimum y coordinate.
        /// </value>
        public double MinimumY { get; set; }

        /// <summary>
        /// Gets or sets the maximum y coordinate.
        /// </summary>
        /// <value>
        /// The maximum y coordinate.
        /// </value>
        public double MaximumY { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new RectanglePresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Rectangle" />.
        /// </summary>
        private class RectanglePresenter : Presenter<Rectangle>
        {
            /// <summary>
            /// The rectangle.
            /// </summary>
            private OxyRect rect;

            /// <summary>
            /// Initializes a new instance of the <see cref="RectanglePresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public RectanglePresenter(Rectangle model, DrawingViewModel v)
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
                this.rect = new OxyRect(p1, p2);
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                rc.DrawRectangle(this.rect, this.Model.Fill, this.Model.Stroke, this.Transform(this.Model.Thickness));
            }
        }
    }
}
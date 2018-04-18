// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Image.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an image element.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Represents an image element.
    /// </summary>
    public class Image : DrawingElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
            this.Opacity = 1.0;
            this.Interpolate = true;
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the source x.
        /// </summary>
        /// <value>
        /// The source x.
        /// </value>
        public int SourceX { get; set; }

        /// <summary>
        /// Gets or sets the source y.
        /// </summary>
        /// <value>
        /// The source y.
        /// </value>
        public int SourceY { get; set; }

        /// <summary>
        /// Gets or sets the width of the source.
        /// </summary>
        /// <value>
        /// The width of the source.
        /// </value>
        public int SourceWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the source.
        /// </summary>
        /// <value>
        /// The height of the source.
        /// </value>
        public int SourceHeight { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public OxyImage Source { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        public double Rotation { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Image"/> is interpolate.
        /// </summary>
        /// <value>
        ///   <c>true</c> if interpolate; otherwise, <c>false</c>.
        /// </value>
        public bool Interpolate { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new ImagePresentationmodel(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Image" />.
        /// </summary>
        private class ImagePresentationmodel : Presenter<Image>
        {
            /// <summary>
            /// The rectangle
            /// </summary>
            private OxyRect rect;

            /// <summary>
            /// Initializes a new instance of the <see cref="ImagePresentationmodel"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public ImagePresentationmodel(Image model, DrawingViewModel v)
                : base(model, v)
            {
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
                var p1 = this.Transform(this.Model.X, this.Model.Y);
                var w = this.Transform(this.Model.Width);
                var h = this.Transform(this.Model.Height);
                this.rect = new OxyRect(p1.X, p1.Y, w, h);
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
                var actualWidth = this.Model.Width;
                var actualHeight = this.Model.Height;
                if (double.IsNaN(this.Model.Width) || double.IsNaN(this.Model.Height))
                {
                    actualWidth = this.Model.Source.Width;
                    actualHeight = this.Model.Source.Height;
                }

                return new BoundingBox(this.Model.X, this.Model.Y - actualHeight, this.Model.X + actualWidth, this.Model.Y);
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                var sourceWidth = this.Model.SourceWidth > 0 ? this.Model.SourceWidth : this.Model.Source.Width;
                var sourceHeight = this.Model.SourceHeight > 0 ? this.Model.SourceHeight : this.Model.Source.Height;
                rc.DrawImage(
                    this.Model.Source,
                    (uint)this.Model.SourceX,
                    (uint)this.Model.SourceY,
                    (uint)sourceWidth,
                    (uint)sourceHeight,
                    this.rect.Left,
                    this.rect.Top,
                    this.rect.Width,
                    this.rect.Height,
                    this.Model.Opacity,
                    this.Model.Interpolate);
            }
        }
    }
}
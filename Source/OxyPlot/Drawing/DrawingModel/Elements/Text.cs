// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Text.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an element displaying text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Represents an element displaying text.
    /// </summary>
    public class Text : DrawingElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        public Text()
        {
            this.Color = OxyColors.Black;
            this.FontFamily = "Segoe UI";
            this.FontSize = -11;
            this.FontWeight = FontWeights.Normal;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The point.
        /// </value>
        public DataPoint Point { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the text.
        /// </summary>
        /// <value>
        /// The rotate.
        /// </value>
        public double Rotate { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>
        /// The horizontal alignment.
        /// </value>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        /// <value>
        /// The vertical alignment.
        /// </value>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new TextPresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Text" />.
        /// </summary>
        private class TextPresenter : Presenter<Text>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TextPresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public TextPresenter(Text model, DrawingViewModel v)
                : base(model, v)
            {
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                var screenPoints = this.Transform(this.Model.Point);
                rc.DrawText(
                    screenPoints,
                    this.Model.Content,
                    this.Model.Color,
                    this.Model.FontFamily,
                    this.Transform(this.Model.FontSize),
                    this.Model.FontWeight,
                    this.Model.Rotate,
                    this.Model.HorizontalAlignment,
                    this.Model.VerticalAlignment);
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
                // todo: adjust for rotating and alignment
                var size = rc.MeasureText(
                    this.Model.Content,
                    this.Model.FontFamily,
                    this.Transform(this.Model.FontSize),
                    this.Model.FontWeight);
                var w = this.InverseTransform(size.Width);
                var h = this.InverseTransform(size.Height);
                var dx = 0d;
                if (this.Model.HorizontalAlignment == HorizontalAlignment.Center)
                {
                    dx = -w / 2;
                }

                if (this.Model.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    dx = -w;
                }

                var dy = 0d;
                if (this.Model.VerticalAlignment == VerticalAlignment.Middle)
                {
                    dy = -h / 2;
                }

                if (this.Model.VerticalAlignment == VerticalAlignment.Top)
                {
                    dy = -h;
                }

                double x = this.Model.Point.X + dx;
                double y = this.Model.Point.Y + dy;

                // TODO: account for rotation
                return new BoundingBox(x, y, x + w, y + h);
            }
        }
    }
}
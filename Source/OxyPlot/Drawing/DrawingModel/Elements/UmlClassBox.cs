// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UmlClassBox.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an UML box.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents an UML box.
    /// </summary>
    public class UmlClassBox : ShapeElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmlClassBox"/> class.
        /// </summary>
        public UmlClassBox()
        {
            this.FontFamily = "Consolas";
            this.FontSize = 6;
            this.Position = new DataPoint(0, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmlClassBox"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public UmlClassBox(Type type)
            : this()
        {
            this.Methods = type.GetTypeInfo().DeclaredMethods.Select(mi => mi.Name).ToArray();
            this.Properties = type.GetTypeInfo().DeclaredProperties.Select(mi => mi.Name).ToArray();
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public DataPoint Position { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public string[] Properties { get; set; }

        /// <summary>
        /// Gets or sets the methods.
        /// </summary>
        /// <value>
        /// The methods.
        /// </value>
        public string[] Methods { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new UmlClassBoxPresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="UmlClassBox" />.
        /// </summary>
        private class UmlClassBoxPresenter : Presenter<UmlClassBox>
        {
            /// <summary>
            /// The position
            /// </summary>
            private ScreenPoint position;

            /// <summary>
            /// The font size
            /// </summary>
            private double fontSize;

            /// <summary>
            /// Initializes a new instance of the <see cref="UmlClassBoxPresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The v.</param>
            public UmlClassBoxPresenter(UmlClassBox model, DrawingViewModel v)
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
                var currentFontSize = this.Transform(this.Model.FontSize);
                var titleSize = rc.MeasureText(this.Model.Title, this.Model.FontFamily, currentFontSize, FontWeights.Bold);
                var propertySizes = this.Model.Properties.Select(p => rc.MeasureText(p, this.Model.FontFamily, currentFontSize)).ToArray();
                var methodSizes = this.Model.Properties.Select(p => rc.MeasureText(p, this.Model.FontFamily, currentFontSize)).ToArray();
                var maxWidth = Math.Max(titleSize.Width, Math.Max(propertySizes.Max(p => p.Width), methodSizes.Max(p => p.Width)));
                var totalHeight = titleSize.Height + propertySizes.Sum(p => p.Height) + methodSizes.Sum(p => p.Height);
                maxWidth = this.InverseTransform(maxWidth);
                totalHeight = this.InverseTransform(totalHeight);
                var bb = new BoundingBox();
                bb.Union(this.Model.Position);
                bb.Union(this.Model.Position.X + maxWidth, this.Model.Position.Y - totalHeight);
                return bb;
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
                this.position = this.Transform(this.Model.Position);
                this.fontSize = this.Transform(this.Model.FontSize);
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                var x = this.position.X + 5;
                var y = this.position.Y;
                rc.DrawText(new ScreenPoint(x, y), this.Model.Title, OxyColors.Black, this.Model.FontFamily, this.fontSize, FontWeights.Bold);
                var titleSize = rc.MeasureText(this.Model.Title, this.Model.FontFamily, this.fontSize, FontWeights.Bold);
                y += titleSize.Height;
                var y0 = y;
                var maxWidth = titleSize.Width;
                foreach (var p in this.Model.Properties)
                {
                    rc.DrawText(new ScreenPoint(x, y), p, OxyColors.Black, this.Model.FontFamily, this.fontSize);
                    var size = rc.MeasureText(p, this.Model.FontFamily, this.fontSize);
                    y += size.Height;
                    maxWidth = Math.Max(maxWidth, size.Width);
                }

                var y1 = y;
                foreach (var p in this.Model.Methods)
                {
                    rc.DrawText(new ScreenPoint(x, y), p, OxyColors.Black, this.Model.FontFamily, this.fontSize);
                    var size = rc.MeasureText(p, this.Model.FontFamily, this.fontSize);
                    y += size.Height;
                    maxWidth = Math.Max(maxWidth, size.Width);
                }

                var rect = new OxyRect(this.position.X, this.position.Y, maxWidth + (this.fontSize / 2), y - this.position.Y);
                rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Black);
                rc.DrawLineSegments(new[] { new ScreenPoint(this.position.X, y0), new ScreenPoint(rect.Right, y0), new ScreenPoint(this.position.X, y1), new ScreenPoint(rect.Right, y1) }, OxyColors.Black);
            }
        }
    }
}

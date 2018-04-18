// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShapeElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for elements that has a shape.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Provides an abstract base class for elements that has a shape.
    /// </summary>
    public abstract class ShapeElement : DrawingElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeElement"/> class.
        /// </summary>
        protected ShapeElement()
        {
            this.Stroke = OxyColors.Black;
            this.Fill = OxyColors.Undefined;
            this.Thickness = -1;
            this.TextColor = OxyColors.Black;
            this.FontFamily = "Arial";
            this.FontSize = 12;
            this.FontWeight = FontWeights.Normal;
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        /// <value>
        /// The color. The default is <see cref="OxyColors.Black" />.
        /// </value>
        public OxyColor Stroke { get; set; }

        /// <summary>
        /// Gets or sets the fill color.
        /// </summary>
        /// <value>
        /// The color. The default is <see cref="OxyColors.Undefined" />.
        /// </value>
        public OxyColor Fill { get; set; }

        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        /// <value>
        /// The thickness. The default is <c>-1</c>.
        /// </value>
        /// <remarks>Negative numbers specifies the thickness in output device units.</remarks>
        public double Thickness { get; set; }
        
        /// <summary>
        /// Gets or sets the text of the element.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>
        /// The color of the text.
        /// </value>
        public OxyColor TextColor { get; set; }
    }
}
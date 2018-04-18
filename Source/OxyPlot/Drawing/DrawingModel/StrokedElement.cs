// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrokedElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an element with a stroke.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    /// <summary>
    /// Represents an element with a stroke.
    /// </summary>
    public abstract class StrokedElement : DrawingElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrokedElement"/> class.
        /// </summary>
        protected StrokedElement()
        {
            this.Color = OxyColors.Black;
            this.Thickness = -1;
            this.LineJoin = LineJoin.Miter;
            this.LineStyle = LineStyle.Solid;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the thickness.
        /// </summary>
        /// <value>
        /// The thickness.
        /// </value>
        public double Thickness { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="StrokedElement"/> is aliased.
        /// </summary>
        /// <value>
        ///   <c>true</c> if aliased; otherwise, <c>false</c>.
        /// </value>
        public bool Aliased { get; set; }

        /// <summary>
        /// Gets or sets the line join type.
        /// </summary>
        /// <value>
        /// The line join type.
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
        /// Gets or sets the minimum length of the segments.
        /// </summary>
        /// <value>
        /// The minimum length of the segment.
        /// </value>
        public double MinimumSegmentLength { get; set; }
    }
}

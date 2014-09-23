// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleAdorner.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a rectangle adorner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Provides a rectangle adorner.
    /// </summary>
    public class RectangleAdorner : Adorner
    {
        /// <summary>
        /// The brush
        /// </summary>
        private Brush brush;

        /// <summary>
        /// The pen
        /// </summary>
        private Pen pen;

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The element to bind the adorner to.</param>
        public RectangleAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.brush = new SolidColorBrush(Color.FromArgb(40, 255, 255, 0));
            this.pen = new Pen(Brushes.Black, 1);
        }

        /// <summary>
        /// Gets or sets the rectangle.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        public Rect Rect { get; set; }

        /// <summary>
        /// Renders the adorner.
        /// </summary>
        /// <param name="dc">The drawing context.</param>
        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(this.brush, this.pen, this.Rect);
        }
    }
}
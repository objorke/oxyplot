// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Grid.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an infinite grid element.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an infinite grid element.
    /// </summary>
    public class Grid : ShapeElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        public Grid()
        {
            this.MajorThickness = -1;
            this.MinorThickness = -1;
            this.MajorDistance = 10;
            this.MinorDistance = 1;
            this.MajorColor = OxyColor.FromAColor(60, OxyColors.Blue);
            this.MinorColor = OxyColor.FromAColor(20, OxyColors.Blue);
        }

        /// <summary>
        /// Gets or sets the major thickness.
        /// </summary>
        /// <value>
        /// The major thickness.
        /// </value>
        public double MajorThickness { get; set; }

        /// <summary>
        /// Gets or sets the minor thickness.
        /// </summary>
        /// <value>
        /// The minor thickness.
        /// </value>
        public double MinorThickness { get; set; }

        /// <summary>
        /// Gets or sets the major distance.
        /// </summary>
        /// <value>
        /// The major distance.
        /// </value>
        public double MajorDistance { get; set; }

        /// <summary>
        /// Gets or sets the minor distance.
        /// </summary>
        /// <value>
        /// The minor distance.
        /// </value>
        public double MinorDistance { get; set; }

        /// <summary>
        /// Gets or sets the color of the major.
        /// </summary>
        /// <value>
        /// The color of the major.
        /// </value>
        public OxyColor MajorColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the minor.
        /// </summary>
        /// <value>
        /// The color of the minor.
        /// </value>
        public OxyColor MinorColor { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new GridPresenter(this, v);
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="Grid" />.
        /// </summary>
        private class GridPresenter : Presenter<Grid>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GridPresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public GridPresenter(Grid model, DrawingViewModel v)
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
                return new BoundingBox();
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
                var majorLineSegments = new List<ScreenPoint>();
                var minorLineSegments = new List<ScreenPoint>();

                var clientRect = this.DrawingViewModel.ClientArea;
                var p0 = this.InverseTransform(new ScreenPoint(clientRect.Left, clientRect.Bottom));
                var p1 = this.InverseTransform(new ScreenPoint(clientRect.Right, clientRect.Top));

                double x = (int)(p0.X / this.Model.MinorDistance) * this.Model.MinorDistance;
                while (x <= p1.X)
                {
                    var majorLine = Math.Abs((Math.Round(x / this.Model.MajorDistance) * this.Model.MajorDistance) - x) < 1e-6;
                    var q0 = this.Transform(x, p0.Y);
                    var q1 = this.Transform(x, p1.Y);
                    if (majorLine)
                    {
                        majorLineSegments.Add(q0);
                        majorLineSegments.Add(q1);
                    }
                    else
                    {
                        minorLineSegments.Add(q0);
                        minorLineSegments.Add(q1);
                    }

                    x += this.Model.MinorDistance;
                }

                double y = (int)(p0.Y / this.Model.MinorDistance) * this.Model.MinorDistance;
                while (y <= p1.Y)
                {
                    var majorLine = Math.Abs((Math.Round(y / this.Model.MajorDistance) * this.Model.MajorDistance) - y) < 1e-6;
                    var q0 = this.Transform(p0.X, y);
                    var q1 = this.Transform(p1.X, y);
                    if (majorLine)
                    {
                        majorLineSegments.Add(q0);
                        majorLineSegments.Add(q1);
                    }
                    else
                    {
                        minorLineSegments.Add(q0);
                        minorLineSegments.Add(q1);
                    }

                    y += this.Model.MinorDistance;
                }

                rc.DrawLineSegments(majorLineSegments, this.Model.MajorColor, this.Transform(this.Model.MajorThickness), aliased: true);
                rc.DrawLineSegments(minorLineSegments, this.Model.MinorColor, this.Transform(this.Model.MinorThickness), aliased: true);
            }
        }
    }
}
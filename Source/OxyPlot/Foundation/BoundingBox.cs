// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoundingBox.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a 2D bounding box.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a 2D bounding box.
    /// </summary>
    public class BoundingBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox"/> class.
        /// </summary>
        public BoundingBox()
        {
            this.MinimumX = double.NaN;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox"/> class.
        /// </summary>
        /// <param name="minimumX">The minimum x.</param>
        /// <param name="minimumY">The minimum y.</param>
        /// <param name="maximumX">The maximum x.</param>
        /// <param name="maximumY">The maximum y.</param>
        public BoundingBox(double minimumX, double minimumY, double maximumX, double maximumY)
        {
            this.MinimumX = minimumX;
            this.MinimumY = minimumY;
            this.MaximumX = maximumX;
            this.MaximumY = maximumY;
        }

        /// <summary>
        /// Gets or sets the minimum x coordinate.
        /// </summary>
        /// <value>
        /// The minimum x.
        /// </value>
        public double MinimumX { get; set; }

        /// <summary>
        /// Gets or sets the maximum x coordinate.
        /// </summary>
        /// <value>
        /// The maximum x.
        /// </value>
        public double MaximumX { get; set; }

        /// <summary>
        /// Gets or sets the minimum y coordinate.
        /// </summary>
        /// <value>
        /// The minimum y.
        /// </value>
        public double MinimumY { get; set; }

        /// <summary>
        /// Gets or sets the maximum y coordinate.
        /// </summary>
        /// <value>
        /// The maximum y.
        /// </value>
        public double MaximumY { get; set; }

        /// <summary>
        /// Unions the specified bb.
        /// </summary>
        /// <param name="bb">The bb.</param>
        public void Union(BoundingBox bb)
        {
            if (!bb.IsEmpty())
            {
                this.Union(bb.MinimumX, bb.MinimumY);
                this.Union(bb.MaximumX, bb.MaximumY);
            }
        }

        /// <summary>
        /// Includes the specified point in the bounding box.
        /// </summary>
        /// <param name="p">The point.</param>
        public void Union(DataPoint p)
        {
            this.Union(p.X, p.Y);
        }

        /// <summary>
        /// Includes the specified point with a translation in the bounding box.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <param name="dx">The x translation.</param>
        /// <param name="dy">The y translation.</param>
        public void Union(DataPoint p, double dx, double dy)
        {
            this.Union(p.X + dx, p.Y + dy);
        }

        /// <summary>
        /// Includes the specified point in the bounding box.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public void Union(double x, double y)
        {
            if (this.IsEmpty())
            {
                this.MinimumX = this.MaximumX = x;
                this.MinimumY = this.MaximumY = y;
                return;
            }

            if (x < this.MinimumX)
            {
                this.MinimumX = x;
            }

            if (x > this.MaximumX)
            {
                this.MaximumX = x;
            }

            if (y < this.MinimumY)
            {
                this.MinimumY = y;
            }

            if (y > this.MaximumY)
            {
                this.MaximumY = y;
            }
        }

        /// <summary>
        /// Determines whether the bounding box is empty.
        /// </summary>
        /// <returns><c>true</c> if the bounding box is empty.</returns>
        public bool IsEmpty()
        {
            return double.IsNaN(this.MinimumX);
        }
    }
}
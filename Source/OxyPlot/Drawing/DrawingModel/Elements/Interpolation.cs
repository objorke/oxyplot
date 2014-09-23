// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Interpolation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides interpolation methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides interpolation methods.
    /// </summary>
    public static class Interpolation
    {
        /// <summary>
        /// Creates an arc.
        /// </summary>
        /// <param name="c">The center of the arc.</param>
        /// <param name="rx">The x-axis radius.</param>
        /// <param name="ry">The y-axis radius.</param>
        /// <param name="t0">The start angle (degrees).</param>
        /// <param name="t1">The end angle (degrees).</param>
        /// <param name="n">The number of point.</param>
        /// <returns>A sequence of <see cref="DataPoint" />.</returns>
        public static IEnumerable<DataPoint> Arc(DataPoint c, double rx, double ry, double t0, double t1, int n = 40)
        {
            for (int i = 0; i < n; i++)
            {
                double t = t0 + ((t1 - t0) * i / (n - 1));
                var th = Math.PI / 180 * t;
                yield return new DataPoint(c.X + (Math.Cos(th) * rx), c.Y + (Math.Sin(th) * ry));
            }
        }

        /// <summary>
        /// Creates an arc.
        /// </summary>
        /// <param name="c">The center of the arc.</param>
        /// <param name="rx">The x-axis radius.</param>
        /// <param name="ry">The y-axis radius.</param>
        /// <param name="t0">The start angle (degrees).</param>
        /// <param name="t1">The end angle (degrees).</param>
        /// <param name="n">The number of point.</param>
        /// <returns>A sequence of <see cref="DataPoint" />.</returns>
        public static IEnumerable<ScreenPoint> Arc(ScreenPoint c, double rx, double ry, double t0, double t1, int n = 40)
        {
            for (int i = 0; i < n; i++)
            {
                double t = t0 + ((t1 - t0) * i / (n - 1));
                var th = Math.PI / 180 * t;
                yield return new ScreenPoint(c.X + (Math.Cos(th) * rx), c.Y + (Math.Sin(th) * ry));
            }
        }
    }
}
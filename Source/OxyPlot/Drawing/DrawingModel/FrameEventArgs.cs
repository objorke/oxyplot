// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameEventArgs.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides data for the <see cref="DrawingElement.Frame" /> event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="DrawingElement.Frame" /> event.
    /// </summary>
    public class FrameEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameEventArgs"/> class.
        /// </summary>
        /// <param name="nowTicks">The now ticks.</param>
        /// <param name="previousTicks">The previous ticks.</param>
        public FrameEventArgs(long nowTicks, long previousTicks)
        {
            this.CumulativeTime = TimeSpan.FromTicks(nowTicks);
            this.DeltaTime = TimeSpan.FromTicks(nowTicks - previousTicks);
        }

        /// <summary>
        /// Gets the cumulative time.
        /// </summary>
        /// <value>
        /// The cumulative time.
        /// </value>
        public TimeSpan CumulativeTime { get; private set; }

        /// <summary>
        /// Gets the delta time.
        /// </summary>
        /// <value>
        /// The delta time.
        /// </value>
        public TimeSpan DeltaTime { get; private set; }
    }
}
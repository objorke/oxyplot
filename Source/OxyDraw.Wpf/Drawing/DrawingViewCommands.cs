// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingViewCommands.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a standard set of commands for the <see cref="DrawingView" /> control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows.Input;

    /// <summary>
    /// Provides a standard set of commands for the <see cref="DrawingView" /> control.
    /// </summary>
    public static class DrawingViewCommands
    {
        /// <summary>
        /// Initializes static members of the <see cref="DrawingViewCommands"/> class.
        /// </summary>
        static DrawingViewCommands()
        {
            ZoomExtents = new RoutedCommand("ZoomExtents", typeof(DrawingViewCommands));
            ZoomExtents.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Alt));
            SelectAll = new RoutedCommand("SelectAll", typeof(DrawingViewCommands));
            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            SelectNone = new RoutedCommand("SelectNone", typeof(DrawingViewCommands));
            SelectNone.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            SelectInverse = new RoutedCommand("SelectInverse", typeof(DrawingViewCommands));
            SelectInverse.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Control));
        }

        /// <summary>
        /// Gets the zoom extents command.
        /// </summary>
        public static RoutedCommand ZoomExtents { get; private set; }

        /// <summary>
        /// Gets the select all command.
        /// </summary>
        public static RoutedCommand SelectAll { get; private set; }

        /// <summary>
        /// Gets the select none command.
        /// </summary>
        public static RoutedCommand SelectNone { get; private set; }

        /// <summary>
        /// Gets the select inverse command.
        /// </summary>
        public static RoutedCommand SelectInverse { get; private set; }
    }
}
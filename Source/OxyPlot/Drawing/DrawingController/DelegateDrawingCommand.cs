// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateDrawingCommand.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a controller command for the IDrawingView implemented by a delegate.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;

    /// <summary>
    /// Provides a controller command for the <see cref="IDrawingView" /> implemented by a delegate.
    /// </summary>
    /// <typeparam name="T">The type of the event arguments.</typeparam>
    public class DelegateDrawingCommand<T> : DelegateViewCommand<T>
        where T : OxyInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateDrawingCommand{T}" /> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public DelegateDrawingCommand(Action<IDrawingView, IController, T> handler)
            : base((v, c, e) => handler((IDrawingView)v, c, e))
        {
        }
    }
}
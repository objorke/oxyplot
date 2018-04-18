// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a drawing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a drawing.
    /// </summary>
    public class DrawingModel : Model
    {
        /// <summary>
        /// The elements
        /// </summary>
        private readonly IList<DrawingElement> elements;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingModel"/> class.
        /// </summary>
        public DrawingModel()
        {
            this.elements = new ElementCollection<DrawingElement>(this);
        }

        /// <summary>
        /// Occurs when the model is changed.
        /// </summary>
        public event EventHandler<ChangedEventArgs> Changed;

        /// <summary>
        /// Gets or sets the background color of the drawing.
        /// </summary>
        /// <value>
        /// The background color.
        /// </value>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Gets the elements of the drawing.
        /// </summary>
        /// <value>
        /// The elements.
        /// </value>
        public IList<DrawingElement> Elements
        {
            get
            {
                return this.elements;
            }
        }

        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="e">The element.</param>
        public void Add(DrawingElement e)
        {
            this.elements.Add(e);
        }

        /// <summary>
        /// Clears the drawing.
        /// </summary>
        public void Clear()
        {
            this.elements.Clear();
        }

        /// <summary>
        /// Removes the specified element.
        /// </summary>
        /// <param name="e">The element.</param>
        public void Remove(DrawingElement e)
        {
            this.elements.Remove(e);
        }

        /// <summary>
        /// Invalidates the model.
        /// </summary>
        public void Invalidate()
        {
            this.OnChanged(new ChangedEventArgs());
        }

        /// <summary>
        /// Gets all elements of the model, sorted by rendering priority.
        /// </summary>
        /// <returns>
        /// An enumerator of the elements.
        /// </returns>
        public override IEnumerable<UIElement> GetElements()
        {
            return this.elements;
        }

        /// <summary>
        /// Raises the <see cref="E:Changed" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnChanged(ChangedEventArgs args)
        {
            var handler = this.Changed;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}

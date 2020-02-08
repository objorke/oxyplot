// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an element of a DrawingModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;

    /// <summary>
    /// Represents an element of a <see cref="DrawingModel" />.
    /// </summary>
    public abstract class DrawingElement : UIElement, IDrawingElement
    {
        /// <summary>
        /// Occurs when the touch gesture is completed.
        /// </summary>
        public event EventHandler<FrameEventArgs> Frame;

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <value>
        /// The font weight.
        /// </value>
        public double FontWeight { get; set; }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>
        /// The font family.
        /// </value>
        public string FontFamily { get; set; }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>The presentation model.</returns>
        Presenter IDrawingElement.CreatePresentationModel(DrawingViewModel v)
        {
            return this.CreatePresenter(v);
        }

        /// <summary>
        /// Raises the <see cref="Frame" /> event.
        /// </summary>
        /// <param name="e">The <see cref="FrameEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnFrame(FrameEventArgs e)
        {
            var handler = this.Frame;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>The presentation model.</returns>
        protected abstract Presenter CreatePresenter(DrawingViewModel v);

        /// <summary>
        /// Provides a generic base class for element presentation models.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        public abstract class Presenter<T> : Presenter
            where T : DrawingElement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Presenter{T}" /> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The parent view model.</param>
            protected Presenter(T model, DrawingViewModel v)
                : base(v)
            {
                this.Model = model;
            }

            /// <summary>
            /// Gets the model.
            /// </summary>
            /// <value>
            /// The model.
            /// </value>
            protected T Model { get; private set; }
        }

        /// <summary>
        /// Provides an abstract base class for element presentation models.
        /// </summary>
        public abstract class Presenter
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Presenter"/> class.
            /// </summary>
            /// <param name="drawingViewModel">The drawing presentation model.</param>
            protected Presenter(DrawingViewModel drawingViewModel)
            {
                this.DrawingViewModel = drawingViewModel;
            }

            /// <summary>
            /// Gets the presentation model of the parent <see cref="DrawingModel" />.
            /// </summary>
            /// <value>
            /// The presentation model.
            /// </value>
            protected DrawingViewModel DrawingViewModel { get; private set; }

            /// <summary>
            /// Gets the bounding box of the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            /// <returns>The bounding box.</returns>
            public abstract BoundingBox GetBounds(IRenderContext rc);

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public abstract void Update(IRenderContext rc);

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public abstract void Render(IRenderContext rc);

            /// <summary>
            /// Tests if the element is hit by the specified point.
            /// </summary>
            /// <param name="args">The hit test arguments.</param>
            /// <returns>
            /// A hit test result.
            /// </returns>
            public HitTestResult HitTest(HitTestArguments args)
            {
                return this.HitTestOverride(args);
            }

            /// <summary>
            /// When overridden in a derived class, tests if the element is hit by the specified point.
            /// </summary>
            /// <param name="args">The hit test arguments.</param>
            /// <returns>
            /// The result of the hit test.
            /// </returns>
            protected virtual HitTestResult HitTestOverride(HitTestArguments args)
            {
                return null;
            }

            /// <summary>
            /// Transforms the specified point.
            /// </summary>
            /// <param name="p">The point to transform.</param>
            /// <returns>The transformed point.</returns>
            protected ScreenPoint Transform(DataPoint p)
            {
                return this.DrawingViewModel.Transform(p);
            }

            /// <summary>
            /// Transforms the specified point.
            /// </summary>
            /// <param name="x">The x coordinate of the point.</param>
            /// <param name="y">The y coordinate of the point.</param>
            /// <returns>
            /// The transformed point.
            /// </returns>
            protected ScreenPoint Transform(double x, double y)
            {
                return this.DrawingViewModel.Transform(x, y);
            }

            /// <summary>
            /// Transforms the specified length.
            /// </summary>
            /// <param name="p">The length to transform.</param>
            /// <returns>The transformed length.</returns>
            protected double Transform(double p)
            {
                return this.DrawingViewModel.Transform(p);
            }

            /// <summary>
            /// Inverse-transforms the specified point.
            /// </summary>
            /// <param name="p">The point.</param>
            /// <returns>The inverse-transformed point.</returns>
            protected DataPoint InverseTransform(ScreenPoint p)
            {
                return this.DrawingViewModel.InverseTransform(p);
            }

            /// <summary>
            /// Inverse-transforms the specified length.
            /// </summary>
            /// <param name="length">The length.</param>
            /// <returns>The inverse-transformed length.</returns>
            protected double InverseTransform(double length)
            {
                return this.DrawingViewModel.InverseTransform(length);
            }
        }
    }
}

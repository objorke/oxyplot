// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a DrawingModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using OxyPlot.Drawing;

    /// <summary>
    /// Represents a control that displays a <see cref="DrawingModel" />.
    /// </summary>
    public class DrawingView : GraphicsView, IDrawingView
    {
        /// <summary>
        /// Identifies the <see cref="DrawingPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DrawingPaddingProperty = DependencyProperty.Register(
            "DrawingPadding", typeof(double), typeof(DrawingView), new PropertyMetadata(10.0));

        /// <summary>
        /// Identifies the <see cref="Drawing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DrawingProperty = DependencyProperty.Register(
            "Drawing", typeof(DrawingModel), typeof(DrawingView), new PropertyMetadata(null, (s, e) => ((DrawingView)s).DrawingChanged()));

        /// <summary>
        /// The default controller.
        /// </summary>
        private IController defaultController;

        /// <summary>
        /// The canvas
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// The zoom adorner
        /// </summary>
        private RectangleAdorner zoomAdorner;

        /// <summary>
        /// The render context
        /// </summary>
        private IRenderContext rc;

        /// <summary>
        /// Initializes static members of the <see cref="DrawingView"/> class.
        /// </summary>
        static DrawingView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawingView), new FrameworkPropertyMetadata(typeof(DrawingView)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingView"/> class.
        /// </summary>
        public DrawingView()
        {
            this.Loaded += this.ViewLoaded;
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, (s, e) => this.Copy(), (s, e) => e.CanExecute = this.CanCopy()));
            this.CommandBindings.Add(new CommandBinding(DrawingViewCommands.ZoomExtents, (s, e) => this.ZoomExtents(), (s, e) => e.CanExecute = this.CanZoomExtents()));
        }

        /// <summary>
        /// Gets the actual plot controller.
        /// </summary>
        /// <value>The actual plot controller.</value>
        public override IController ActualController
        {
            get
            {
                return this.Controller ?? (this.defaultController ?? (this.defaultController = this.CreateDefaultController()));
            }
        }

        /// <summary>
        /// Gets or sets the drawing.
        /// </summary>
        /// <value>
        /// The drawing.
        /// </value>
        public DrawingModel Drawing
        {
            get
            {
                return (DrawingModel)this.GetValue(DrawingProperty);
            }

            set
            {
                this.SetValue(DrawingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the padding around the drawing.
        /// </summary>
        /// <value>
        /// The padding.
        /// </value>
        public double DrawingPadding
        {
            get
            {
                return (double)this.GetValue(DrawingPaddingProperty);
            }

            set
            {
                this.SetValue(DrawingPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets the actual model in the view.
        /// </summary>
        /// <value>
        /// The actual <see cref="Model" />.
        /// </value>
        public override Model ActualModel
        {
            get
            {
                return this.Drawing;
            }
        }

        /// <summary>
        /// Gets the actual view model.
        /// </summary>
        /// <value>
        /// The actual view model.
        /// </value>
        public DrawingViewModel ActualViewModel
        {
            get
            {
                return this.ViewModel as DrawingViewModel;
            }
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public override void HideZoomRectangle()
        {
            if (this.zoomAdorner != null)
            {
                var parentAdorner = AdornerLayer.GetAdornerLayer(this.canvas);
                parentAdorner.Remove(this.zoomAdorner);
                this.zoomAdorner = null;
            }
        }

        /// <summary>
        /// Zooms the extents.
        /// </summary>
        public void ZoomExtents()
        {
            this.ActualViewModel.ZoomExtents(this.rc);
        }

        /// <summary>
        /// Invalidates the view.
        /// </summary>
        void IDrawingView.Invalidate()
        {
            this.BeginInvoke(this.InvalidateArrange);
        }

        /// <summary>
        /// Gets the bounding box of the current drawing.
        /// </summary>
        /// <returns>The bounding box.</returns>
        public BoundingBox GetBounds()
        {
            return this.ActualViewModel.GetBounds(this.rc);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.canvas = (Canvas)this.GetTemplateChild("PART_Canvas");
            this.rc = new CanvasRenderContext(this.canvas);
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        public override void ShowZoomRectangle(OxyRect rect)
        {
            if (this.zoomAdorner == null)
            {
                var parentAdorner = AdornerLayer.GetAdornerLayer(this.canvas);
                this.zoomAdorner = new RectangleAdorner(this.canvas);
                parentAdorner.Add(this.zoomAdorner);
            }

            this.zoomAdorner.Rect = new Rect(rect.Left, rect.Top, rect.Width, rect.Height); // .ToPixelAlignedRect();
            this.zoomAdorner.InvalidateVisual();
        }

        /// <summary>
        /// Gets the element that the mouse position is relative to.
        /// </summary>
        /// <returns>
        /// The element.
        /// </returns>
        protected override IInputElement GetRelativeTo()
        {
            return this.canvas;
        }

        /// <summary>
        /// Called to arrange and size the content of a <see cref="T:System.Windows.Controls.Control" /> object.
        /// </summary>
        /// <param name="arrangeBounds">The computed size that is used to arrange the content.</param>
        /// <returns>
        /// The size of the control.
        /// </returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            this.Render();
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <summary>
        /// Creates the default controller.
        /// </summary>
        /// <returns>
        /// The default controller.
        /// </returns>
        protected override IController CreateDefaultController()
        {
            return new DrawingController();
        }

        /// <summary>
        /// Invokes the specified action on the UI Thread (without blocking the calling thread).
        /// </summary>
        /// <param name="action">The action.</param>
        private void BeginInvoke(Action action)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Determines whether this instance can zoom to extents.
        /// </summary>
        /// <returns><c>true</c> if zoom to extents is available; otherwise <c>false</c>.</returns>
        private bool CanZoomExtents()
        {
            return this.Drawing != null && this.ActualWidth > 0 && this.ActualHeight > 0 && !this.ActualViewModel.GetBounds(this.rc).IsEmpty();
        }

        /// <summary>
        /// Copies the current content of the view to the clipboard.
        /// </summary>
        private void Copy()
        {
            // Clipboard.SetImage(Drawing);
        }

        /// <summary>
        /// Determines whether the content of the view can be copied.
        /// </summary>
        /// <returns><c>true</c> if copy is available; otherwise <c>false</c>.</returns>
        private bool CanCopy()
        {
            return this.ActualViewModel != null;
        }

        /// <summary>
        /// Handles changes to the <see cref="Drawing" />.
        /// </summary>
        private void DrawingChanged()
        {
            if (this.Drawing == null)
            {
                this.ViewModel = null;
                return;
            }

            this.ViewModel = new DrawingViewModel(this, this.Drawing);
            this.ActualViewModel.ZoomExtents(this.rc);
        }

        /// <summary>
        /// Handles the loaded event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            this.Render();
        }

        /// <summary>
        /// Renders the content of the view.
        /// </summary>
        private void Render()
        {
            this.canvas.Children.Clear();
            this.canvas.Background = this.Drawing == null || this.Drawing.Background.IsInvisible()
                                               ? Brushes.Transparent
                                               : this.Drawing.Background.ToBrush();

            var vm = this.ActualViewModel;
            if (vm != null)
            {
                vm.Update(this.rc);
                vm.Render(this.rc);
            }
        }
    }
}

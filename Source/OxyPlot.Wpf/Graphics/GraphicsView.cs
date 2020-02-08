// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for graphics controls.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Provides an abstract base class for graphics controls.
    /// </summary>
    public abstract class GraphicsView : Control, IView
    {
        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IController), typeof(GraphicsView), new PropertyMetadata(null));

        /// <summary>
        /// The mouse down point
        /// </summary>
        private ScreenPoint mouseDownPoint;

        /// <summary>
        /// Gets the actual model in the view.
        /// </summary>
        /// <value>
        /// The actual <see cref="Model" />.
        /// </value>
        public abstract Model ActualModel { get; }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public IViewModel ViewModel { get; protected set; }

        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        /// <value>
        /// The client area rectangle.
        /// </value>
        OxyRect IView.ClientArea
        {
            get { return new OxyRect(0, 0, this.ActualWidth, this.ActualHeight); }
        }

        /// <summary>
        /// Gets or sets the plot controller.
        /// </summary>
        /// <value>The plot controller.</value>
        public IController Controller
        {
            get { return (IController)this.GetValue(ControllerProperty); }
            set { this.SetValue(ControllerProperty, value); }
        }

        /// <summary>
        /// Gets the actual plot controller.
        /// </summary>
        /// <value>The actual plot controller.</value>
        public abstract IController ActualController { get; }

        ///// <summary>
        ///// Gets the actual plot controller.
        ///// </summary>
        ///// <value>The actual plot controller.</value>
        //public IController ActualController
        //{
        //    get
        //    {
        //        return this.Controller ?? (this.defaultController ?? (this.defaultController = this.CreateDefaultController()));
        //    }
        //}

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType(OxyPlot.CursorType cursorType)
        {
            switch (cursorType)
            {
                case OxyPlot.CursorType.Pan:
                    this.Cursor = Cursors.Hand;
                    break;
                case OxyPlot.CursorType.ZoomRectangle:
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case OxyPlot.CursorType.ZoomHorizontal:
                    this.Cursor = Cursors.SizeWE;
                    break;
                case OxyPlot.CursorType.ZoomVertical:
                    this.Cursor = Cursors.SizeNS;
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;
            }
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public virtual void HideZoomRectangle()
        {
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="zoomRectangle">The zoom rectangle.</param>
        public virtual void ShowZoomRectangle(OxyRect zoomRectangle)
        {
        }

        /// <summary>
        /// Invoked when an unhandled KeyDown attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled)
            {
                return;
            }

            var args = new OxyKeyEventArgs { ModifierKeys = Keyboard.GetModifierKeys(), Key = e.Key.Convert() };
            e.Handled = this.ActualController.HandleKeyDown(this, args);
        }

        /// <summary>
        /// Creates the default controller.
        /// </summary>
        /// <returns>The default controller.</returns>
        protected abstract IController CreateDefaultController();

        /// <summary>
        /// Invoked when an unhandled MouseDown attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Handled)
            {
                return;
            }

            this.Focus();
            this.CaptureMouse();

            // store the mouse down point, check it when mouse button is released to determine if the context menu should be shown
            this.mouseDownPoint = e.GetPosition(this).ToScreenPoint();

            e.Handled = this.ActualController.HandleMouseDown(this, e.ToMouseDownEventArgs(this, this.GetRelativeTo()));
        }

        /// <summary>
        /// Gets the element that the mouse position is relative to.
        /// </summary>
        /// <returns>The element.</returns>
        protected virtual IInputElement GetRelativeTo()
        {
            return this;
        }

        /// <summary>
        /// Invoked when an unhandled MouseMove attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseMove(this, e.ToMouseEventArgs(this, this.GetRelativeTo()));
        }

        /// <summary>
        /// Invoked when an unhandled MouseUp routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the mouse button was released.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Handled)
            {
                return;
            }

            this.ReleaseMouseCapture();

            if (this.ActualController.HandleMouseUp(this, e.ToMouseReleasedEventArgs(this, this.GetRelativeTo())))
            {
                e.Handled = true;
                return;
            }

            // Open the context menu
            var p = e.GetPosition(this).ToScreenPoint();
            double d = p.DistanceTo(this.mouseDownPoint);

            if (this.ContextMenu != null)
            {
                if (Math.Abs(d) < 1e-8 && e.ChangedButton == MouseButton.Right)
                {
                    // TODO: why is the data context not passed to the context menu??
                    this.ContextMenu.DataContext = this.DataContext;

                    this.ContextMenu.Visibility = Visibility.Visible;
                    this.ContextMenu.IsOpen = true;
                }
                else
                {
                    this.ContextMenu.Visibility = Visibility.Collapsed;
                    this.ContextMenu.IsOpen = false;
                }
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseEnter(this, e.ToMouseEventArgs(this, this.GetRelativeTo()));
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseLeave(this, e.ToMouseEventArgs(this, this.GetRelativeTo()));
        }

        /// <summary>
        /// Invoked when an unhandled MouseWheel attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseWheel(this, e.ToMouseWheelEventArgs(this.GetRelativeTo()));
        }
    }
}

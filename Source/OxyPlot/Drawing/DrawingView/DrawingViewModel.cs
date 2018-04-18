// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a presentation model for a <see cref="DrawingModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a presentation model for a <see cref="DrawingModel" />.
    /// </summary>
    public class DrawingViewModel : IDrawingViewModel
    {
        /// <summary>
        /// The drawing
        /// </summary>
        private readonly DrawingModel drawing;

        /// <summary>
        /// The view
        /// </summary>
        private readonly IDrawingView view;

        /// <summary>
        /// The element to element presentation model map
        /// </summary>
        private readonly Dictionary<DrawingElement, DrawingElement.Presenter> elementMap;

        /// <summary>
        /// The x-offset.
        /// </summary>
        private double ox;

        /// <summary>
        /// The y-offset.
        /// </summary>
        private double oy;

        /// <summary>
        /// The scale.
        /// </summary>
        private double scale;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingViewModel"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="drawing">The drawing model.</param>
        public DrawingViewModel(IDrawingView view, DrawingModel drawing)
        {
            this.view = view;
            this.drawing = drawing;
            this.drawing.Changed += this.HandleChangedDrawing;
            this.elementMap = new Dictionary<DrawingElement, DrawingElement.Presenter>();
        }

        /// <summary>
        /// Gets the client rectangle.
        /// </summary>
        /// <value>
        /// The client rectangle.
        /// </value>
        public OxyRect ClientArea
        {
            get
            {
                return this.view.ClientArea;
            }
        }

        /// <summary>
        /// Gets the element presentation models.
        /// </summary>
        /// <value>
        /// The element presentation models.
        /// </value>
        public IEnumerable<DrawingElement.Presenter> ElementPresentationModels
        {
            get
            {
                foreach (var e in this.drawing.Elements)
                {
                    DrawingElement.Presenter vm;
                    if (!this.elementMap.TryGetValue(e, out vm))
                    {
                        vm = ((IDrawingElement)e).CreatePresentationModel(this);
                        this.elementMap[e] = vm;
                    }

                    yield return vm;
                }
            }
        }

        /// <summary>
        /// Returns the elements that are hit at the specified position.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A sequence of hit results.
        /// </returns>
        public IEnumerable<HitTestResult> HitTest(HitTestArguments args)
        {
            // Revert the order to handle the top-level elements first
            foreach (var element in this.ElementPresentationModels.Reverse())
            {
                var result = element.HitTest(args);
                if (result != null)
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Transforms the specified point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The transformed point.</returns>
        public ScreenPoint Transform(DataPoint p)
        {
            return new ScreenPoint((p.X - this.ox) * this.scale, (this.oy - p.Y) * this.scale);
        }

        /// <summary>
        /// Pans the view by the specified vector.
        /// </summary>
        /// <param name="delta">The panning vector.</param>
        public void Pan(ScreenVector delta)
        {
            this.ox -= delta.X / this.scale;
            this.oy += delta.Y / this.scale;
            this.view.Invalidate();
        }

        /// <summary>
        /// Pans the view by the specified vector.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <param name="current">The current position.</param>
        public void Pan(ScreenVector delta, ScreenPoint current)
        {
            this.Pan(delta);
        }

        /// <summary>
        /// Transforms the specified point.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The transformed point.</returns>
        public ScreenPoint Transform(double x, double y)
        {
            return new ScreenPoint((x - this.ox) * this.scale, (this.oy - y) * this.scale);
        }

        /// <summary>
        /// Transforms the specified length.
        /// </summary>
        /// <param name="t">The length.</param>
        /// <returns>The transformed length.</returns>
        public double Transform(double t)
        {
            return t < 0 ? -t : t * this.scale;
        }

        /// <summary>
        /// Inverses the transform.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The transformed point.</returns>
        public DataPoint InverseTransform(double x, double y)
        {
            return new DataPoint((x / this.scale) + this.ox, this.oy - (y / this.scale));
        }

        /// <summary>
        /// Inverse-transforms the specified point.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>The transformed point.</returns>
        public DataPoint InverseTransform(ScreenPoint p)
        {
            return new DataPoint((p.x / this.scale) + this.ox, this.oy - (p.y / this.scale));
        }

        /// <summary>
        /// Inverse-transforms the specified length.
        /// </summary>
        /// <param name="t">The length.</param>
        /// <returns>The transformed length.</returns>
        public double InverseTransform(double t)
        {
            return this.scale.Equals(0) || double.IsNaN(this.scale) ? 0 : t / this.scale;
        }

        /// <summary>
        /// Invalidates the view.
        /// </summary>
        public void Invalidate()
        {
            this.view.Invalidate();
        }

        /// <summary>
        /// Resets the view.
        /// </summary>
        public void Reset()
        {
            this.view.ZoomExtents();
            this.view.Invalidate();
        }

        /// <summary>
        /// Zooms the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        public void Zoom(OxyRect rect)
        {
            var p0 = this.InverseTransform(rect.Left, rect.Bottom);
            var p1 = this.InverseTransform(rect.Right, rect.Top);
            var padding = this.view.DrawingPadding;
            var sx = (this.view.ClientArea.Width - (padding * 2)) / (p1.X - p0.X);
            var sy = (this.view.ClientArea.Height - (padding * 2)) / (p1.Y - p0.Y);
            this.scale = Math.Min(sx, sy);
            this.ox = ((p0.X + p1.X) * 0.5) - ((this.view.ClientArea.Center.X + padding) / this.scale);
            this.oy = ((p0.Y + p1.Y) * 0.5) + ((this.view.ClientArea.Center.Y + padding) / this.scale);
            this.view.Invalidate();
        }

        /// <summary>
        /// Zooms the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public void Zoom(double delta)
        {
            this.ZoomAt(new ScreenVector(delta, delta), this.view.ClientArea.Center);
        }

        /// <summary>
        /// Zooms at the specified position.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <param name="p">The position.</param>
        public void ZoomAt(ScreenVector delta, ScreenPoint p)
        {
            var newscale = this.scale * delta.Y;
            var x = this.scale > 0 ? (p.X / this.scale) + this.ox : 0;
            var y = this.scale > 0 ? this.oy - (p.Y / this.scale) : 0;

            this.ox = x - ((x - this.ox) * this.scale / newscale);
            this.oy = y - ((y - this.oy) * this.scale / newscale);
            this.scale = newscale;

            this.view.Invalidate();
        }

        /// <summary>
        /// Zooms the extents.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public void ZoomExtents(IRenderContext rc)
        {
            this.scale = 1;
            this.ox = this.oy = 0;
            if (this.drawing == null)
            {
                return;
            }

            if (this.view.ClientArea.Width <= 0 || this.view.ClientArea.Height <= 0)
            {
                this.scale = double.NaN;
            }

            var bbox = this.GetBounds(rc);
            if (bbox == null || bbox.IsEmpty())
            {
                return;
            }

            var rx = bbox.MaximumX - bbox.MinimumX;
            var ry = bbox.MaximumY - bbox.MinimumY;
            var padding = this.view.DrawingPadding;
            var w = this.view.ClientArea.Width - (padding * 2);
            var h = this.view.ClientArea.Height - (padding * 2);
            var sx = rx > 0 ? w / rx : 1;
            var sy = ry > 0 ? h / ry : 1;

            this.scale = Math.Min(sx, sy);

            this.ox = ((bbox.MaximumX + bbox.MinimumX) * 0.5) - (this.view.ClientArea.Width * 0.5 / this.scale);
            this.oy = ((bbox.MaximumY + bbox.MinimumY) * 0.5) + (this.view.ClientArea.Height * 0.5 / this.scale);

            this.view.Invalidate();
        }

        /// <summary>
        /// Updates the element view models.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public void Update(IRenderContext rc)
        {
            foreach (var e in this.ElementPresentationModels)
            {
                e.Update(rc);
            }
        }

        /// <summary>
        /// Gets the bounding box.
        /// </summary>
        /// <param name="rc">The render context..</param>
        /// <returns>The bounding box.</returns>
        public BoundingBox GetBounds(IRenderContext rc)
        {
            var bbox = new BoundingBox();
            foreach (var e in this.ElementPresentationModels)
            {
                bbox.Union(e.GetBounds(rc));
            }

            return bbox;
        }

        /// <summary>
        /// Renders the drawing by the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public void Render(IRenderContext rc)
        {
            try
            {
                foreach (var e in this.drawing.Elements)
                {
                    DrawingElement.Presenter vm;
                    if (!this.elementMap.TryGetValue(e, out vm))
                    {
                        vm = ((IDrawingElement)e).CreatePresentationModel(this);
                        this.elementMap[e] = vm;
                    }

                    vm.Render(rc);

                    // this.RenderBounds(rc, vm);
                }
            }
            catch (Exception e)
            {
                rc.DrawText(new ScreenPoint(10, 10), e.Message, OxyColors.Red);
            }
            finally
            {
                rc.CleanUp();
            }
        }

        /// <summary>
        /// Renders the bounding box for the specified element.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="vm">The element presentation model.</param>
        public void RenderBounds(IRenderContext rc, DrawingElement.Presenter vm)
        {
            var bounds = vm.GetBounds(rc);
            var p0 = this.Transform(bounds.MinimumX, bounds.MinimumY);
            var p1 = this.Transform(bounds.MaximumX, bounds.MaximumY);
            var rect = new OxyRect(p0, p1);
            rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Blue);
        }

        /// <summary>
        /// Handles the changed drawing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ChangedEventArgs"/> instance containing the event data.</param>
        private void HandleChangedDrawing(object sender, ChangedEventArgs e)
        {
            this.Invalidate();
        }
    }
}
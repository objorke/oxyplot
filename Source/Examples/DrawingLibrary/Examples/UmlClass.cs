namespace OxyPlot.Drawing
{
    using System;
    using System.Linq;

    public class UmlClassBox : ShapeElement
    {
        public DataPoint Position { get; set; }
        public string Title { get; set; }
        public string[] Properties { get; set; }
        public string[] Methods { get; set; }
        public string FontFamily { get; set; }
        public double FontSize { get; set; }

        public UmlClassBox()
        {
            this.FontFamily = "Consolas";
            this.FontSize = 6;
            this.Position = new DataPoint(0, 0);
        }

        protected override DrawingElementViewModel CreatePresenter(DrawingViewModel v)
        {
            return new ViewModel(this, v);
        }

        private class ViewModel : DrawingElementViewModel
        {
            private readonly UmlClassBox model;

            private readonly IDrawingViewModel v;

            private ScreenPoint position;

            private double fontSize;

            public ViewModel(UmlClassBox model, IDrawingViewModel v)
            {
                this.model = model;
                this.v = v;
            }

            public override BoundingBox GetBounds(IRenderContext rc)
            {
                var position = v.Transform(this.model.Position);
                var fontSize = v.Transform(this.model.FontSize);
                var titleSize = rc.MeasureText(this.model.Title, this.model.FontFamily, fontSize, FontWeights.Bold);
                var propertySizes = this.model.Properties.Select(p => rc.MeasureText(p, this.model.FontFamily, fontSize)).ToArray();
                var methodSizes = this.model.Methods.Select(p => rc.MeasureText(p, this.model.FontFamily, fontSize)).ToArray();
                var maxWidth = Math.Max(titleSize.Width, Math.Max(propertySizes.Max(p => p.Width), methodSizes.Max(p => p.Width)));
                var totalHeight = titleSize.Height + propertySizes.Sum(p => p.Height) + methodSizes.Sum(p => p.Height);

                maxWidth = v.InverseTransform(maxWidth + fontSize / 2);
                totalHeight = v.InverseTransform(totalHeight);
                var bb = new BoundingBox();
                bb.Union(this.model.Position);
                bb.Union(this.model.Position.X + maxWidth, this.model.Position.Y - totalHeight);
                return bb;
            }

            public override void Update(IRenderContext rc)
            {
                position = v.Transform(this.model.Position);
                fontSize = v.Transform(this.model.FontSize);
            }

            public override void Render(IRenderContext rc)
            {
                var x = position.X + 5;
                var y = position.Y;
                rc.DrawText(new ScreenPoint(x, y), this.model.Title, OxyColors.Black, this.model.FontFamily, fontSize, FontWeights.Bold);
                var titleSize = rc.MeasureText(this.model.Title, this.model.FontFamily, fontSize, FontWeights.Bold);
                y += titleSize.Height;
                var y0 = y;
                double maxWidth = titleSize.Width;
                foreach (var p in this.model.Properties)
                {
                    rc.DrawText(new ScreenPoint(x, y), p, OxyColors.Black, this.model.FontFamily, fontSize);
                    var size = rc.MeasureText(p, this.model.FontFamily, fontSize);
                    y += size.Height;
                    maxWidth = Math.Max(maxWidth, size.Width);
                }

                var y1 = y;
                foreach (var p in this.model.Methods)
                {
                    rc.DrawText(new ScreenPoint(x, y), p, OxyColors.Black, this.model.FontFamily, fontSize);
                    var size = rc.MeasureText(p, this.model.FontFamily, fontSize);
                    y += size.Height;
                    maxWidth = Math.Max(maxWidth, size.Width);
                }

                var rect = new OxyRect(position.X, position.Y, maxWidth + fontSize / 2, y - position.Y);
                rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Black, 1);
                rc.DrawLineSegments(new[]
                {
                    new ScreenPoint(position.X, y0), new ScreenPoint(rect.Right, y0), 
                    new ScreenPoint(position.X, y1), new ScreenPoint(rect.Right, y1), 
                    
                }, OxyColors.Black, 1);
            }
        }
    }
}

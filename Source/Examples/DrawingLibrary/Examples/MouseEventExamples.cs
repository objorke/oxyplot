namespace DrawingDemo
{
    using OxyPlot;
    using OxyPlot.Drawing;

    public static class MouseEventExamples
    {
        [Example("MouseEvents: Circle")]
        public static Example CircleFromThreePoints()
        {
            var drawing = new DrawingModel();
            var p1 = drawing.AddPoint(new DataPoint(0, 0), OxyColors.Red);
            p1.FontSize = 120;
            p1.FontWeight = FontWeights.Bold;
            var originalFill = OxyColors.Undefined;
            p1.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    p1.Text = "Pressed";
                    originalFill = p1.Fill;
                    p1.Fill = OxyColors.Red;
                    drawing.Invalidate();
                    e.Handled = true;
                }
            };
            p1.MouseUp += (s, e) =>
            {
                p1.Text = null;
                p1.Fill = originalFill;
                drawing.Invalidate();
                e.Handled = true;
            };

            return new Example(drawing);
        }
    }
}
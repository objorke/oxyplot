namespace DrawingDemo
{
    using OxyPlot;
    using OxyPlot.Drawing;

    public class Example
    {
        public Example(DrawingModel model, IController controller = null)
        {
            this.Model = model;
            this.Controller = controller;
        }

        public DrawingModel Model { get; set; }

        public IController Controller { get; set; }
    }
}
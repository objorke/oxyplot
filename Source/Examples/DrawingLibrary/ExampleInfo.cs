namespace DrawingDemo
{
    using System;

    using OxyPlot.Drawing;

    public class ExampleInfo
    {
        private readonly Func<Example> exampleGetter;

        private Example example;

        public ExampleInfo(string title, Func<Example> exampleGetter)
        {
            this.exampleGetter = exampleGetter;
            this.Title = title;
        }

        public string Title { get; set; }

        public DrawingModel Model
        {
            get
            {
                if (this.example == null)
                {
                    this.example = this.exampleGetter();
                }

                return this.example.Model;
            }
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
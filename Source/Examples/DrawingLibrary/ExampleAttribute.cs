namespace DrawingDemo
{
    using System;

    public class ExampleAttribute : Attribute
    {
        public ExampleAttribute(string title)
        {
            this.Title = title;
        }

        public string Title { get; set; }
    }
}
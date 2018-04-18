namespace DemoCore
{
    using System;

    public class DemoAttribute : Attribute
    {
        public DemoAttribute(string description)
            : this(null, description)
        {
        }

        public DemoAttribute(string title, string description)
        {
            this.Title = title;
            this.Description = description;
        }

        public string Title { get; private set; }

        public string Description { get; private set; }
    }
}
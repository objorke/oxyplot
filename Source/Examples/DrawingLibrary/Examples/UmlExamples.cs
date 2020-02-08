namespace DrawingDemo
{
    using OxyPlot.Drawing;

    public static class UmlExamples
    {
        [Example("UML diagram")]
        public static Example Uml()
        {
            var drawing = new DrawingModel();
            drawing.Add(new UmlClassBox { Title = "BankAccount", Properties = new[] { "owner : String", "balance : Dollars = 0" }, Methods = new[] { "deposit ( amount : Dollars )", "withdrawal ( amount : Dollars )" } });
            return new Example(drawing);
        }
    }
}
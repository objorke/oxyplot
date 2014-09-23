namespace DrawingDemo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Examples
    {
        public static IEnumerable<ExampleInfo> Get()
        {
            foreach (var type in typeof(Examples).GetTypeInfo().Assembly.DefinedTypes)
            {
                foreach (var method in type.AsType().GetRuntimeMethods())
                {
                    var exampleAttributes = method.GetCustomAttributes(typeof(ExampleAttribute), true).ToArray();
                    if (exampleAttributes.Length == 1)
                    {
                        var m = method;
                        yield return
                            new ExampleInfo(
                                ((ExampleAttribute)exampleAttributes[0]).Title,
                                () => (Example)m.Invoke(null, null));
                    }
                }
            }
        }
    }
}
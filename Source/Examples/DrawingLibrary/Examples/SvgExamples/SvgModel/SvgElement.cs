namespace SvgLibrary
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a base class for elements.
    /// </summary>
    public abstract class SvgElement
    {
        public SvgElement()
        {
            this.Style = new SvgStyle();
            this.Transform = new SvgTransform();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("style")]
        public string StyleAttribute
        {
            get
            {
                return this.Style.ToString();
            }

            set
            {
                this.Style.Set(value);
            }
        }

        [XmlIgnore]
        public SvgStyle Style { get; set; }

        [XmlIgnore]
        public SvgTransform Transform { get; set; }

        [XmlAttribute("transform")]
        public string TransformAttribute
        {
            get
            {
                return Transform.ToString();
            }
            set
            {
                this.Transform.Set(value);
            }
        }

        [XmlAttribute("stroke-width")]
        public double StrokeWidth
        {
            get
            {
                return this.Style.StrokeWidth;
            }

            set
            {
                this.Style.StrokeWidth = value;
            }
        }

        [XmlAttribute("stroke")]
        public string Stroke
        {
            get
            {
                return this.Style.Stroke;
            }

            set
            {
                this.Style.Stroke = value;
            }
        }

        [XmlAttribute("fill")]
        public string Fill
        {
            get
            {
                return this.Style.Fill;
            }

            set
            {
                this.Style.Fill = value;
            }
        }
    }

    public class SvgTransform
    {
        public SvgTransform()
        {
            this.A = 1;
            this.D = 1;
        }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }
        public double E { get; set; }
        public double F { get; set; }

        public void Set(string transform)
        {
            // https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/transform

            foreach (Match m in Regex.Matches(transform, @"(matrix)\((.*)\)"))
            {
                var values = m.Groups[2].Value.Split(", ".ToCharArray()).Select(v => double.Parse(v, CultureInfo.InvariantCulture)).ToArray();
                if (m.Groups[1].Value == "matrix")
                {
                    this.A = values[0];
                    this.B = values[1];
                    this.C = values[2];
                    this.D = values[3];
                    this.E = values[4];
                    this.F = values[5];
                }
            }
        }

        public SvgTransform Append(SvgTransform x)
        {
            var output = new SvgTransform
            {
                A = this.A * x.A + this.C * x.B,
                B = this.B * x.A + this.D * x.B,
                C = this.A * x.C + this.C * x.D,
                D = this.B * x.C + this.D * x.D,
                E = this.A * x.E + this.C * x.F + this.E,
                F = this.B * x.E + this.D * x.F + this.F
            };
            return output;
        }

        public void Transform(double x, double y, out double xx, out double yy)
        {
            xx = this.A * x + this.C * y + this.E;
            yy = this.B * x + this.D * y + this.F;
        }
    }

    public class SvgStyle
    {
        public SvgStyle()
        {
            this.StrokeWidth = double.NaN;
        }

        public double StrokeWidth { get; set; }

        public string Stroke { get; set; }

        public string Fill { get; set; }

        public void Set(string style)
        {
            foreach (var item in style.Split(';'))
            {
                var keyValue = item.Split(':');
                switch (keyValue[0])
                {
                    case "fill":
                        this.Fill = keyValue[1];
                        break;
                    case "stroke":
                        this.Stroke = keyValue[1];
                        break;
                    case "stroke-width":
                        this.StrokeWidth = double.Parse(keyValue[1], CultureInfo.InvariantCulture);
                        break;
                }
            }
        }

        public override string ToString()
        {
            var b = new StringBuilder();
            if (this.Fill != null) b.Append("fill:" + this.Fill + ";");
            if (this.Stroke != null) b.Append("stroke:" + this.Stroke + ";");
            if (!double.IsNaN(this.StrokeWidth)) b.Append("stroke-width:" + this.StrokeWidth.ToString(CultureInfo.InvariantCulture) + ";");
            return base.ToString();
        }

        public SvgStyle Append(SvgStyle s)
        {
            var computedStyle = new SvgStyle()
            {
                Fill = this.Fill,
                Stroke = this.Stroke,
                StrokeWidth = this.StrokeWidth
            };

            if (s.Fill != null) computedStyle.Fill = s.Fill;
            if (s.Stroke != null) computedStyle.Stroke = s.Stroke;
            if (!double.IsNaN(s.StrokeWidth)) computedStyle.StrokeWidth = s.StrokeWidth;

            // default values
            if (double.IsNaN(computedStyle.StrokeWidth)) computedStyle.StrokeWidth = 1;
            return computedStyle;
        }
    }
}
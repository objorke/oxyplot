namespace DrawingLibrary.Examples
{
    using System;

    using OxyPlot;

    public class NacaAirfoil
    {
        public NacaAirfoil()
        {
            this.MaximumCamber = 0.02;
            this.PositionOfMaximumCamber = 0.4;
            this.Thickness = 0.12;
        }

        public NacaAirfoil(string id)
        {
            this.MaximumCamber = int.Parse(id.Substring(0, 1)) * 0.01;
            this.PositionOfMaximumCamber = int.Parse(id.Substring(1, 1)) * 0.1;
            this.Thickness = int.Parse(id.Substring(2, 2)) * 0.01;
        }

        public double Thickness { get; set; }

        public double MaximumCamber { get; set; }

        public double PositionOfMaximumCamber { get; set; }

        public override string ToString()
        {
            return string.Format("NACA {0}{1}{2}", (int)(this.MaximumCamber * 100), (int)(this.PositionOfMaximumCamber * 10), (int)(this.Thickness * 100));
        }

        public void GetProfile(int n, double c, out DataPoint[] camberLine, out DataPoint[] thickness, out DataPoint[] upper, out DataPoint[] lower)
        {
            camberLine = new DataPoint[n];
            for (int i = 0; i < n; i++)
            {
                double beta = Math.PI * i / (n - 1);
                double x = c * (1 - Math.Cos(beta)) / 2;
                camberLine[i] = new DataPoint(x, Yc(c, x, this.MaximumCamber, this.PositionOfMaximumCamber));
            }

            upper = new DataPoint[n];
            lower = new DataPoint[n];
            thickness = new DataPoint[n];

            for (int i = 0; i < n; i++)
            {
                var i0 = i > 0 ? i - 1 : i;
                var i1 = i < n - 1 ? i + 1 : i;
                var theta = Math.Atan2(camberLine[i1].Y - camberLine[i0].Y, camberLine[i1].X - camberLine[i0].X);

                double x = camberLine[i].X;
                var yt = Yt(c, x, this.Thickness);
                thickness[i] = new DataPoint(x, yt);
                upper[i] = new DataPoint(x - yt * Math.Sin(theta), camberLine[i].Y + yt * Math.Cos(theta));
                lower[i] = new DataPoint(x + yt * Math.Sin(theta), camberLine[i].Y - yt * Math.Cos(theta));
            }
        }

        private static double Yt(double c, double x, double t)
        {
            var xc = x / c;
            return t / 0.2 * c * ((0.2969 * Math.Sqrt(xc)) - (0.126 * xc) - (0.3516 * xc * xc) + (0.2843 * xc * xc * xc) - (0.1015 * xc * xc * xc * xc));
        }

        private static double Yc(double c, double x, double m, double p)
        {
            if (x < p * c)
            {
                return m * x / (p * p) * ((2 * p) - (x / c));
            }

            return m * (c - x) / ((1 - p) * (1 - p)) * (1 + (x / c) - (2 * p));
        }
    }
}

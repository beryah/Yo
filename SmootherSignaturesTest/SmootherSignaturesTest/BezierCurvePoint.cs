using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmootherSignaturesTest
{
    public class BezierCurvePoint
    {
        public BezierCurvePoint(double x = 0, double y = 0)
        {
            this.X = x;
            this.Y = y;
            this.Time = DateTime.Now;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public DateTime Time { get; set; }

        public double VelocityFrom(BezierCurvePoint start)
        {
            return this.Time != start.Time ? this.DistanceTo(start) / (this.Time - start.Time).Milliseconds : 0;
        }

        private double DistanceTo(BezierCurvePoint point)
        {
            return Math.Sqrt(Math.Pow(this.X - point.X, 2) + Math.Pow(this.Y - point.Y, 2));
        }
    }
}

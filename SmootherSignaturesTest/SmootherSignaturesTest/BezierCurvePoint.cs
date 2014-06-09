using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmootherSignaturesTest
{
    public class BezierCurvePoint
    {
        public BezierCurvePoint()
        {
            this.Time = DateTime.Now;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public DateTime Time { get; set; }

        public double Size { get; set; }

        public double VelocityFrom(BezierCurvePoint start)
        {
            return this.Time != start.Time ? this.DistanceTo(start) / (this.Time - start.Time).Seconds : 1;
        }

        private double DistanceTo(BezierCurvePoint point)
        {
            return Math.Sqrt(Math.Pow(this.X - point.X, 2) + Math.Pow(this.Y - point.Y, 2));
        }
    }
}

using System;
namespace SmootherSignaturesTest
{
    public class BezierCurve
    {
        public BezierCurve(
            BezierCurvePoint startPoint,
            BezierCurvePoint controlPoint1,
            BezierCurvePoint controlPoint2,
            BezierCurvePoint endPoint)
        {
            this.StartPoint = startPoint;
            this.ControlPoint1 = controlPoint1;
            this.ControlPoint2 = controlPoint2;
            this.EndPoint = endPoint;
        }

        public BezierCurvePoint StartPoint { get; private set; }

        public BezierCurvePoint ControlPoint1 { get; private set; }

        public BezierCurvePoint ControlPoint2 { get; private set; }

        public BezierCurvePoint EndPoint { get; private set; }

        public double Length
        {
            get
            {
                var steps = 10d;
                var length = 0d;
                var p = new BezierCurvePoint();

                for (int i = 0; i < steps; i++)
                {
                    var t = i / steps;
                    var c = this.GetPoint(t);

                    if (i > 0)
                    {
                        var dx = c.X - p.X;
                        var dy = c.Y - p.Y;
                        var a = dx * dx + dy * dy;
                        length += Math.Sqrt((double)a);
                    }

                    p = c;
                }

                return length;
            }
        }

        private BezierCurvePoint GetPoint(double t)
        {
            var tt = t * t;
            var ttt = tt * t;

            var u = 1 - t;
            var uu = u * u;
            var uuu = uu * u;

            var x = this.StartPoint.X * uuu + 3 * this.ControlPoint1.X * uu * t + 3 * this.ControlPoint2.X * u * tt + this.EndPoint.X * ttt;
            var y = this.StartPoint.Y * uuu + 3 * this.ControlPoint1.Y * uu * t + 3 * this.ControlPoint2.Y * u * tt + this.EndPoint.Y * ttt;
            return new BezierCurvePoint(x, y);
        }
    }
}

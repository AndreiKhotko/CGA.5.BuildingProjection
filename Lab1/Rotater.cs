using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    static class Rotater
    {
        public static SpaceFloatPoint RotateZ(SpaceFloatPoint point, double angle = Math.PI / 180)
        {
            var newPoint = new SpaceFloatPoint();
            newPoint.X = (float)(point.X * Math.Cos(angle) - point.Y * Math.Sin(angle));
            newPoint.Y = (float)(point.X * Math.Sin(angle) + point.Y * Math.Cos(angle));
            newPoint.Z = point.Z;
            return newPoint;
        }

        public static SpaceFloatPoint RotateX(SpaceFloatPoint point, double angle = Math.PI / 180)
        {
            var newPoint = new SpaceFloatPoint();
            newPoint.X = point.X;
            newPoint.Y = (float)((point.Y * Math.Cos(angle) - point.Z * Math.Sin(angle)));
            newPoint.Z = (float)((point.Y * Math.Sin(angle) + point.Z * Math.Cos(angle)));
            return newPoint;
        }

        public static SpaceFloatPoint RotateY(SpaceFloatPoint point, double angle = Math.PI / 180)
        {
            var newPoint = new SpaceFloatPoint();
            newPoint.X = (float)((point.Z * Math.Sin(angle) + point.X * Math.Cos(angle)));
            newPoint.Y = point.Y;
            newPoint.Z = (float)((point.Z * Math.Cos(angle) - point.X * Math.Sin(angle)));
            return newPoint;
        }
    }
}

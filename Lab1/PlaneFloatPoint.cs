using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public struct PlaneFloatPoint
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PlaneFloatPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            PlaneFloatPoint floatPoint;
            try
            {
                floatPoint = (PlaneFloatPoint)obj;
            }
            catch
            {
                return false;
            }
            return this.X == floatPoint.X && this.Y == floatPoint.Y;
        }
    }
}

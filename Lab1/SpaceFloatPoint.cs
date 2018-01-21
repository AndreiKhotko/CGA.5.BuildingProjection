using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class SpaceFloatPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public SpaceFloatPoint(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SpaceFloatPoint()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public override bool Equals(object obj)
        {
            SpaceFloatPoint floatPoint;
            try
            {
                floatPoint = (SpaceFloatPoint)obj;
            }
            catch
            {
                return false;
            }
            return this.X == floatPoint.X && this.Y == floatPoint.Y && this.Z == floatPoint.Z;
        }
    }
}

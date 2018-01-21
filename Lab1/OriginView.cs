using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class OriginView
    {
        private static OriginView originView;
        const int DEFAULT_WINDOW_HEIGHT = 800;
        public double AxisX { get; set; }
        public double AxisZ { get; set; }
        public float P { get; set; }
        public float D { get; set; }
        public float EyeD { get; set; }
        public float TanView { get; set; }
        public float R { get { return D*TanView; } }
        public CameraPosition position { get; set; }

        private static object syncRoot = new Object();

        protected OriginView()
        {
            AxisX = 0;
            AxisZ = 0;
            P = 200;
            D = 100;
            EyeD = 0;
            TanView = 1;
            position = CameraPosition.Middle;
        }

        public static OriginView GetInstance()
        {
            if (originView == null)
            {
                lock (syncRoot)
                {
                    if (originView == null)
                        originView = new OriginView();
                }
            }
            return originView;
        } 

        public PlaneFloatPoint ToFloatPoint(SpaceFloatPoint spacePoint)
        {
            switch (position)
            {
                case CameraPosition.Left:
                    return ToFloatPointForLeftCamera(spacePoint);
                case CameraPosition.Middle:
                    return ToFloatPointForMiddleCamera(spacePoint);
                case CameraPosition.Right:
                    return ToFloatPointForRightCamera(spacePoint);
                default:
                    return ToFloatPointForMiddleCamera(spacePoint);
            }
        }

        public Point ToIntPoint(SpaceFloatPoint spacePoint)
        {
            switch (position)
            {
                case CameraPosition.Left:
                    return ToIntPointForLeftCamera(spacePoint);
                case CameraPosition.Middle:
                    return ToIntPointForMiddleCamera(spacePoint);
                case CameraPosition.Right:
                    return ToIntPointForRightCamera(spacePoint);
                default:
                    return ToIntPointForMiddleCamera(spacePoint);
            }
        }

        private PlaneFloatPoint ToFloatPointForMiddleCamera(SpaceFloatPoint spacePoint)
        {
            var dest = GetDestination(spacePoint);
            var k = P - dest != 0 ? D / (P - dest) : D > 0 ? float.PositiveInfinity : float.NegativeInfinity;
            //var k = 1;
            return new PlaneFloatPoint(DEFAULT_WINDOW_HEIGHT / 2 + k * GetOriginViewX(spacePoint), DEFAULT_WINDOW_HEIGHT / 2 - k * GetOriginViewY(spacePoint));
        }

        private Point ToIntPointForMiddleCamera(SpaceFloatPoint spacePoint)
        {
            var floatPoint = ToFloatPointForMiddleCamera(spacePoint);
            return new Point((int)Math.Round(floatPoint.X), (int)Math.Round(floatPoint.Y));
        }

        private PlaneFloatPoint ToFloatPointForLeftCamera(SpaceFloatPoint spacePoint)
        {
            var dest = GetDestination(spacePoint);
            var k = P - dest != 0 ? D / (P - dest) : D > 0 ? float.PositiveInfinity : float.NegativeInfinity;
            //var k = 1;
            return new PlaneFloatPoint(DEFAULT_WINDOW_HEIGHT / 2 + k * (GetOriginViewX(spacePoint) + EyeD/2), DEFAULT_WINDOW_HEIGHT / 2 - k * GetOriginViewY(spacePoint));
        }

        private Point ToIntPointForLeftCamera(SpaceFloatPoint spacePoint)
        {
            var floatPoint = ToFloatPointForLeftCamera(spacePoint);
            return new Point((int)Math.Round(floatPoint.X), (int)Math.Round(floatPoint.Y));
        }

        private PlaneFloatPoint ToFloatPointForRightCamera(SpaceFloatPoint spacePoint)
        {
            var dest = GetDestination(spacePoint);
            var k = P - dest != 0 ? D / (P - dest) : D > 0 ? float.PositiveInfinity : float.NegativeInfinity;
            //var k = 1;
            return new PlaneFloatPoint(DEFAULT_WINDOW_HEIGHT / 2 + k * (GetOriginViewX(spacePoint) - EyeD/2), DEFAULT_WINDOW_HEIGHT / 2 - k * GetOriginViewY(spacePoint));
        }

        private Point ToIntPointForRightCamera(SpaceFloatPoint spacePoint)
        {
            var floatPoint = ToFloatPointForRightCamera(spacePoint);
            return new Point((int)Math.Round(floatPoint.X), (int)Math.Round(floatPoint.Y));
        }

        public bool IsInCamera(float x, float y)
        {
            var X = x - DEFAULT_WINDOW_HEIGHT / 2;
            var Y = y - DEFAULT_WINDOW_HEIGHT / 2;
            return (X >= -R / 2 && X <= R / 2 && Y >= -R / 2 && Y <= R / 2);
        }

        private float GetOriginViewX(SpaceFloatPoint spacePoint)
        {
            var X = spacePoint.X;
            var Y = spacePoint.Y;
            var cosX = (float)Math.Cos(AxisX);
            var sinX = (float)Math.Sin(AxisX);
            return Y * cosX + X * sinX;
        }

        private float GetOriginViewY(SpaceFloatPoint spacePoint)
        {
            var X = spacePoint.X;
            var Y = spacePoint.Y;
            var Z = spacePoint.Z;
            var cosX = (float)Math.Cos(AxisX);
            var sinX = (float)Math.Sin(AxisX);
            var cosZ = (float)Math.Cos(AxisZ);
            var sinZ = (float)Math.Sin(AxisZ);
            return Z*sinZ - X*cosZ*cosX + Y*cosZ*sinX;
        }

        private float GetDestination(SpaceFloatPoint spacePoint)
        {
            var X = spacePoint.X;
            var Y = spacePoint.Y;
            var Z = spacePoint.Z;
            var cosX = (float)Math.Cos(AxisX);
            var sinX = (float)Math.Sin(AxisX);
            var cosZ = (float)Math.Cos(AxisZ);
            var sinZ = (float)Math.Sin(AxisZ);

            var prYX = X * cosX + Y * sinX;
            return Z * cosZ + prYX * sinZ;
        }


    }
}

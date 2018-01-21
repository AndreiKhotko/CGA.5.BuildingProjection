using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class CustomObject
    {
        private const int LEFT_HOLE = 2;
        private const int RIGHT_HOLE = 4;

        private List<CustomRectangle> outCubeSides = new List<CustomRectangle>();
        private SpaceFloatPoint center = new SpaceFloatPoint();
        public float W { get; private set; }
        public float L { get { return 4 * W; } }
        public float H { get { return 0.7f * W; } }
        
        public SpaceFloatPoint pA
        {
            get
            {
                return outCubeSides[1].Points[1];
            }
        }

        public SpaceFloatPoint pB
        {
            get
            {
                return outCubeSides[0].Points[3];
            }
        }

        public CustomObject(float width, CustomRectangle.DrawFunc drawVisible, CustomRectangle.DrawFunc drawHidden, CustomRectangle.LinePointsFunc getPoints)
        {
            W = width;
            outCubeSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    new SpaceFloatPoint( W / 2,  L / 2,  H / 2),
                    new SpaceFloatPoint( W / 2, -L / 2,  H / 2),
                    new SpaceFloatPoint(-W / 2, -L / 2,  H / 2),
                    new SpaceFloatPoint(-W / 2,  L / 2,  H / 2),
                }, drawVisible, drawHidden, getPoints
            ));
            outCubeSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    new SpaceFloatPoint( W / 2,  L / 2, -H / 2),
                    new SpaceFloatPoint( W / 2, -L / 2, -H / 2),
                    new SpaceFloatPoint( W / 2, -L / 2,  H / 2),
                    new SpaceFloatPoint( W / 2,  L / 2,  H / 2),
                }, drawVisible, drawHidden, getPoints
            ));
            outCubeSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    new SpaceFloatPoint(-W / 2,  L / 2,  H / 2),
                    new SpaceFloatPoint(-W / 2, -L / 2,  H / 2),
                    new SpaceFloatPoint(-W / 2, -L / 2, -H / 2),
                    new SpaceFloatPoint(-W / 2,  L / 2, -H / 2),
                }, drawVisible, drawHidden, getPoints
            ));
            outCubeSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    new SpaceFloatPoint(-W / 2,  L / 2, -H / 2),
                    new SpaceFloatPoint(-W / 2, -L / 2, -H / 2),
                    new SpaceFloatPoint( W / 2, -L / 2, -H / 2),
                    new SpaceFloatPoint( W / 2,  L / 2, -H / 2),
                }, drawVisible, drawHidden, getPoints
            ));
            outCubeSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    new SpaceFloatPoint( W / 2,  L / 2,  H / 2),
                    new SpaceFloatPoint(-W / 2,  L / 2,  H / 2),
                    new SpaceFloatPoint(-W / 2,  L / 2, -H / 2),
                    new SpaceFloatPoint( W / 2,  L / 2, -H / 2),
                }, drawVisible, drawHidden, getPoints
            ));
            outCubeSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    new SpaceFloatPoint( W / 2, -L / 2,  H / 2),
                    new SpaceFloatPoint( W / 2, -L / 2, -H / 2),
                    new SpaceFloatPoint(-W / 2, -L / 2, -H / 2),
                    new SpaceFloatPoint(-W / 2, -L / 2,  H / 2),
                }, drawVisible, drawHidden, getPoints
            ));
        }

        public void RotateX(double angle = Math.PI / 180)
        {
            foreach (CustomRectangle cr in outCubeSides)
            {
                cr.Points = cr.Points.Select(p => Rotater.RotateX(p, angle)).ToList();
            }
            center = Rotater.RotateX(center, angle);
        }

        public void RotateY(double angle = Math.PI / 180)
        {
            foreach (CustomRectangle cr in outCubeSides)
            {
                cr.Points = cr.Points.Select(p => Rotater.RotateY(p, angle)).ToList();
            }
            center = Rotater.RotateY(center, angle);
        }

        public void RotateZ(double angle = Math.PI / 180)
        {
            foreach (CustomRectangle cr in outCubeSides)
            {
                cr.Points = cr.Points.Select(p => Rotater.RotateZ(p, angle)).ToList();
            }
            center = Rotater.RotateZ(center, angle);
        }

        public void Displace(float dx, float dy, float dz)
        {
            foreach (CustomRectangle cr in outCubeSides)
            {
                cr.Points.ForEach(p =>
                {
                    p.X += dx;
                    p.Y += dy;
                    p.Z += dz;
                });
            }
            center.X += dx;
            center.Y += dy;
            center.Z += dz;
        }

        public void RotateAxis(double angle = Math.PI / 180)
        {
            var pointA = new SpaceFloatPoint(pA.X, pA.Y, pA.Z);
            Displace(-pointA.X, -pointA.Y, -pointA.Z);
            var axisA = Math.Acos(pB.Z / Math.Sqrt(pB.Z * pB.Z + pB.Y * pB.Y));
            var Y = pB.Y;
            RotateX(axisA);
            var isPosRotateX = true;
            if (Math.Abs(pB.Y) > Math.Abs(Y))
            {
                RotateX(-2 * axisA);
                isPosRotateX = false;
            }

            var axisB = Math.Acos(pB.Z / Math.Sqrt(pB.Z * pB.Z + pB.X * pB.X));
            var X = pB.X;
            RotateY(axisB);
            var isPosRotateY = true;
            if (Math.Abs(pB.X) > Math.Abs(X))
            {
                RotateY(-2*axisB);
                isPosRotateY = false;
            }

            RotateZ(angle);

            if (isPosRotateY)
            {
                RotateY(-axisB);
            }
            else
            {
                RotateY(axisB);
            }

            if (isPosRotateX)
            {
                RotateX(-axisA);
            }
            else
            {
                RotateX(axisA);
            }

            Displace(pointA.X, pointA.Y, pointA.Z);
        }

        public void DrawOutCube()
        {
            CustomRectangle cr;
            for (int i = 0; i < 4; i++)
            {
                cr = outCubeSides[i];
                if (cr.IsCounterClockPoints())
                {
                    //cr.DrawAsDoubleFilled(Color.Red, Color.Blue);
                    cr.DrawAsVisible();
                }
            }
            cr = outCubeSides[4];
            if (cr.IsCounterClockPoints())
            {
                cr.DrawAsFilled(Color.Red);
                cr.DrawAsVisible();
            }
            cr = outCubeSides[5];
            if (cr.IsCounterClockPoints())
            {
                cr.DrawAsFilled(Color.Blue);
                cr.DrawAsVisible();
            }
        }
    }
}

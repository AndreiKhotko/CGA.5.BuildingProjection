using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class CustomShape
    {
        private const int LEFT_HOLE = 2;
        private const int RIGHT_HOLE = 4;

        private const int EDGE_BACK = 0;
        private const int EDGE_FRONT = 1;
        private const int EDGE_LEFT = 2;
        private const int EDGE_RIGHT = 3;
        private const int EDGE_TOP = 4;

        private List<CustomRectangle> outSides = new List<CustomRectangle>();
        private SpaceFloatPoint center = new SpaceFloatPoint();
        public float H { get; private set; }
        public float D => 1.5f * H;
        public float A => 4.5f * H;


        public SpaceFloatPoint pA
        {
            get
            {
                return outSides[1].Points[1];
            }
        }

        public SpaceFloatPoint pB
        {
            get
            {
                return outSides[0].Points[2];
            }
        }

        public CustomShape(float h, CustomRectangle.DrawFunc drawVisible, CustomRectangle.DrawFunc drawHidden, CustomRectangle.LinePointsFunc getPoints)
        {
            H = h;

            float ra = (float)Math.Sqrt(3) * A / 6;
            float Ra = (float)Math.Sqrt(3) * A / 3;
            float rd = (float)Math.Sqrt(3) * D / 6;
            float Rd = (float)Math.Sqrt(3) * D / 3;

            var p0 = new SpaceFloatPoint(-A / 2, -ra, -H / 2);
            var p1 = new SpaceFloatPoint(0, Ra, -H / 2);
            var p2 = new SpaceFloatPoint(A / 2, -ra, -H / 2);
            var p3 = new SpaceFloatPoint(p0.X, p0.Y, H / 2);
            var p4 = new SpaceFloatPoint(p1.X, p1.Y, H / 2);
            var p5 = new SpaceFloatPoint(p2.X, p2.Y, H / 2);
            
            var p6 = new SpaceFloatPoint(-D / 2, -rd, -H / 2);
            var p7 = new SpaceFloatPoint( 0, rd, -H / 2);
            var p8 = new SpaceFloatPoint( D / 2, -rd, -H / 2);
            var p9 = new SpaceFloatPoint(p6.X, p6.Y, H / 2);
            var p10 = new SpaceFloatPoint(p7.X, p7.Y, H / 2);
            var p11 = new SpaceFloatPoint(p8.X, p8.Y, H / 2);

            outSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    p2, p1, p0
                }, drawVisible, drawHidden, getPoints
            ));
            outSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    p3, p4, p5
                }, drawVisible, drawHidden, getPoints
            ));
            outSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    p1, p4, p3, p0
                }, drawVisible, drawHidden, getPoints
            ));
            outSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    p4, p1, p2, p5
                }, drawVisible, drawHidden, getPoints
            ));
            outSides.Add(new CustomRectangle(
                new List<SpaceFloatPoint>
                {
                    p3, p5, p2, p0
                }, drawVisible, drawHidden, getPoints
            ));
        }

        public void RotateX(double angle = Math.PI / 180)
        {
            foreach (CustomRectangle cr in outSides)
            {
                cr.Points = cr.Points.Select(p => Rotater.RotateX(p, angle)).ToList();
            }
            center = Rotater.RotateX(center, angle);
        }

        public void RotateY(double angle = Math.PI / 180)
        {
            foreach (CustomRectangle cr in outSides)
            {
                cr.Points = cr.Points.Select(p => Rotater.RotateY(p, angle)).ToList();
            }
            center = Rotater.RotateY(center, angle);
        }

        public void RotateZ(double angle = Math.PI / 180)
        {
            foreach (CustomRectangle cr in outSides)
            {
                cr.Points = cr.Points.Select(p => Rotater.RotateZ(p, angle)).ToList();
            }
            center = Rotater.RotateZ(center, angle);
        }

        public void Displace(float dx, float dy, float dz)
        {
            foreach (CustomRectangle cr in outSides)
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
                RotateY(-2 * axisB);
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
            for (int i = 0; i < outSides.Count; i++)
            {
                cr = outSides[i];

                Color color = Color.White;

                if (cr.IsCounterClockPoints())
                {
                    if (i == EDGE_BACK || i == EDGE_FRONT)
                        color = Color.Red;
                    else if (i == EDGE_LEFT)
                        color = Color.Green;
                    else if (i == EDGE_RIGHT)
                        color = Color.Yellow;
                    else if (i == EDGE_TOP)
                        color = Color.Blue;

                    cr.DrawAsFilled(color);
                    cr.DrawAsVisible();
                }
            }
        }
    }
}

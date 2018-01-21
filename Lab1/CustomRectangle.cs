using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class CustomRectangle
    {
        public List<SpaceFloatPoint> Points { get; set; }
        public delegate void DrawFunc(int x0, int y0, int x1, int y1, Color clr);
        public delegate IEnumerable<Point> LinePointsFunc(int x0, int y0, int x1, int y1);
        private DrawFunc DrawVisible { get; set; }
        private DrawFunc DrawHidden { get; set; }
        private LinePointsFunc GetPoints { get; set; }
        private PointYComparer comparer = new PointYComparer();

        public CustomRectangle(List<SpaceFloatPoint> points, DrawFunc drawVisible, DrawFunc drawHidden, LinePointsFunc getPoints)
        {
            if (drawHidden == null || drawVisible == null || getPoints == null) throw new Exception();
            DrawVisible = drawVisible;
            DrawHidden = drawHidden;
            GetPoints = getPoints;
            if (points == null)
            {
                Points = new List<SpaceFloatPoint>();
            }
            else
            {
                Points = points;
            }
        }

        public void DrawAsFilled(Color color)
        {
            var originView = OriginView.GetInstance();
            var points = Points.Select(p => originView.ToIntPoint(p)).ToList();

            var tr1 = new List<Point>() { points[0], points[1], points[2] };
            tr1.Sort(comparer);
            FillTriangle(tr1, color);
            
            if (points.Count == 4)
            {
                var tr2 = new List<Point>() { points[3], points[2], points[0] };
                tr2.Sort(comparer);

                FillTriangle(tr2, color);
            }
            
            DrawVisible(points[0].X, points[0].Y, points[2].X, points[2].Y, color);
        }
        /*
        public void DrawAsDoubleFilled(Color color1, Color color2)
        {
            var originView = OriginView.GetInstance();
            var points = Points.Select(p => originView.ToIntPoint(p)).ToList();
            var m1 = GetMiddlePoint(points[0].X, points[0].Y, points[1].X, points[1].Y);
            var m2 = GetMiddlePoint(points[2].X, points[2].Y, points[3].X, points[3].Y);
            var tr1 = new List<Point>() { points[0], m1, m2 };
            var tr2 = new List<Point>() { points[3], m2, points[0] };
            var tr3 = new List<Point>() { points[1], m1, m2 };
            var tr4 = new List<Point>() { points[2], m2, points[1] };
            tr1.Sort(comparer);
            tr2.Sort(comparer);
            tr3.Sort(comparer);
            tr4.Sort(comparer);
            FillTriangle(tr1, color1);
            FillTriangle(tr2, color1);
            FillTriangle(tr3, color2);
            FillTriangle(tr4, color2);
            DrawVisible(points[0].X, points[0].Y, m2.X, m2.Y, color1);
            DrawVisible(points[1].X, points[1].Y, m2.X, m2.Y, color2);
            DrawVisible(m1.X, m1.Y, m2.X, m2.Y, color2);
        }
        */
        public void DrawAsVisible()
        {
            var originView = OriginView.GetInstance();
            var points = Points.Select(p => originView.ToIntPoint(p)).ToList();
            for (int i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % points.Count;
                DrawVisible(points[i].X, points[i].Y, points[j].X, points[j].Y, Color.Black);
            }
        }

        public void DrawAsHidden()
        {
            var originView = OriginView.GetInstance();
            var points = Points.Select(p => originView.ToIntPoint(p)).ToList();
            for (int i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % points.Count;
                DrawHidden(points[i].X, points[i].Y, points[j].X, points[j].Y, Color.Black);
            }
        }

        public void DrawAsCombine(CustomRectangle windowRectangle)
        {
            var originView = OriginView.GetInstance();
            var windowPoints = windowRectangle.Points.Select(p => originView.ToFloatPoint(p)).ToList();
            var points = Points.Select(p => originView.ToIntPoint(p)).ToList();
            for (int i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % points.Count;
                DrawHidden(points[i].X, points[i].Y, points[j].X, points[j].Y, Color.Black);
                var visibleLine = CBAlgoritm.GetVisible(points[i].X, points[i].Y, points[j].X, points[j].Y, windowPoints);
                if (visibleLine.Length == 2)
                {
                    var intVisible = visibleLine.Select(p => new Point((int)Math.Round(p.X), (int)Math.Round(p.Y))).ToList();
                    DrawVisible(intVisible[0].X, intVisible[0].Y, intVisible[1].X, intVisible[1].Y, Color.Black);
                }
            }
        }

        public bool IsCounterClockPoints()
        {
            var originView = OriginView.GetInstance();
            float S = 0;
            var points = Points.Select(p => originView.ToFloatPoint(p)).ToList();
            for (int i = 0; i < points.Count; i++)
            {
                var j = (i + 1) % points.Count;
                S += GetOpr(new PlaneFloatPoint(0, 0), points[i], points[j]);
            }
            if (S > 0) return true;
            return false;
        }

        private float GetOpr(PlaneFloatPoint P1, PlaneFloatPoint P2, PlaneFloatPoint P3)
        {
            return P1.X * P2.Y * 1 +
            P1.Y * P3.X * 1 +
            P2.X * P3.Y * 1 -
            P3.X * P2.Y * 1 -
            P2.X * P1.Y * 1 -
            P3.Y * P1.X * 1;        
        }

        private void FillTriangle(List<Point> points, Color color)
        {
            var p1 = GetPoints(points[0].X, points[0].Y, points[1].X, points[1].Y).ToList();
            var p2 = GetPoints(points[0].X, points[0].Y, points[2].X, points[2].Y).ToList();
            var p3 = GetPoints(points[2].X, points[2].Y, points[1].X, points[1].Y).ToList();
            var middleY = p1.LastOrDefault().Y;
            for (int y = p1[0].Y; y <= middleY; y++)
            {
                var lp1 = p1.Find(m => m.Y == y);
                var lp2 = p2.Find(m => m.Y == y);
                DrawVisible(lp1.X, lp1.Y, lp2.X, lp2.Y, color);
            }
            for (int y = p3[0].Y; y >= middleY; y--)
            {
                var lp2 = p2.Find(m => m.Y == y);
                var lp3 = p3.Find(m => m.Y == y);
                DrawVisible(lp2.X, lp2.Y, lp3.X, lp3.Y, color);
            }
        }

        private Point GetMiddlePoint(int x1, int y1, int x2, int y2)
        {
            var p = GetPoints(x1, y1, x2, y2).ToList();
            return p[p.Count() / 2];
        }
    }
}

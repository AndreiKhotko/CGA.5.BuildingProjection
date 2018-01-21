using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class PointYComparer: IComparer<Point>
    {
        public int Compare(Point p1, Point p2)
        {
            return p1.Y - p2.Y;
        }
    }
}

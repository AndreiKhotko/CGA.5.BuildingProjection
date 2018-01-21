using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    static class CBAlgoritm
    {
        const int VERTEX_COUNT = 4;

        public static PlaneFloatPoint[] GetVisible(float x0, float y0, float x1, float y1, List<PlaneFloatPoint> wp)
        {
            int i, j, visible, kw;
            float xn, yn, dx, dy, r;
            float CB_t0, CB_t1;
            float Qx, Qy;
            float Nx, Ny;
            float Pn, Qn;
            float xt0 = -1, yt0 = -1, xt1 = -1, yt1 = -1;

            kw = wp.Count;
            var np = new List<PlaneFloatPoint>();
            for (i = 0; i < kw; i++)
            {
                j = (i + 1) % kw;
                np.Add(GetNormalPoint(wp[i].X, wp[i].Y, wp[j].X, wp[j].Y));
            }

            visible = 1;
            CB_t0 = 0; CB_t1 = 1;
            dx = x1 - x0;
            xn = x0;
            dy = y1 - y0;
            yn = y0;

            for (i = 0; i < kw; ++i)
            {
                Qx = xn - wp[i].X;
                Qy = yn - wp[i].Y;
                Nx = np[i].X;
                Ny = np[i].Y;
                Pn = dx * Nx + dy * Ny;
                Qn = Qx * Nx + Qy * Ny;

                if (Pn == 0)
                {
                    if (Qn > 0)
                    {
                        visible = 0;
                        break;
                    }
                }
                else
                {
                    r = -Qn / Pn;
                    if (Pn < 0)
                    {          
                        if (r < CB_t0) { visible = 0; break; }
                        if (r < CB_t1) CB_t1 = r;
                    }
                    else
                    {
                        if (r > CB_t1) { visible = 0; break; }
                        if (r > CB_t0) CB_t0 = r;
                    }
                }
            }
            if (visible == 1)
            {
                if (CB_t0 > CB_t1) visible = 0;
                else
                {
                    if (CB_t0 > 0)
                    {
                        xt0 = xn + CB_t0 * dx;
                        yt0 = yn + CB_t0 * dy;
                    }
                    else
                    {
                        xt0 = xn;
                        yt0 = yn;
                    }
                    if (CB_t1 < 1)
                    {
                        xt1 = xn + CB_t1 * dx;
                        yt1 = yn + CB_t1 * dy;
                    }
                    else
                    {
                        xt1 = xn + dx;
                        yt1 = yn + dy;
                    }
                }
            }
            return visible == 1 ? new PlaneFloatPoint[] { new PlaneFloatPoint(xt0, yt0), new PlaneFloatPoint(xt1, yt1) } : new PlaneFloatPoint[0];
        }

        private static PlaneFloatPoint GetNormalPoint(float x0, float y0, float x1, float y1)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            return new PlaneFloatPoint() { X = dy, Y = -dx };
        }
    }
}

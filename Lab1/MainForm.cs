using System;
using System.Threading;
using System.Windows.Forms;
using SDL2;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Lab1
{
    public partial class MainForm : Form
    {
        const int DEFAULT_WINDOW_WIDTH = 800;
        const int DEFAULT_WINDOW_HEIGHT = 800;

        const int ROTATE_ANGLE = 5;
        const int DASH_STEP = 5;

        const double DELTA_ANGLE = Math.PI / 30;

        int windowWidth = DEFAULT_WINDOW_WIDTH;
        int windowHeight = DEFAULT_WINDOW_HEIGHT;

        private IntPtr renderer;

        public MainForm()
        {
            InitializeComponent();
            Thread thread = new Thread(() =>
            {
                SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
                IntPtr wnd = SDL.SDL_CreateWindow(
                    "Лабораторная работа 5", 
                    20 , 20,
                    windowWidth, windowHeight, 
                    SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
                );
                renderer = SDL.SDL_CreateRenderer(wnd, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

                CustomObject cf = new CustomObject(100, DrawLine, DrawDashLine, GetPoints);
                CustomShape cs = new CustomShape(150, DrawLine, DrawDashLine, GetPoints);
                var originView = OriginView.GetInstance();
                originView.AxisX = Math.PI / 4;
                originView.AxisZ = Math.PI / 6;
                originView.D = 1000;
                originView.P = 2000;
                originView.EyeD = 0;
                originView.TanView = 0.4f;
                originView.position = CameraPosition.Middle;
                var check = true;
                while (check)
                {
                    SDL.SDL_Event sdlEvent;
                    SDL.SDL_PollEvent(out sdlEvent);
                    switch (sdlEvent.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                        {
                            check = false;
                            break;
                        }
                        case SDL.SDL_EventType.SDL_KEYDOWN:
                        {
                            switch (sdlEvent.key.keysym.sym) {
                                case SDL.SDL_Keycode.SDLK_e:
                                    cs.RotateX(DELTA_ANGLE);
                                    break;
                                case SDL.SDL_Keycode.SDLK_q:
                                    cs.RotateX(-DELTA_ANGLE);
                                    break;
                                case SDL.SDL_Keycode.SDLK_d:
                                    cs.RotateY(DELTA_ANGLE);
                                    break;
                                case SDL.SDL_Keycode.SDLK_a:
                                    cs.RotateY(-DELTA_ANGLE);
                                    break;
                                case SDL.SDL_Keycode.SDLK_c:
                                    cs.RotateZ(DELTA_ANGLE);
                                    break;
                                case SDL.SDL_Keycode.SDLK_z:
                                    cs.RotateZ(-DELTA_ANGLE);
                                    break;
                                case SDL.SDL_Keycode.SDLK_w:
                                    cs.RotateAxis(DELTA_ANGLE);
                                    break;
                                case SDL.SDL_Keycode.SDLK_x:
                                    cs.RotateAxis(-DELTA_ANGLE);
                                    break;
                                case SDL.SDL_Keycode.SDLK_UP:
                                    if ((originView.AxisZ - DELTA_ANGLE) >= -Math.PI) originView.AxisZ -= DELTA_ANGLE;
                                    break;
                                case SDL.SDL_Keycode.SDLK_DOWN:
                                    if ((originView.AxisZ + DELTA_ANGLE) <= Math.PI) originView.AxisZ += DELTA_ANGLE;
                                    break;
                                case SDL.SDL_Keycode.SDLK_LEFT:
                                    if ((originView.AxisX - DELTA_ANGLE) >= -Math.PI) originView.AxisX -= DELTA_ANGLE;
                                    break;
                                case SDL.SDL_Keycode.SDLK_RIGHT:
                                    if ((originView.AxisX + DELTA_ANGLE) <= Math.PI) originView.AxisX += DELTA_ANGLE;
                                    break;
                                case SDL.SDL_Keycode.SDLK_s:
                                    if (originView.position == CameraPosition.Left)
                                    {
                                        originView.position = CameraPosition.Middle;
                                    }
                                    else if (originView.position == CameraPosition.Middle)
                                    {
                                        originView.position = CameraPosition.Right;
                                    }
                                    else if (originView.position == CameraPosition.Right)
                                    {
                                        originView.position = CameraPosition.Left;
                                    }
                                    break;
                                case SDL.SDL_Keycode.SDLK_1:
                                    originView.P += 10;
                                    break;
                                case SDL.SDL_Keycode.SDLK_2:
                                    originView.P -= 10;
                                    break;
                                case SDL.SDL_Keycode.SDLK_3:
                                    originView.D += 10;
                                    break;
                                case SDL.SDL_Keycode.SDLK_4:
                                    originView.D -= 10;
                                    break;
                                case SDL.SDL_Keycode.SDLK_5:
                                    originView.TanView += 0.05f;
                                    break;
                                case SDL.SDL_Keycode.SDLK_6:
                                    originView.TanView -= 0.05f;
                                    break;
                                case SDL.SDL_Keycode.SDLK_7:
                                    originView.EyeD += 5;
                                    break;
                                case SDL.SDL_Keycode.SDLK_8:
                                    originView.EyeD -= 5;
                                    break;
                            }
                            break;
                        }
                    }
                    SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 0);
                    SDL.SDL_RenderClear(renderer);
                    cs.DrawOutCube();
                    DrawScreen(originView.R);
                    SDL.SDL_RenderPresent(renderer);
                }
                SDL.SDL_DestroyRenderer(renderer);
                SDL.SDL_DestroyWindow(wnd);
                SDL.SDL_Quit();

            });
            thread.Start();
            thread.Join();
        }

        private void DrawScreen(float radius)
        {
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
            var p1 = new SDL.SDL_Point();
            var p2 = new SDL.SDL_Point();
            var p3 = new SDL.SDL_Point();
            var p4 = new SDL.SDL_Point();

            p1.x = windowWidth / 2 - (int)radius / 2;
            p2.x = windowWidth / 2 + (int)radius / 2;
            p3.x = windowWidth / 2 + (int)radius / 2;
            p4.x = windowWidth / 2 - (int)radius / 2;

            p1.y = windowHeight / 2 - (int)radius / 2;
            p2.y = windowHeight / 2 - (int)radius / 2;
            p3.y = windowHeight / 2 + (int)radius / 2;
            p4.y = windowHeight / 2 + (int)radius / 2;

            SDL.SDL_RenderDrawLine(renderer, p1.x, p1.y, p2.x, p2.y);
            SDL.SDL_RenderDrawLine(renderer, p2.x, p2.y, p3.x, p3.y);
            SDL.SDL_RenderDrawLine(renderer, p3.x, p3.y, p4.x, p4.y);
            SDL.SDL_RenderDrawLine(renderer, p4.x, p4.y, p1.x, p1.y);

            /*float phi = 0;
            var currPoint = new SDL.SDL_Point();
            currPoint.x = windowWidth/2 + (int)(radius * Math.Cos(phi));
            currPoint.y = windowHeight/2 + (int)(radius * Math.Sin(phi));
            while (phi <= 2 * Math.PI)
            {
                phi += 0.05f;
                var nextPoint = new SDL.SDL_Point();
                nextPoint.x = windowWidth/2 + (int)(radius * Math.Cos(phi));
                nextPoint.y = windowHeight/2 + (int)(radius * Math.Sin(phi));
                SDL.SDL_RenderDrawLine(renderer, currPoint.x, currPoint.y, nextPoint.x, nextPoint.y);
                currPoint = nextPoint;
            }
            */
        }


        private void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            var originView = OriginView.GetInstance();
            SDL.SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x2 >= x1 ? 1 : -1;
            int sy = y2 >= y1 ? 1 : -1;

            if (dy <= dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;

                if (originView.IsInCamera(x1, y1)) SDL.SDL_RenderDrawPoint(renderer, x1, y1);
                for (int x = x1 + sx, y = y1, i = 1; i <= dx; i++, x += sx)
                {
                    if (d > 0)
                    {
                        d += d2; y += sy;
                    }
                    else
                        d += d1;
                    if (originView.IsInCamera(x, y)) SDL.SDL_RenderDrawPoint(renderer, x, y);
                }
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;

                if (originView.IsInCamera(x1, y1)) SDL.SDL_RenderDrawPoint(renderer, x1, y1);
                for (int x = x1, y = y1 + sy, i = 1; i <= dy; i++, y += sy)
                {
                    if (d > 0)
                    {
                        d += d2; x += sx;
                    }
                    else
                        d += d1;
                    if (originView.IsInCamera(x, y)) SDL.SDL_RenderDrawPoint(renderer, x, y);
                }
            }
        }

        private void DrawDashLine(int x1, int y1, int x2, int y2, Color color)
        {
            var originView = OriginView.GetInstance();
            SDL.SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x2 >= x1 ? 1 : -1;
            int sy = y2 >= y1 ? 1 : -1;

            if (dy <= dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;

                if (originView.IsInCamera(x1, y1)) SDL.SDL_RenderDrawPoint(renderer, x1, y1);
                for (int x = x1 + sx, y = y1, i = 1; i <= dx; i++, x += sx)
                {
                    if (d > 0)
                    {
                        d += d2; y += sy;
                    }
                    else
                        d += d1;
                    if ((x % 10 < 6) && originView.IsInCamera(x, y)) SDL.SDL_RenderDrawPoint(renderer, x, y);
                }
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;

                if (originView.IsInCamera(x1, y1)) SDL.SDL_RenderDrawPoint(renderer, x1, y1);
                for (int x = x1, y = y1 + sy, i = 1; i <= dy; i++, y += sy)
                {
                    if (d > 0)
                    {
                        d += d2; x += sx;
                    }
                    else
                        d += d1;
                    if ((y % 10 < 6) && originView.IsInCamera(x, y)) SDL.SDL_RenderDrawPoint(renderer, x, y);
                }
            }
        }

        private IEnumerable<Point> GetPoints(int x1, int y1, int x2, int y2)
        {
            var originView = OriginView.GetInstance();
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x2 >= x1 ? 1 : -1;
            int sy = y2 >= y1 ? 1 : -1;

            if (dy <= dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;

                yield return new Point(x1, y1);
                for (int x = x1 + sx, y = y1, i = 1; i <= dx; i++, x += sx)
                {
                    if (d > 0)
                    {
                        d += d2; y += sy;
                    }
                    else
                        d += d1;
                    yield return new Point(x, y);
                }
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;

                yield return new Point(x1, y1);
                for (int x = x1, y = y1 + sy, i = 1; i <= dy; i++, y += sy)
                {
                    if (d > 0)
                    {
                        d += d2; x += sx;
                    }
                    else
                        d += d1;
                    yield return new Point(x, y);
                }
            }
            yield break;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Hide();
            Close();
        }
    }
}

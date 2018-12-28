using Asteroids.SpaceObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids
{
    internal class Game
    {
        private static BufferedGraphicsContext BufferedGraphicsContext;
        public static BufferedGraphics Buffer;

        private static List<BaseObject> SpaceObjectList;

        private static readonly Random Random;

        public static int Width { get; set; }
        public static int Height { get; set; }

        static Game()
        {
            Random = new Random();
        }

        public static void Init(Form form)
        {
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            LoadSpaceObjects();

            BufferedGraphicsContext = BufferedGraphicsManager.Current;
            Buffer = BufferedGraphicsContext.Allocate(form.CreateGraphics(), new Rectangle(0, 0, Width, Height));

            InitFrameUpdateTimer();
        }

        private static void InitFrameUpdateTimer()
        {
            Timer Timer = new Timer { Interval = 100 };
            Timer.Tick += TimerTick;

            Timer.Start();
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in SpaceObjectList)
            {
                obj.Draw();
            }
            Buffer.Render();
        }

        private static void LoadSpaceObjects()
        {
            SpaceObjectList = new List<BaseObject>();

            //TODO: optimize objects spreading
            for (int startX = 1, i = 1; startX < Width; startX += Width / 15, i++)
            {
                int MaxConstellationsCount = 4;

                for (int startY = 1, j = 1; startY < Height; startY += Height / 20, j++)
                {
                    if (
                        (i * (j + i / 2)) % 5 == 0
                        && i % 5 != 0
                        && MaxConstellationsCount > 0
                        )
                    {
                        SpaceObjectList.Add(
                            new PulsingConstellation(
                                new Point(startX, startY),
                                new Point(Random.Next(1, 20), Random.Next(1, 10)),
                                new Size(6, 6)
                            )
                        );

                        MaxConstellationsCount--;
                    }

                    if (
                       (j + 1) % 3 == 0
                       && j % 3 != 0
                       )
                    {
                        SpaceObjectList.Add(
                            new XStar(
                                new Point(startX, startY),
                                new Point(Random.Next(1, 10), Random.Next(1, 20)),
                                new Size(3, 3)
                            )
                        );
                    }

                }
            }

            SpaceObjectList.Add(new Ufo(new Point(Width / 2, 0)));
        }

        private static void Update()
        {
            foreach (BaseObject obj in SpaceObjectList)
            {
                obj.Update();
            }
        }

        private static void TimerTick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}

using Asteroids.SpaceObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asteroids
{
    internal class Game
    {
        /// <summary>
        /// Max pixels for window width and height
        /// </summary>
        private const int GameWindowMaxSize = 1000;

        //TODO: think about levels and asteroids count
        private static BufferedGraphicsContext BufferedGraphicsContext;
        public static BufferedGraphics Buffer;

        private static List<SpaceObject> SpaceObjectList;
        private static SpaceShip SpaceShip;
        private static Ufo Ufo;
        private static PlayerControlls PlayerControlls;
        private static SpaceObjectSpawner SpaceObjectSpawner;

        private static readonly Random Random;
        private static Timer Timer = new Timer();

        //Game window dimentions
        public static int Width { get; set; }
        public static int Height { get; set; }

        public static int AsteroidsLeft = 0;

        static Game()
        {
            Random = new Random();
            SpaceObjectList = new List<SpaceObject>();
        }

        public static void Init(Form form)
        {
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            PlayerControlls = new PlayerControlls();
            SpaceObjectSpawner = new SpaceObjectSpawner(Width, Height);

            ValidateGameWindowSize(Width, Height);

            SpawnSpaceObjects();

            BufferedGraphicsContext = BufferedGraphicsManager.Current;
            Buffer = BufferedGraphicsContext.Allocate(form.CreateGraphics(), new Rectangle(0, 0, Width, Height));

            InitFrameUpdateTimer();

            PlayerControlls.InitGameControlls(form);
        }

        private static void ValidateGameWindowSize(int width, int height)
        {
            if (
                width > GameWindowMaxSize
                || width < 0
                || height > GameWindowMaxSize
                || height < 0
                )
            {
                throw new ArgumentOutOfRangeException($"Game window width/height values must be in range [0,{GameWindowMaxSize}]");
            }
        }

        private static void InitFrameUpdateTimer()
        {
            Timer.Interval = 60;

            Timer.Tick += TimerTick;

            Timer.Start();
        }


        private static void ProcessGameControls()
        {
            var Bullet = PlayerControlls.ProcessGameControls(SpaceShip);

            if (Bullet != null)
            {
                SpaceObjectList.Add(Bullet);
            }
        }

        /// <summary>
        /// Draw game objects, detect collisions
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (SpaceObject spaceObject in SpaceObjectList.ToArray())
            {
                spaceObject.Draw();
            }

            foreach (SpaceObject spaceObject in SpaceObjectList.ToArray())
            {
                if (spaceObject is IColliding)
                {
                    foreach (SpaceObject anotherSpaceObject in SpaceObjectList.ToArray())
                    {
                        if (
                            anotherSpaceObject is IColliding
                            && !ReferenceEquals(spaceObject, anotherSpaceObject)
                            && spaceObject.IsCollidedWithObject(anotherSpaceObject)
                            )
                        {
                            spaceObject.OnCollideWithObject(anotherSpaceObject);
                        }
                    }
                }
            }
            Buffer.Render();
        }

        /// <summary>
        /// Spawn all space objects
        /// </summary>
        private static void SpawnSpaceObjects()
        {
            SpaceObjectSpawner.LoadBackgroundStars(SpaceObjectList);

            Ufo = SpaceObjectSpawner.SpawnUfo(SpaceObjectList);
            SpaceShip = SpaceObjectSpawner.SpawnSpaceShip(SpaceObjectList);

            SpaceObjectSpawner.SpawnLevelAsteroids(SpaceObjectList);

            AsteroidsLeft = SpaceObjectList.Where(s => s is Asteroid).Count();

            SpaceObjectSpawner.SpawnEnergyPack(SpaceObjectList);
        }

        /// <summary>
        /// Update state of every object in game
        /// </summary>
        private static void Update()
        {
            foreach (SpaceObject obj in SpaceObjectList.ToArray())
            {
                obj.Update();

                if (obj.Destroyed)
                {
                    if (obj is Asteroid)
                    {
                        SpaceObjectIsDestroyed(obj as Asteroid);
                    }

                    if (obj is Bullet)
                    {
                        SpaceObjectIsDestroyed(obj as Bullet);
                    }
                }
            }
        }

        /// <summary>
        /// Game tick frame timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerTick(object sender, EventArgs e)
        {
            if (PlayerControlls.IsGameOnPause)
            {
                WriteGameMessage("Paused... press P for continue");

                return;
            }

            ProcessGameControls();

            Draw();
            Update();

            if (SpaceShip.Destroyed)
            {
                EndGame();
            }
        }

        /// <summary>
        /// EndGame game logic
        /// </summary>
        public static void EndGame()
        {
            Timer.Stop();
            WriteGameMessage("Ship destroyed!!! The End...");
        }

        /// <summary>
        /// On destroyed bullet
        /// </summary>
        /// <param name="bullet"></param>
        public static void SpaceObjectIsDestroyed(Bullet bullet) {
            SpaceObjectList.Remove(bullet);
        }

        /// <summary>
        /// On destroyed asteroid
        /// </summary>
        /// <param name="asteroid"></param>
        public static async void SpaceObjectIsDestroyed(Asteroid asteroid)
        {
            SpaceObjectList.Remove(asteroid);
            AsteroidsLeft--;

            if (SpaceObjectList.Where(s => s is Asteroid).Count() == 0)
            {
                WriteGameMessage("Get ready for next wave!");
                Timer.Stop();
                await Task.Delay(5000);
                Timer.Start();

                SpaceObjectSpawner.SpawnLevelAsteroids(SpaceObjectList);

                AsteroidsLeft = SpaceObjectList.Where(s => s is Asteroid).Count();
            }
        }

        private static void WriteGameMessage(string message)
        {
            Font Font = new Font(FontFamily.GenericSansSerif, 30, FontStyle.Underline);
            SizeF MessageSize = Buffer.Graphics.MeasureString(message, Font);
            Buffer.Graphics.DrawString(message, Font, Brushes.White, (Width - MessageSize.Width) / 2, Height / 2);
            Buffer.Render();
        }
    }
}

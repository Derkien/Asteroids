using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class Ufo : BaseObject
    {
        private const int AmplitudeX = 50;
        private static bool SwitchDirection;
        private static int InitialPosition;

        public Ufo(Point leftTopPosition) : base(leftTopPosition)
        {
            MoveDirection = new Point(1, 3);

            InitialPosition = LeftTopPosition.X;
            SwitchDirection = false;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Asteroids.Properties.Resources.VerySmallUfo, new Point(LeftTopPosition.X, LeftTopPosition.Y));
        }

        //TODO: move Ufo more unexpected 
        public override void Update()
        {
            if (LeftTopPosition.X == InitialPosition)
            {
                SwitchDirection = false;
            }

            if (LeftTopPosition.X - InitialPosition == AmplitudeX)
            {
                SwitchDirection = true;
            }

            if (SwitchDirection)
            {
                LeftTopPosition.X -= MoveDirection.X;
            }
            else
            {
                LeftTopPosition.X += MoveDirection.X;
            }

            LeftTopPosition.Y += MoveDirection.Y;

            if (LeftTopPosition.Y < 0)
            {
                MoveDirection.Y = -MoveDirection.Y;
            }

            if (LeftTopPosition.Y > Game.Height)
            {
                MoveDirection.Y = -MoveDirection.Y;
            }
        }
    }
}

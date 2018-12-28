using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class BaseObject
    {
        protected Point LeftTopPosition;
        protected Point MoveDirection;
        protected Size Size;

        public BaseObject(Point leftTopPosition)
        {
            LeftTopPosition = leftTopPosition;
        }

        public BaseObject(Point leftTopPosition, Point moveDirection): this (leftTopPosition)
        {
            MoveDirection = moveDirection;
        }

        public BaseObject(Point leftTopPosition, Point moveDirection, Size size): this(leftTopPosition, moveDirection)
        {
            Size = size;
        }

        public virtual void Draw()
        {
            Game.Buffer.Graphics.DrawEllipse(Pens.White, LeftTopPosition.X, LeftTopPosition.Y, Size.Width, Size.Height);
        }

        public virtual void Update()
        {
            LeftTopPosition.X += MoveDirection.X;
            LeftTopPosition.Y += MoveDirection.Y;
            if (LeftTopPosition.X < 0)
            {
                MoveDirection.X = -MoveDirection.X;
            }

            if (LeftTopPosition.X > Game.Width)
            {
                MoveDirection.X = -MoveDirection.X;
            }

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

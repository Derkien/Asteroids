using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class XStar : SpaceObject
    {
        public XStar(Point leftTopPosition, Point moveDirection, Size size) : base(leftTopPosition, moveDirection, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, LeftTopPosition.X, LeftTopPosition.Y, LeftTopPosition.X + Size.Width, LeftTopPosition.Y + Size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, LeftTopPosition.X + Size.Width, LeftTopPosition.Y, LeftTopPosition.X, LeftTopPosition.Y + Size.Height);
        }

        public override bool IsObjectTypeValidForCollision(IColliding obj)
        {
            return false;
        }

        protected override void OnAfterCollideRegistered(IColliding obj)
        {
            return;
        }

        public override void Update()
        {
            LeftTopPosition.X -= MoveDirection.X;
            if (LeftTopPosition.X < 0)
            {
                LeftTopPosition.X += Game.Width;
            }

            LeftTopPosition.Y += MoveDirection.Y;
            if (LeftTopPosition.Y > Game.Height)
            {
                LeftTopPosition.Y -= Game.Height;
            }
        }
    }
}

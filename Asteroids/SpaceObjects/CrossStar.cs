using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class CrossStar : XStar
    {
        public CrossStar(Point leftTopPosition, Point moveDirection, Size size) : base(leftTopPosition, moveDirection, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, LeftTopPosition.X + Size.Width / 2, LeftTopPosition.Y, (float)(LeftTopPosition.X + Size.Width / 2), LeftTopPosition.Y + Size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, LeftTopPosition.X, LeftTopPosition.Y + Size.Height / 2, LeftTopPosition.X + Size.Width, (float)(LeftTopPosition.Y + Size.Height / 2));
        }
    }
}

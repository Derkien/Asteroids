using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class PulsingConstellation : SpaceObject
    {
        private const int DefaultSizeWidth = 6;
        private const int DefaultSizeHeight = 6;
        private const int SmallStarsAspectRatio = 3;
        private static int DrawCounter;

        private XStar LeftXStar;
        private XStar RightXStar;
        private XStar TopXStar;
        private XStar BottomXStar;
        private CrossStar CenterCrossStar;
        private XStar CenterXStar;

        public PulsingConstellation(Point leftTopPosition, Point moveDirection, Size size) : base(leftTopPosition, moveDirection, size)
        {
            InitConstellationStars(leftTopPosition, moveDirection);
        }

        public PulsingConstellation(Point leftTopPosition, Point moveDirection) : base(leftTopPosition, moveDirection)
        {
            Size = new Size(DefaultSizeWidth, DefaultSizeHeight);

            InitConstellationStars(leftTopPosition, moveDirection);
        }

        private void InitConstellationStars(Point leftTopPosition, Point moveDirection)
        {
            DrawCounter = 0;

            CenterXStar = new XStar(leftTopPosition, moveDirection, Size);

            Size SmallStarSize = new Size(Size.Width / SmallStarsAspectRatio, Size.Height / SmallStarsAspectRatio);

            CenterCrossStar = new CrossStar(
                new Point(
                    leftTopPosition.X + Size.Width / 2 - SmallStarSize.Width / 2,
                    leftTopPosition.Y + Size.Height / 2 - SmallStarSize.Height / 2
                ),
                moveDirection,
                SmallStarSize
            );
            LeftXStar = new XStar(
                new Point(
                    leftTopPosition.X - SmallStarSize.Width - 2,
                    leftTopPosition.Y + Size.Height / 2 - SmallStarSize.Width / 2
                ),
                moveDirection,
                SmallStarSize
            );
            RightXStar = new XStar(
                new Point(
                    leftTopPosition.X + Size.Width + 2,
                    leftTopPosition.Y + Size.Height / 2 - SmallStarSize.Width / 2
                ),
                moveDirection,
                SmallStarSize
            );
            TopXStar = new XStar(
                new Point(
                    leftTopPosition.X + Size.Width / 2 - SmallStarSize.Width / 2,
                    leftTopPosition.Y - SmallStarSize.Height - 2
                ),
                moveDirection,
                SmallStarSize
            );
            BottomXStar = new XStar(
                new Point(
                    leftTopPosition.X + Size.Width / 2 - SmallStarSize.Width / 2,
                    leftTopPosition.Y + Size.Height + 2
                ),
                moveDirection,
                SmallStarSize
            );
        }

        public override void Draw()
        {
            CenterCrossStar.Draw();

            if (DrawCounter / 100 % 2 == 0)
            {
                CenterXStar.Draw();
            }

            //TODO: choose another "pulse" strategy...
            if (DrawCounter < 100)
            {
                LeftXStar.Draw();
            }
            else if (DrawCounter < 200)
            {
                TopXStar.Draw();
            }
            else if (DrawCounter < 300)
            {
                RightXStar.Draw();
            }
            else if (DrawCounter < 400)
            {
                BottomXStar.Draw();
            }
            else if (DrawCounter < 500)
            {
                RightXStar.Draw();
                TopXStar.Draw();
            }
            else if (DrawCounter < 600)
            {
                TopXStar.Draw();
                LeftXStar.Draw();
            }
            else if (DrawCounter < 700)
            {
                LeftXStar.Draw();
                BottomXStar.Draw();
            }
            else if (DrawCounter < 800)
            {
                BottomXStar.Draw();
                RightXStar.Draw();
            }

            DrawCounter++;

            if (DrawCounter > 1000)
            {
                DrawCounter = 0;
            }
        }

        public override bool IsCollidedWithObject(IColliding obj)
        {
            return false;
        }

        public override void OnCollideWithObject(IColliding obj)
        {
            return;
        }

        public override void Update()
        {
            LeftXStar.Update();
            RightXStar.Update();
            TopXStar.Update();
            BottomXStar.Update();
            CenterCrossStar.Update();
            CenterXStar.Update();
        }
    }
}

using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class SmallAsteroid : Asteroid
    {
        private const int AsteroidHealth = 5;
        private const int AsteroidXSpeed = 7;
        private const int AsteroidYSpeed = 25;

        public SmallAsteroid(Point leftTopPosition) : base(leftTopPosition)
        {
        }

        protected override Image GetAsteroidImage()
        {
            return Properties.Resources.SmallAsteroid;
        }

        protected override int GetAsteroidMaxHealth()
        {
            return AsteroidHealth;
        }

        protected override Point GetMoveDirection()
        {
            return new Point(AsteroidXSpeed, AsteroidYSpeed);
        }
    }
}

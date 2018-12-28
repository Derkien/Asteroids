using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class BigAsteroid : Asteroid
    {
        private const int AsteroidHealth = 15;
        private const int AsteroidXSpeed = 2;
        private const int AsteroidYSpeed = 50;

        public BigAsteroid(Point leftTopPosition) : base(leftTopPosition)
        {
        }

        protected override Image GetAsteroidImage()
        {
            return Properties.Resources.BigAsteroid;
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

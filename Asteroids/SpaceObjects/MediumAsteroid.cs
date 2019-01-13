using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class MediumAsteroid : Asteroid
    {
        private const int AsteroidHealth = 10;
        private const int AsteroidXSpeed = 4;
        private const int AsteroidYSpeed = 25;
        private const int AsteroidDamage = 10;

        public MediumAsteroid(Point leftTopPosition) : base(leftTopPosition)
        {
        }

        protected override Image GetAsteroidImage()
        {
            return Properties.Resources.MediumAsteroid;
        }

        protected override int GetAsteroidMaxHealth()
        {
            return AsteroidHealth;
        }

        protected override Point GetMoveDirection()
        {
            return new Point(AsteroidXSpeed, AsteroidYSpeed);
        }

        public override int GetPower()
        {
            return AsteroidDamage;
        }
    }
}

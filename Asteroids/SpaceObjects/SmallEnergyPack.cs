using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class SmallEnergyPack : EnergyPack
    {
        private const int EnergyPackPower = 15;
        private const int EnergyPackXSpeed = 10;
        private const int EnergyPackYSpeed = 35;

        public SmallEnergyPack(Point leftTopPosition) : base(leftTopPosition)
        {
        }

        public override int GetPower()
        {
            return EnergyPackPower;
        }

        protected override Image GetEnergyPackImage()
        {
            return Properties.Resources.SmallEnergy;
        }

        protected override Point GetMoveDirection()
        {
            return new Point(EnergyPackXSpeed, EnergyPackYSpeed);
        }
    }
}

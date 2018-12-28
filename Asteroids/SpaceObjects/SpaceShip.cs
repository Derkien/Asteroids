using Asteroids.Exceptions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids.SpaceObjects
{
    internal class SpaceShip : SpaceObject
    {
        private const int SpaceShipHealth = 100;
        private const int BaseGunAmmoLimit = 5;
        private const int BaseGunAmmoRefreshInterval = 1000;
        private const int ShipImpactDamage = 100;
        private const int SpaceShipSpeed = 5;

        private static int BulletsLimitCount;

        public SpaceShip(Point leftTopPosition) : base(leftTopPosition)
        {
            if (LeftTopPosition.X < 0 || LeftTopPosition.Y < 0 || LeftTopPosition.X > Game.Width || LeftTopPosition.Y > Game.Height)
            {
                throw new InvalidSpaceObjectException("Initial position of space ship can't be out of screen");
            }

            MoveDirection = new Point(SpaceShipSpeed, SpaceShipSpeed);

            Size = new Size(Properties.Resources.SmallSpaceShip.Width, Properties.Resources.SmallSpaceShip.Height);

            BulletsLimitCount = BaseGunAmmoLimit;

            //Health = SpaceShipHealth;

            Timer Timer = new Timer { Interval = BaseGunAmmoRefreshInterval };
            Timer.Tick += (object sender, EventArgs e) => RefillAmmo();

            Timer.Start();
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Properties.Resources.SmallSpaceShip, new Point(LeftTopPosition.X, LeftTopPosition.Y));
        }

        public override bool IsCollidedWithObject(IColliding obj)
        {
            if (!(obj is Asteroid))
            {
                return false;
            }

            return BodyShape.IntersectsWith(obj.BodyShape);
        }

        public override void OnCollideWithObject(IColliding obj)
        {
            if (!IsCollidedWithObject(obj))
            {
                return;
            }

            //TODO: remove GOD MODE :)

            obj.OnCollideWithObject(this);
        }

        public override void Update()
        {
            return;
        }

        public bool IsAmmoAvailable()
        {
            return BulletsLimitCount > 0;
        }

        private void RefillAmmo()
        {
            if (BulletsLimitCount < BaseGunAmmoLimit)
            {
                BulletsLimitCount++;
            }
        }

        public Bullet Shoot()
        {
            if (!IsAmmoAvailable())
            {
                throw new OutOfAmmoException();
            }

            BulletsLimitCount--;


            return new Bullet(new Point(LeftTopPosition.X + Size.Width, LeftTopPosition.Y + Size.Height / 2));
        }

        public override int GetDamage()
        {
            return ShipImpactDamage;
        }

        public void MoveHorizontal(int Direction)
        {
            LeftTopPosition.X += Direction*MoveDirection.X;
        }

        public void MoveVertical(int Direction)
        {
            LeftTopPosition.Y += Direction*MoveDirection.Y;
        }
    }
}

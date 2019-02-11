using Asteroids.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids.SpaceObjects
{
    internal class SpaceShip : SpaceObject
    {
        private const int SpaceShipHealth = 100;
        private const int BaseGunAmmoLimit = 15;
        private const int BaseGunAmmoRefreshInterval = 500;
        private const int ShipImpactDamage = 100;
        private const int SpaceShipSpeed = 10;

        private static int SmallAsteroidsKilled = 0;
        private static int MediumAsteroidsKilled = 0;
        private static int BigAsteroidsKilled = 0;
        private static int RewardTotal = 0;

        //TODO: move/incapsulate rewards to asteroids object
        private static readonly int SmallAsteroidReward = 200;
        private static readonly int MediumAsteroidReward = 300;
        private static readonly int BigAsteroidReward = 500;

        private static int BulletsLimitCount;

        private int NextPositionX;
        private int NextPositionY;
        private bool ShipIsMovedX = false;
        private bool ShipIsMovedY = false;

        protected Image SpaceShipImage;
        protected Image SpaceShipHPLogoImage;
        protected Image SpaceShipGunImage;

        public SpaceShip(Point leftTopPosition) : base(leftTopPosition)
        {
            if (LeftTopPosition.X < 0 || LeftTopPosition.Y < 0 || LeftTopPosition.X > Game.Width || LeftTopPosition.Y > Game.Height)
            {
                throw new InvalidSpaceObjectException("Initial position of space ship can't be out of screen");
            }

            MoveDirection = new Point(SpaceShipSpeed, SpaceShipSpeed);

            SpaceShipImage = Properties.Resources.SmallSpaceShip;
            SpaceShipHPLogoImage = Properties.Resources.SpaceShipHPLogo;
            SpaceShipGunImage = Properties.Resources.PlasmaGunLogo;

            Size = new Size(SpaceShipImage.Width, SpaceShipImage.Height);

            BulletsLimitCount = BaseGunAmmoLimit;
            Health = SpaceShipHealth;

            Timer Timer = new Timer { Interval = BaseGunAmmoRefreshInterval };
            Timer.Tick += (object sender, EventArgs e) => RefillAmmo();
            Timer.Start();
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(SpaceShipImage, new Point(LeftTopPosition.X, LeftTopPosition.Y));

            int MarginSize = 4;

            Font Font = new Font("Arial", 10, FontStyle.Bold);
            string KillBoardMessage = $"Killboard: {SmallAsteroidsKilled}s;{MediumAsteroidsKilled}m;{BigAsteroidsKilled}b | Reward: {RewardTotal}$ | Asteroids left: {Game.AsteroidsLeft}";
            SizeF KillBoardMessageSize = Game.Buffer.Graphics.MeasureString(KillBoardMessage, Font);
            Game.Buffer.Graphics.DrawString(
                KillBoardMessage,
                Font,
                new SolidBrush(Color.Aquamarine),
                MarginSize,
                MarginSize
            );

            Rectangle LifeBarBorder = new Rectangle(MarginSize + SpaceShipHPLogoImage.Width, MarginSize + (int)KillBoardMessageSize.Height, SpaceShipHealth, SpaceShipHPLogoImage.Height);
            Game.Buffer.Graphics.DrawImage(SpaceShipHPLogoImage, MarginSize, LifeBarBorder.Y);
            Game.Buffer.Graphics.FillRectangle(new SolidBrush(Color.Red), LifeBarBorder);
            Rectangle LifeBar = new Rectangle(LifeBarBorder.X, LifeBarBorder.Y, Health > SpaceShipHealth ? SpaceShipHealth : Health, LifeBarBorder.Height);
            Game.Buffer.Graphics.FillRectangle(new SolidBrush(Color.LightGreen), LifeBar);

            Rectangle AmmoBarBorder = new Rectangle(MarginSize + SpaceShipGunImage.Width, MarginSize * 2 + (int)KillBoardMessageSize.Height + SpaceShipGunImage.Height, BaseGunAmmoLimit, SpaceShipGunImage.Height);
            Game.Buffer.Graphics.DrawImage(SpaceShipGunImage, MarginSize, AmmoBarBorder.Y);
            Game.Buffer.Graphics.FillRectangle(new SolidBrush(Color.Red), AmmoBarBorder);
            Rectangle AmmoBar = new Rectangle(AmmoBarBorder.X, AmmoBarBorder.Y, BulletsLimitCount, AmmoBarBorder.Height);
            Game.Buffer.Graphics.FillRectangle(new SolidBrush(Color.LightGreen), AmmoBar);

            string HealthText = $"{Health}%";
            SizeF HealthTextSize = Game.Buffer.Graphics.MeasureString(HealthText, Font);
            Game.Buffer.Graphics.DrawString(
                HealthText,
                Font,
                new SolidBrush(Color.Black),
                (LifeBarBorder.Width / 2) - (HealthTextSize.Width / 2) + LifeBarBorder.X,
                LifeBarBorder.Y
            );
        }

        public override bool IsObjectTypeValidForCollision(IColliding obj)
        {
            return (obj is Asteroid) || (obj is EnergyPack);
        }

        protected override void OnAfterCollideRegistered(IColliding obj)
        {
            string CollisionResult = "";
            if (obj is EnergyPack)
            {
                Health += obj.GetPower();
                CollisionResult = $"HP restored: {obj.GetPower()}";
            }
            else
            {
                Health -= obj.GetPower();
                CollisionResult = $"Damage taken: {obj.GetPower()}";
            }

            if (Health <= 0)
            {
                DestroySpaceObject();
            }

            Logger.LogInformation(
                "After collision registered. " +
                $"Object1: {this.GetType()}.{this.GetHashCode()}, " +
                $"{CollisionResult}, " +
                $"Object1 Health: {Health}"
            );

            CollisionsList.Clear();
        }

        /// <summary>
        /// Subscriber for AsteroidDestructionEvent
        /// </summary>
        /// <param name="obj"></param>
        public static void CalculateScore(Asteroid obj)
        {
            if (obj is SmallAsteroid)
            {
                SmallAsteroidsKilled++;
                RewardTotal += SmallAsteroidReward;
            }
            if (obj is MediumAsteroid)
            {
                MediumAsteroidsKilled++;
                RewardTotal += MediumAsteroidReward;
            }
            if (obj is BigAsteroid)
            {
                BigAsteroidsKilled++;
                RewardTotal += BigAsteroidReward;
            }
        }

        public override void Update()
        {
            if (ShipIsMovedX && NextPositionX >= -BodyShape.Width / 2 && NextPositionX <= Game.Width - BodyShape.Width)
            {
                LeftTopPosition.X = NextPositionX;

                ShipIsMovedX = false;
            }

            if (ShipIsMovedY && NextPositionY >= -BodyShape.Height / 2 && NextPositionY <= Game.Height - BodyShape.Height / 2)
            {
                LeftTopPosition.Y = NextPositionY;

                ShipIsMovedY = false;
            }
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

        public override int GetPower()
        {
            return ShipImpactDamage;
        }

        public void MoveHorizontal(int Direction)
        {
            NextPositionX = LeftTopPosition.X + Direction * MoveDirection.X;
            ShipIsMovedX = true;
        }

        public void MoveVertical(int Direction)
        {
            NextPositionY = LeftTopPosition.Y + Direction * MoveDirection.Y;
            ShipIsMovedY = true;
        }

        private void DestroySpaceObject()
        {
            Destroyed = true;

            LeftTopPosition.X = -1100;

            CollisionsList.Clear();

            MessageDie?.Invoke();
        }

        public static event Message MessageDie;
    }
}

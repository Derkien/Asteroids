using Asteroids.Exceptions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids.SpaceObjects
{
    internal class SpaceShip : SpaceObject
    {
        private const int SpaceShipHealth = 100;
        private const int BaseGunAmmoLimit = 10;
        private const int BaseGunAmmoRefreshInterval = 500;
        private const int ShipImpactDamage = 100;
        private const int SpaceShipSpeed = 5;

        private static int SmallAsteroidsKilled = 0;
        private static int MediumAsteroidsKilled = 0;
        private static int BigAsteroidsKilled = 0;
        private static int RewardTotal = 0;

        //TODO: move/incapsulate rewards to asteroids object
        private static readonly int SmallAsteroidReward = 200;
        private static readonly int MediumAsteroidReward = 300;
        private static readonly int BigAsteroidReward = 500;

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
            Health = SpaceShipHealth;

            Timer Timer = new Timer { Interval = BaseGunAmmoRefreshInterval };
            Timer.Tick += (object sender, EventArgs e) => RefillAmmo();
            Timer.Start();
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Properties.Resources.SmallSpaceShip, new Point(LeftTopPosition.X, LeftTopPosition.Y));

            Rectangle LifeBarBorder = new Rectangle(5, 5, SpaceShipHealth + 2, 17);
            Game.Buffer.Graphics.DrawRectangle(new Pen(Color.OrangeRed, 17), LifeBarBorder);

            Game.Buffer.Graphics.DrawRectangle(new Pen(Color.Aquamarine, 15), 6, 6, Health > SpaceShipHealth ? SpaceShipHealth : Health, 15);

            string HealthText = $"{Health}%";
            Font Font = new Font("Arial", 10, FontStyle.Bold);

            SizeF size = Game.Buffer.Graphics.MeasureString(HealthText, Font);

            Game.Buffer.Graphics.DrawString(HealthText, Font, new SolidBrush(Color.Black), (LifeBarBorder.Width / 2) - (size.Width / 2) + 5, LifeBarBorder.Height / 2);

            Game.Buffer.Graphics.DrawString($"Killboard: {SmallAsteroidsKilled}s;{MediumAsteroidsKilled}m;{BigAsteroidsKilled}b | Reward: {RewardTotal}$", Font, new SolidBrush(Color.Aquamarine), LifeBarBorder.Width + 15, LifeBarBorder.Height / 2);
        }

        public override bool IsCollidedWithObject(IColliding obj)
        {
            if ((!(obj is Asteroid) && !(obj is EnergyPack)) || CollisionsList.Contains(obj))
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

            //TODO: simplify collision logic
            if (!CollisionsList.Contains(obj))
            {
                CollisionsList.Add(obj);
            }

            string CollisionResult = "";
            if (obj is EnergyPack)
            {
                Health += obj.GetPower();
                CollisionResult = "HP restored";
            }
            else
            {
                Health -= obj.GetPower();
                CollisionResult = "Damage taken";
            }

            obj.OnCollideWithObject(this);

            if (Health <= 0)
            {
                DestroySpaceObject();
            }

            Debug.WriteLine($"Object: {this.GetHashCode()}. Collision detected! Source: {this.GetType()}. Target: {obj.GetType()}. {CollisionResult}: {obj.GetPower()}. Current Health: {Health}");

            CollisionsList.Clear();
        }

        //HARDCODE just for idea, replace for event listening from destroyed asteroid
        public static void CalculateScore(IColliding obj)
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

        public override int GetPower()
        {
            return ShipImpactDamage;
        }

        public void MoveHorizontal(int Direction)
        {
            LeftTopPosition.X += Direction * MoveDirection.X;
        }

        public void MoveVertical(int Direction)
        {
            LeftTopPosition.Y += Direction * MoveDirection.Y;
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

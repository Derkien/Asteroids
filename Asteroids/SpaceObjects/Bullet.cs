using Microsoft.Extensions.Logging;
using System;
using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class Bullet : SpaceObject, IDisposable
    {
        private Image BulletImage;

        private const int BulletXSpeed = 15;
        private const int BulletYSpeed = 0;
        private const int BulletDamage = 2;

        public Bullet(Point leftTopPosition) : base(leftTopPosition)
        {
            MoveDirection = new Point(BulletXSpeed, BulletYSpeed);
            BulletImage = Properties.Resources.SmallPlasmaBullet;

            Size = new Size(BulletImage.Width, BulletImage.Height);
        }

        public void Dispose()
        {
            BulletImage.Dispose();
            GC.SuppressFinalize(this);
        }

        public override void Draw()
        {
            if (Destroyed)
            {
                return;
            }

            Game.Buffer.Graphics.DrawImage(BulletImage, new Point(LeftTopPosition.X, LeftTopPosition.Y));
        }

        public override bool IsObjectTypeValidForCollision(IColliding obj)
        {
            return (obj is Asteroid);
        }

        protected override void OnAfterCollideRegistered(IColliding obj)
        {
            Logger.LogInformation(
                "After collision registered. " +
                $"Object1: {this.GetType()}.{this.GetHashCode()}, " +
                $"Damage done: {this.GetPower()}"
            );


            DestroySpaceObject();
        }

        public override int GetPower()
        {
            return BulletDamage;
        }

        public override void Update()
        {
            if (Destroyed)
            {
                return;
            }

            LeftTopPosition.X += MoveDirection.X;

            if (LeftTopPosition.X > Game.Width)
            {
                DestroySpaceObject();
            }
        }

        private void DestroySpaceObject()
        {
            Destroyed = true;
            LeftTopPosition.X = -500;
            CollisionsList.Clear();

            Dispose();
        }
    }
}

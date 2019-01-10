using System;
using System.Diagnostics;
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

            Debug.WriteLine($"Object: {this.GetHashCode()}. Bullet destroyed and disposed!");
        }

        public override void Draw()
        {
            if (Destroyed)
            {
                return;
            }

            Game.Buffer.Graphics.DrawImage(BulletImage, new Point(LeftTopPosition.X, LeftTopPosition.Y));
        }

        public override bool IsCollidedWithObject(IColliding obj)
        {
            if (!(obj is Asteroid) || Destroyed || CollisionsList.Contains(obj))
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

            if (!CollisionsList.Contains(obj))
            {
                CollisionsList.Add(obj);
            }

            obj.OnCollideWithObject(this);

            Debug.WriteLine($"Object: {this.GetHashCode()}. Collision detected! Source: {this.GetType()}. Target: {obj.GetType()}. Damage done: {this.GetPower()}.");

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

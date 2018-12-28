using System;
using System.Collections.Generic;
using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal class Bullet : SpaceObject, IDisposable
    {
        private List<IColliding> CollisionsList;

        private bool Destroyed;

        private Image BulletImage;

        private const int BulletXSpeed = 15;
        private const int BulletYSpeed = 0;
        private const int BulletDamage = 2;

        public Bullet(Point leftTopPosition) : base(leftTopPosition)
        {
            MoveDirection = new Point(BulletXSpeed, BulletYSpeed);
            BulletImage = Properties.Resources.SmallPlasmaBullet;

            Size = new Size(BulletImage.Width, BulletImage.Height);

            Destroyed = false;

            CollisionsList = new List<IColliding>();
        }

        public void Dispose()
        {
            Destroyed = true;
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

            DestroySpaceObject();
            Dispose();
        }

        public override int GetDamage()
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

            if(LeftTopPosition.X > Game.Width)
            {
                DestroySpaceObject();
                Dispose();
            }
        }

        private void DestroySpaceObject()
        {
            Destroyed = true;
            LeftTopPosition.X = -500;
        }
    }
}

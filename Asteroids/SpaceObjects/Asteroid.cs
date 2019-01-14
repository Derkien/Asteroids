using Asteroids.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal abstract class Asteroid : SpaceObject, IDisposable
    {
        protected Image AsteroidImage;
        protected Random Random;

        public delegate void AsteroidDestruction(Asteroid asteroid);

        public Asteroid(Point leftTopPosition) : base(leftTopPosition)
        {
            if (LeftTopPosition.X < 0 || LeftTopPosition.Y < 0 || LeftTopPosition.X < Game.Width / 2 || LeftTopPosition.Y > Game.Height)
            {
                throw new InvalidSpaceObjectException("Initial position of asteroid can't be in the middle of screen or out of top or bottom of screen");
            }

            Random = new Random();

            MoveDirection = GetMoveDirection();
            AsteroidImage = GetAsteroidImage();
            Health = GetAsteroidMaxHealth();

            Size = new Size(AsteroidImage.Width, AsteroidImage.Height);
        }

        public void Dispose()
        {
            AsteroidImage.Dispose();
            GC.SuppressFinalize(this);
        }

        protected abstract Point GetMoveDirection();
        protected abstract Image GetAsteroidImage();
        protected abstract int GetAsteroidMaxHealth();

        public override void Draw()
        {
            if (Destroyed)
            {
                return;
            }

            Game.Buffer.Graphics.FillEllipse(Brushes.White, BodyShape);
            Game.Buffer.Graphics.DrawImage(AsteroidImage, new Point(LeftTopPosition.X, LeftTopPosition.Y));
        }

        public override bool IsObjectTypeValidForCollision(IColliding obj)
        {
            return (obj is Bullet) || (obj is SpaceShip);
        }

        protected override void OnAfterCollideRegistered(IColliding obj)
        {
            Health -= obj.GetPower();

            Logger.LogInformation(
                "After collision registered. " +
                $"Object1: {this.GetType()}.{this.GetHashCode()}, " +
                $"Damage taken: {obj.GetPower()}, " +
                $"Object1 Health: {Health}"
            );

            if (Health <= 0)
            {
                DestroySpaceObject();
            }
        }

        public override void Update()
        {
            if (Destroyed)
            {
                return;
            }

            LeftTopPosition.X -= MoveDirection.X;

            if (LeftTopPosition.X < -Size.Width)
            {
                RenewSpaceObject();
            }
        }

        private void DestroySpaceObject()
        {
            Destroyed = true;

            LeftTopPosition.X = -1000;

            CollisionsList.Clear();

            Dispose();

            AsteroidDestructionEvent?.Invoke(this);

            Logger.LogInformation(
                "Asteroid is desroyed. " +
                $"Object1: {this.GetType()}.{this.GetHashCode()}"
            );
        }

        private void RenewSpaceObject()
        {
            Health = GetAsteroidMaxHealth();

            LeftTopPosition.X = Game.Width + 2 * Size.Width;

            LeftTopPosition.Y += Random.Next(-5, 5) * MoveDirection.Y;

            if (LeftTopPosition.Y > Game.Height)
            {
                LeftTopPosition.Y -= Game.Height / Random.Next(2, 4);
            }

            if (LeftTopPosition.Y < 0)
            {
                LeftTopPosition.Y += Game.Height / Random.Next(2, 4);
            }
        }

        public event AsteroidDestruction AsteroidDestructionEvent;
    }
}

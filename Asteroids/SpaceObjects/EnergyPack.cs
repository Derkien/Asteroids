using Asteroids.Exceptions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids.SpaceObjects
{
    internal abstract class EnergyPack : SpaceObject
    {
        private const int EnergyRefreshInterval = 20000;

        protected Image EnergyPackImage;
        protected Random Random;

        protected EnergyPack(Point leftTopPosition) : base(leftTopPosition)
        {
            if (LeftTopPosition.X < 0 || LeftTopPosition.Y < 0 || LeftTopPosition.X < Game.Width / 2 || LeftTopPosition.Y > Game.Height)
            {
                throw new InvalidSpaceObjectException("Initial position of energy can't be in the middle of screen or out of top or bottom of screen");
            }

            Random = new Random();

            EnergyPackImage = GetEnergyPackImage();
            MoveDirection = GetMoveDirection();

            Size = new Size(EnergyPackImage.Width, EnergyPackImage.Height);

            Timer Timer = new Timer { Interval = EnergyRefreshInterval };
            Timer.Tick += (object sender, EventArgs e) => InitNewSpaceObject();
            Timer.Start();
        }

        protected abstract Point GetMoveDirection();
        protected abstract Image GetEnergyPackImage();

        public override bool IsCollidedWithObject(IColliding obj)
        {
            if (!(obj is SpaceShip) || Destroyed || CollisionsList.Contains(obj))
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

            obj.OnCollideWithObject(this);

            Debug.WriteLine($"Object: {this.GetHashCode()}. Collision detected! Source: {this.GetType()}. Target: {obj.GetType()}. HP restored: {this.GetPower()}.");

            DestroySpaceObject();
        }

        public override void Draw()
        {
            if (Destroyed)
            {
                return;
            }

            Game.Buffer.Graphics.DrawImage(EnergyPackImage, new Point(LeftTopPosition.X, LeftTopPosition.Y));
        }

        public override void Update()
        {
            if (Destroyed)
            {
                // InitNewSpaceObject();
                return;
            }

            LeftTopPosition.X -= MoveDirection.X;

            if (LeftTopPosition.X < 0)
            {
                DestroySpaceObject();
            }
        }

        private void DestroySpaceObject()
        {
            Destroyed = true;

            LeftTopPosition.X = -1100;

            CollisionsList.Clear();
        }

        private void InitNewSpaceObject()
        {
            Destroyed = false;

            LeftTopPosition.X = Game.Width + 2 * Size.Width;

            LeftTopPosition.Y += Random.Next(-5, 5) * MoveDirection.Y;

            if (LeftTopPosition.Y > Game.Height)
            {
                LeftTopPosition.Y -= Game.Height;
            }
        }
    }
}

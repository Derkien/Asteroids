using Asteroids.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal abstract class SpaceObject : IColliding
    {
        public delegate void Message();
        public bool Destroyed { get; protected set; } = false;

        protected ILogger Logger;
        protected List<IColliding> CollisionsList;

        protected Point LeftTopPosition;
        protected Point MoveDirection;
        protected Size Size;
        protected int Health = 0;

        protected SpaceObject(Point leftTopPosition)
        {
            CollisionsList = new List<IColliding>();

            LeftTopPosition = leftTopPosition;

            Logger = ApplicationLogging.CreateLogger(GetType());
        }

        protected SpaceObject(Point leftTopPosition, Point moveDirection) : this(leftTopPosition)
        {
            MoveDirection = moveDirection;
        }

        protected SpaceObject(Point leftTopPosition, Point moveDirection, Size size) : this(leftTopPosition, moveDirection)
        {
            Size = size;
        }

        public Rectangle BodyShape => new Rectangle(LeftTopPosition.X, LeftTopPosition.Y, Size.Width, Size.Height);

        /// <summary>
        /// Draw space object on the screen
        /// </summary>
        public abstract void Draw();

        public abstract bool IsObjectTypeValidForCollision(IColliding obj);

        public bool IsCollidedWithObject(IColliding obj)
        {
            if (!IsObjectTypeValidForCollision(obj) || Destroyed || CollisionsList.Contains(obj))
            {
                return false;
            }

            return BodyShape.IntersectsWith(obj.BodyShape);
        }

        public void OnCollideWithObject(IColliding obj)
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

            Logger.LogInformation(
                "Collision of objects registered. " +
                $"Object1: {this.GetType()}.{this.GetHashCode()}, " +
                $"Object2: {obj.GetType()}.{obj.GetHashCode()}"
            );

            OnAfterCollideRegistered(obj);
        }

        protected abstract void OnAfterCollideRegistered(IColliding obj);

        /// <summary>
        /// Update space object state, e.g. move it
        /// </summary>
        public abstract void Update();

        public int GetHealth()
        {
            return Health;
        }

        public virtual int GetPower()
        {
            return 0;
        }
    }
}

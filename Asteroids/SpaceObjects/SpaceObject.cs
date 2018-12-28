using Asteroids.Exceptions;
using System.Drawing;

namespace Asteroids.SpaceObjects
{
    internal abstract class SpaceObject : IColliding
    {
        protected Point LeftTopPosition;
        protected Point MoveDirection;
        protected Size Size;
        protected int Health = 0;

        protected SpaceObject(Point leftTopPosition)
        {
            LeftTopPosition = leftTopPosition;
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

        public abstract bool IsCollidedWithObject(IColliding obj);

        public abstract void OnCollideWithObject(IColliding obj);

        /// <summary>
        /// Update space object state, e.g. move it
        /// </summary>
        public abstract void Update();

        public int GetHealth() {
            return Health;
        }

        public virtual int GetDamage()
        {
            return 0;
        }
    }
}

using System;

namespace Asteroids.Exceptions
{
    internal class InvalidSpaceObjectException : Exception
    {
        public InvalidSpaceObjectException(string message): base(message)
        {

        }
    }
}

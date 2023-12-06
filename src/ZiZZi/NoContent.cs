using System;
namespace ZiZZi
{
    /// <summary>
    /// Content that should not be taken.
    /// </summary>
    public sealed class NoContent<T> : IContent<T>
    {
        /// <summary>
        /// Content that should not be taken.
        /// </summary>
        public NoContent()
        { }

        public T Value()
        {
            throw new InvalidOperationException("Content must not be taken.");
        }

        public bool CanTake()
        {
            return false;
        }
    }
}


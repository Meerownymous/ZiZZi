using System;
namespace ZiZZi
{
    /// <summary>
    /// Content that should be taken.
    /// </summary>
    public sealed class TakeContent<T> : IContent<T>
    {
        private readonly Func<T> content;

        /// <summary>
        /// Content that should be taken.
        /// </summary>
        public TakeContent(T content) : this(() => content)
        { }

        /// <summary>
        /// Content that should be taken.
        /// </summary>
        public TakeContent(Func<T> content)
        {
            this.content = content;
        }

        public T Value()
        {
            return this.content();
        }

        public bool CanTake()
        {
            return true;
        }
    }

    /// <summary>
    /// Content that should be taken.
    /// </summary>
    public static class TakeContent
    {
        /// <summary>
        /// Content that should be taken.
        /// </summary>
        public static TakeContent<T> _<T>(T content) => new TakeContent<T>(content);

        /// <summary>
        /// Content that should be taken.
        /// </summary>
        public static TakeContent<T> _<T>(Func<T> content) => new TakeContent<T>(content);
    }
}


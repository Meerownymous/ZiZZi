using System;
namespace BLox.Shape
{
    /// <summary>
    /// Dead media refusing everything.
    /// </summary>
    public class Dead<T> : IShape<T>
    {
        public Dead()
        {
        }

        public T Content()
        {
            throw new InvalidOperationException("Media is dead.");
        }

        public IShape<T> Open(string contentType, string name)
        {
            throw new InvalidOperationException("Media is dead.");
        }

        public void Put(string name, string content)
        {
            throw new InvalidOperationException("Media is dead.");
        }

        public void Put(string name, string dataType, byte[] content)
        {
            throw new InvalidOperationException("Media is dead.");
        }
    }
}


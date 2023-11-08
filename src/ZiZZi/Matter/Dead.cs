using System;
using System.IO;

namespace ZiZZi.Matter
{
    /// <summary>
    /// Dead media refusing everything.
    /// </summary>
    public class Dead<T> : IMatter<T>
    {
        public Dead()
        {
        }

        public T Content()
        {
            throw new InvalidOperationException("Media is dead.");
        }

        public IMatter<T> Open(string contentType, string name)
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

        public void Put(string name, string dataType, Stream content)
        {
            throw new InvalidOperationException("Media is dead.");
        }
    }
}


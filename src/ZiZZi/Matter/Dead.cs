﻿using System;
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

        public void Present(string name, Func<IContent<string>> content)
        {
            throw new InvalidOperationException("Media is dead.");
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            throw new InvalidOperationException("Media is dead.");
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            throw new InvalidOperationException("Media is dead.");
        }
    }
}


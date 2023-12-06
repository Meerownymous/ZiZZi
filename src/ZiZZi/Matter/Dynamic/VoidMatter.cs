using System;
using System.IO;

namespace ZiZZi.Matter.Dynamic
{
    public sealed class VoidMatter<T> : IMatter<T>
    {
        public VoidMatter()
        {
        }

        public T Content()
        {
            return default(T);
        }

        public IMatter<T> Open(string contentType, string name)
        {
            return this;
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            
        }
    }
}


using System;
using System.IO;

namespace ZiZZi.Matter.Object
{
    public sealed class VoidMatter<TResult> : IMatter<TResult> where TResult : class
    {
        public VoidMatter()
        {

        }

        public TResult Content()
        {
            return default(TResult);
        }

        public IMatter<TResult> Open(string contentType, string name)
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
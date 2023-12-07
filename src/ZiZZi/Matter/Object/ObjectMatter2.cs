using System;
using System.IO;

namespace ZiZZi.Matter.Object
{
    public sealed class ObjectMatter2<TResult> : IMatter<TResult>
    {
        private readonly dynamic blueprint;
        private readonly MatterOrigin subMatters;

        public ObjectMatter2(dynamic blueprint, MatterOrigin matter)
        {
            this.blueprint = blueprint;
            this.subMatters = matter;
        }

        public TResult Content()
        {
            throw new NotImplementedException();
        }

        public IMatter<TResult> Open(string contentType, string name)
        {
            throw new NotImplementedException();
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            throw new NotImplementedException();
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            throw new NotImplementedException();
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            throw new NotImplementedException();
        }
    }
}


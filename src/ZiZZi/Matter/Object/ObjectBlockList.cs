using System;
using System.IO;

namespace ZiZZi.Matter.Object
{
    public sealed class ObjectBlockList<TBlock> : IMatter<TBlock>
    {
        public ObjectBlockList()
        {
        }

        public TBlock Content()
        {
            throw new NotImplementedException();
        }

        public IMatter<TBlock> Open(string contentType, string name)
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


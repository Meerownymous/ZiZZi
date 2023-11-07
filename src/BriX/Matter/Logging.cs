using System;
using System.IO;
using ZiZZi.Matter.JSON;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.Validated
{
    /// <summary>
    /// Media which logs its actions to the given function.
    /// </summary>
    public sealed class Logging<T> : IMatter<T>
    {
        private readonly IMatter<T> origin;
        private readonly BytesAsToken bytesAsToken;
        private readonly Action<string> log;

        /// <summary>
        /// Media which logs its actions to the given function.
        /// </summary>
        public Logging(IMatter<T> origin, BytesAsToken pipe, Action<string> log)
        {
            this.origin = origin;
            this.bytesAsToken = pipe;
            this.log = log;
        }

        public T Content()
        {
            return this.origin.Content();
        }

        public IMatter<T> Open(string contentType, string name)
        {
            this.log.Invoke($"Open {contentType} \"{name}\"");
            return
                new Logging<T>(
                    this.origin.Open(contentType, name),
                    this.bytesAsToken,
                    this.log
                );
        }

        public void Put(string name, string content)
        {
            this.log.Invoke($"Put \"{name}\" : \"content\"");
            this.origin.Put(name, content);
        }

        public void Put(string name, string dataType, byte[] content)
        {
            this.log.Invoke($"Put \"{dataType}\" named \"{name}\" \"{this.bytesAsToken.Flip(dataType, content)}\"");
            this.origin.Put(name, dataType, content);
        }

        public void Put(string name, string dataType, Stream content)
        {
            this.Put(name, dataType, new AsBytes(new AsInput(content)).Bytes());
        }
    }
}


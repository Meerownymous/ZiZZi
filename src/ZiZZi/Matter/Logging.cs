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

        public void Present(string name, Func<IContent<string>> content)
        {
            var text = content().Value();
            this.log.Invoke($"Put \"{name}\" : \"{text}\"");
            this.origin.Present(name, () => TakeContent._(text));
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            var bytes = content().Value();
            this.log.Invoke($"Put \"{dataType}\" named \"{name}\" \"{this.bytesAsToken.Flip(dataType, bytes)}\"");
            this.origin.Present(name, dataType, () => TakeContent._(bytes));
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            var stream = content().Value();
            var bytes = new AsBytes(new AsInput(stream)).Bytes();
            this.log.Invoke($"Put \"{dataType}\" named \"{name}\" \"{this.bytesAsToken.Flip(dataType, bytes)}\"");
            this.origin.Present(name, dataType, () => TakeContent._(bytes));
        }
    }
}
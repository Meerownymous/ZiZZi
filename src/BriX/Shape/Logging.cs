using System;
using BLox.Shape.JSON;

namespace BLox.Shape.Validated
{
    /// <summary>
    /// Media which logs its actions to the given function.
    /// </summary>
    public sealed class Logging<T> : IShape<T>
    {
        private readonly IShape<T> origin;
        private readonly BytePipe pipe;
        private readonly Action<string> log;

        /// <summary>
        /// Media which logs its actions to the given function.
        /// </summary>
        public Logging(IShape<T> origin, BytePipe pipe, Action<string> log)
        {
            this.origin = origin;
            this.pipe = pipe;
            this.log = log;
        }

        public T Content()
        {
            return this.origin.Content();
        }

        public IShape<T> Open(string contentType, string name)
        {
            this.log.Invoke($"Open {contentType} \"{name}\"");
            return
                new Logging<T>(
                    this.origin.Open(contentType, name),
                    this.pipe,
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
            this.log.Invoke($"Put \"{dataType}\" named \"{name}\" \"{this.pipe.Flip(dataType, content)}\"");
        }
    }
}


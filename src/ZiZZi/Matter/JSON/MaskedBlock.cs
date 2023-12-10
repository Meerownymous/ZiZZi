using System;
using System.IO;
using Newtonsoft.Json.Linq;
using ZiZZi.Matter.Object;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Matter which ignores a properties that do not match a given json mask.
    /// </summary>
    public sealed class MaskedBlock<TResult> : IMatter<TResult>
        where TResult : class
    {
        private readonly IMatter<TResult> origin;
        private readonly JToken mask;

        /// <summary>
        /// Matter which ignores a properties that do not match a given json mask.
        /// </summary>
        public MaskedBlock(IMatter<TResult> origin, JToken mask)
        {
            this.origin = origin;
            this.mask = mask;
        }

        public TResult Content()
        {
            return this.origin.Content();
        }

        public IMatter<TResult> Open(string contentType, string name)
        {
            IMatter<TResult> result = new VoidMatter<TResult>();
            if ((this.mask as JObject).ContainsKey(name))
            {
                result = this.origin.Open(contentType, name);
                if (contentType == "block")
                    result = new MaskedBlock<TResult>(result, (this.mask as JObject)[name]);
                else if (contentType == "value-list")
                    result = new MaskedValueList<TResult>(result, (this.mask as JArray));
                else if (contentType == "block-list")
                    result = new MaskedValueList<TResult>(result, (this.mask as JArray));
            }
            return result;
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            if ((this.mask as JObject).ContainsKey(name))
                this.origin.Present(name, content);
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            if ((this.mask as JObject).ContainsKey(name))
                this.origin.Present(name, dataType, content);
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            if ((this.mask as JObject).ContainsKey(name))
                this.origin.Present(name, dataType, content);
        }
    }
}


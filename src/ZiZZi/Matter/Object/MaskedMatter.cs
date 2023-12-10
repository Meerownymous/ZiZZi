using System;
using System.IO;
using Newtonsoft.Json.Linq;
using ZiZZi.Matter.JSON;

namespace ZiZZi.Matter.Object
{
    /// <summary>
    /// Processes only the contents which are included in the
    /// given JSON mask.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public sealed class MaskedMatter<TResult> : IMatter<TResult>
        where TResult : class
    {
        private readonly IMatter<TResult> origin;
        private readonly JToken mask;

        public MaskedMatter(IMatter<TResult> origin, JToken mask)
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
            IMatter<TResult> matter = this.origin.Open(contentType, name); ;
            var subMask = name == "root" ? mask : mask[name];
            if (subMask.Type == JTokenType.Object)
                matter = new MaskedBlock<TResult>(matter, subMask);
            else if (subMask.Type == JTokenType.Array)
            {
                if ((subMask as JArray).Count == 0)
                    matter = new VoidMatter<TResult>();
                else if ((mask as JArray)[0].Type != JTokenType.Object)
                    matter = new MaskedValueList<TResult>(matter, subMask);
                else
                    matter = new MaskedObjectList<TResult>(matter, subMask);
            }
            return matter;
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of media. " +
                $"You must first open a content type."
            );
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of media. " +
                $"You must first open a content type."
            );
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of media. " +
                $"You must first open a content type."
            );
        }
    }
}


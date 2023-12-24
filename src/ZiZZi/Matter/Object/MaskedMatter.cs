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
        private readonly Lazy<JToken> mask;
        private bool isRoot;

        public MaskedMatter(IMatter<TResult> origin, Func<JToken> mask)
        {
            this.origin = origin;
            this.mask = new Lazy<JToken>(mask);
            this.isRoot = true;
        }

        public TResult Content()
        {
            return this.origin.Content();
        }

        public IMatter<TResult> Open(string contentType, string name)
        {
            IMatter<TResult> matter = this.origin.Open(contentType, name);
            var subMask = this.isRoot ? mask.Value : mask.Value[name];
            this.isRoot = false;
            if (subMask.Type == JTokenType.Object)
                matter = new MaskedBlock<TResult>(matter, subMask);
            else if (subMask.Type == JTokenType.Array)
            {
                if ((subMask as JArray).Count == 0)
                    matter = new VoidMatter<TResult>();
                else if ((mask.Value as JArray)[0].Type != JTokenType.Object)
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


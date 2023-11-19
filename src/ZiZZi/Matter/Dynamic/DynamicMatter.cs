using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace ZiZZi.Matter.Dynamic
{
    public sealed class DynamicMatter : IMatter<object>
    {
        private readonly MatterOrigin matter;
        private readonly dynamic root;
        private readonly List<dynamic> sub;

        public DynamicMatter()
        {
            this.matter = new MatterOrigin(new BytesAsTyped());
            this.root = new ExpandoObject();
            this.sub = new List<dynamic>();
        }

        public dynamic Content()
        {
            if (this.sub.Count == 0)
                throw new InvalidOperationException("This matter is not yet formed.");
            return this.sub[0].Content();
        }

        public IMatter<object> Open(string contentType, string name)
        {
            if (this.sub.Count > 0)
                throw new InvalidOperationException(
                    $"You are trying to open a {contentType} named {name} on "
                    + $"root level of the object, but it already has a root."
                );
            var sub = this.matter.Flip(contentType, this.root, name);
            this.sub.Add(sub);
            return sub;
        }

        public void Put(string name, string content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of the dynamic. " +
                $"You must first open a block or array."
            );
        }

        public void Put(string name, string dataType, byte[] content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of the dynamic. " +
                $"You must first open a block or array."
            );
        }

        public void Put(string name, string dataType, Stream content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of the dynamic. " +
                $"You must first open a block or array."
            );
        }
    }
}


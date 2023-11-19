using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Xml.Linq;
using Tonga;

namespace ZiZZi.Matter.Dynamic
{
    /// <summary>
    /// List of blocks which themselves can contain more properties and sub blocks.
    /// </summary>
    public sealed class DynBlockList : IMatter<object>
    {
        private readonly Lazy<IList<object>> container;
        private readonly ISwap<string, dynamic, string, IMatter<dynamic>> matter;

        /// <summary>
        /// List of blocks which themselves can contain more properties and sub blocks.
        /// </summary>
        public DynBlockList(
            ISwap<string, dynamic, string, IMatter<dynamic>> matter,
            dynamic parent,
            string listName
        )
        {
            this.container = new Lazy<IList<object>>(() =>
            {
                IList<object> content = new System.Collections.Generic.List<object>();
                ((IDictionary<string, object>)parent)[listName] = content;
                return content;
            });
            this.matter = matter;
        }

        public dynamic Content()
        {
            return this.container.Value;
        }

        /// <summary>
        /// Opens a block inside the array. Block name must always be the same because it is an array.
        /// </summary>
        public IMatter<object> Open(string contentType, string name)
        {
            if (contentType == "block") contentType = "block-inside-list";
            var subMedia = this.matter.Flip(contentType, this.container.Value, name);
            subMedia.Content();
            return subMedia;
        }

        public void Put(string name, string content)
        {
            throw new InvalidOperationException();
        }

        public void Put(string name, string dataType, byte[] content)
        {
            throw new InvalidOperationException();
        }

        public void Put(string name, string dataType, Stream content)
        {
            throw new InvalidOperationException();
        }
    }
}


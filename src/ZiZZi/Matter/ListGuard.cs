using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter
{
    /// <summary>
    /// Asserts that new blocks are placed using the correct item name,
    /// with which the array was defined.
    /// </summary>
    public sealed class ListGuard<T> : IMatter<T>
    {
        private readonly IMatter<T> origin;
        private readonly IList<string> itemName;
        private readonly bool isValueArray;
        private readonly string arrayName;

        /// <summary>
        /// Asserts that new blocks are placed using the correct item name,
        /// with which the array was defined.
        /// </summary>
        public ListGuard(IMatter<T> origin, string arrayName, bool isValueArray)
        {
            this.origin = origin;
            this.itemName = new System.Collections.Generic.List<string>();
            this.isValueArray = isValueArray;
            this.arrayName = arrayName;
        }

        public T Content()
        {
            return this.origin.Content();
        }

        public IMatter<T> Open(string contentType, string name)
        {
            if(this.isValueArray)
                throw new InvalidOperationException($"Opening a {contentType} inside a value-array is not allowed.");
            return this.origin.Open(contentType, Validated(this.itemName, this.arrayName, name));
        }

        public void Put(string name, Func<string> content)
        {
            if (!this.isValueArray)
                throw new InvalidOperationException($"Cannot put simple values into a block array.");
            this.origin.Put(Validated(this.itemName, this.arrayName, name), content);
        }

        public void Put(string name, string dataType, Func<byte[]> content)
        {
            if (!this.isValueArray)
                throw new InvalidOperationException($"Cannot put simple values into a block array.");
            this.origin.Put(Validated(this.itemName, this.arrayName, name), dataType, content);
        }

        public void Put(string name, string dataType, Func<Stream> content)
        {
            this.Put(name, dataType, () => new AsBytes(new AsInput(content)).Bytes());
        }

        private static string Validated(IList<string> itemName, string arrayName, string candidate)
        {
            if (itemName.Count == 0)
            {
                itemName.Add(candidate);
            }
            else if (!candidate.Equals(itemName[0]))
            {
                throw new ArgumentException(
                    $"The array \"{arrayName}\" expects items to be named the same. Items are named \"{itemName[0]}\". "
                    + $"\"{candidate}\" is therefore not a valid name for an array item."
                );
            }
            return itemName[0];
        }
    }
}


using System;

namespace BLox.Shape
{
    /// <summary>
    /// Asserts that new blocks are placed using the correct item name,
    /// with which the array was defined.
    /// </summary>
    public sealed class AssertsArray<T> : IShape<T>
    {
        private readonly IShape<T> origin;
        private readonly string itemName;
        private readonly bool isValueArray;
        private readonly string arrayName;

        /// <summary>
        /// Asserts that new blocks are placed using the correct item name,
        /// with which the array was defined.
        /// </summary>
        public AssertsArray(IShape<T> origin, string arrayName, string itemName, bool isSimpleArray)
        {
            this.origin = origin;
            this.itemName = itemName;
            this.isValueArray = isSimpleArray;
            this.arrayName = arrayName;
        }

        public T Content()
        {
            return this.origin.Content();
        }

        public IShape<T> Open(string contentType, string name)
        {
            if(this.isValueArray)
                throw new InvalidOperationException($"Opening a {contentType} inside a value-array is not allowed.");
            return this.origin.Open(contentType, Validated(name, this.arrayName, this.itemName));
        }

        public void Put(string name, string content)
        {
            if (!this.isValueArray)
                throw new InvalidOperationException($"Cannot put simple values into a block array.");
            this.origin.Put(Validated(name, this.arrayName, this.itemName), content);
        }

        public void Put(string name, string dataType, byte[] content)
        {
            if (!this.isValueArray)
                throw new InvalidOperationException($"Cannot put simple values into a block array.");
            this.origin.Put(Validated(name, this.arrayName, this.itemName), dataType, content);
        }

        private static string Validated(string itemName, string arrayName, string candidate)
        {
            if (!candidate.Equals(itemName))
                throw new ArgumentException(
                    $"The array \"{arrayName}\" expects items which are named \"{itemName}\". "
                    + $"{candidate} is not a valid name for an array item"
                );
            return itemName;
        }
    }
}


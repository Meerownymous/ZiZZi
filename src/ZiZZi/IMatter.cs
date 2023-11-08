using System.IO;

namespace ZiZZi
{
    /// <summary>
    /// A matter formed by information.
    /// </summary>
    public interface IMatter<T>
    {
        /// <summary>
        /// Open a matter box.
        /// </summary>
        IMatter<T> Open(string contentType, string name);

        /// <summary>
        /// Put text content into the matter.
        /// </summary>
        void Put(string name, string content);

        /// <summary>
        /// Put content into the matter.
        /// </summary>
        void Put(string name, string dataType, byte[] content);

        /// <summary>
        /// Put content into the matter.
        /// </summary>
        void Put(string name, string dataType, Stream content);

        /// <summary>
        /// The final matter.
        /// </summary>
        T Content();
    }
}

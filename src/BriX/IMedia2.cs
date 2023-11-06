using System.IO;

namespace BLox
{
    /// <summary>
    /// A media which you can print content to.
    /// </summary>
    public interface IShape<T>
    {
        /// <summary>
        /// Open a content box.
        /// </summary>
        IShape<T> Open(string contentType, string name);

        /// <summary>
        /// Put text content into the media.
        /// </summary>
        void Put(string name, string content);

        /// <summary>
        /// Put content into the media.
        /// </summary>
        void Put(string name, string dataType, byte[] content);

        /// <summary>
        /// Put content into the media.
        /// </summary>
        void Put(string name, string dataType, Stream content);

        /// <summary>
        /// The printed media.
        /// </summary>
        T Content();
    }
}

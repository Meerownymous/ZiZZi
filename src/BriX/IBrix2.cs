

using BLox.Shape;

namespace BLox
{
    /// <summary>
    /// Content which ca be printed to a media.
    /// </summary>
    public interface IBrix2
    {
        /// <summary>
        /// Print the content to a media.
        /// </summary>
        T Print<T>(IShape<T> media);    
    }
}

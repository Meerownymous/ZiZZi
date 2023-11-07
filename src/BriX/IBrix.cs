

using ZiZZi.Matter;

namespace ZiZZi
{
    /// <summary>
    /// Content which ca be printed to a media.
    /// </summary>
    public interface IBrix
    {
        /// <summary>
        /// Print the content to a media.
        /// </summary>
        T Print<T>(IMedia<T> media);    
    }
}

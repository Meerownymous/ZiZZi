

using ZiZZi.Matter;

namespace ZiZZi
{
    /// <summary>
    /// Brix that does nothing when printing.
    /// </summary>
    public sealed class BxNothing : IBrix
    {
        public T Print<T>(IMedia<T> media)
        {
            return media.Content();
        }
    }
}

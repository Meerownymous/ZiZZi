

using ZiZZi.Matter;

namespace ZiZZi
{
    /// <summary>
    /// Blox that does nothing to a matter.
    /// </summary>
    public sealed class ZiNothing : IBlox
    {
        public T Form<T>(IMatter<T> matter)
            where T : class
        {
            return matter.Content();
        }
    }
}

using ZiZZi.Matter;

namespace ZiZZi
{
    /// <summary>
    /// Information focus which can form matter.
    /// </summary>
    public interface IBlox
    {
        /// <summary>
        /// Form matter from this focus.
        /// </summary>
        T Form<T>(IMatter<T> matter);    
    }
}
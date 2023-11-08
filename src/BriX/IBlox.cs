namespace ZiZZi
{
    /// <summary>
    /// Information focus which can form matter.
    /// </summary>
    public interface IBlox
    {
        /// <summary>
        /// Form matter from this blox.
        /// </summary>
        T Form<T>(IMatter<T> matter);
    }
}
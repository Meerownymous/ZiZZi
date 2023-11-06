

namespace BLox
{
    /// <summary>
    /// A media which you can print content to.
    /// </summary>
    public interface IMedia<T>
    {
        /// <summary>
        /// Create a prop in the current area.
        /// Props can only be created in a block.
        /// </summary>
        IMedia<T> Prop(string name);

        /// <summary>
        /// Create an array in the current area.
        /// Arrays can only be created inside objects or inside arrays.
        /// </summary>
        IMedia<T> Array(string arrayName, string itemName);

        /// <summary>
        /// Create a block in the current area.
        /// Blocks can be created inside blocks, props or arrays. 
        /// If you create them inside an array, you must either not specify a name
        /// or specify the one you gave the arrayItem when making the array.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IMedia<T> Block(string name);

        /// <summary>
        /// Put a value into a prop or an array.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IMedia<T> Put(string value);

        /// <summary>
        /// The printed media.
        /// </summary>
        /// <returns></returns>
        T Content();
    }
}

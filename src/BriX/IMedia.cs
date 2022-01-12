//MIT License

//Copyright (c) 2022 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

namespace BriX
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

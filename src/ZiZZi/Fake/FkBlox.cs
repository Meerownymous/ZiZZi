


using System;
using ZiZZi.Matter;

namespace ZiZZi.Fake
{
    /// <summary>
    /// Fake brix.
    /// </summary>
    public sealed class FkBlox : IBlox
    {
        private readonly Action form;

        /// <summary>
        /// Fake brix.
        /// </summary>
        public FkBlox(): this(() => { })
        { }

        /// <summary>
        /// Fake brix.
        /// </summary>
        public FkBlox(Action form)
        {
            this.form = form;
        }

        public T Form<T>(IMatter<T> matter)
        {
            this.form();
            return matter.Content();
        }
    }
}

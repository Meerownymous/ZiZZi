using System;
using System.Globalization;
using Tonga;

namespace ZiZZi
{
    /// <summary>
    /// A property which has a name and a value.
    /// </summary>
    public sealed class ZiProp : IBlox
    {
        private readonly string name;
        private readonly Func<string> value;

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, long value) : this(name, () => value.ToString(CultureInfo.InvariantCulture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, long value, IFormatProvider culture) : this(name, () => value.ToString(culture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, float value) : this(name, () => value.ToString(CultureInfo.InvariantCulture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, float value, IFormatProvider culture) : this(name, () => value.ToString(culture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, double value) : this(name, () => value.ToString(CultureInfo.InvariantCulture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, double value, IFormatProvider culture) : this(name, () => value.ToString(culture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, int value) : this(name, () => value.ToString(CultureInfo.InvariantCulture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, int value, IFormatProvider culture) : this(name, () => value.ToString(culture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, INumber value) : this(name, () => value.AsDouble().ToString())
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, IScalar<string> value) : this(name, () => value.Value())
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, IText value) : this(name, () => value.AsString())
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(IPair<string, string> pair) : this(pair.Key(), () => pair.Value())
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, string value) : this(name, () => value)
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, Func<string> value)
        {
            this.name = name;
            this.value = value;
        }

        public T Form<T>(IMatter<T> matter)
        {
            matter.Put(this.name, this.value());
            return matter.Content();
        }

        public override string ToString()
        {
            return $"PROP({this.name}:{this.value})";
        }
    }
}

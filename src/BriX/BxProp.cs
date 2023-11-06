﻿using System;
using System.Globalization;
using Tonga;

namespace BLox
{
    /// <summary>
    /// A property which has a name and a value.
    /// </summary>
    public sealed class BxProp : IBrix
    {
        private readonly string name;
        private readonly Func<string> value;

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, long value) : this(name, () => value.ToString(CultureInfo.InvariantCulture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, long value, IFormatProvider culture) : this(name, () => value.ToString(culture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, float value) : this(name, () => value.ToString(CultureInfo.InvariantCulture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, float value, IFormatProvider culture) : this(name, () => value.ToString(culture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, double value) : this(name, () => value.ToString(CultureInfo.InvariantCulture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, double value, IFormatProvider culture) : this(name, () => value.ToString(culture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, int value) : this(name, () => value.ToString(CultureInfo.InvariantCulture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, int value, IFormatProvider culture) : this(name, () => value.ToString(culture))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, INumber value) : this(name, () => value.AsDouble().ToString())
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, IScalar<string> value) : this(name, () => value.Value())
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, IText value) : this(name, () => value.AsString())
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(IPair<string, string> pair) : this(pair.Key(), () => pair.Value())
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, string value) : this(name, () => value)
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public BxProp(string name, Func<string> value)
        {
            this.name = name;
            this.value = value;
        }

        public T Print<T>(IMedia<T> media)
        {
            var prop = media.Prop(this.name);
            prop.Put(this.value());
            return media.Content();
        }

        public override string ToString()
        {
            return $"PROP({this.name}:{this.value})";
        }
    }
}

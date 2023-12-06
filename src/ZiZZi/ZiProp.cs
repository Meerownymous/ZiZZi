using System;
using Tonga;
using Tonga.Bytes;
using Tonga.Text;

namespace ZiZZi
{
    /// <summary>
    /// A property which has a name and a value.
    /// </summary>
    public sealed class ZiProp : IBlox
    {
        private readonly string name;
        private readonly string dataType;
        private readonly Func<IBytes> value;

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, bool value) : this(name, "bool",
            () => AsBytes._(BitConverter.GetBytes(value))
        )
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, long value) : this(name, "long", AsBytes._(value))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, float value) : this(name, "float", AsBytes._(value))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, double value) : this(name, "double", AsBytes._(value))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, int value) : this(name, "integer", AsBytes._(value))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, IScalar<string> value) : this(
            name,
            "text",
            AsBytes._(
                AsText._(value.Value)
            )
        )
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, IText value) : this(name, "text", AsBytes._(value))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(IPair<string, string> pair) : this(
            pair.Key(),
            "string",
            AsBytes._(AsText._(pair.Value))
        )
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, string value) : this(name, () => value)
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, Func<string> value) : this(name, "string", () => AsBytes._(value()))
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, string dataType, IBytes value) : this(name, dataType, () => value)
        { }

        /// <summary>
        /// A property which has a name and a value.
        /// </summary>
        public ZiProp(string name, string dataType, Func<IBytes> value)
        {
            this.name = name;
            this.dataType = dataType;
            this.value = value;
        }

        public T Form<T>(IMatter<T> matter)
        {
            matter.Present(this.name, this.dataType, () => TakeContent._(this.value().Bytes()));
            return matter.Content();
        }

        public override string ToString()
        {
            return $"PROP({this.name}:{this.dataType}/{this.value().Bytes().Length} bytes)";
        }
    }
}

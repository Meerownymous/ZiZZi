using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Tonga;
using Tonga.Bytes;
using Tonga.Enumerable;
using Tonga.IO;
using Tonga.Map;
using ZiZZi.Matter.Dynamic;

namespace ZiZZi.Matter.Object
{
    public sealed class ObjectBlock2<TResult> : IMatter<object>
    {
        private readonly string name;
        private readonly dynamic blueprint;
        private readonly BytesAsTyped conversion;
        private readonly Dictionary<string, IMatter<dynamic>> subObjects;
        private readonly IDictionary<string, object> properties;
        private readonly AsMap<string, Type> propertyTypes;

        public ObjectBlock2(string name, dynamic blueprint, BytesAsTyped conversion)
        {
            this.subObjects = new Dictionary<string, IMatter<dynamic>>();
            this.properties = new Dictionary<string, object>();
            this.propertyTypes =
                AsMap._(
                    AsEnumerable._(
                        Mapped._(
                            prop => AsPair._(prop.Name, prop.PropertyType),
                            typeof(TResult).GetProperties()
                        )
                    )
                );
            this.name = name;
            this.blueprint = blueprint;
            this.conversion = conversion;

        }

        public dynamic Content()
        {
            var parameters = new List<dynamic>();
            foreach(var propName in this.propertyTypes.Keys())
            {
                if(this.properties.ContainsKey(propName))
                {
                    parameters.Add(this.properties[propName]);
                }
                else if(this.subObjects.ContainsKey(propName))
                {
                    parameters.Add(this.subObjects[propName].Content());
                }
                else
                {
                    parameters.Add(typeof(TResult).GetProperty(propName).GetValue(this.blueprint));
                }
            }

            TResult result = (TResult)Activator.CreateInstance(typeof(TResult), parameters.ToArray());
            return result;
        }

        public IMatter<dynamic> Open(string contentType, string name)
        {
            dynamic result = new VoidMatter<dynamic>();
            if (!HasProperty(this.propertyTypes, name))
                throw new ArgumentException($"Object has no property \"{name}\" to be filled.");

            if (contentType == "block")
            {
                IMatter<dynamic> sub =
                    Activator.CreateInstance(
                        typeof(ObjectBlock2<>)
                            .MakeGenericType(this.propertyTypes[name]),
                            name,
                            typeof(TResult).GetProperty(name).GetValue(this.blueprint),
                            new BytesAsTyped()
                    );
                this.subObjects.Add(name, sub);
                result = sub;
            }
            else if(contentType == "value-list")
            {
                var sub = SubValueList(name, this.propertyTypes[name].GetElementType());
                this.subObjects.Add(name, sub);
                result = sub;
            }
            return result;
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            if (HasProperty(this.propertyTypes, name))
            {
                CheckType(this.propertyTypes, name, "string");
                this.properties.Add(name, content().Value());
            }
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            if (HasProperty(this.propertyTypes, name))
            {
                CheckType(this.propertyTypes, name, dataType);
                this.properties.Add(name, 
                    this.conversion.Flip(dataType, name, content().Value())
                );
            }
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            if (HasProperty(this.propertyTypes, name))
            {
                CheckType(this.propertyTypes, name, dataType);
                this.properties.Add(name,
                    this.conversion.Flip(
                        dataType,
                        name,
                        AsBytes._(
                            new AsInput(content().Value())
                        ).Bytes()
                    )
                );
            }
        }

        private static bool HasProperty(IMap<string,Type> props, string propName)
        {
            return props.Keys().Contains(propName);
        }

        private static void CheckType(IMap<string, Type> props, string propName, string typeName)
        {
            if (!props[propName].Name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Cannot fill property '{propName}' with type '{typeName}'. Expected '{props[propName].Name}'");
        }

        private static dynamic SubValueList(string name, Type type)
        {
            return
                Activator.CreateInstance(
                    typeof(ObjectValueList<>)
                        .MakeGenericType(type),
                    name,
                    new BytesAsTyped()
                );
        }
    }
}


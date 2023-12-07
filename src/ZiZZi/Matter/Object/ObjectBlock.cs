//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Dynamic;
//using System.IO;
//using Tonga;
//using Tonga.Bytes;
//using Tonga.Enumerable;
//using Tonga.IO;
//using Tonga.Map;

//namespace ZiZZi.Matter.Object
//{
//    public sealed class ObjectBlock<TResult> : IMatter<object>
//    {
//        private readonly AsMap<string, Type> properties;
//        private readonly BytesAsTyped conversion;
//        private readonly Lazy<IDictionary<string, object>> content;
//        private readonly MatterOrigin2 subMatters;

//        public ObjectBlock(string name, object parent, BytesAsTyped conversion, MatterOrigin2 subMatters)
//        {
//            this.properties =
//                AsMap._(
//                    AsEnumerable._(
//                        Mapped._(
//                            prop => AsPair._(prop.Name, prop.PropertyType),
//                            typeof(TResult).GetProperties()
//                        )
//                    )
//                );
//            this.conversion = conversion;
//            this.content =
//                new Lazy<IDictionary<string, object>>(() =>
//                {
//                    dynamic block = new ExpandoObject();
//                    foreach(var prop in typeof(TResult).GetProperties())
//                    {
//                        if (prop.PropertyType.IsValueType)
//                        {
//                            (block as IDictionary<string,object>)[prop.Name] = Activator.CreateInstance(prop.PropertyType);
//                        }
//                    }
//                    if (IsArrayFamily(parent.GetType()))
//                    {
//                        ((IList<object>)parent).Add(block);
//                    }
//                    else
//                    {
//                        ((IDictionary<string, object>) parent)[name] = block;
//                    }
//                    return block;
//            });
//            this.subMatters = subMatters;

//        }

//        public dynamic Content()
//        {
//            foreach(var key in ((IDictionary<string,object>)this.content.Value).Keys)
//            {
//                if (Array.Exists(
//                    this.content.Value[key].GetType().GetInterfaces(),
//                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMatter<>))
//                )
//                {
//                    this.content.Value[key] = ((dynamic)this.content.Value[key]).Content() as dynamic;
//                }
//            }
//            return this.content.Value;
//        }

//        public IMatter<dynamic> Open(string contentType, string name)
//        {
//            var sub = this.subMatters.Flip(contentType, name);
//            this.content.Value[name] = sub;
//            return sub;
//        }

//        public void Present(string name, Func<IContent<string>> content)
//        {
//            if (HasProperty(this.properties, name))
//            {
//                CheckType(this.properties, name, "string");
//                this.content.Value[name] = content().Value();
//            }
//        }

//        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
//        {
//            if (HasProperty(this.properties, name))
//            {
//                CheckType(this.properties, name, dataType);
//                this.content.Value[name] =
//                    this.conversion.Flip(dataType, name, content().Value());
//            }
//        }

//        public void Present(string name, string dataType, Func<IContent<Stream>> content)
//        {
//            if (HasProperty(this.properties, name))
//            {
//                CheckType(this.properties, name, dataType);
//                this.content.Value[name] =
//                    this.conversion.Flip(
//                        dataType,
//                        name,
//                        AsBytes._(
//                            new AsInput(content().Value())
//                        ).Bytes()
//                    );
//            }
//        }

//        private static bool IsArrayFamily(Type candidate)
//        {
//            return
//                candidate.IsArray
//                ||
//                (candidate.IsGenericType && candidate.GetGenericTypeDefinition().GetInterface("ICollection") != null);
//        }

//        private static bool HasProperty(IMap<string,Type> props, string propName)
//        {
//            return props.Keys().Contains(propName);
//        }

//        private static void CheckType(IMap<string, Type> props, string propName, string typeName)
//        {
//            if (!props[propName].Name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
//                throw new ArgumentException($"Cannot fill property '{propName}' with type '{typeName}'. Expected '{props[propName].Name}'");
//        }
//    }
//}


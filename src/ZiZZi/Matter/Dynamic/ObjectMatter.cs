using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Tonga.Enumerable;
using Tonga.Scalar;
using ZiZZi.Matter.Dynamic;

namespace ZiZZi.Matter.Object
{
    /// <summary>
    /// Expands a ZiZZi object into a real object.
    /// </summary>
    public sealed class ObjectMatter<TResult> : IMatter<TResult>
        where TResult : class
    {
        private readonly IMatter<object> matter;
        private readonly Type blueprint;
        private readonly int level;
        private int contents;

        /// <summary>
        /// Expands a ZiZZi object into a real object.
        /// </summary>
        public ObjectMatter() : this(
            new DynamicMatter(),
            typeof(TResult),
            0
        )
        { }

        /// <summary>
        /// Expands a ZiZZi object into a real object.
        /// </summary>
        private ObjectMatter(IMatter<object> origin, Type blueprint, int level)
        {
            this.matter = origin;
            this.blueprint = blueprint;
            this.level = level;
            this.contents = 0;
        }

        public TResult Content()
        {
            TResult result = default(TResult);
            if (this.level == 1 && this.contents != 0)
            {
                this.contents--;
            }
            else if (this.level == 1 && this.contents == 0)
            {
                ExpandoObject content = (ExpandoObject)this.matter.Content();
                result =
                    typeof(TResult).Name.StartsWith("<>f__AnonymousType") ?
                    ExpandedAnonymousType<TResult>(content)
                    :
                    ExpandedSolidType(content);
            }
            else if(this.level == 0 && this.contents == 0)
            {
                var c = this.matter.Content();
                if(IsArrayFamily(typeof(TResult)))
                {
                    var elementType = typeof(TResult).GetElementType();
                    Type genericType = typeof(List<>).MakeGenericType(elementType);
                    dynamic casted = Activator.CreateInstance(genericType);
                    MethodInfo addMethod = casted.GetType().GetMethod("Add");
                    foreach (var item in c as List<object>)
                    {
                        addMethod.Invoke(casted, new object[] { ExpandedAnonymousType(elementType, item as ExpandoObject) });
                    }
                    result = casted;
                }
            }
            return result;
        }

        public IMatter<TResult> Open(string contentType, string name)
        {
            IMatter<TResult> result;
            if (this.level == 0)
            {
                result =
                    new ObjectMatter<TResult>(
                        this.matter.Open(contentType, name),
                        this.blueprint,
                        this.level + 1
                    );
            }
            else
            {
                var has = HasProperty(name, this.blueprint) || IsArrayFamily(this.blueprint);
                result =
                    has ?
                    new ObjectMatter<TResult>(
                        this.matter.Open(contentType, name),
                        IsArrayFamily(this.blueprint)
                        ?
                        this.blueprint
                        :
                        this.blueprint.GetProperty(name).PropertyType,
                        this.level + 1
                    )
                    :
                    new VoidMatter<TResult>();
            }
            return result;
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            this.contents++;
            if(HasProperty(name, this.blueprint) || IsArrayFamily(this.blueprint))
                this.matter.Present(name, content);
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            this.contents++;
            if (HasProperty(name, this.blueprint) || IsArrayFamily(this.blueprint))
                this.matter.Present(name, dataType, content);
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            this.contents++;
            if (HasProperty(name, this.blueprint) || IsArrayFamily(this.blueprint))
                this.matter.Present(name, dataType, content);
        }

        private TExpanded ExpandedAnonymousType<TExpanded>(ExpandoObject source)
        {
            return (TExpanded)ExpandedAnonymousType(typeof(TExpanded), source);
        }

        private object ExpandedAnonymousType(Type texpanded, ExpandoObject source)
        {
            IDictionary<string, object> dict = source;
            var ctor =
                First._(
                    texpanded.GetConstructors()
                ).Value();

            IList<object> parameters = new List<object>();
            foreach (var parameter in ctor.GetParameters())
            {
                if (dict.ContainsKey(parameter.Name))
                {
                    if (dict[parameter.Name] is ExpandoObject)
                    {
                        parameters.Add(ExpandedAnonymousType(parameter.ParameterType, dict[parameter.Name] as ExpandoObject));
                    }
                    else
                    {
                        parameters.Add(dict[parameter.Name]);
                    }
                }
                else
                {
                    var def = parameter.GetType().IsValueType ? Activator.CreateInstance(parameter.GetType()) : null;
                    parameters.Add(def);
                }
            }
            var obj = ctor.Invoke(parameters.ToArray());
            return obj;
        }

        private TResult ExpandedSolidType(ExpandoObject source)
        {
            throw new ArgumentException("Expanding into non-Anonymous types is not yet supported.");
        }

        private static bool HasProperty(string name, Type blueprint)
        {
            return
                Length._(
                    Filtered._(
                        propName => propName == name,
                        Mapped._(
                            prop => prop.Name,
                            blueprint.GetProperties()
                        )
                    )
                ).Value() > 0;
        }

        private static bool IsArrayFamily(Type candidate)
        {
            return
                candidate.IsArray
                ||
                (candidate.IsGenericType && candidate.GetGenericTypeDefinition().GetInterface("ICollection") != null);
        }
    }

    /// <summary>
    /// Expands a ZiZZi object into a real object.
    /// </summary>
    public static class ObjectMatter
    {
        /// <summary>
        /// Expands a ZiZZi object into a real object.
        /// </summary>
        public static ObjectMatter<T> Fill<T>(T typeHolder)
            where T : class
        {
            return new ObjectMatter<T>();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Tonga.Scalar;

namespace ZiZZi.Matter.Dynamic
{
    /// <summary>
    /// Expands a ZiZZi object into a real object.
    /// </summary>
    public sealed class ObjectMatter<TResult> : IMatter<TResult>
        where TResult : class
    {
        private readonly IMatter<object> matter;
        private readonly int level;
        private int contents;

        /// <summary>
        /// Expands a ZiZZi object into a real object.
        /// </summary>
        public ObjectMatter() : this(
            new DynamicMatter(), 0
        )
        { }

        /// <summary>
        /// Expands a ZiZZi object into a real object.
        /// </summary>
        private ObjectMatter(IMatter<object> origin, int level)
        {
            this.matter = origin;
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
                
                if (this.level == 1 && this.contents == 0)
                {
                    result =
                        typeof(TResult).Name.StartsWith("<>f__AnonymousType") ?
                        ExpandedAnonymousType<TResult>(content)
                        :
                        ExpandedSolidType(content);
                }
            }

            return result;
        }

        public IMatter<TResult> Open(string contentType, string name)
        {
            return
                new ObjectMatter<TResult>(
                    this.matter.Open(contentType, name), this.level + 1
                );
        }

        public void Put(string name, string content)
        {
            this.contents++;
            this.matter.Put(name, content);
        }

        public void Put(string name, string dataType, byte[] content)
        {
            this.contents++;
            this.matter.Put(name, dataType, content);
        }

        public void Put(string name, string dataType, Stream content)
        {
            this.contents++;
            this.matter.Put(name, dataType, content);
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


using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZiZZi.Matter.JSON;

namespace ZiZZi.Matter.Object
{
    /// <summary>
    /// Expands a ZiZZi object into a real object.
    /// </summary>
    public sealed class ObjectMatter<TResult> : IMatter<TResult>
        where TResult : class
    {
        private readonly IMatter<JContainer> matter;
        private readonly TResult blueprint;
        private readonly int level;
        private int contents;

        /// <summary>
        /// Expands a ZiZZi object into a real object.
        /// </summary>
        public ObjectMatter(TResult blueprint) : this(
            new MaskedMatter<JContainer>(
                new JsonMatter(),
                () => JToken.Parse(JsonConvert.SerializeObject(blueprint))
            ),
            blueprint,
            0
        )
        { }

        /// <summary>
        /// Expands a ZiZZi object into a real object.
        /// </summary>
        private ObjectMatter(IMatter<JContainer> origin, TResult blueprint, int level)
        {
            this.matter = origin;
            this.blueprint = blueprint;
            this.level = level;
            this.contents = 0;
        }

        public TResult Content()
        {
            TResult result = default(TResult);
            if (this.level == 1) //object
            {
                var content = this.matter.Content();
                result = JsonConvert.DeserializeAnonymousType(
                    content.ToString(),
                    this.blueprint
                );
            }
            else if (this.level == 0 && this.contents == 0) //array
            {
                var content = this.matter.Content() as JProperty;
                result = JsonConvert.DeserializeAnonymousType(content.Value.ToString(), this.blueprint);
            }
            return result;
        }

        public IMatter<TResult> Open(string contentType, string name) =>
            new ObjectMatter<TResult>(
                this.matter.Open(contentType, name),
                this.blueprint,
                this.level + 1
            );

        public void Present(string name, Func<IContent<string>> content)
        {
            this.contents++;
            this.matter.Present(name, content);
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            this.contents++;
            this.matter.Present(name, dataType, content);
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            this.contents++;
            this.matter.Present(name, dataType, content);
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
        public static ObjectMatter<T> Fill<T>(T blueprint)
            where T : class
        {
            return new ObjectMatter<T>(blueprint);
        }
    }
}


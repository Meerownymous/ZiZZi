

using System;
using System.Collections.Generic;
using Yaapii.Atoms.Text;

namespace ZiZZi.Matter
{
    /// <summary>
    /// A media in Yaml format.
    /// </summary>
    public sealed class YamlMedia : IMedia<string>
    {
        private readonly int spaces;
        private readonly IList<string> current;
        private readonly int tabs;
        private readonly string last;

        /// <summary>
        /// A media in Yaml format.
        /// </summary>
        /// <param name="spaces">Enter the amount of spaces standard is 2 other is 4</param>
        public YamlMedia(int spaces = 2) : this(spaces, new List<string>(), 0, string.Empty)
        { }

        private YamlMedia(int spaces, IList<string> current, int tabs, string last)
        {
            this.spaces = spaces;
            this.current = current;
            this.tabs = tabs;
            this.last = last;
        }

        public IMedia<string> Array(string arrayName, string itemName)
        {
            current.Add($"{Spaces()}{arrayName}:{Environment.NewLine}");
            return new YamlMedia(this.spaces, current, this.tabs + 1, "array");
        }

        public IMedia<string> Block(string name)
        {
            var tabs = 0;
            if (!name.Equals(string.Empty))
            {
                current.Add($"{Spaces()}{name}:{Environment.NewLine}");
                tabs = 1;
            }
            return new YamlMedia(this.spaces, current, this.tabs + tabs, string.IsNullOrEmpty(name) ? "array" : "block");
        }

        public string Content()
        {
            return new Joined("", this.current).AsString();
        }

        public IMedia<string> Prop(string name)
        {
            if (last.Equals("array"))
            {
                current.Add($"{Spaces()}- {name}:");
            }
            else
            {
                current.Add($"{Spaces()}{name}:");
            }
            return new YamlMedia(this.spaces, current, this.tabs, "prop");
        }

        public IMedia<string> Put(string value)
        {
            if (last.Equals("array"))
                current.Add($"{Spaces()}- {value}{Environment.NewLine}");
            else
                current.Add($" {value}{Environment.NewLine}");
            return new YamlMedia(this.spaces, current, this.tabs, last);
        }

        private string Spaces()
        {
            return new Repeated(new Repeated(" ", this.spaces).AsString(), this.tabs).AsString();
        }
    }
}

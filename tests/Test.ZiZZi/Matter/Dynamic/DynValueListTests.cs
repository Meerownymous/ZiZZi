using System;
using System.Dynamic;
using Xunit;

namespace ZiZZi.Matter.Object.Test
{
    public sealed class DynValueListTests
    {
        [Fact]
        public void CreatesList()
        {
            dynamic root = new ExpandoObject();
            new DynValueList(
                new BytesAsTyped(),
                root,
                "Todos"
            ).Content();

            Assert.Equal(
                new object[0],
                root.Todos
            );
        }

        [Fact]
        public void PutsTextIntoList()
        {
            dynamic root = new ExpandoObject();
            new DynValueList(
                new BytesAsTyped(),
                root,
                "Todos"
            ).Present("Todo", () => TakeContent._("Remove Typesafety"));

            Assert.Equal(
                "Remove Typesafety",
                root.Todos[0]
            );
        }

        [Fact]
        public void PutsByteBasedContentAsStringIntoList()
        {
            dynamic root = new ExpandoObject();
            new DynValueList(
                new BytesAsTyped(),
                root,
                "MagicNumbers"
            ).Present("MagicNumber", "double", () => TakeContent._(BitConverter.GetBytes(12.12)));

            Assert.Equal(
                "12.12",
                root.MagicNumbers[0]
            );
        }
    }
}


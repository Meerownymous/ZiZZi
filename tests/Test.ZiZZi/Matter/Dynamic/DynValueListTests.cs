using System;
using System.Dynamic;
using Xunit;

namespace ZiZZi.Matter.Dynamic.Test
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
            ).Put("Todo", "Remove Typesafety");

            Assert.Equal(
                "Remove Typesafety",
                root.Todos[0]
            );
        }

        [Fact]
        public void PutsByteBasedContentIntoList()
        {
            dynamic root = new ExpandoObject();
            new DynValueList(
                new BytesAsTyped(),
                root,
                "MagicNumbers"
            ).Put("MagicNumber", "double", BitConverter.GetBytes(12.12));

            Assert.Equal(
                12.12,
                root.MagicNumbers[0]
            );
        }
    }
}


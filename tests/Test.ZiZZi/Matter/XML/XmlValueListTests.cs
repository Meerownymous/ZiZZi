using System;
using System.Xml.Linq;
using Xunit;

namespace ZiZZi.Matter.XML.Test
{
    public sealed class XmlValueListTests
    {
        [Fact]
        public void CreatesList()
        {
            var matter =
                new XmlValueList(
                    new BytesAsXElement(),
                    new XElement("Root"),
                    "Todos"
                );

            Assert.Equal(
                "<Todos />",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void PutsTextIntoArray()
        {
            var matter =
                new XmlValueList(
                    new BytesAsXElement(),
                    new XElement("Root"),
                    "Todos"
                );
            matter.Put("Todo", "Nothing");

            Assert.Equal(
                "<Todos><Todo>Nothing</Todo></Todos>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void PutsByteBasedContentIntoList()
        {
            var matter =
                new XmlValueList(
                    new BytesAsXElement(),
                    new XElement("Root"),
                    "Todos"
                );

            matter.Put("Todo", "double", BitConverter.GetBytes(12.12));

            Assert.Equal(
                "<Todos><Todo>12.12</Todo></Todos>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}


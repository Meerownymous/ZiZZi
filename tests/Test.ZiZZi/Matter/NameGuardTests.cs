using System;
using System.Xml.Linq;
using Xunit;
using ZiZZi.Matter.XML;

namespace ZiZZi.Matter.Test
{
    public sealed class NameGuardTests
    {
        [Fact]
        public void RejectsWrongName()
        {
            Assert.Throws<ArgumentException>(() =>
                new NameGuard<XNode>(
                    "Kevin",
                    new XmlMatter()
                )
                .Open("block", "Gustavo")
            );
        }

        [Fact]
        public void InsertsNameWhenNoneIsGiven()
        {
            Assert.Equal(
                "<Kevin />",
                new NameGuard<XNode>(
                    "Kevin",
                    new XmlMatter()
                )
                .Open("block", String.Empty)
                .Content()
                .ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AcceptsNameWhenEqualToGiven()
        {
            Assert.Equal(
                "<Kevin />",
                new NameGuard<XNode>(
                    "Kevin",
                    new XmlMatter()
                )
                .Open("block", "Kevin")
                .Content()
                .ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}


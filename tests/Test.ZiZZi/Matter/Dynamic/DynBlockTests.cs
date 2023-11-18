using System;
using System.Dynamic;
using System.Xml.Linq;
using Tonga.Bytes;
using Xunit;
using ZiZZi.Matter.XML;

namespace ZiZZi.Matter.Dynamic.Tests
{
    public sealed class DynBlockTests
    {
        [Fact]
        public void PutsStringProperty()
        {
            dynamic parent = new ExpandoObject();
            new DynBlock(
                new MatterOrigin(new BytesAsTyped()),
                new BytesAsTyped(),
                parent,
                "TheBlock",
                false
            ).Put("Name", "Mr.Block");

            Assert.Equal("Mr.Block", parent.TheBlock.Name);
        }

        [Fact]
        public void PutsTypedProperty()
        {
            dynamic parent = new ExpandoObject();
            new DynBlock(
                new MatterOrigin(new BytesAsTyped()),
                new BytesAsTyped(),
                parent,
                "TheBlock",
                false
            ).Put("Age", "integer", AsBytes._(2000).Bytes());

            Assert.Equal(2000, parent.TheBlock.Age);
        }

        [Fact]
        public void OpensSubBlock()
        {
            dynamic root = new ExpandoObject();
            new DynBlock(
                new MatterOrigin(new BytesAsTyped()),
                new BytesAsTyped(),
                root,
                "User",
                false
            ).Open("block", "Skills")
            .Put("Thinking", "Dynamic");

            Assert.Equal(
                "Dynamic",
                root.User.Skills.Thinking
            );
        }
    }
}


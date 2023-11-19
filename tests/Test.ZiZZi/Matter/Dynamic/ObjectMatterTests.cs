using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace ZiZZi.Matter.Dynamic.Test
{
    public sealed class ObjectMatterTests
    {

        [Fact]
        public void ExpandsPropertyValues()
        {
            Assert.Equal(
                "The Expandables",
                new ZiBlock("root",
                    new ZiProp("ID", "1000000"),
                    new ZiProp("Name", "The Expandables"),
                    new ZiBlock("Address",
                        new ZiProp("Street", "Heroplace 1")
                    )
                ).Form(
                    ObjectMatter.Fill(
                        new { ID = "", Name = "" }
                    )
                ).Name
            );
        }

        [Fact]
        public void ExpandsNonAnonymousNestedObjects()
        {
            Assert.Equal(
                "Heroplace 1",
                new ZiBlock("root",
                    new ZiValueList("Addresses", "Address",
                        "Heroplace 1"
                    )
                ).Form(
                    ObjectMatter.Fill(
                        new { ID = "", Name = "", Addresses = new List<string>() }
                    )
                ).Addresses[0]
            );
        }

        [Fact]
        public void ExpandsNestedObjectsPropertyValues()
        {
            Assert.Equal(
                "Heroplace 1",
                new ZiBlock("root",
                    new ZiProp("ID", "1000000"),
                    new ZiProp("Name", "The Expandables"),
                    new ZiBlock("Address",
                        new ZiProp("Street", "Heroplace 1")
                    )
                ).Form(
                    ObjectMatter.Fill(
                        new { ID = "", Name = "", Address = new { Street = "" } }
                    )
                ).Address.Street
            );
        }
    }
}


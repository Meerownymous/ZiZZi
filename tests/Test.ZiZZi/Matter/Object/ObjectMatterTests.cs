using System;
using Tonga.Enumerable;
using Xunit;

namespace ZiZZi.Matter.Object.Test
{
    public sealed class ObjectMatterTests
    {
        [Fact]
        public void ExpandsPropertyValuesOfBlock()
        {
            Assert.Equal(
                "The Expandables",
                new ZiBlock("Propulation",
                    new ZiProp("ID", "1000000"),
                    new ZiProp("Name", "The Expandables"),
                    new ZiProp("Name2", "The Expandables")
                ).Form(
                    ObjectMatter.Fill(
                        new { ID = "", Name = "", Name2 = "" }
                    )
                ).Name
            );
        }

        [Fact]
        public void ExpandsWithOnlyOneProperty()
        {
            Assert.Equal(
                "Success-Avenue",
                new ZiBlock("Something",
                    new ZiProp("Street", "Success-Avenue")
                )
                .Form(ObjectMatter.Fill(new { Street = String.Empty }))
                .Street
            );
        }

        [Fact]
        public void ExpandsPropertyValuesOfBlockInArray()
        {
            Assert.Equal(
                "The Expandables",
                new ZiBlockList("Listing", "block",
                    new ZiBlock("block",
                        new ZiProp("ID", "1000000"),
                        new ZiProp("Name", "The Expandables")
                    )
                ).Form(
                    ObjectMatter.Fill(
                        new[]
                        {
                            new { ID = "", Name = "" }
                        }
                    )
                )[0].Name
            );
        }

        [Fact]
        public void DoesNotCompileNonRequestedProperties()
        {
            Assert.Equal(
                "1000000",
                new ZiBlock("Lazy",
                    new ZiProp("ID", "1000000"),
                    new ZiProp("Name", () => throw new System.Exception("I shall not be called")),
                    new ZiValueList("Addresses", "Address", () =>
                        Lambda._(
                            () => throw new System.Exception("I shall not be raised"),
                            AsEnumerable._("I shall not be enumerated")
                        )
                    )
                ).Form(
                    ObjectMatter.Fill(
                        new { ID = "" }
                    )
                ).ID
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


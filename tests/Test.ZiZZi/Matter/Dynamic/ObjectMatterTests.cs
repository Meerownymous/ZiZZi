using System.Collections;
using System.Collections.Generic;
using Tonga.Enumerable;
using Xunit;

namespace ZiZZi.Matter.Object.Test
{
    public sealed class ObjectMatterTests
    {
        [Fact]
        public void ExpandsPropertyValues()
        {

            var t =
                new ZiBlockList("root", "block",
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
                );

            //Assert.Equal(
            //    "The Expandables",
            //    new ZiBlockList("root", "block",
            //        new ZiBlock("block",
            //            new ZiProp("ID", "1000000"),
            //            new ZiProp("Name", "The Expandables")
            //        )
            //    ).Form(
            //        ObjectMatter.Fill(
            //            new[]
            //            {
            //                new { ID = "", Name = "" }
            //            }
            //        )
            //    )[0].Name
            //);
        }

        [Fact]
        public void DoesNotCompileNonRequestedProperties()
        {
            Assert.Equal(
                "1000000",
                new ZiBlock("root",
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


using System;
using System.Dynamic;
using System.Xml.Linq;
using Xunit;

namespace ZiZZi.Matter.Dynamic.Test
{
    public sealed class DynBlockListTests
    {
        [Fact]
        public void CreatesList()
        {
            dynamic root = new ExpandoObject();
            new DynBlockList(
                new MatterOrigin(new BytesAsTyped()),
                root,
                "Worlds"
            ).Content();

            Assert.Equal(
                new object[0],
                root.Worlds
            );
        }

        [Fact]
        public void OpensBlockInList()
        {
            dynamic root = new ExpandoObject();
            new DynBlockList(
                new MatterOrigin(new BytesAsTyped()),
                root,
                "Worlds"
            )
            .Open("block", "UpsideDown")
            .Put("FloorLocation", () => "up");

            Assert.Equal(
                "up",
                root.Worlds[0].FloorLocation
            );
        }
    }
}




using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace BLox.Shape.Test
{
    public sealed class DocuTests
    {
        [Fact(Skip = "For documentation only")]
        public void ComplexExampleForDocu()
        {
            var brix = new BxBlock("rootBlock",
                new BxBlock("Block1",
                    new BxBlockArray("BlockArrayName", "BlockArrayItemName",
                        new BxBlock(
                            new BxProp("Prop-Inside-a-empty-block", "the Value with special chars §$%%&/><|öä#ü'*#\"")
                        )
                    )
                ),
                new BxBlock("Block2",
                    new BxArray("Array", "arrayItemName", "first", "second", "third")
                ),
                new BxBlock("Block3",
                    new BxMap(
                        "BxMap", "thefirstValue",
                        "bxMapKey", "second entry"
                    )
                )
            );
            var json = brix.Print(new JsonMedia()).ToString();
            var xml = brix.Print(new XmlMedia()).ToString();
            var rebuild = 
                new TextOf(
                    new BytesOf(brix.Print(new RebuildMedia()).Document.Root.ToString(SaveOptions.DisableFormatting))
                ).AsString();
            var yaml = brix.Print(new YamlMedia()).ToString();
        }
    }
}

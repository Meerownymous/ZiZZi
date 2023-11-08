

using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;
using ZiZZi.Matter.XML;

namespace ZiZZi.Matter.Test
{
    public sealed class DocuTests
    {
        [Fact(Skip = "For documentation only")]
        public void ComplexExampleForDocu()
        {
            var blox = new ZiBlock("rootBlock",
                new ZiBlock("Block1",
                    new ZiBlockList("BlockArrayName", "BlockArrayItemName",
                        new ZiBlock(
                            new ZiProp("Prop-Inside-a-empty-block", "the Value with special chars §$%%&/><|öä#ü'*#\"")
                        )
                    )
                ),
                new ZiBlock("Block2",
                    new ZiValueList("Array", "arrayItemName", "first", "second", "third")
                ),
                new ZiBlock("Block3",
                    new ZiMap(
                        "BxMap", "thefirstValue",
                        "bxMapKey", "second entry"
                    )
                )
            );
            var json = blox.Form(new JsonMatter()).ToString();
            var xml = blox.Form(new XmlMatter()).ToString();
        }
    }
}

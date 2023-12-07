using System;
using System.Collections.Generic;
using System.Dynamic;
using Tonga.Bytes;
using Xunit;

namespace ZiZZi.Matter.Object.Test
{
    public sealed class ObjectBlockTests
    {
        [Fact]
        public void PutsStringProperty()
        {
            dynamic parent = new ExpandoObject();
            var blueprint = new { Name = "" };

            Type blockType =
                typeof(ObjectBlock2<>)
                    .MakeGenericType(blueprint.GetType());

            dynamic block =
                Activator.CreateInstance(
                    blockType,
                    blueprint,
                    new BytesAsTyped()
                );

            Func<IContent<string>> ct = () => TakeContent._("Mr.Block");
            block.Present("Name", ct);

            Assert.Equal("Mr.Block", block.Content().Name);
        }

        [Fact]
        public void PutsIntProperty()
        {
            dynamic parent = new ExpandoObject();
            var blueprint = new { Name = "", Age = 0 };

            Type blockType =
                typeof(ObjectBlock2<>)
                    .MakeGenericType(blueprint.GetType());

            dynamic block =
                Activator.CreateInstance(
                    blockType,
                    blueprint,
                    new BytesAsTyped()
                );

            Func<IContent<byte[]>> ct = () => TakeContent._(AsBytes._(2000).Bytes());
            block.Present("Age", "int32", ct);

            Assert.Equal(2000, block.Content().Age);
        }

        [Fact]
        public void PutsDoubleProperty()
        {
            dynamic parent = new ExpandoObject();
            var blueprint = new { Name = "", Factor = 0.5 };

            Type blockType =
                typeof(ObjectBlock2<>)
                    .MakeGenericType(blueprint.GetType());

            dynamic block =
                Activator.CreateInstance(
                    blockType,
                    blueprint,
                    new BytesAsTyped()
                );

            Func<IContent<byte[]>> ct = () => TakeContent._(AsBytes._(0.8).Bytes());
            block.Present("Factor", "double", ct);

            Assert.Equal(0.8, block.Content().Factor);
        }

        [Fact]
        public void PutsLongProperty()
        {

            var blueprint = new { Name = "", BigNumber = 0L };
            dynamic block =
                Activator.CreateInstance(
                    typeof(ObjectBlock2<>)
                        .MakeGenericType(blueprint.GetType()),
                    blueprint,
                    new BytesAsTyped()
                );

            Func<IContent<byte[]>> ct = () => TakeContent._(BitConverter.GetBytes(999L));
            block.Present("BigNumber", "int64", ct);

            Assert.Equal(999L, block.Content().BigNumber);
        }

        [Fact]
        public void IncludesBlueprintProperties()
        {
            var blueprint = new { BigNumber = 0L };
            dynamic block =
                Activator.CreateInstance(
                    typeof(ObjectBlock2<>)
                        .MakeGenericType(blueprint.GetType()),
                    blueprint,
                    new BytesAsTyped()
                );

            Assert.Equal(0L, block.Content().BigNumber);
        }

        [Fact]
        public void RejectsTypeMismatch()
        {
            dynamic parent = new ExpandoObject();
            var blueprint = new { Name = "", Age = 0 };

            Type blockType =
                typeof(ObjectBlock2<>)
                    .MakeGenericType(blueprint.GetType());

            dynamic block =
                Activator.CreateInstance(
                    blockType,
                    blueprint,
                    new BytesAsTyped()
                );

            Func<IContent<byte[]>> ct = () => TakeContent._(AsBytes._("BOOM").Bytes());
            Assert.Throws<ArgumentException>(() =>
            {
                block.Present("Age", "string", ct);
            });
        }

        [Fact]
        public void OpensSubBlock()
        {
            var blueprint =
                new
                {
                    Name = "",
                    Author = new { Name = "" }
                };

            dynamic block =
                Activator.CreateInstance(
                    typeof(ObjectBlock2<>)
                        .MakeGenericType(blueprint.GetType()),
                    blueprint,
                    new BytesAsTyped()
                );

            Func<IContent<string>> nameContent = () => TakeContent._("How to ignore your cat");
            block.Present("Name", nameContent);

            var author =
                block.Open("block", "Author");

            Func<IContent<string>> authorContent = () => TakeContent._("The annoyed programmer");
            author.Present("Name", authorContent);

            Assert.Equal("The annoyed programmer", block.Content().Author.Name);
        }

        [Fact]
        public void OpensSubValueList()
        {
            var blueprint =
                new
                {
                    Name = "",
                    Contents = new[] { "" }
                };

            dynamic block =
                Activator.CreateInstance(
                    typeof(ObjectBlock2<>)
                        .MakeGenericType(blueprint.GetType()),
                    blueprint,
                    new BytesAsTyped()
                );

            Func<IContent<string>> nameContent = () => TakeContent._("How to ignore your cat");
            block.Present("Name", nameContent);

            var contents =
                block.Open("value-list", "Contents");

            Func<IContent<string>> authorContent = () => TakeContent._("The annoyed programmer");
            contents.Present("Content", authorContent);

            Assert.Equal("The annoyed programmer", block.Content().Contents[0]);
        }
    }
}
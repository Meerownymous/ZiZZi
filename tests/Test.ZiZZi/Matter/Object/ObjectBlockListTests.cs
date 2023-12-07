using System;
using System.Dynamic;
using Tonga.Bytes;
using Xunit;

namespace ZiZZi.Matter.Object.Test
{
    public sealed class ObjectBlockListTests
    {
        [Fact]
        public void AddsBlocksToList()
        {
            dynamic parent = new ExpandoObject();
            var users = new string[] { "Bob", "Jay" };
            var blueprint = new { ID = 0, Name = "" };

            Type listType =
                typeof(ObjectBlockList<>)
                    .MakeGenericType(blueprint.GetType());

            dynamic block =
                Activator.CreateInstance(
                    listType,
                    blueprint,
                    new BytesAsTyped()
                );

            for(var i=0;i<users.Length;i++)
            {
                var user = block.Open("block-inside-list", "User");
                Func<IContent<string>> name = () => TakeContent._(users[i]);
                Func<IContent<byte[]>> id = () => TakeContent._(BitConverter.GetBytes(i));
                user.Present("Name", name);
                user.Present("ID", "int32", id);
            }

            for (var i = 0; i < users.Length; i++)
            {
                Assert.Equal(users[i], block.Content()[i].Name);
            }
        }
    }
}
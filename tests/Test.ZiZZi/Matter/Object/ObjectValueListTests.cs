using System;
using Xunit;

namespace ZiZZi.Matter.Object.Test
{
    public sealed class ObjectValueListTests
    {
        [Fact]
        public void AddsValues()
        {
            var lst =
                new ObjectValueList<string>(
                    "books", new BytesAsTyped()
                );
            lst.Present("book", () => TakeContent._("1x1 of testing"));

            Assert.Equal(
                new string[] { "1x1 of testing" },
                lst.Content()
            );
        }

        [Fact]
        public void RejectsTypeMismatch()
        {
            var lst =
                new ObjectValueList<string>(
                    "books", new BytesAsTyped()
                );
            Assert.Throws<ArgumentException>(() =>
                lst.Present("book", "int32", () => TakeContent._(BitConverter.GetBytes(1 * 1)))
            );
        }
    }
}


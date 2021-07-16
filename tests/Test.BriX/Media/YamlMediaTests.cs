//MIT License

//Copyright (c) 2021 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BriX.Media.Test
{
    public sealed class YamlMediaTests
    {
        [Fact]
        public void CreatesPropertyInBlock()
        {
            var media = new YamlMedia();

            media.Block("root")
                .Prop("key")
                .Put("lock");

            Assert.Equal(
                "root:\r\n  key: lock\r\n",
                media.Content().ToString(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void CreatesBlockInRoot()
        {
            var media = new YamlMedia();
            media.Block("root")
                .Prop("key")
                .Put("value");

            Assert.Contains(
                "value",
                media.Content()
            );
        }

        [Fact]
        public void CreatesBlockInProp()
        {
            var media = new YamlMedia();
            media.Block("root")
                .Prop("my-block")
                .Block("contents");

            Assert.Equal(
                "root:\r\n  my-block:  contents:\r\n",
                media.Content(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void BuildsBlockInBlock()
        {
            var media = new YamlMedia();
            media.Block("root")
                .Block("contents");

            Assert.Equal(
                "root:\r\n  contents:\r\n",
                media.Content(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void CreatesArrayAtRoot()
        {
            var media = new YamlMedia();
            media
                .Array("wealth", "dev")
                .Put("100 coins")
                .Put("200 coins");

            Assert.Equal(
                "wealth:\r\n  - 100 coins\r\n  - 200 coins\r\n",
                media.Content().ToString(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void CreatesArrayInBlock()
        {
            var media = new YamlMedia();

            media
                .Block("root")
                .Array("keys", "key");

            Assert.Equal(
                "root:\r\n  keys:\r\n",
                media.Content(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void CreatesArrayInArray()
        {
            var media = new YamlMedia();

            media
                .Array("keys", "key")
                .Array("subarray", "subkey");

            Assert.Equal(
                "keys:\r\n  subarray:\r\n",
                media.Content(),
                false,
                true,
                false
            );
        }

    }
}

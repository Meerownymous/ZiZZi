﻿using System;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Xunit;
using Yaapii.JSON;

namespace ZiZZi.Matter.XML.Test
{
    public sealed class XMLBlockArrayTests
    {
        [Fact]
        public void CreatesArray()
        {
            Assert.Equal(
                "<Worlds />",
                new XmlBlockList(
                    new MatterOrigin(new BytesAsXElement()),
                    new XElement("root"),
                    "Worlds"
                ).Content()
                .ToString()
            );
        }

        [Fact]
        public void OpensBlockInArray()
        {
            var matter =
                new XmlBlockList(
                    new MatterOrigin(new BytesAsXElement()),
                    new XElement("root"),
                    "Worlds"
                );

            matter.Open("block", "UpsideDown");

            Assert.Equal(
                "<Worlds><UpsideDown /></Worlds>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}


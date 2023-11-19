using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Tonga.Enumerable;
using Xunit;

namespace ZiZZi.Matter.Dynamic.Tests
{
    public sealed class DynMatterTests
    {
        public static T Expanded<T>(T target, object source)
        {
            foreach(var property in target.GetType().GetProperties())
            {
                if (source is ExpandoObject)
                {
                    foreach (var pair in (source as IDictionary<string, object>))
                    {
                        if(pair.Key == property.Name)
                        {
                            property.SetValue(target, pair.Value);
                        }
                    }
                }
                //var value = property.GetValue(source);
                //property.SetValue(target, value);
            }
            return target;
        }

        [Fact]
        public void OpensBlockAsRoot()
        {
            Assert.False(
                new ZiBlock("Dynamic",
                    new ZiProp("IsStatic", false)
                ).Form<dynamic>(new DynamicMatter())
                .IsStatic
            );
        }

        [Fact]
        public void CanContainMultipleProps()
        {
            Assert.True(
                new ZiBlock("Dynamic",
                    new ZiProp("IsStatic", false),
                    new ZiProp("IsDynamic", true)
                )
                .Form<dynamic>(new DynamicMatter())
                .IsDynamic
            );
        }

        [Fact]
        public void OpensValueListAsRoot()
        {
            Assert.Equal(
                "5",
                new ZiValueList("Dynamic", "MagicNumber",
                    AsEnumerable._("1", "3", "5", "7", "11")
                ).Form<dynamic>(new DynamicMatter())
                [2]
            );
        }

        [Fact]
        public void OpensBlockListAsRoot()
        {
            Assert.Equal(
                "Spinning",
                new ZiBlockList("Dynamic", "Object",
                    new ZiBlock(
                        new ZiProp("PropellerStatus", "Spinning")
                    )
                ).Form<dynamic>(new DynamicMatter())
                [0].PropellerStatus
            );
        }
    }
}


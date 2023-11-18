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
        public class TestObject
        {
            public TestObject() { }
            public string Name { get; set; }
            public string Value { get; set; }
        }
        [Fact]
        public void Test()
        {
            var target = new TestObject();

            Mapper.Map(
                new ZiBlock("KeyValuePair",
                    new ZiProp("Name", "Uber"),
                    new ZiProp("Value", "Object")
                ).Form(new DynamicMatter()),
                target
            );

            var success = target.Value;
        }

        // By using a generic class we can take advantage
        // of the fact that .NET will create a new generic type
        // for each type T. This allows us to avoid creating
        // a dictionary of Dictionary<string, PropertyInfo>
        // for each type T. We also avoid the need for the 
        // lock statement with every call to Map.
        public static class Mapper
        {
            static Mapper()
            {

            }

            public static void Map<T>(object source, T destination)
            {
                
                // At this point we can convert each
                // property name to lower case so we avoid 
                // creating a new string more than once.
                Dictionary<string, PropertyInfo>
                    _propertyMap =
                        typeof(T)
                        .GetProperties()
                        .ToDictionary(
                            p => p.Name.ToLower(),
                            p => p
                        );

                // By iterating the KeyValuePair<string, object> of
                // source we can avoid manually searching the keys of
                // source as we see in your original code.
                foreach (var kv in source as ExpandoObject)
                {
                    PropertyInfo p;
                    if (_propertyMap.TryGetValue(kv.Key.ToLower(), out p))
                    {
                        var propType = p.PropertyType;
                        if (kv.Value == null)
                        {
                            if (!propType.IsByRef && propType.Name != "Nullable`1")
                            {
                                // Throw if type is a value type 
                                // but not Nullable<>
                                throw new ArgumentException("not nullable");
                            }
                        }
                        else if (kv.Value.GetType() != propType)
                        {
                            // You could make this a bit less strict 
                            // but I don't recommend it.
                            throw new ArgumentException("type mismatch");
                        }
                        p.SetValue(destination, kv.Value, null);
                    }
                }
            }
        }

        private static void TestMethod(Object x)
        {
            // This is a dummy value, just to get 'a' to be of the right type
            var a = new { Id = 0, Name = "" };
            a = Cast(a, x);
            Console.Out.WriteLine(a.Id + ": " + a.Name);
        }

        private static T Cast<T>(T typeHolder, Object x)
        {
            // typeHolder above is just for compiler magic
            // to infer the type to cast x to
            return (T)x;
        }

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


//using System.Xml.Linq;
//using Tonga;
//using Tonga.Swap;

//namespace ZiZZi.Matter.Object
//{
//    /// <summary>
//    /// Delivers matter of given contentype  which is rooted in the given parent object.
//    /// </summary>
//    public sealed class MatterOrigin2
//    {
//        private readonly SwapSwitch<string, IMatter<dynamic>> matterOrigin;


//        /// <summary>
//        /// Delivers matter of given contentype  which is rooted in the given dynamic object.
//        /// </summary>
//        public MatterOrigin2(BytesAsTyped bytesAsTyped)
//        {
//            this.matterOrigin =
//                SwapSwitch._<string, IMatter<object>>(
//                    "block",
//                    AsSwap._<string, IMatter<dynamic>>((name) =>
//                        new ContentAware<dynamic>(
//                            new ObjectBlock2<dynamic>(name, bytesAsTyped, this)
//                        )
//                    ),
//                    "bla",
//                    AsSwap._<string, IMatter<dynamic>>((name) =>
//                        new ContentAware<dynamic>(
//                            new ObjectBlock2<dynamic>(name, bytesAsTyped, this)
//                        )
//                    )
//                //,
//                //"value-list",
//                //AsSwap._<object, string, IMatter<object>>((parent, name) =>
//                //    new ListGuard<object>(
//                //        new ContentAware<object>(
//                //            new DynValueList(bytesAsTyped, parent, name)
//                //        ),
//                //        name,
//                //        true
//                //    )
//                //),
//                //"block-list",
//                //AsSwap._<object, string, IMatter<object>>((parent, name) =>
//                //    new ListGuard<object>(
//                //        new ContentAware<object>(
//                //            new DynBlockList(this, parent, name)
//                //        ),
//                //        name,
//                //        false
//                //    )
//                //),
//                //"block-inside-list",
//                //AsSwap._<dynamic, string, IMatter<dynamic>>((parent, name) =>
//                //    new ContentAware<object>(
//                //        new DynBlock(this, bytesAsTyped, parent, name, true)
//                //    )
//                //)
//                );
//        }

//        public IMatter<TResult> Flip<TResult>(TResult typeHolder, string contentType, string name)
//        {
//            return this.matterOrigin.Flip(contentType, name);
//        }
//    }
//}
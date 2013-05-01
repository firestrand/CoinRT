/*
 * Copyright 2011 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.IO;
using CoinRT;
using CoinRT.Store;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CoinSharp.Test.Store
{
    [TestClass]
    public class BoundedOverheadBlockStoreTest
    {
        [TestMethod]
        public void TestStorage()
        {
            var temp = new FileInfo(Path.GetTempFileName());
            try
            {
                Console.WriteLine(temp.FullName);
                var networkParams = NetworkParameters.UnitTests();
                var to = EcKey.Create().ToAddress(networkParams);
                StoredBlock b1;
                using (var store = new BoundedOverheadBlockStore(networkParams, temp))
                {
                    // Check the first block in a new store is the genesis block.
                    var genesis = store.GetChainHead();
                    Assert.AreEqual(networkParams.GenesisBlock, genesis.Header);

                    // Build a new block.
                    b1 = genesis.Build(genesis.Header.CreateNextBlock(to).CloneAsHeader());
                    store.Put(b1);
                    store.SetChainHead(b1);
                }
                // Check we can get it back out again if we rebuild the store object.
                using (var store = new BoundedOverheadBlockStore(networkParams, temp))
                {
                    var b2 = store.Get(b1.Header.Hash);
                    Assert.AreEqual(b1, b2);
                    // Check the chain head was stored correctly also.
                    Assert.AreEqual(b1, store.GetChainHead());
                }
            }
            finally
            {
                temp.Delete();
            }
        }
    }
}
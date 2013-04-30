using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Math;
using ProtoBuf;
using ProtoBuf.Meta;
using Windows.Storage;
using Windows.Storage.Streams;

namespace CoinRT
{
    public class WalletSerializer
    {
        [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
        internal class BigIntegerSurrogate
        {
            public byte[] Value { get; set; }

            public static implicit operator BigIntegerSurrogate(BigInteger i)
            {
                return i == null ? null : new BigIntegerSurrogate { Value = i.ToByteArray() };
            }

            public static implicit operator BigInteger(BigIntegerSurrogate s)
            {
                return s == null || s.Value == null ? null : new BigInteger(s.Value);
            }
        }

        static WalletSerializer()
        {
            RuntimeTypeModel.Default.Add(typeof(BigInteger), false).SetSurrogate(typeof(BigIntegerSurrogate));
        }

        /// <summary>
        /// Uses ProtoBuf serialization to save the wallet to the given file.
        /// </summary>
        /// <exception cref="IOException"/>
        public static async void SaveToFileAsync(Wallet wallet, string path)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                Serializer.Serialize(stream, wallet);
                await stream.FlushAsync();
            }
        }

        /// <summary>
        /// Returns a wallet deserialized from the given file.
        /// </summary>
        /// <exception cref="IOException"/>
        public static async Task<Wallet> LoadFromFileAsync(string path)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(path);
                using (IInputStream inputStream = await file.OpenSequentialReadAsync())
                using (Stream stream = inputStream.AsStreamForRead())
                {
                    return Serializer.Deserialize<Wallet>(stream);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}

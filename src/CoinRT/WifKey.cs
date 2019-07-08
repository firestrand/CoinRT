using System;
using System.Collections.Generic;
using CoinRT.Networks;

namespace CoinRT
{
	/// <summary>
	/// A private key in the Wallet Import Format.
	/// </summary>
	/// <typeparam name="N">A network that the key belongs to.</typeparam>
	public class WifKey<N> : EncodedKey where N :INetwork
	{
		private const string LengthError = "Provided key has a wrong length";
		private const string NetworkMismatch = "Provided key doesn't belong to {0}";

		public WifKey(IEnumerable<byte> privateKey)
			: base(Nets.Get<N>().PrivateKeyPrefix, privateKey)
		{
		}

		public WifKey(string encoded)
			: base(encoded)
		{
			if (this.RawKeyLength != 32) throw new ArgumentException(LengthError);
			if (this.Version != Nets.Get<N>().PrivateKeyPrefix) throw new ArgumentException(NetworkMismatch.With(typeof(N).Name));
		}
	}
}

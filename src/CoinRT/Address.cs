using System;
using System.Collections.Generic;
using CoinRT.Networks;

namespace CoinRT
{
	/// <summary>
	/// An address in Base58 Checked encoding.
	/// </summary>
	/// <typeparam name="N">A network that the address belongs to.</typeparam>
	public class Address<N> : EncodedKey where N : INetwork
	{
		private const string LengthError = "Provided address has wrong size";
		private const string NetworkMismatch = "Provided address doesn't belong to {0}";
	
		public Address(IEnumerable<byte> publicKey)
			: base(Nets.Get<N>().AddressPrefix, publicKey.Sha256().RipeMD160())
		{
		}

		public Address(string encoded)
			: base(encoded)
		{
			if (this.RawKeyLength != 20) throw new ArgumentException(LengthError);
			if (this.Version != Nets.Get<N>().AddressPrefix) throw new ArgumentException(NetworkMismatch.With(typeof(N).Name));
		}
	}
}

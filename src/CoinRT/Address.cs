using System;
using CoinRT.Networks;

namespace CoinRT
{
	public class Address<N> : EncodedKey where N : INetwork
	{
		public Address(byte[] publicKey)
			: base(Nets.Get<N>().AddressPrefix, publicKey.Sha256().RipeMD160())
		{
		}

		public Address(string encoded)
			: base(encoded)
		{
			if (this.Version != Nets.Get<N>().AddressPrefix)
				throw new ArgumentException("Provided address doesn't belong to " + typeof(N).Name);
		}
	}
}

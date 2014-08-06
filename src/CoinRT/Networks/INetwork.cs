using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinRT.Networks
{
	public interface INetwork
	{
		uint Version { get; }

		uint PacketMagic { get; }

		byte AddressPrefix { get; }

		byte PrivateKeyPrefix { get; }
	}
}

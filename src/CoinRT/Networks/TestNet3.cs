using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinRT.Networks
{
	public class TestNet3 : INetwork
	{
		public uint Version
		{
			get { return 60001; }
		}

		public uint PacketMagic
		{
			get { return 0x0b110907; }
		}

		public byte AddressPrefix
		{
			get { return 111; }
		}

		public byte PrivateKeyPrefix
		{
			get { return 239; }
		}
	}
}

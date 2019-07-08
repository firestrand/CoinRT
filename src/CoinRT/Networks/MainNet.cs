namespace CoinRT.Networks
{
	public class MainNet : INetwork
	{
		private static readonly MainNet instance = new MainNet();

		public static MainNet Instance { get { return instance; } }

		private MainNet() { }

		public uint Version
		{
			get { return 60001; }
		}

		public uint PacketMagic
		{
			get { return 0xf9beb4d9; }
		}

		public byte AddressPrefix
		{
			get { return 0; }
		}

		public byte PrivateKeyPrefix
		{
			get { return 128; }
		}
	}
}

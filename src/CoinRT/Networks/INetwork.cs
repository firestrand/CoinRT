
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

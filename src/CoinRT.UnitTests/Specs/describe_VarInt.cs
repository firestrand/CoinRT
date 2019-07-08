using System;
using NSpec;

namespace CoinRT.UnitTests.Specs
{
	class describe_VarInt : nspec
	{
		void it_should_be_little_endian()
		{
			var num = new VarInt(new byte[] { 255, 0, 0, 0, 0, 0, 0, 0, 1 });
			((ulong)num).should_be(Math.Pow(2, 56));
		}

		void it_should_be_a_value_type()
		{
			(new VarInt(1) == new VarInt(1)).should_be_true();
			(new VarInt(1) != new VarInt(1)).should_be_false();
		}

		void it_should_be_comparable()
		{
			(new VarInt(1) < 2).should_be_true();
			(new VarInt(2) < 2).should_be_false();
			(new VarInt(3) < 2).should_be_false();
			(new VarInt(4) > 2).should_be_true();
		}

		void given_byte()
		{
			it["should take 1 byte"] = () => ((byte[])(new VarInt(new byte[] { 252 }))).Length.should_be(1);
		}

		void given_ushort()
		{
			it["should take 3 bytes"] = () => ((byte[])(new VarInt(new byte[] { 253, 0, 1 }))).Length.should_be(3);
		}
		void given_uint()
		{
			it["should take 5 bytes"] = () => ((byte[])(new VarInt(new byte[] { 254, 0, 0, 0, 1 }))).Length.should_be(5);
		}

		void given_ulong()
		{
			it["should take 9 bytes"] = () => ((byte[])(new VarInt(new byte[] { 255, 0, 0, 0, 0, 0, 0, 0, 1 }))).Length.should_be(9);
		}
	}
}

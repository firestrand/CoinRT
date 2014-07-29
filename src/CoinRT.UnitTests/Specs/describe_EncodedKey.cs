using NSpec;

namespace CoinRT.UnitTests.Specs
{
	class describe_EncodedKey : nspec
	{
		EncodedKey key;

		void when_encoding()
		{
			context["hello, version 0"] = () =>
			{
				before = () => key = new EncodedKey(0, new byte[] { 0x68, 0x65, 0x6c, 0x6c, 0x6f });
				it["should become 12L5B5yqsf7vwb"] = () => key.ToString().should_be("12L5B5yqsf7vwb");
			};
		}

		void when_decoding()
		{
			context["12L5B5yqsf7vwb"] = () =>
			{
				before = () => key = new EncodedKey("12L5B5yqsf7vwb");
				it["should become hello"] = () => key.RawKey.should_be(new byte[] { 0x68, 0x65, 0x6c, 0x6c, 0x6f });
				it["should have version 0"] = () => key.Version.should_be(0);
			};
		}

	}
}

using System.Text;
using NSpec;

namespace CoinRT.UnitTests.Specs
{
	class describe_Base58 : nspec
	{
		byte[] raw = null;
		string encoded = null;
		Base58 sut = null;

		void the_value()
		{
			before = () => {
				sut = new Base58("aaaa");
				raw = sut.ToByteArray();
			};
			act = () => raw[0]++;
			it["should be immutable"] = () => raw.should_not_be_same(sut.ToByteArray());
		}

		void when_encoding()
		{
			act = () => encoded = new Base58(raw).ToString();

			context["Hello World"] = () =>
			{
				before = () => raw = Encoding.UTF8.GetBytes("Hello World");
				it["should become JxF12TrwUP45BMd"] = () => encoded.should_be("JxF12TrwUP45BMd");
			};

			context["255"] = () =>
			{
				before = () => raw = new byte[] { 255 };
				it["should become 5Q"] = () => encoded.should_be("5Q");
			};
		}

		void when_decoding()
		{
			string encoded = null;
			byte[] raw = null;

			act = () => raw = new Base58(encoded).ToByteArray();

			context["JxF12TrwUP45BMd"] = () =>
			{
				before = () => encoded = "JxF12TrwUP45BMd";
				it["should become Hello World"] = () => raw.as_string().should_be("Hello World");
			};

			context["1-prefixed string"] = () =>
			{
				before = () => encoded = "111a";
				it["should become 0-prefixed number"] = () => raw.should_be(new byte[] { 0, 0, 0, 33 });
			};

			context["number with extra byte added for sign in BigInteger representation"] = () =>
			{
				before = () => encoded = "5Q";
				it["shouldn't have extra byte in array representation"] = () => raw.should_be(new byte[] { 255 });
			};
		}
	}
}

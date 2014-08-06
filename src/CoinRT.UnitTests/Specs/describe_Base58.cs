using System;
using System.Text;
using NSpec;

namespace CoinRT.UnitTests.Specs
{
	class describe_Base58 : nspec
	{
		byte[] raw = null;
		string encoded = null;
		Base58 sut = null;

		void it_should_be_immutable()
		{
			sut = new Base58("aaaa");
			raw = sut.ToByteArray();
			raw[0]++;

			raw.should_not_be_same(sut.ToByteArray());
		}

		void string_representation()
		{
			it["should not contain zero"] = () => expect<ArgumentException>(() => new Base58("0"));
			it["should not contain one"] = () => expect<ArgumentException>(() => new Base58("1"));
			it["should not contain lowercase l"] = () => expect<ArgumentException>(() => new Base58("l"));
			it["should not contain uppercase I"] = () => expect<ArgumentException>(() => new Base58("I"));
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

			context["0-prefixed number"] = () =>
			{
				before = () => raw = new byte[] { 0, 0, 0, 33 };
				it["should become 1-prefixed string"] = () => encoded.should_be("111a");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSpec;

namespace CoinRT.UnitTests.Specs
{
	class describe_Base58 : nspec
	{
		void when_encoding()
		{
			byte[] input = null;
			string output = null;

			act = () => output = new Base58(input).ToString();

			context["Hello World"] = () =>
			{
				before = () => input = Encoding.UTF8.GetBytes("Hello World");
				it["should become JxF12TrwUP45BMd"] = () => output.should_be("JxF12TrwUP45BMd");
			};

			context["255"] = () =>
			{
				before = () => input = new byte[] { 255 };
				it["should become 5Q"] = () => output.should_be("5Q");
			};


		}

		void when_decoding()
		{
			string input = null;
			byte[] output = null;

			act = () => output = new Base58(input).ToByteArray();

			context["JxF12TrwUP45BMd"] = () =>
			{
				before = () => input = "JxF12TrwUP45BMd";
				it["should become Hello World"] = () => output.as_string().should_be("Hello World");
			};

			context["1-prefixed string"] = () =>
			{
				before = () => input = "111a";
				it["should become 0-prefixed number"] = () => output.should_be(new byte[] { 0, 0, 0, 33 });
			};

			context["number with extra byte added for sign in BigInteger representation"] = () =>
			{
				before = () => input = "5Q";
				it["shouldn't have extra byte in array representation"] = () => output.should_be(new byte[] { 255 });
			};
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinRT.Networks;
using NSpec;

namespace CoinRT.UnitTests.Specs
{
	class describe_WifKey : nspec
	{
		void given_MainNet()
		{
			it["should start with 5"] = () =>
				new WifKey<MainNet>(Enumerable.Repeat<byte>(255, 32)).ToString().should_start_with("5");
			it["should not accept addresses from other network"] = expect<ArgumentException>("Provided key doesn't belong to MainNet",
				() => new WifKey<MainNet>("93KCDD4LdP4BDTNBXrvKUCVES2jo9dAKKvhyWpNEMstuxDauHty"));
			it["should be 32-bytes long"] = expect<ArgumentException>("Provided key has a wrong length",
				() => new WifKey<MainNet>("12L5B5yqsf7vwb"));
		}

		void when_comparing()
		{
			context["operator =="] = () =>
			{
				it["should return true for the same values"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss") == new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss")).should_be_true();
				it["should return false for different values"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss") == new WifKey<MainNet>("5KMWWy2d3Mjc8LojNoj8Lcz9B1aWu8bRofUgGwQk959Dw5h2iyw")).should_be_false();
				it["should return false when comparing with null"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss") == null).should_be_false();
			};

			context["operator !="] = () =>
			{
				it["should return false for the same values"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss") != new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss")).should_be_false();
				it["should return true for different values"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss") != new WifKey<MainNet>("5KMWWy2d3Mjc8LojNoj8Lcz9B1aWu8bRofUgGwQk959Dw5h2iyw")).should_be_true();
				it["should return true when comparing with null"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss") != null).should_be_true();
			};

			context["Equals method"] = () =>
			{
				it["should return true for the same values"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss").Equals(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss"))).should_be_true();
				it["should return false for different values"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss").Equals(new WifKey<MainNet>("5KMWWy2d3Mjc8LojNoj8Lcz9B1aWu8bRofUgGwQk959Dw5h2iyw"))).should_be_false();
				it["should return false when comparing with null"] = () =>
					(new WifKey<MainNet>("5KYZdUEo39z3FPrtuX2QbbwGnNP5zTd7yyr2SC1j299sBCnWjss").Equals(null)).should_be_false();
			};
		}
	}
}

using System;
using System.Linq;
using CoinRT.Networks;
using NSpec;

namespace CoinRT.UnitTests.Specs
{
	class describe_Address : nspec
	{
		void given_MainNet()
		{
			it["should start with 1 or 3"] = () =>
				new Address<MainNet>(Enumerable.Repeat<byte>(255, 20)).ToString().should_start_with("1");
			it["should not accept addresses from other network"] = expect<ArgumentException>("Provided address doesn't belong to MainNet",
				() => new Address<MainNet>("mx5u3nqdPpzvEZ3vfnuUQEyHg3gHd8zrrH"));
		}

		void when_comparing()
		{
			context["operator =="] = () =>
			{
				it["should return true for the same values"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa") == new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa")).should_be_true();
				it["should return false for different values"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa") == new Address<MainNet>("1HXN72vMwhzgVQnX5JzgnExED8Tg3RCtEr")).should_be_false();
				it["should return false when comparing with null"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa") == null).should_be_false();
			};

			context["operator !="] = () =>
			{
				it["should return false for the same values"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa") != new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa")).should_be_false();
				it["should return true for different values"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa") != new Address<MainNet>("1HXN72vMwhzgVQnX5JzgnExED8Tg3RCtEr")).should_be_true();
				it["should return true when comparing with null"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa") != null).should_be_true();
			};

			context["Equals method"] = () =>
			{
				it["should return true for the same values"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa").Equals(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa"))).should_be_true();
				it["should return false for different values"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa").Equals(new Address<MainNet>("1HXN72vMwhzgVQnX5JzgnExED8Tg3RCtEr"))).should_be_false();
				it["should return false when comparing with null"] = () =>
					(new Address<MainNet>("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa").Equals(null)).should_be_false();
			};
		}
	}
}

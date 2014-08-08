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
			it["should start with 1"] = () =>
				new Address<MainNet>(Enumerable.Repeat<byte>(255, 20)).ToString().should_start_with("1");
			it["should not accept addresses from other network"] = expect<ArgumentException>("Provided address doesn't belong to MainNet",
				() => new Address<MainNet>("mx5u3nqdPpzvEZ3vfnuUQEyHg3gHd8zrrH"));
			it["should be 20-bytes long"] = expect<ArgumentException>("Provided address has a wrong length",
				() => new Address<MainNet>("12L5B5yqsf7vwb"));
		}
	}
}

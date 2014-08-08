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
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinRT.Networks;
using NSpec;

namespace CoinRT.UnitTests.Specs
{
	class describe_Address : nspec
	{
		void given_MainNet()
		{
			it["should start with 1 or 3"] = () =>
				new Address<MainNet>(new byte[] { 255 }).ToString().should_start_with("1");
			it["should not accept addresses from other network"] = expect<ArgumentException>("Provided address doesn't belong to MainNet",
				() => new Address<MainNet>("mx5u3nqdPpzvEZ3vfnuUQEyHg3gHd8zrrH"));
		}
	}
}

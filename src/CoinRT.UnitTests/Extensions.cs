using System.Collections.Generic;
using System.Text;

namespace CoinRT.UnitTests
{
	static class Extensions
	{
		public static string as_string(this List<byte> bytes)
		{
			return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
		}
	}
}

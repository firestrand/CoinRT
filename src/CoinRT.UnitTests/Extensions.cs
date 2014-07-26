using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinRT.UnitTests
{
	static class Extensions
	{
		public static string as_string(this byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}
	}
}

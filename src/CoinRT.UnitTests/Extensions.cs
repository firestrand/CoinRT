using System.Text;

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

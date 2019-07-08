namespace CoinRT.Networks
{
	public class Nets
	{
		public static INetwork Get<T>() where T : INetwork
		{
			if (typeof(T) == typeof(MainNet))
			{
				return MainNet.Instance;
			}
			else
			{
				return new TestNet3();
			}
		}
	}
}

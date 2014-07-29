using System.Collections.Generic;
using System.Linq;
using Org.BouncyCastle.Crypto.Digests;

namespace CoinRT
{
	public static class Extensions
	{
		private static Sha256Digest Sha256Digest = new Sha256Digest();
		private static RipeMD160Digest RipeMD160Digest = new RipeMD160Digest();

		public static byte[] Sha256(this IEnumerable<byte> input)
		{
			var block = input.ToArray();
			Sha256Digest.BlockUpdate(block, 0, block.Length);
			var output = new byte[Sha256Digest.GetDigestSize()];
			Sha256Digest.DoFinal(output, 0);
			return output;
		}

		public static byte[] RipeMD160(this IEnumerable<byte> input)
		{
			var block = input.ToArray();
			RipeMD160Digest.BlockUpdate(block, 0, block.Length);
			var output = new byte[RipeMD160Digest.GetDigestSize()];
			RipeMD160Digest.DoFinal(output, 0);
			return output;
		}

		public static IEnumerable<byte> Concat(this byte first, IEnumerable<byte> tail)
		{
			return new byte[] { first }.Concat(tail);
		}
	}
}

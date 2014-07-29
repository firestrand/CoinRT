using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Math;

namespace CoinRT
{
	/// <summary>
	/// Represents a base58-encoded number.
	/// </summary>
	/// <remarks>
	/// From original Satoshi's code:
	/// Why base-58 instead of standard base-64 encoding?
	/// - Don't want 0OIl characters that look the same in some fonts and could be used to create visually identical looking account numbers.
	/// - A string with non-alphanumeric characters is not as easily accepted as an account number.
	/// - E-mail usually won't line-break if there's no punctuation to break at.
	/// - Doubleclicking selects the whole number as one word if it's all alphanumeric.
	/// </remarks>
	public class Base58
	{
		private const string CharSet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz"; // 0OIl removed
		private static readonly BigInteger Base = BigInteger.ValueOf(58);
		private byte[] bytes;
		private readonly BigInteger number;
		private readonly string encoded;

		public Base58(IEnumerable<byte> input)
		{
			this.bytes = input.ToArray();
			this.number = new BigInteger(1, this.bytes);

			var sb = new StringBuilder();
			while (number.CompareTo(BigInteger.Zero) > 0)
			{
				var div = number.DivideAndRemainder(Base);
				sb.Insert(0, CharSet[div[1].IntValue]);
				number = div[0];
			}

			string unpadded = sb.ToString();
			this.encoded = unpadded.PadLeft(unpadded.Length + input.TakeWhile(i => i == 0).Count(), CharSet[0]);
		}

		public Base58(string input)
		{
			this.number = BigInteger.Zero;
			for (var i = 0; i < input.Length; i++)
			{
				var value = CharSet.IndexOf(input[i]);
				if (value == -1) throw new ArgumentException("Illegal character " + input[i] + " at " + i);

				this.number = this.number.Add(BigInteger.ValueOf(value).Multiply(Base.Pow(input.Length - 1 - i)));
			}

			this.bytes = this.number.ToByteArray();
			bool stripSignByte = this.bytes.Length > 1 && this.bytes[0] == 0 && this.bytes[1] >= 0x80;
			int leadingZeros = input.ToCharArray().TakeWhile(ch => ch == CharSet[0]).Count();

			this.bytes = new byte[leadingZeros].Concat(bytes.Skip(stripSignByte ? 1 : 0)).ToArray();
		}

		public override string ToString()
		{
			return this.encoded;
		}

		public byte[] ToByteArray()
		{
			return this.bytes.ToArray();
		}

		public BigInteger ToBigInteger()
		{
			return this.number;
		}

		public override int GetHashCode()
		{
			return this.encoded.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this.encoded.Equals(obj);
		}

		public static bool operator ==(Base58 left, Base58 right)
		{
			return left.encoded == right.encoded;
		}

		public static bool operator !=(Base58 left, Base58 right)
		{
			return left.encoded != right.encoded;
		}
	}
}

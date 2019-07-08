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
		private const string IllegalChar = "Illegal character {0} at {1}";

		private static readonly BigInteger Base = BigInteger.ValueOf(58);
		private readonly byte[] bytes;
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
			this.encoded = input;
			this.number = BigInteger.Zero;
			for (var i = 0; i < input.Length; i++)
			{
				var value = CharSet.IndexOf(input[i]);
				if (value == -1) throw new ArgumentException(IllegalChar.With(input[i], i));

				this.number = this.number.Add(BigInteger.ValueOf(value).Multiply(Base.Pow(input.Length - 1 - i)));
			}

			this.bytes = this.number.ToByteArray();
			bool stripSignByte = this.bytes.Length > 1 && this.bytes[0] == 0 && this.bytes[1] >= 0x80;
			int leadingZeros = input.ToCharArray().TakeWhile(ch => ch == CharSet[0]).Count();

			this.bytes = new byte[leadingZeros].Concat(bytes.Skip(stripSignByte ? 1 : 0)).ToArray();
		}

		public byte this[int index]
		{
			get
			{
				return this.bytes[index];
			}
		}

		public int Length
		{
			get
			{
				return this.bytes.Length;
			}
		}

		public override string ToString()
		{
			return this.encoded;
		}

		public List<byte> ToByteList()
		{
			return this.bytes.ToList();
		}

		public BigInteger ToBigInteger()
		{
			return this.number;
		}

		public override int GetHashCode()
		{
			return this.encoded.GetHashCode();
		}

		public bool Equals(Base58 other)
		{
			if (object.ReferenceEquals(other, null)) return false;

			return this.encoded == other.encoded;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Base58);
		}

		public static bool operator ==(Base58 left, Base58 right)
		{
			return object.ReferenceEquals(left, right) || !object.ReferenceEquals(left, null) && left.Equals(right);
		}

		public static bool operator !=(Base58 left, Base58 right)
		{
			return !(left == right);
		}
	}
}

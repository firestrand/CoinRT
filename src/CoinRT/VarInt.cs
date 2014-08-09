using System;
using System.Collections.Generic;
using System.Linq;

namespace CoinRT
{
	/// <summary>
	/// A variable length integer.
	/// </summary>
	public struct VarInt
	{
		private const byte UInt16Prefix = 253;
		private const byte UInt32Prefix = 254;
		private const byte UInt64Prefix = 255;

		private readonly ulong value;

		public VarInt(ulong value)
		{
			this.value = value;
		}

		public VarInt(IEnumerable<byte> bytes)
		{
			this.value = Parse(bytes);
		}

		public int Size
		{
			get
			{
				if (this.value < 253) return 1;
				if (this.value <= ushort.MaxValue) return 3;
				if (this.value <= uint.MaxValue) return 5;
				return 9;
			}
		}

		public override string ToString()
		{
			return this.value.ToString();
		}

		public static ulong Parse(IEnumerable<byte> bytes)
		{
			var first = bytes.First();
			var tail = bytes.Skip(1);
			switch (first)
			{
				case UInt64Prefix: return BitConverter.ToUInt64(tail.Take(8).ToArray(), 0);
				case UInt32Prefix: return BitConverter.ToUInt32(tail.Take(4).ToArray(), 0);
				case UInt16Prefix: return BitConverter.ToUInt16(tail.Take(2).ToArray(), 0);
				default: return first;
			}
		}

		public static implicit operator ulong(VarInt num)
		{
			return num.value;
		}

		public static explicit operator byte[](VarInt num)
		{
			switch (num.Size)
			{
				case 1: return new[] { (byte)num.value };
				case 3: return UInt16Prefix.Before(BitConverter.GetBytes((ushort)num.value));
				case 5: return UInt32Prefix.Before(BitConverter.GetBytes((uint)num.value));
				default: return UInt64Prefix.Before(BitConverter.GetBytes(num.value));
			}
		}
	}
}

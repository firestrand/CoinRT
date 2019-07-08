using System;
using System.Collections.Generic;
using System.Linq;

namespace CoinRT
{
	/// <summary>
	/// A key in form of [1 byte version] : [n bytes raw key] : [4 bytes checksum].
	/// </summary>
	public class EncodedKey
	{
		private const string ChecksumError = "Checksum is invalid";

		private readonly Base58 data;

		public EncodedKey(byte version, IEnumerable<byte> key)
		{
			var rawData = new List<byte>(25);
			rawData.Add(version);
			rawData.AddRange(key);
			rawData.AddRange(ChecksumOf(rawData));

			this.data = new Base58(rawData);
		}

		public EncodedKey(string encoded)
		{
			this.data = new Base58(encoded);

			if (!this.Checksum.SequenceEqual(ChecksumOf(this.VersionedKey))) throw new ArgumentException(ChecksumError);
		}

		public byte Version
		{
			get
			{
				return this.data[0];
			}
		}

		public byte[] RawKey
		{
			get
			{
				return this.data.ToByteList()
					.GetRange(1, this.RawKeyLength)
					.ToArray();
			}
		}

		public int RawKeyLength
		{
			get
			{
				return this.data.Length - 5;
			}
		}

		public byte[] VersionedKey
		{
			get
			{
				return this.data.ToByteList()
					.GetRange(0, this.data.Length - 4)
					.ToArray();
			}
		}

		public byte[] Checksum
		{
			get
			{

				return this.data.ToByteList()
					.GetRange(this.data.Length - 4, 4)
					.ToArray();
			}
		}

		public override string ToString()
		{
			return this.data.ToString();
		}

		public override int GetHashCode()
		{
			return this.data.GetHashCode();
		}

		public bool Equals(EncodedKey other)
		{
			if (object.ReferenceEquals(other, null)) return false;

			return this.data == other.data;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as EncodedKey);
		}

		public static bool operator ==(EncodedKey left, EncodedKey right)
		{
			return object.ReferenceEquals(left, right) || !object.ReferenceEquals(left, null) && left.Equals(right);
		}

		public static bool operator !=(EncodedKey left, EncodedKey right)
		{
			return !(left == right);
		}

		private static IEnumerable<byte> ChecksumOf(IEnumerable<byte> value)
		{
			return value.Sha256().Sha256().Take(4);
		}
	}
}

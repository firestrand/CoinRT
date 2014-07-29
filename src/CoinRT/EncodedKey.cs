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
		private readonly Base58 data;

		public EncodedKey(byte version, byte[] key)
		{
			var rawData = new List<byte>(20);
			rawData.Add(version);
			rawData.AddRange(key);
			rawData.AddRange(ChecksumOf(rawData));

			this.data = new Base58(rawData);
		}

		public EncodedKey(string encoded)
		{
			this.data = new Base58(encoded);

			if (!this.Checksum.SequenceEqual(ChecksumOf(this.Version.Concat(this.RawKey))))
			{
				throw new ArgumentException("Checksum is invalid");
			}
		}

		public byte Version
		{
			get
			{
				return this.data.ToByteArray()[0];
			}
		}

		public byte[] RawKey
		{
			get
			{
				var data = this.data.ToByteArray();
				return data.Skip(1).Take(data.Length - 5).ToArray();
			}
		}

		public byte[] Checksum
		{
			get
			{
				var data = this.data.ToByteArray();
				return data.Skip(data.Length - 4).ToArray();
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

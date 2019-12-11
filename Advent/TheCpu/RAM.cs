using System;

using Word = System.Numerics.BigInteger;

namespace TheCpu
{
	public class RAM
	{
		private Word[] memory;

		public int Size => memory.Length;

		public int MaxSize { get; }

		public Word this[Word address]
		{
			get => Peek(address);
			set => Poke(address, value);
		}

		public RAM(int size, int maxSize)
		{
			memory = new Word[size];
			MaxSize = maxSize;
		}

		public void Load(Word[] data)
		{
			if (IsBadAddress(data.Length - 1))
			{
				throw new AddressException(data.Length - 1, MaxSize);
			}

			Array.Copy(data, memory, data.Length);
		}

		private Word Peek(Word address)
		{
			if (IsBadAddress(address))
			{
				throw new AddressException(address, MaxSize);
			}

			return memory[(int)address];
		}

		private void Poke(Word address, Word value)
		{
			if (IsBadAddress(address))
			{
				throw new AddressException(address, MaxSize);
			}

			memory[(int)address] = value;
		}

		private bool IsBadAddress(Word address)
		{
			if (address < 0) return true;
			if (address >= MaxSize) return true;

			if (address >= Size)
			{
				var temp = new Word[(int)address + 1];
				Array.Copy(memory, temp, memory.Length);
				memory = temp;				
			}

			return false;
		}
	}

	class AddressException : Exception
	{
		public Word Address { get; }
		public int RAMSize { get; }

		public AddressException(Word address, int ramSize)
		{
			Address = address;
			RAMSize = ramSize;
		}
	}
}

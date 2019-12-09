using System;

using Word = System.Numerics.BigInteger;

namespace TheCpu
{
	class RAM
	{
		private Word[] ram;

		public int Size => ram.Length;

		public Word this[Word address]
		{
			get => Peek(address);
			set => Poke(address, value);
		}

		public RAM(int size)
		{
			ram = new Word[size];
		}

		public void Load(Word[] data)
		{
			IsBadAddress(data.Length + 1);
			Array.Copy(data, ram, data.Length);
		}

		private Word Peek(Word address)
		{
			if (IsBadAddress(address))
			{
				throw new AddressException(address, Size);
			}

			return ram[(int)address];
		}

		private void Poke(Word address, Word value)
		{
			if (IsBadAddress(address))
			{
				throw new AddressException(address, Size);
			}

			ram[(int)address] = value;
		}

		private bool IsBadAddress(Word address)
		{
			if (address < 0) return true;

			if (address >= Size)
			{
				var temp = new Word[(int)address + 1];
				Array.Copy(ram, temp, ram.Length);
				ram = temp;				
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

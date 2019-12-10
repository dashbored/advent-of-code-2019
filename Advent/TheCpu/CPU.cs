using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Word = System.Numerics.BigInteger;

namespace TheCpu
{
	enum ReadMode
	{
		Address = 0,
		Immediate = 1,
		Relative = 2
	};

	class CPU
	{
		public RAM RAM { get; private set; }
		public int IC { get; private set; }
		public int ArgFlags { get; private set; }
		public int PC { get; private set; }
		public bool Halt { get; internal set; }
		public int RBO { get; internal set; }
        public Word OUT { get; internal set; }
        public Stack<Word> IN { get; internal set; }

		public CPU(int ramSize = 4096)
		{
			RAM = new RAM(ramSize);
			IN = new Stack<Word>();
		}

		public void Load(params Word[] program)
		{
			RAM.Load(program);
			PC = 0;
			Halt = false;
		}

		public void Load(string programPath)
		{
			var program = System.IO.File
				.ReadAllText(programPath)
				.Split(',')
				.Select(Word.Parse)
				.ToArray();

			Load(program);
		}

		public void Run()
		{
			while (Halt == false)
			{
				var instruction = Decode();
				Execute(instruction);
			}
		}

		private Instruction Decode()
		{
			Word word = Peek(PC);
			ArgFlags = (int)(word / 100);

			return InstructionSet.Get((int)word);
		}

		private void Execute(Instruction instruction)
		{
			Print(instruction);

			int newPC = PC + 1 + instruction.ArgumentCount;
			instruction.Execute(this, ref newPC);
			PC = newPC;
			IC++;
		}

		private void Print(Instruction instruction)
		{
			string pc = PC.ToString().PadLeft(4, '0');
			string code = ToString(instruction);
			Console.WriteLine($"{pc} :: {code}");
		}

		private string ToString(Instruction instruction)
		{
			string GetArg(int index)
			{
				Word value = Peek(PC + 1 + index);
				return GetFlag(index) switch
				{
					ReadMode.Immediate => $"{value}",
					ReadMode.Address => $"*{value}",
					ReadMode.Relative => $"[{value}]",
					_ => throw new Exception("Invalid read mode"),
				};
			}

			var args = Enumerable.Range(0, instruction.ArgumentCount).Select(GetArg);
			return $"{instruction.Name} {string.Join(" ", args)}".TrimEnd();
		}

		internal ReadMode GetFlag(int argIndex)
		{
			int mask = (int)Math.Pow(10, argIndex);
			int flag = (ArgFlags / mask) % 10;
			return (ReadMode)flag;
		}

		internal Word ReadArg(int argIndex)
		{
			Word address = PC + 1 + argIndex;
			var mode = GetFlag(argIndex);
			return Read(address, mode);
		}

		internal void WriteArg(int argIndex, Word value)
		{
			Word argAddress = PC + 1 + argIndex;
			Word writeAddress = Read(argAddress);
			Write(writeAddress, value);
		}

		internal Word Read(Word address, ReadMode mode = ReadMode.Immediate)
		{
			Word value = Peek(address);
			//Console.WriteLine($"   RAM {address} -> {value}");
			return mode switch
			{
				ReadMode.Immediate => value,
				ReadMode.Address => Read(value),
				ReadMode.Relative => Read(RBO + value),
				_ => throw new Exception("Invalid read mode"),
			};
		}

		internal void Write(Word address, Word value)
		{
			//Console.WriteLine($"   RAM {address} <- {value}");
			RAM[address] = value;
		}

		private Word Peek(Word address)
		{
			return RAM[address];
		}

		internal void ReadArgs(out Word arg1)
		{
			arg1 = ReadArg(0);
		}

		internal void ReadArgs(out Word arg1, out Word arg2)
		{
			ReadArgs(out arg1);
			arg2 = ReadArg(1);
		}

		internal void ReadArgs(out Word arg1, out Word arg2, out Word arg3)
		{
			ReadArgs(out arg1, out arg2);
			arg3 = ReadArg(2);
		}
	}
}

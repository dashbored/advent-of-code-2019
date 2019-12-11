using System;
using System.Linq;

using Word = System.Numerics.BigInteger;

namespace TheCpu
{
	enum ParameterMode
	{
		Position = 0,
		Immediate = 1,
		Relative = 2
	};

	public class CPU
	{
		const int KiB = 1024;
		const int MiB = KiB * KiB;

		private int ArgFlags { get; set; }
		private RAM RAM { get; }
		public IOPort IO { get; }
		
		public bool PrintInstructions { get; set; }
		public bool PrintRamAccess { get; set; }
		
		public int IC { get; private set; }
		public int PC { get; private set; }
		public Word RelativeBase { get; internal set; }
		public bool Halt { get; internal set; }

		public CPU(IOPort io = null)
		{
			RAM = new RAM(KiB, MiB);
			IO = io ?? new IOPort();
		}

		public void Load(params Word[] program)
		{
			RAM.Load(program);
			PC = 0;
			IC = 0;
			RelativeBase = 0;
			Halt = false;
			ArgFlags = 0;
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
			int word = (int)Peek(PC);
			ArgFlags = (word / 100);

			return InstructionSet.Get(word);
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
			if (PrintInstructions)
			{
				string pc = PC.ToString().PadLeft(4, '0');
				string code = ToString(instruction);
				Console.WriteLine($"{pc} :: {code}");
			}
		}

		private void Print(string ramAccessMessage)
		{
			if (PrintRamAccess)
			{
				Console.WriteLine(ramAccessMessage);
			}
		}

		private string ToString(Instruction instruction)
		{
			string ArgToString(int index)
			{
				Word value = Peek(PC + 1 + index);

				return GetArgMode(index) switch
				{
					ParameterMode.Immediate => $"{value}",
					ParameterMode.Position => $"*{value}",
					ParameterMode.Relative => $"[{value}]",
					_ => throw new Exception("Invalid read mode"),
				};
			}

			var args = Enumerable.Range(0, instruction.ArgumentCount).Select(ArgToString);
			return $"{instruction.Name} {string.Join(" ", args)}".TrimEnd();
		}

		private ParameterMode GetArgMode(int argIndex)
		{
			int mask = (int)Math.Pow(10, argIndex);
			int flag = (ArgFlags / mask) % 10;
			return (ParameterMode)flag;
		}

		internal Word ReadArg(int argIndex)
		{
			GetArg(argIndex, out var argValue, out var mode);

			return mode switch
			{
				ParameterMode.Immediate => argValue,
				_ => Read(argValue + GetOffset(mode)),
			};
		}

		internal void WriteArg(int argIndex, Word value)
		{
			GetArg(argIndex, out var argValue, out var mode);

			Word writeAddress = argValue + GetOffset(mode);
			Write(writeAddress, value);
		}

		private void GetArg(int argIndex, out Word argValue, out ParameterMode mode)
		{
			Word argAddress = PC + 1 + argIndex;
			argValue = Read(argAddress);
			mode = GetArgMode(argIndex);
		}

		private Word GetOffset(ParameterMode mode)
		{
			return mode == ParameterMode.Relative ? RelativeBase : 0;
		}

		internal Word Read(Word address)
		{
			Word value = Peek(address);
			Print($"   RAM {address} -> {value}");
			return value;
		}

		internal void Write(Word address, Word value)
		{
			Print($"   RAM {address} <- {value}");
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
	}
}

using System;
using System.Collections.Generic;

using Word = System.Numerics.BigInteger;

namespace TheCpu
{
	delegate void InstructionExecutor(CPU cpu, ref int newPC);

	class DecodeException : Exception
	{
		public Word Word { get; }
		public int OpCode { get; }

		public DecodeException(Word word, int opCode)
		{
			Word = word;
			OpCode = opCode;
		}
	}

	static class InstructionSet
	{
		private static readonly Dictionary<int, Instruction> instructions = new Dictionary<int, Instruction>();

		public static Instruction Get(int word)
		{
			int opCode = word % 100;

			if (instructions.ContainsKey(opCode))
			{
				return instructions[opCode];
			}
			else
			{
				throw new DecodeException(word, opCode);
			}
		}

		static void Register(int opCode, int argumentCount, InstructionExecutor execute, string name)
		{
			instructions[opCode] = new Instruction(opCode, argumentCount, execute, name);
		}

		static InstructionSet()
		{
			// Arithmetic
			Register(1, 3, Add, "ADD");
			Register(2, 3, Mul, "MUL");

			// I/O
			Register(3, 1, In, "IN");
			Register(4, 1, Out, "OUT");

			// Conditionals			
			Register(5, 2, JumpIfTrue, "JNZ");
			Register(6, 2, JumpIfFalse, "JZ");
			Register(7, 3, TestLessThan, "TLT");
			Register(8, 3, TestEqual, "TEQ");

			// RBO
			Register(9, 1, SetRelativeBase, "RBO");

			// Halt
			Register(99, 0, Halt, "HLT");
		}

		static void Halt(CPU cpu, ref int newPC)
		{
			cpu.Halt = true;
			newPC = cpu.PC;
		}

		static void Add(CPU cpu, ref int newPC)
		{
			cpu.ReadArgs(out var lhs, out var rhs);
			cpu.WriteArg(2, lhs + rhs);
		}

		static void Mul(CPU cpu, ref int newPC)
		{
			cpu.ReadArgs(out var lhs, out var rhs);
			cpu.WriteArg(2, lhs * rhs);
		}

		static void In(CPU cpu, ref int newPC)
		{
			Console.Write("?>");

			cpu.IN.Push(int.Parse(Console.ReadLine()));

			Word input = cpu.IN.Pop();
			cpu.WriteArg(0, input);
		}

		static void Out(CPU cpu, ref int newPC)
		{
			Word output = cpu.ReadArg(0);
			cpu.OUT = output;
			Console.WriteLine($":>{output}");
		}

		static void JumpIfTrue(CPU cpu, ref int newPC)
		{
			Word input = cpu.ReadArg(0);

			if (input != 0)
			{
				newPC = (int)cpu.ReadArg(1);
			}
		}

		static void JumpIfFalse(CPU cpu, ref int newPC)
		{
			Word input = cpu.ReadArg(0);

			if (input == 0)
			{
				newPC = (int)cpu.ReadArg(1);
			}
		}

		static void TestLessThan(CPU cpu, ref int newPC)
		{
			cpu.ReadArgs(out var lhs, out var rhs);

			Word value = lhs < rhs ? 1 : 0;
			cpu.WriteArg(2, value);

		}

		static void TestEqual(CPU cpu, ref int newPC)
		{
			cpu.ReadArgs(out var lhs, out var rhs);

			Word value = lhs == rhs ? 1 : 0;
			cpu.WriteArg(2, value);
		}

		static void SetRelativeBase(CPU cpu, ref int newPC)
		{
			cpu.ReadArgs(out var offset);
			cpu.RBO += (int)offset;
		}
	}
}

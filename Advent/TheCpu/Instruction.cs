namespace TheCpu
{
	struct Instruction
	{
		public int OpCode { get; }
		public int ArgumentCount { get; }
		public InstructionExecutor Execute { get; }
		public string Name { get; }

		public Instruction(int opCode, int argumentCount, InstructionExecutor execute, string name)
		{
			OpCode = opCode;
			ArgumentCount = argumentCount;
			Execute = execute;
			Name = name;
		}
	}
}

using System;
using System.Collections.Concurrent;
using System.Linq;

using Word = System.Numerics.BigInteger;

namespace TheCpu
{
	public class IOPort
	{
		public virtual Word Read()
		{
			Console.Write("?>");
			return Word.Parse(Console.ReadLine());
		}

		public virtual void Write(Word word)
		{			
			Console.WriteLine($":>{word}");
		}
	}

	public class Bus
	{
		private readonly BlockingCollection<Word>[] ports;

		public Bus(int portCount)
		{
			ports = Enumerable
				.Range(0, portCount)
				.Select(_ => new BlockingCollection<Word>())
				.ToArray();
		}

		public Word Read(int port)
		{
			var word = ports[port].Take();
			//Console.WriteLine($"  READ-{port} >> {word}");
			return word;
		}

		public void Write(int port, Word word)
		{
			//Console.WriteLine($"  WRITE-{port} << {word}");
			ports[port].Add(word);
		}
	}

	public class IOBus : IOPort
	{
		private readonly Bus bus;
		private readonly int inPort;
		private readonly int outPort;

		public IOBus(Bus bus, int inPort, int outPort)
		{
			this.bus = bus;
			this.inPort = inPort;
			this.outPort = outPort;
		}

		public override Word Read()
		{
			return bus.Read(inPort);
		}

		public override void Write(Word word)
		{
			bus.Write(outPort, word);
		}
	}

	public class IORoboPort : IOPort
	{


		public override Word Read()
		{
			return base.Read();
		}

		public override void Write(Word word)
		{
			base.Write(word);
		}
	}
}

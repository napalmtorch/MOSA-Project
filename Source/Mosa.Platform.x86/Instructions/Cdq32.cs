// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Cdq32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class Cdq32 : X86Instruction
	{
		public override int ID { get { return 205; } }

		internal Cdq32()
			: base(2, 1)
		{
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 2);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.OpcodeEncoder.AppendByte(0x99);
		}
	}
}

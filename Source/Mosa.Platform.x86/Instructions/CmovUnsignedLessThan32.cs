// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// CmovUnsignedLessThan32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class CmovUnsignedLessThan32 : X86Instruction
	{
		public override string AlternativeName { get { return "CmovB32"; } }

		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0x0F, 0x42 } );

		internal CmovUnsignedLessThan32()
			: base(1, 1)
		{
		}

		public override BaseInstruction GetOpposite()
		{
			return X86.CmovUnsignedGreaterOrEqual32;
		}

		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.Emit(LegacyOpcode, node.Result, node.Operand1);
		}
	}
}


// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Rcr32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class Rcr32 : X86Instruction
	{
		public static readonly LegacyOpCode LegacyOpcode = new LegacyOpCode(new byte[] { 0xD3 } , 0x03);

		internal Rcr32()
			: base(1, 2)
		{
		}

		public override bool ThreeTwoAddressConversion { get { return true; } }

		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 2);
			System.Diagnostics.Debug.Assert(node.Result.IsCPURegister);
			System.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);
			System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

			emitter.Emit(LegacyOpcode, node.Result);
		}
	}
}


// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ESP32.Instructions
{
	/// <summary>
	/// Addmi - Add Immediate with Shift by 8, ADDMI extends the range of constant addition. It is often used in conjunction with load and store instructions to extend the range of the base, plus offset the calculation. ADDMI calculates the two’s complement 32-bit sum of address register as and an operand encoded in the imm8 field. The low 32 bits of the sum are written to address register at. Arithmetic overflow is not detected. The operand encoded in the instruction can have values that are multiples of 256 ranging from -32768 to 32512. It is decoded by sign-extending imm8 and shifting the result left by eight bits.
	/// </summary>
	/// <seealso cref="Mosa.Platform.ESP32.ESP32Instruction" />
	public sealed class Addmi : ESP32Instruction
	{
		public override int ID { get { return 697; } }

		internal Addmi()
			: base(1, 3)
		{
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 1);
			System.Diagnostics.Debug.Assert(node.OperandCount == 3);

			emitter.OpcodeEncoder.Append8BitImmediate(node.Operand2);
			emitter.OpcodeEncoder.AppendNibble(0b1101);
			emitter.OpcodeEncoder.AppendNibble(node.Operand1.Register.RegisterCode);
			emitter.OpcodeEncoder.AppendNibble(node.Operand2.Register.RegisterCode);
			emitter.OpcodeEncoder.AppendNibble(0b0010);
		}
	}
}
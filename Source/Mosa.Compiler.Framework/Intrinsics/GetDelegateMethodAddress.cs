﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Runtime.Intrinsic:GetDelegateMethodAddress")]
		private static void GetDelegateMethodAddress(Context context, MethodCompiler methodCompiler)
		{
			var load = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.LoadInt32 : IRInstruction.LoadInt64;

			context.SetInstruction(load, context.Result, context.Operand1, methodCompiler.CreateConstant(2 * methodCompiler.Architecture.NativePointerSize));
		}
	}
}

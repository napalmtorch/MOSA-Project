﻿/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Debugger
	/// </summary>
	public static class Debugger
	{

		#region Codes

		public static class Codes
		{
			public const int Connecting = 10;
			public const int Connected = 11;
			public const int Disconnected = 12;

			public const int UnknownData = 99;

			public const int Alive = 1000;
			public const int Ping = 1001;
			public const int InformationalMessage = 1002;
			public const int WarningMessage = 1003;
			public const int ErrorMessage = 1004;
			public const int SendNumber = 1005;
			public const int ReadPhysicalMemory = 1010;
			public const int WritePhysicalMemory = 1011;
		}

		#endregion // Codes

		private static ushort com = Serial.COM1;

		private static uint _buffer = 0x1412000;
		private static uint _index = 0;
		private static int _length = -1;

		public static void Setup(ushort com)
		{
			Serial.SetupPort(com);
			Debugger.com = com;
		}

		private static void SendByte(int i)
		{
			Serial.Write(com, (byte)i);
		}

		private static void SendInteger(int i)
		{
			SendByte(i >> 24 & 0xFF);
			SendByte(i >> 16 & 0xFF);
			SendByte(i >> 8 & 0xFF);
			SendByte(i & 0xFF);
		}

		private static void SendMagic()
		{
			SendByte('M');
			SendByte('O');
			SendByte('S');
			SendByte('A');
		}

		// MAGIC-ID-CODE-LEN-CHECKSUM-DATA

		public static void SendResponse(int id, int code)
		{
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(0);
			SendInteger(0); // TODO: not implemented
		}

		public static void SendResponse(int id, int code, int data)
		{
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(4);
			SendInteger(0); // TODO: not implemented
			SendInteger(data);
		}

		public static void SendResponse(int id, int code, int data, int data2)
		{
			SendMagic();
			SendInteger(id);
			SendInteger(code);
			SendInteger(8);
			SendInteger(0); // TODO: not implemented
			SendInteger(data);
			SendInteger(data2);
		}

		public static void SendAlive()
		{
			SendResponse(0, Codes.Alive);
		}

		public static void SendNumber(int i)
		{
			SendResponse(0, Codes.SendNumber, i);
		}

		private static void BadDataAbort()
		{
			_index = 0;
			_length = -1;
		}

		private static int GetInteger(uint index)
		{
			return (Native.Get8(_buffer + index) << 24) | (Native.Get8(_buffer + index + 1) << 16) | (Native.Get8(_buffer + index + 2) << 8) | Native.Get8(_buffer + index + 3);
		}

		public static void GetCommand()
		{
			if (Serial.IsDataReady(com))
			{
				byte b = Serial.Read(com);

				Native.Set8(_buffer + _index, b);
				_index++;

				//SendNumber((int)_index);

				if (_index == 1 && Native.Get8(_buffer) != (byte)'M')
					BadDataAbort();
				else if (_index == 2 && Native.Get8(_buffer + 1) != (byte)'O')
					BadDataAbort();
				else if (_index == 3 && Native.Get8(_buffer + 2) != (byte)'S')
					BadDataAbort();
				else if (_index == 4 && Native.Get8(_buffer + 3) != (byte)'A')
					BadDataAbort();

				if (_index >= 16 && _length == -1)
				{
					_length = (int)GetInteger(12);
				}

				if (_length > 4096 || _index > 4096)
				{
					BadDataAbort();
					return;
				}

				if (_length + 20 == _index)
				{
					ProcessCommand();

					BadDataAbort();
				}

			}
		}

		private static void ProcessCommand()
		{
			int id = GetInteger(4);
			int code = GetInteger(8);
			int len = GetInteger(12);
			int checksum = GetInteger(16);
			uint datastart = _buffer + 20;
			uint firstdata = Native.Get32(datastart);

			switch (code)
			{
				case Codes.Ping: SendResponse(id, Codes.Ping); return;
				case Codes.ReadPhysicalMemory: SendResponse(id, Codes.ReadPhysicalMemory, (int)datastart, (int)firstdata); return;
				default: return;
			}

		}

	}
}
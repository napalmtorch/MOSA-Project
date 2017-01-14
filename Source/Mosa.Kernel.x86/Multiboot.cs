﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86.Helpers;
using Mosa.Runtime;
using Mosa.Runtime.x86;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Static class of helpful memory functions
	/// </summary>
	unsafe public static class Multiboot
	{
		/// <summary>
		/// Magic value that indicates that kernel was loaded by a Multiboot-compliant boot loader
		/// </summary>
		public const uint MultibootMagic = 0x2BADB002;

		private static MultiBootInfo* multiBootInfo = null;
		private static uint memoryMapCount = 0;

		public static uint MultibootAddress = 0x0;

		/// <summary>
		/// Gets the memory map count.
		/// </summary>
		/// <value>The memory map count.</value>
		public static uint MemoryMapCount => memoryMapCount;

		/// <summary>
		/// Gets a value indicating whether this instance is multiboot enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is multiboot enabled; otherwise, <c>false</c>.
		/// </value>
		public static bool IsMultibootEnabled => (multiBootInfo != null);

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public static uint Flags => multiBootInfo->Flags;

		/// <summary>
		/// Gets the memory lower.
		/// </summary>
		/// <value>The lower memory.</value>
		public static uint MemoryLower => multiBootInfo->MemLower;

		/// <summary>
		/// Gets the memory upper.
		/// </summary>
		/// <value>The memory upper.</value>
		public static uint MemoryUpper => multiBootInfo->MemUpper;

		/// <summary>
		/// Gets the boot device.
		/// </summary>
		/// <value>The boot device.</value>
		public static uint BootDevice => multiBootInfo->BootDevice;

		/// <summary>
		/// Gets the CMD line address.
		/// </summary>
		/// <value>The CMD line address.</value>
		public static uint CmdLineAddress => multiBootInfo->CommandLine;

		/// <summary>
		/// Gets the modules start.
		/// </summary>
		/// <value>The modules start.</value>
		public static uint ModulesStart => multiBootInfo->ModuleAddress;

		/// <summary>
		/// Gets the modules count.
		/// </summary>
		/// <value>The modules count.</value>
		public static uint ModulesCount => multiBootInfo->ModuleCount;

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <value>The length of the memory map.</value>
		public static uint MemoryMapLength => multiBootInfo->MemMapLength;

		/// <summary>
		/// Gets the memory map start.
		/// </summary>
		/// <value>The memory map start.</value>
		public static uint MemoryMapStart => multiBootInfo->MemMapAddress;

		/// <summary>
		/// Gets the length of the drive.
		/// </summary>
		/// <value>The length of the drive.</value>
		public static uint DriveLength => multiBootInfo->DrivesLength;

		/// <summary>
		/// Gets the drive start.
		/// </summary>
		/// <value>The drive start.</value>
		public static uint DriveStart => multiBootInfo->DrivesAddress;

		/// <summary>
		/// Gets the configuration table.
		/// </summary>
		/// <value>The configuration table.</value>
		public static uint ConfigurationTable => multiBootInfo->ConfigTable;

		/// <summary>
		/// Gets the name of the boot loader.
		/// </summary>
		/// <value>The name of the boot loader.</value>
		public static uint BootLoaderName => multiBootInfo->BootLoaderName;

		/// <summary>
		/// Gets the APM table.
		/// </summary>
		/// <value>The APM table.</value>
		public static uint APMTable => multiBootInfo->ApmTable;

		/// <summary>
		/// Gets the VBE control information.
		/// </summary>
		/// <value>The VBE control information.</value>
		public static uint VBEControlInformation => multiBootInfo->VbeControlInfo;

		/// <summary>
		/// Gets the VBE mode info.
		/// </summary>
		/// <value>The VBE mode info.</value>
		public static uint VBEModeInfo => multiBootInfo->VbeModeInfo;

		/// <summary>
		/// Gets the VBE mode.
		/// </summary>
		/// <value>The VBE mode.</value>
		public static uint VBEMode => multiBootInfo->VbeMode;

		/// <summary>
		/// Gets the VBE interface seg.
		/// </summary>
		/// <value>The VBE interface seg.</value>
		public static uint VBEInterfaceSeg => multiBootInfo->VbeInterfaceSeg;

		/// <summary>
		/// Gets the VBE interface off.
		/// </summary>
		/// <value>The VBE interface off.</value>
		public static uint VBEInterfaceOff => multiBootInfo->VbeInterfaceOff;

		/// <summary>
		/// Gets the VBE interface len.
		/// </summary>
		/// <value>The VBE interface len.</value>
		public static uint VBEInterfaceLen => multiBootInfo->VbeInterfaceLength;

		/// <summary>
		/// Setups this multiboot.
		/// </summary>
		public static void Setup()
		{
			uint magic = Native.GetMultibootEAX();
			uint address = Native.GetMultibootEBX();

			SetMultibootLocation(address, magic);
		}

		/// <summary>
		/// Sets the multiboot location, if given the proper magic value
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="magic">The magic value.</param>
		public static void SetMultibootLocation(uint address, uint magic)
		{
			if (magic == MultibootMagic)
			{
				SetMultibootLocation(address);
			}
		}

		/// <summary>
		/// Sets the multiboot location.
		/// </summary>
		/// <param name="address">The address.</param>
		public static void SetMultibootLocation(uint address)
		{
			MultibootAddress = address;
			multiBootInfo = (MultiBootInfo*)address;

			CountMemoryMap();
		}

		/// <summary>
		/// Counts the memory map.
		/// </summary>
		private static void CountMemoryMap()
		{
			memoryMapCount = 0;
			MultiBootMemoryMap* location = (MultiBootMemoryMap*)MemoryMapStart;

			while ((uint)location < (MemoryMapStart + MemoryMapLength))
			{
				memoryMapCount++;

				//location = (MultiBootMemoryMap*)(((uint)location) + location->size + 4);
				location = location->Next;
			}
		}

		/// <summary>
		/// Gets the memory map index location.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private static MultiBootMemoryMap* GetMemoryMapIndexLocation(uint index)
		{
			MultiBootMemoryMap* location = (MultiBootMemoryMap*)MemoryMapStart;

			for (uint i = 0; i < index; i++)
			{
				location = location->Next;
			}
			return location;
		}

		/// <summary>
		/// Gets the memory map base.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static uint GetMemoryMapBase(uint index)
		{
			return (uint)GetMemoryMapIndexLocation(index)->BaseAddr;
		}

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static uint GetMemoryMapLength(uint index)
		{
			return (uint)GetMemoryMapIndexLocation(index)->Length;
		}

		/// <summary>
		/// Gets the type of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static byte GetMemoryMapType(uint index)
		{
			return (byte)GetMemoryMapIndexLocation(index)->Type;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe public struct MultiBootInfo
	{
		public uint Flags;              //required
		public uint MemLower;           //if bit 0 in flags are set
		public uint MemUpper;           //if bit 0 in flags are set
		public uint BootDevice;       //if bit 1 in flags are set
		public uint CommandLine;        //if bit 2 in flags are set
		public uint ModuleCount;        //if bit 3 in flags are set
		public uint ModuleAddress;  //if bit 3 in flags are set
		public uint Syms1; //if bits 4 or 5 in flags are set
		public uint Syms2; //if bits 4 or 5 in flags are set
		public uint Syms3; //if bits 4 or 5 in flags are set
		public uint Syms4; //if bits 4 or 5 in flags are set
		public uint MemMapLength;       //if bit 6 in flags is set
		public uint MemMapAddress;  //if bit 6 in flags is set
		public uint DrivesLength;       //if bit 7 in flags is set
		public uint DrivesAddress;  //if bit 7 in flags is set
		public uint ConfigTable;        //if bit 8 in flags is set
		public uint BootLoaderName;
		public uint ApmTable;               //if bit 9 in flags is set
		public uint VbeControlInfo; //if bit 10 in flags is set
		public uint VbeModeInfo;        //if bit 11 in flags is set
		public uint VbeMode;                // all vbe_* set if bit 12 in flags are set
		public uint VbeInterfaceSeg;
		public uint VbeInterfaceOff;
		public uint VbeInterfaceLength;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MultiBootMemoryMap
	{
		public uint Size;
		public ulong BaseAddr;
		public ulong Length;
		public uint Type;

		unsafe public MultiBootMemoryMap* Next
		{
			get
			{
				fixed (MultiBootMemoryMap* ptr = &this)
				{
					return (MultiBootMemoryMap*)(((uint)ptr) + Size + 4);
				}
			}
		}
	}
}

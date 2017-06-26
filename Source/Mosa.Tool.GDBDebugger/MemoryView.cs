﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger
{
	public partial class MemoryView : DebugDockContent
	{
		//private uint multibootStructure = Multiboot.MultibootStructure;
		//private uint physicalPageFreeList = MosaKernel.PageTable;
		//private uint cr3 = MosaKernel.SmallTables;

		private int Rows = 0;
		private int Columns = 16;
		private int Bytes = 0;
		private ulong Address = 0;

		public MemoryView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			tbMemory.Text = "0xB8000";
			Query();
		}

		public override void OnRunning()
		{
			lbMemory.Clear();
		}

		public override void OnPause()
		{
			Query();
		}

		private ulong ParseMemoryAddress()
		{
			string nbr = tbMemory.Text.ToUpper().Trim();
			int digits = 10;
			int where = nbr.IndexOf('X');

			if (where >= 0)
			{
				digits = 16;
				nbr = nbr.Substring(where + 1);
			}

			var value = Convert.ToUInt64(nbr, digits);

			return value;
		}

		private void UpdateDisplay(byte[] bytes)
		{
			ulong at = Address;

			var newlines = new string[Rows];

			for (int line = 0; line < Rows; line++)
			{
				string l = at.ToString("X").PadLeft(8, '0') + ':';
				string d = string.Empty;

				for (int x = 0; x < Columns; x++)
				{
					int index = (line * Columns) + x;

					if (index >= bytes.Length)
						break;

					byte mem = bytes[index];

					if (x % 4 == 0) l = l + ' ';
					l = l + mem.ToString("X").PadLeft(2, '0');
					char b = (char)mem;
					d = d + (char.IsLetterOrDigit(b) ? b : '.');
					at++;
				}

				newlines[line] = l + ' ' + d;
			}

			lbMemory.Lines = newlines;
		}

		private void Query()
		{
			Columns = (lbMemory.Width - 100) / ((int)lbMemory.Font.Size * 3);
			Rows = lbMemory.Height / (lbMemory.Font.Height + 2);

			Address = ParseMemoryAddress();
			Bytes = Rows * Columns;

			if (Bytes > 0x800)
			{
				Rows = 0x800 / Columns;
				Bytes = Rows * Columns;
			}

			GDBConnector.ReadMemory(Address, Bytes, OnMemoryRead);
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			Query();
		}

		private void OnMemoryRead(byte[] bytes)
		{
			if (bytes.Length != Bytes)
				return;

			MethodInvoker method = delegate ()
			{
				UpdateDisplay(bytes);
			};

			BeginInvoke(method);
		}
	}
}

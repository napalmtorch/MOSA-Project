﻿/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.Permutation
{
	public class I8
	{
		private static IList<long> samples = null;
		public static IList<long> SampleData { get { if (samples == null) samples = GetSamples(); return samples; } }

		public static IEnumerable<long> Samples
		{
			get
			{
				foreach (long value in SampleData)
					yield return value;
			}
		}

		public static IList<long> GetSamples()
		{
			List<long> list = new List<long>();

			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(sbyte.MinValue);
			list.Add(sbyte.MaxValue);
			list.Add(sbyte.MinValue + 1);
			list.Add(sbyte.MaxValue - 1);
			list.Add(byte.MaxValue);
			list.Add(byte.MinValue);
			list.Add(short.MinValue);
			list.Add(short.MaxValue);
			list.Add(short.MinValue + 1);
			list.Add(short.MaxValue - 1);
			list.Add(int.MinValue);
			list.Add(int.MaxValue);
			list.Add(int.MinValue + 1);
			list.Add(int.MaxValue - 1);
			list.Add(long.MinValue);
			list.Add(long.MaxValue);
			list.Add(long.MinValue + 1);
			list.Add(long.MaxValue - 1);

			// Get negatives
			list.AddIfNew<long>(GetNegatives(list));

			list.Sort();

			return list;
		}

		protected static IList<long> GetNegatives(IList<long> list)
		{
			List<long> negs = new List<long>();

			foreach (long value in list)
			{
				if (value > 0)
					negs.AddIfNew<long>((long)-value);
			}

			return negs;
		}
	}
}

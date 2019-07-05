using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using System.Collections.Generic;
#pragma warning disable 0618

namespace UUCSL.Benchmarks
{
	public sealed class MinimalColumnProvider : IColumnProvider
	{
		public IEnumerable<IColumn> GetColumns(Summary summary)
		{
			yield return TargetMethodColumn.Method;
			foreach(var column in JobCharacteristicColumn.AllColumns)
			{
				yield return column;
			}
			yield return StatisticColumn.Mean;
		}
	}
}

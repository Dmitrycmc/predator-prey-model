using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
	internal static class Utils
	{
		private static double initStep = 0.01;

		internal static double BinSearch(Func<double, double> f, double a, double c, double eps)
		{
			double b = (a + c) / 2;
			if (Math.Abs(c - a) < eps) return b;
			if (f(b) > 0)
				return BinSearch(f, b, c, eps);
			else
				return BinSearch(f, a, b, eps);
		}

		internal static double LogSearch(Func<double, double> f, double eps)
		{
			double step = initStep;
			while (f(step) > 0)
			{
				step *= 2;
			}
			return BinSearch(f, step / 2, step, eps);
			/// [..|..|.....|xxxxxxxxxxx|
		}

		internal static double Search(Func<double, double> f, double eps)
		{
			if (f(0) > 0) return LogSearch(f, eps);
			return -LogSearch(x => -f(-x), eps);
		}
	}
}

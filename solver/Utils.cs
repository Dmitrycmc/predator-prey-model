using System;

namespace Solver
{
	internal static class Utils
	{
		private static double initStep = 0.01;

		internal static double BinSearch(Func<double, double> f, double a, double c, double eps)
		{
			double b = (a + c) / 2;
			if (Math.Abs(c - a) > eps)
			{
				return BinSearch(f, b, f(a) * f(b) < 0 ? a : c, eps);
			} else
			{
				return b;
			}
		}

		internal static double LogSearch(Func<double, double> f, double eps)
		{
			double x = initStep;
			while (f(0) * f(x) > 0)
			{
				x *= 2;
			}
			return BinSearch(f, x / 2, x, eps);
			/// [..|..|.....|xxxxxxxxxxx|
		}
	}
}

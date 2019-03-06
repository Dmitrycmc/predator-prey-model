﻿using System;
using System.Diagnostics;

namespace Solver
{
	internal static class Utils
	{
		private const double initStep = 1;

		internal static double BinSearch(Func<double, double> f, double a, double c, double eps)
		{
			double b = (a + c) / 2;
			if (Math.Abs(c - a) > eps)
			{
				return BinSearch(f, b, f(a) * f(b) <= 0 ? a : c, eps);
			} else
			{
				return b;
			}
		}

		internal static double LogSearch(Func<double, double> f, double eps, double x0 = 0)
		{
			double step = initStep;
			double fa = f(x0);
			double fc = f(x0 + step);

			while (double.IsNaN(fc) || (fa * fc > 0))
			{
				if (double.IsNaN(fc))
				{
					step /= 2;
				} else
				{
					return LogSearch(f, eps, step);
				}
				fc = f(x0 + step);
			};
			return BinSearch(f, x0, x0 + step, eps);
		}
	}
}

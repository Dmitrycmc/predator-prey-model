using System;
using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Math;

namespace Randomizer
{
    static public class Generator
	{
		static public double[] getRandomParams(double minValue = 0.5, double maxValue = 5)
		{
			double alpha = Rand.UniformBetween(minValue, maxValue);
			double beta = Rand.UniformBetween(minValue, maxValue);
			double gamma = Rand.UniformBetween(minValue, maxValue);
			double delta = Rand.UniformBetween(minValue, maxValue);
			double x0 = gamma/delta * (1 - 0.5 * Rand.UniformBetween(-1, 1));
			double y0 = alpha/beta * (1 - 0.5 * Rand.UniformBetween(-1, 1));
			
			return new double[] { alpha, beta, gamma, delta, x0, y0 };
		}


		static private List<int> GetRandomCombination(int from, int to)
		{
			if (from < to)
			{
				to = from;
			}
			List<int> res = new List<int>();
			int t;
			for (int i = 0; i < to; i++)
			{
				do
				{
					t = Rand.Int(from);
				} while (res.Contains(t));
				res.Add(t);
			}
			res.Sort();
			return res;
		}

		static private List<double[]> ChooseCombinationFromList(List<double[]> list, List<int> combination)
		{
			List<double[]> res = new List<double[]>();
			foreach (var i in combination)
			{
				res.Add(list[i]);
			}
			return res;
		}

		static private List<double[]> getRandomElementsFromList(List<double[]> list, int n)
		{
			return ChooseCombinationFromList(list, GetRandomCombination(list.Count, n));
		}

		static public List<double[]> getMeasurements(List<double[]> exactSol, double stdDev, int n)
		{
			List<double[]> res = new List<double[]>();
			var randomMeasurements = getRandomElementsFromList(exactSol, n);

			foreach (var point in randomMeasurements)
			{
				double dx = Rand.Normal(0, stdDev);
				double dy = Rand.Normal(0, stdDev);
				double[] newPoint = (double[])point.Clone();
				newPoint[0] += dx;
				newPoint[1] += dy;
				res.Add(newPoint);
			}
			return res;
		}
    }
}

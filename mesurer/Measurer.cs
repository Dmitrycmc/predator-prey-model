using System;
using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Math;

namespace Measurer
{
    static public class Noise
	{

		static private List<int> GetRandomCombination(int from, int to)
		{
			if (from < to)
			{
				//throw new Exception("List < n");
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

		static public List<double[]> getMeasurements(List<double[]> accurateMeasurements, double stdDev, int n)
		{
			List<double[]> res = new List<double[]>();
			var randomMeasurements = getRandomElementsFromList(accurateMeasurements, n);

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

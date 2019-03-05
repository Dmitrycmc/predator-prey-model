using System;
using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Math;

namespace Measurer
{
    static public class Noise
	{
		static public List<double[]> getMeasurements(List<double[]> accurateMeasurements, double variance, int n)
		{
			// todo: implement Gauss

			List<double[]> res = new List<double[]>();
			foreach (var point in accurateMeasurements)
			{
				double dx = Rand.Normal(0, variance);
				double dy = Rand.Normal(0, variance);
				res.Add(new double[] { point[0] + dx, point[1] + dy });
			}
			return res;
		}
    }
}

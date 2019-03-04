using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurer
{
    static public class Measurer
	{
		// todo: improve seed
		static Random rand = new Random(new DateTime().Millisecond);

		static public List<double[]> getMeasurements(List<double[]> accurateMeasurements, double variance, int n)
		{
			// todo: implement Gauss

			List<double[]> res = new List<double[]>();
			foreach (var point in accurateMeasurements)
			{
				double dx = variance * rand.Next(-10, 10) / 10;
				double dy = variance * rand.Next(-10, 10) / 10;
				res.Add(new double[] { point[0] + dx, point[1] + dy });
			}
			return res;
		}
    }
}

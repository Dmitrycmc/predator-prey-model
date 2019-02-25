using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Research.Oslo;

namespace Solver
{
	public class SDEPP
	{
		public readonly double alpha, beta, gamma, delta, x0, y0;

		private double[,] solution = null;
		public double[,] getSolution
		{
			get {
				if (solution == null) new Exception("not solved");
				return (double[,])solution.Clone();
			}
		}


		public SDEPP(double alpha, double beta, double gamma, double delta, double x0, double y0)
		{
			this.alpha = alpha;
			this.beta = beta;
			this.gamma = gamma;
			this.delta = delta;
			this.x0 = x0;
			this.y0 = y0;
		}

		private double calcC(double x, double y)
		{
			return x + y - Math.Log(x) - Math.Log(y);
		}

		public void Projection()
		{
			double x = x0;
			double y = y0;

			solution = new double[181, 3];
			solution[0, 1] = x;
			solution[0, 2] = y;

			double C = calcC(x, y);
			double k = 0.1;
			for (int i = 0; i < 180; i++)
			{
				var grad = new Tuple<double, double>(1 - 1 / x, 1 - 1 / y);
				var n = new Tuple<double, double>(-grad.Item2, grad.Item1);
				var p = new Tuple<double, double>(x + k * n.Item1, y + k * n.Item2);
				var anti = new Tuple<double, double>(1 / p.Item1 - 1, 1 / p.Item2 - 1);
				Func<double, double> f = t => p.Item1 + t * anti.Item1 + p.Item2 + t * anti.Item2 - Math.Log(p.Item1 + t * anti.Item1) - Math.Log(p.Item2 + t * anti.Item2) - C;
				for (int j = 0; j < 1000000; j++)
				{
					if (f(j * 0.001) < 0)
					{
						x = p.Item1 + j * 0.001 * anti.Item1;
						y = p.Item2 + j * 0.001 * anti.Item2;
						break;
					}
				}

				solution[i + 1, 0] = calcC(x, y);
				solution[i + 1, 1] = x;
				solution[i + 1, 2] = y;
			}
		}


		public void OSLO()
		{
			var sol = Ode.RK547M(0, new Vector(x0, y0), (t, x) => new Vector(
					x[0] - x[0] * x[1],
					-x[1] + x[0] * x[1]
				)
			);
			var points = sol.SolveFromToStep(0, 9, 0.05).ToArray();
			int len = points.Length;
			int i = 0;
			solution = new double[len, 3];
			foreach (var sp in points)
			{
				solution[i, 0] = sp.T;
				solution[i, 1] = sp.X[0];
				solution[i, 2] = sp.X[1];
				i++;
			}
		}
	}
}

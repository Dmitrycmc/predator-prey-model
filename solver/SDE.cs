using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Research.Oslo;

namespace Solver
{
	public class Model
	{
		public readonly double alpha, beta, gamma, delta, x0, y0;

		private List<double[]> solution = null;
		public List<double[]> getSolution
		{
			get {
				if (solution == null) new Exception("not solved");
				return solution;
			}
		}


		public Model(double alpha, double beta, double gamma, double delta, double x0, double y0)
		{
			this.alpha = alpha;
			this.beta = beta;
			this.gamma = gamma;
			this.delta = delta;
			this.x0 = x0;
			this.y0 = y0;
		}

		private double CalcC(double x, double y)
		{
			return delta * x + beta * y - gamma * Math.Log(x) - alpha * Math.Log(y);
		}

		public void Projection(double step)
		{
			double x = x0;
			double y = y0;

			solution = new List<double[]>();
			solution.Add(new double[] { x, y });

			double C = CalcC(x, y);

			for (int i = 0; i < 100; i++)
			{
				var grad = new Tuple<double, double>(delta - gamma / x, beta - alpha / y);
				var n = new Tuple<double, double>(-grad.Item2, grad.Item1);
				var p = new Tuple<double, double>(x + step * n.Item1, y + step * n.Item2);
				var anti = new Tuple<double, double>(gamma / p.Item1 - delta, alpha / p.Item2 - beta);
				Func<double, double> f = t => delta * (p.Item1 + t * anti.Item1) + beta * (p.Item2 + t * anti.Item2) - gamma * Math.Log(p.Item1 + t * anti.Item1) - alpha * Math.Log(p.Item2 + t * anti.Item2) - C;
				for (int j = 0; j < 1000000; j++)
				{
					if (f(j * 0.001) < 0)
					{
						x = p.Item1 + j * 0.001 * anti.Item1;
						y = p.Item2 + j * 0.001 * anti.Item2;
						break;
					}
				}

				solution.Add(new double[] { x, y });
			}
		}


		public void OSLO(double step)
		{
		    double T = 2 * Math.PI / Math.Sqrt(alpha * gamma);
			T *= 1.5;

			var sol = Ode.RK547M(0, new Vector(x0, y0), (t, x) => new Vector(
					(alpha - beta * x[1]) * x[0],
					(-gamma + delta * x[0]) * x[1]
				)
			);
			var points = sol.SolveFromToStep(0, T, step).ToArray();

			solution = new List<double[]>();
			foreach (var sp in points)
			{
				solution.Add(new double[] { sp.X[0], sp.X[1], sp.T });
			}
		}
	}
}

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
				if (solution == null) Solve();
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

		private void Solve()
		{
			var sol = Ode.RK547M(0, new Vector(5.0, 1.0), (t, x) => new Vector(
					x[0] - x[0] * x[1],
					-x[1] + x[0] * x[1]
				)
			);
			var points = sol.SolveFromToStep(0, 200, 0.1).ToArray();
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

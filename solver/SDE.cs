using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
	public class SDE
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


		public SDE(double alpha, double beta, double gamma, double delta, double x0, double y0)
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
			solution = new double[,] { { 1, 2 }, { 3, 4 } };
		}
	}
}

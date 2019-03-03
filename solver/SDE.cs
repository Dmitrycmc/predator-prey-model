using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Research.Oslo;

namespace Solver
{
	public class Model
	{
		public readonly double alpha, beta, gamma, delta, x0, y0, xE, yE;

		private List<double[]> solution = null;
		public List<double[]> getSolution
		{
			get {
				if (solution == null) throw new Exception("Not solved");
				return solution;
			}
		}
		
		private double[] GetEquilibriumPoint()
		{
			return new double[2] { gamma / delta, alpha / beta };
		}

		public Model(double alpha, double beta, double gamma, double delta, double x0, double y0)
		{
			this.alpha = alpha;
			this.beta = beta;
			this.gamma = gamma;
			this.delta = delta;
			this.x0 = x0;
			this.y0 = y0;

			double[] equilibriumPoint = GetEquilibriumPoint();
			xE = equilibriumPoint[0];
			yE = equilibriumPoint[1];
		}

		private double CalcC(double x, double y)
		{
			return delta * x + beta * y - gamma * Math.Log(x) - alpha * Math.Log(y);
		}

		/*
		public void Projection(double step)
		{
			double x = x0;
			double y = y0;
			
			int N = 5;

			solution = new List<double[]>();
			solution.Add(new double[] { x, y });

			double C = CalcC(x, y);
			Debug.WriteLine("C!!! = " + C.ToString());

			for (int i = 0; i < N; i++)
			{
				var grad = new Tuple<double, double>(delta - gamma / x, beta - alpha / y);

				solution.Add(new double[] { x+grad.Item1, y+grad.Item2 });
				var n = new Tuple<double, double>(-grad.Item2, grad.Item1);
				var p = new Tuple<double, double>(x + step * n.Item1, y + step * n.Item2);
				var anti = new Tuple<double, double>(gamma / p.Item1 - delta, alpha / p.Item2 - beta);
				double f(double z) => 
					delta * (p.Item1 + z * anti.Item1) 
					+ beta * (p.Item2 + z * anti.Item2) 
					- gamma * Math.Log(p.Item1 + z * anti.Item1) 
					- alpha * Math.Log(p.Item2 + z * anti.Item2) 
					- C;
				Debug.WriteLine("0 = " + f(0).ToString());
				Debug.WriteLine("-5 = " + f(-5).ToString());
				double z0 = Utils.Search(f, 0.01);

				x = p.Item1 + z0 * anti.Item1;
				y = p.Item2 + z0 * anti.Item2;
				Debug.WriteLine("CP = " + CalcC(x, y).ToString());
				if (CalcC(x, y) < 6.1) x = solution[50][50];
				solution.Add(new double[] { x, y });
			}
		}
		*/

		public void Rays(double step)
		{
			solution = new List<double[]>();
			double eps = 0.001;
			double C = CalcC(x0, y0);
			Func<double, double, double>
				X = (phi, t) => xE + t * Math.Cos(phi),
				Y = (phi, t) => yE + t * Math.Sin(phi),
			    F = (phi, t) => CalcC(X(phi, t), Y(phi, t)) - C;
			
			for (double phi = 0; phi < 2 * Math.PI; phi += step)
			{
				double t0 = Utils.LogSearch(t => F(phi, t), eps);
				double x = X(phi, t0);
				double y = Y(phi, t0);
				solution.Add(new double[] { x, y });
			}
			solution.Add(solution[0]);
		}

		public void OSLO(double step)
		{
		    double T = 2 * Math.PI / Math.Sqrt(alpha * gamma);
			T *= 15;

			var sol = Ode.RK547M(0, new Vector(x0, y0), (t, x) => new Vector(
					(alpha - beta * x[1]) * x[0],
					(-gamma + delta * x[0]) * x[1]
				)
			);
			var points = sol.SolveFromToStep(0, T, step).ToArray();

			solution = new List<double[]>();
			foreach (var sp in points)
			{
				//Debug.WriteLine("CO = " + CalcC(sp.X[0], sp.X[1]).ToString());
				solution.Add(new double[] { sp.X[0], sp.X[1], sp.T });
			}
		}

		public double GetAverageSquaredError()
		{
			double C0 = CalcC(x0, y0);
			double sum = 0;
			foreach (var p in solution)
			{
				double x = p[0];
				double y = p[1];
				double C = CalcC(x, y);
				sum += Math.Pow(C - C0, 2);
			}
			sum /= solution.Count;
			return Math.Sqrt(sum);
		}
	}
}

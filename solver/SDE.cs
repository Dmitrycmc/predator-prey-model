﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Research.Oslo;

namespace Solver
{
	public class SDE
	{
		public readonly double alpha, beta, gamma, delta, x0, y0, xE, yE, C;
		private bool byInitPoint;

		private List<double[]> solution = null;
		public List<double[]> getSolution
		{
			get {
				if (solution == null) throw new Exception("Not solved");
				return solution;
			}
		}
		
		public double[] GetEquilibriumPoint()
		{
			return new double[2] { xE, yE };
		}

		public SDE(double alpha, double beta, double gamma, double delta, double x0, double y0)
		{
			this.alpha = alpha;
			this.beta = beta;
			this.gamma = gamma;
			this.delta = delta;
			this.x0 = x0;
			this.y0 = y0;
			
			byInitPoint = true;

			xE = gamma / delta;
			yE = alpha / beta;
		}

		public SDE(double alpha, double beta, double gamma, double delta, double C)
		{
			this.alpha = alpha;
			this.beta = beta;
			this.gamma = gamma;
			this.delta = delta;
			this.C = C;

			byInitPoint = false;

			xE = gamma / delta;
			yE = alpha / beta;
		}

		private double CalcC(double x, double y)
		{
			return delta * x + beta * y - gamma * Math.Log(x) - alpha * Math.Log(y);
		}
		
		public void Rays(double step)
		{
			solution = new List<double[]>();
			double eps = 0.001;
			double C = byInitPoint ? CalcC(x0, y0) : this.C;
			double t0;
			Func<double, double, double>
				X = (phi, t) => xE + t * Math.Cos(phi),
				Y = (phi, t) => yE + t * Math.Sin(phi),
			    F = (phi, t) => CalcC(X(phi, t), Y(phi, t)) - C;
			
			for (double phi = 0; phi < 2 * Math.PI; phi += step / t0)
			{
				try
				{
					t0 = Utils.LogSearch(t => F(phi, t), eps);
				} catch (Exception)
				{
					throw new Exception("Error in logSearch");
				}
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
			double[] p0 = new double[2] { points[0].X[0], points[0].X[1] };
			double[] p1 = new double[2] { points[1].X[0], points[1].X[1] };
			double d0 = Utils.Distance(p0, p1);

			solution.Add(new double[] { points[0].X[0], points[0].X[1], points[0].T });
			solution.Add(new double[] { points[1].X[0], points[1].X[1], points[1].T });

			for (int i = 2; i < points.Count(); i++)
			{
				double d = Utils.Distance(p0, new double[2] { points[i].X[0], points[i].X[1] });
				if (d > d0)
				{
					solution.Add(new double[] { points[i].X[0], points[i].X[1], points[i].T });
				} else
				{
					solution.Add(new double[] { points[0].X[0], points[0].X[1], points[0].T });
					break;
				}
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

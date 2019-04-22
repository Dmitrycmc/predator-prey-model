using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor
{
	static public class Model
	{

		static public double[] numericalMethodInfer(List<double[]> points, double strictAlpha = 0, bool visualize = false)
		{
			Variable<double> alpha = Variable.GaussianFromMeanAndVariance(5, 2).Named("alpha");
			Variable<double> beta = Variable.GaussianFromMeanAndVariance(5, 2).Named("beta");
			Variable<double> gamma = Variable.GaussianFromMeanAndVariance(5, 2).Named("gamma");
			Variable<double> delta = Variable.GaussianFromMeanAndVariance(5, 2).Named("delta");
			
			Range dataRange = new Range(points.Count);
			VariableArray<double> dxdt = Variable.Array<double>(dataRange);
			VariableArray<double> dydt = Variable.Array<double>(dataRange);
			VariableArray<double> x = Variable.Array<double>(dataRange);
			VariableArray<double> y = Variable.Array<double>(dataRange);
			
			using (Variable.ForEach(dataRange))
			{
				dxdt[dataRange] = (alpha - beta * y[dataRange]) * x[dataRange];
				dydt[dataRange] = (-gamma + delta * x[dataRange]) * y[dataRange];
			}
			
			double[] xObserved = new double[points.Count];
			double[] yObserved = new double[points.Count];

			double[] dxdtObserved = new double[points.Count];
			double[] dydtObserved = new double[points.Count];
			
			for (int i = 0; i < points.Count; i++)
			{
				xObserved[i] = points[i][0];
				yObserved[i] = points[i][1];
			}
			
			dxdtObserved[0] = (xObserved[1] - xObserved[0]) / (points[1][2] - points[0][2]);
			dydtObserved[0] = (yObserved[1] - yObserved[0]) / (points[1][2] - points[0][2]);
			
			dxdtObserved[points.Count - 1] = (xObserved[points.Count - 1] - xObserved[points.Count - 2]) / (points[points.Count - 1][2] - points[points.Count - 2][2]);
			dydtObserved[points.Count - 1] = (yObserved[points.Count - 1] - yObserved[points.Count - 2]) / (points[points.Count - 1][2] - points[points.Count - 2][2]);
			
			for (int i = 1; i < points.Count - 1; i++)
			{
				dxdtObserved[i] = (xObserved[i + 1] - xObserved[i - 1]) / (points[i + 1][2] - points[i - 1][2]);
				dydtObserved[i] = (yObserved[i + 1] - yObserved[i - 1]) / (points[i + 1][2] - points[i - 1][2]);	
			}
			
			x.ObservedValue = xObserved;
			y.ObservedValue = yObserved;
			dxdt.ObservedValue = dxdtObserved;
			dydt.ObservedValue = dydtObserved;
			Debug.WriteLine(1);
			InferenceEngine engine = new InferenceEngine(new ExpectationPropagation());

			/*
			if (visualize)
			{
				InferenceEngine.Visualizer = new Microsoft.ML.Probabilistic.Compiler.Visualizers.WindowsVisualizer();
				engine.ShowFactorGraph = true;
			}*/

			Debug.WriteLine(2);
			string alphaString = engine.Infer(alpha).ToString();
			string betaString = engine.Infer(beta).ToString();
			string gammaString = engine.Infer(gamma).ToString();
			string deltaString = engine.Infer(delta).ToString();

			Debug.WriteLine(3);
			string ans = "alpha: " + alphaString + Environment.NewLine;
			ans += "beta: " + betaString + Environment.NewLine;
			ans += "gamma: " + gammaString + Environment.NewLine;
			ans += "delta: " + deltaString + Environment.NewLine;

			Debug.WriteLine(4);
			return new double[] {
				Utils.parseBetween(alphaString, '(', ' '),
				Utils.parseBetween(betaString, '(', ' '),
				Utils.parseBetween(gammaString, '(', ' '),
				Utils.parseBetween(deltaString, '(', ' ')
			};
			
		}
		

		static public double[] FirstIntegralInfer(List<double[]> points, double strictAlpha = 0, bool visualize = false)
		{
			Variable<double> alpha = Variable.GaussianFromMeanAndVariance(5, 2).Named("alpha");
			Variable<double> beta = Variable.GaussianFromMeanAndVariance(5, 2).Named("beta");
			Variable<double> gamma = Variable.GaussianFromMeanAndVariance(5, 2).Named("gamma");
			Variable<double> delta = Variable.GaussianFromMeanAndVariance(5, 2).Named("delta");
			Variable<double> C = Variable.GaussianFromMeanAndVariance(5, 5).Named("C");
			Variable<double> sum = 0;

			if (strictAlpha != 0)
			{
				alpha.ObservedValue = strictAlpha;
			}

			Range dataRange = new Range(points.Count);
			VariableArray<double> x = Variable.Array<double>(dataRange);
			VariableArray<double> y = Variable.Array<double>(dataRange);
			
			using (Variable.ForEach(dataRange))
			{
				sum +=
					(delta * x[dataRange]
					+ beta * y[dataRange]
					- gamma * Variable.Log(x[dataRange])
					- alpha * Variable.Log(y[dataRange])
					- C) / points.Count;
			}

			sum.ObservedValue = 0;
			double[] xObserved = new double[points.Count];
			double[] yObserved = new double[points.Count];

			for (int i = 0; i < points.Count; i++)
			{
				xObserved[i] = points[i][0];
				yObserved[i] = points[i][1];
			}

			x.ObservedValue = xObserved;
			y.ObservedValue = yObserved;

			InferenceEngine engine = new InferenceEngine(new ExpectationPropagation());
			/*
			if (visualize)
			{
				InferenceEngine.Visualizer = new Microsoft.ML.Probabilistic.Compiler.Visualizers.WindowsVisualizer();
				engine.ShowFactorGraph = true;
			}*/
			

			string cString = engine.Infer(C).ToString();
			string alphaString = engine.Infer(alpha).ToString();
			string betaString = engine.Infer(beta).ToString();
			string gammaString = engine.Infer(gamma).ToString();
			string deltaString = engine.Infer(delta).ToString();

			string ans = "Noise: " + cString + Environment.NewLine;
			ans += "alpha: " + alphaString + Environment.NewLine;
			ans += "beta: " + betaString + Environment.NewLine;
			ans += "gamma: " + gammaString + Environment.NewLine;
			ans += "delta: " + deltaString + Environment.NewLine;
			
			return new double[] {
				strictAlpha != 0 ? strictAlpha : Utils.parseBetween(alphaString, '(', ' '),
				Utils.parseBetween(betaString, '(', ' '),
				Utils.parseBetween(gammaString, '(', ' '),
				Utils.parseBetween(deltaString, '(', ' '),
				Utils.parseBetween(cString, '(', ' '),
			};
		}
	}
}

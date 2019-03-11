using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor
{
	static public class Model
	{
		static public double[] FirstIntegralInfer(List<double[]> points, double strictAlpha = 0, bool visualize = false)
		{
			Variable<double> alpha = Variable.GaussianFromMeanAndVariance(5, 25/9).Named("alpha");
			Variable<double> beta = Variable.GaussianFromMeanAndVariance(5, 25 / 9).Named("beta");
			Variable<double> gamma = Variable.GaussianFromMeanAndVariance(5, 25 / 9).Named("gamma");
			Variable<double> delta = Variable.GaussianFromMeanAndVariance(5, 25 / 9).Named("delta");
			Variable<double> C = Variable.GaussianFromMeanAndVariance(5, 10).Named("C");
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
					delta * x[dataRange]
					+ beta * y[dataRange]
					- gamma * Variable.Log(x[dataRange])
					- alpha * Variable.Log(y[dataRange])
					- C;
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
			
			/*
			this.var = (double)1 / parseBetween(precisionString, '=', ']');
			this.a = parseBetween(aString, '(', ' ');
			this.b = parseBetween(bString, '(', ' ');

			for (int i = 0; i < sample.Length; i++)
			{
				double value = this.a * i + this.b;
				inferredLine[i] = value;
			}
			*/

		}
	}
}

﻿using Microsoft.ML.Probabilistic.Algorithms;
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
		static private string trimEnd(string str)
		{
			while (!char.IsNumber(str[str.Length - 1])) str = str.Substring(0, str.Length - 1);
			return str;
		}

		static private double parseBetween(string str, char c1, char c2)
		{
			int firstSign = str.IndexOf(c1) + 1;
			int lastSign = str.IndexOf(c2);
			string substr = str.Substring(firstSign, lastSign - firstSign);
			substr = trimEnd(substr);
			double res = double.Parse(substr);
			return res;
		}

		static public string FirstIntegralInfer(List<double[]> points, bool visualize = false)
		{
			Variable<double> alpha = Variable.GaussianFromMeanAndVariance(4, 1).Named("alpha");
			Variable<double> beta = Variable.GaussianFromMeanAndVariance(4, 1).Named("beta");
			Variable<double> gamma = Variable.GaussianFromMeanAndVariance(4, 1).Named("gamma");
			Variable<double> delta = Variable.GaussianFromMeanAndVariance(4, 1).Named("delta");
			Variable<double> noise = Variable.GaussianFromMeanAndVariance(0, 0.0025).Named("noise");

			Range dataRange = new Range(points.Count);
			VariableArray<double> C = Variable.Array<double>(dataRange);
			VariableArray<double> x = Variable.Array<double>(dataRange);
			VariableArray<double> y = Variable.Array<double>(dataRange);

			using (Variable.ForEach(dataRange))
			{
				C[dataRange] =
					delta * x[dataRange]
					+ beta * y[dataRange]
					- gamma * Variable.Log(x[dataRange])
					- alpha * Variable.Log(y[dataRange])
					;//+ noise;
			}

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
			}
			*/

			string noiseString = "-";//engine.Infer(noise).ToString();
			string alphaString = engine.Infer(alpha).ToString();
			string betaString = engine.Infer(beta).ToString();
			string gammaString = engine.Infer(gamma).ToString();
			string deltaString = engine.Infer(delta).ToString();

			string ans = "Noise: " + noiseString + Environment.NewLine;
			ans += "alpha: " + alphaString + Environment.NewLine;
			ans += "beta: " + betaString + Environment.NewLine;
			ans += "gamma: " + gammaString + Environment.NewLine;
			ans += "delta: " + deltaString + Environment.NewLine;
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
			return ans;
			
		}
	}
}
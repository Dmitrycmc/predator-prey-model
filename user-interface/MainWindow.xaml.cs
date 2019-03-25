using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Wpf.CartesianChart.PointShapeLine;
using Solver;
using Randomizer;
using Predictor;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace user_interface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const double dt = 0.01;
		SDE sde0, sde1;
		List<double[]> measurements;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Generate(object sender, RoutedEventArgs e)
		{
			double[] rand = Generator.getRandomParams();
			textBoxAlpha.Text = rand[0].ToString();
			textBoxBeta.Text = rand[1].ToString();
			textBoxGamma.Text = rand[2].ToString();
			textBoxDelta.Text = rand[3].ToString();
			textBoxX0.Text = rand[4].ToString();
			textBoxY0.Text = rand[5].ToString();
		}

		private void GetMeasurements(object sender, RoutedEventArgs e)
		{
			sde0 = new SDE(
				double.Parse(textBoxAlpha.Text),
				double.Parse(textBoxBeta.Text),
				double.Parse(textBoxGamma.Text),
				double.Parse(textBoxDelta.Text),
				double.Parse(textBoxX0.Text),
				double.Parse(textBoxY0.Text)
				);

			int n = int.Parse(textBoxN.Text);
			double stdDev = double.Parse(textBoxStdDev.Text);

			bool myWay = (bool)checkBoxMyWay.IsChecked;
			string wayName;

			if (myWay)
			{
				wayName = "Rays";
				sde0.Rays(dt);
			}
			else
			{
				wayName = "OLSO";
				sde0.OSLO(dt);
			}

			textBlockrRes.Text = wayName + " squared error: " + sde0.GetAverageSquaredError() + Environment.NewLine;

			var exactSol = sde0.getSolution;
			plot.drawLine(wayName + " exact sol", exactSol);

			plot.drawPoints("Exact eq", sde0.GetEquilibriumPoint());

			plot.drawPoints("Initial point", new double[] { sde0.x0, sde0.y0 });

			measurements = Generator.getMeasurements(exactSol, stdDev, n);
			plot.drawPoints("Meas", measurements);
		}

		private string printParamReport(double val0, double val1, string name)
		{
			string res = "";
			res += name + " origin: " + val0 + Environment.NewLine;
			res += name + " predicted: " + val1 + Environment.NewLine;
			res += name + " abs error: " + (val1 - val0) + Environment.NewLine;
			res += name + " rel error: " + (val1 - val0) / val0 + Environment.NewLine;
			return res;
		}

		private string printParamsReport(SDE sde0, SDE sde1, bool myWay)
		{
			string res = "";
			double alpha0 = sde0.alpha, beta0 = sde0.beta, gamma0 = sde0.gamma, delta0 = sde0.delta;
			double alpha1 = sde1.alpha, beta1 = sde1.beta, gamma1 = sde1.gamma, delta1 = sde1.delta;

			if (myWay)
			{
				beta0 /= alpha0;
				gamma0 /= alpha0;
				delta0 /= alpha0;
				alpha0 = 1;

				beta1 /= alpha1;
				gamma1 /= alpha1;
				delta1 /= alpha1;
				alpha1 = 1;
			}
			else
			{

				res += printParamReport(alpha0, alpha1, "Alpha");
			}
			res += printParamReport(beta0, beta1, "Beta");
			res += printParamReport(gamma0, gamma1, "Gamma");
			res += printParamReport(delta0, delta1, "Delta");
			double sqerror = Math.Sqrt((
					Math.Pow(alpha1 - alpha0, 2) +
					Math.Pow(beta1 - beta0, 2) +
					Math.Pow(gamma1 - gamma0, 2) +
					Math.Pow(delta1 - delta0, 2)
				) / (myWay ? 3 : 4));

			res += "Squared error: " + sqerror;

			return res;

		}

		private void Infer(object sender, RoutedEventArgs e)
		{
			bool myWay = (bool)checkBoxMyWay.IsChecked;
			try
			{
				double[] inferedParams;
				if (myWay)
				{
					inferedParams = Model.FirstIntegralInfer(measurements);
					sde1 = new SDE(inferedParams[0], inferedParams[1], inferedParams[2], inferedParams[3], inferedParams[4]);
					textBlockrRes.Text += printParamsReport(sde0, sde1, myWay);
					try
					{
						sde1.Rays(dt);
						var predictedSol = sde1.getSolution;
						plot.drawLine("Infered sol", predictedSol);
					}
					catch (Exception) { MessageBox.Show("Error occured during drawing"); }
				} else
				{
					inferedParams = Model.numericalMethodInfer(measurements);
					sde1 = new SDE(inferedParams[0], inferedParams[1], inferedParams[2], inferedParams[3], 0, 0);
					textBlockrRes.Text += printParamsReport(sde0, sde1, myWay);
				}

				plot.drawPoints("Infered eq 1", sde1.GetEquilibriumPoint());
			}
			catch (Exception)
			{
				MessageBox.Show("Failed");
			}
		}
	}
}

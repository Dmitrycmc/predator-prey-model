using System.Windows;
using System.Windows.Media;
using Solver;
using Randomizer;
using Predictor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
			plot.drawLine(wayName + " exact sol", exactSol, Brushes.Green);

			plot.drawPoints("Exact eq", sde0.GetEquilibriumPoint(), Brushes.Green);

			plot.drawPoints("Initial point", new double[] { sde0.x0, sde0.y0 }, Brushes.Green);

			measurements = Generator.getMeasurements(exactSol, stdDev, n);
			plot.drawPoints("Meas", measurements, Brushes.LightGreen);
		}

		private string printParamReport(double val0, double val1, string name)
		{
			return 
				name + " " + 
				Math.Round(val0, 2) + " -> " + 
				Math.Round(val1, 2) + " (" + 
				Math.Round(val1 - val0, 2) + ") " + 
				Math.Round((val1 - val0) * 100 / val0, 2) + "%" + 
				Environment.NewLine;
		}

		private string printParamsReport(SDE sde0, SDE sde1, bool myWay)
		{
			string res = "";
			double alpha0 = sde0.alpha, beta0 = sde0.beta, gamma0 = sde0.gamma, delta0 = sde0.delta;
			double alpha1 = sde1.alpha, beta1 = sde1.beta, gamma1 = sde1.gamma, delta1 = sde1.delta;

			if (myWay)
			{
				beta1 *= alpha0 / alpha1;
				gamma1 *= alpha0 / alpha1;
				delta1 *= alpha0 / alpha1;
				alpha1 *= alpha0 / alpha1;
			}
			res += printParamReport(alpha0, alpha1, "Alpha");
			res += printParamReport(beta0, beta1, "Beta");
			res += printParamReport(gamma0, gamma1, "Gamma");
			res += printParamReport(delta0, delta1, "Delta");
			double sqerror = Math.Sqrt((
					Math.Pow(alpha1 - alpha0, 2) +
					Math.Pow(beta1 - beta0, 2) +
					Math.Pow(gamma1 - gamma0, 2) +
					Math.Pow(delta1 - delta0, 2)
				) / 4);

			res += "Squared error: " + sqerror;
			MessageBox.Show("Squared error: " + sqerror);

			return res + Environment.NewLine + Environment.NewLine;
		}

		private void Clear(object sender, RoutedEventArgs e)
		{
			plot.Clear();
			textBlockrRes.Text = "";
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Process p = new Process();
			p.StartInfo.FileName = Directory.GetCurrentDirectory() + @"\..\..\..\..\collector\bin\Debug\collector.exe";
			p.Start();
			this.Close();
		}

		private void Infer(object sender, RoutedEventArgs e)
		{
			bool myWay = (bool)checkBoxMyWay.IsChecked;
			Brush color;
			try
			{
				double[] inferedParams;
				if (myWay)
				{
					Debug.WriteLine('1');
					inferedParams = Model.FirstIntegralInfer(measurements);
					Debug.WriteLine('2');
					sde1 = new SDE(inferedParams[0], inferedParams[1], inferedParams[2], inferedParams[3], inferedParams[4]);
					Debug.WriteLine('3');
					textBlockrRes.Text += printParamsReport(sde0, sde1, myWay);
					color = Brushes.Orange;
				} else
				{
					inferedParams = Model.numericalMethodInfer(measurements);
					sde1 = new SDE(inferedParams[0], inferedParams[1], inferedParams[2], inferedParams[3], sde0.x0, sde0.y0);
					textBlockrRes.Text += printParamsReport(sde0, sde1, myWay);
					color = Brushes.Blue;
				}
				try
				{
					Debug.WriteLine('4');
					sde1.Rays(dt);
					Debug.WriteLine('5');
					var predictedSol = sde1.getSolution;
					Debug.WriteLine('6');
					plot.drawLine("Infered sol", predictedSol, color);
				}
				catch (Exception) { MessageBox.Show("Error occured during drawing"); }
				plot.drawPoints("Infered eq 1", sde1.GetEquilibriumPoint(), color);
			}
			catch (Exception)
			{
				MessageBox.Show("Failed");
			}
		}
	}
}

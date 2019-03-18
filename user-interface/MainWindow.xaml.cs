﻿using System.Windows;
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
			sde0 = Generator.getRandomSystem();
		}

		private void Generate(object sender, RoutedEventArgs e)
		{
			int n = int.Parse(textBoxN.Text);
			double stdDev = double.Parse(textBoxStdDev.Text);

			bool myWay = (bool) checkBoxMyWay.IsChecked;
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
			plot.drawLine(wayName + " exact solution", exactSol);

			plot.drawPoints("Equilibrium point 0", sde0.GetEquilibriumPoint());
			
			plot.drawPoints("Initial point", new double[] { sde0.x0, sde0.y0 });

			measurements = Generator.getMeasurements(exactSol, stdDev, n);
			plot.drawPoints("Measurements", measurements);
			
		}

		private void infer(object sender, RoutedEventArgs e)
		{
			try
			{
				var a = Model.FirstIntegralInfer(measurements, sde0.alpha);
				sde1 = new SDE(a[0], a[1], a[2], a[3], a[4]);

				sde1.Rays(dt);
				var predictedSol = sde1.getSolution;
				plot.drawLine("Infered", predictedSol);
				
				plot.drawPoints("Equilibrium point 1", sde1.GetEquilibriumPoint());
			}
			catch (Exception)
			{
				MessageBox.Show("Error");
			}
		}
	}
}

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

namespace user_interface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SDE sde;

		public MainWindow()
		{
			InitializeComponent();
			sde = Generator.getRandomSystem();
		}

		public void Demonstrate(bool myWay)
		{
			const double dt = 0.01;
			double stdDev = 0.05;
			int n = 20;

			string wayName;

			if (myWay)
			{
				wayName = "Rays";
				sde.Rays(dt);
			} else
			{
				wayName = "OLSO";
				sde.OSLO(dt);
			}

			//MessageBox.Show(wayName + " squared error: " + sde.GetAverageSquaredError());

			var equilibriumPoint = sde.GetEquilibriumPoint();
			plot.drawPoints("Equilibrium point", equilibriumPoint);

			var exactSol = sde.getSolution;
			plot.drawLine(wayName + " orig", sde.getSolution);

			var measurements = Generator.getMeasurements(exactSol, stdDev, n);
			plot.drawPoints(wayName + "noised", measurements);
			try
			{
				var a = Model.FirstIntegralInfer(measurements);
				MessageBox.Show(a);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		private void Button_solve1_Click(object sender, RoutedEventArgs e)
		{
			Demonstrate(true);
		}

		private void Button_solve2_Click(object sender, RoutedEventArgs e)
		{
			Demonstrate(false);
		}
	}
}

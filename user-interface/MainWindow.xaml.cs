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
			Demonstrate(true);
		}

		public void Demonstrate(bool myWay = false)
		{
			const double dt = 0.01;
			double stdDev = 0.05;
			int n = 50;

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

			label_squaredError.Content = wayName + " squared error: " + sde.GetAverageSquaredError();

			plot.drawPoints("Equilibrium point", sde.GetEquilibriumPoint());

			plot.drawPoints("Initial point", new double[] {sde.x0, sde.y0 });

			var exactSol = sde.getSolution;
			plot.drawLine(wayName + " exact", exactSol);

			var measurements = Generator.getMeasurements(exactSol, stdDev, n);
			plot.drawPoints(wayName + " noised", measurements);
			
			
			try
			{
				var a = Model.FirstIntegralInfer(measurements, sde.alpha);
				SDE predicted = new SDE(a[0], a[1], a[2], a[3], a[4]);

				MessageBox.Show(sde.alpha + " " + sde.beta + " " + sde.gamma + " " + sde.delta + '\n' + a[0] + " " + a[1] + " " + a[2] + " " + a[3] + " " + a[4]);
				
				predicted.Rays(dt);
				var predictedSol = predicted.getSolution;
				plot.drawLine("Infered", predictedSol);
				// log.normal
				plot.drawPoints("Equilibrium point 2", predicted.GetEquilibriumPoint());
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

using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Wpf.CartesianChart.PointShapeLine;
using Solver;
using Randomizer;

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

		public void demonstrate(bool myWay)
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

			var exactSol = sde.getSolution;
			plot.drawLine(wayName + " orig", sde.getSolution);

			var measurements = Generator.getMeasurements(exactSol, stdDev, n);
			plot.drawPoints(wayName + "noised", measurements);

			var equilibriumPoint = sde.GetEquilibriumPoint();
			plot.drawPoints("Equilibrium point", equilibriumPoint);
		}

		private void Button_solve1_Click(object sender, RoutedEventArgs e)
		{
			demonstrate(true);
		}

		private void Button_solve2_Click(object sender, RoutedEventArgs e)
		{
			demonstrate(false);
		}
	}
}

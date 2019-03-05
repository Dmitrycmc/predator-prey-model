using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Wpf.CartesianChart.PointShapeLine;
using Solver;
using Measurer;

namespace user_interface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public void demonstrate(bool myWay)
		{
			var sde = new Model(1, 1, 3, 5, 1, 2);
			const double dt = 0.1;
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
			MessageBox.Show(wayName + " squared error: " + sde.GetAverageSquaredError());
			var exactSol = sde.getSolution;
			var measurements = Noise.getMeasurements(exactSol, 0.05, 20);
			plot.drawLine(wayName + " orig", sde.getSolution);
			plot.drawPoints(wayName + "noised", measurements);

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

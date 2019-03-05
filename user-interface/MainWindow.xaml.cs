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

		private void Button_solve_Click(object sender, RoutedEventArgs e)
		{
			var sde = new Model(1, 1, 3, 5, 1, 2);
			const double dt = 0.1;

			sde.OSLO(dt);
			MessageBox.Show("OSLO method squared error: " + sde.GetAverageSquaredError());
			var distortedMeasurements = Noise.getMeasurements(sde.getSolution, 0, 100);
			var SolutionOSLO = Utils.getPhasePathPoints(distortedMeasurements);
			plot.SeriesCollection.Add(new LineSeries
				{
					Title = "OSLO",
					PointGeometrySize = 0,
					Values = new ChartValues<ObservablePoint>(SolutionOSLO),
					Fill = Brushes.Transparent
				}
			);

			sde.Rays(dt);
			MessageBox.Show("Rays method squared error: " + sde.GetAverageSquaredError());
		    distortedMeasurements = Noise.getMeasurements(sde.getSolution, 0.01, 100);
			var SolutionRays = Utils.getPhasePathPoints(distortedMeasurements);

			plot.SeriesCollection.Add(new LineSeries
				{
					Title = "Rays",
					Values = new ChartValues<ObservablePoint>(SolutionRays),
					PointGeometry = DefaultGeometries.Circle,
					StrokeThickness = 2,
					Fill = Brushes.Transparent
				}
			);

		}
	}
}

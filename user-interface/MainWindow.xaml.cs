using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Solver;
using Wpf.CartesianChart.PointShapeLine;

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

			var sde = new Model(1, 2, 3, 5, 1, 2);
			const double dt = 0.01;

			//sde.OSLO(dt);
			var SolutionOSLO = Utils.getPhasePathPoints(sde.getSolution);
			plot.SeriesCollection.Add(new LineSeries
				{
					Title = "OSLO",
					PointGeometrySize = 0,
					Values = new ChartValues<ObservablePoint>(SolutionOSLO),
				}
			);

			sde.Projection(dt);
			var SolutionProj = Utils.getPhasePathPoints(sde.getSolution);
		
			plot.SeriesCollection.Add(new LineSeries
				{
					Title = "Projection",
					PointGeometrySize = 0,
					Values = new ChartValues<ObservablePoint>(SolutionProj),
				}
			);
		}
	}
}

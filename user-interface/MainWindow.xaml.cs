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
			SDEPP sde = new SDEPP(1, 1, 1, 1, 1, 2);
			sde.OSLO();
			double[,] solution1 = sde.getSolution;
			sde.Projection();
			double[,] solution2 = sde.getSolution;

			List<ObservablePoint> op1 = new List<ObservablePoint>();

			for (int i = 0; i < solution1.GetLength(0); i++)
			{
				op1.Add(new ObservablePoint(solution1[i, 1], solution1[i, 2]));
			}

			List<ObservablePoint> op2 = new List<ObservablePoint>();

			for (int i = 0; i < solution1.GetLength(0); i++)
			{
				op2.Add(new ObservablePoint(solution2[i, 1], solution2[i, 2]));
			}

			plot.SeriesCollection.Add(new LineSeries
				{
					Title = "PROJECT",
					PointGeometrySize = 0,
					Values = new ChartValues<ObservablePoint>(op2),
				}
			);
			plot.SeriesCollection.Add(new LineSeries
				{
					Title = "OSLO",
					PointGeometrySize = 0,
					Values = new ChartValues<ObservablePoint>(op1),
				}
			);
		}
	}
}

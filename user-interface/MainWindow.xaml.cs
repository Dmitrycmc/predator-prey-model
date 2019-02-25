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
			SDEPP sde = new SDEPP(1, 1, 5, 4, 1, 1);
			double[,] solution = sde.getSolution;

			List<ObservablePoint> op = new List<ObservablePoint>();

			for (int i = 0; i < solution.GetLength(0); i++)
			{
				op.Add(new ObservablePoint(solution[i,1],solution[i,2]));
			}

			plot.SeriesCollection.Add(new LineSeries
					{
						Title = "Real line",
						PointGeometrySize = 0,
						Values = new ChartValues<ObservablePoint>(op),
					});
		}
	}
}

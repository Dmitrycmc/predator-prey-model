using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.PointShapeLine
{
	public static class Utils {
		public static List<ObservablePoint> getPhasePathPoints (List<double[]> src) {
			var result = new List<ObservablePoint>();
			src.ForEach(points => {
				result.Add(new ObservablePoint(points[0], points[1]));
			});
			return result;
		}
	}

public partial class PointShapeLine : UserControl
	{
		public PointShapeLine()
		{
			InitializeComponent();

			SeriesCollection = new SeriesCollection
			{
				
			};
			
			YFormatter = value => string.Format("{0:0.00}", value);
			DataContext = this;
		}

		public SeriesCollection SeriesCollection { get; set; }
		public Func<double, string> YFormatter { get; set; }

	}
}
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.PointShapeLine
{
	public partial class PointShapeLine : UserControl
	{
		public static List<ObservablePoint> getPhasePathPoints(List<double[]> src)
		{
			var result = new List<ObservablePoint>();
			src.ForEach(points => {
				result.Add(new ObservablePoint(points[0], points[1]));
			});
			return result;
		}

		public void drawLine(string title, List<double[]> values, Brush color, Geometry geometry = null)
		{
			drawLine(title, new ChartValues<ObservablePoint>(getPhasePathPoints(values)), color, geometry);
		}

		public void drawLine(string title, ChartValues<ObservablePoint> values, Brush color, Geometry geometry = null)
		{
			SeriesCollection.Add(new LineSeries
			{
				Title = title,
				PointGeometry = geometry,
				Values = values,
				Fill = Brushes.Transparent,
				Stroke = color
			}
			);
		}

		public void drawPoints(string title, double[] point, Brush color, Geometry geometry = null)
		{
			drawPoints(title, new List<double[]>() { point }, color, geometry);
		}

		public void drawPoints(string title, List<double[]> values, Brush color, Geometry geometry = null)
		{
			drawPoints(title, new ChartValues<ObservablePoint>(getPhasePathPoints(values)), color, geometry);
		}

		public void drawPoints(string title, ChartValues<ObservablePoint> values, Brush color, Geometry geometry = null)
		{
			if (geometry == null)
			{
				geometry = DefaultGeometries.Circle;
			}
			SeriesCollection.Add(new ScatterSeries {
				Title = title,
				PointGeometry = geometry,
				Values = values,
				Fill = color
				}
			);
		}

		public PointShapeLine()
		{
			InitializeComponent();

			SeriesCollection = new SeriesCollection { };
			YFormatter = value => string.Format("{0:0.00}", value);
			DataContext = this;
		}

		public SeriesCollection SeriesCollection { get; set; }
		public Func<double, string> YFormatter { get; set; }

	}
}
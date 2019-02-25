using System;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.PointShapeLine
{
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
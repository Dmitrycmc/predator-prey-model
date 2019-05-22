using System;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;

namespace Wpf.CartesianChart.HeatChart
{
	public partial class HeatSeriesExample : UserControl
	{
		public HeatSeriesExample()
		{
			InitializeComponent();
			
			Values = new ChartValues<HeatPoint>
			{
                //X means sales man
                //Y is the day
 
                //"Jeremy Swanson"
                new HeatPoint(0, 0, 1),
				new HeatPoint(0, 1, 2),
 
                //"Lorena Hoffman"
                new HeatPoint(1, 0, 4),
				new HeatPoint(1, 1, 2),
 
                //"Robyn Williamson"
                new HeatPoint(2, 0, 2),
				new HeatPoint(2, 1, 3),
 
                //"Carole Haynes"
                new HeatPoint(3, 0, 2),
				new HeatPoint(3, 1, 7),
 
			};

			Days = new[]
			{
				1, 2
			};

			SalesMan = new[]
			{
				1, 2, 3, 4
			};

			DataContext = this;
		}

		public ChartValues<HeatPoint> Values { get; set; }
		public int[] Days { get; set; }
		public int[] SalesMan { get; set; }
	
		public void draw(int a)
		{
			Values = new ChartValues<HeatPoint>
			{
                //X means sales man
                //Y is the day
 
                //"Jeremy Swanson"
                new HeatPoint(0, 0, a),
				new HeatPoint(0, 1, a),
 
                //"Lorena Hoffman"
                new HeatPoint(1, 0, 4),
				new HeatPoint(1, 1, 2),
 
                //"Robyn Williamson"
                new HeatPoint(2, 0, 2),
				new HeatPoint(2, 1, 3),
 
                //"Carole Haynes"
                new HeatPoint(3, 0, 2),
				new HeatPoint(3, 1, 7),

			};
		}
	}
}
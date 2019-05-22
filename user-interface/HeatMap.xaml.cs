using System;
using System.Collections.Generic;
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

			List<string> alphaBetaList = new List<string>();
			List<string> gammaDeltaList = new List<string>();

			for (double a = 0.5; a <= 2.5; a += 0.5)
			{
				for (double b = 0.5; b <= 2.5; b += 0.5)
				{
					alphaBetaList.Add(a + " " + b);
					gammaDeltaList.Add(a + " " + b);
				}
			}

			alphaBeta = alphaBetaList.ToArray();
			gammaDelta = gammaDeltaList.ToArray();

			DataContext = this;
		}

		public ChartValues<HeatPoint> Values { get; set; }
		public string[] alphaBeta { get; set; }
		public string[] gammaDelta { get; set; }
	
		public void draw(double[,] Values)
		{
			this.Values = new ChartValues<HeatPoint>();

			for (int i = 0; i < alphaBeta.Length; i++)
			{
				for (int j = 0; j < alphaBeta.Length; j++)
				{
					this.Values.Add(new HeatPoint(i, j, Values[i, j]));
				}
			}
		}
	}
}
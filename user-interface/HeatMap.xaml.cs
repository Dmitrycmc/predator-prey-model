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
					if (Values[i, j] >= 0)
					{
						this.Values.Add(new HeatPoint(i, j, Values[i, j]));
					} 
				}
			}
		}

		public void labels(string[] alphaBetaList, string[] gammaDeltaList)
		{
			alphaBeta = alphaBetaList;
			gammaDelta = gammaDeltaList;
		}
	}
}
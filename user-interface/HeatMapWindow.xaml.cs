using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace user_interface
{
	/// <summary>
	/// Interaction logic for HeatMapWindow.xaml
	/// </summary>
	public partial class HeatMapWindow : Window
	{
		public HeatMapWindow()
		{
			InitializeComponent();

			List<Tuple<double, double>> alphaBeta = new List<Tuple<double, double>>();

			for (double a = 0.5; a <= 2.5; a += 0.5)
			{
				for (double b = 0.5; b <= 2.5; b += 0.5)
				{
					alphaBeta.Add(new Tuple<double, double>(a, b));
				}
			}

			alphaBeta.Sort((first, second) => {
				double a = (first.Item1 / first.Item2);
				double b = (second.Item1 / second.Item2);
				if (a == b) return 0;
				if (a > b) return 1;
				return -1;
			});

			double[,] values = new double[25, 25];

			string path = Directory.GetCurrentDirectory() + @"\..\..\..\..\" + @"\reports\HeatMap.txt";
			try
			{
				using (StreamReader sr = new StreamReader(path))
				{
					for (double alpha = 0.5; alpha <= 2.5; alpha += 0.5)
					{
						for (double beta = 0.5; beta <= 2.5; beta += 0.5)
						{
							for (double gamma = 0.5; gamma <= 2.5; gamma += 0.5)
							{
								for (double delta = 0.5; delta <= 2.5; delta += 0.5)
								{
									double value = sr.Peek() > -1 ? double.Parse(sr.ReadLine()) : -1;
									int ind1 = alphaBeta.FindIndex(pair => pair.Item1 == alpha && pair.Item2 == beta);
									int ind2 = alphaBeta.FindIndex(pair => pair.Item1 == gamma && pair.Item2 == delta);
									values[ind1, ind2] = value;
								}
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("The process failed: {0}", e.ToString());
			}
			
			string[] alphaBetaLabels = alphaBeta.Select(pair => pair.Item1 + " " + pair.Item2).ToArray();
			
			heatMap.labels(alphaBetaLabels, alphaBetaLabels);
			heatMap.draw(values);
		}
	}
}

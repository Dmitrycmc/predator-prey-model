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

			double[,] values = new double[25, 25];

			string path = Directory.GetCurrentDirectory() + @"\..\..\..\..\" + @"\reports\HeatMap.txt";
			try
			{
				using (StreamReader sr = new StreamReader(path))
				{
					for (int i = 0; i < 25; i++)
					{
						for (int j = 0; j < 25; j++)
						{
							double value = sr.Peek() > -1 ? double.Parse(sr.ReadLine()) : -1;
							values[i, j] = value;
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("The process failed: {0}", e.ToString());
			}

			heatMap.draw(values);
		}
	}
}

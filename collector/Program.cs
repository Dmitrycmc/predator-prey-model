using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collector
{
	class Program
	{
		static void WriteToFile(string str)
		{
			using (System.IO.StreamWriter file =
			   new System.IO.StreamWriter(Directory.GetCurrentDirectory() + @"\..\..\..\..\file.txt", true))
			{
				file.WriteLine(str);
			}
		}

		static int ReadInt(string name)
		{
			int res = 0;
			bool success = false;

			Console.WriteLine(name);
			while (!success)
			{
				try
				{
					res = int.Parse(Console.ReadLine());
					success = true;
				}
				catch
				{
				}
			}
			return res;
		}

		static double ReadDouble(string name)
		{
			double res = 0;
			bool success = false;

			Console.WriteLine("Enter " + name + ":");
			while (!success)
			{
				try
				{
					res = double.Parse(Console.ReadLine());
					success = true;
				}
				catch
				{
				}
			}
			return res;
		}

		static void Main(string[] args)
		{
			int experimentsNum = ReadInt("number of experiments");
			int sampleSize = ReadInt("size of sample");
			double minValue = ReadInt("minValue");
			double maxValue = ReadInt("maxValue");
			getRandomParams






			Console.ReadKey();
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Randomizer;
using Solver;


namespace collector
{
	class Program
	{
		static readonly double dt = 0.01;

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
			double stdDev = ReadDouble("stdDev");
			double minValue = ReadInt("minValue");
			double maxValue = ReadInt("maxValue");

			double[] parameters = Generator.getRandomParams(minValue, maxValue);
			SDE sde0 = new SDE(
				parameters[0],
				parameters[1],
				parameters[2],
				parameters[3],
				parameters[4],
				parameters[5]
			);

			for (int i = 0; i < experimentsNum; i++) {


				sde0.Rays(dt);

				string report = i + ": " + sde0.GetAverageSquaredError() + " ";
				
				var exactSol = sde0.getSolution;
				var measurements = Generator.getMeasurements(exactSol, stdDev, sampleSize);






				sde0.OSLO(dt);

				exactSol = sde0.getSolution;
				measurements = Generator.getMeasurements(exactSol, stdDev, sampleSize);



				WriteToFile(report);
			}

			Console.ReadKey();
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Randomizer;
using Solver;
using Predictor;

namespace collector
{
	class Program
	{
		static readonly double dt = 0.01;

		static void WriteToFile(string str)
		{
			using (StreamWriter file =
			   new StreamWriter(Directory.GetCurrentDirectory() + @"\..\..\..\..\" + "Report.txt", true))
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

			string report;

			for (int i = 0; i < experimentsNum; i++) {
				double[] parameters = Generator.getRandomParams(minValue, maxValue);
				SDE sde1, sde0 = new SDE(
					parameters[0],
					parameters[1],
					parameters[2],
					parameters[3],
					parameters[4],
					parameters[5]
				);

				report =
					parameters[0] + ";" +
					parameters[1] + ";" +
					parameters[2] + ";" +
					parameters[3] + ";" +
					parameters[4] + ";" +
					parameters[5];

				foreach (bool myWay in new bool[] { false, true })
				{
					if (myWay)
					{
						sde0.Rays(dt);
					} else
					{
						sde0.OSLO(dt);
					}


					var exactSol = sde0.getSolution;
					var measurements = Generator.getMeasurements(exactSol, stdDev, sampleSize);

					try
					{
						double[] inferedParams;

						if (myWay)
						{
							inferedParams = Model.FirstIntegralInfer(measurements);
							sde1 = new SDE(inferedParams[0], inferedParams[1], inferedParams[2], inferedParams[3], inferedParams[4]);
						} else
						{
							inferedParams = Model.numericalMethodInfer(measurements);
							sde1 = new SDE(inferedParams[0], inferedParams[1], inferedParams[2], inferedParams[3], sde0.x0, sde0.y0);
						}
						
						double alpha0 = sde0.alpha, beta0 = sde0.beta, gamma0 = sde0.gamma, delta0 = sde0.delta;
						double alpha1 = sde1.alpha, beta1 = sde1.beta, gamma1 = sde1.gamma, delta1 = sde1.delta;
						if (myWay)
						{
							beta1 *= alpha0 / alpha1;
							gamma1 *= alpha0 / alpha1;
							delta1 *= alpha0 / alpha1;
							alpha1 *= alpha0 / alpha1;
						}

						double sqerror = Math.Sqrt((
								Math.Pow(alpha1 - alpha0, 2) +
								Math.Pow(beta1 - beta0, 2) +
								Math.Pow(gamma1 - gamma0, 2) +
								Math.Pow(delta1 - delta0, 2)
							) / 4);

						report += ";" + sqerror;
						
						sde1.Rays(dt);
						var predictedSol = sde1.getSolution;
					}
					catch (Exception)
					{
						report += ";failed";
					}

				}
				WriteToFile(report);

			}

			Console.Write("Finished!!");
			Console.ReadKey();
		}
	}
}

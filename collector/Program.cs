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

		static void WriteToFile(string str, string fileName)
		{
			using (StreamWriter file =
			   new StreamWriter(Directory.GetCurrentDirectory() + @"\..\..\..\..\" + @"\reports\" + fileName, true))
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
			bool myWay = ReadInt("myWay") == 1;
			int medianIndex = (int)Math.Floor((double)((experimentsNum - 1) / 2));
			
			string report;

			for (double alpha = 0.5; alpha <= 2.5; alpha += 0.5)
			{
				for (double beta = 0.5; beta <= 2.5; beta += 0.5)
				{
					for (double gamma = 0.5; gamma <= 2.5; gamma += 0.5)
					{
						for (double delta = 0.5; delta <= 2.5; delta += 0.5)
						{
							List<double> results = new List<double>();
							for (int k = 0; k < experimentsNum; k++)
							{

								report = sampleSize + ";";
								double[] parameters = Generator.getRandomInitPoint(alpha, beta, gamma, delta);

								SDE sde1, sde0 = new SDE(
									alpha,
									beta,
									gamma,
									delta,
									parameters[0],
									parameters[1]
								);

								report +=
									alpha + ";" +
									beta + ";" +
									gamma + ";" +
									delta + ";" +
									parameters[0] + ";" +
									parameters[1];

								if (myWay)
								{
									sde0.Rays(dt);
								}
								else
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
									}
									else
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
									results.Add(sqerror);
								}
								catch (Exception)
								{
									report += ";failed";
								}
								WriteToFile(report, "Report.csv");
							}
							results.Sort();
							double median;
							if (results.Count >= medianIndex + 1)
							{
								median = results[medianIndex];
							} else
							{
								median = -1;
							}
							WriteToFile(median.ToString(), "HeatMap.txt");
						}
					
					}
				}
			}

			Console.Write("Finished!!");
			Console.ReadKey();
		}
	}
}

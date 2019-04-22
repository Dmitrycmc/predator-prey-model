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
		
		static int ReadInt()
		{
			int res = 0;
			bool success = false;

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

		static void Main(string[] args)
		{
			Console.WriteLine("Enter n:");
			Console.WriteLine(ReadInt());
			Console.ReadKey();

		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor
{
	public static class Utils
	{
		static internal string trimEnd(string str)
		{
			while (!char.IsNumber(str[str.Length - 1])) str = str.Substring(0, str.Length - 1);
			return str;
		}

		static internal double parseBetween(string str, char c1, char c2)
		{
			int firstSign = str.IndexOf(c1) + 1;
			int lastSign = str.IndexOf(c2);
			string substr = str.Substring(firstSign, lastSign - firstSign);
			substr = trimEnd(substr);
			double res = double.Parse(substr);
			return res;
		}

	}
}

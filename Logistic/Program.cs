using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistic
{
    class Program
    {
        static void Main(string[] args)
        {
			double LogisticFunc(double x, double r) => r * x * (1 - x);


			int iterations = 1024;
			int iter_min = 64;
			int iter_max = iterations;
			double match_tolerance = 1e-5;
			Random rand = new Random();
			double x = rand.NextDouble();
			x = 0.4042476770372687;
			Console.WriteLine($"x: {x}");
			double r = 3.54409;
			r = 1 + Math.Sqrt(8);
			//r = 3.5693;
			//r = 3.574;
			//r = 3.82843;
			//r = 3.543;
			Console.WriteLine($"r: {r}");

			List<double> values = new List<double>();
			bool stop = false;
			do
			{
				int current_iter = iter_max - iterations;
				values.Add(x);
				x = LogisticFunc(x, r);
			} while (--iterations > 0 && !stop);

			// reverse and trim the list
			values = values.TakeLast(64).Reverse().ToList();
			
			List<double> OscillationValues = new List<double>();
			for (int i = 0; i < values.Count; i++)
			{
				if (OscillationValues.Any(ov => NearlyEqual(values[i], ov, match_tolerance))) break;
				for (int j = i + 1; j < values.Count; j++)
				{
					if (NearlyEqual(values[i], values[j], match_tolerance))
					{
						OscillationValues.Add(values[i]);
						break;
					}
				}
			}

			Console.WriteLine("Oscillation Values");
			foreach (var v in OscillationValues.OrderByDescending(ov => ov))
			{
				Console.WriteLine(v);
			}
		}

		static bool NearlyEqual(double a, double b, double epsilon)
		{
			const double MinNormal = 2.2250738585072014E-308d;
			double absA = Math.Abs(a);
			double absB = Math.Abs(b);
			double diff = Math.Abs(b - a);

			if (a.Equals(b))
			{ // shortcut, handles infinities
				return true;
			}
			else if (a == 0 || b == 0 || absA + absB < MinNormal)
			{
				// a or b is zero or both are extremely close to it
				// relative error is less meaningful here
				return diff < (epsilon * MinNormal);
			}
			else
			{ // use relative error
				return diff / (absB) < epsilon;
			}
		}

	}

}

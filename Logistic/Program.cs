using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistic
{
    class Program
    {
        static void Main(string[] args)
        {
			decimal LogisticFunc(decimal x, decimal r) => r * x * (1 - x);


			int iterations = 1024;
			int iter_min = 64;
			int iter_max = iterations;
			decimal match_tolerance = 1e-5m;
			Random rand = new Random();
			decimal x = (decimal)rand.NextDouble();
			//x = 0.4042476770372687;
			Console.WriteLine($"x: {x}");
			decimal r = 3.54409m;
			//r = 1 + Math.Sqrt(8);
			//r = 3.5693;
			//r = 3.574;
			//r = 3.82843;
			r = 3.543m;
			r = 3.56440726m;

            Console.WriteLine($"r: {r}");

			List<decimal> values = new List<decimal>();
			bool stop = false;
			do
			{
				int current_iter = iter_max - iterations;
				values.Add(x);
				x = LogisticFunc(x, r);
			} while (--iterations > 0 && !stop);

			// reverse and trim the list
			values = values.TakeLast(64).Reverse().ToList();
			
			List<decimal> OscillationValues = new List<decimal>();
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

			Console.WriteLine($"Oscillation Values ({OscillationValues.Count})");
			foreach (var v in OscillationValues.OrderByDescending(ov => ov))
			{
				Console.WriteLine(v);
			}
		}

		static bool NearlyEqual(decimal a, decimal b, decimal epsilon)
		{
			const decimal MinNormal = 2.2250738585072014E-308m;
			decimal absA = Math.Abs(a);
			decimal absB = Math.Abs(b);
			decimal diff = Math.Abs(b - a);

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

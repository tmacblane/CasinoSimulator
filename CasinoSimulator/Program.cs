using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoSimulator
{
	class Program
	{
		#region Type specific properties

		public static int Main(string[] args)
		{
			// Prompt to select a simulator
			Options options = new Options();

			if(!options.Parse(args))
			{
				return 1;
			}

			return 0;
		}

		#endregion
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoSimulator
{
	public class Options
	{
		#region Type specific methods

		public bool Parse(string[] args)
		{
			bool result = false;

			if(args.Count() < 1)
			{
				this.ShowUsage();
				result = false;
			}
			else
			{
				foreach(string arg in args)
				{
					string param = arg.Substring(1).ToLower();

					if(param == "?" || param == "help" || param == "h")
					{
						this.ShowUsage();
					}
					else
					{
						switch(arg.ToLower())
						{
							case "c":
							case "craps":
								// Run Craps Simulator
								Craps.Simulator crapsSimulator = new Craps.Simulator();
								crapsSimulator.RunSimulator();
								result = true;
								break;

							case "r":
							case "roulette":
								// Run Roulette Simulator
								result = true;
								break;

							case "b":
							case "baccarat":
								// Run Baccarat Simulator
								result = true;
								break;

							default:
								this.ShowUsage("Unknown parameter: " + arg);
								result = false;
								break;
						}
					}

					result = false;
					break;
				}
			}

			return result;
		}

		#endregion

		#region Helpers

		private void ShowUsage(string errorMsg = null)
		{
			Console.WriteLine(string.Empty);
			if(!String.IsNullOrEmpty(errorMsg))
			{
				Console.WriteLine(String.Format("ERROR: {0}\n", errorMsg));
			}

			string exeName = System.AppDomain.CurrentDomain.FriendlyName;

			Console.WriteLine("Usage: " + exeName + " [options]\n");
			Console.WriteLine("Options:\n");

			Console.WriteLine("    h | help \t\t Displays command line help\n");
			Console.WriteLine("    b | baccarat \t Run Baccarat Simulator\n");
			Console.WriteLine("    c | craps \t\t Run Craps Simulator\n");
			Console.WriteLine("    r | roulette \t Run Roulette Simulator");
		}

		#endregion
	}
}

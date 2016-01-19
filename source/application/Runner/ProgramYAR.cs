/* Runner can support both solution file
 * and dll file. But for now I have only
 * programmed to support dll file and if
 * dll is not passed it will use current
 * executing assembly.
*/

namespace Tarsvin.Runner
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Configuration;
	using System.IO;
	using TestPipe.Common;

	internal class ProgramYAR
	{
		private static void Main(string[] args)
		{
			string[] ags = args;
			string[] includeS = null;
			string[] excludeS = null;
			string[] includeF = null;
			string[] excludeF = null;

			var options = new Options();

			if (!CommandLine.Parser.Default.ParseArguments(args, options))
			{
				Console.Error.Write("Invalid arguments.");
				return;
			}

			if(options.IncludeS != null && !StringComparer.OrdinalIgnoreCase.Equals(options.IncludeS, "none"))
				includeS = options.IncludeS.Split(',');

			if (options.ExcludeS != null && !StringComparer.OrdinalIgnoreCase.Equals(options.ExcludeS, "none"))
				excludeS = options.ExcludeS.Split(',');

			if (options.IncludeF != null && !StringComparer.OrdinalIgnoreCase.Equals(options.IncludeF, "none"))
				includeF = options.IncludeF.Split(',');

			if (options.ExcludeF != null && !StringComparer.OrdinalIgnoreCase.Equals(options.ExcludeF, "none"))
				excludeF = options.ExcludeF.Split(',');

			if (options.Project != null)
				if (!options.Project.EndsWith(".dll"))
				{
					Console.Error.WriteLine("-d has invalided argument.");
					Console.Error.WriteLine("argument should end with \".dll\"");
					Console.Error.Write(options.GetUsage());
					Console.Error.WriteLine("");
					return;
				}

			if(options.XmlResultPath != null)
				if (!options.XmlResultPath.EndsWith(".xml"))
				{
					Console.Error.WriteLine("-x has invalid argument.");
					Console.Error.WriteLine("argument should end with \".xml\"");
					Console.Error.Write(options.GetUsage());
					Console.Error.WriteLine("");
					return;
				}

			AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", string.Format("{0}{1}", options.Project, ".config"));

			if (Convert.ToBoolean(options.RunnerSelection.CompareTo("Sequential")) &&
				Convert.ToBoolean(options.RunnerSelection.CompareTo("Parallel")))
			{
				Console.Error.WriteLine("-n is a required argument.");
				Console.Error.WriteLine("");
				Console.Error.Write(options.GetUsage());
				Console.Error.WriteLine("");
				return;
			}

			DllInfo dllInfo = new DllInfo() {path = options.Project };
			new StepLoader(dllInfo);
			Executor exe = new Executor(options.RunnerSelection, dllInfo, 
				options.Project, options.XmlResultPath, includeF, excludeF, includeS, excludeS);
			exe.InitializeSystem(exe);
			while (true) ;
		}
	}
}
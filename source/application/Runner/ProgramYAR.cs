namespace Tarsvin.Runner
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using TestPipe.Common;

    internal class ProgramYAR
    {
        private static void Main(string[] args)
        {
            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.Error.Write("Invalid arguments.");
                return;
            }

            if (!options.SolutionFile.EndsWith(".sln"))
            {
                Console.Error.WriteLine("-s is a required argument.");
                Console.Error.WriteLine("");
                Console.Error.Write(options.GetUsage());
                Console.Error.WriteLine("");
                return;
            }

            if (Convert.ToBoolean(options.RunnerSelection.CompareTo("Sequential")) &&
                Convert.ToBoolean(options.RunnerSelection.CompareTo("Parallel")))
            {
                Console.Error.WriteLine("-r is a required argument.");
                Console.Error.WriteLine("");
                Console.Error.Write(options.GetUsage());
                Console.Error.WriteLine("");
                return;
            }

            string actualPath = options.SolutionFile;
            Console.WriteLine("Solution File: {0}", actualPath);

            string projectPrefix = options.ProjectPrefix ?? ConfigurationManager.AppSettings["tarsvin.test.projectnameprefix"];
            Console.WriteLine("Project Prefix: {0}", projectPrefix);

            Solution sln = new Solution(actualPath);
            List<DllInfo> dllList = RunnerHelper.GetDllList(options, actualPath, projectPrefix, sln);

            new StepLoader(dllList);
            Executor exe = new Executor(options.RunnerSelection, dllList);
            exe.InitializeSystem(exe);
            Console.ReadKey();
        }
    }
}
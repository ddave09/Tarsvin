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
            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.Error.Write("Invalid arguments.");
                return;
            }

            if(options.Project != null)
                if (!options.Project.EndsWith(".dll"))
                {
                    Console.Error.WriteLine("-d has invalided argument.");
                    Console.Error.WriteLine("argument should end with \".dll\"");
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

            string actualPath = options.SolutionFile;
            Console.WriteLine("Solution File: {0}", actualPath);

            string projectPrefix = options.ProjectPrefix ?? ConfigurationManager.AppSettings["tarsvin.test.projectnameprefix"];
            Console.WriteLine("Project Prefix: {0}", projectPrefix);

            if (actualPath != null)
            {
                Solution sln = new Solution(actualPath);
                RunnerHelper.GetDllList(options, actualPath, projectPrefix, sln);
            }
            
            List<DllInfo> dllList = new List<DllInfo>() { new DllInfo() { path = options.Project} };

            new StepLoader(dllList);
            Executor exe = new Executor(options.RunnerSelection, dllList);
            exe.InitializeSystem(exe);
            Console.ReadKey();
        }
    }
}
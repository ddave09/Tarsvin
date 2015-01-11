namespace Runner
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;

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

            string actualPath = options.SolutionFile;
            Console.WriteLine("Solution File: {0}", actualPath);

            string projectPrefix = options.ProjectPrefix ?? ConfigurationManager.AppSettings["tarsvin.test.projectnameprefix"];
            Console.WriteLine("Project Prefix: {0}", projectPrefix);

            Executor exe = new Executor();
            Solution sln = new Solution(actualPath);
            List<DllInfo> dllList = RunnerHelper.GetDllList(options, actualPath, projectPrefix, sln);

            new StepLoader(dllList);
            exe.ExcecuteTest(dllList);

            IEnumerator ie = GlobalTestStates.manageState.GetEnumerator();
            IEnumerator fie = GlobalTestStates.featureState.GetEnumerator();

            while (fie.MoveNext())
            {
                IndividualFeatureTestState itfs = fie.Current as IndividualFeatureTestState;
                Console.WriteLine("********************************************************************************");
                Console.WriteLine("Feature Namespace {0}", itfs.FeatureName);
                Console.WriteLine("Execution Time {0}", itfs.FeatureExecutionTime);
            }

            while (ie.MoveNext())
            {
                IndividualTestState its = ie.Current as IndividualTestState;
                //Console.WriteLine("Time Name {0} Test Span {1} Result {2}", its.ExecTime, its.TestName, its.Result);
                Console.WriteLine("********************************************************************************");
                Console.WriteLine("Scenario Namespace {0}", its.NameSpace);
                Console.WriteLine("Scenario Name {0}", its.TestName);
                Console.WriteLine("Execution Time {0}", its.ExecTime);
                Console.Write("Attributes:  ");
                foreach (string s in its.Attributes)
                {
                    Console.Write("{0}   ", s.Trim('"'));
                }
                Console.WriteLine();
                Console.WriteLine("Result {0}", its.Result);
                if (its.ThrownException != null)
                {
                    Console.WriteLine("--------------------");
                    Console.WriteLine(its.ExceptionMessageStackTrace);
                    Console.WriteLine("--------------------");
                }
            }
            Console.WriteLine("*******************************************************************************");
            Console.WriteLine("All available tests are completed");
            Console.WriteLine("*******************************************************************************");
            Console.ReadKey();
        }
    }
}
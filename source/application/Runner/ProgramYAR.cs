namespace Runner
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
            ILogManager log = new Logger();
            while (ie.MoveNext())
            {
                IndividualTestState its = ie.Current as IndividualTestState;
                string formattedScenario = string.Format("\n********************************************************************************\nScenario Namespace {0}\nScenario Name {1}\nExecution Time {2}\nAttributes:  ",
                    its.NameSpace, its.TestName, its.ExecTime);
                log.Info(formattedScenario);
                Console.Write(formattedScenario);
                foreach (string s in its.Attributes)
                {
                    string formattedAttr = string.Format("{0}   ", s.Trim('"'));
                    log.Info(formattedAttr);
                    Console.Write(formattedAttr);
                }
                string formattedResult = string.Format("\nResult {0}", its.Result);
                log.Info(formattedResult);
                Console.WriteLine(formattedResult);
                if (its.ThrownException != null)
                {
                    string formattedException = string.Format("\n--------------------\n{0}\n--------------------\n",its.ExceptionMessageStackTrace);
                    log.Error(formattedException);
                    Console.Write(formattedException);
                }
            }

            IEnumerator fie = GlobalTestStates.featureState.GetEnumerator();
            while (fie.MoveNext())
            {
                IndividualFeatureTestState itfs = fie.Current as IndividualFeatureTestState;
                string formattedFeature = string.Format("\n********************************************************************************\nFeature Namespace {0}\nExecution Time {1}\n", itfs.FeatureName, itfs.FeatureExecutionTime);
                log.Info(formattedFeature);
                Console.Write(formattedFeature);
            }

            string formattedTrollTime = string.Format("\n*******************************************************************************\nTroll Time: {0}\n*******************************************************************************\nAll available tests are completed\n*******************************************************************************\n", GlobalTestStates.GrandExecTime);
            log.Info(formattedTrollTime);
            Console.Write(formattedTrollTime);
            Console.ReadKey();
        }
    }
}
namespace Runner
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Build.Construction;
    using TestPipe.Common;

    class ProgramYAR
    {
        static void Main(string[] args)
        {
            // test_nunit_test to test
            string actualPath = @"C:\_Automation\test\SunGard.PNE.Test.sln";
            string slnDirPath = Path.GetDirectoryName(actualPath) + @"\";
            Executor exe = new Executor();
            Solution sln = new Solution(actualPath);
            string findTestProj = "specs";
            List<DllInfo> addRefList = new List<DllInfo>();
            foreach (SolutionProject sp in sln.Projects)
            {
                if (!Regex.IsMatch(sp.ProjectName, "SunGard.PNE.Test.*"))
                    continue;
                string[] pName = sp.ProjectName.Split('.');
                string projectType = pName.Last<string>();
                Array.Resize(ref pName, pName.Length - 1);
                string projectName = pName.Last<string>();
                if (Convert.ToBoolean(args.Length))
                    if (!args.Contains(projectName.ToLower()))
                        continue;
                if (StringComparer.OrdinalIgnoreCase.Equals(projectType, findTestProj))
                {
                    DllInfo info = new DllInfo();
                    info.name = sp.ProjectName;
                    info.path = slnDirPath + Path.GetDirectoryName(sp.RelativePath) + @"\bin\debug\" + sp.RelativePath.Split('\\').Last<string>().Replace(".csproj",".dll");
                    addRefList.Add(info);
                }       
            }
            new StepLoader(addRefList);
            exe.ExcecuteTest(addRefList);
            IEnumerator ie = GlobalTestStates.manageState.GetEnumerator();
            ILogManager log = new Logger();
            while (ie.MoveNext())
            {
                IndividualTestState its = ie.Current as IndividualTestState;
                string formattedScenario = string.Format("********************************************************************************\nScenario Namespace {0}\nScenario Name {1}\nExecution Time {2}\nAttributes:  ",
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
                Console.WriteLine(formattedResult);
                if (its.ThrownException != null)
                {
                    string formattedException = string.Format("--------------------\n{0}\n--------------------\n",its.ExceptionMessageStackTrace);
                    log.Info(formattedException);
                    Console.Write(formattedException);
                }
            }

            IEnumerator fie = GlobalTestStates.featureState.GetEnumerator();
            while (fie.MoveNext())
            {
                IndividualFeatureTestState itfs = fie.Current as IndividualFeatureTestState;
                string formattedFeature = string.Format("********************************************************************************\nFeature Namespace {0}\nExecution Time {1}\n", itfs.FeatureName, itfs.FeatureExecutionTime);
                Console.Write(formattedFeature);
            }

            string formattedTrollTime = string.Format("*******************************************************************************\nTroll Time: {0}\n*******************************************************************************\nAll available tests are completed\n*******************************************************************************\n", GlobalTestStates.GrandExecTime);
            log.Info(formattedTrollTime);
            Console.Write(formattedTrollTime);
            Console.ReadKey();
        }
    }
}
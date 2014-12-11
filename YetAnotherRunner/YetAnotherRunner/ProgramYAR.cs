namespace YetAnotherRunner
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

    class ProgramYAR
    {
        static void Main(string[] args)
        {
            string actualPath = @"C:\_Automation\test_nunit_test\SunGard.PNE.Test.sln";
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
            //Console.WriteLine("Total Execution time in Parallel: {0}", GlobalTestStates.TotalExecP());
            Console.ReadKey();
        }
    }
}
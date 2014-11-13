namespace YetAnotherRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
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
                string[] pName = sp.ProjectName.Split('.');
                if (StringComparer.OrdinalIgnoreCase.Equals(pName.Last<string>(), findTestProj))
                {
                    DllInfo info = new DllInfo();
                    info.name = sp.ProjectName;
                    info.path = slnDirPath + Path.GetDirectoryName(sp.RelativePath) + @"\bin\debug\" + sp.RelativePath.Split('\\').Last<string>().Replace(".csproj",".dll");
                    addRefList.Add(info);
                }       
            }
            new StepLoader(addRefList);
            exe.ExcecuteTest(addRefList);
            Console.ReadKey();
        }
    }
}


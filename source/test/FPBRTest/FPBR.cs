using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
namespace FPBRTest
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Build.Construction;
    using Microsoft.Build.Evaluation;
    using StepBinder;
    using Runner;

    [TestClass]
    public class FPBR
    {
        public FPBR()
        {
            string actualPath = @"FPBRTest.sln";
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
                    info.path = slnDirPath + Path.GetDirectoryName(sp.RelativePath) + @"\bin\debug\" + sp.RelativePath.Split('\\').Last<string>().Replace(".csproj", ".dll");
                    addRefList.Add(info);
                }
            }
            new StepLoader(addRefList);
        }

        [TestMethod]
        public void StepLoaderTest()
        {
            CustomerSiteLogin csl = new CustomerSiteLogin();
            csl.LoginWithEmptyPassword();
            csl.LoginWithEmptyPasswordAndInvalidPassword();
        }

        [TestMethod]
        public void KillTasks()
        {
            Process p = Process.Start("TaskKiller.bat");
            p.WaitForExit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepBinder;
using YetAnotherRunner;


namespace FPBRTest
{
    [TestClass]
    public class FPBR
    {
        public FPBR()
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
            CustomerSiteLogin csl1 = new CustomerSiteLogin();
            //csl.LoginWithEmptyPassword();
            //csl1.LoginWithEmptyPasswordAndInvalidPassword();
            Task t1 = Task.Factory.StartNew(() => csl.LoginWithEmptyPassword());
            Task t2 = Task.Factory.StartNew(() => csl.LoginWithEmptyPasswordAndInvalidPassword());
            Task.WaitAll(t1, t2);
        }
    }
}

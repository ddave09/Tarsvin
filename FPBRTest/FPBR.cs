using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using StepBinder;
using Runner;


namespace FPBRTest
{
    [TestClass]
    public class FPBR
    {
        public FPBR()
        {
            //TODO: Pass solution file path
            string actualPath = @"SolutionFile";
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
            //Project p = new Project(@"C:\_Automation\test_nunit_test\source\application\SunGard.PNE.Test.CustomerSite.Specs\SunGard.PNE.Test.CustomerSite.Specs.csproj");
            //var itemsT = p.ItemTypes;
            //var items = p.GetItems("Compile");
            //p.AddItem("Compile", @"Features\removeimmediately.cs");
            //ICollection<ProjectItem> itemsAfter = p.GetItems("Compile");
            //List<ProjectItem> pis = new List<ProjectItem>();
            //foreach (ProjectItem pi in itemsAfter)
            //{
            //    if (StringComparer.OrdinalIgnoreCase.Equals(pi.EvaluatedInclude, @"features\removeimmediately.cs"))
            //    {
            //        pis.Add(pi);
            //    }

            //}
            //p.RemoveItems(pis.AsEnumerable());
            //items = p.GetItems("Compile");
            //p.Save(@"C:\_Automation\test_nunit_test\source\application\SunGard.PNE.Test.CustomerSite.Specs\SunGard.PNE.Test.CustomerSite.Specs.csproj");

            CustomerSiteLogin csl = new CustomerSiteLogin();
            //CustomerSiteLogin csl1 = new CustomerSiteLogin();
            csl.LoginWithEmptyPassword();
            csl.LoginWithEmptyPasswordAndInvalidPassword();
            //Task t1 = Task.Factory.StartNew(() => csl.LoginWithEmptyPassword());
            //Task t2 = Task.Factory.StartNew(() => csl.LoginWithEmptyPasswordAndInvalidPassword());
            //Task.WaitAll(t1, t2);
        }

        [TestMethod]
        public void KillTasks()
        {
            Process p = Process.Start("TaskKiller.bat");
            p.WaitForExit();
            //ProcessStartInfo ps = new ProcessStartInfo("TaskKiller.bat");
            //ps.UseShellExecute = false;
            //try
            //{
            //    Process p = Process.Start(ps);
            //    p.WaitForExit();
            //}
            //catch (Exception e)
            //{
            //    string x = e.StackTrace;
            //    string m = e.Message;
            //}
            
        }
    }
}

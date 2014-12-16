using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixtureParser
{
    class ProgramFP
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\_Automation\test_nunit_test\source\application\SunGard.PNE.Test.CustomerSite.Specs\Features\1_Login.feature";
            string filePath1 = @"C:\_Automation\test_nunit_test\source\application\SunGard.PNE.Test.CustomerSite.Specs\Features\Customer\9_MyProfile.feature";
            string projectName = "CustomerSite";
            string rmAddPath = filePath1.Substring(filePath1.IndexOf("Features"));
            Parser ps = new Parser();
            ps.Parse(filePath, projectName, rmAddPath);
            Console.ReadKey();
        }
    }
}

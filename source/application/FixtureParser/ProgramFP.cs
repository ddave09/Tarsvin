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
            // test_nunit_test to test
            string filePath = @"C:\_Automation\test\source\application\SunGard.PNE.Test.CustomerSite.Specs\Features\Customer\Jobs\BatchDetail.feature";
            string projectName = "CustomerSite";
            string rmAddPath = filePath.Substring(filePath.IndexOf("Features"));
            Parser ps = new Parser();
            ps.Parse(filePath, projectName, rmAddPath);
            Console.ReadKey();
        }
    }
}

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
            //TODO: Change "Path to feature file" string with actual path
            string filePath = @"Path to feature file";
            string filePath1 = @"Path to feature file";
            //TODO: Configured for internal use make changes accordingly.
            string projectName = "ProjectName";
            string rmAddPath = filePath1.Substring(filePath1.IndexOf("Features"));
            Parser ps = new Parser();
            ps.Parse(filePath, projectName, rmAddPath);
            Console.ReadKey();
        }
    }
}

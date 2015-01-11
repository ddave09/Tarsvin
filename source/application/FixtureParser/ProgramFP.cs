namespace FixtureParser
{
    using System;

    internal class ProgramFP
    {
        private const string FeatureFolder = "Features";
        private const string ProjectExtension = "csproj";

        private static void Main(string[] args)
        {
            Options options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.Error.Write("Invalid arguments.");
                return;
            }

            //@"C:\_Automation\test\source\application\CustomerSite.Specs\Features\Customer\MyProfile.feature"
            string filePath = options.File;

            //"CustomerSite.Specs"
            string projectName = options.Project;
            string rootProjectPath = filePath.Substring(0, filePath.IndexOf(FeatureFolder));
            string projectPath = string.Format("{0}{1}.{2}", rootProjectPath, projectName, ProjectExtension);
            string fileName = filePath.Substring(filePath.IndexOf(FeatureFolder));

            Parser ps = new Parser();
            ps.Parse(filePath, projectPath, fileName);
            Console.ReadKey();
        }
    }
}
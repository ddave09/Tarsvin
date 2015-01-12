namespace FixtureParser
{
    using System;
    using System.IO;

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

            string filePath = options.File;
            bool multiple = options.Multiple;

           string projectName = options.Project;
            string rootProjectPath = filePath.Substring(0, filePath.IndexOf(FeatureFolder));
            string projectPath = string.Format("{0}{1}.{2}", rootProjectPath, projectName, ProjectExtension);
            string fileName = filePath.Substring(filePath.IndexOf(FeatureFolder));

            if (multiple)
            {
                foreach (string file in Directory.EnumerateFiles(rootProjectPath, "*.feature", SearchOption.AllDirectories))
                {
                    Parser ps = new Parser();
                    fileName = filePath.Substring(filePath.IndexOf(FeatureFolder));
                    ps.Parse(file, projectPath, fileName);
                }
            }
            else
            {
                Parser ps = new Parser();
                ps.Parse(filePath, projectPath, fileName);
            }

            Console.ReadKey();
        }
    }
}
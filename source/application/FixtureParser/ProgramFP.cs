namespace Tarsvin.FixtureParser
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	internal class ProgramFP
	{
		private const string FeatureFolder = "Features";

		private static void Main(string[] args)
		{
			Options options = new Options();

			if (!CommandLine.Parser.Default.ParseArguments(args, options))
			{
				Console.Error.Write("Invalid arguments.");
				return;
			}

			//-f "C:\_Tarsvin - Copy\source\test\FPBRTest\Features\Search.feature" -p FPBRTest.Specs -m
			string filePath = options.File;
			bool multiple = options.Multiple;
			string projectName = options.Project;

			//C:\_Tarsvin - Copy\source\test\FPBRTest\
			string rootProjectPath = string.Empty;
			try
			{
				rootProjectPath = filePath.Substring(0, filePath.IndexOf(ProgramFP.FeatureFolder));
			}
			catch (Exception e)
			{
				Console.WriteLine("File path must contain \"Features\" sub direcotory");
				System.Environment.Exit(0);
			}

			// User must pass full project name including project extension
			//C:\_Tarsvin - Copy\source\test\FPBRTest\FPBRTest.Specs.csproj
			string projectPath = string.Format("{0}{1}", rootProjectPath, projectName);

			if (multiple)
			{
				foreach (string file in Directory.EnumerateFiles(rootProjectPath, "*.feature", SearchOption.AllDirectories))
				{
					string fileName = file.Substring(filePath.IndexOf(ProgramFP.FeatureFolder));
					Parser ps = new Parser();
					ps.Parse(file, projectPath, fileName);
				}
			}
			else
			{
				//Features\Search.feature
				string fileName = filePath.Substring(filePath.IndexOf(ProgramFP.FeatureFolder));
				Parser ps = new Parser();
				ps.Parse(filePath, projectPath, fileName);
			}
		}
	}
}
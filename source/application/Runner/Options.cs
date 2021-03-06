﻿namespace Tarsvin.Runner
{
	using System;
	using System.Text;
	using CommandLine;
	using CommandLine.Text;

	public class Options
	{
		[Option('d', "project", Required = true, HelpText = "If project dll in not passed current project will be used")]
		public string Project { get; set; }

		[Option('n', "runner", Required = true, HelpText = "Sequential or Parallel")]
		public string RunnerSelection { get; set; }

		[Option('x', "result", Required = true, HelpText = "xml output file path")]
		public string XmlResultPath { get; set; }

		[Option("IS", Required = false, HelpText = "Include scenario comma separated tags without white spaces")]
		public string IncludeS { get; set; }

		[Option("ES", Required = false, HelpText = "Exclude scenario comma sperated tags without white spaces")]
		public string ExcludeS { get; set; }

		[Option("IF", Required = false, HelpText = "Include feature comma separated tags without white spaces")]
		public string IncludeF { get; set; }

		[Option("EF", Required = false, HelpText = "Exclude feature comma sperated tags without white spaces")]
		public string ExcludeF { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			var help = new HelpText
			{
				Heading = new HeadingInfo("Tarsvin.Runner", "1.1.0"),
				Copyright = new CopyrightInfo("Darshit Dave", 2014),
				AdditionalNewLineAfterOption = true,
				AddDashesToOption = true
			};
			help.AddPreOptionsLine("");
			help.AddPreOptionsLine("Ussage: > Runner.exe -d <project dll file> -n <Runner Option> -x <xml result file path>" + 
				"--IF <comma separated tags to include features> --IS <comma separated tags to include scenarios> --EF <comma separated tags to exclude features> --ES <comma separated tags to exclude scenarios>");
			help.AddOptions(this);
			return help;
		}
	}
}
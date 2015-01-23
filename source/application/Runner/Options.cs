namespace Tarsvin.Runner
{
    using System;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;

    public class Options
    {
        [Option('r', "prefix", Required = false, HelpText = "Project name prefix for tests to run. If provided, test project names must begin with this text in order to run.")]
        public string ProjectPrefix { get; set; }

        [OptionArray('p', "projects", Required = false, HelpText = "Space seperated list of test project names to run.")]
        public string[] Projects { get; set; }

        [Option('u', "suffix", Required = false, HelpText = "Project name suffix for tests to run. If provided, test project names must end with this text in order to run.")]
        public string ProjectSuffix { get; set; }

        [Option('s', "solution", Required = true, HelpText = "Path to solution file containing test projects to run.")]
        public string SolutionFile { get; set; }

        [Option('r', "runner", Required = true, HelpText = "Sequential or Parallel")]
        public string RunnerSelection { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("Tarsvin.Runner", "0.0.1"),
                Copyright = new CopyrightInfo("Darshit Dave", 2014),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Usage: > Runner.exe -s Someone.sln -r Parallel");
            help.AddOptions(this);
            return help;
        }
    }
}
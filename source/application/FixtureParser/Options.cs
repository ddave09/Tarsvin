namespace FixtureParser
{
    using System;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;

    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "Path to feature file.")]
        public string File { get; set; }

        [Option('p', "project", Required = true, HelpText = "Name of project containing feature file.")]
        public string Project { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("Tarsvin.FixtureParser", "0.0.1"),
                Copyright = new CopyrightInfo("Darshit Dave", 2014),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine(@"Usage: > FixtureParser.exe -f ""C:\test\Site.Specs\Features\Customer\MyProfile.feature"" -p ""Site.Specs""");
            help.AddOptions(this);
            return help;
        }
    }
}

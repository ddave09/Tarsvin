namespace Tarsvin.FPBRTest.Specs.Features
{
using System;
using Tarsvin.StepBinder;

[Tarsvin.CustomAttributes.FixtureAttr("FPBRTest")]
[Tarsvin.CustomAttributes.FixtureAttr("Search")]
public class FPBRTestSearch
{
	public FPBRTestSearch()
	{
		FunctionBinder.CallBeforeFeature("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch");
	}

	[Tarsvin.CustomAttributes.FixtureEndAttr()]
	public void FeatureTearDown()
	{
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch");
	}

	[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
	[Tarsvin.CustomAttributes.CaseAttr("Search")]
	[Tarsvin.CustomAttributes.CaseAttr("Smoke")]
	public void SearchForTestPipe()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "1. Search for TestPipe", "FPBRTest.Search.Smoke");
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", obj);
	}
}
}
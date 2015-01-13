namespace Tarsvin.FPBRTest.Specs.Features
{
using System;
using StepBinder;

[CustomAttributes.FixtureAttr("FPBRTest")]
[CustomAttributes.FixtureAttr("Search")]
public class FPBRTestSearch
{
	public FPBRTestSearch()
	{
		FunctionBinder.CallBeforeFeature("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch");
	}

	[CustomAttributes.FixtureEndAttr()]
	public void FeatureTearDown()
	{
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch");
	}

	[CustomAttributes.CaseAttr("FPBRTest")]
	[CustomAttributes.CaseAttr("Search")]
	[CustomAttributes.CaseAttr("Smoke")]
	public void SearchForTestPipe()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "1. Search for TestPipe", "FPBRTest.Search.Smoke");
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.When("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "WhenISubmitASearch", obj);
		FunctionBinder.Then("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "ThenResultsShouldBeDisplayed", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", obj);
	}

	[CustomAttributes.CaseAttr("FPBRTest")]
	[CustomAttributes.CaseAttr("Search")]
	public void SearchForSomething()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "2. Search for Something", "FPBRTest.Search");
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.When("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "WhenISubmitASearch", obj);
		FunctionBinder.Then("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "ThenResultsShouldBeDisplayed", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", obj);
	}
}
}
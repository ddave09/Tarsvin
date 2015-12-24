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

	public void FeatureBackground(Object obj)
	{
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "GivenBackgroundTest", obj);
	}

	[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
	[Tarsvin.CustomAttributes.CaseAttr("Search")]
	[Tarsvin.CustomAttributes.CaseAttr("Smoke")]
	public void SearchForTestPipe()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "1. Search for TestPipe", "FPBRTest.Search.Smoke");
		this.FeatureBackground(obj);
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.When("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "WhenISubmitASearch", obj);
		FunctionBinder.Then("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "ThenResultsShouldBeDisplayed", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", obj);
	}

	[Tarsvin.CustomAttributes.CaseAttr("FPBRTest")]
	[Tarsvin.CustomAttributes.CaseAttr("Search")]
	public void SearchForSomething()
	{
		Object obj= FunctionBinder.CallBeforeScenario("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "2. Search for Something", "FPBRTest.Search");
		this.FeatureBackground(obj);
		FunctionBinder.Given("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "GivenIAmOnTheSearchPage", obj);
		FunctionBinder.When("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "WhenISubmitASearch", obj);
		FunctionBinder.Then("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", "ThenResultsShouldBeDisplayed", obj);
		FunctionBinder.CallAfterX("Tarsvin.FPBRTest.Specs.Steps", "", "FPBRTestSearch", obj);
	}
}
}
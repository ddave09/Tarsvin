namespace FPBRTest
{
using System;
using StepBinder;

[CustomAttributes.FixtureAttr("CustomerSite")]
[CustomAttributes.FixtureAttr("CustomerSiteLogin")]
public class CustomerSiteLogin
{
	public CustomerSiteLogin()
	{
		FunctionBinder.CallBeforeX("CustomerSite", "", "CustomerSiteLogin", "feature");
	}

	[CustomAttributes.FixtureEndAttr()]
	public void FeatureTearDown()
	{
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", "feature");
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithEmptyPassword()
	{
		FunctionBinder.CallBeforeX("CustomerSite", "", "CustomerSiteLogin", "4. Login with empty password", "CustomerSite.CustomerSiteLogin", "scenario");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterInvalidUsername");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterEmptyPassword");
		FunctionBinder.When("CustomerSite", "", "CustomerSiteLogin", "WhenILogin");
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldSeeADeniedErrorMessage");
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", "scenario");
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithEmptyUsername()
	{
		FunctionBinder.CallBeforeX("CustomerSite", "", "CustomerSiteLogin", "5. Login with empty username", "CustomerSite.CustomerSiteLogin", "scenario");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterEmptyUsername");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterInvalidPassword");
		FunctionBinder.When("CustomerSite", "", "CustomerSiteLogin", "WhenILogin");
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldSeeADeniedErrorMessage");
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", "scenario");
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithEmptyUsernameAndInvalidPassword()
	{
		FunctionBinder.CallBeforeX("CustomerSite", "", "CustomerSiteLogin", "6. Login with empty username and invalid password", "CustomerSite.CustomerSiteLogin", "scenario");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterEmptyUsername");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterInvalidPassword");
		FunctionBinder.When("CustomerSite", "", "CustomerSiteLogin", "WhenILogin");
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldSeeADeniedErrorMessage");
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", "scenario");
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithEmptyPasswordAndInvalidPassword()
	{
		FunctionBinder.CallBeforeX("CustomerSite", "", "CustomerSiteLogin", "7. Login with empty password and invalid password", "CustomerSite.CustomerSiteLogin", "scenario");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterEmptyUsername");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIEnterInvalidPassword");
		FunctionBinder.When("CustomerSite", "", "CustomerSiteLogin", "WhenILogin");
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldSeeADeniedErrorMessage");
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", "scenario");
	}

	[CustomAttributes.CaseAttr("CustomerSite")]
	[CustomAttributes.CaseAttr("CustomerSiteLogin")]
	public void LoginWithTooManyInvalidAttempts()
	{
		FunctionBinder.CallBeforeX("CustomerSite", "", "CustomerSiteLogin", "8. Login with too many invalid attempts", "CustomerSite.CustomerSiteLogin", "scenario");
		FunctionBinder.Given("CustomerSite", "", "CustomerSiteLogin", "GivenIAmOnTheLoginPage");
		FunctionBinder.And("CustomerSite", "", "CustomerSiteLogin", "GivenIAttemptToLoginWithTooManyInvalidAttempts");
		FunctionBinder.Then("CustomerSite", "", "CustomerSiteLogin", "ThenIShouldBeLockedOut");
		FunctionBinder.CallAfterX("CustomerSite", "", "CustomerSiteLogin", "scenario");
	}
}
}
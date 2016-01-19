namespace Tarsvin.FPBRTest.Specs.Steps
{
	using System;
	using TestPipe.Assertions;
	using TestPipe.Core;
	using TestPipe.Core.Page;
	using TestPipe.Core.Session;
	using Tarsvin.FPBRTest.Specs.Pages;
	using TechTalk.SpecFlow;
	using TestPipe.Runner;

	[Binding]
	public class FPBRTestSearchSteps
	{
		private static SessionFeature feature;
		private BasePage resultPage;
		private SessionScenario scenario;
		private string searchText;
		private SearchPage sut;

		#region Tarsvin friends
		[Tarsvin.CustomAttributes.BeforeFeature]
		public static void SetupFeature(string[] tags, string title)
		{
			feature = StepHelper.SetupFeature(tags, title);
		}

		[Tarsvin.CustomAttributes.BeforeScenario]
		public void SetupScenario(string[] tags, string fTitle, string sTitle)
		{
			scenario = StepHelper.SetupScenario(tags, fTitle, sTitle);
			this.sut = new SearchPage(this.scenario.Browser, TestSession.Environment);
		}

		[Tarsvin.CustomAttributes.AfterScenario]
		public void TearDownScenario()
		{
			StepHelper.TearDownScenario(scenario);
		}

		[Tarsvin.CustomAttributes.AfterFeature]
		public static void TearDownFeatureS()
		{
			RunnerBase.TeardownSuite();
		}
		#endregion

		[BeforeFeature("@Search")]
		public static void SetupFeature()
		{
			feature = StepHelper.SetupFeature();
		}

		[Given(@"I am on the search page")]
		public void GivenIAmOnTheSearchPage()
		{
			this.sut.Open();
		}

		[BeforeScenario("@Search")]
		public void SetupScenario()
		{
			this.scenario = StepHelper.SetupScenario();
			this.sut = new SearchPage(this.scenario.Browser, TestSession.Environment);
		}

		[AfterScenario("@Search")]
		public void TeardownScenario()
		{
			StepHelper.TearDownScenario(this.scenario);
		}

		[Given(@"Background test")]
		public void GivenBackgroundTest()
		{

		}

		[Then(@"results should be displayed")]
		public void ThenResultsShouldBeDisplayed()
		{
			string pageState = string.Format("Page Title: {0}, Browser Title: {1}, Page Url: {2}, Browser Page: {3}", this.resultPage.Title, this.resultPage.Browser.Title, this.resultPage.PageUrl, this.resultPage.Browser.Url);
			bool isOpen = this.resultPage.IsOpen();
			Asserts.IsTrue(isOpen, pageState);
		}

		[When(@"I submit a search")]
		public void WhenISubmitASearch()
		{
			this.searchText = this.scenario.Data.Q;

			this.sut.Search.TypeText(this.searchText);

			//this.sut.EnterText(this.sut.Search, this.searchText);

			this.sut = new SearchPage(this.scenario.Browser, TestSession.Environment);

			//this.resultPage = this.sut.Submit(this.searchText);
		}
	}
}

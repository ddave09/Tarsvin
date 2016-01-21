@FPBRTest @Search2
Feature: FPBRTest Search2

@FPBRTest @Search2 @Smoke
Scenario: 1. Search for TestPipe
	Given I am on the search page

@FPBRTest @Search2 @Smoke
Scenario: 2. Search for Something
	Given I am on the search page
	Then Fail
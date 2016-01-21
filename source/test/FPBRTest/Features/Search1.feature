@FPBRTest @Search1
Feature: FPBRTest Search1

@FPBRTest @Search1 @Smoke
Scenario: 1. Search for TestPipe
	Given I am on the search page

@FPBRTest @Search1 @Smoke
Scenario: 2. Search for Something
	Given I am on the search page
	Then Fail
@FPBRTest @Search
Feature: FPBRTest Search
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given Background test

@FPBRTest @Search @Smoke
Scenario: 1. Search for TestPipe
	Given I am on the search page
	When I submit a search
	Then results should be displayed

@FPBRTest @Search
Scenario: 2. Search for Something
	Given I am on the search page
	When I submit a search
	Then results should be displayed
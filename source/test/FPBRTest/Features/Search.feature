﻿@FPBRTest @Search
Feature: FPBRTest Search

@FPBRTest @Search @Smoke
Scenario: 1. Search for TestPipe
	Given I am on the search page

@FPBRTest @Search @Smoke
Scenario: 2. Search for Something
	Given I am on the search page
	Then Fail
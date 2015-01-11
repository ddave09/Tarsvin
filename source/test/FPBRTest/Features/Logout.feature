@CustomerSite @Logout
Feature: Customer Site Logout
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background:
	Given User is logged in to customer site

@CustomerSite @Logout @Smoke
Scenario: 1. User clicks on logout link
	When User clicks on logout link user goes to login page
Feature: DSICMainBreaks

Background: users exist
	Given a user "user" exists with username: "user"
	And a role "role" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user"

Scenario: user should be able to search and get results
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a work description "replace" exists with description: "water main break replace"
	And a work order "one" exists with work description: "replace", approved on: "today", operating center: "nj7"
	And a main break "one" exists with work order: "one", footage replaced: "4"
	And I am logged in as "user"
	When I visit the /FieldOperations/DSICMainBreaks/Search page
	And I press "Search"	
	Then I should see the following values in the results table
	| Operating Center | Work Order Number | Work Description         | Shutdown Time | Depth | Boil Alert Issued |
	| NJ7 - Shrewsbury | 1                 | WATER MAIN BREAK REPLACE | 5             | 4     | Yes               |
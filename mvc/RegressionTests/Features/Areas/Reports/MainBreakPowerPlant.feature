Feature: MainBreakPowerPlant
	
Background: users exist
	Given a user "user" exists with username: "user"
	And a role "role" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user"

Scenario: user should be able to search and get results
	Given a work description "replace" exists with description: "water main break replace"
	And a work order "one" exists with work description: "replace", approved on: "today"
	And a main break "one" exists with work order: "one", footage replaced: "4"
	And I am logged in as "user"
	When I visit the /Reports/MainBreakPowerPlant/Search page
	And I press "Search"
	Then I should see main break "one"'s FootageReplaced in the "Footage Installed" column
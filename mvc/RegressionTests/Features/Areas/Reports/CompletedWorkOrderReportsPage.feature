Feature: CompletedWorkOrderReportsPage

Background: users exist
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood"
	And a role "rolenj4" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
	And a role "rolenj7" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"

Scenario: user should be able to load the search page and see operating centers
	Given a work description "replace" exists with description: "water main break replace" 
	And a work order "one" exists with work description: "replace", approved on: "today"
	And a main break "one" exists with work order: "one", footage replaced: "4"
	And I am logged in as "user"
	When I visit the /Reports/CompletedWorkOrdersWithPreJobSafetyBriefs/Search page
	Then I should see operating center "nj7"'s Description in the OperatingCenter dropdown
	And I should see operating center "nj4"'s Description in the OperatingCenter dropdown
	When I visit the /Reports/CompletedWorkOrdersWithMaterial/Search page
	Then I should see operating center "nj7"'s Description in the OperatingCenter dropdown
	And I should see operating center "nj4"'s Description in the OperatingCenter dropdown
	When I visit the /Reports/CompletedWorkOrdersWithMarkout/Search page
	Then I should see operating center "nj7"'s Description in the OperatingCenter dropdown
	And I should see operating center "nj4"'s Description in the OperatingCenter dropdown
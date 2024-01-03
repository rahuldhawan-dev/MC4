Feature: Completed Work Orders

Background:
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town "aberdeen" exists with name: "Aberdeen", , operating center: "nj7"
	And operating center "nj7" exists in town "aberdeen"
	And a role "read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a crew "one" exists with description: "one", operating center: "nj7"
	And a work order "one" exists with operating center: "nj7", town: "aberdeen", date received: "5/2/2023 07:00:00", date completed: "5/2/2023 13:00:00", approved on: "5/2/2023 15:00:00", materials approved on: "5/2/2023 19:00:00"
	And a crew assignment "one" exists with work order: "one", crew: "one", date started: "5/2/2023 08:00:00", date ended: "5/2/2023 11:00:00", employees on job: 3
	And a work order "two" exists with operating center: "nj7", town: "aberdeen", date received: "5/10/2023 07:00:00", date completed: "5/10/2023 13:00:00", approved on: "5/10/2023 15:00:00", materials approved on: "5/10/2023 19:00:00"
	And a crew assignment "two" exists with work order: "two", crew: "one", date started: "5/10/2023 08:00:00", date ended: "5/10/2023 11:00:00", employees on job: 3

Scenario: user can search the complete work orders report
	Given I am logged in as "user"
	And I am at the Reports/CompletedWorkOrders/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should see the following values in the results table
	| Operating Center | Town     |
	| NJ7 - Shrewsbury | Aberdeen |
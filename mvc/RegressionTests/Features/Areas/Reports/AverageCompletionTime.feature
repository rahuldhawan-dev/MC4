Feature: AverageCompletionTime
	
Background:
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a crew "one" exists with description: "one", operating center: "nj7"
	And a work order "one" exists with operating center: "nj7", date received: "5/2/2023 07:00:00", date completed: "5/2/2023 13:00:00", approved on: "5/2/2023 15:00:00", materials approved on: "5/2/2023 19:00:00"
	And a crew assignment "one" exists with work order: "one", crew: "one", date started: "5/2/2023 08:00:00", date ended: "5/2/2023 11:00:00", employees on job: 3
	And a work order "two" exists with operating center: "nj7", date received: "5/10/2023 07:00:00", date completed: "5/10/2023 13:00:00", approved on: "5/10/2023 15:00:00", materials approved on: "5/10/2023 19:00:00"
	And a crew assignment "two" exists with work order: "two", crew: "one", date started: "5/10/2023 08:00:00", date ended: "5/10/2023 11:00:00", employees on job: 3
	
Scenario: user can search average completion time report
	Given I am logged in as "user"
	And I am at the Reports/AverageCompletionTime/Search page
	When I press Search
	Then I should see the validation message "The StartDate field is required." 
	And I should see the validation message "The EndDate field is required." 
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "5/1/2023" into the StartDate field
	And I enter "5/5/2023" into the EndDate field
	And I press Search
	Then I should see the following values in the results table
	| Operating Center | Average Time to Complete (hrs) | Average Man Hours to Complete (hrs) | Average time to Approve (hrs) | Average time to issue stock (hrs) |
	| NJ7 - Shrewsbury | 6.00                           | 9.00                                | 2.00                          | 4.00                              |
Feature: IncompleteLeaks

Background:
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town "aberdeen" exists with name: "Aberdeen", , operating center: "nj7"
	And operating center "nj7" exists in town "aberdeen"
	And a work description "hydrant leaking" exists with description: "hydrant leaking", id: "68"
	And a role "read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"	
	And a work order "one" exists with operating center: "nj7", town: "aberdeen", date received: "5/2/2023 07:00:00", approved on: "5/2/2023 15:00:00", materials approved on: "5/2/2023 19:00:00", work description: "hydrant leaking"
	
@headful
Scenario: user can search the incomplete leaks report
	Given I am logged in as "user"
	And I am at the Reports/IncompleteLeaks/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search	
	Then I should see the following values in the results table
	| Order Number | Town     | Description of Job (Hover for Notes) | Job Priority |
	| 1            | Aberdeen | hey this is a note HYDRANT LEAKING   | Routine      |
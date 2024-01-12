Feature: IncompleteWorkOrdersReport

Background:
	Given a user "user" exists with username: "user"
	And an operating center "one" exists with opcode: "NJ7", name: "OP1"	
	And an operating center "two" exists with opcode: "NJ5", name: "OP2"
	And an operating center "three" exists with opcode: "NJ3", name: "OP3"
	And a town "town1" exists with name: "Aberdeen", , operating center: "one"	
	And a town "town2" exists with name: "Demo", , operating center: "two"
	And a town "town3" exists with name: "Demo1", , operating center: "three"
	And operating center "one" exists in town "town1"	
	And operating center "two" exists in town "town2"	
	And operating center "three" exists in town "town3"	
	And a work description "hydrant leaking" exists with description: "hydrant leaking", id: "68"	
	And a work description "hydrant investigation" exists with description: "hydrant investigation"
	And a role "read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "one"	
	And a work order "one" exists with operating center: "one", town: "town1", date received: "5/2/2023 07:00:00", approved on: "5/2/2023 15:00:00", materials approved on: "5/2/2023 19:00:00", work description: "hydrant leaking"
	And a work order "two" exists with operating center: "two", town: "town2", date received: "5/2/2023 07:00:00", approved on: "5/2/2023 15:00:00", materials approved on: "5/2/2023 19:00:00", work description: "hydrant leaking"
	And a work order "three" exists with operating center: "one", town: "town2", date received: "5/2/2023 07:00:00", approved on: "5/2/2023 15:00:00", materials approved on: "5/2/2023 19:00:00", work description: "hydrant leaking"
	And a work order "four" exists with operating center: "three", town: "town2", date received: "5/2/2023 07:00:00", approved on: "5/2/2023 15:00:00", materials approved on: "5/2/2023 19:00:00", work description: "hydrant leaking"
	
@headful
Scenario: user can search the incomplete workOrders report
	Given I am logged in as "user"
	And I am at the Reports/IncompleteWorkOrders/Search page
	When I select work description "hydrant leaking" from the WorkDescription dropdown
	And I press Search	
	Then I should see the following values in the results table
	| Work Order Number | 
	| 1				    | 
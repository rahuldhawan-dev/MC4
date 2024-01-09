Feature: CrewPage

Background: admin user exists
	Given an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
	And a state "NJ" exists with abbreviation: "NJ"
	And a town "Swedesboro" exists with state: "NJ"	
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "NJ", mapId: "01d4ebf78acc489695b930d5bc2850f3"	
	And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-edit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a work description "hydrant installation" exists with description: "hydrant installation"
	And a work order "one" exists with operating center: "nj7", work description: "hydrant installation"
	And a crew "one" exists with description: "one", availability: "8", operating center: "nj7", active: true	
	And a crew "two" exists with description: "one", availability: "8", operating center: "nj7", active: false	
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
    And a crew assignment "ca1" exists with work order: "one", crew: "two", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	

Scenario: user can search the crew with active status
	Given I am logged in as "user"
	And I am at the FieldOperations/Crew/Search page
	When I select state "NJ" from the State dropdown	
	And I select operating center "nj7" from the OperatingCenter dropdown		 
	And I enter "one" into the Description field	
	And I check the Active field
	And I press Search	
	Then I should see the following values in the results table
	| Crew Name | Availability (hours) | Operating Center | Active |
	| one       | 8.00                 | NJ7 - Shrewsbury | yes    |

@headful
Scenario: user can search the crew with both status
	Given I am logged in as "user"
	And I am at the FieldOperations/Crew/Search page
	When I select state "NJ" from the State dropdown	
	And I select operating center "nj7" from the OperatingCenter dropdown		 
	And I enter "one" into the Description field
	And I press Search	
	Then I should see the following values in the results table
	| Crew Name | Availability (hours) | Operating Center | Active |
	| one       | 8.00                 | NJ7 - Shrewsbury | yes    |
	| one       | 8.00                 | NJ7 - Shrewsbury | No     |
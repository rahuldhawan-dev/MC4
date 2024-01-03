Feature: CompletedWorkOrdersWithMarkout

Scenario: Links to the work order with markout search results should work
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an operating center "nj4" exists with opcode: "NJ4", name: "NJ4bury"
	And a user "user" exists with username: "user"
	And a role "workorder-read-nj7" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-read-nj4" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
	And a contractor "one" exists
	And work descriptions exist
	And markout requirements exist
	And a markout requirement "one" exists with description: "none"
	And a markout requirement "two" exists with description: "routine"
	And a work order "wo-good-1" exists with operating center: "nj7", approved by: "user", approved on: "10/31/2018", date completed: "7/15/2019", work description: "hydrant flushing", markout requirement: "routine"
	And a work order "wo-good-2" exists with operating center: "nj7", approved by: "user", date completed: "7/15/2019", work description: "hydrant flushing", markout requirement: "none"
	And a work order "wo-good-3" exists with operating center: "nj7", approved by: "user", date completed: "7/16/2019", work description: "hydrant flushing", markout requirement: "emergency"
	And a work order "wo-good-4" exists with operating center: "nj7", approved by: "user", date completed: "7/16/2019", work description: "hydrant flushing", markout requirement: "none"
	And a work order "wo-good-5" exists with operating center: "nj7", approved by: "user", date completed: "7/16/2019", work description: "hydrant flushing", markout requirement: "none"
	And a work order "wo-good-6" exists with operating center: "nj7", approved by: "user", date completed: "7/16/2019", work description: "hydrant flushing", markout requirement: "routine"
	And I am logged in as "user"
	When I visit the Reports/CompletedWorkOrdersWithMarkout/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "7/15/2019" into the DateCompleted_Start field
	And I enter "7/16/2019" into the DateCompleted_End field
	And I press Search
	Then I should see the following values in the search-results table
	| Work Description | Total Orders | Markout None | Markout Routine | Markout Emergency |
	| HYDRANT FLUSHING | 6            | 3            | 2               | 1                 |
	When I follow "6"
	Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-1"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-2"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-3"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-4"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-5"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-6"
	When I go back
	And I follow "3" 
	Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-2"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-4"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-5"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-1"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-6"
	When I go back
	And I follow "2" 
	Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-1"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-6"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-2"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-3"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-4"
	When I go back
	And I follow "1" 
	Then I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-1"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-2"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-3"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-4"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-5"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-6"
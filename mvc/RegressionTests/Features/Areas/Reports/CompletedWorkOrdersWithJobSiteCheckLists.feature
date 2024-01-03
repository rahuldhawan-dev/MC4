Feature: CompletedWorkOrdersWithJobSiteCheckLists

Scenario: Links to the work order search results should work
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an operating center "nj4" exists with opcode: "NJ4", name: "NJ4bury"
	And a user "user" exists with username: "user"
	And a role "workorder-read-nj7" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-read-nj4" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
	And a contractor "one" exists
	And work descriptions exist
	And a work order "wo-good-1" exists with operating center: "nj7", approved by: "user", approved on: "10/31/2018", date completed: "7/15/2019", work description: "hydrant flushing"
	And a work order "wo-good-2" exists with operating center: "nj7", approved by: "user", date completed: "7/15/2019", work description: "hydrant flushing"
	And a work order "wo-good-3" exists with operating center: "nj7", approved by: "user", date completed: "7/15/2019", work description: "hydrant flushing"
	And a job site check list "one" exists with map call work order: "wo-good-1", has excavation: "true"
	And a job site check list "two" exists with map call work order: "wo-good-2", has excavation: "true"
	And I am logged in as "User"
	When I visit the Reports/CompletedWorkOrdersWithJobSiteCheckLists/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "7/15/2019" into the DateCompleted_Start field
	And I enter "7/15/2019" into the DateCompleted_End field
	And I press Search
	Then I should see the following values in the search-results table
	| Work Description | Total | With Job Site Check List | Without Job Site Check List |
	| HYDRANT FLUSHING | 3     | 2                       | 1                          |
	When I follow "3"
	Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-1"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-2"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-3"
	When I go back
	And I follow "2" 
	Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-1"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-2"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-3"
	When I go back
	And I follow "1" 
	Then I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-1"
	And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-2"
	And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "wo-good-3"
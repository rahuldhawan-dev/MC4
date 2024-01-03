Feature: CompletedWorkOrdersWithMaterials

Scenario: Links to the work order with completed materials search results should work
	Given an operating center "ca40" exists with opcode: "ca40", name: "Monterey", town: "seaside"	
	And a user "user" exists with username: "user"
	And a role "workorder-read-ca40" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "ca40"
	And work descriptions exist	
	And a work order "wo-good-1" exists with operating center: "ca40", approved by: "user", date completed: "2/12/2020", work description: "service line repair", has materials used: "True"
	And a work order "wo-good-2" exists with operating center: "ca40", approved by: "user", date completed: "2/12/2020", work description: "service line repair", has materials used: "True"
	And a work order "wo-good-3" exists with operating center: "ca40", approved by: "user", date completed: "2/12/2020", work description: "service line repair", has materials used: "False"
	And an material used "mat1" exists with work order: "wo-good-1"
	And an material used "mat2" exists with work order: "wo-good-2"
	And I am logged in as "user"
	When I visit the Reports/CompletedWorkOrdersWithMaterial/Search page
	And I select operating center "ca40" from the OperatingCenter dropdown
	And I enter "2/12/2020" into the DateCompleted_Start field
	And I enter "2/13/2020" into the DateCompleted_End field
	And I press Search
	Then I should see the following values in the search-results table
	| Work Description    | Total | With Materials | Without Materials |
	| SERVICE LINE REPAIR | 3     | 2              | 1                 |
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
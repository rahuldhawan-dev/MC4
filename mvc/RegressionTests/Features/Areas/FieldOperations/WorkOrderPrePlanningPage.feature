Feature: WorkOrderPrePlanningPage

Background: stuff exists
    Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg"
    And a town section "inactive" exists with town: "nj7burg", active: false
    And a street "one" exists with town: "nj7burg", full st name: "Easy St"
    And a street "two" exists with town: "nj7burg"
    And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "main" exists with description: "main"
    And an asset type "service" exists with description: "service"
    And an asset type "sewer opening" exists with description: "sewer opening"
    And an asset type "sewer lateral" exists with description: "sewer lateral"
    And an asset type "sewer main" exists with description: "sewer main"
    And an asset type "storm catch" exists with description: "storm catch"
    And an asset type "equipment" exists with description: "equipment"
    And an asset type "facility" exists with description: "facility"
    And an asset type "main crossing" exists with description: "main crossing"
    And operating center: "nj7" has asset type "valve"
    And operating center: "nj7" has asset type "hydrant"
    And operating center: "nj7" has asset type "main"
    And operating center: "nj7" has asset type "service"
    And operating center: "nj7" has asset type "sewer opening"
    And operating center: "nj7" has asset type "sewer lateral"
    And operating center: "nj7" has asset type "sewer main"
    And operating center: "nj7" has asset type "storm catch"
    And operating center: "nj7" has asset type "equipment"
    And operating center: "nj7" has asset type "facility"
    And operating center: "nj7" has asset type "main crossing"
    And a hydrant "one" exists with street: "one"
    And a valve "one" exists with operating center: "nj7", town: "nj7burg", street: "one"
    And a sewer opening "one" exists with street: "one"
    And a facility "one" exists with town: "nj7burg"
    And an equipment "one" exists with facility: "one"
    And a main crossing "one" exists with length of segment: "100.01", stream: "one", town: "nj7burg", operating center: "nj7"
    And work order requesters exist
    And work order purposes exist
    And work order priorities exist
    And work descriptions exist
    And markout requirements exist
    And a user "nj7" exists with username: "nj7", default operating center: "nj7", full name: "Foo"
	And a contractor "nj7" exists with operating center: "nj7", is active: true
    And a coordinate "one" exists	
	And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
    And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a work order "one" exists with operating center: "nj7", markout requirement: "routine", sap notification number: "222222", sap work order number: "111111"    
    And a work order "two" exists with operating center: "nj7", s o p required: true, sap notification number: "11111", sap work order number: "333333"
    And a work order "three" exists with operating center: "nj7", work order priority: "emergency"
    And a work order "four" exists with operating center: "nj7"
    And a street opening permit "one" exists with StreetOpeningPermitNumber: "1234", DateRequested: "01/01/2021", DateIssued: "01/02/2021", WorkOrder: "two"
  
Scenario: admin can search for and assign work to users
	Given I am logged in as "admin"
	And I am at the FieldOperations/WorkOrderPrePlanning/Search page
	When I press "Search"
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj7"'s Description from the OperatingCenter dropdown
	And I press "Search"
	Then I should be at the FieldOperations/WorkOrderPrePlanning page
	When I click the checkbox named WorkOrderIds with work order "one"'s Id
    And I click the checkbox named WorkOrderIds with work order "two"'s Id
	And I select user "nj7"'s FullName from the AssignedTo dropdown
	And I press Assign
	Then I should be at the FieldOperations/WorkOrderPrePlanning page
	And the OfficeAssignment value for work order "one" should be user "nj7"
	And the OfficeAssignment value for work order "two" should be user "nj7"
	And the OfficeAssignedOn value for work order "one" should not be null
	And the OfficeAssignedOn value for work order "two" should not be null

Scenario: admin can search for and assign work to contractors
	Given I am logged in as "admin"
	And I am at the FieldOperations/WorkOrderPrePlanning/Search page
	When I press "Search"
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj7"'s Description from the OperatingCenter dropdown
	And I press "Search"
	Then I should be at the FieldOperations/WorkOrderPrePlanning page
	When I check the AssignTo radio button with the value contractor
    And I click the checkbox named WorkOrderIds with work order "one"'s Id
    And I click the checkbox named WorkOrderIds with work order "two"'s Id 
    Then the checkbox named WorkOrderIds with work order "four"'s Id should be disabled
	When I select contractor "nj7" from the ContractorAssignedTo dropdown
	And I press Assign
	Then I should be at the FieldOperations/WorkOrderPrePlanning page
    And the AssignedContractor value for work order "one" should be contractor "nj7"
	And the AssignedContractor value for work order "two" should be contractor "nj7"
    And the AssignedToContractorOn value for work order "one" should not be null
	And the AssignedToContractorOn value for work order "two" should not be null

Scenario: admin can search with sop requested selected
	Given I am logged in as "admin"
	And I am at the FieldOperations/WorkOrderPrePlanning/Search page
    When I select operating center "nj7"'s Description from the OperatingCenter dropdown
    And I select "Yes" from the StreetOpeningPermitRequested dropdown
    And I select "Yes" from the StreetOpeningPermitIssued dropdown
	And I press "Search"
	Then I should be at the FieldOperations/WorkOrderPrePlanning page
    When I click the checkbox named WorkOrderIds with work order "two"'s Id
	And I select user "nj7"'s FullName from the AssignedTo dropdown
	And I press Assign

Scenario: admin can search and change planned completion dates
	Given I am logged in as "admin"
	And I am at the FieldOperations/WorkOrderPrePlanning/Search page
	When I select operating center "nj7"'s Description from the OperatingCenter dropdown
	And I press "Search"
	Then I should be at the FieldOperations/WorkOrderPrePlanning page
	Then I should see the following values in the workOrdersTable table
	  | Order # | Planned Completion Date (back office use only) |
	  | 1       |                                                |
	  | 2       |                                                |
	  | 3       |                                                |
	  | 4       |                                                |
	When I click the checkbox named WorkOrderIds with work order "one"'s Id
	And I click the checkbox named WorkOrderIds with work order "three"'s Id
    And I enter the date "yesterday" into the PlannedCompletionDate field
    And I press "Update"
	Then I should see the validation message "The field Planned Completion Date must be greater than or equal to today for an Emergency priority. For all other priorities it must be between 2 days from now and 12/31/9999 11:59:59 PM."
	When I enter the date "3 days from now" into the PlannedCompletionDate field
	And I click cancel in the dialog after pressing "Update"
    And I enter the date "2 days from now" into the PlannedCompletionDate field
	And I click ok in the dialog after pressing "Update"
	And I wait for the page to reload
	Then I should see the following values in the workOrdersTable table
	  | Order # | Planned Completion Date (back office use only) |
	  | 1       | 2 days from now                                |
	  | 2       |                                                |
	  | 3       | 2 days from now                                |
	  | 4       |                                                |

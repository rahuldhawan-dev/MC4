Feature: SpecialDialogsPage
	In order to make sure SAP dialogs are working
	As a user
	I want to be to click the links and see the dialogs	

Background:
	Given a plant maintenance activity type "one" exists with description: "one"
	And a plant maintenance activity type "two" exists with description: "two"
	And a plant maintenance activity type "three" exists with description: "three"
	And a plant maintenance activity type "four" exists with description: "four"
	And a plant maintenance activity type "five" exists with description: "five"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg"
    And a street "one" exists with town: "nj7burg", full st name: "Easy St"
    And a street "two" exists with town: "nj7burg"
    And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "main" exists with description: "main"
    And an asset type "service" exists with description: "service"
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
	And an admin user "admin" exists with username: "admin", full name: "Admin McAdminy"
    And a user "user" exists with username: "user"
	And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"

Scenario: user can view the wbs lookup dialog on recurring projects
	Given I am logged in as "admin"
	And I am at the ProjectManagement/RecurringProject/New page
	When I follow "Click here to Lookup and Verify the WBS Number"
	And I wait for the dialog to open
	Then I should see "Open" in the IsOpen dropdown

Scenario: user can view the workorder wbs element dialog
	Given I am logged in as "user"
	And I am at the SAP/WBSElement/Find page
	And I am at the FieldOperations/WorkOrder/New page
	When I select plant maintenance activity type "five" from the PlantMaintenanceActivityTypeOverride dropdown
	And I follow "Click here to Lookup and Verify the WBS Number"
	And I wait for the dialog to open
	Then I should see "Open" in the IsOpen dropdown
	When I visit the FieldOperations/WorkOrder/New/ page 
	When I select plant maintenance activity type "five" from the PlantMaintenanceActivityTypeOverride dropdown
	And I follow "Click here to Lookup and Verify the WBS Number"
	And I wait for the dialog to open
	Then I should see "Open" in the IsOpen dropdown
	When I visit the FieldOperations/WorkOrder/New/0?foo=bar page 
	When I select plant maintenance activity type "five" from the PlantMaintenanceActivityTypeOverride dropdown
	And I follow "Click here to Lookup and Verify the WBS Number"
	And I wait for the dialog to open
	Then I should see "Open" in the IsOpen dropdown
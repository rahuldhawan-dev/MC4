Feature: Hydrant Inspections

Background:
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
    And an asset type "hydrant" exists with description: "hydrant"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant inspection type "flush" exists with description: "FLUSH"
	And a hydrant tag status "tag" exists with description: "Tag!"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an operating center "ew4" exists with opcode: "EW4", name: "Edison", is active: "false"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a state "nj" exists
	And a town "one" exists with name: "Loch Arbour", state: "nj"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "hydrantinspections-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a hydrant "one" exists with operating center: "nj7", critical: true, critical notes: "This is critical", town: "one"
	And a hydrant "two" exists with operating center: "ew4", critical: true, critical notes: "This is critical", town: "one"
	And a hydrant inspection "one" exists with hydrant: "one", date inspected: "1/1/2015"

Scenario: Hydrant Inspection Add should not be visible in the action bar
	Given I am logged in as "user"
	And I am at the FieldOperations/HydrantInspection/Search page
	Then I should not see the "new" button in the action bar

Scenario: User should not see delete in action bar 
	Given I am logged in as "user"
	And I am at the show page for hydrant inspection: "one"
	Then I should not see "Delete"

Scenario: Admin should see delete in action bar 
	Given I am logged in as "admin"
	And I am at the show page for hydrant inspection: "one"
	Then I should see "Delete"

Scenario: User can not alter InspectionDate
	Given I am logged in as "user"
	And I am at the show page for hydrant: "one"
	And I have clicked the "Inspections" tab
	When I follow "New Inspection"
	Then I should not see the DateInspected field

Scenario: Admin can alter InspectionDate
	Given I am logged in as "admin"
	And I am at the show page for hydrant: "one"
	And I have clicked the "Inspections" tab
	When I follow "New Inspection"
	Then I should see the DateInspected field

Scenario: Adding a hydrant inspection has all sorts of validation
	Given I am logged in as "user"
	And I am at the show page for hydrant: "one"
	And I have clicked the "Inspections" tab
	When I follow "New Inspection"
	And I press Save
	Then I should see a validation message for HydrantInspectionType with "The Inspection Type field is required."
	And I should see a validation message for HydrantTagStatus with "The HydrantTagStatus field is required."

Scenario: Adding a hydrant inspection displays a popup whenever Chlorine values outside of 0.2-3.2 limits
	Given I am logged in as "user"
	And I am at the show page for hydrant: "one"
	And I have clicked the "Inspections" tab
	When I follow "New Inspection"
	And I press Save
	#popup doesn't appear
	 Then I should see a validation message for HydrantInspectionType with "The Inspection Type field is required."
	And I should see a validation message for HydrantTagStatus with "The HydrantTagStatus field is required."
	And I should see a validation message for TotalNoReadReason with "The TotalNoReadReason field is required."
	And I should see a validation message for FreeNoReadReason with "The FreeNoReadReason field is required."
	When I enter "3.3" into the TotalChlorine field
	And I click ok in the dialog after pressing "Save"
	And I wait 1 second
	When I enter "3.1" into the TotalChlorine field
	And I press Save
	#popup doesn't appear
	When I enter "3.3" into the ResidualChlorine field
	And I click ok in the dialog after pressing "Save"
	When I enter "3.1" into the ResidualChlorine field
	And I press Save
	When I enter "0.1" into the TotalChlorine field
	And I click ok in the dialog after pressing "Save"
	When I enter "0.3" into the TotalChlorine field
	And I press Save
	When I enter "0.1" into the ResidualChlorine field
	And I click ok in the dialog after pressing "Save"
	When I enter "0.3" into the ResidualChlorine field
	And I press Save
	#popup doesn't appear

Scenario: Adding a hydrant inspection displays critical notes if critical
	Given I am logged in as "user"
	And I am at the show page for hydrant: "one"
	And I have clicked the "Inspections" tab
	When I follow "New Inspection"
	#Then I should see a display for hydrant: "one"'s CriticalNotes
	Then I should see a display for HydrantDisplay_CriticalNotes with hydrant "one"'s CriticalNotes

Scenario: User can't add inspection if operating center is inactive
	Given I am logged in as "admin"
	When I visit the Show page for hydrant: "two"
	And I click the "Inspections" tab
	Then I should not see "New Inspection"

Scenario: user cant add inspection from index if operating center is inactive
	Given I am logged in as "admin"
    When I visit the FieldOperations/Hydrant/Search page
    And I press Search
    Then I should see a link to the Show page for hydrant: "two"
    And I should not see a link to the FieldOperations/HydrantInspection/New page for hydrant: "two"

Scenario: can edit a hydrant inspection
  Given I am logged in as "user"
	When I visit the show page for hydrant inspection: "one"
	And I follow "Edit"
	And I enter -1 into the PreResidualChlorine field
	And I enter 5 into the PreTotalChlorine field
	And I press "Save"
	Then I should see the validation message "Residual chlorine must be between 0 and 9.99"
	And I should see the validation message "Total chlorine must be between 0 and 4."
	When I enter 3 into the PreResidualChlorine field
	And I enter 3 into the PreTotalChlorine field
	And I press "Save"

Scenario: user can view work orders associated with hydrant when creating a new inspection
	Given I am logged in as "admin"
    And a work order "one" exists with operating center: "nj7", town: "one", street: "one", asset type: "hydrant", hydrant: "one"
	And I am at the show page for hydrant: "one"
	When I click the "Inspections" tab
	And I follow "New Inspection"
	Then I should see a link to the show page for hydrant "one"
	And I should see the following values in the workOrdersTable table
	| Order Number | Street Number | Street                          | Nearest Cross Street            | Town        | Town Section | Asset | Description of Job | Priority | Created By | Purpose       | Date Received | Date Completed | Digital As-Built Completed |
	| 1            | 1234          | *StreetPrefix*					 | *StreetPrefix*				   | Loch Arbour | *Section*    | HAB-1 |                    | Routine  | *user*     | Revenue >1000 | today	     |                | n/a                        |

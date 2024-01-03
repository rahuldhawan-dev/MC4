Feature: Sewer Opening Inspections

Background:
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
    And an asset type "sewer opening" exists with description: "sewer opening"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a state "nj" exists
	And a town "one" exists with name: "Loch Arbour", state: "nj"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a role "sewer opening-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "sewer openinginspections-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a sewer opening "one" exists with operating center: "nj7", critical: true, critical notes: "This is critical", town: "one", route: "1", opening suffix: "123"
	And a sewer opening inspection "one" exists with sewer opening: "one", date inspected: "1/1/2015"

Scenario: Sewer Opening Inspection Add should not be visible in the action bar
	Given I am logged in as "user"
	And I am at the FieldOperations/SewerOpeningInspection/Search page
	Then I should not see the "new" button in the action bar

Scenario: User should not see delete in action bar 
	Given I am logged in as "user"
	And I am at the Show page for sewer opening inspection: "one"
	Then I should not see "Delete"

Scenario: Admin should see delete in action bar 
	Given I am logged in as "admin"
	And I am at the Show page for sewer opening inspection: "one"
	Then I should see "Delete"

Scenario: User can not alter InspectionDate
	Given I am logged in as "user"
	And I am at the Show page for sewer opening: "one"
	When I click the "Inspections" tab
	And I follow "New Inspection"
	Then I should not see the DateInspected field

Scenario: Admin can alter InspectionDate
	Given I am logged in as "admin"
	And I am at the Show page for sewer opening: "one"
	When I click the "Inspections" tab
	And I follow "New Inspection"
	Then I should see the DateInspected field

Scenario: Adding a sewer opening inspection has all sorts of validation
	Given I am logged in as "user"
	And I am at the Show page for sewer opening: "one"
	When I click the "Inspections" tab
	And I follow "New Inspection"
	And I enter "eee" into the AmountOfDebrisGritCubicFeet field
	And I press Save
	Then I should see the validation message "The Rim Height Above / Below Grade (IN) field is required."
	And I should see the validation message "The Rim To Water Level Depth (FT) field is required."
    And I should see the validation message "The field AMOUNT OF DEBRIS/GRIT REMOVED (Cubic Ft): must be a number."

Scenario: Adding a sewer opening inspection displays critical notes if critical
	Given I am logged in as "user"
	And I am at the show page for sewer opening: "one"
	When I click the "Inspections" tab
	And I follow "New Inspection"
	Then I should see a display for SewerOpeningDisplay_CriticalNotes with sewer opening "one"'s CriticalNotes

Scenario: I can search for an inspection
	Given I am logged in as "user"
	And I am at the FieldOperations/SewerOpeningInspection/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I select "1" from the Route dropdown
	And I press Search
	Then I should see a link to the Show page for sewer opening inspection: "one"
	When I follow the Show link for sewer opening inspection "one"
	Then I should be at the Show page for sewer opening inspection: "one"
	When I visit the FieldOperations/SewerOpeningInspection/Search page
	And I press Search
	Then I should see the validation message "The OperatingCenter field is required."

Scenario: I can add / edit a inspection
	Given I am logged in as "user"
    When I visit the Show page for sewer opening: "one"
	And I click the "Inspections" tab
	And I follow "New Inspection"
	When I enter "123" into the PipesIn field
	And  I enter "123" into the PipesOut field
	And  I enter "12" into the RimToWaterLevelDepth field
	And  I enter "12" into the RimHeightAboveBelowGrade field
	And  I enter "123" into the AmountOfDebrisGritCubicFeet field
	And  I enter "123" into the Remarks field	
	And I press Save
	Then the currently shown sewer opening inspection will now be referred to as "new"
	And I should be at the Show page for sewer opening inspection "new"
	And I should see a display for InspectedBy with "user"
	And I should see a display for PipesIn with "123"
	And I should see a display for PipesOut with "123"
	And I should see a display for RimToWaterLevelDepth with "12"
	And I should see a display for RimHeightAboveBelowGrade with "12"
	And I should see a display for AmountOfDebrisGritCubicFeet with "123"
	And I should see a display for Remarks with "123"
	When I visit the Edit page for sewer opening inspection "new"
	And I enter "234" into the PipesIn field
	And I enter "234" into the PipesOut field
	And I enter "23" into the RimToWaterLevelDepth field
	And I enter "23" into the RimHeightAboveBelowGrade field
	And I enter "234" into the AmountOfDebrisGritCubicFeet field
	And I enter "234" into the Remarks field
	And I press Save
	Then I should be at the Show page for sewer opening inspection "new"
	And I should see a display for InspectedBy with "user"
	And I should see a display for PipesIn with "234"
	And I should see a display for PipesOut with "234"
	And I should see a display for RimToWaterLevelDepth with "23"
	And I should see a display for RimHeightAboveBelowGrade with "23"
	And I should see a display for AmountOfDebrisGritCubicFeet with "234"
	And I should see a display for Remarks with "234"
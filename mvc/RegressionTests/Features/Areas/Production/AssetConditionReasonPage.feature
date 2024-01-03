Feature: AssetConditionReason
	I want to be able to manage as AssetConditionReason

Background: 
	Given an admin user "admin" exists with username: "admin"
	And a role "reasonread" exists with action: "Read", module: "ProductionDataAdministration"
	And a role "reasonedit" exists with action: "Edit", module: "ProductionDataAdministration"
	And a role "reasonadd" exists with action: "Add", module: "ProductionDataAdministration"
	And a user "user" exists with username: "user", roles: reasonread;reasonedit;reasonadd
	And a condition type "one" exists with description: "As Found"
	And a condition description "one" exists with description: "Unable to Inspect", condition type: "one"
	And a condition type "two" exists with description: "As Left"
	And a condition description "two" exists with description: "Unable to Inspect 2", condition type: "two"
	And an asset condition reason "one" exists with code: "AFCL", description: "Cannot Locate", condition description: "one"
	And an asset condition reason "two" exists with code: "AFCL", description: "Cannot Locate 2", condition description: "two"
	And an asset condition reason "three" exists with code: "AFOC", description: "Operational Constraint", condition description: "one"
	And an asset condition reason "four" exists with code: "AFOC", description: "Operational Constraint 2", condition description: "two"
	And an asset condition reason "five" exists with code: "AFOO", description: "Out Of Service", condition description: "one"
	And an asset condition reason "six" exists with code: "AFOO", description: "Out Of Service 2", condition description: "two"
	And an asset condition reason "seven" exists with code: "AFSP", description: "Scheduling Problem", condition description: "one"
	And an asset condition reason "eight" exists with code: "AFSP", description: "Scheduling Problem 2", condition description: "two"
	And an asset condition reason "nine" exists with code: "AFSC", description: "Seasonal Constraint", condition description: "one"
	And an asset condition reason "ten" exists with code: "AFSC", description: "Seasonal Constraint 2", condition description: "two"

Scenario: User can view as asset condition reason with As Found condition type
	Given I am logged in as "user"
	When I visit the Production/AssetConditionReason/Show page for asset condition reason "one"
	Then I should see a display for Description with "Cannot Locate"
	And I should see a display for ConditionDescription with "Unable to Inspect"
	And I should see a display for ConditionDescription_ConditionType with "As Found"

Scenario: User can view as asset condition reason with As Left condition type
	Given I am logged in as "user"
	When I visit the Production/AssetConditionReason/Show page for asset condition reason "two"
	Then I should see a display for Description with "Cannot Locate 2"
	And I should see a display for ConditionDescription with "Unable to Inspect 2"
	And I should see a display for ConditionDescription_ConditionType with "As Left"

Scenario: User can create an Asset Condition Reason
    Given I am logged in as "user"
	When I visit the Production/AssetConditionReason/New page
    And I press Save
    Then I should see a validation message for Code with " The Code field is required."
	When I enter "ALCL" into the Code field
	And I enter "Cannot Delete" into the Description field
	And I press Save
	Then I should see a validation message for ConditionType with " The ConditionType field is required."
	When I select "As Found" from the ConditionType dropdown
	And I press Save
	Then I should see a validation message for ConditionDescription with " The ConditionDescription field is required."
	When I select "Unable to Inspect" from the ConditionDescription dropdown
	And I press Save
	Then I should see a display for ConditionDescription with "Unable to Inspect"
	And I should see a display for Description with "Cannot Delete"
	And I should see a display for Code with "ALCL"
	And I should see a display for ConditionDescription_ConditionType with "As Found"

Scenario: User can edit an Asset Condition Reason
    Given I am logged in as "user"
	When I visit the Production/AssetConditionReason/Edit/1 page
	And I enter "Cannot Delete" into the Description field
	And I select "As Left" from the ConditionType dropdown
	And I select "Unable to Inspect 2" from the ConditionDescription dropdown
	And I press Save
	Then I should see a display for ConditionDescription with "Unable to Inspect 2"
	And I should see a display for Description with "Cannot Delete"
	And I should see a display for Code with "AFCL"
	And I should see a display for ConditionDescription_ConditionType with "As Left"

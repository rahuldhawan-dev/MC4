Feature: ProductionWorkDescriptionPage
	In order to manage production work descriptions
	As a user
	I want to be able to manage production work descriptions

Background: 
    Given an operating center "nj7" exists with opcode: "NJ71111", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And a user "user" exists with username: "user"
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg"
    And a street "one" exists with town: "nj7burg", full st name: "Easy St"
	And an equipment type "aerator" exists with description: "ET01 - Aerator"
	And a plant maintenance activity type "two" exists with description: "two" 
	And a plant maintenance activity type "one" exists with description: "one" 
    And order types exist
	And a maintenance plan task type "one" exists with description: "Calibration"
	And a maintenance plan task type "two" exists with description: "Inspection"
 	And a role "roleRead" exists with action: "Read", module: "ProductionWorkManagement", user: "user", operating center: "nj7"
	And a role "roleAdd" exists with action: "Add", module: "ProductionWorkManagement", user: "user", operating center: "nj7"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionWorkManagement", user: "user", operating center: "nj7"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionWorkManagement", user: "user", operating center: "nj7"
	And a production work description "one" exists with description: "Bah"

Scenario: user can view a production work description
	Given I am logged in as "user"
	And I am at the Production/ProductionWorkDescription/Search page
	When I press Search
	And I follow the Show link for production work description "one"
	Then I should be at the Show page for production work description "one"
	And I should see a display for Description with "Bah"

Scenario: user can create a production work description
	Given I am logged in as "user"
	And a task group category "one" exists with description: "This is the description", type: "Chemical", abbreviation: "CHEM", is active: "true"
    And a task group category "two" exists with description: "This is the description two", type: "Electrical", abbreviation: "ELEC", is active: "true"
    And a task group "tasky" exists with task group id: "ABC12", task group name: "TaskyName", description: "Tasky description", required: "true", frequency: "123", equipment types: "aerator", task group categories: "one;two"
    When I visit the Production/ProductionWorkDescription/New page
    And I enter "description here" into the Description field
	And I select equipment type "aerator"'s ToString from the EquipmentType dropdown
	And I select order type "routine"'s Description from the OrderType dropdown
	And I select plant maintenance activity type "two"'s Description from the PlantMaintenanceActivityType dropdown
	And I select "Yes" from the BreakdownIndicator dropdown
	And I select "Calibration" from the MaintenancePlanTaskType dropdown
	And I select task group "tasky"'s Caption from the TaskGroup dropdown
	And I press Save
	Then I should see a display for Description with "description here"
	And I should see a display for OrderType with order type "routine"'s ToString
	And I should see a display for PlantMaintenanceActivityType with plant maintenance activity type "two"'s ToString
	And I should see a display for BreakdownIndicator with "Yes"
	And I should see a display for MaintenancePlanTaskType with "Calibration"
	And I should see a display for TaskGroup with "ABC12 - TaskyName"

Scenario: user can edit a production work description
	Given I am logged in as "user"
	And a task group category "one" exists with description: "This is the description", type: "Chemical", abbreviation: "CHEM", is active: "true"
    And a task group category "two" exists with description: "This is the description two", type: "Electrical", abbreviation: "ELEC", is active: "true"
    And a task group "tasky" exists with task group id: "ABC12", task group name: "TaskyName", description: "Tasky description", required: "true", frequency: "123", equipment types: "aerator", task group categories: "one;two"
	When I visit the Edit page for production work description "one"
    And I type "New description here" into the Description field
	And I select "No" from the BreakdownIndicator dropdown
	And I select order type "routine"'s Description from the OrderType dropdown
	And I select plant maintenance activity type "one"'s Description from the PlantMaintenanceActivityType dropdown
	And I select "Inspection" from the MaintenancePlanTaskType dropdown
	And I select task group "tasky"'s Caption from the TaskGroup dropdown
    And I press Save
    Then I should be at the Show page for production work description "one"
    And I should see a display for Description with "New description here"
	And I should see a display for OrderType with order type "routine"'s ToString
	And I should see a display for PlantMaintenanceActivityType with plant maintenance activity type "one"'s ToString
	And I should see a display for BreakdownIndicator with "No"
	And I should see a display for MaintenancePlanTaskType with "Inspection"
	And I should see a display for TaskGroup with "ABC12 - TaskyName"

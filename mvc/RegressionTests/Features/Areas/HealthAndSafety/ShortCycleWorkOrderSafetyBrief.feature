Feature: ShortCycleWorkOrderSafetyBrief

Background:
Given an employee status "active" exists with description: "Active"
And an state "one" exists
And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "one"
And an employee "one" exists with status: "active", first name: "Bill", last name: "S. Preston", employee id: "1000001", operating center: "nj7", state: "one"
And an employee "two" exists with status: "active", first name: "Not Bill", last name: "S. Not Preston", employee id: "1202011", reports to: "one", operating center: "nj7", state: "one"
And a user "user" exists with username: "user", employee: "two"
And a role "FieldServicesShortCycle-useradmin" exists with action: "UserAdministrator", module: "FieldServicesShortCycle", user: "user"
And short cycle work order safety brief location types exist
And short cycle work order safety brief hazard types exist
And short cycle work order safety brief ppe types exist
And short cycle work order safety brief tool types exist
And a short cycle work order safety brief "one" exists with FSR: "one", DateCompleted: "09/01/2020"
And a short cycle work order safety brief "two" exists with FSR: "two", DateCompleted: "11/01/2020"

Scenario: user should see required validation
    Given I am logged in as "user"
    And I am at the HealthAndSafety/ShortCycleWorkOrderSafetyBrief/New page
    When I press "Save"
    Then I should see a validation message for LocationTypes with "The Where will you be working today? field is required."
    Then I should see a validation message for IsPPEInGoodCondition with "The Is your PPE in good condition? field is required."
    Then I should see a validation message for HasCompletedDailyStretchingRoutine with "The Have you completed your daily stretching routine? field is required."
    Then I should see a validation message for HazardTypes with "The What hazards will likely be present while working today? field is required."
    Then I should see a validation message for HasPerformedInspectionOnVehicle with "The Have you performed inspection on your vehicle and is it safe to operate? field is required."
    Then I should see a validation message for PPETypes with "The What PPE do you need today based on the hazards you will face? field is required."
    Then I should see a validation message for ToolTypes with "The What tools do you need today? field is required."

Scenario: user can create safety brief
    Given I am logged in as "user"
    And I am at the HealthAndSafety/ShortCycleWorkOrderSafetyBrief/New page
    When I select "Meter Reading" from the LocationTypes dropdown
    And I select "FSR Work (Residence)" from the LocationTypes dropdown
    And I select "Yes" from the IsPPEInGoodCondition dropdown
    And I select "Yes" from the HasCompletedDailyStretchingRoutine dropdown
    And I select "Slips,Trips,Falls" from the HazardTypes dropdown
    And I select "Limited Workspace" from the HazardTypes dropdown
    And I select "Yes" from the HasPerformedInspectionOnVehicle dropdown
    And I select "Hardhat" from the PPETypes dropdown
    And I select "Class III Apparel" from the PPETypes dropdown
    And I select "Hand Tools" from the ToolTypes dropdown
    And I select "Multimeter" from the ToolTypes dropdown
    And I press "Save"
    Then the currently shown short cycle work order safety brief shall henceforth be known throughout the land as "Googly"
    Then I should be at the show page for short cycle work order safety brief "Googly"
    And I should see a display for FSR with employee "two"'s FullName
    And I should see a display for DateCompleted with today's date
    And I should see "FSR Work (Residence), Meter Reading"
    And I should see a display for IsPPEInGoodCondition with "Yes"
    And I should see a display for HasCompletedDailyStretchingRoutine with "Yes"
    And I should see "Limited Workspace, Slips,Trips,Falls"
    And I should see a display for HasPerformedInspectionOnVehicle with "Yes"
    And I should see "Class III Apparel, Hardhat"
    And I should see "Hand Tools, Multimeter"

Scenario: user can search for safety brief
    Given I am logged in as "user"
    And I am at the HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Search page
    When I select state "one" from the State dropdown
    And I press "Search"
    Then I should see a link to the Show page for short cycle work order safety brief "two"
    When I visit the HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Search page
    And I press "Search" 
    Then I should see a link to the Show page for short cycle work order safety brief "one"
    And I should see a link to the Show page for short cycle work order safety brief "two"
    When I visit the HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Search page
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select employee "one"'s Description from the FSR dropdown
    And I press "Search"
    Then I should see a link to the Show page for short cycle work order safety brief "one"
    When I visit the HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Search page
    And I enter "09/01/2020" into the DateCompleted.Start field
    And I enter "09/03/2020" into the DateCompleted.End field
    And I press "Search"
    Then I should see a link to the Show page for short cycle work order safety brief "one"
    And I should not see a link to the Show page for short cycle work order safety brief "two"
    When I visit the HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Search page
    And I enter "11/01/2020" into the DateCompleted.Start field
    And I enter "11/03/2020" into the DateCompleted.End field
    And I press "Search"
    Then I should see a link to the Show page for short cycle work order safety brief "two"
    And I should not see a link to the Show page for short cycle work order safety brief "one"
    When I visit the HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Search page
    And I enter "08/01/2020" into the DateCompleted.Start field
    And I enter "11/30/2020" into the DateCompleted.End field
    And I press "Search"
    Then I should see a link to the Show page for short cycle work order safety brief "one"
    And I should see a link to the Show page for short cycle work order safety brief "two"
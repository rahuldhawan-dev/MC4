Feature: GasMonitors

Background:
    Given an employee "one" exists
    And a user "user" exists with username: "user", employee: "one"
    And a state "nj" exists with abbreviation: "NJ"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
    And a facility "one" exists with operating center: "nj7" 
    And a equipment type "generator" exists with description: "generator" 
    And an equipment manufacturer "generator" exists with equipment type: "generator", description: "a description"
    And a role "operationslockoutforms-useradmin" exists with action: "UserAdministrator", module: "OperationsLockoutForms", user: "user", operating center: "nj7"
    And a role "productionequipment-useradmin" exists with action: "UserAdministrator", module: "ProductionEquipment", user: "user", operating center: "nj7"
    And I am logged in as "user"

Scenario: User can create a gas monitor 
    Given a gas monitor equipment "one" exists with facility: "one", equipment manufacturer: "generator" 
    And I am at the HealthAndSafety/GasMonitor/New page
    When I press "Save"
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    And I should see a validation message for CalibrationFrequencyDays with "The Calibration Frequency (days) field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press "Save"
    Then I should see a validation message for Equipment with "The Equipment field is required."
    When I enter "0" into the CalibrationFrequencyDays field
    And I press "Save"
    Then I should see a validation message for CalibrationFrequencyDays with "The field Calibration Frequency (days) must be greater than or equal to 1"
    When I select gas monitor equipment "one"'s Description from the Equipment dropdown
    And I enter "1" into the CalibrationFrequencyDays field
    And I press "Save" 
    Then I should see a display for OperatingCenter with operating center "nj7" 
    And I should see a display for Manufacturer with equipment manufacturer "generator"
    And I should see a display for Equipment_EquipmentStatus with "In Service"
    And I should see a link to the Show page for gas monitor equipment: "one"
    And I should see a display for CalibrationFrequencyDays with "1"

Scenario: User can add a calibration from a gas monitor record
    Given a equipment "one" exists with facility: "one", operating center: "nj7"
    And a gas monitor "one" exists with operating center: "nj7", equipment: "one" 
    And I am at the Show page for gas monitor "one"  
    When I click the "Calibrations" tab
    And I press "Add Calibration"
    And I enter "4/24/1984" into the ViewModel_CalibrationDate field
    And I select "No" from the ViewModel_CalibrationPassed dropdown
    And I press "Save Calibration"
    Then I should see a validation message for ViewModel.CalibrationFailedNotes with "The CalibrationFailedNotes field is required."
    When I enter "blah blah blah" into the ViewModel_CalibrationFailedNotes field
    And I press "Save Calibration"
    And I click the "Calibrations" tab
    Then I should see the following values in the calibrations-table table
    | Calibration Date | Calibration Passed | Calibration Failed Notes |
    | 4/24/1984        | No                 | blah blah blah           |

Scenario: User can remove a calibration from a gas monitor record
    Given a equipment "one" exists with facility: "one", operating center: "nj7"
    And a gas monitor "one" exists with operating center: "nj7", equipment: "one"
    And a gas monitor calibration "one" exists with gas monitor: "one"
    And a gas monitor calibration "two" exists with gas monitor: "one"
    And I am at the Show page for gas monitor "one"
    When I click the "Calibrations" tab
    And I click the "Remove" button in the 2nd row of calibrations-table and then click ok in the confirmation dialog
    And I wait for the page to reload
    And I click the "Calibrations" tab
    Then I should see a link to the Show page for gas monitor calibration "one"
    And I should not see a link to the Show page for gas monitor calibration "two"
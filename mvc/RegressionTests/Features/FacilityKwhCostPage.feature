Feature: FacilityKwhCost Page
    More of the same

Background: there are things
	Given a role "roleRead" exists with action: "Read", module: "ProductionFacilities"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionFacilities"
	And a role "roleAdd" exists with action: "Add", module: "ProductionFacilities"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionFacilities"
	And a user "user" exists with username: "user", roles: roleRead;roleEdit;roleAdd;roleDelete
    And an admin user "admin" exists with username: "admin"
    And an operating center "one" exists with opcode: "nj7"
    And an operating center "two" exists with opcode: "nj4"
    And a facility "one" exists with operating center: "one"
    And a facility "two" exists with operating center: "two"

Scenario: admin can search for and view costs by facility or operating center
    Given a facility kwh cost "one" exists with facility: "one"
    And a facility kwh cost "two" exists with facility: "two"
    And a facility kwh cost "three" exists with facility: "two"
    And I am logged in as "admin"
    When I visit the /FacilityKwhCost/Search page
    And I select operating center "one" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select facility "one"'s Description from the Facility dropdown
    And I press Search
    Then I should be at the Show page for facility kwh cost: "one"
    When I visit the /FacilityKwhCost/Search page
    And I select operating center "two" from the OperatingCenter dropdown
    And I press Search
    Then I should be at the FacilityKwhCost page
    And I should see a link to the Show page for facility kwh cost: "two"
    And I should see a link to the Show page for facility kwh cost: "three"
    And I should not see a link to the Show page for facility kwh cost: "one"

Scenario: admin can add a facility kwh cost
    Given I am logged in as "admin"
    When I visit the /FacilityKwhCost/New page
    And I press Save
    Then I should be at the FacilityKwhCost/New page
	And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "The Cost per kWh field is required."
    And I should see the validation message "The StartDate field is required."
    And I should see the validation message "The EndDate field is required."
    When I select operating center "one" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I enter 1234.56 into the CostPerKwh field
    And I enter today's date into the StartDate field
    And I enter today's date into the EndDate field
    And I press Save
    Then I should be at the FacilityKwhCost/New page
    And I should see the validation message "The Facility field is required."
    When I select facility "one"'s Description from the Facility dropdown
    And I press Save
    Then the currently shown facility kwh cost will now be referred to as "new cost"
    And I should be at the Show page for facility kwh cost: "new cost"

Scenario: admin can edit a facility kwh cost
    Given a facility kwh cost "one" exists with facility: "one"
	And I am logged in as "admin"
    When I visit the Edit page for facility kwh cost: "one"
    And I enter "666.66" into the CostPerKwh field
    And I press Save
    Then I should be at the Show page for facility kwh cost: "one"
    And I should see a display for CostPerKwh with "$666.66"

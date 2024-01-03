Feature: EquipmentManufacturerPage

Background: stuff exists
    Given an admin user "admin" exists with username: "admin"
	And an equipment type "aerator" exists with description: "ET01 - Aerator"
    And an equipment type "generator" exists with description: "Generator"
	And an equipment manufacturer "one" exists with equipment type: "aerator", description: "meh"

Scenario: user can add an equipment manufacturer
    Given I am logged in as "admin"
    When I visit the EquipmentManufacturer/New page
    And I press Save
    Then I should see the validation message The EquipmentType field is required.
    And I should see the validation message The Description field is required.
    When I select equipment type "generator"'s Display from the EquipmentType dropdown
    And I enter "foo" into the Description field
    And I press Save
    Then I should be at the EquipmentManufacturer/Search page
    When I press "Search"
    Then I should be at the EquipmentManufacturer page
    And I should see equipment type "generator"'s Display in the "Equipment Type" column
    And I should see "foo" in the "Description" column

Scenario: user can edit an equipment manufacturer
    Given I am logged in as "admin"
    When I visit the Edit page for equipment manufacturer: "one"
    And I select equipment type "generator"'s Display from the EquipmentType dropdown
    And I enter "foo" into the Description field
    And I press Save
    Then I should be at the EquipmentManufacturer/Search page
    When I press "Search"
    Then I should be at the EquipmentManufacturer page
    And I should see equipment type "generator"'s Display in the "Equipment Type" column
    And I should see "foo" in the "Description" column

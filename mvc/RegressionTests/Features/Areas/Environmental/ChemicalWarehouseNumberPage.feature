Feature: ChemicalWarehouseNumberPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists
    And a chemical warehouse number "one" exists
    And a chemical warehouse number "two" exists
    And I am logged in as "admin"

Scenario: user can search for a chemical warehouse number
    When I visit the Environmental/ChemicalWarehouseNumber/Search page
    And I press Search
    Then I should see a link to the Show page for chemical warehouse number: "one"
    When I follow the Show link for chemical warehouse number "one"
    Then I should be at the Show page for chemical warehouse number: "one"

Scenario: user can view a chemical warehouse number
    When I visit the Show page for chemical warehouse number: "one"
    Then I should see a display for chemical warehouse number: "one"'s WarehouseNumber

Scenario: user can add a chemical warehouse number
    When I visit the Environmental/ChemicalWarehouseNumber/New page
    And I enter "foo" into the WarehouseNumber field
	And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Save
    Then the currently shown chemical warehouse number will now be referred to as "new"
    And I should see a display for WarehouseNumber with "foo"

Scenario: user can edit a chemical warehouse number
    When I visit the Edit page for chemical warehouse number: "one"
    And I enter "bar" into the WarehouseNumber field
    And I press Save
    Then I should be at the Show page for chemical warehouse number: "one"
    And I should see a display for WarehouseNumber with "bar"
	
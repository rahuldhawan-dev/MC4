Feature: BondPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a bond "one" exists
    And a bond "two" exists
    And I am logged in as "admin"

Scenario: user can search for a bond
    When I visit the FieldOperations/Bond/Search page
    And I press Search
    Then I should see a link to the Show page for bond: "one"
    When I follow the Show link for bond "one"
    Then I should be at the Show page for bond: "one"

Scenario: user can view a bond
    When I visit the Show page for bond: "one"
    Then I should see a display for bond: "one"'s BondNumber

Scenario: user can add a bond
    When I visit the FieldOperations/Bond/New page
    And I enter "foo" into the BondNumber field
    And I press Save
    Then the currently shown bond will now be referred to as "Francis Black"
    And I should see a display for BondNumber with "foo"

Scenario: user can edit a bond
    When I visit the Edit page for bond: "one"
    And I enter "bar" into the BondNumber field
    And I press Save
    Then I should be at the Show page for bond: "one"
    And I should see a display for BondNumber with "bar"

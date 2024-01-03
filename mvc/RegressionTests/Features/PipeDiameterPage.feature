Feature: PipeDiameterPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a pipe diameter "one" exists
    And a pipe diameter "two" exists
    And I am logged in as "admin"

Scenario: user can view a pipe diameter
    When I visit the PipeDiameter page
    And I follow the Show link for pipe diameter "one"
    Then I should see a display for pipe diameter: "one"'s Diameter

Scenario: user can add a pipe diameter
    When I visit the PipeDiameter/New page
    And I enter "12" into the Diameter field
    And I press Save
    Then the currently shown pipe diameter will now be referred to as "florence"
    And I should see a display for Diameter with "12"

Scenario: user can edit a pipe diameter
    When I visit the Edit page for pipe diameter: "one"
    And I enter "13" into the Diameter field
    And I press Save
    Then I should be at the Show page for pipe diameter: "one"
    And I should see a display for Diameter with "13"

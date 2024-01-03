Feature: MaterialPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a material "one" exists
    And a material "two" exists
    And I am logged in as "admin"

Scenario: user can search for a material
    When I visit the Material/Search page
    And I press Search
    Then I should see a link to the Show page for material: "one"
    When I follow the Show link for material "one"
    Then I should be at the Show page for material: "one"

Scenario: user can view a material
    When I visit the Show page for material: "one"
    Then I should see a display for material: "one"'s Description

Scenario: user can add a material
    When I visit the Material/New page
    And I select "Yes" from the IsActive dropdown
    And I enter "12345678901234" into the PartNumber field
    And I enter "foo" into the Description field
    And I select "No" from the DoNotOrder dropdown
    And I press Save
    Then the currently shown material will now be referred to as "new"
    And I should see a display for Description with "foo"

Scenario: user can edit a material
    When I visit the Edit page for material: "one"
    And I enter "bar" into the Description field
    And I press Save
    Then I should be at the Show page for material: "one"
    And I should see a display for Description with "bar"

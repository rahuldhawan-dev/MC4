Feature: GISLayerUpdatePage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a g i s layer update "one" exists with is active: true
    And a g i s layer update "two" exists
    And I am logged in as "admin"

Scenario: user can search for a g i s layer update
    When I visit the GISLayerUpdate page
    Then I should see a link to the Show page for g i s layer update: "one"

Scenario: user can view a g i s layer update
    When I visit the Show page for g i s layer update: "one"
    Then I should see a display for g i s layer update: "one"'s Updated

Scenario: user can add a g i s layer update
    When I visit the GISLayerUpdate/New page
    And I enter today's date into the Updated field
    And I select "Yes" from the IsActive dropdown
    And I enter "1234567890" into the MapId field
    And I press Save
    Then the currently shown g i s layer update will now be referred to as "new"
    And I should be at the Show page for g i s layer update: "new"
    And I should see a display for IsActive with "Yes"
    And I should see a display for Updated with today's date
    And I should see a display for MapId with "1234567890"
    When I visit the show page for g i s layer update: "one"
	Then I should see a display for IsActive with "No"

Scenario: user can edit a g i s layer update
    When I visit the Edit page for g i s layer update: "one"
    And I enter today's date into the Updated field
    And I press Save
    Then I should be at the Show page for g i s layer update: "one"
    And I should see a display for Updated with today's date

Scenario: user cannot set the currently active mapid inactive
    Given I am at the edit page for g i s layer update: "one"
    Then I should not see the IsActive element
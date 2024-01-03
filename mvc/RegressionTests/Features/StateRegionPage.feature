Feature: StateRegionPage

Background: here you go have some stuff
    Given an admin user "admin" exists with username: "admin"
    And a state "one" exists
    And a state "two" exists
    And a state region "one" exists with state: "one"
    And a state region "two" exists with state: "two"
	And I am logged in as "admin"

Scenario: admin can search for state regions
    When I search for StateRegions with no conditions chosen
    Then I should be at the StateRegion page
    And I should see a link to the Show page for state region "one"
    And I should see a link to the Show page for state region "two"

Scenario: admin can create a new state region
    Given I am at the StateRegion/New page
    When I press Save
    Then I should be at the StateRegion/New page
    And I should see the validation message "The State field is required."
    And I should see the validation message "The Region field is required."
    When I select state "one" from the State dropdown
    And I enter "asdf" into the Region field
    And I press Save
    Then the currently shown state region will now be referred to as "new"
    And I should be at the Show page for state region: "new"
    And I should see a display for State with state "one"
    And I should see a display for Region with "asdf"
Feature: GradientPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a gradient "one" exists with description: "test"
    And I am logged in as "admin"

Scenario: user can search for a gradient
    When I visit the Gradient/Search page
    And I press Search
    Then I should see a link to the Show page for gradient: "one"
    When I follow the Show link for gradient "one"
    Then I should be at the Show page for gradient: "one"

Scenario: user can view a gradient
	When I visit the Show page for gradient: "one"
	Then I should see a display for gradient: "one"'s Description

Scenario: user can add a gradient
	When I visit the Gradient/New page
	And I enter "foo" into the Description field
	And I press Save
	Then I should see a display for Description with "foo"

Scenario: user can edit a gradient
	When I visit the Edit page for gradient: "one"
	And I enter "bar" into the Description field
	And I press Save
	Then I should be at the Show page for gradient: "one"
	And I should see a display for Description with "bar"


	

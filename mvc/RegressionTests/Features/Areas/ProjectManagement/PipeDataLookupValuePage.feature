Feature: PipeDataLookupValuePage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a pipe data lookup type "one" exists with description: "Internal Lining"
    And a pipe data lookup value "one" exists
    And a pipe data lookup value "two" exists
    And I am logged in as "admin"

Scenario: user can search for a pipe data lookup value
    When I visit the ProjectManagement/PipeDataLookupValue/Search page
    And I press Search
    Then I should see a link to the Show page for pipe data lookup value: "one"
    When I follow the Show link for pipe data lookup value "one"
    Then I should be at the Show page for pipe data lookup value: "one"

Scenario: user can view a pipe data lookup value
    When I visit the Show page for pipe data lookup value: "one"
    Then I should see a display for pipe data lookup value: "one"'s Description

Scenario: user can add a pipe data lookup value
    When I visit the ProjectManagement/PipeDataLookupValue/New page
    And I select pipe data lookup type "one" from the PipeDataLookupType dropdown
	And I enter "foo" into the Description field
	And I enter "12" into the VariableScore field
	And I enter "13" into the PriorityWeightedScore field
	And I select "Yes" from the IsEnabled dropdown
	And I select "No" from the IsDefault dropdown
    And I press Save
    Then the currently shown pipe data lookup value will now be referred to as "Stavros"
	And I should see a display for PipeDataLookupType with pipe data lookup type "one"'s Description
    And I should see a display for Description with "foo"
	And I should see a display for VariableScore with "12.00"
	And I should see a display for PriorityWeightedScore with "13.00"
	And I should see a display for IsEnabled with "Yes"
	And I should see a display for IsDefault with "No"

Scenario: user can edit a pipe data lookup value
    When I visit the Edit page for pipe data lookup value: "one"
    And I enter "bar" into the Description field
    And I press Save
    Then I should be at the Show page for pipe data lookup value: "one"
    And I should see a display for Description with "bar"

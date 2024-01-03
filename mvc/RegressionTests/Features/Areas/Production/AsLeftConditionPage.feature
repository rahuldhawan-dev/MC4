Feature: AsLeftConditionPage
	In order to manage as left condition
	As a user
	I want to be able to manage as left condition

Background: 
	Given a role "read" exists with action: "Read", module: "ProductionDataAdministration"
	And a role "edit" exists with action: "Edit", module: "ProductionDataAdministration"
	And a role "add" exists with action: "Add", module: "ProductionDataAdministration"
	And a role "delete" exists with action: "Delete", module: "ProductionDataAdministration"
	And a user "user" exists with username: "user", roles: read;edit;add;delete
	And a as left condition "one" exists with description: "Unable to Inspect"

Scenario: user can view as left condition
	Given I am logged in as "user"
	And I am at the Production/AsLeftCondition/index page
	When I follow the Show link for as left condition "one"
	Then I should be at the Show page for as left condition "one"
	And I should see a display for Description with "Unable to Inspect"

Scenario: user can create as left condition
	Given I am logged in as "user"
    When I visit the Production/AsLeftCondition/New page
	When I press Save
	Then I should see a validation message for Description with "The Description field is required."
    When I enter "description here" into the Description field
	And I press Save
	Then I should see a display for Description with "description here"

Scenario: user can edit as left condition
	Given I am logged in as "user"
	When I visit the Edit page for as left condition "one"
    And I type "New description here" into the Description field
    And I press Save
    Then I should be at the Show page for as left condition "one"
    And I should see a display for Description with "New description here"
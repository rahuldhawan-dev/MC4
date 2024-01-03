Feature: MaintenancePlanTaskTypePage
	In order to manage task types
	As a user
	I want to be able to manage task types

Background: 
    Given an admin user "admin" exists with username: "admin"
	And a role "tasktyperead" exists with action: "Read", module: "ProductionDataAdministration"
	And a role "tasktypeedit" exists with action: "Edit", module: "ProductionDataAdministration"
	And a role "tasktypeadd" exists with action: "Add", module: "ProductionDataAdministration"
	And a role "tasktypedelete" exists with action: "Delete", module: "ProductionDataAdministration"
	And a user "user" exists with username: "user", roles: tasktyperead;tasktypeedit;tasktypeadd;tasktypedelete
	And a maintenance plan task type "one" exists with description: "Task Type 1", is active: "true", abbreviation: "ABR1"
	And a maintenance plan task type "two" exists with description: "Task Type 2", is active: "true", abbreviation: "ABR2"

Scenario: Admin can view a task type
	Given I am logged in as "admin"
	And I am at the Production/MaintenancePlanTaskType/Show page for maintenance plan task type: "one"
	Then I should see a display for Description with "Task Type 1"
	And I should see a display for Abbreviation with "ABR1"
	And I should see a display for IsActive with "Yes"

Scenario: Admin can search a task type
	Given I am logged in as "admin"
	And I am at the Production/MaintenancePlanTaskType/Search page
	When I press Search
	Then I should be at the Production/MaintenancePlanTaskType page
	And I should see a link to the show page for maintenance plan task type: "one"
	And I should see a link to the show page for maintenance plan task type: "two"

Scenario: Admin can create a task type
	Given I am logged in as "admin"
    When I visit the Production/MaintenancePlanTaskType/New page
	And I press "Save"
	Then I should see a validation message for Description with "The Description field is required."
	When I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Description field
	And I press "Save"
	Then I should see a validation message for Description with "The field Description must be a string with a maximum length of 50."
	When I enter "description here" into the Description field 
	And I press Save
	Then I should see a validation message for Abbreviation with "The Abbreviation field is required."
	When I enter "ABBR" into the Abbreviation field
	And I press "Save"
	Then I should see a validation message for IsActive with "The IsActive field is required."
	When I select "Yes" from the IsActive dropdown
	And I press Save
	Then I should see a display for Description with "description here"
	And I should see a display for Abbreviation with "ABBR"
	And I should see a display for IsActive with "Yes"

Scenario: Admin can edit a task type
	Given I am logged in as "admin"
	When I visit the Edit page for maintenance plan task type: "one"
	And I enter "" into the Description field
	And I press "Save"
	Then I should see a validation message for Description with "The Description field is required."
	When I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Description field
	And I press "Save"
	Then I should see a validation message for Description with "The field Description must be a string with a maximum length of 50."
	When I enter "New description here" into the Description field
	And I enter "" into the Abbreviation field
	And I press Save
	Then I should see a validation message for Abbreviation with "The Abbreviation field is required."
	When I enter "ABBR" into the Abbreviation field
	And I select "-- Select --" from the IsActive dropdown
	And I press "Save"
	Then I should see a validation message for IsActive with "The IsActive field is required."
	When I select "No" from the IsActive dropdown
	And I press Save
    Then I should be at the Show page for maintenance plan task type: "one"
    And I should see a display for Description with "New description here"
	And I should see a display for Abbreviation with "ABBR"
	And I should see a display for IsActive with "No"

Scenario: User can view a task type
	Given I am logged in as "user"
	And I am at the Show page for maintenance plan task type: "one"
	Then I should see a display for Description with "Task Type 1"
	And I should see a display for Abbreviation with "ABR1"
	And I should see a display for IsActive with "Yes"

Scenario: User can search a task type
	Given I am logged in as "user"
	And I am at the Production/MaintenancePlanTaskType/Search page
	When I press Search
	Then I should be at the Production/MaintenancePlanTaskType page
	And I should see a link to the show page for maintenance plan task type: "one"
	And I should see a link to the show page for maintenance plan task type: "two"

Scenario: User can create a task type
	Given I am logged in as "user"
    When I visit the Production/MaintenancePlanTaskType/New page
	And I press "Save"
	Then I should see a validation message for Description with "The Description field is required."
	When I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Description field
	And I press "Save"
	Then I should see a validation message for Description with "The field Description must be a string with a maximum length of 50."
	When I enter "description here" into the Description field 
	And I press Save
	Then I should see a validation message for Abbreviation with "The Abbreviation field is required."
	When I enter "ABBR" into the Abbreviation field
	And I press "Save"
	Then I should see a validation message for IsActive with "The IsActive field is required."
	When I select "Yes" from the IsActive dropdown
	And I press Save
	Then I should see a display for Description with "description here"
	And I should see a display for IsActive with "Yes"

Scenario: User can edit a task type
	Given I am logged in as "user"
	When I visit the Edit page for maintenance plan task type: "one"
	And I enter "" into the Description field
	And I press "Save"
	Then I should see a validation message for Description with "The Description field is required."
	When I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Description field
	And I press "Save"
	Then I should see a validation message for Description with "The field Description must be a string with a maximum length of 50."
	When I enter "New description here" into the Description field 
	And I enter "" into the Abbreviation field
	And I press Save
	Then I should see a validation message for Abbreviation with "The Abbreviation field is required."
	When I enter "ABBR" into the Abbreviation field
	And I select "-- Select --" from the IsActive dropdown
	And I press "Save"
	And I select "-- Select --" from the IsActive dropdown
	And I press Save
	Then I should see a validation message for IsActive with "The IsActive field is required."
	When I select "No" from the IsActive dropdown
	And I press Save
    Then I should be at the Show page for maintenance plan task type: "one"
    And I should see a display for Description with "New description here"
	And I should see a display for IsActive with "No"
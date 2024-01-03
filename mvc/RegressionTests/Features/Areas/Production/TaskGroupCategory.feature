Feature: TaskGroupCategory
	In order to manage task group categories
	I need for the pages to not be broken
	So I can manage the task group categories

Background: IfIWasCreativeIWouldNameThisTaskGroupMaster
	Given an admin user "admin" exists with username: "admin"
	And a role "proddataadminread" exists with action: "Read", module: "ProductionDataAdministration"
	And a role "proddataadminadd" exists with action: "Add", module: "ProductionDataAdministration"
	And a role "proddataadminedit" exists with action: "Edit", module: "ProductionDataAdministration"
	And a user "user" exists with username: "user", roles: proddataadminread;proddataadminadd;proddataadminedit
	And a task group category "one" exists with description: "This is the description", type: "chemical", abbreviation: "CHEM", is active: "true"
	And a task group category "two" exists with description: "This is the description two", type: "electrical", abbreviation: "ELEC", is active: "true"

Scenario: Admin can view a task group category if they really wish hard enough
	Given I am logged in as "admin"
	And I am at the show page for task group category: "one"
	Then I should see a display for Description with "This is the description"
	And I should see a display for Type with "chemical"
	And I should see a display for Abbreviation with "CHEM"
	And I should see a display for IsActive with "Yes"

Scenario: Admin can edit a task group category, but probably shouldn't
	Given I am logged in as "admin"
	And I am at the show page for task group category "one"
	When I follow "Edit"
	And I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Description field
	And I press "Save"
	Then I should see a validation message for Description with "The field Description must be a string with a maximum length of 50."
	When I enter "Shorter text" into the Description field 
	And I enter "abcabc" into the Abbreviation field
	And I press "Save"
	Then I should see a validation message for Abbreviation with "The field Abbreviation must be a string with a maximum length of 4."
	When I enter "abc" into the Abbreviation field
	And I select "Yes" from the IsActive dropdown
	And I press "Save"
	Then I should be at the show page for task group category: "one"
	And I should see a display for Description with "Shorter text"
	And I should see a display for Abbreviation with "abc"
	And I should see a display for IsActive with "Yes"

Scenario: Admin can search for task group category
	Given I am logged in as "admin"
	And I am at the Production/TaskGroupCategory/Search page
	When I enter "electrical" into the Type field
	And I press "Search"
	Then I should be at the Production/TaskGroupCategory page
	And I should not see a link to the show page for task group category: "one"
	And I should see a link to the show page for task group category: "two"
	When I visit the Production/TaskGroupCategory/Search page
	And I press "Search"
	Then I should be at the Production/TaskGroupCategory page
	And I should see a link to the show page for task group category: "one"
	And I should see a link to the show page for task group category: "two"

Scenario: Admin can create a task group category
	Given I am logged in as "admin"
	When I visit the Production/TaskGroupCategory/New page
	And I press Save
	Then I should be at the Production/TaskGroupCategory/New page	
	And I should see the validation message "The Category Type field is required."
	And I should see the validation message "The Abbreviation field is required."
	And I should see the validation message "The IsActive field is required."
	And I should see the validation message "The Description field is required."	
	When I enter "Test Type" into the Type field
	And I enter "abb" into the Abbreviation field
	And I select "Yes" from the IsActive dropdown
	And I enter "Test Description" into the Description field
	And I press Save
	Then the currently shown task group category will now be known throughout the land as "taskcat"
	And I should be at the show page for task group category: "taskcat"
	And I should see a display for Type with "Test Type"
	And I should see a display for Abbreviation with "abb"
	And I should see a display for IsActive with "Yes"
	And I should see a display for Description with "Test Description"

Scenario: User can view a task group category
	Given I am logged in as "user"
	And I am at the show page for task group category: "two"
	Then I should see a display for Description with "This is the description two"
	And I should see a display for Type with "electrical"
	And I should see a display for Abbreviation with "ELEC"
	And I should see a display for IsActive with "Yes"

Scenario: User can edit a task group category
	Given I am logged in as "user"
	And I am at the show page for task group category "two"
	When I follow "Edit"
	When I enter "New Test Type" into the Type field
	When I enter "Description text" into the Description field 
	When I enter "abc" into the Abbreviation field
	And I select "No" from the IsActive dropdown 
	And I press "Save"
	Then I should be at the show page for task group category: "two"
	And I should see a display for Type with "New Test Type"
	And I should see a display for Description with "Description text"
	And I should see a display for Abbreviation with "abc"
	And I should see a display for IsActive with "No"

Scenario: User can search for task group category
	Given I am logged in as "user"
	And I am at the Production/TaskGroupCategory/Search page
	When I enter "chemical" into the Type field
	And I press "Search"
	Then I should be at the Production/TaskGroupCategory page
	And I should see a link to the show page for task group category: "one"
	And I should not see a link to the show page for task group category: "two"
	When I visit the Production/TaskGroupCategory/Search page
	And I press "Search"
	Then I should be at the Production/TaskGroupCategory page
	And I should see a link to the show page for task group category: "one"
	And I should see a link to the show page for task group category: "two"

Scenario: User can create a task group category
	Given I am logged in as "admin"
	When I visit the Production/TaskGroupCategory/New page
	And I press Save
	Then I should be at the Production/TaskGroupCategory/New page	
	And I should see the validation message "The Category Type field is required."
	And I should see the validation message "The Abbreviation field is required."
	And I should see the validation message "The IsActive field is required."
	And I should see the validation message "The Description field is required."	
	When I enter "Test Type 2" into the Type field
	And I enter "abb" into the Abbreviation field
	And I select "No" from the IsActive dropdown
	And I enter "Test Description 2" into the Description field
	And I press Save
	Then the currently shown task group category will now be known throughout the land as "taskcat2"
	And I should be at the show page for task group category: "taskcat2"
	And I should see a display for Type with "Test Type 2"
	And I should see a display for Abbreviation with "abb"
	And I should see a display for IsActive with "No"
	And I should see a display for Description with "Test Description 2"

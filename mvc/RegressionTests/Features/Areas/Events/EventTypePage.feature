Feature: EventTypePage

Background: users and supporting data exists
	Given a user "user" exists with username: "user"
	Given a user "nonuser" exists with username: "nonuser"
	And a role "roleRead" exists with action: "Read", module: "EventsEvents", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "EventsEvents", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "EventsEvents", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "EventsEvents", user: "user"
	And an event type "monolith" exists with id: 1, created by: "nielson jersey 4", description: "semi annual boat toss"

Scenario: user can view event type
	Given an event type "one" exists with created by: "nielson jersey 4", description: "Semi annual boat toss"
	And I am logged in as "user"
	When I visit the Events/EventType page
	When I visit the Show page for event type "one"
	Then I should see a display for CreatedBy with "nielson jersey 4"
	And I should see a display for Description with "Semi annual boat toss"

Scenario: user can add an event type
	Given I am logged in as "user"
	When I visit the Events/event type page
	And I follow "Add"
	And I press Save
	Then I should see the validation message "The Description field is required."
	When I enter "the main break because it was clogged with shoes" into the Description field
	And I enter "John frakes" into the CreatedBy field
	And I press "Save"
	And I wait for the page to reload
	Then I should see a display for Description with "the main break because it was clogged with shoes"
	And I should see a display for CreatedBy with "John frakes"

Scenario: user can edit an event
	Given an event type "one" exists with created by: "nielson jersey 4", description: "Semi annual boat toss"
	And I am logged in as "user"
	When I visit the Edit page for event type: "one"
	And I enter "person wanted to flush their shows" into the Description field
	And I enter "nielson jersey 4jr" into the CreatedBy field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for Description with "person wanted to flush their shows"
	And I should see a display for CreatedBy with "nielson jersey 4jr"

Scenario: user can destroy an event
	Given an event type "four" exists with created by: "nielson jersey 4", description: "Semi annual boat toss"
	And I am logged in as "user"
	When I visit the Show page for event type: "four"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Events/EventType page
	When I try to access the Show page for event type: "four" expecting an error
	Then I should see a 404 error message

Scenario: user without role cannot access the search/index/new/edit/show pages
	Given an event type "four" exists with created by: "nielson jersey 4", description: "Semi annual boat toss"
	And I am logged in as "nonuser"
	When I visit the Events/EventType/ page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Events/EventType/New page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Show page for event type: "four"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Edit page for event type: "four"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
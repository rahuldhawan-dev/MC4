Feature: EventPage

Background: users and supporting data exists
	Given a user "user" exists with username: "user"
	Given a user "nonuser" exists with username: "nonuser"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "roleRead" exists with action: "Read", module: "EventsEvents", user: "user", operating center: "nj7"
	And a role "roleEdit" exists with action: "Edit", module: "EventsEvents", user: "user", operating center: "nj7"
	And a role "roleAdd" exists with action: "Add", module: "EventsEvents", user: "user", operating center: "nj7"
	And a role "roleDelete" exists with action: "Delete", module: "EventsEvents", user: "user", operating center: "nj7"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a state "two" exists with name: "New York", abbreviation: "NY"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And an operating center "ny1" exists with opcode: "NY1", name: "Rockland, companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "two"
	And an employee status "active" exists with description: "Active"
	And a personnel area "one" exists
    And an employee "one" exists with operating center: "nj4", status: "active", personnel area: "one"
    And an employee "two" exists with operating center: "ny1", status: "active", personnel area: "one"
	And an event category "one" exists with id: 1, description: "categorized"
	And an event subcategory "one" exists with id: 1, description: "clog with things"
	And an event "monolith" exists with id: 1, operating center: "nj4", event category: "one", event subcategory: "one", start date: 10/2/2020, end date: 10/2/2020

Scenario: user can view event
	Given an event "one" exists with operating center: "nj7", event category: "one", event subcategory: "one", start date: 10/2/2020, end date: 10/2/2020
	And I am logged in as "user"
	When I visit the Events/Event page
	When I visit the Show page for event "one"
	Then I should see a display for StartDate with "10/2/2020 12:00:00 AM"
	And I should see a display for EndDate with "10/2/2020 12:00:00 AM"

Scenario: user can add an event
	Given I am logged in as "user"
	When I visit the Events/event page
	And I follow "Add"
	And I press Save
	Then I should see the validation message "The OperatingCenter field is required."
	Then I should see the validation message "The EventCategory field is required."
	And I should see the validation message "The EventSubcategory field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select event category "one" from the EventCategory dropdown
	And I select event subcategory "one" from the EventSubcategory dropdown
	And I enter "2/4/2014" into the StartDate field
	And I enter "2/4/2014" into the EndDate field
	And I enter "the main break because it was clogged with shoes" into the EventSummary field
	And I enter "1234567" into the NumberCustomersImpacted field
	And I enter "7654321" into the EstimatedDurationHours field
	And I press "Save"
	And I wait for the page to reload
	Then I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for EventCategory with event category "one"
	And I should see a display for EventSubcategory with event subcategory "one"
	And I should see a display for StartDate with "2/4/2014 12:00:00 AM"
	And I should see a display for EndDate with "2/4/2014 12:00:00 AM"
	And I should see a display for EventSummary with "the main break because it was clogged with shoes"
	And I should see a display for NumberCustomersImpacted with "1234567"
	And I should see a display for EstimatedDurationHours with "7654321"

Scenario: user can edit an event
	Given an event "one" exists with operating center: "nj7", event category: "one", event subcategory: "one", start date: 10/2/2020, end date: 10/2/2020
	And I am logged in as "user"
	When I visit the Edit page for event: "one"
	And I enter "person wanted to flush their shows" into the RootCause field
	And I enter "90001" into the EstimatedDurationHours field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for EventCategory with event category "one"
	And I should see a display for EventSubcategory with event subcategory "one"
	And I should see a display for StartDate with "10/2/2020 12:00:00 AM"
	And I should see a display for EndDate with "10/2/2020 12:00:00 AM"
	And I should see a display for EstimatedDurationHours with "90001"

Scenario: user can destroy an event
	Given an event "four" exists with operating center: "nj7", event category: "one", event subcategory: "one", start date: 10/2/2020, end date: 10/2/2020
	And I am logged in as "user"
	When I visit the Show page for event: "four"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Events/Event/Search page
	When I try to access the Show page for event: "four" expecting an error
	Then I should see a 404 error message

Scenario: user without role cannot access the search/index/new/edit/show pages
	Given an event "four" exists with operating center: "nj7", event category: "one", event subcategory: "one", start date: 10/2/2020, end date: 10/2/2020
	And I am logged in as "nonuser"
	When I visit the Events/Event/Search page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Events/Event/ page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Events/Event/New page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Show page for event: "four"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Edit page for event: "four"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
Feature: EventDocumentPage

Background: users and supporting data exists
	Given a user "user" exists with username: "user"
	Given a user "nonuser" exists with username: "nonuser"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrews7bury"
	And a role "roleRead" exists with action: "Read", module: "EventsEvents", user: "user", operating center: "nj7"
	And a role "roleEdit" exists with action: "Edit", module: "EventsEvents", user: "user", operating center: "nj7"
	And a role "roleAdd" exists with action: "Add", module: "EventsEvents", user: "user", operating center: "nj7"
	And a role "roleDelete" exists with action: "Delete", module: "EventsEvents", user: "user", operating center: "nj7"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a town "one" exists with stateId: "one"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "PPF1"
	And an operating center "nj4" exists with opcode: "NJ7", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And an operating center "ny1" exists with opcode: "NY1", name: "Rockland, companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one"
	And a facility "two" exists with operating center: "nj7", facility name: "NJ Facility"
	And a facility "one" exists with operating center: "nj7", facility name: "NJ Facility", town: "one", planning plant: "one"
	And an event type "one" exists with created by: "nielson jersey 4", description: "Semi annual boat toss"
	And an event type "two" exists with created by: "nielson jersey 4", description: "annual boat toss"
	And an event document "monolith" exists with operating center: "nj7", facility: "one", description: "boats tossed", event type: "one"

Scenario: user can view event document
	Given an event document "one" exists with operating center: "nj7", facility: "one", event type: "one", description: "boats tossed"
	And I am logged in as "user"
	When I visit the Events/EventDocument page
	When I visit the Show page for event document "one"
	Then I should see a display for Description with "boats tossed"

Scenario: user can add an event document
	Given I am logged in as "user"
	When I visit the Events/event document page
	And I follow "Add"
	And I press Save
	Then I should see the validation message "The OperatingCenter field is required."
	Then I should see the validation message "The EventType field is required."
	And I should see the validation message "The Description field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select event type "one"'s Description from the EventType dropdown
	And I enter "the main break because it was clogged with shoes" into the Description field
	And I press "Save"
	And I wait for the page to reload
	Then I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for Description with "the main break because it was clogged with shoes"

Scenario: user can edit an event document
	Given an event document "one" exists with operating center: "nj7", description: "boats tossed", event type: "one"
	And I am logged in as "user"
	When I visit the Edit page for event document: "one"
	And I enter "person wanted to flush their shows" into the Description field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for Description with "person wanted to flush their shows"

Scenario: user can destroy an event document
	Given an event document "four" exists with operating center: "nj7", description: "boats tossed", event type: "one"
	And I am logged in as "user"
	When I visit the Show page for event document: "four"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Events/EventDocument/Search page
	When I try to access the Show page for event document: "four" expecting an error
	Then I should see a 404 error message

Scenario: user without role cannot access the search/index/new/edit/show pages
	Given an event document "four" exists with operating center: "nj7", description: "boats tossed", event type: "one"
	And I am logged in as "nonuser"
	When I visit the Events/EventDocument/Search page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Events/EventDocument/ page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Events/EventDocument/New page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Show page for event document: "four"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
	When I visit the Edit page for event document: "four"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "Events"
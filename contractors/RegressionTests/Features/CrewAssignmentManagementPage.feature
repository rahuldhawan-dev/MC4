Feature: Crew Assignments Management Page
    In order to 
    As a user
    I want to be

Background: admin user exists
    Given an operating center "one" exists with opcntr: "NJ7", opcntrname: "Shrewsburgh"
    And a contractor "one" exists with name: "one", operating center: "one"
    And a contractor "bad contractor" exists with name: "bad contractor", operating center: "one"
    And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
    And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
    And a crew "one" exists with description: "one", contractor: "one", availability: "8"
    And a hydrant work description "one" exists with time to complete: "2"
    And a hydrant work description "two" exists with time to complete: "5"
    And a town "Foo" exists with shortname: "Foo", operating center: "one"
    And a town "Foo Two" exists with shortname: "Foo Two", operating center: "one"
    And a town section "Little Foo" exists with name: "Little Foo", town: "Foo"
    And a town section "Electric Foogaloo" exists with name: "Electric Foogaloo", town: "Foo Two"
    And a street "one" exists with name: "Street One", town: "Foo"
    And a street "two" exists with name: "Street Two", town: "Foo Two"
    And a work order "one" exists with street number: "12345", hydrant work description: "one", contractor: "one", town: "Foo", town section: "Little Foo", street: "one"
    And a work order "two" exists with street number: "678910", hydrant work description: "two", contractor: "one", town: "Foo Two", town section: "Electric Foogaloo", street: "two"
    And a crew assignment "one" exists with crew: "one", work order: "one", assignedfor: "today", priority: "1"
    And a crew assignment "two" exists with crew: "one", work order: "two", assignedfor: "today", priority: "2"

Scenario: admin user visits Crew Assignments Management page
    Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the CrewAssignment/ShowCalendar page 
	And I have selected crew "one" from the Crew dropdown
	And I have entered "today's date" into the Date field
	And I have pressed "Search"
	When I follow "Manage Crew Assignments"
    Then I should see work order "one"'s Id in the table assignmentsTable's "Order Number" column
    And I should see crew assignment "one"'s Priority in the table assignmentsTable's "Priority" column
    And I should see work order "one"'s StreetNumber in the table assignmentsTable's "Street Number" column
    And I should see street "one"'s FullStName in the table assignmentsTable's "Street" column
    And I should see town "Foo"'s ShortName in the table assignmentsTable's "Town" column
    And I should see town section "Little Foo"'s Name in the table assignmentsTable's "Town Section" column
    And I should see work order "two"'s Id in the table assignmentsTable's "Order Number" column
    And I should see crew assignment "two"'s Priority in the table assignmentsTable's "Priority" column
    And I should see work order "two"'s StreetNumber in the table assignmentsTable's "Street Number" column
    And I should see street "two"'s FullStName in the table assignmentsTable's "Street" column
    And I should see town "Foo Two"'s ShortName in the table assignmentsTable's "Town" column
    And I should see town section "Electric Foogaloo"'s Name in the table assignmentsTable's "Town Section" column

Scenario: user tries visiting the Crew Assignments Management page
    Given I am logged in as "user@site.com", password: "testpassword#1"
    When I try to access the CrewAssignment/Manage page for crew: "one" with date: "today"
    Then I should be at the forbidden screen

Scenario: admin user deletes a crew assignment
    Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the CrewAssignment/ShowCalendar page 
	And I have selected crew "one" from the Crew dropdown
	And I have entered "today's date" into the Date field
	And I have pressed "Search"
	When I follow "Manage Crew Assignments"
    And I click ok in the dialog after pressing the Remove link for crew assignment: "two"
    And I wait for the page to reload
	And I wait for ajax to finish loading
    Then I should see work order "one"'s Id in the table assignmentsTable's "Order Number" column
    And I should see crew assignment "one"'s Priority in the table assignmentsTable's "Priority" column
    And I should see work order "one"'s StreetNumber in the table assignmentsTable's "Street Number" column
    And I should see street "one"'s FullStName in the table assignmentsTable's "Street" column
    And I should see town "Foo"'s ShortName in the table assignmentsTable's "Town" column
    And I should see town section "Little Foo"'s Name in the table assignmentsTable's "Town Section" column
    And I should not see work order "two"'s Id in the table assignmentsTable's "Order Number" column
    And I should not see crew assignment "two"'s Priority in the table assignmentsTable's "Priority" column
    And I should not see work order "two"'s StreetNumber in the table assignmentsTable's "Street Number" column
    And I should not see street "two"'s FullStName in the table assignmentsTable's "Street" column
    And I should not see town "Foo Two"'s ShortName in the table assignmentsTable's "Town" column
    And I should not see town section "Electric Foogaloo"'s Name in the table assignmentsTable's "Town Section" column

Scenario: admin user changes the order of priorities for crew assignments and hits ok
    Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the CrewAssignment/ShowCalendar page 
	And I have selected crew "one" from the Crew dropdown
	And I have entered "today's date" into the Date field
	And I have pressed "Search"
	When I follow "Manage Crew Assignments"
	And I drag the 2rd row in the "assignmentsTable" table up 1 row
    And I click ok in the dialog after pressing "Save Crew Assignment Priorities"
	# A wait is needed here. I do not know why.
	And I wait 1 second
    Then the Priority value for crew assignment "two" should be "1"
    And the Priority value for crew assignment "one" should be "2"
    And I should be at the CrewAssignment/ShowCalendar page
	And crew "one"'s Description should be selected in the Crew dropdown
	And I should see "today's date" in the Date field

Scenario: admin user saves but hits cancel
    Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the CrewAssignment/ShowCalendar page 
	And I have selected crew "one" from the Crew dropdown
	And I have entered "today's date" into the Date field
	And I have pressed "Search"
	When I follow "Manage Crew Assignments"
    And I drag the 2rd row in the "assignmentsTable" table up 1 row
    And I click cancel in the dialog after pressing "Save Crew Assignment Priorities"
    Then the Priority value for crew assignment "one" should be "1"
    And the Priority value for crew assignment "two" should be "2"
    And I should be at the CrewAssignment/Manage page
	And I should see a display for CrewDescription with crew "one"'s Description
	And I should see a display for Date with "today's date"

Feature: Crew Assignments Page
    In order to 
	As a user
	I want to be

Background: admin user exists
	Given a contractor "one" exists with name: "one"
	And a contractor "bad contractor" exists with name: "bad contractor"
	And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
    And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
	And a crew "one" exists with description: "one", contractor: "one", availability: "8"
	And a crew "bad crew" exists with description: "bad crew", contractor: "bad contractor"

Scenario: admin user visits Crew Assignments page
    Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	Then I should see crew "one"'s Description in the Crew dropdown
	And I should not see crew "bad crew"'s Description in the Crew dropdown
	
Scenario: user visits Crew Assignments page
    Given I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	Then I should see crew "one"'s Description in the Crew dropdown
	And I should not see crew "bad crew"'s Description in the Crew dropdown

Scenario: user should not see manage link
    Given a work order "one" exists with street number: "STREETERIFIC!"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
    And I am logged in as "user@site.com", password: "testpassword#1"
	When I visit the CrewAssignment/ShowCalendar page for crew: "one" with date: "1/1/2000"
	Then I should not see "Manage Crew Assignments"

Scenario: admin user clicks the "Manage" button.
    Given a work order "one" exists with street number: "STREETERIFIC!"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
    And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the CrewAssignment/ShowCalendar page for crew: "one" with date: "1/1/2000"
	When I select crew "one" from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press "Search"
	And I follow "Manage Crew Assignments"
	Then I should be at the CrewAssignment/Manage page
	And I should see a display for CrewDescription with crew "one"'s Description
	And I should see a display for Date with "1/1/2000"

Scenario: user selects crew with no assignments and a date does not see assignments
    Given I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press Search
	Then I should see "No assignments to display." in the assignments element

Scenario: user selects crew with assignments and a date sees assignments
	Given a work order "one" exists with street number: "STREETERIFIC!"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press Search
	Then I should see "STREETERIFIC!"

Scenario: user views crews that are completed
	Given a work order "one" exists with street number: "STREETERIFIC!", date completed: "1/1/2000"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000", assigned on: "1/1/2000"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press Search
	Then the assignments table row should have the css class "completed"
	
Scenario: user tries to search but does not enter a date    
	Given I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter " " into the Date field
	And I press Search
	Then I should see the validation message "The Date field is required."

Scenario: user tries to search but does not select a crew   
	Given I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I enter "1/1/2000" into the Date field
	And I press Search
	Then I should see the validation message "The Crew field is required."

Scenario: user starts a crew assignment
	Given a hydrant work description "one" exists with time to complete: "0"
	And a finalization work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one", contractor: "one"
	# if we don't delete this will be considered the "current" markout
    And I have deleted the default markout for finalization work order "one"
    And a markout "one" exists with ready date: "yesterday", expiration date: "tomorrow", finalization work order: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "today", assigned on: "yesterday"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
    # expiration date should be dbnull
    Then the ExpirationDate date value for markout "one" should be the special date value "tomorrow"
	When I select crew "one"'s Description from the Crew dropdown
	And I enter the date "today" into the Date field
	And I press Search
	And I click the start assignment link for crew assignment: "ca"
	Then I should be at the edit page for finalization work order: "one"
	And I should not see "Cannot start work. A markout is required but no valid markout exists."
	When I click the "Markouts" tab
	And I wait for ajax to finish loading
	Then I should not see "Edit"
	And I should not see "Delete"
    And the ExpirationDate date value for markout "one" should not be the special date value "tomorrow"

Scenario: user starts and ends a future crew assignment
	Given a scheduling work order with valve "one" exists with street number: "STREETERIFIC!", hydrant work description: "one", contractor: "one"
    And a markout "one" exists with ready date: "yesterday", expiration date: "10 days from now", scheduling work order with valve: "one"
    And a crew assignment "ca" exists with scheduling work order with valve: "one", crew: "one", assigned for: "tomorrow", assigned on: "yesterday"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter the date "tomorrow" into the Date field
	And I press Search
	And I click the start assignment link for crew assignment: "ca"
	Then I should not see "No such work order."
	When I click the "Crew Assignments" tab
	And I wait for ajax to finish loading
	Then I should see the end assignment link for crew assignment: "ca"
	When I enter "2" into the EmployeesOnJob field
	And I press the end assignment link for crew assignment: "ca"
	Then I should not see "The EmployeesOnJob field is required"
	When I click the "Crew Assignments" tab
	And I wait for ajax to finish loading
	Then I should not see the end assignment link for crew assignment: "ca"
	And I should see "2" in the table assignmentsTable's "Employees On Crew" column
		
Scenario: user ends a crew assignment
	Given a hydrant work description "one" exists with time to complete: "0"
	And a finalization work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one", contractor: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984", date started: "4/24/1984"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	And I enter "42" into the EmployeesOnJob field 
	And I press the end assignment link for crew assignment: "ca"
	Then I should be at the edit page for finalization work order: "one"

Scenario: user does not enter employees on crew when ending a crew assignment
	Given a hydrant work description "one" exists with time to complete: "0", contractor: "one"
	And a finalization work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one", contractor: "one"
    And a crew assignment "ca" exists with finalization work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984", date started: "4/24/1984"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	And I press the end assignment link for crew assignment: "ca"
	Then I should see the validation message "The EmployeesOnJob field is required."
	
Scenario: user should not see a crew assignment start link that has not been assigned to their contractor
    Given a hydrant work description "one" exists with time to complete: "0"
	And a work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one", contractor: "bad contractor"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then I should not see a link to the start page for crew assignment: "ca"

Scenario: user should not see a crew assignment start link for an assignment in the future
    Given a hydrant work description "one" exists with time to complete: "0"
	And a work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one", contractor: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "tomorrow", assigned on: "tomorrow"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter the date "tomorrow" into the Date field
	And I press Search
	Then I should not see a link to the start page for crew assignment: "ca"

Scenario: user sees a crew assignment that has not been started
    Given a hydrant work description "one" exists with time to complete: "0"
	And a work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one", contractor: "one", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	And I wait 1 seconds
	Then I should see the start assignment link for crew assignment: "ca"

Scenario: user visits Crew Assignments page and there is 0% crew availability for date
    Given a hydrant work description "one" exists with time to complete: "0"
	And a work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then the calendar date for "4/24/1984" should have the css class "day-0"
	
Scenario: user visits Crew Assignments page and there is 0-50% crew availability for date
    Given I am logged in as "user@site.com", password: "testpassword#1"
	And a hydrant work description "one" exists with time to complete: "2"
	And a work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then the calendar date for "4/24/1984" should have the css class "day-0-50"

Scenario: user visits Crew Assignments page and there is 50-100% crew availability for date
    Given a hydrant work description "one" exists with time to complete: "6"
	And a work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then the calendar date for "4/24/1984" should have the css class "day-50-100"
	
Scenario: user visits Crew Assignments page and there is 100% crew availability for date
    Given a hydrant work description "one" exists with time to complete: "8"
	And a work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then the calendar date for "4/24/1984" should have the css class "day-100"

Scenario: user visits Crew Assignments page and should see a link to the print page for each crew assignment
    Given a hydrant work description "one" exists with time to complete: "8"
	And a read only work order "one" exists with street number: "STREETERIFIC!", hydrant work description: "one", contractor: "one"
    And a crew assignment "ca" exists with read only work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then I should see a link to the show page for read only work order: "one"

Scenario: user should see a start link for an emergency markout order 
	Given a work order priority "one" exists with description: "Emergency"
	And a planning work order with service "one" exists with contractor: "one", premise number: "123456789", service number: "12345678", markout requirement: "emergency", priority: "emergency"
	And a crew assignment "ca" exists with planning work order with service: "one", crew: "one", assigned on: "today", assigned for: "today"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Description from the Crew dropdown
	And I enter the date "today" into the Date field
	And I press Search
	Then I should see the start assignment link for crew assignment: "ca"
	
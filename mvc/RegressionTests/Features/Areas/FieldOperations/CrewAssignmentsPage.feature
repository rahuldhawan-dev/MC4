Feature: Crew Assignments Page
    In order to 
	As a user
	I want to be

Background: admin user exists
	Given an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-edit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a crew "one" exists with description: "one", availability: "8", operating center: "nj7", active: true
	And a crew "no operating center" exists with description: "no operating center", active: true
	And a crew "inactive" exists with description: "inactive", operating center: "nj7", active: false
	And a work description "hydrant installation" exists with description: "hydrant installation"
	And a work description "hydrant investigation" exists with description: "hydrant investigation"
	And a work description "hydrant leaking" exists with description: "hydrant leaking"
	And a markout requirement "none" exists with description: "none"
	And a markout requirement "emergency" exists with description: "emergency"
	And a markout requirement "routine" exists with description: "routine"
	And a street "one" exists
	And a street "two" exists

Scenario: admin user visits Crew Assignments page
    Given I am logged in as "admin"
	And I am at the crew assignments calendar page
	Then I should see crew "one"'s Display in the Crew dropdown
	And I should not see crew "no operating center"'s Display in the Crew dropdown
	And I should not see crew "inactive"'s Display in the Crew dropdown
	
Scenario: user visits Crew Assignments page
    Given I am logged in as "user"
	And I am at the crew assignments calendar page
	Then I should see crew "one"'s Display in the Crew dropdown
	And I should not see crew "no operating center"'s Display in the Crew dropdown
	And I should not see crew "inactive"'s Display in the Crew dropdown
	And I should not see a link to the FieldOperations/CrewAssignment/Index page

Scenario: admin user clicks the "Manage" button
    Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
    And I am logged in as "admin"
	And I am at the FieldOperations/CrewAssignment/ShowCalendar page for crew: "one" with date: "1/1/2000"
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press "Search"
	And I follow "Manage Crew Assignments"
	Then I should be at the FieldOperations/CrewAssignment/Manage page
	And I should see a display for CrewDescription with crew "one"'s Description
	And I should see a display for Date with "1/1/2000"

Scenario: user clicks the "Manage" button
    Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
    And I am logged in as "admin"
	And I am at the FieldOperations/CrewAssignment/ShowCalendar page for crew: "one" with date: "1/1/2000"
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press "Search"
	And I follow "Manage Crew Assignments"
	Then I should be at the FieldOperations/CrewAssignment/Manage page
	And I should see a display for CrewDescription with crew "one"'s Description
	And I should see a display for Date with "1/1/2000"

Scenario: user selects crew with no assignments and a date does not see assignments
    Given I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press Search
	Then I should see "No assignments to display." in the assignments element

Scenario: user selects crew with assignments and a date sees assignments
	Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none", street: "one", nearest cross street: "two"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press Search
	Then I should see the following values in the assignmentsTable table
        |        |       | Order Number | SAP WorkOrder # | SAP Notification # | Priority | Street Number | Street | Cross Street | Town | Town Section | Description of Job (Hover for Notes) | Est. TTC (hours) | Priority | Markout Ready | Markout Expiration | Assigned On | Start Time | End Time | Employees on Crew |
        | Select | Print | 1            |                 |                    | 1        | 1234          | *      | *            | *    |   *          | HYDRANT INSTALLATION                 | 3.50             | Routine  |               |                    | 1/1/2000 12:00:00 AM (EST)| Start      |          |					|

Scenario: user views crews that are completed
	Given a work order "one" exists with date completed: "1/1/2000", operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000", assigned on: "1/1/2000"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "1/1/2000" into the Date field
	And I press Search
	Then the assignments table row should have the css class "completed"
	
Scenario: user tries to search but does not enter a date    
	Given I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter " " into the Date field
	And I press Search
	Then I should see the validation message "The Date field is required."

Scenario: user tries to search but does not select a crew   
	Given I am logged in as "user"
	And I am at the crew assignments calendar page
	When I enter "1/1/2000" into the Date field
	And I press Search
	Then I should see the validation message "The Crew field is required."

Scenario: user starts a crew assignment
	Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a markout "one" exists with ready date: "yesterday", expiration date: "tomorrow", work order: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "today", assigned on: "yesterday"
	And a job site check list "one" exists with safety brief date time: "10/26/2020", map call work order: "one"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
    # expiration date should be dbnull
    Then the ExpirationDate date value for markout "one" should be the special date value "tomorrow"
	When I select crew "one"'s Display from the Crew dropdown
	And I enter the date "today" into the Date field
	And I press Search
	And I click the start assignment link for crew assignment: "ca"
	Then I should be at the HealthAndSafety/JobSiteCheckList/New page
	And I should see operating center "nj7"'s ToString in the OperatingCenter dropdown
	And I should see work order "one"'s Id in the MapCallWorkOrder field
	# TODO: this won't work until there's an edit page in mvc
	#And I should not see "Cannot start work. A markout is required but no valid markout exists."
	#When I click the "Markouts" tab
	#And I wait for ajax to finish loading
	#Then I should not see "Edit"
	#And I should not see "Delete"
    #And the ExpirationDate date value for markout "one" should not be the special date value "tomorrow"

Scenario: user starts a crew assignment by double clicking the link
	Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a markout "one" exists with ready date: "yesterday", expiration date: "tomorrow", work order: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "today", assigned on: "yesterday"
	And a job site check list "one" exists with safety brief date time: "10/26/2020", map call work order: "one"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
    # expiration date should be dbnull
    Then the ExpirationDate date value for markout "one" should be the special date value "tomorrow"
	When I select crew "one"'s Display from the Crew dropdown
	And I enter the date "today" into the Date field
	And I press Search
	And I double click the start assignment link for crew assignment: "ca"
	Then I should be at the HealthAndSafety/JobSiteCheckList/New page
	And I should see operating center "nj7"'s ToString in the OperatingCenter dropdown
	And I should see work order "one"'s Id in the MapCallWorkOrder field

Scenario: user starts and ends a future crew assignment
	Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a markout "one" exists with ready date: "yesterday", expiration date: "10 days from now", work order: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "tomorrow", assigned on: "yesterday"
	And a job site check list "one" exists with safety brief date time: "10/26/2020", map call work order: "one"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter the date "tomorrow" into the Date field
	And I press Search
	And I click the start assignment link for crew assignment: "ca"
	Then I should not see "No such work order."
	# TODO: this expects the user to be on an edit page for the order
	#When I click the "Crew Assignments" tab
	#And I wait for ajax to finish loading
	#Then I should see the end assignment link for crew assignment: "ca"
	#When I enter "2" into the EmployeesOnJob field
	#And I press the end assignment link for crew assignment: "ca"
	#Then I should not see "The EmployeesOnJob field is required"
	#When I click the "Crew Assignments" tab
	#And I wait for ajax to finish loading
	#Then I should not see the end assignment link for crew assignment: "ca"
	#And I should see "2" in the table assignmentsTable's "Employees On Crew" column
		
Scenario: user ends a crew assignment
	Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984", date started: "4/24/1984"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	And I enter "42" into the EmployeesOnJob field 
	And I press the end assignment link for crew assignment: "ca"
	Then I should be at the FieldOperations/WorkOrderFinalization/Edit page for work order: "one"

Scenario: user does not enter employees on crew when ending a crew assignment
	Given a work order "one" exists with operating center: "nj7", work description: "hydrant investigation", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984", date started: "4/24/1984"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	And I press the end assignment link for crew assignment: "ca"
	Then I should see the validation message "The EmployeesOnJob field is required."

Scenario: user should not see a crew assignment start link for an assignment with an expired markout
    Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "routine"
	And a markout "one" exists with ready date: "yesterday", expiration date: "yesterday", work order: "one"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "today", assigned on: "yesterday"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter the date "today" into the Date field
	And I press Search
	Then I should not see a secure link to the start page for crew assignment: "ca"

Scenario: user sees a crew assignment that has not been started
    Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	And I wait 1 seconds
	Then I should see the start assignment link for crew assignment: "ca"
	
Scenario: user visits Crew Assignments page and there is 0-50% crew availability for date
    Given I am logged in as "user"
	And a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then the calendar date for "4/24/1984" should have the css class "day-0-50"

Scenario: user visits Crew Assignments page and there is 50-100% crew availability for date
    Given a work order "one" exists with operating center: "nj7", work description: "hydrant leaking", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then the calendar date for "4/24/1984" should have the css class "day-50-100"
	
Scenario: user visits Crew Assignments page and there is 0% crew availability for date
    Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a work order "two" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a work order "three" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
    And a crew assignment "ca2" exists with work order: "two", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
    And a crew assignment "ca3" exists with work order: "three", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
	And I am logged in as "user"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter "4/24/1984" into the Date field
	And I press Search
	Then the calendar date for "4/24/1984" should have the css class "day-100"

# TODO: this requires the "print" link to link to an mvc page
#Scenario: user visits Crew Assignments page and should see a secure link to the print page for each crew assignment
#    Given a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none"
#    And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "4/24/1984", assigned on: "4/24/1984"
#	And I am logged in as "user"
#	And I am at the crew assignments calendar page
#	When I select crew "one"'s Display from the Crew dropdown
#	And I enter "4/24/1984" into the Date field
#	And I press Search
#	Then I should see a secure link to the show page for work order: "one"

Scenario: user should see a start link for an emergency markout order 
	Given a work order priority "one" exists with description: "Emergency"
	And a work order "one" exists with premise number: "123456789", service number: "12345678", markout requirement: "emergency", priority: "emergency", operating center: "nj7", work description: "hydrant installation"
	And a crew assignment "ca" exists with work order: "one", crew: "one", assigned on: "today", assigned for: "today"
	And I am logged in as "admin"
	And I am at the crew assignments calendar page
	When I select crew "one"'s Display from the Crew dropdown
	And I enter the date "today" into the Date field
	And I press Search
	Then I should see the start assignment link for crew assignment: "ca"
	
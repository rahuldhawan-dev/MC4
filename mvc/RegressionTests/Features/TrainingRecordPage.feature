Feature: TrainingRecordPage
	Stuart a little less
	Verm-in-ant Vacation
	Murder she roach

Background: users and supporting data exists
	Given an operating center "opc" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an admin user "admin" exists with username: "admin"
	And a state "one" exists with abbreviation: "QQ", name: "Q State"
	And an operating center "one" exists with opcode: "FFS", name: "Shrewsburgh", state: "one"
	And a town "lazytown" exists
	And a role "roleUserAdmin" exists with action: "UserAdministrator", module: "OperationsTrainingRecords", operating center: "one"
	And a role "roleRead" exists with action: "Read", module: "OperationsTrainingRecords", operating center: "one"
	And a role "roleEdit" exists with action: "Edit", module: "OperationsTrainingRecords", operating center: "one"
	And a role "roleAdd" exists with action: "Add", module: "OperationsTrainingRecords", operating center: "one"
	And a role "roleDelete" exists with action: "Delete", module: "OperationsTrainingRecords", operating center: "one"
	And a role "roleUnionRead" exists with action: "Read", module: "HumanResourcesUnion", operating center: "one"
	And a user "user" exists with username: "user", roles: roleRead;roleEdit;roleAdd;roleDelete;roleUnionRead
	And a user "lori" exists with username: "lori", roles: roleUserAdmin;roleRead;roleEdit;roleAdd;roleDelete;roleUnionRead
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a user "mcuser" exists with username: "mcuser"
	And a role "roleRead2" exists with action: "Read", module: "OperationsTrainingRecords", user: "mcuser"
	And a training module "one" exists with title: "training module one"
	And a data type "employees attended" exists with table name: "tblTrainingRecords", name: "Employees Attended"
	And a data type "employees scheduled" exists with table name: "tblTrainingRecords", name: "Employees Scheduled"
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "one", status: "active"
	And an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "11111117", operating center: "one", status: "active"
	And a class location "one" exists with Description: "Down the hall", operating center: "one"
	And a training contact hours program coordinator "one" exists

Scenario: training module user cannot access training records
	Given a role "roleReadModules" exists with action: "Read", module: "OperationsTrainingModules", operating center: "one"
	And a user "modules" exists with username: "modules", roles: roleReadModules
	And a training record "one" exists with training module: "one", held on: "today"
	And I am logged in as "modules"
	When I visit the Show page for training record: "one"
	Then I should see "You do not have the roles necessary to access this resource."

Scenario: user can view training records
	Given a training record "one" exists with training module: "one", held on: "today"
	And I am logged in as "mcuser"
	When I visit the TrainingRecord/Search page
	And I press Search
    Then I should be at the TrainingRecord page
	And I should see a link to the Show page for training module "one"
	And I should see training record "one"'s HeldOn as a date in the "Held On" column

Scenario: user can schedule attendees and send notifications
	Given a training record "one" exists with training module: "one", scheduled date: "tomorrow", maximum class size: 2
	And a notification purpose "one" exists with purpose: "Training Record", module: "OperationsTrainingRecords"
	And I am logged in as "user"
	When I visit the Show page for training record: "one"
	And I click the "Employees Scheduled (0)" tab
	And I press "Link Employee" 
    And I click the checkbox named EmployeeIds with employee "one"'s Id under the "Employees Scheduled (0)" tab
    And I press "Link Employees" 
    Then I should be at the Show page for training record: "one" on the Employees tab
    And I should see employee "one"'s EmployeeId in the table employeesTable's "Employee ID" column under the "Employees Scheduled (1)" tab
	When I click the "Employees Scheduled (1)" tab
	And I click ok in the dialog after pressing "Send Notification"
	Then I should be at the Show page for training record: "one" on the Employees tab
	And I should see "Notification Sent"

Scenario: user cannot schedule more attendees once maximum class size has been reached
	Given a training record "one" exists with training module: "one", scheduled date: "tomorrow", maximum class size: 1
	And I am logged in as "user"
	When I visit the Show page for training record: "one"
	And I click the "Employees Scheduled (0)" tab
	And I press "Link Employee" 
    And I click the checkbox named EmployeeIds with employee "one"'s Id under the "Employees Scheduled (0)" tab
    And I press "Link Employees"
    Then I should be at the Show page for training record: "one" on the Employees tab
    When I click the "Employees Scheduled (1)" tab
    Then I should not see the button "Link Employee" under the "Employees Scheduled (1)" tab

Scenario: user admin can record attending employees
	# is open needs to be false so that the "Employees Scheduled" tab is put into readonly mode.
	# otherwise there are two "Link Employee" buttons and this test fails cause it finds the hidden one first.
	Given a training record "one" exists with training module: "one", held on: "today", is open: "false"
	And I am logged in as "lori"
	When I visit the Show page for training record: "one"
	And I click the "Employees Attended (0)" tab
	And I wait 1 second
	#And I press "Link Employee" under the "Employees Attended (0)" tab
	And I press "Link Employee" 
    And I click the checkbox named EmployeeIds with employee "one"'s Id under the "Employees Attended (0)" tab
   # And I press "Link Employees" under the "Employees Attended (0)" tab
    And I press "Link Employees" 
    Then I should be at the Show page for training record: "one" on the Employees tab
    And I should see employee "one"'s EmployeeId in the table employeesTable's "Employee ID" column under the "Employees Attended (1)" tab

Scenario: regular user cannot record attending employees
	Given a training record "one" exists with training module: "one", held on: "today"
	And I am logged in as "user"
	When I visit the Show page for training record: "one"
	And I click the "Employees Attended (0)" tab
    Then I should not see the button "Link Employee" under the "Employees Attended (0)" tab

Scenario: user can add a training record
	Given I am logged in as "user"
	When I visit the TrainingRecord/New page
	And I enter "none" into the OutsideInstructor field
	And I press Save
	Then I should see the validation message "The OutsideInstructorTitle field is required."
    And I should see the validation message "The ScheduledDate field is required."
    And I should see "10" in the MaximumClassSize field
    When I enter today's date into the ScheduledDate field
	And I enter "trai" and select training module "one"'s Display from the TrainingModule combobox
	And I enter today's date into the HeldOn field
	And I select employee "one"'s Description from the Instructor dropdown
	And I select employee "two"'s Description from the SecondInstructor dropdown
	And I enter "theless" into the OutsideInstructorTitle field
	And I select class location "one"'s Description from the ClassLocation dropdown
	And I enter "this is course location in the db" into the CourseLocation field
	And I select training contact hours program coordinator "one"'s Description from the ProgramCoordinator dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown training record shall henceforth be known throughout the land as "one"
	And I should see a display for training record: "one"'s HeldOn
	And I should see a link to the Show page for training module: "one"
	And I should see a display for Instructor with "Theodore Logan"
	And I should see a display for SecondInstructor with "Bill Preston"
	And I should see a display for OutsideInstructor with "none"
	And I should see a display for OutsideInstructorTitle with "theless"
	And I should see a display for ClassLocation with "FFS - Down the hall"
	And I should see a display for CourseLocation with "this is course location in the db"
    And I should see a display for MaximumClassSize with "10"
    And I should see a display for ScheduledDate with today's date
	And I should see a display for "ProgramCoordinator" with training contact hours program coordinator: "one"'s Description
	
Scenario: user can edit a training record
	Given a training module "two" exists with title: "training module two"
	And a training record "one" exists with training module: "two", held on: "yesterday"
	And I am logged in as "user"
	When I visit the Show page for training record: "one"
	And I follow "Edit"
	And I enter "trai" and select training module "one"'s Display from the TrainingModule combobox
    And I enter "10" into the MaximumClassSize field
	And I enter "4/1/2014" into the ScheduledDate field
	And I enter "4/1/2014" into the HeldOn field
	And I select employee "one"'s Description from the Instructor dropdown
	And I select employee "two"'s Description from the SecondInstructor dropdown
	And I enter "none" into the OutsideInstructor field
	And I enter "theless" into the OutsideInstructorTitle field
	And I select class location "one"'s Description from the ClassLocation dropdown
	And I enter "this is course location in the db" into the CourseLocation field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for HeldOn with "4/1/2014"
	And I should see a link to the Show page for training module: "one"
	And I should see a display for Instructor with "Theodore Logan"
	And I should see a display for SecondInstructor with "Bill Preston"
	And I should see a display for OutsideInstructor with "none"
	And I should see a display for OutsideInstructorTitle with "theless"
	And I should see a display for ClassLocation with "FFS - Down the hall"
	And I should see a display for CourseLocation with "this is course location in the db"

Scenario: user can destroy a training record
	Given a training record "one" exists with training module: "one", held on: "today"
	And I am logged in as "user"
	When I visit the Show page for training record: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the TrainingRecord/Search page
	When I try to access the Show page for training record: "one" expecting an error
	Then I should see a 404 error message

Scenario: User can download an incident's pdf
	Given a training record "one" exists with training module: "one", held on: "today"
	And I am logged in as "user"
	Then I should be able to download training record "one"'s pdf

Scenario: user can search for open training events
    Given a training record "one" exists with training module: "one", scheduled date: "tomorrow", maximum class size: 1
    And a training record "two" exists with training module: "one", scheduled date: "tomorrow", maximum class size: 2
    And a training record "three" exists with training module: "one", scheduled date: "tomorrow", maximum class size: 3
    And employee: "one" is scheduled for training record: "one"
    And employee: "one" is scheduled for training record: "two"
    And employee: "one" is scheduled for training record: "three"	
	And I am logged in as "user"
    When I visit the TrainingRecord/Search page
    And I check the OnlyOpen field
	And I press Search
    Then I should be at the TrainingRecord page
    And I should not see a link to the Show page for training record: "one"
    And I should see a link to the Show page for training record: "two"
    And I should see a link to the Show page for training record: "three"

Scenario: admin can add a training session and view it on the calendar
	Given a training record "one" exists with training module: "one", held on: "tomorrow", maximum class size: "5"
	And I am logged in as "admin"
	When I visit the Show page for training record: "one"
	And I click the "Training Sessions" tab
	And I press "Add Training Session"
	And I enter today's date into the StartDateTime field
	And I enter today's date into the EndDateTime field
	And I press "Save Training Session" 
	And I wait for the page to reload
	Then I should see "0" in the table trainingSessions's "Duration" column
	When I follow "Search"
	And I press Search
	And I click the "Calendar" tab
	Then I should see "12t training module one" in the calendar element
	And I should not see "4t training module one" in the calendar element

Scenario: admin can add a training session and edit it
	Given a training record "one" exists with training module: "one", held on: "tomorrow", maximum class size: "5"
	And I am logged in as "admin"
	When I visit the Show page for training record: "one"
	And I click the "Training Sessions" tab
	And I press "Add Training Session"
	And I enter today's date into the StartDateTime field
	And I enter today's date into the EndDateTime field
	And I press "Save Training Session" 
	And I wait for the page to reload
	Then I should see "0" in the table trainingSessions's "Duration" column
	When I click the "Training Sessions" tab
	And I click the "Edit" link in the 1st row of trainingSessions
	And I enter "12/08/1980 10:50 PM" into the StartDateTime field
	And I enter "12/08/1980 11:07 PM" into the EndDateTime field
	And I press "Save"
	Then I should be at the Show page for training record: "one"
	When I click the "Training Sessions" tab
	Then I should see "0.283333333333333" in the table trainingSessions's "Duration" column

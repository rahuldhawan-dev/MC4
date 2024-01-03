Feature: TrainingModulePage
	Boys are from mars
	Girls are from Venus
	I've got a yum yum

Background: users and supporting data exists
	Given a user "user" exists with username: "user"
	And a town "lazytown" exists
	And a role "roleRead" exists with action: "Read", module: "OperationsTrainingModules", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "OperationsTrainingModules", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "OperationsTrainingModules", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "OperationsTrainingModules", user: "user"
	And an operating center "opc" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a user "mcuser" exists with username: "mcuser"
	And a role "roleRead2" exists with action: "Read", module: "OperationsTrainingModules", user: "mcuser"

Scenario: training record user cannot access training modules
	Given a role "roleReadRecords" exists with action: "Read", module: "OperationsTrainingRecords", operating center: "opc"
	And a user "records" exists with username: "records", roles: roleReadRecords
	And a training module "one" exists with title: "training module one"
	And I am logged in as "records"
	When I visit the Show page for training module: "one"
	Then I should see "You do not have the roles necessary to access this resource."

Scenario: user can view training modules
	Given a training module "one" exists with title: "training module one"
	And I am logged in as "user"
	When I visit the Show page for training module: "one"
	Then I should see a display for training module: "one"'s Title
	When I visit the TrainingModule/Search page
	And I press Search
	Then I should see training module "one"'s Title in the "Title" column

Scenario: user can add a training module
	Given I am logged in as "user"
	When I visit the TrainingModule/New page
	And I check the TCHCertified field
	And I press Save
	Then I should see the validation message "The Title field is required."
	And I should see the validation message "The NJDEP TCH COURSE APPROVAL NUMBER field is required."
	And I should see the validation message "The TCHCreditValue field is required."
	And I should see the validation message "The TotalHours field is required."
	And I should see the validation message "The SafetyRelated field is required."
	When I enter "blergh" into the Title field
	And I enter "5" into the TCHCreditValue field
	And I enter "16" into the TotalHours field
	And I select "Yes" from the SafetyRelated dropdown
	And I enter "99-010101-31" into the CourseApprovalNumber field
	And I press Save
	And I wait for the page to reload
	Then I should see "Edit"
	And I should see a display for Title with "blergh"
	And I should see a display for TCHCreditValue with "5"
	And I should see a display for TotalHours with "16"
	And I should see a display for CourseApprovalNumber with "99-010101-31"
	And I should see a display for SafetyRelated with "Yes"
	
Scenario: user can edit a training module
	Given a training module "one" exists with title: "training module one"
	And I am logged in as "user"
	When I visit the Edit page for training module: "one"
	And I enter "training module title" into the Title field
	And I press Save
	Then I should see a display for Title with "training module title"

Scenario: user can destroy a training module
	Given a training module "one" exists with title: "training module one"
	And I am logged in as "user"
	When I visit the Show page for training module: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the TrainingModule/Search page
	When I try to access the Show page for training module: "one" expecting an error
    Then I should see a 404 error message
	
Scenario: user can see all the employees that are on record for having trained for a training module
	Given a data type "employees attended" exists with table name: "tblTrainingRecords", name: "Employees Attended"
	And a training module "one" exists with title: "training module one"
	And a training record "one" exists with training module: "one", held on: "4/24/1984"
	And a training record "two" exists with training module: "one", held on: "5/25/1985"
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "opc"
	And an employee "two" exists with first name: "Joe", last name: "Shmoe", employee id: "22222222", operating center: "opc"
	And employee: "one" attended training record: "one"
	And employee: "two" attended training record: "two"
	And a position "one" exists with position description: "This is a job?"
	And a position history "one" exists with position: "one", employee: "one"
	And I am logged in as "user"
	When I visit the Show page for training module: "one"
	And I click the "Attendees" tab
	Then I should see the following values in the attendees-table table
	| Employee ID | Full Name      | Operating Center | Current Position              | Held On   |
	| 11111111    | Theodore Logan | NJ4 - Lakewood   | position: "one"'s Description | 4/24/1984 |
	| 22222222    | Joe Shmoe      | NJ4 - Lakewood   |                               | 5/25/1985 |

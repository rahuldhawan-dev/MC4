Feature: OperatingCenterTrainingSummary

Background: users and supporting data exists
	Given a state "one" exists with abbreviation: "NJ"
	And an operating center "nj7" exists with opcode: "NJ7", state: "one", name: "Lakewood"
	And an operating center "nj4" exists with opcode: "NJ4", state: "one", name: "Shrewsbury"
	And a role "roleReadNj7" exists with action: "Read", module: "OperationsTrainingRecords", operating center: "nj7"
	And an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user", roles: roleReadNj7
	And an employee status "active" exists with description: "Active"
	And a position group common name "one" exists with description: "Surveyor"
	And a position group "one" exists with description: "Foo", common name: "one"
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "nj7", status: "active", position group: "one"
	
Scenario: user can run a summary search and get results
	Given a training module category "one" exists with description: "Safety"
	And a training requirement "one" exists with is o s h a requirement: true, is active: true, training frequency unit: "Y", training frequency: "2", description: "Description"
	And position group common name: "one" exists in training requirement: "one"
	And a training module "one" exists with title: "training module one", course approval number: "123", training module category: "one", training requirement: "one"
	And a data type "employees attended" exists with table name: "tblTrainingRecords", name: "Employees Attended"
	And a data type "employees scheduled" exists with table name: "tblTrainingRecords", name: "Employees Scheduled"
	And a class location "one" exists with Description: "Down the hall", operating center: "nj7"
	And a training record "one" exists with training module: "one", held on: "4/1/2015", class location: "one"
	And employee: "one" is scheduled for training record: "one"
	And employee: "one" attended training record: "one"
	And I am logged in as "admin"
	When I visit the /Reports/OperatingCenterTrainingSummary/Search page
	Then I should see "Training Summary"
	When I select state "one" from the State dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I press Search
	Then I should see the following values in the trainingRequirementsTable table
	| Operating Center | Training Requirement  | Recurring             | Employees in Position Groups | Employees Scheduled | Classes Scheduled | Operating Center Classes Scheduled |
	| NJ4 - Shrewsbury | Description | Yes                   | 0                            | 0                   | 0                 | 0                                  |

Scenario: user can run an overview search and get results
	Given a training module category "one" exists with description: "Safety"
	And a training requirement "one" exists with is o s h a requirement: true, is active: true, training frequency unit: "Y", training frequency: "250"
	And position group common name: "one" exists in training requirement: "one"
	And a training module "one" exists with title: "training module one", course approval number: "123", training module category: "one", training requirement: "one"
	And a data type "employees attended" exists with table name: "tblTrainingRecords", name: "Employees Attended"
	And a data type "employees scheduled" exists with table name: "tblTrainingRecords", name: "Employees Scheduled"
	And a class location "one" exists with Description: "Down the hall", operating center: "nj7"
	And a training record "one" exists with training module: "one", held on: "4/1/2017", class location: "one"
	And employee: "one" is scheduled for training record: "one"
	And employee: "one" attended training record: "one"
	And I am logged in as "admin"
	When I visit the /Reports/OperatingCenterTrainingOverview/Search page
	Then I should see "Training Overview"
	When I select state "one" from the State dropdown
	And I press Search
	Then I should see the following values in the trainingRequirementsTable table
	| Operating Center | Employees | % Complete | Training Records Required | Training Records Due | Training Records Complete |
    | !@#$ -           | 0         |            | 0                         | 0                    | 0                         |
    | NJ4 - Shrewsbury | 0         |            | 0                         | 0                    | 0                         |
    | NJ7 - Lakewood   | 1         | 100.00 %   | 1                         | 0                    | 1                         |
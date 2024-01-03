Feature: TailgateTalksPage

Background: users and supporting data exists
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood"
	And a role "roleReadNj7" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleReadNj4" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleUnionRead" exists with action: "Read", module: "HumanResourcesUnion", operating center: "nj7"
	And an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And a user "invalid" exists with username: "invalid"
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
	And a user "user-admin-nj4" exists with username: "user-admin-nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
	And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7;roleUnionRead
    And a data type "tailgate talks" exists with table name: "tblTailgateTalks", name: "Tailgate Talks"
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "nj7", status: "active"
	And an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "11111117", operating center: "nj7", status: "active"
	And a tailgate topic category "one" exists with description: "category"
    And a tailgate talk topic "one" exists with category: "one"
    And a tailgate talk topic "two" exists with category: "one"
    And a tailgate talk topic "inactive" exists with category: "one", is active: false
    And a tailgate talk "one" exists with topic: "one", presented by: "two"

Scenario: user can view tailgate talks
    Given I am logged in as "user-admin-nj7"
	When I visit the Show page for tailgate talk: "one"
    Then I should see a display for tailgate talk: "one"'s HeldOn
    And I should see a link to the Show page for tailgate talk topic: "one"
    And I should see a display for tailgate talk: "one"'s PresentedBy
    And I should see a display for tailgate talk: "one"'s TrainingTimeHours		
	When I visit the HealthAndSafety/TailgateTalk/Search page
	And I press Search
	Then I should see tailgate talk "one"'s OperatingCenter in the "Operating Center" column
    And I should see tailgate talk "one"'s HeldOn as a date in the "Held On" column
    And I should see tailgate talk "one"'s Category in the "Category" column
    And I should see tailgate talk "one"'s Topic in the "Topic" column
    #And I should see tailgate talk "one"'s PresentedBy in the "Presenter" column
	And I should see employee "two"'s FullName in the "Presenter" column
    And I should see tailgate talk "one"'s OrmReferenceNumber in the "ORM Reference Number" column

Scenario: user can add a tailgate talk
	Given I am logged in as "user-admin-nj7"
	When I visit the HealthAndSafety/TailgateTalk/New page
	And I press Save
	Then I should see the validation message "The Topic field is required."
	And I should see the validation message "The HeldOn field is required."
	And I should see the validation message "The Presenter field is required."
	And I should see the validation message "The TrainingTimeHours field is required."
	When I enter employee "two"'s Description and select employee "two"'s Description from the PresentedBy combobox
	And I enter tailgate talk topic "one"'s Description and select tailgate talk topic "one"'s Description from the Topic combobox
	And I enter "0.1" into the TrainingTimeHours field
	And I enter "4/1/2014" into the HeldOn field
	And I press Save
	And I wait for the page to reload
    Then the currently shown tailgate talk will now be referred to as "new"
    And I should be at the Show page for tailgate talk: "new"
    Then I should see a display for HeldOn with "4/1/2014"
	And I should see a display for TrainingTimeHours with "0.1"
	And I should see a display for "PresentedBy" with employee: "two"
	And I should see a link to the Show page for tailgate talk topic: "one"
	When I visit the HealthAndSafety/TailgateTalk/New page
	When I enter employee "two"'s Description and select employee "two"'s Description from the PresentedBy combobox
	And I enter tailgate talk topic "one"'s Description and select tailgate talk topic "one"'s Description from the Topic combobox
	And I enter ".25" into the TrainingTimeHours field
	And I enter "4/1/2014" into the HeldOn field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for TrainingTimeHours with "0.25"

Scenario: site admin can add a tailgate talk
	Given I am logged in as "admin"
	When I visit the HealthAndSafety/TailgateTalk/New page
	When I enter employee "two"'s Description and select employee "two"'s Description from the PresentedBy combobox
	And I enter tailgate talk topic "one"'s Description and select tailgate talk topic "one"'s Description from the Topic combobox
	And I enter "1" into the TrainingTimeHours field
	And I enter "4/1/2014" into the HeldOn field
	And I press Save
	And I wait for the page to reload
    Then the currently shown tailgate talk will now be referred to as "new"
    And I should be at the Show page for tailgate talk: "new"
    Then I should see a display for HeldOn with "4/1/2014"
	And I should see a display for TrainingTimeHours with "1"
	And I should see a display for "PresentedBy" with employee: "two"
	And I should see a link to the Show page for tailgate talk topic: "one"
	#And I should see a display for PresentedBy with employee "two"'s FullName

Scenario: user can edit a tailgate talk
    Given I am logged in as "user-admin-nj7"
	When I visit the Edit page for tailgate talk: "one"
	When I enter employee "two"'s Description and select employee "two"'s Description from the PresentedBy combobox
	And I enter tailgate talk topic "one"'s Description and select tailgate talk topic "one"'s Description from the Topic combobox
	And I press Save
    Then I should be at the Show page for tailgate talk: "one"
	And I should see a display for "PresentedBy" with employee: "two"
	And I should see a link to the Show page for tailgate talk topic: "one"

Scenario: user can destroy a tailgate talk
    Given I am logged in as "user-admin-nj7"
	When I visit the Show page for tailgate talk: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the HealthAndSafety/TailgateTalk/Search page
	When I try to access the Show page for tailgate talk: "one" expecting an error
	Then I should see a 404 error message

Scenario: user can search for a tailgate talk
	Given I am logged in as "user-admin-nj7"
	When I visit the HealthAndSafety/TailgateTalk/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select employee "two"'s Description from the PresentedBy dropdown
	And I select tailgate topic category "one"'s Description from the TailgateTopicCategory dropdown
	And I wait for ajax to finish loading
	And I select tailgate talk topic "one"'s Description from the Topic dropdown
	And I enter tailgate talk topic "one"'s OrmReferenceNumber into the OrmReferenceNumber field
	And I press Search
	Then I should see tailgate talk topic "one"'s Description in the "Topic" column
	Then I should see tailgate talk topic "one"'s OrmReferenceNumber in the "ORM Reference Number" column
	And I should see employee "two"'s FullName in the "Presenter" column
	And I should see operating center "nj7"'s Description in the "Operating Center" column

Scenario: user can send notifications
	Given I am logged in as "user-admin-both"
	When I visit the Show page for tailgate talk: "one"
	And I click the "Employees" tab
	And I press "Link Employee"
	And I check employee "one"'s Description in the EmployeeIds checkbox list
    And I press "Link Employees"
    Then I should be at the Show page for tailgate talk: "one" on the Employees tab
	When I click the "Employees" tab 
    Then I should see employee "one"'s EmployeeId in the table employeesTable's "Employee ID" column
	When I click ok in the dialog after pressing "Send Notification"
	Then I should be at the Show page for tailgate talk: "one" on the Employees tab
	And I should see "Notification Sent"

Scenario: user with edit rights can add employees to and delete employees from tailgate talks
    Given an employee "bill" exists with operating center: "nj7", status: active
	And I am logged in as "user-admin-nj7"
	When I visit the Show page for tailgate talk: "one"
    And I click the "Employees" tab
    And I press "Link Employee"
	And I check employee "bill"'s Description in the EmployeeIds checkbox list
    And I press "Link Employees"
    Then I should be at the Show page for tailgate talk: "one" on the Employees tab
	When I click the "Employees" tab
    Then I should see employee "bill"'s EmployeeId in the table employeesTable's "Employee ID" column
    When I click the "Remove" button in the 1st row of employeesTable and then click ok in the confirmation dialog
    Then I should be at the Show page for tailgate talk: "one" on the Employees tab
    And I should not see employee "bill"'s EmployeeId in the table employeesTable's "Employee ID" column

Scenario: User can filter employees by operating center when linking employees
    Given an employee "bill" exists with operating center: "nj7", employee id: "123456789", status: active
	And an employee "jillnifer" exists with operating center: "nj4", employee id: "987654321", status: active
	And I am logged in as "user-admin-nj7"
	When I visit the Show page for tailgate talk: "one"
    And I click the "Employees" tab
	And I press "Link Employee"
    And I select operating center "nj7" from the OperatingCenter dropdown
	Then I should see "123456789"
	And I should not see "987654321"
	When I select operating center "nj4" from the OperatingCenter dropdown
	Then I should not see "123456789"
	And I should see "987654321"
	
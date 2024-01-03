Feature: EmployeeAssignmentPage
	In order to start and end production work orders
	As an employee who works on production work orders
	I want to be able to view, start and end my assigned production work orders

Background: tings
    Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And an operating center "nj4" exists with opcode: "NJ4"
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg"
    And a street "one" exists with town: "nj7burg", full st name: "Easy St"
	And an equipment type "aerator" exists with description: "ET01 - Aerator"
	And a plant maintenance activity type "two" exists with description: "two" 
	And a plant maintenance activity type "one" exists with description: "one" 
    And order types exist
    And a production skill set "one" exists
	And a production work description "one" exists 
	And a production work description "two" exists with production skill set: "one"
    And a production work order "one" exists with productionWorkDescription: "one", s a p work order: "11111"
    And a production work order "two" exists with productionWorkDescription: "two", s a p work order: "22222"
    And a production work order "three" exists with productionWorkDescription: "one", s a p work order: "33333"
    And a production work order "four" exists with productionWorkDescription: "one", s a p work order: "11111", operating center: "nj4"
    And a production work order "completed" exists with productionWorkDescription: "one", dateCompleted: "2010-07-15"
    And a production work order "cancelled" exists with productionWorkDescription: "two", dateCancelled: "2008-04-02"
	And an employee status "active" exists with description: "Active"
    And an employee "one" exists with operating center: "nj7", status: "active", employee id: "12345678"
    And an employee "two" exists with operating center: "nj7", status: "active"
    And an employee "three" exists with operating center: "nj4", status: "active"
    And an employee "nj4" exists with operating center: "nj4", status: "active"
    And employee "two" has production skill set "one"
 	And a role "roleRead" exists with action: "Read", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "roleAdd" exists with action: "Add", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "roleUserAdmin" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "nj7"
    And a user "user" exists with username: "user", employee: "one", roles: roleRead;roleAdd;roleEdit;roleDelete
    And a user "usersearch" exists with username: "usersearch", employee: "three", roles: roleRead;roleAdd;roleEdit;roleDelete;roleUserAdmin
    And a user "useradmin" exists with username: "useradmin", roles: roleRead;roleAdd;roleEdit;roleDelete;roleUserAdmin
	# Needed because the end assignment action creates a Note
	And a data type "productionworkorder" exists with table name: "ProductionWorkOrders"
    And a production pre job safety brief "one" exists with production work order: "one"
    And a production pre job safety brief worker exists with production pre job safety brief: "one", employee: "one"
    And a production pre job safety brief "two" exists with production work order: "two"
    And a production pre job safety brief worker exists with production pre job safety brief: "two", employee: "one"
    
Scenario: user can view and start/end their own work
	Given an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today"
    And an employee assignment "two" exists with production work order: "two", assigned to: "two", assigned for: "today"
    And I am logged in as "user"
    And I am at the Production/EmployeeAssignment/Search page
    When I press Search
    Then I should be at the Production/EmployeeAssignment page
    And I should see a link to the Show page for production work order "one"
    And I should not see a link to the Show page for production work order "two"
	And I should see the following values in the employeeAssignmentsTable table
         | Production Work Order				| Start                | End |
         | production work order: "one"'s Id	| Start                |     |
    When I press "Start"
	Then I should be at the Show page for production work order: "one"
	# Since this redirects to the work order page, we need to go back to where we were.
	When I go to the Production/EmployeeASsignment/Index page
	Then I should see the following values in the employeeAssignmentsTable table
         | Production Work Order				| Start | End |
         | production work order: "one"'s Id	| *employee assignment: "one"'s DateStarted* | End |
	When I press "End" 
	And I enter "this is a note" into the Notes field
	And I enter "1.23" into the HoursWorked field
	And I press "End Assignment"
	Then I should be at the Show page for production work order: "one"
	When I click the "Notes" tab
	# Can't do a proper table cell text lookup of the following due to the invisible elements used for editing notes
	Then I should see "this is a note"
	# And again with the redirect, need to go back
	When I go to the Production/EmployeeASsignment/Index page
	Then I should see the following values in the employeeAssignmentsTable table
         | Production Work Order				| Start                                      | End                                      |
         | production work order: "one"'s Id	| *employee assignment: "one"'s DateStarted* | *employee assignment: "one"'s DateEnded* |

Scenario: user can search for Employee Assignments by Assigned For date range
	Given an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "9/15/2023"
	And an employee assignment "two" exists with production work order: "two", assigned to: "one", assigned for: "9/25/2023"
	And I am logged in as "usersearch"
    And I am at the Production/EmployeeAssignment/Search page
	When I enter "9/01/2023" into the AssignedFor_Start field
	And I enter "9/20/2023" into the AssignedFor_End field
	When I press Search
	Then I should be at the Production/EmployeeAssignment page
	And I should see a link to the Show page for production work order "one"
	Then I should see the following values in the employeeAssignmentsTable table
         | Production Work Order				| Start                                      | End                                      | Assigned For |
         | production work order: "one"'s Id	| *employee assignment: "one"'s DateStarted* | *employee assignment: "one"'s DateEnded* | 9/15/2023    |

Scenario: user can search work for OC when they have user admin to another OC
    Given an employee assignment "two" exists with production work order: "two", assigned to: "three", assigned for: "today"
    And  an employee assignment "three" exists with production work order: "four", assigned to: "three", assigned for: "today"
    And I am logged in as "usersearch"
    And I am at the Production/EmployeeAssignment/Search page
    When I press Search
    Then I should be at the Production/EmployeeAssignment page
    And I should see a link to the Show page for production work order "two"
    And I should see a link to the Show page for production work order "four"
	When I go back
	And I select "NJ" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should see a link to the Show page for production work order "two"
 
Scenario: user administrator can start/end work for other employees within operating centers they have the role for
	Given an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today"
    And I am logged in as "useradmin"
    And I am at the Production/EmployeeAssignment/Search page
    When I press Search
    Then I should be at the Production/EmployeeAssignment page
	And I should see the following values in the employeeAssignmentsTable table
         | Production Work Order			| Start                | End |
         | production work order: "one"'s Id	| Start                |     |
    When I press "Start"
	Then I should be at the Show page for production work order: "one"
	# Since this redirects to the work order page, we need to go back to where we were.
	When I go to the Production/EmployeeAssignment/Index page
	Then I should see the following values in the employeeAssignmentsTable table
         | Production Work Order				| Start | End |
         | production work order: "one"'s Id	| *employee assignment: "one"'s DateStarted* | End |
	When I press "End" 
	And I enter "this is a note" into the Notes field
	And I enter "1.23" into the HoursWorked field
	And I press "End Assignment"
	Then I should be at the Show page for production work order: "one"
	When I click the "Notes" tab
	# Can't do a proper table cell text lookup of the following due to the invisible elements used for editing notes
	Then I should see "this is a note"
	# And again with the redirect, need to go back
	When I go to the Production/EmployeeAssignment/Index page
	Then I should see the following values in the employeeAssignmentsTable table
         | Production Work Order				| Start                                      | End                                      |
         | production work order: "one"'s Id	| *employee assignment: "one"'s DateStarted* | *employee assignment: "one"'s DateEnded* |

Scenario: user administrator cannot start/end work for other employees within operating centers they do not have the role for
    Given an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today"
    And an employee assignment "two" exists with production work order: "two", assigned to: "two", assigned for: "today"
    And an employee assignment "three" exists with production work order: "three", assigned to: "nj4", assigned for: "today"
    And I am logged in as "useradmin"
    When I visit the Production/EmployeeAssignment/Search page
	Then I should not see operating center "nj4" in the OperatingCenter dropdown
	When I press Search
    Then I should be at the Production/EmployeeAssignment page
    And I should see a link to the Show page for production work order "one"
    And I should see a link to the Show page for production work order "two"
    And I should not see a link to the Show page for production work order "three"
    
Scenario: user can view and edit Hours Worked
	Given an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today"
	And I am logged in as "user"
	And I am at the Show page for production work order: "one"
	When I click the "Employee Assignments" tab	
	And I wait for the page to reload
	And I click the "View" link in the 1st row of employeeAssignmentsTable
	And I wait for the page to reload
	Then I should see a display for HoursWorked with "0.00"
	When I follow "Edit"
	And I wait for the page to reload
	Then I should see "0.00" in the HoursWorked field
	When I enter "" into the HoursWorked field
	And I press Save 
	Then I should see a validation message for HoursWorked with "The Hours Worked field is required."
	When I enter "1.234" into the HoursWorked field
	Then I should see a validation message for HoursWorked with "Must be a number with maximum of 2 decimal places" 
	When I enter "1.23" into the HoursWorked field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for HoursWorked with "1.23"

Scenario: user can end last assignment from employee assignments and go to show page
	Given an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today"
    And I am logged in as "user"
    And I am at the Production/EmployeeAssignment/Search page
    When I press Search
    Then I should be at the Production/EmployeeAssignment page
    And I should see a link to the Show page for production work order "one"
	And I should see the following values in the employeeAssignmentsTable table
         | Production Work Order				| Start                | End |
         | production work order: "one"'s Id	| Start                |     |
    When I press "Start"
	Then I should be at the Show page for production work order: "one"
	When I go to the Production/EmployeeASsignment/Index page
	Then I should see the following values in the employeeAssignmentsTable table
         | Production Work Order				| Start | End |
         | production work order: "one"'s Id	| *employee assignment: "one"'s DateStarted* | End |
	When I press "End" 
	And I enter "this is a note" into the Notes field
	And I enter "1.23" into the HoursWorked field
	And I press "End Assignment"
	Then I should be at the Show page for production work order: "one" 
		
Scenario: user can end last assignment from production work orders and go to finalization tab
	Given an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today"
    And I am logged in as "user"
    And I am at the Production/ProductionWorkOrder/Search page
    When I press Search
    Then I should be at the Production/ProductionWorkOrder page
    And I should see a link to the Show page for production work order "one"
	When I follow the Show link for production work order "one"
	And I click the "Employee Assignments" tab
    When I press "Start"
	Then I should be at the Show page for production work order: "one"
	When I go to the Production/ProductionWorkOrder/Index page
	And I follow the Show link for production work order "one"
	And I click the "Employee Assignments" tab
	And I press "End" 
	And I enter "this is a note" into the Notes field
	And I enter "1.23" into the HoursWorked field
	And I press "End Assignment"
	Then I should be at the Show page for production work order: "one" and fragment of "#FinalizationTab"

Scenario: user can end assignment from production work order when more exist and go to show page
	Given an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today"
    And an employee assignment "two" exists with production work order: "two", assigned to: "two", assigned for: "today"
    And I am logged in as "user"
    And I am at the Production/ProductionWorkOrder/Search page
    When I press Search
    Then I should be at the Production/ProductionWorkOrder page
    And I should see a link to the Show page for production work order "one"
	When I follow the Show link for production work order "one"
	And I click the "Employee Assignments" tab
    When I press "Start"
	Then I should be at the Show page for production work order: "one"
	When I go to the Production/ProductionWorkOrder/Index page
	And I follow the Show link for production work order "one"
	And I click the "Employee Assignments" tab
	And I press "End" 
	And I enter "this is a note" into the Notes field
	And I enter "1.23" into the HoursWorked field
	And I press "End Assignment"
	Then I should be at the Show page for production work order: "one"

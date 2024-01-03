Feature: SchedulingPage
    In order to assign production work orders to employees
    As a person who assigns production work orders to employees
    I want to be able to assign production work orders to employees

Background: things exist
    Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", sap enabled: "true", sap work orders enabled: "true"
	And an employee status "active" exists with description: "Active"
	And an employee "both-edit" exists with operating center: "nj7", status: "active", employee id: "12345678"
    And an employee "both-read" exists with operating center: "nj7", status: "active", employee id: "22345678"
	And an employee "nj7-edit" exists with operating center: "nj4", status: "active", employee id: "32345678"
    And an employee "nj4-edit" exists with operating center: "nj4", status: "active", employee id: "42345678""
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town "nj4ton" exists
    And operating center: "nj4" exists in town: "nj4ton"
    And a town section "one" exists with town: "nj7burg"
    And a street "one" exists with town: "nj7burg", full st name: "Easy St"
    And a town section "two" exists with town: "nj4ton"
    And a street "two" exists with town: "nj4ton", full st name: "Skid Row"
    And equipment types exist
	And a plant maintenance activity type "two" exists with description: "two" 
	And a plant maintenance activity type "one" exists with description: "one" 
    And order types exist
	And a role "productionworkorder-read-nj7" exists with action: "Read", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "productionworkorder-add-nj7" exists with action: "Add", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "productionworkorder-edit-nj7" exists with action: "Edit", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "productionworkorder-all-nj7" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "productionworkorder-read-nj4" exists with action: "Read", module: "ProductionWorkManagement", operating center: "nj4"
	And a role "productionworkorder-add-nj4" exists with action: "Add", module: "ProductionWorkManagement", operating center: "nj4"
	And a role "productionworkorder-edit-nj4" exists with action: "Edit", module: "ProductionWorkManagement", operating center: "nj4"
	And a role "productionworkorder-all-nj4" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "nj4"
	And a role "facilityrole" exists with action: "Read", module: "ProductionFacilities"
	And a user "user" exists with username: "user", roles: productionworkorder-read-nj7;productionworkorder-add-nj7;productionworkorder-edit-nj7;productionworkorder-read-nj4;productionworkorder-add-nj4;productionworkorder-edit-nj4;facilityrole
	And a user "adminuser" exists with username: "adminuser", roles: productionworkorder-all-nj7;productionworkorder-all-nj4;facilityrole
    And a user "both-edit" exists with username: "both-edit", employee: "both-edit", roles: productionworkorder-edit-nj7;productionworkorder-edit-nj4
    And a user "both-read" exists with username: "both-read", employee: "both-read", roles: productionworkorder-read-nj7;productionworkorder-read-nj4
    And a user "nj7-edit" exists with username: "nj7-edit", employee: "nj7-edit", roles: productionworkorder-edit-nj7
    And a user "nj4-edit" exists with username: "nj4-edit", employee: "nj4-edit", roles: productionworkorder-edit-nj4
    And a production skill set "one" exists
	And a production work description "one" exists 
	And a production work description "two" exists with production skill set: "one"
    And a production work order "one" exists with production work description: "one", operating center: "nj7"
    And a production work order "two" exists with production work description: "two", operating center: "nj4"
    And a production work order "three" exists with production work description: "two", operating center: "nj7"
    And a production work order "completed" exists with productionWorkDescription: "one", dateCompleted: "2010-07-15"
    And a production work order "cancelled" exists with productionWorkDescription: "two", dateCancelled: "2008-04-02"
    And employee "nj7-edit" has production skill set "one"
    And employee "nj4-edit" has production skill set "one"
    And a maintenance plan task type "one" exists with description: "task type one", abbreviation: "TTT", is active: true
    And a maintenance plan task type "two" exists with description: "task type two", abbreviation: "TT2", is active: true
    And a task group category "one" exists with description: "This is the description", type: "chemical", abbreviation: "CHEM", is active: "true"
    And a task group "group2" exists with task group id: "id1", task group name: "SirRobin", task details: "details", task details summary: "details summary", task group category: "one", task group categories: "one", equipment types: "rtu", maintenance plan task type: "two"
    And I am logged in as "adminuser"
    
Scenario: user can assign work to employees
    Given I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should be at the Production/Scheduling page
    When I press Assign
    Then I should be at the Production/Scheduling page
    And I should see a validation message for AssignedTo with "The AssignedTo field is required."
    And I should see a validation message for AssignedFor with "The AssignedFor field is required."
    When I select employee "both-edit"'s Description from the AssignedTo dropdown
    And I enter today's date into the AssignedFor field
    And I press Assign
    Then I should be at the Production/Scheduling page
    And I should see the error message "At least one order must be selected for assignment."
    When I click the checkbox named ProductionWorkOrderIds with production work order "one"'s Id
    And I select employee "both-edit"'s Description from the AssignedTo dropdown
    And I enter today's date into the AssignedFor field
    And I press Assign
    Then I should be at the Production/Scheduling page
    And I should see today's date in the table schedulingTable's "Current Assignment" column
    When I click the checkbox named ProductionWorkOrderIds with production work order "one"'s Id
    And I select employee "both-edit"'s Description from the AssignedTo dropdown
    And I enter today's date into the AssignedFor field
    And I press Assign
    Then I should see the error message "Cannot add duplicate Employee Assignment(s) to Production Work Order with the ID: '1'"

Scenario: user can assign work to employees in other operating centers
    Given I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should be at the Production/Scheduling page
    When I press Assign
    Then I should be at the Production/Scheduling page
    And I should see a validation message for AssignedTo with "The AssignedTo field is required."
    And I should see a validation message for AssignedFor with "The AssignedFor field is required."
    When I select employee "nj7-edit"'s Description from the AssignedTo dropdown
    And I enter today's date into the AssignedFor field
    And I press Assign
    Then I should be at the Production/Scheduling page
    And I should see the error message "At least one order must be selected for assignment."
    When I click the checkbox named ProductionWorkOrderIds with production work order "one"'s Id
    And I select employee "nj7-edit"'s Description from the AssignedTo dropdown
    And I enter today's date into the AssignedFor field
    And I press Assign
    Then I should be at the Production/Scheduling page
    And I should see today's date in the table schedulingTable's "Current Assignment" column
    
Scenario: user must choose an operating center when searching
    Given I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I press Search
    Then I should be at the Production/Scheduling/Search page
    And I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
 
Scenario: user must choose a task type when searching by task group name
    Given I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I press Search
    Then I should be at the Production/Scheduling/Search page
    And I should see "Please select a task type" in the TaskGroupName dropdown
    When I select maintenance plan task type "one" from the TaskType dropdown
    Then I should see "No Results" in the TaskGroupName dropdown
    When I select maintenance plan task type "two" from the TaskType dropdown
    Then I should see "SirRobin" in the TaskGroupName dropdown

Scenario: user can filter orders and employees by skill set
    Given I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select production skill set "one" from the ProductionSkillSet dropdown
    And I press Search
    Then I should be at the Production/Scheduling page
	And I should not see employee "both-edit"'s Description in the AssignedTo dropdown
	And I should see employee "nj7-edit"'s Description in the AssignedTo dropdown

Scenario: user can search for a production work order with multiple equipment and does not see duplicate rows
	Given a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
    And a facility "one" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility", planning plant: "one", has confined space requirement: "True"
    And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", equipment type: "rtu", description: "RTU1", s a p equipment id: 1
	And an equipment "two" exists with identifier: "NJSB-2-EQID-1", facility: "one", equipment type: "rtu", description: "RTU2", s a p equipment id: 2
    And a production work order "multi" exists with productionWorkDescription: "one", operating center: "nj7", planning plant: "one"
    And equipment: "one" exists in production work order: "multi"
	And equipment: "two" exists in production work order: "multi"
	And I am at the Production/Scheduling/Search page
	When I enter production work order "multi"'s Id into the EntityId field
    And I select "NJ" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should see "Records found: 1"
    
Scenario: user can search for a production work order scheduling returning records in date received order
	Given a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
    And a facility "one" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility", planning plant: "one", has confined space requirement: "True"
    And a production work order "pwo-search" exists with productionWorkDescription: "one", operating center: "nj7", planning plant: "one"
    And a production work order "pwo-later" exists with productionWorkDescription: "two", operating center: "nj7", planning plant: "one", date received: "6/12/2017"
	And I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should see "Records found: 4"
	And I should see the following values in the schedulingTable table
    | Id | Date Received | Operating Center | Work Description | Notes              | Local Task Description             | Order Type                  | Priority | Current Assignment | Estimated Completion Time (Hrs) | WBS Element       |
    | 1  | 1/1/2017      | NJ7 - Shrewsbury | *Process*        | This is OrderNotes | This is the Local Task Description | 0010 - Operational Activity | High     | n/a                | 0                               | Sample WBSElement |
    | 3  | 1/1/2017      | NJ7 - Shrewsbury | *Process*        | This is OrderNotes | This is the Local Task Description | 0010 - Operational Activity | High     | n/a                | 0                               | Sample WBSElement |
    | 6  | 1/1/2017      | NJ7 - Shrewsbury | *Process*        | This is OrderNotes | This is the Local Task Description | 0010 - Operational Activity | High     | n/a                | 0                               | Sample WBSElement |
    | 7  | 6/12/2017     | NJ7 - Shrewsbury | *Process*        | This is OrderNotes | This is the Local Task Description | 0010 - Operational Activity | High     | n/a                | 0                               | Sample WBSElement |

Scenario: user can search for scheduling and click the production work order id
    Given I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should be at the Production/Scheduling page
    When I click the "Add Assignments" tab
   	Then I should see a link to the show page for production work order "one"
	When I follow one of the Show links for production work order "one"
	And I wait for the page to reload
	Then I should be at the Show page for production work order "one"   

Scenario: user can remove assigned work from employees
    Given an employee assignment "one" exists with assignedTo: "nj7-edit", production work order: "one"
    And I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should be at the Production/Scheduling page
    When I click the "Remove Assignments" tab
    Then I should see a checkbox named EmployeeAssignmentIds with employee assignment "one"'s Id
    When I press "Remove Selected Assignments"
    Then I should be at the Production/Scheduling page
    And I should see the error message "At least one assignment must be selected for removal."
    When I click the "Remove Assignments" tab
    And I click the checkbox named EmployeeAssignmentIds with employee assignment "one"'s Id
    And I press "Remove Selected Assignments"
    Then I should be at the Production/Scheduling page
    When I click the "Remove Assignments" tab
    Then I should not see a checkbox named EmployeeAssignmentIds with employee assignment "one"'s Id

Scenario: user can search for a scheduling on plan number without requiring operating center
    Given I am at the Production/Scheduling/Search page
    When I select "NJ" from the State dropdown
    And I press Search
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    When I enter 1 into the PlanNumber field
    And I press Search
    Then I should not see a validation message for OperatingCenter with "The OperatingCenter field is required."
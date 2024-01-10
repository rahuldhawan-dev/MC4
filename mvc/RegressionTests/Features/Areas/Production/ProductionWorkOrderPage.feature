Feature: ProductionWorkOrderPage

Background: 
	Given production work order priorities exist
	And a state "one" exists with abbreviation: "NJ"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true", state: "one"
	And an operating center "nj4" exists with state: "one", opcode: "NJ4", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
	And an operating center "nj3" exists with state: "one", opcode: "NJ3", name: "Fire Road", sap enabled: "true", sap work orders enabled: "true"
	And an employee status "active" exists with description: "Active"
	And an employee "both-edit" exists with operating center: "nj7", status: "active", employee id: "12345678", first name: "jay", last name: "bob"
    And an employee "both-read" exists with operating center: "nj7", status: "active", employee id: "22345678", first name: "bob", last name: "jay"
	And an employee "nj7-edit" exists with operating center: "nj4", status: "active", employee id: "32345678", first name: "johnny", last name: "hotdog"
    And an employee "nj4-edit" exists with operating center: "nj4", status: "active", employee id: "42345678", first name: "hotdog", last name: "johnny"
	And a role "productionworkorder-read-nj7" exists with action: "Read", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "productionworkorder-add-nj7" exists with action: "Add", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "productionworkorder-edit-nj7" exists with action: "Edit", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "productionworkorder-all-nj7" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "productionworkorder-read-nj4" exists with action: "Read", module: "ProductionWorkManagement", operating center: "nj4"
	And a role "productionworkorder-add-nj4" exists with action: "Add", module: "ProductionWorkManagement", operating center: "nj4"
	And a role "productionworkorder-edit-nj4" exists with action: "Edit", module: "ProductionWorkManagement", operating center: "nj4"
	And a role "roleReadEquipment" exists with action: "Read", module: "ProductionEquipment"
	And a role "roleEditEquipment" exists with action: "Edit", module: "ProductionEquipment"
	And a role "productionworkorder-all-nj4" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "nj4"
	And a role "facilityrole" exists with action: "Read", module: "ProductionFacilities"
	And a role "equipmentrole" exists with action: "Read", module: "ProductionEquipment"
	And a role "prodplannedread" exists with action: "Read", module: "ProductionPlannedWork"
	And a user "user" exists with username: "user", roles: productionworkorder-read-nj7;productionworkorder-add-nj7;productionworkorder-edit-nj7;productionworkorder-read-nj4;productionworkorder-add-nj4;productionworkorder-edit-nj4;facilityrole;equipmentrole;prodplannedread;roleReadEquipment;roleEditEquipment
	And a role "facilitrolenj4user" exists with action: "Read", module: "ProductionFacilities", operating center: "nj4", user: "user"
	And a role "equipmentrolenj4user" exists with action: "Read", module: "ProductionEquipment", operating center: "nj4", user: "user"
	And a user "adminuser" exists with username: "adminuser", roles: productionworkorder-all-nj7;productionworkorder-all-nj4;facilityrole;equipmentrole
    And a user "both-edit" exists with username: "both-edit", employee: "both-edit", roles: productionworkorder-edit-nj7;productionworkorder-edit-nj4
    And a user "both-read" exists with username: "both-read", employee: "both-read", roles: productionworkorder-read-nj7;productionworkorder-read-nj4
    And a user "nj7-edit" exists with username: "nj7-edit", employee: "nj7-edit", roles: productionworkorder-edit-nj7
    And a user "nj4-edit" exists with username: "nj4-edit", employee: "nj4-edit", roles: productionworkorder-edit-nj4
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town "nj4ton" exists
	And operating center: "nj4" exists in town: "nj4ton"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
	And a planning plant "two" exists with operating center: "nj4", code: "D214", description: "Hgra"
	And a facility "one" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility", planning plant: "one", has confined space requirement: "True", functional location: "East"
	And a facility "three" exists with operating center: "nj7", facility id: "NJSB-2", facility name: "A Facility jr", planning plant: "one", has confined space requirement: "True", functional location: "West"
	And a facility "two" exists with operating center: "nj4", facility id: "NJSB-3", facility name: "A Nother Facility", planning plant: "two", has confined space requirement: "True", functional location: "North"
	And a facility "five" exists with operating center: "nj4", facility id: "NJSB-4", facility name: "A Nother Facility sr", planning plant: "two", has confined space requirement: "True", functional location: "Ni"
	And a facility area "one" exists with description: "facilityLab"
	And a facility area "two" exists with description: "chemical"
	And a facility sub area "one" exists with description: wet, area: "one"
	And a facility sub area "two" exists with description: Lime, area: "two"
	And a facility facility area "one" exists with facility: "one", facilityArea: "one", facilitySubArea: "one"
	And a facility facility area "two" exists with facility: "one", facilityArea: "two", facilitySubArea: "two"
	And equipment types exist
	#And production prerequisites exist
	And a production prerequisite "one" exists with description: "has lockout requirement"
	And a production prerequisite "two" exists with description: "is confined space"
	And a production prerequisite "six" exists with description: "pre job safety brief"
	#corrective order code exists
	And a corrective order problem code "two" exists with code: "CORR", description: "corrosion", id: 2
	And a corrective order problem code "three" exists with code: "MUD", description: "dirty", id: 3
	And equipment type: "rtu" exists in corrective order problem code: "two"
	And equipment type: "rtu" exists in corrective order problem code: "three"
	And an equipment category "two" exists with description: "Cat"
	And an equipment subcategory "two" exists with description: "SubCat"
	And an equipment purpose "aerator" exists with description: "aerator", equipment type: "rtu", equipment category: "two", equipment subcategory: "two"
	And an equipment purpose "rehab" exists with description: "rehab purpose", equipment type: "rtu", equipment category: "two", equipment subcategory: "two", equipment lifespan: "one"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", facility facility area: "one", equipment type: "rtu", description: "RTU1", s a p equipment id: 1
	And an equipment "two" exists with identifier: "NJSB-2-EQID-1", facility: "two", facility facility area: "two", equipment type: "rtu", description: "RTU2", s a p equipment id: 2
	And an equipment "rehab" exists with identifier: "NJSB-3-EQID-1", facility: "one", facility facility area: "one", equipment type: "rtu", description: "RTU3", s a p equipment id: 3, equipment purpose: "rehab", date installed: "3652 days ago", planned replacement year: "2061", estimated replace cost: "4242.42" 
	And equipment: "one" has production prerequisite: "one"
	And equipment: "one" has production prerequisite: "two"
	And equipment: "two" has production prerequisite: "two"
	And a production skill set "one" exists
	And a production work order cancellation reason "one" exists with description: "No Longer Valid"
	And order types exist
	And a production work description "one" exists with production skill set: "one", equipment type: "generator"
	And a production work description "oneRoutine" exists with production skill set: "one", equipment type: "generator", order type: "routine"
	And a production work description "two" exists with production skill set: "one", equipment type: "rtu", description: "two", order type: "operational activity"
	And a production work description "three" exists with production skill set: "one", equipment type: "rtu", description: "three", order type: "pm work order"
	And a production work description "approval" exists with production skill set: "one", equipment type: "rtu", description: "approval", order type: "corrective action"
	And a production work description "rehab" exists with production skill set: "one", equipment type: "rtu", description: "REHAB/RENEW", order type: "corrective action"
	And a production work description "maintenance plan" exists with order type: "routine", description: "Maintenance Plan"
	# the following description needs to exist so a RequiredWhen validator doesn't crash.
	And a production work description "four" exists with production skill set: "one", equipment type: "rtu", description: "four", order type: "corrective action"
	And a production work description "routine" exists with production skill set: "one", equipment type: "generator", description: "routine", order type: "routine"
	And a production work description "general" exists with production skill set: "one", equipment type: "generator", description: "GENERAL REPAIR", order type: "corrective action"
	And a production work order priority "one" exists with description: "routine"
	And a production work order "one" exists with productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one", equipment: "one", estimated completion hours: "1", local task description: "description one"
	And a production work order "two" exists with productionWorkDescription: "one", operating center: "nj7", planning plant: "one", facility: "one", equipment: "one", functional location: "1-2-3-4", estimated completion hours: "1"
	And a production work order "three" exists with productionWorkDescription: "one", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", estimated completion hours: "1"
	And a production work order "four" exists with productionWorkDescription: "one", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", s a p maintenance plan id: "123456", estimated completion hours: "1"
	And a production work order "five" exists with productionWorkDescription: "three", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", order type: "pm work order", priority: "one", estimated completion hours: "1", local task description: "description five"
	And a production work order "approval" exists with productionWorkDescription: "approval", operating center: "nj7", date completed: "8/17/2019"
	And an employee "one" exists with operating center: "nj7" 
	And a production work order "approval1" exists with productionWorkDescription: "approval", operating center: "nj3", date completed: "8/17/2019"
	And an employee assignment "one" exists with production work order: "one", assigned to: "both-edit", assigned for: "today"
	And an employee assignment "two" exists with production work order: "one", assigned to: "nj7-edit", assigned for: "today"
	And an employee assignment "three" exists with production work order: "two", assigned to: "both-edit", assigned for: "today"
	And an employee assignment "four" exists with production work order: "two", assigned to: "nj4-edit", assigned for: "today", date started: "today"
	And an employee assignment "five" exists with production work order: "two", assigned to: "nj7-edit", assigned for: "today", date started: "today", assigned by: "both-edit"
	And a unit of measure "one" exists with description: "gallons"
	And a measurement point equipment type "one" exists with equipment type: "rtu", unit of measure: "one", description: "test", min: "0.0", max: "100.0", category: "g", position: "100", is active: "true"
	And a production work order production prerequisite "one" exists with production work order: "three"
	And a production pre job safety brief "one" exists with production work order: "one"
	And a production pre job safety brief worker exists with production pre job safety brief: "one", employee: "both-edit"
	And a production pre job safety brief worker exists with production pre job safety brief: "one", employee: "nj7-edit"
	And a production pre job safety brief "two" exists with production work order: "two"
	And a production pre job safety brief worker exists with production pre job safety brief: "two", employee: "both-edit"
	And a production pre job safety brief worker exists with production pre job safety brief: "two", employee: "nj4-edit"
	And a task group category "one" exists with description: "This is the description", type: "chemical", abbreviation: "CHEM", is active: "true"
	And a task group "one" exists with task group id: "Tasky Id", task group name: "TaskyName", task details: "Tasky details", task details summary: "Tasky summary", task group category: "one", resources: "1", estimated hours: "2", contractor cost: "3", equipment types: "rtu", task group categories: "one"
	And production work order frequencies exist
	And a maintenance plan "one" exists with start: "02/24/2020", planning plant: "one", facilities: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", production work description: "one"
	And an equipment lifespan "one" exists with description: "rehab equipment lifespan", estimated life span: "20", extended life major: "7"
	And an as found condition "one" exists with description: "Unable to Inspect"
	And an as found condition "two" exists with description: "Serious Deterioration"
	And an as found condition "three" exists with description: "Some Deterioration"
	And an as found condition "four" exists with description: "Questionable"
	And an as found condition "five" exists with description: "Acceptable / Good"
	And as left conditions exist
	And a condition type "one" exists with description: "As Found"
	And a condition description "one" exists with description: "Unable to Inspect 1", condition type: "one"
	And an asset condition reason "one" exists with code: "AFCL", description: "Cannot Locate 1", condition description: "one"
	And an asset condition reason "three" exists with code: "AFOC", description: "Operational Constraint 1", condition description: "one"
	And an asset condition reason "five" exists with code: "AFOO", description: "Out Of Service 1", condition description: "one"
	And an asset condition reason "seven" exists with code: "AFSP", description: "Scheduling Problem 1", condition description: "one"
	And an asset condition reason "nine" exists with code: "AFSC", description: "Seasonal Constraint 1", condition description: "one"
	And a condition type "two" exists with description: "As Left"
	And a condition description "two" exists with description: "Unable to Inspect 2", condition type: "two"
	And an asset condition reason "two" exists with code: "AFCL", description: "Cannot Locate 2", condition description: "two"
	And an asset condition reason "four" exists with code: "AFOC", description: "Operational Constraint 2", condition description: "two"
	And an asset condition reason "six" exists with code: "AFOO", description: "Out Of Service 2", condition description: "two"
	And an asset condition reason "eight" exists with code: "AFSP", description: "Scheduling Problem 2", condition description: "two"
	And an asset condition reason "ten" exists with code: "AFSC", description: "Seasonal Constraint 2", condition description: "two"

Scenario: user can search for a production work order with multiple equipment and does not see duplicate rows
	Given a production work order "multi" exists with productionWorkDescription: "oneRoutine", operating center: "nj7", planning plant: "one", facility: "one", equipment: "one"
	And a production work order equipment "pwoe1" exists with production work order: "multi", equipment: "one", s a p notification number: "123", completed on: "10/01/2021"
	And a production work order equipment "pwoe2" exists with production work order: "multi", equipment: "one", s a p notification number: "123", completed on: "10/01/2021"
	And a production work order equipment "pwoe3" exists with production work order: "multi", equipment: "two", s a p notification number: "456", completed on: "10/05/2021"
	And a production work order equipment "pwoe4" exists with production work order: "multi", equipment: "two", s a p equipment id: "123456789", s a p notification number: "456", completed on: "10/06/2021"
	And a production work order equipment "pwoe5" exists with production work order: "multi", s a p equipment id: "123456789", s a p notification number: "789", completed on: "10/10/2021"
	And a production work order equipment "pwoe6" exists with production work order: "multi", s a p equipment id: "123456789", s a p notification number: "789", completed on: "10/10/2021"
	And I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/Search page
	When I enter production work order "multi"'s Id into the EntityId field
	And I press Search
	Then I should see "Records found: 1"
	When I follow the show link for production work order "multi"
	And I click the "Equipment" tab
	Then I should see the following values in the equipmentsTable table
	| SAP Equipment | MapCall EquipmentId | Description | As Found / As Left  | SAP Notification Number | Completed On | Equipment Type |
	| 1             | NJ7-1-ETTT-1        | RTU1        | *Acceptable / Good* | 123                     | *10/1/2021*  | RTU_PLC - RTU  |
	| 2             | NJ4-3-ETTT-2        | RTU2        | *Acceptable / Good* | 456                     | *10/5/2021*  | RTU_PLC - RTU  |
	| 2             | NJ4-3-ETTT-2        | RTU2        | *Acceptable / Good* | 456                     | *10/6/2021*  | RTU_PLC - RTU  |
	| 123456789     |                     |             | *Acceptable / Good* | 789                     | *10/10/2021* |                |
	
Scenario: User can create a production work order
	Given I am logged in as "user"
	And production prerequisites exist
	And I am at the Production/ProductionWorkOrder/New page
	When I press Save
	Then I should see a validation message for Facility with "The Facility field is required."
	And I should see a validation message for RequestedBy with "The RequestedBy field is required."
	And I should see a validation message for OrderNotes with "The Notes field is required."
	And I should see a validation message for EstimatedCompletionHours with "The Estimated Completion Time (Hrs) field is required."
	When I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select production work order priority "high" from the Priority dropdown
	And I enter "some notes&lt;html<br>" into the OrderNotes field
	And I select employee "both-edit"'s Description from the RequestedBy dropdown
	And I enter "1.1111" into the EstimatedCompletionHours field
	Then I should see a validation message for EstimatedCompletionHours with "Must be a number with maximum of 2 decimal places"
	When I enter "1.23" into the EstimatedCompletionHours field
	And I press Save
	Then I should see a validation message for Equipment with "The Equipment field is required."
	When I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select corrective order problem code "two" from the CorrectiveOrderProblemCode dropdown
	Then I should not see production work description "one" in the ProductionWorkDescription dropdown
	And I should not see production work description "three" in the ProductionWorkDescription dropdown
	When I select equipment "one"'s Description from the Equipment dropdown
	Then production prerequisite "one" should be checked in the Prerequisites checkbox list
	And production prerequisite "one" should not be enabled in the Prerequisites checkbox list
	When I press Save
	And I wait for the page to reload
	Then I should see a validation message for ProductionWorkDescription with "The Work Description field is required."
	When I select production work description "two" from the ProductionWorkDescription dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown production work order shall henceforth be known throughout the land as "Phillipe"
	And I should see a display for OrderNotes with "some notes&lt;html<br>"	
	And I should see a display for EstimatedCompletionHours with "1.23"

Scenario: Equipment critical notes are displayed when a piece of equipment with critical notes is chosen
	Given an equipment "critical" exists with identifier: "NJSB-1-EQID-2", facility: "one", equipment type: "rtu", critical notes: "foo bar", description: "RTU 1"
	And I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/New page
	When I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
    And I wait for ajax to finish loading
	And I select production work order priority "high" from the Priority dropdown
	And I enter "some notes" into the OrderNotes field
	And I select employee "both-edit"'s Description from the RequestedBy dropdown
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select equipment "one"' Description from the Equipment dropdown
    And I wait for ajax to finish loading
    Then I should see the notification message with equipment: "critical"'s CriticalNotes
	
Scenario: User can create a production work order for another operating center
	Given I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/New page
	When I press Save
	Then I should see a validation message for Facility with "The Facility field is required."
	And I should see a validation message for RequestedBy with "The RequestedBy field is required."
	And I should see a validation message for OrderNotes with "The Notes field is required."
	When I select operating center "nj4" from the OperatingCenter dropdown
	And I select planning plant "two" from the PlanningPlant dropdown
	And I select facility "two"'s Description from the Facility dropdown
	And I select equipment "two"'s Description from the Equipment dropdown
	#facilities with confined space confirmed should check off the prereq
	Then production prerequisite "two" should be checked in the Prerequisites checkbox list
	When I select production work order priority "high" from the Priority dropdown
	And I enter "some notes" into the OrderNotes field
	And I select employee "nj7-edit"'s Description from the RequestedBy dropdown
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select equipment "two"'s Description from the Equipment dropdown
	And I enter "1" into the EstimatedCompletionHours field
	And I press Save
	And I wait for the page to reload
	Then I should see a validation message for ProductionWorkDescription with "The Work Description field is required."
	When I select equipment type "rtu"'s Description from the EquipmentType dropdown
	Then I should not see production work description "one" in the ProductionWorkDescription dropdown
	And I should not see production work description "three" in the ProductionWorkDescription dropdown
	When I select equipment "two"'s Description from the Equipment dropdown
	And I select production work description "two" from the ProductionWorkDescription dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown production work order shall henceforth be known throughout the land as "Phillipe"
	And I should see a display for OrderNotes with "some notes"
	
Scenario: user can create a production work order with requested by from another operating center
	Given I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/New page
	When I press Save
	Then I should see a validation message for Facility with "The Facility field is required."
	And I should see a validation message for RequestedBy with "The RequestedBy field is required."
	And I should see a validation message for OrderNotes with "The Notes field is required."
	When I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select production work order priority "high" from the Priority dropdown
	And I enter "some notes" into the OrderNotes field
	And I select employee "nj7-edit"'s Description from the RequestedBy dropdown
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	And I enter "1" into the EstimatedCompletionHours field
	And I press Save
	And I wait for the page to reload
	Then I should see a validation message for ProductionWorkDescription with "The Work Description field is required."
	When I select equipment type "rtu"'s Description from the EquipmentType dropdown
	Then I should not see production work description "one" in the ProductionWorkDescription dropdown
	And I should not see production work description "three" in the ProductionWorkDescription dropdown
	When I select equipment "one"' Description from the Equipment dropdown
	And I select production work description "two" from the ProductionWorkDescription dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown production work order shall henceforth be known throughout the land as "Phillipe"
	And I should see a display for OrderNotes with "some notes"

Scenario: User on corrective order should not see Pre Job Safety Brief pre requisite 
	Given I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/New page
	When I select facility "one"'s Description from the Facility dropdown
	And I wait for ajax to finish loading
	Then I should see production prerequisite "six" in the Prerequisites checkbox list
	When I wait for ajax to finish loading
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	When I wait for ajax to finish loading
	And I select corrective order problem code "two" from the CorrectiveOrderProblemCode dropdown
	And I wait for ajax to finish loading
	Then I should not see production prerequisite "six" in the Prerequisites checkbox list
	
Scenario: user with edit can add and start and end an employee assignment
	Given I am logged in as "adminuser"
	# Needed because the end assignment action creates a Note
	And a data type "productionworkorder" exists with table name: "ProductionWorkOrders"
	And I am at the Show page for production work order: "two"
	When I click the "Employee Assignments" tab
	And I press "Add Employee Assignment"
    Then I should not see employee "both-read"'s Description in the AssignedTo dropdown
    And I should not see employee "nj4-edit"'s Description in the AssignedTo dropdown
    And I should see employee "nj7-edit"'s Description in the AssignedTo dropdown
	When I select employee "nj7-edit"'s Description from the AssignedTo dropdown
	And I enter today's date into the AssignedFor field
	And I press "Save Employee Assignment"
	And I wait for the page to reload
	When I click the "Employee Assignments" tab
	Then I should see employee "nj7-edit"'s FullName in the table employeeAssignmentsTable's "Assigned To" column
	And I should see employee "both-edit"'s FullName in the table employeeAssignmentsTable's "Assigned By" column
	When I click the "Employee Assignments" tab
	And I click the "Start" button in the 1st row of employeeAssignmentsTable
	And I wait for the page to reload
	And I click the "Employee Assignments" tab
	And I click the "End" button in the 2nd row of employeeAssignmentsTable
	And I press "End Assignment"
	Then I should see the validation message "The Notes field is required."
	Then I should see a validation message for HoursWorked with "The Hours Worked field is required."
	When I enter "1.234" into the HoursWorked field
	Then I should see a validation message for HoursWorked with "Must be a number with maximum of 2 decimal places"
	When I enter "here are some notes" into the Notes field
	And I enter "1.23" into the HoursWorked field
	And I enter "4/24/2084" into the DateEnded field
	And I press "End Assignment"
	And I wait for the page to reload
	And I click the "Employee Assignments" tab
	Then I should see the following values in the employeeAssignmentsTable table
	| Assigned To   | Date Ended                  | Hours Worked |
	| jay bob       | End                         | 0.00         |
	| hotdog johnny | 4/24/2084 12:00:00 AM (EST) | 1.23         |

Scenario: admin can assign to employee from another operating center
	Given I am logged in as "adminuser"
	And I am at the Show page for production work order: "two"
	When I click the "Employee Assignments" tab
	And I press "Add Employee Assignment"
    And I select operating center "nj7" from the OperatingCenter dropdown
	And I select employee "both-edit"'s Description from the AssignedTo dropdown
	And I enter today's date into the AssignedFor field
	And I press "Save Employee Assignment"
	And I wait for the page to reload
	Then I should see employee "both-edit"'s FullName in the table employeeAssignmentsTable's "Assigned To" column

Scenario: user can search for a production work order
	Given I am logged in as "user"
	And production prerequisites exist
	And I am at the Production/ProductionWorkOrder/Search page
	When I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
    Then I should be at the Production/ProductionWorkOrder page
    And I should see a link to the Show page for production work order "one"
    And I should see a link to the Show page for production work order "two"
    And I should not see a link to the Show page for production work order "three"
    And I should not see a link to the Show page for production work order "four"
	When I visit the Production/ProductionWorkOrder/Show/1 page
	Then I shouldn't see the "materials" tab
	When I visit the Production/ProductionWorkOrder/Search page
	And I select state "one" from the State dropdown
    And I select operating center "nj4" from the OperatingCenter dropdown
	And I press Search
    Then I should be at the Production/ProductionWorkOrder page
    And I should not see a link to the Show page for production work order "one"
    And I should not see a link to the Show page for production work order "two"
    And I should see a link to the Show page for production work order "three"
    And I should see a link to the Show page for production work order "four"
	And I should see the following values in the WorkOrderSearchResultsTable table
	| Id | Date Created | Estimated Completion Time (Hrs) |
	| 3  | 1/1/2017      | 1                               |
	When I visit the Production/ProductionWorkOrder/Search page
	And I select state "one" from the State dropdown 
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
    And I should see a link to the Show page for production work order "three"
	When I visit the Production/ProductionWorkOrder/Show/3 page
	Then I shouldn't see the "materials" tab
	When I visit the Production/ProductionWorkOrder/Search page
	And I select "Has Lockout Requirement" from the Prerequisites dropdown 
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
    And I should see a link to the Show page for production work order "three"
	When I visit the Production/ProductionWorkOrder/Search page
	And I type "123456" into the SAPMaintenancePlanId field
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
	And I should see a link to the Show page for production work order "four"
	And I should not see a link to the Show page for production work order "one"
    And I should not see a link to the Show page for production work order "two"
	And I should not see a link to the Show page for production work order "three"
	When I visit the Production/ProductionWorkOrder/Show/4 page
	Then I shouldn't see the "materials" tab
	When I visit the Production/ProductionWorkOrder/Search page
	And I type "one" into the LocalTaskDescription field
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
	And I should see a link to the Show page for production work order "one"
	And I should not see a link to the Show page for production work order "five"
	When I visit the Production/ProductionWorkOrder/Search page
	And I type "five" into the LocalTaskDescription field
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
	And I should not see a link to the Show page for production work order "one"
	And I should see a link to the Show page for production work order "five"

Scenario: User can search for a production work order with a maintenance plan number
	Given a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", production work description: "two", is active: true, local task description: "What is your favorite color?", plan number: "900000002"
	And a production work order "pwo-mpnumber" exists with productionWorkDescription: "three", operating center: "nj7", date completed: "4/24/1984", planning plant: "two", facility: "two", equipment: "two", order type: "pm work order", priority: "routine", estimated completion hours: "1", local task description: "pwo with mp description", maintenance plan: "two" 
	And I am logged in as "user"
	When I visit the Production/ProductionWorkOrder/Search page
	And I enter "900000002" into the PlanNumber field
	And I press "Search"
	Then I should be at the Production/ProductionWorkOrder page
	And I should see a link to the Show page for production work order "pwo-mpnumber"

Scenario: User can search for a production work order with a Plan Type
	Given a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", production work description: "two", is active: true, local task description: "What is your favorite color?", plan number: "900000002"
	And a maintenance plan task type "one" exists with description: "A good plan"
	And a production work order "pwo-mpnumber" exists with productionWorkDescription: "three", operating center: "nj7", date completed: "4/24/1984", planning plant: "two", facility: "two", equipment: "two", order type: "pm work order", priority: "routine", estimated completion hours: "1", local task description: "pwo with mp description", maintenance plan: "two" 
	And I am logged in as "user"
	When I visit the Production/ProductionWorkOrder/Search page
	And I select maintenance plan task type "one" from the TaskType dropdown
	And I press "Search"
	Then I should be at the Production/ProductionWorkOrder page		

Scenario: User can search for a production work order with a Task Group
	Given a maintenance plan task type "one" exists with description: "A good plan"
	And a task group "tasky" exists with task group id: "ABC12", task group name: "TaskyName", IsActive: true, description: "Tasky description", required: "true", frequency: "123", equipment types: "aerator", maintenance plan task type: "one"
	And a production work order "pwo-mpnumber" exists with productionWorkDescription: "three", operating center: "nj7", date completed: "4/24/1984", planning plant: "two", facility: "two", equipment: "two", order type: "pm work order", priority: "routine", estimated completion hours: "1", local task description: "pwo with mp description"
	And I am logged in as "user"
	When I visit the Production/ProductionWorkOrder/Search page
	And I select maintenance plan task type "one" from the TaskType dropdown
	And I select "TaskyName" from the TaskGroup dropdown
	And I press "Search"
	Then I should be at the Production/ProductionWorkOrder page	

Scenario: user can search for a supervisor approval
	Given I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/Search page
	When I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
	And I select "Yes" from the RequiresSupervisorApproval dropdown
	And I press Search
    Then I should be at the Production/ProductionWorkOrder page
    And I should see a link to the Show page for production work order "approval"
	And I should not see a link to the Show page for production work order "one"
	And I should not see a link to the Show page for production work order "two"
	When I follow the Show link for production work order "approval"
	Then I should be at the Show page for production work order "approval"

Scenario: user can search for employee assignments
	Given I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/Search page
	When I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select "Yes" from the HasAssignmentsOnNonCancelledWorkOrder dropdown
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
	And I should see a link to the Show page for production work order "one"
	And I should see a link to the Show page for production work order "two"
	And I should not see a link to the Show page for production work order "three"
	And I should not see a link to the Show page for production work order "approval"
	When I follow the Show link for production work order "one"
	Then I should be at the Show page for production work order "one"
	When I visit the Production/ProductionWorkOrder/Search page
	When I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select "No" from the HasAssignmentsOnNonCancelledWorkOrder dropdown
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
	And I should not see a link to the Show page for production work order "one"
	And I should not see a link to the Show page for production work order "two"
	And I should see a link to the Show page for production work order "approval"
	
Scenario: admin can add and start and end an employee assignment
	Given I am logged in as "adminuser"
	# Needed because the end assignment action creates a Note
	And a data type "productionworkorder" exists with table name: "ProductionWorkOrders"
	And I am at the Show page for production work order: "two"
	When I click the "Employee Assignments" tab
	And I press "Add Employee Assignment"
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select employee "both-edit"'s Description from the AssignedTo dropdown
	And I enter today's date into the AssignedFor field
	And I press "Save Employee Assignment"
	And I wait for the page to reload
	Then I should see employee "both-edit"'s FullName in the table employeeAssignmentsTable's "Assigned To" column
	When I click the "Employee Assignments" tab
	And I click the "Start" button in the 1st row of employeeAssignmentsTable
	And I wait for the page to reload
	And I click the "Employee Assignments" tab
	And I click the "End" button in the 1st row of employeeAssignmentsTable
	And I press "End Assignment"
	Then I should see the validation message "The Notes field is required."
	When I enter "here are some notes" into the Notes field
	And I enter "4/24/2084" into the DateEnded field
	And I enter "1.23" into the HoursWorked field
	And I press "End Assignment"
	And I wait for the page to reload
	Then I should see the following values in the employeeAssignmentsTable table
	| Assigned To | Date Ended                  | Hours Worked |
	| jay bob     | 4/24/2084 12:00:00 AM (EST) | 1.23         |

Scenario: User can see supervisor approval section on show view regardless of order type
	# MC-1331 Supervisor Approval is only supposed to be visible for corrective and capital work orders
	# but old work orders still have this value set and must be visible.
	Given a production work description "notcapitalorcorrective" exists with production skill set: "one", equipment type: "rtu", description: "two", order type: "pm work order"
	And a production work order "old-no-date" exists with productionWorkDescription: "notcapitalorcorrective", operating center: "nj7", date completed: "4/24/1984"
	And a production work order "old-with-date" exists with productionWorkDescription: "notcapitalorcorrective", operating center: "nj7", date completed: "4/24/1984", approved on: "4/24/1984"
	And I am logged in as "user"
	When I visit the Show page for production work order "old-no-date"
	And I click the "Finalization" tab
	Then I should not see "Supervisor Approval"
	When I visit the Show page for production work order "old-with-date"
	And I click the "Finalization" tab
	Then I should see a display for ApprovedOn with "4/24/1984 12:00:00 AM (EST)"

Scenario: User can set supervisor approval for corrective or capital orders
	Given a production work description "corrective" exists with production skill set: "one", equipment type: "rtu", description: "two", order type: "corrective action"
	And a production work description "capital" exists with production skill set: "one", equipment type: "rtu", description: "two", order type: "rp capital"
	And a production work order "corrective" exists with productionWorkDescription: "corrective", operating center: "nj7", date completed: "4/24/1984"
	And a production work order "capital" exists with productionWorkDescription: "capital", operating center: "nj7", date completed: "4/24/1984"
	And a production work order cause code "one" exists
	And I am logged in as "user"
	# for corrective orders
	When I visit the Show page for production work order "corrective"
	And I click the "Finalization" tab
	And I select production work order cause code "one" from the CauseCode dropdown
	And I click ok in the dialog after pressing "Approve Order"
	Then I should be at the Show page for production work order "corrective"
	When I click the "Finalization" tab
	Then I should see a display for ApprovedBy with "user"
	# for capital orders
	When I visit the Show page for production work order "capital"
	And I click the "Finalization" tab
	Then I should not see the CauseCode field
	When I click ok in the dialog after pressing "Approve Order"
	Then I should be at the Show page for production work order "capital"
	When I click the "Finalization" tab
	Then I should see a display for ApprovedBy with "user"

Scenario: User can add a lockout form prerequisite and enter a lockout form then start assignments
	Given production prerequisites exist
	And a role "operations-all-nj7" exists with action: "UserAdministrator", module: "OperationsLockoutForms", operating center: "nj7"
	And a user "user-all" exists with username: "user-all", roles: productionworkorder-all-nj7;operations-all-nj7
	And a role "user-all-facility" exists with action: "UserAdministrator", module: "ProductionFacilities", user: "user-all"
	And I am logged in as "user-all"
	When I visit the Show page for production work order "one"
	And I click the "Prerequisites" tab
	And I press "Add Prerequisite"
	And I select production prerequisite "has lockout requirement" from the ProductionPrerequisite dropdown
	And I press "Save Prerequisite"
	And I wait for the page to reload
	When I click the "Employee Assignments" tab
	Then I should see the following values in the employeeAssignmentsTable table
	| Assigned To   | Date Started                                                                        |
	| jay bob       | Work cannot be started. Please ensure you have met all the prerequisite conditions. |
	| johnny hotdog | Work cannot be started. Please ensure you have met all the prerequisite conditions. |
	When I click the "Lockout Forms" tab
	And I follow "New Lockout Form"
	And I wait for the page to reload
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	And I should see facility "one"'s Description in the Facility dropdown
	And I should see production work order "one" in the ProductionWorkOrder dropdown
	Given a lockout form "nj7" exists with operating center: "nj7", production work order: "one"
	When I visit the Show page for production work order "one"
	When I click the "Employee Assignments" tab
	Then I should see the following values in the employeeAssignmentsTable table
	| Assigned To   | Date Started                            |
	| jay bob       | *Press 'Start' to use the current time* |
	| johnny hotdog | *Press 'Start' to use the current time* |	

Scenario: User can see equipments on a lockout form that are only available from Production workorder
	Given production prerequisites exist
	And a role "operations-all-nj7" exists with action: "UserAdministrator", module: "OperationsLockoutForms", operating center: "nj7"
	And a user "user-all" exists with username: "user-all", roles: productionworkorder-all-nj7;operations-all-nj7
	And a role "user-all-facility" exists with action: "UserAdministrator", module: "ProductionFacilities", user: "user-all"
	And an equipment category "one" exists with description: "Cat"
	And an equipment subcategory "one" exists with description: "SubCat"
	And an equipment purpose "well" exists with equipment category: "one", equipment subcategory: "one", description: "WELL", abbreviation: "abba", equipment type: "well"
	And an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well", s a p equipment id: 4, EquipmentPurpose: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "nj7", production work description: "three", equipment type: "well"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And I am logged in as "user-all"
	When I visit the Show page for production work order "pwo-01"
	And I click the "Prerequisites" tab
	And I press "Add Prerequisite"
	And I select production prerequisite "has lockout requirement" from the ProductionPrerequisite dropdown
	And I press "Save Prerequisite"
	And I wait for the page to reload
	When I click the "Lockout Forms" tab
	And I follow "New Lockout Form"
	And I wait for the page to reload
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	And I should see equipment purpose "well"'s Description in the EquipmentType dropdown

Scenario: User can add a confined space form prerequisite and assignments can or cannot be started based on it
	Given production prerequisites exist
	And I am logged in as "both-edit"
	When I visit the Show page for production work order "one"
	And I click the "Prerequisites" tab
	And I press "Add Prerequisite"
	And I select production prerequisite "is confined space" from the ProductionPrerequisite dropdown
	And I press "Save Prerequisite"
	And I wait for the page to reload
	Then I should see the notification message "A confined space form is required but has not been entered. Please use the confined space form tab to create one."
	When I click the "Employee Assignments" tab
	Then I should see the following values in the employeeAssignmentsTable table
	| Assigned To   | Date Started                                                                        |
	| jay bob       | Work cannot be started. Please ensure you have met all the prerequisite conditions. |
	| johnny hotdog | Work cannot be started. Please ensure you have met all the prerequisite conditions. |
	When I visit the Show page for production work order "one"
	And I click the "Prerequisites" tab
	Then I should see the following values in the prerequisitesTable table 
	| Production Prerequisite | Remove Requirement |
	| Is Confined Space | No               |
	When I click the "Edit" link in the 1st row of prerequisitesTable
	And I select "Yes" from the SkipRequirement dropdown
	And I enter "6/15/2018 15:07:00" into the SatisfiedOn field
	And I press Save
	Then I should see a validation message for SkipRequirementComments with "Please describe why the prerequisite is no longer required."
	When I enter "this was a mistake" into the SkipRequirementComments field
	And I press Save
	Then I should be at the Show page for production work order: "one"
	When I click the "Prerequisites" tab
	Then I should see the following values in the prerequisitesTable table 
	| Production Prerequisite | Satisfied On         | Remove Requirement | Remove Requirement Comments |
	| Is Confined Space       | 6/15/2018 3:07:00 PM (EST) | Yes              | this was a mistake        |
	When I click the "Employee Assignments" tab
	Then I should see the following values in the employeeAssignmentsTable table
	| Assigned To   | Date Started                            |
	| jay bob       | *Press 'Start' to use the current time* |
	| johnny hotdog | *Press 'Start' to use the current time* |

Scenario: User can add a lockout form prerequisite set it to skip and not see the notification message
	Given production prerequisites exist
	And I am logged in as "both-edit"
	When I visit the Show page for production work order "one"
	And I click the "Prerequisites" tab
	And I press "Add Prerequisite"
	And I select production prerequisite "has lockout requirement" from the ProductionPrerequisite dropdown
	And I press "Save Prerequisite"
	And I wait for the page to reload
	Then I should see the notification message "A lockout form is required but has not been entered. Please use the lockout form tab to create one."
	When I click the "Prerequisites" tab
	And I click the "Edit" link in the 1st row of prerequisitesTable
	And I wait for the page to reload
	And I select "Yes" from the SkipRequirement dropdown
	And I enter "to get passed this requirement" into the SkipRequirementComments field
	And I press "Save"
	And I wait for the page to reload
	Then I should not see the notification message "A lockout form is required but has not been entered. Please use the lockout form tab to create one."
	And I should not see the "Lockout Forms" tab

Scenario: Editing the Production Work Order sets Confined Space Prerequisite if the Facility requires it
	Given production prerequisites exist
	And I am logged in as "user"
	When I visit the Show page for production work order "one"
	When I visit the Edit page for production work order: "one"
	And I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	And I wait for ajax to finish loading
	Then production prerequisite "one" should be checked in the Prerequisites checkbox list
	
Scenario: User cannot set a prerequisite twice via edit
	Given I am logged in as "user"
	And production prerequisites exist
	And I am at the Production/ProductionWorkOrder/New page	
	When I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I enter "1" into the EstimatedCompletionHours field
	And I select production work order priority "high" from the Priority dropdown
	And I enter "some notes" into the OrderNotes field
	And I select employee "both-edit"'s Description from the RequestedBy dropdown
	And I check production prerequisite "is confined space" in the Prerequisites checkbox list
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select production work description "two" from the ProductionWorkDescription dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	Then production prerequisite "one" should be checked in the Prerequisites checkbox list
	When I press Save
	And I wait for the page to reload
	Then the currently shown production work order shall henceforth be known throughout the land as "PhillipeJr"
	When I click the "Prerequisites" tab
	Then I should see the following values in the prerequisitesTable table 
	| Production Prerequisite | Remove Requirement |
	| Has Lockout Requirement | No               |
	| Is Confined Space       | No               |
	When I visit the Edit page for production work order "PhillipeJr"
	And I check production prerequisite "is confined space" in the Prerequisites checkbox list
	And I select equipment "one"'s Description from the Equipment dropdown
	And I press Save
	When I click the "Prerequisites" tab
	Then I should see the following values in the prerequisitesTable table 
	| Production Prerequisite | Remove Requirement |
	| Has Lockout Requirement | No               |
	| Is Confined Space       | No               |

Scenario: User can add a lockout form prerequisite and assignments can or cannot be started based on it
	Given production prerequisites exist
	And I am logged in as "both-edit"
	When I visit the Show page for production work order "one"
	And I click the "Prerequisites" tab
	And I press "Add Prerequisite"
	And I select production prerequisite "has lockout requirement" from the ProductionPrerequisite dropdown
	And I press "Save Prerequisite"
	And I wait for the page to reload
	When I click the "Employee Assignments" tab
	Then I should see the following values in the employeeAssignmentsTable table
	| Assigned To   | Date Started                                                                        |
	| jay bob       | Work cannot be started. Please ensure you have met all the prerequisite conditions. |
	| johnny hotdog | Work cannot be started. Please ensure you have met all the prerequisite conditions. |
	When I visit the Production/EmployeeAssignment/Search page
	And I press "Search"
	Then I should see the following values in the employeeAssignmentsTable table
	| Production Work Order | Assigned To | Start                                                                               |
	| 1                     | jay bob     | Work cannot be started. Please ensure you have met all the prerequisite conditions. |
	When I click the "1" link in the 1st row of employeeAssignmentsTable
	And I click the "Employee Assignments" tab
	And I click the "View" link in the 1st row of employeeAssignmentsTable
	Then I should see "Work cannot be started. Please ensure you have met all the prerequisite conditions."
	When I visit the Show page for production work order "one"
	And I click the "Prerequisites" tab
	Then I should see the following values in the prerequisitesTable table 
	| Production Prerequisite | Remove Requirement |
	| Has Lockout Requirement | No               |
	When I click the "Edit" link in the 1st row of prerequisitesTable
	And I select "Yes" from the SkipRequirement dropdown
	And I press Save
	Then I should see a validation message for SkipRequirementComments with "Please describe why the prerequisite is no longer required."
	When I enter "this was a mistake" into the SkipRequirementComments field
	And I press Save
	Then I should be at the Show page for production work order: "one"
	When I click the "Prerequisites" tab
	Then I should see the following values in the prerequisitesTable table 
	| Production Prerequisite | Remove Requirement | Remove Requirement Comments |
	| Has Lockout Requirement | Yes              | this was a mistake        |
	When I click the "Employee Assignments" tab
	Then I should see the following values in the employeeAssignmentsTable table
	| Assigned To   | Date Started                            |
	| jay bob       | *Press 'Start' to use the current time* |
	| johnny hotdog | *Press 'Start' to use the current time* |
	And I should not see "Work cannot be started. Please ensure you have met all the prerequisite conditions."
	When I visit the Production/EmployeeAssignment/Search page
	And I press "Search"
	Then I should not see "Work cannot be started. Please ensure you have met all the prerequisite conditions."
	When I click the "1" link in the 1st row of employeeAssignmentsTable
	And I click the "Employee Assignments" tab
	And I click the "View" link in the 1st row of employeeAssignmentsTable
	Then I should not see "Work cannot be started. Please ensure you have met all the prerequisite conditions."

Scenario: User can view measurement points after work order is completed
	Given a user "testuser" exists with username: "stuff", full name: "the useriest user"
	And a production work order "twotwo" exists with equipment: "two", operating center: "nj7", completed by: "testuser", date completed: "02/10/2020 12:00"
	And a production work order equipment "pwoe" exists with production work order: "twotwo", equipment: "two"
	And a production work order measurement point value exists with production work order: "twotwo", equipment: "two", value: "2", measurement point equipment type: "one"
	And I am logged in as "user"
	When I visit the Show page for production work order: "twotwo"
	And I click the "Measuring Points" tab
	Then I should see a link to the show page for equipment: "two"
	And I should see "2" in the table measurementPointsTable's "Value" column
	And I should see "g" in the table measurementPointsTable's "Category" column
	And I should see "test" in the table measurementPointsTable's "Description" column
	And I should see "100" in the table measurementPointsTable's "Position" column
	And I should see "gallons" in the table measurementPointsTable's "Unit Of Measure" column
	And I should see "0" in the table measurementPointsTable's "Min" column
	And I should see "100" in the table measurementPointsTable's "Max" column
	And I should not see the button "Create Measurement Point"
	And I should not see the button "Update Measurement Point"

Scenario: User can edit measurement points when work order is not complete
	Given a user "testuser" exists with username: "stuff", full name: "the useriest user"
	And a production work order "twotwotwo" exists with equipment: "two", operating center: "nj7", completed by: "testuser"
	And a production work order equipment "pwoe" exists with production work order: "twotwotwo", equipment: "two"
	And a production work order measurement point value exists with production work order: "twotwotwo", equipment: "two", value: "2", measurement point equipment type: "one"
	And I am logged in as "user"
	When I visit the Show page for production work order: "twotwotwo"
	And I click the "Measuring Points" tab
    Then I should see "2" in the Value field
	When I enter "3" into the Value field
	And I press "Update Measurement Point"
	And I reload the page
	And I click the "Measuring Points" tab
	Then I should see "3" in the Value field

Scenario: User can add measurements points when work order is not complete
	Given a user "testuser" exists with username: "stuff", full name: "the useriest user"
	And a production work order "twotwotwotwo" exists with equipment: "two", operating center: "nj7", completed by: "testuser"
	And a production work order equipment "pwoe" exists with production work order: "twotwotwotwo", equipment: "two"
	And I am logged in as "user"
	When I visit the Show page for production work order: "twotwotwotwo"
	And I click the "Measuring Points" tab
	Then I should see "" in the Value field
	When I enter "2" into the Value field 
	And I press "Create Measurement Point"
	And I reload the page 
	And I click the "Measuring Points" tab
	Then I should see "2" in the Value field

Scenario: user can search by lockout form created yes no
	Given I am logged in as "user"
	And a lockout form "nj7" exists with operating center: "nj7", production work order: "one"
	And I am at the Production/ProductionWorkOrder/Search page
	When I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
    And I select "Yes" from the LockoutForms dropdown
	And I press Search
    Then I should be at the Production/ProductionWorkOrder page
    And I should see a link to the Show page for production work order "one"
	And I should not see a link to the Show page for production work order "two"
	And I should not see a link to the Show page for production work order "approval"
	When I visit the Production/ProductionWorkOrder/Search page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select "No" from the LockoutForms dropdown
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
	And I should not see a link to the Show page for production work order "one"
	And I should see a link to the Show page for production work order "two"
	And I should see a link to the Show page for production work order "approval"

Scenario: user can search by lockout form still open
	Given I am logged in as "user"
	And a lockout form "nj7" exists with operating center: "nj7", production work order: "one"
	And a lockout form "nj4" exists with operating center: "nj4", production work order: "two", ReturnedToServiceDateTime: "01/28/2021"
	And I am at the Production/ProductionWorkOrder/Search page
	When I select "Yes" from the IsLockoutFormStillOpen dropdown
	And I press Search
	Then I should be at the Production/ProductionWorkOrder page
	And I should see a link to the Show page for production work order "one"
	And I should not see a link to the Show page for production work order "two"
	When I visit the Production/ProductionWorkOrder/Search page
	And I select "No" from the IsLockoutFormStillOpen dropdown
	And I press Search 
	Then I should be at the Production/ProductionWorkOrder page
	And I should not see a link to the Show page for production work order "one"
	And I should see a link to the Show page for production work order "two"

Scenario: User can search which Production Work Orders have equipment with well tests
	Given an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "nj7", production work description: "three"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And a well test "wt-01" exists with equipment: "equipment-with-well", employee: "one", production work order: "pwo-01", pumping rate: "402", measurement point: "20.50", static water level: "500.50", pumping water level: "600.60"
	And I am logged in as "user"
	When I visit the Production/ProductionWorkOrder/Search page
	And I select "Yes" from the WellTests dropdown
	And I press "Search"
	Then I should see a link to the show page for production work order "pwo-01"

Scenario: User should not see tab for well tests if a production work order is not order type plant maintenance
	Given an equipment "equipment-without-well" exists with facility: "one", equipment type: "generator", description: "generator"
	And a production work order "pwo-01" exists with equipment: "equipment-without-well", operating center: "nj7", production work description: "two"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-without-well"
	And I am logged in as "user"
	When I visit the Show page for equipment: "equipment-without-well"
	Then I should not see the "Well Test Results" tab

Scenario: User can see tab for well tests when production work order is order type plant maintenance
	Given an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "nj7", production work description: "three"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And a well test "wt-01" exists with equipment: "equipment-with-well", employee: "one", production work order: "pwo-01", pumping rate: "402", measurement point: "20.50", static water level: "500.50", pumping water level: "600.60", date of test: "4/1/2021"
	And I am logged in as "user"
	When I visit the Show page for production work order: "pwo-01"
	Then I should see the "Well Test Results" tab
	When I click the "Well Test Results" tab
	Then I should see the following values in the well-tests-table table
	| Id | Equipment | Date Of Test | Employee |
	| well test: "wt-01"'s Id | equipment: "equipment-with-well" | 4/1/2021     | employee: "one" |

Scenario: User can add a well test for a production work order and equipment if it is of type well
	Given an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "nj7"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And I am logged in as "user"
	When I visit the Show page for production work order: "pwo-01"
	Then I should see the "Equipment" tab
	When I click the "Equipment" tab
	Then I should see the link "New Well Test"
	When I click the "New Well Test" link in the 1st row of equipmentsTable
	Then I should be at the Production/WellTest/New page
	
Scenario: safety brief should be required on order type to start assingment and tab should be visible:
	Given a production work description "corrective" exists with production skill set: "one", equipment type: "rtu", description: "two", order type: "corrective action"
	And a production work order "corrective" exists with productionWorkDescription: "corrective", operating center: "nj7"
	And I am logged in as "adminuser"
	When I visit the show page for production work order: "corrective"
	Then I should see the "Pre-Job Safety Briefs" tab
	When I click the "Employee Assignments" tab
	And I press "Add Employee Assignment"
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I select employee "both-edit"'s Description from the AssignedTo dropdown
	And I enter today's date into the AssignedFor field
	And I press "Save Employee Assignment"
	Then I should be at the Show page for production work order "corrective"
	When I click the "Employee Assignments" tab
	Then I should see "Work cannot be started. Please ensure you have met all the prerequisite conditions."

Scenario: safety brief should not be required on order type to start assingment and tab should be invisible:
	Given a production work description "notcapitalorcorrective" exists with production skill set: "one", equipment type: "rtu", description: "two", order type: "pm work order"
	And a production work order "notcorrective" exists with productionWorkDescription: "notcapitalorcorrective", operating center: "nj7"
	And I am logged in as "adminuser"
	When I visit the show page for production work order: "notcorrective"
	Then I should not see the "Pre-Job Safety Briefs" tab
	When I click the "Employee Assignments" tab
	And I press "Add Employee Assignment"
	When I select employee "both-edit"'s Description from the AssignedTo dropdown
	And I enter today's date into the AssignedFor field
	And I press "Save Employee Assignment"
	Then I should be at the Show page for production work order "notcorrective"
	When I click the "Employee Assignments" tab
	Then I should see "Start"

Scenario: safety brief should not be required on routine order type to start assingment and tab should be invisible:
	Given a production work description "routine work description" exists with production skill set: "one", equipment type: "rtu", description: "two", order type: "routine"
	And a production work order "routinePwo" exists with productionWorkDescription: "routine work description", operating center: "nj7"
	And I am logged in as "adminuser"
	When I visit the show page for production work order: "routinePwo"
	Then I should not see the "Pre-Job Safety Briefs" tab
	When I click the "Employee Assignments" tab
	And I press "Add Employee Assignment"
	When I select employee "both-edit"'s Description from the AssignedTo dropdown
	And I enter today's date into the AssignedFor field
	And I press "Save Employee Assignment"
	Then I should be at the Show page for production work order "routinePwo"
	When I click the "Employee Assignments" tab
	Then I should see "Start"

Scenario: Pre job safety brief tab should still be visible if the safety brief is not required but was added already:	
	Given a production work description "notcapitalorcorrective" exists with production skill set: "one", equipment type: "rtu", description: "two", order type: "pm work order"
	And a production work order "notcorrective" exists with productionWorkDescription: "notcapitalorcorrective", operating center: "nj7"
	And a production pre job safety brief "safetybrief" exists with production work order: "notcorrective"
	And I am logged in as "adminuser"
	When I visit the show page for production work order: "notcorrective"
	Then I should see the "Pre-Job Safety Briefs" tab 
	
Scenario: Red tag permit prerequisites only show initially as checked for equipment that is eligible for them:
	Given a production prerequisite "pr-red-tag" exists with description: "red tag permit"
	And an equipment "e-red-tag" exists with identifier: "NJSB-3-EQID-1", facility: "one", equipment type: "fire suppression", description: "red tag permit equipment", s a p equipment id: 4
	And a production work description "pwd-red-tag" exists with production skill set: "one", equipment type: "fire suppression", description: "pwd-red-tag", order type: "operational activity"
	And equipment: "e-red-tag" has production prerequisite: "pr-red-tag"
	And production prerequisites exist
	And I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/New page	
	When I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select production work order priority "high" from the Priority dropdown
	And I enter "some notes" into the OrderNotes field
	And I select employee "both-edit"'s Description from the RequestedBy dropdown
	When I select equipment type "fire suppression"'s Description from the EquipmentType dropdown
	And I select equipment "e-red-tag"'s Description from the Equipment dropdown
	Then production prerequisite "red tag permit" should be checked in the Prerequisites checkbox list
	And production prerequisite "red tag permit" should not be enabled in the Prerequisites checkbox list
	When I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	Then production prerequisite "one" should be checked in the Prerequisites checkbox list
	And production prerequisite "one" should not be enabled in the Prerequisites checkbox list
	And production prerequisite "red tag permit" should not be checked in the Prerequisites checkbox list
	And production prerequisite "red tag permit" should be enabled in the Prerequisites checkbox list

Scenario: User can search for production work orders that have open red tag permits
	Given an equipment "equipment-with-closed-red-tag-permit" exists with facility: "one", equipment type: "fire suppression", description: "equipment with closed red tag"
	And an equipment "equipment-with-open-red-tag-permit" exists with facility: "one", equipment type: "fire suppression", description: "equipment with open red tag"
	And a production work order "pwo-with-closed-red-tag-permit" exists with equipment: "equipment-with-open-red-tag-permit", operating center: "nj7", date completed: "02/10/2020 12:00"
	And a production work order "pwo-with-open-red-tag-permit" exists with equipment: "equipment-with-closed-red-tag-permit", operating center: "nj7", date completed: "02/10/2020 12:00"
	And a red tag permit "rtp-open" exists with equipment: "equipment-with-open-red-tag-permit", person responsible: "nj7-edit", production work order: "pwo-with-open-red-tag-permit"
	And a red tag permit "rtp-closed" exists with equipment: "equipment-with-closed-red-tag-permit", person responsible: "nj7-edit", production work order: "pwo-with-closed-red-tag-permit", equipment restored on: "04/28/2021"
	And I am logged in as "nj7-edit"
	When I visit the Production/ProductionWorkOrder/Search page
	And I check the IsRedTagPermitStillOpen field
	And I press "Search"
	Then I should be at the Production/ProductionWorkOrder page
	And I should see a link to the show page for production work order "pwo-with-open-red-tag-permit"
	And I should not see a link to the show page for production work order "pwo-with-closed-red-tag-permit"
	When I visit the Production/ProductionWorkOrder/Search page
	And I uncheck the IsRedTagPermitStillOpen field
	And I press "Search"
	Then I should be at the Production/ProductionWorkOrder page
	And I should see a link to the show page for production work order "pwo-with-open-red-tag-permit"
	And I should see a link to the show page for production work order "pwo-with-closed-red-tag-permit"

Scenario: User must have an employee record associated in order to authorize a production work order for red tag equipment
	Given an equipment "e" exists with facility: "one", equipment type: "fire suppression", description: "equipment"
	And production prerequisites exist
	And equipment: "e" has production prerequisite: "red tag permit"
	And a production work order "pwo" exists with equipment: "e", operating center: "nj7", equipment type: "fire suppression"
	And a production work order production prerequisite "pwopp" exists with production work order: "pwo", production prerequisite: "red tag permit"
	And a user "admin-no-employee" exists with username: "admin-no-employee", roles: productionworkorder-all-nj7;productionworkorder-all-nj4;facilityrole;equipmentrole
	And I am logged in as "admin-no-employee"
	When I visit the Show page for production work order: "pwo"
	Then I should see the "Red Tag Permit" tab
	When I click the "Red Tag Permit" tab
	And I select "No" from the NeedsRedTagPermitAuthorization dropdown
	And I click ok in the dialog after pressing "Authorize"
	And I wait for the page to reload
	Then I should be at the Show page for production work order: "pwo"
	And I should see "Your user account must have an associated employee record before you can authorize this record."

Scenario: User with employee record can authorize a red tag permit as not required
	Given an equipment "e" exists with facility: "one", equipment type: "fire suppression", description: "equipment"
	And production prerequisites exist
	And equipment: "e" has production prerequisite: "red tag permit"
	And a production work order "pwo" exists with equipment: "e", operating center: "nj7", equipment type: "fire suppression"
	And a production work order production prerequisite "pwopp" exists with production work order: "pwo", production prerequisite: "red tag permit"
	And a production work order equipment "pwoe" exists with is parent: "true", production work order: "pwo", equipment: "e"
	And I am logged in as "nj7-edit"
	When I visit the Show page for production work order: "pwo"
	Then I should see the "Red Tag Permit" tab
	When I click the "Red Tag Permit" tab
	And I select "No" from the NeedsRedTagPermitAuthorization dropdown
	And I click ok in the dialog after pressing "Authorize"
	And I wait for the page to reload
	Then I should be at the Show page for production work order: "pwo" and fragment of "#RedTagPermitTab"
	And I should see a display for NeedsRedTagPermitAuthorizedOn with a date time close to now
	And I should see a display for NeedsRedTagPermitAuthorizedBy with employee "nj7-edit"

Scenario: User with employee record can authorize a red tag permit as required and create a red tag permit
	Given an equipment "e" exists with facility: "one", equipment type: "fire suppression", description: "equipment"
	And production prerequisites exist
	And equipment: "e" has production prerequisite: "red tag permit"
	And a production work order "pwo" exists with equipment: "e", operating center: "nj7", equipment type: "fire suppression"
	And a production work order production prerequisite "pwopp" exists with production work order: "pwo", production prerequisite: "red tag permit"
	And a production work order equipment "pwoe" exists with is parent: "true", production work order: "pwo", equipment: "e"
	And I am logged in as "nj7-edit"
	When I visit the Show page for production work order: "pwo"
	Then I should see the "Red Tag Permit" tab
	When I click the "Red Tag Permit" tab
	And I select "Yes" from the NeedsRedTagPermitAuthorization dropdown
	And I click ok in the dialog after pressing "Authorize"
	And I wait for the page to reload
	Then I should be at the Show page for production work order: "pwo" and fragment of "#RedTagPermitTab"
	And I should see a display for NeedsRedTagPermitAuthorizedOn with a date time close to now
	And I should see a display for NeedsRedTagPermitAuthorizedBy with employee "nj7-edit"
	When I follow "Create Red Tag Permit"
	And I wait for the page to reload
	Then I should be at the HealthAndSafety/RedTagPermit/New page

Scenario: user search for a supervisor approval returns operating center results based on employee role
	Given I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/Search page
	When I select "Yes" from the RequiresSupervisorApproval dropdown
	And I press Search
    Then I should be at the Production/ProductionWorkOrder page
    And I should see a link to the Show page for production work order "approval"
	And I should not see a link to the Show page for production work order "approval1"
	When I follow the Show link for production work order "approval"
	Then I should be at the Show page for production work order "approval" 
	
Scenario: User can download pdf for completed production work order
	Given a user "testuser" exists with username: "stuff", full name: "the useriest user"
	And a production work order "twotwotwo" exists with productionWorkDescription: "three", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", order type: "pm work order", priority: "one", estimated completion hours: "1", local task description: "description five", date completed: "07/13/2021"
	And a production work order measurement point value exists with production work order: "twotwotwo", equipment: "two", value: "2", measurement point equipment type: "one"
	And I am logged in as "user"
	When I visit the Show page for production work order: "twotwotwo"
	Then I should see the "export" button in the action bar
	And I should be able to download production work order "twotwotwo"'s pdf

Scenario: User can not download pdf for a non completed production work order
	Given I am logged in as "user"
	When I visit the Show page for production work order: "approval" 
	Then I should not see the "export" button in the action bar

Scenario: User can download pdf for cancelled production work order
	Given a user "testuser" exists with username: "stuff", full name: "the useriest user"
	And a production work order "twotwotwo" exists with equipment: "two", operating center: "nj7", cancelled by: "testuser", date cancelled: "7/13/2021" 
	And a production work order measurement point value exists with production work order: "twotwotwo", equipment: "two", value: "2", measurement point equipment type: "one"
	And I am logged in as "user"
	When I visit the Show page for production work order: "twotwotwo"
	Then I should see the "export" button in the action bar
	And I should be able to download production work order "twotwotwo"'s pdf

Scenario: User can see cancelled by field on finalization tab when user cancels production work order
	Given a user "testuser" exists with username: "testuser", full name: "the useriest user", roles: productionworkorder-read-nj7;productionworkorder-add-nj7;productionworkorder-edit-nj7;productionworkorder-read-nj4;productionworkorder-add-nj4;productionworkorder-edit-nj4;facilityrole;equipmentrole;prodplannedread
	And a production work order "twotwotwo" exists with equipment: "two", operating center: "nj7"
	And a production work order measurement point value exists with production work order: "twotwotwo", equipment: "two", value: "2", measurement point equipment type: "one"
	And I am logged in as "testuser"
	When I visit the Show page for production work order: "twotwotwo"
	And I click the "Finalization" tab
	And I select production work order cancellation reason "one" from the CancellationReason dropdown
	And I click ok in the dialog after pressing "Cancel Order"
	And I wait for the page to reload
	Then I should be at the Show page for production work order: "twotwotwo"
	When I click the "Finalization" tab
	Then I should see a display for CancelledBy_FullName with "the useriest user"

Scenario: User can add a pre job safety brief prerequisite
	Given production prerequisites exist
	And I am logged in as "both-edit"
	When I visit the Show page for production work order "one"
	And I click the "Prerequisites" tab
	And I press "Add Prerequisite"
	And I select production prerequisite "pre job safety brief" from the ProductionPrerequisite dropdown
	And I press "Save Prerequisite"
	And I wait for the page to reload
	When I click the "Prerequisites" tab
	Then I should see the following values in the prerequisitesTable table 
	| Production Prerequisite | Remove Requirement |
	| Pre Job Safety Brief    | No                 |        	

Scenario: User can see Pre Job Safety Brief tab when Pre Job Safety Brief prerequisite is selected
	Given production prerequisites exist
	And I am logged in as "both-edit"
	When I visit the Show page for production work order "one"
	And I click the "Prerequisites" tab
	And I press "Add Prerequisite"
	And I select production prerequisite "pre job safety brief" from the ProductionPrerequisite dropdown
	And I press "Save Prerequisite"
	And I wait for the page to reload
	Then I should see the "Pre-Job Safety Briefs" tab

Scenario: User see work cannot be started when pre job safety brief prerequisite is enabled
	Given production prerequisites exist	
	And a production work description "safetybrief" exists with production skill set: "one", equipment type: "rtu", description: "two"
	And a production work order "safetybrief" exists with productionWorkDescription: "safetybrief", operating center: "nj7"
	And I am logged in as "adminuser"
	When I visit the show page for production work order: "safetybrief"
	And I click the "Prerequisites" tab
	And I press "Add Prerequisite"
    And I wait for ajax to finish loading
	And I select production prerequisite "pre job safety brief" from the ProductionPrerequisite dropdown
	And I press "Save Prerequisite"
	And I wait for the page to reload
	Then I should see the "Pre-Job Safety Briefs" tab
	When I click the "Employee Assignments" tab
	And I press "Add Employee Assignment"
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I select employee "both-edit"'s Description from the AssignedTo dropdown
	And I enter today's date into the AssignedFor field
	And I press "Save Employee Assignment"
	When I click the "Employee Assignments" tab
	Then I should see "Work cannot be started. Please ensure you have met all the prerequisite conditions."
	
Scenario: user can add the default as found and as left conditions to production work order equipment
	Given an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "nj7", production work description: "routine"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And I am logged in as "user"
	When I visit the Show page for production work order: "pwo-01"
	Then I should see the "Equipment" tab
	When I click the "Equipment" tab
	And I press "AsLeftAsFoundEditButton"
	And I press "AsLeftAsFoundSaveButton"
	Then I should be at the Show page for production work order: "pwo-01" and fragment of "#EquipmentTab"

Scenario: user can add the as found and as left conditions to production work order equipment with as found condition reason and as left condition reason
	Given an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "nj7", production work description: "routine"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And I am logged in as "user"
	When I visit the Show page for production work order: "pwo-01"
	Then I should see the "Equipment" tab
	When I click the "Equipment" tab
	And I press "AsLeftAsFoundEditButton"
	And I select as found condition "one"'s Description from the AsFoundCondition dropdown
	And I select asset condition reason "one"'s Description from the AsFoundConditionReason dropdown
	And I select "Unable to Inspect" from the AsLeftCondition dropdown
	And I select asset condition reason "two"'s Description from the AsLeftConditionReason dropdown
	And I press "AsLeftAsFoundSaveButton"
	Then I should be at the Show page for production work order: "pwo-01" and fragment of "#EquipmentTab"
	
Scenario: user can add the as found and as left conditions to production work order equipment with as found condition comment and as left condition comment
	Given an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "nj7", production work description: "routine"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And I am logged in as "user"
	When I visit the Show page for production work order: "pwo-01"
	Then I should see the "Equipment" tab
	When I click the "Equipment" tab
	And I select as found condition "two"'s Description from the AsFoundCondition dropdown
	When I press "AsLeftAsFoundEditButton"
	And I press "AsLeftAsFoundSaveButton"
	Then I should see a validation message for AsFoundConditionComment with "The AsFoundConditionComment field is required."
	When I enter "abc" into the AsFoundConditionComment field
	And I select "Needs Re-Inspection" from the AsLeftCondition dropdown
	When I press "AsLeftAsFoundSaveButton"
	Then I should see a validation message for AsLeftConditionComment with "The AsLeftConditionComment field is required."
	When I enter "pqr" into the AsLeftConditionComment field
	And I press "AsLeftAsFoundSaveButton"
	Then I should be at the Show page for production work order: "pwo-01" and fragment of "#EquipmentTab"
	
Scenario: user can add the needs emergency repair as left conditions to production work order equipment and system will automatically create a corrective work order for this asset priority of emergency
	Given an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "nj7", production work description: "routine"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And I am logged in as "user"
	When I visit the Show page for production work order: "pwo-01"
	Then I should see the "Equipment" tab
	When I click the "Equipment" tab
	And I press "AsLeftAsFoundEditButton"
	And I select as found condition "one"'s Description from the AsFoundCondition dropdown
	And I select asset condition reason "one"'s Description from the AsFoundConditionReason dropdown
	And I select "Needs Emergency Repair" from the AsLeftCondition dropdown
	And I press "AsLeftAsFoundSaveButton"
	Then I should see a validation message for RepairComment with "The Repair Comments field is required."
	When I enter "test" into the RepairComment field
	And I press "AsLeftAsFoundSaveButton"
	And I wait for the page to reload
	Then I should be at the Show page for production work order: "pwo-01" and fragment of "#EquipmentTab"
	When I follow the link "9" inside the success message
	And I switch to the last browser tab
	Then I should be at the Production/ProductionWorkOrder/Show/9 page
	And I should see a display for AutoCreatedCorrectiveWorkOrder with "Yes"
	And I should see a display for OrderNotes with "test"
	And I should see a display for RequestedBy with user "user"'s FullName

Scenario: User can create a production work order with facility area
	Given I am logged in as "user"
	And production prerequisites exist
	And I am at the Production/ProductionWorkOrder/New page
	When I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select facility facility area "one"'s Display from the FacilityFacilityArea dropdown
	And I select production work order priority "high" from the Priority dropdown
	And I enter "some notes&lt;html<br>" into the OrderNotes field
	And I enter "some other problem notes&lt;html<br>" into the OtherProblemNotes field
	And I select employee "both-edit"'s Description from the RequestedBy dropdown
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	And I select production work description "two" from the ProductionWorkDescription dropdown
	And I enter "1" into the EstimatedCompletionHours field
	And I press Save
	And I wait for the page to reload
	Then I should see a link to the Show page for facility "one"
	
Scenario: User can search production work order using facility area
	Given a production work order "pwo-01" exists with operating center: "nj7", facility: "one", facility facility area: "one"
	And I am logged in as "user"
	And I am at the Production/ProductionWorkOrder/Search page
	When I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select facility facility area "one"'s Display from the FacilityFacilityArea dropdown
	And I press "Search"
	Then I should be at the Production/ProductionWorkOrder page
	And I should see "Records found: 1"
	And I should see a link to the Show page for production work order: "pwo-01"
	And I should see the following values in the WorkOrderSearchResultsTable table
	  | Id           | Facility Area |
	  | 8 | facilityLab |
   
Scenario: User can edit production work order facility area
	Given a production work order "pwo-01" exists with operating center: "nj7", facility: "one", facility facility area: "two"
	And I am logged in as "user"
	When I visit the Edit page for production work order: "pwo-01"
	When I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select facility facility area "one"'s Display from the FacilityFacilityArea dropdown
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	And I select production work description "two" from the ProductionWorkDescription dropdown
	And I press Save
	And I wait for the page to reload
	Then I should see a link to the Show page for facility "one"

Scenario: User can edit production work order Estimated Completion Hours
	Given a production work order "pwo-01" exists with operating center: "nj7", facility: "one", facility facility area: "one"
	And I am logged in as "user"
	When I visit the Edit page for production work order: "pwo-01"
	And I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select equipment "one"'s Description from the Equipment dropdown
	And I select production work description "two" from the ProductionWorkDescription dropdown
	And I enter "" into the EstimatedCompletionHours field
	And I press Save
	Then I should see a validation message for EstimatedCompletionHours with "The Estimated Completion Time (Hrs) field is required."
	When I enter "1.1111" into the EstimatedCompletionHours field
	Then I should see a validation message for EstimatedCompletionHours with "Must be a number with maximum of 2 decimal places"
	When I enter "1.23" into the EstimatedCompletionHours field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for EstimatedCompletionHours with "1.23"
	
Scenario: Production Work Orders with Renew/Rehab Production Work Description require Corrective Order Problem Code
	Given a production work order "pwo-01" exists with operating center: "nj7", facility: "one", facility facility area: "one", production work description: "rehab", planning plant: "one", functional location: "NJMPW-MP-NBCRS"
	And equipment: "rehab" exists in production work order: "pwo-01"
	And I am logged in as "user"
	When I visit the Edit page for production work order: "pwo-01"
	And I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select equipment type "rtu"'s Description from the EquipmentType dropdown
	And I select equipment "rehab"'s Description from the Equipment dropdown
	And I enter "1.23" into the EstimatedCompletionHours field
	And I press Save
	Then I should see a validation message for ProductionWorkDescription with "The Work Description field is required."
	When I select production work description "rehab"'s Description from the ProductionWorkDescription dropdown
	And I press Save
	Then I should see a validation message for CorrectiveOrderProblemCode with "The CorrectiveOrderProblemCode field is required."
	When I select corrective order problem code "two"'s Display from the CorrectiveOrderProblemCode dropdown
	And I press Save
	And I wait for the page to reload
	Then I should see a display for ProductionWorkDescription with "REHAB/RENEW"
	When I click the "Equipment" tab
	And I wait for the page to reload
	Then I should see a link to the Show page for equipment "rehab"
	When I click the "NJ7-1-ETTT-3" link in the 1st row of equipmentsTable
	And I wait for the page to reload
	And I follow the Edit link for equipment "rehab"
	And I enter "2061" into the PlannedReplacementYear field
	And I enter "500.50" into the EstimatedReplaceCost field
	And I press Save
	Then I should see a validation message for RequestedBy with "The RequestedBy field is required."
	When I select employee "both-edit"'s Description from the RequestedBy dropdown
	And I press Save
	And I wait for the page to reload
	Then I should see a display for PlannedReplacementYear with "2061"
	And I should see a display for EstimatedReplaceCost with "500.50"
	
Scenario: Completing a Renew/Rehab Production Work Order Will Update Asset Life Section In Equipment
	Given a production work order "pwo-01" exists with operating center: "nj7", facility: "one", facility facility area: "one", production work description: "rehab", planning plant: "one"
	And I am logged in as "adminuser"
	And an employee assignment "six" exists with production work order: "pwo-01", assigned to: "nj7-edit", assigned for: "1/1/2020", date started: "1/1/2020", date ended: "1/1/2023", hours worked: "8", assigned by: "both-edit"
	And equipment: "rehab" exists in production work order: "pwo-01"
	And a production work order cause code "one" exists
	And a production work order failure code "one" exists with description: "test failure"
	And a production work order action code "one" exists with description: "test action"
	And I am at the Show page for production work order: "pwo-01"
	When I click the "Finalization" tab
	And I select production work order failure code "one"'s Description from the FailureCode dropdown
	And I select production work order action code "one"'s Description from the ActionCode dropdown
	And I click ok in the dialog after pressing "Complete Order"
	Then I should see a display for ProductionWorkDescription with "REHAB/RENEW"
	When I click the "Equipment" tab
	Then I should see a link to the Show page for equipment "rehab"
	When I click the "NJ7-1-ETTT-3" link in the 1st row of equipmentsTable
	And I wait for the page to reload
	Then I should see a display for ServiceLife with "10"
	And I should see a display for RemainingUsefulLife with "11"
	And I should see a display for LifeExtendedOnDate with today's date
	And I should see a display for ExtendedRemainingUsefulLife with "16.5"

Scenario: Estimated Completion Hours is readonly when production work order is complete
	And a production work order "pwo-01" exists with productionWorkDescription: "three", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", order type: "pm work order", priority: "one", estimated completion hours: "1", local task description: "description five", date completed: "01/01/2021"
	And I am logged in as "user"
	When I visit the Edit page for production work order: "pwo-01"
	Then the EstimatedCompletionHours field should be readonly

Scenario: Estimated Completion Hours is not readonly when production work order is not complete
	Given a production work order "pwo-01" exists with operating center: "nj7", facility: "one"
	And I am logged in as "user"
	When I visit the Edit page for production work order: "pwo-01"
	Then the EstimatedCompletionHours field should not be readonly

Scenario: Users can view Actual Completion Hours in Production Work Order page
	Given a production work order "pwo-01" exists with operating center: "nj7", facility: "one"
	And an employee assignment "ea-1" exists with production work order: "pwo-01", assigned to: "both-edit", assigned on: "today", assigned for: "today", hours worked: "1.23"
	And an employee assignment "ea-2" exists with production work order: "pwo-01", assigned to: "nj7-edit", assigned on: "today", assigned for: "today", hours worked: "4.56"
	And I am logged in as "user"
	When I visit the Show page for production work order: "pwo-01"
	Then I should see a display for ActualCompletionHours with "5.79"
	
Scenario: Users can view Actual Completion Hours in Production Work Order Search Page
	Given a production work order "pwo-01" exists with operating center: "nj7", facility: "one"
	And an employee assignment "ea-1" exists with production work order: "pwo-01", assigned to: "both-edit", assigned on: "today", assigned for: "today", hours worked: "1.23"
	And an employee assignment "ea-2" exists with production work order: "pwo-01", assigned to: "nj7-edit", assigned on: "today", assigned for: "today", hours worked: "4.56"
	And I am logged in as "user"	
	And I am at the Production/ProductionWorkOrder/Search page
	When I enter production work order "pwo-01"'s Id into the EntityId field
	And I press "Search"
	Then I should see "Records found: 1"
	Then I should be at the Production/ProductionWorkOrder page
	And I should see the following values in the WorkOrderSearchResultsTable table
	  | Id | Actual Completion Time (Hrs) |
	  | 8  | 5.79                         |

Scenario: Users do not receive an error visiting a routine production work order not linked to a maintenance plan
	Given a production work order "pwo-mpnumber" exists with productionWorkDescription: "routine", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", order type: "routine", priority: "routine", estimated completion hours: "1", local task description: "pwo with mp description" 
	And I am logged in as "user"
	When I visit the show page for production work order "pwo-mpnumber"
	Then I should see a display for MaintenancePlan_Name with ""
	And I should not see the link "/Production/MaintenancePlan/Show/1"

Scenario: Completed production work orders linked to a maintenance plan days overdue field is based on the completion date
	Given a production work order "pwo-mpnumber" exists with productionWorkDescription: "maintenance plan", operating center: "nj4", planning plant: "two", facility: "two", maintenance plan: "one", due date: "1/1/2022", date completed: "1/3/2022"
	And I am logged in as "user"
	When I visit the show page for production work order "pwo-mpnumber"
	Then I should see a display for DaysOverdue with "2"

Scenario: Users can view Plan Number in Work Order tab
	Given a maintenance plan "two" exists with start: "02/24/2020", planning plant: "one", facilities: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", production work description: "one" 
	And a production work order "pwo-mpnumber" exists with productionWorkDescription: "routine", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", order type: "routine", priority: "routine", estimated completion hours: "1", local task description: "pwo with mp description", maintenance plan: "two" 
	And I am logged in as "user"
	When I visit the show page for production work order "pwo-mpnumber"
	Then I should see the link with href "/Production/MaintenancePlan/Show/2"
	
Scenario: Users can view task information on routine production work orders
	Given a skill set "one" exists with name: "Skill name", abbreviation: "SkAbr", is active: "true", description: "This is the description"    
	And a maintenance plan "two" exists with start: "02/24/2020", planning plant: "one", facilities: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", production work description: "one", resources: "1", estimated hours: "2", contractor cost: "3", skill set: "one"
	And a production work order "pwo-mpnumber" exists with productionWorkDescription: "routine", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", order type: "routine", priority: "routine", estimated completion hours: "1", local task description: "pwo with mp description", maintenance plan: "two" 
	And I am logged in as "user"
	When I visit the show page for production work order "pwo-mpnumber"
	Then I should see a display for EstimatedCompletionHours with "1"
	And I should see a display for MaintenancePlan_Resources with "1"
	And I should see a display for MaintenancePlan_SkillSet with "Skill name"

Scenario: Users cannot edit a routine production work order
	Given a production work order "pwo-mpnumber" exists with productionWorkDescription: "routine", operating center: "nj4", planning plant: "two", facility: "two", equipment: "two", order type: "pm work order", priority: "routine", estimated completion hours: "1", local task description: "pwo with mp description" 
	And I am logged in as "user"
	When I visit the show page for production work order "pwo-mpnumber"
	Then I should see a display for MaintenancePlan_Name with ""
	And I should not see the "edit" button in the action bar
	When I try to access the Edit page for production work order: "pwo-mpnumber"
	Then I should be at the Show page for production work order: "pwo-mpnumber"

Scenario: User can edit production work description if order type is not routine
	Given a production work order "pwo-01" exists with operating center: "nj7", planning plant: "one", facility: "one", facility facility area: "one", equipment type: "rtu"
	And a production work order "pwo-02" exists with operating center: "nj7", planning plant: "one", facility: "one", facility facility area: "one", equipment type: "rtu", production work description: "routine"
	And I am logged in as "user"
	When I visit the Edit page for production work order: "pwo-01"
	Then I should see the ProductionWorkDescription element
	And "-- Select --" should be selected in the ProductionWorkDescription dropdown
	When I try to access the Edit page for production work order: "pwo-02"
	Then I should be at the Show page for production work order: "pwo-02"
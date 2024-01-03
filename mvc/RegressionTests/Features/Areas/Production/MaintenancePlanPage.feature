Feature: MaintenancePlanPage
	In a world of uncontrolled, abject chaos
	Order can still be Maintained
	But to do it we'll need a Plan. 

Background:
	Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with abbreviation: "NJ"
	And an operating center "nj7" exists with state: "one", opcode: "NJ7"
	And an operating center "nj8" exists with state: "one", opcode: "NJ8"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
	And a facility "one" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility", planning plant: "one"
	And equipment types exist
    And equipment statuses exist
	And a facility area "one" exists with description: "The Zone"
	And a facility facility area "one" exists with facility: "one", facility area: "one"
	And an equipment category "one" exists with description: "Cat"
	And an equipment category "two" exists with description: "cat 2"
	And an equipment subcategory "one" exists with description: "SubCat"
	And an equipment subcategory "two" exists with description: "sub cat 2"
	And an equipment purpose "aerator" exists with description: "aerator", equipment type: "rtu", equipment category: "one", equipment subcategory: "one"
	And an equipment purpose "generator" exists with description: "generator", equipment type: "generator", equipment category: "two", equipment subcategory: "two"
	And an equipment lifespan "one" exists with description: "Flux Capacitor"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", equipment type: "rtu", description: "RTU1", s a p equipment id: 1, equipment purpose: "aerator"
	And order types exist
	And a production work description "maintenance plan" exists with order type: "routine", description: "Maintenance Plan"
	And a role "prodplannedread" exists with action: "Read", module: "ProductionPlannedWork"
	And a role "prodplannededit" exists with action: "Edit", module: "ProductionPlannedWork"
	And a role "prodplannedadd" exists with action: "Add", module: "ProductionPlannedWork"
	And a role "prodplanneddelete" exists with action: "Delete", module: "ProductionPlannedWork"
	And a role "prodequipmentread" exists with action: "Read", module: "ProductionEquipment" 
	And a role "prodfacilitiesread" exists with action: "Read", module: "ProductionFacilities"
    And a role "proddataadminread" exists with action: "Read", module: "ProductionDataAdministration"
	And a role "proddataadminadd" exists with action: "Add", module: "ProductionDataAdministration"
	And a role "proddataadminedit" exists with action: "Edit", module: "ProductionDataAdministration"
	And a role "productionworkorder-add-nj7" exists with action: "Add", module: "ProductionWorkManagement", operating center: "nj7"
	And a user "user" exists with username: "user", roles: prodplannedread;prodplannededit;prodplannedadd;prodplanneddelete;prodequipmentread;prodfacilitiesread;proddataadminread;proddataadminadd;proddataadminedit;productionworkorder-add-nj7
    And a maintenance plan task type "one" exists with description: "A good plan"
	And a task group category "one" exists with description: "This is the description", type: "chemical", abbreviation: "CHEM", is active: "true"
    And a task group category "two" exists with description: "This is the description two", type: "electrical", abbreviation: "ELEC", is active: "true"
	And a skill set "one" exists with name: "Skill name", abbreviation: "SkAbr", is active: "true", description: "This is the description"    
	And a task group "one" exists with task group id: "Task Id 1", task group name: "This is group 1", task details: "Task details 1", task details summary: "Task summary 1", task group category: "one", equipment types: "rtu", task group categories: "one", maintenance plan task type: "one"
	And a task group "two" exists with task group id: "Task Id 2", task group name: "This is group 2", task details: "Task details 2", task details summary: "Task summary 2", task group category: "two", equipment types: "rtu", task group categories: "one;two"
	And a task group "three" exists with task group id: "Task Id 3", task group name: "This is group 3", task details: "Task details 3", task details summary: "Task summary 3", task group category: "two", equipment types: "rtu", task group categories: "two"
	And production work order frequencies exist
	And a data type "data type" exists with table name: "MaintenancePlans"
	And a document type "document type" exists with data type: "data type", name: "Document"
	And a production work order priority "one" exists with description: "Routine - Off Scheduled"
	And a maintenance plan "one" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", local task description: "test local desc", resources: "1", estimated hours: "2", contractor cost: "3", skill set: "one"
	
Scenario: user can add a maintenance plan
	Given I am logged in as "user"
	When I visit the Production/MaintenancePlan/New page
	And I press "Save"
	Then I should see a validation message for State with "The State field is required."
	When I select state "one" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for PlanningPlant with "The District field is required."
	When I select planning plant "one" from the PlanningPlant dropdown
	And I press Save
	Then I should see a validation message for Facility with "The Facility field is required."	
	When I select facility "one"'s Description from the Facility dropdown
	And I press Save
	Then I should see a validation message for EquipmentTypes with "The EquipmentTypes field is required."
	When I select equipment type "rtu"'s Description from the EquipmentTypes dropdown
	And I press Save
	Then I should see a validation message for Equipment with "The Equipment field is required."
	When I select equipment "one"'s Description from the Equipment dropdown
	And I press Save
	Then I should see a validation message for TaskGroupCategory with "The TaskGroupCategory field is required."
	When I select task group category "one"'s Type from the TaskGroupCategory dropdown
	And I press Save
	Then I should see a validation message for TaskGroup with "The Task Group Name field is required."
	When I select task group "one"'s Caption from the TaskGroup dropdown
	And I press Save
	Then I should see a validation message for IsActive with "The IsActive field is required."
	When I select "Yes" from the IsActive dropdown
	And I press Save
	Then I should see a validation message for ProductionWorkOrderFrequency with "The Plan Frequency field is required."
	When I select production work order frequency "daily" from the ProductionWorkOrderFrequency dropdown
	And I press Save
	Then I should see a validation message for Start with "The Start field is required."
	When I enter "2/19/2020 01:01" into the Start field
	And I press Save
	Then I should see a validation message for HasACompletionRequirement with "The Auto Cancel Requirement field is required."
	When I select "Yes" from the HasACompletionRequirement dropdown
	And I press Save
	Then I should see a validation message for LocalTaskDescription with "The LocalTaskDescription field is required."
	When I enter "local task desc" into the LocalTaskDescription field
	And I click the "Equipment Information" tab
	And I select equipment purpose "aerator" from the EquipmentPurposes dropdown
	And I click the "Task Information" tab
	And I select skill set "one"'s Name from the SkillSet dropdown
	And I enter "45" into the Resources field 
	And I enter "50" into the EstimatedHours field 
	And I enter "65.20" into the ContractorCost field 
	And I press Save
	Then the currently shown maintenance plan shall henceforth be known throughout the land as "Carl"
	And I should be at the Show page for maintenance plan "Carl"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for PlanningPlant with "D217 - NJ7 - Argh"
	And I should see "Facility"
	And I should see "A Facility"
	When I click the "Task Information" tab
    Then I should see a display for TaskGroup with task group "one"
    And I should see a display for TaskGroupCategory with task group category "one"
	And I should see a display for PlanType with maintenance plan task type "one"
	And I should see a display for Resources with "45"
	And I should see a display for EstimatedHours with "50"
	And I should see a display for ContractorCost with "$65.20"
	And I should see a display for SkillSet with skill set "one"
	When I click the "Equipment Information" tab
	And I wait for the page to reload
	Then I should see the following values in the EquipmentPurposes table
         | Description	     | Abbreviation |
         | aerator			 | ETTT			|

Scenario: user can edit a maintenance plan
    Given I am logged in as "user"
    When I visit the Edit page for maintenance plan "one"
	And I click the "Task Information" tab
	And I select task group category "one"'s Type from the TaskGroupCategory dropdown
    And I select task group "one" from the TaskGroup dropdown
	And I click the "Plan Information" tab
    And I select "Yes" from the IsActive dropdown
    And I check the HasOshaRequirement field
	And I check the HasPsmRequirement field
    And I select production work order frequency "weekly" from the ProductionWorkOrderFrequency dropdown
	And I click the "Equipment Information" tab
	And I select equipment purpose "aerator" from the EquipmentPurposes dropdown
	And I click the "Task Information" tab
	And I select skill set "one"'s Name from the SkillSet dropdown
	And I enter "45" into the Resources field 
	And I enter "50" into the EstimatedHours field 
	And I enter "65.20" into the ContractorCost field 
	And I click ok in the alert after pressing Save
    And I wait for the page to reload
	And I click the "Task Information" tab
    Then I should see a display for TaskGroup with task group "one"
    And I should see a display for TaskGroupCategory with task group category "one"
	And I should see a display for PlanType with maintenance plan task type "one"
	And I should see a display for Resources with "45"
	And I should see a display for EstimatedHours with "50"
	And I should see a display for ContractorCost with "$65.20"
	And I should see a display for SkillSet with skill set "one"
    When I click the "Plan Information" tab
	Then I should see a display for IsActive with "Yes"
    And I should see a display for ProductionWorkOrderFrequency with production work order frequency "weekly"
	When I click the "Equipment Information" tab
	And I wait for the page to reload
	Then I should see the following values in the EquipmentPurposes table
         | Description	     | Abbreviation |
         | aerator			 | ETTT			|

Scenario: user adds a maintenance plan auto cancel sets to false and hides if any completion requirements are true
    Given I am logged in as "user"
    When I visit the Production/MaintenancePlan/New page
	And I select "Yes" from the HasACompletionRequirement dropdown
	Then I should see "Yes" in the HasACompletionRequirement dropdown
    When I check the HasCompanyRequirement field
	Then the HasACompletionRequirement field should not be visible
	When I check the HasCompanyRequirement field
	Then I should see "No" in the HasACompletionRequirement dropdown
	
Scenario: user edits a maintenance plan auto cancel sets to false and hides if any completion requirements are true
    Given I am logged in as "user"
	And a maintenance plan "auto" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", local task description: "test local desc", has company requirement: "false"
    When I visit the Edit page for maintenance plan "auto"
	And I click the "Plan Information" tab
	Then the HasACompletionRequirement field should be visible
	When I select "Yes" from the HasACompletionRequirement dropdown
    And I check the HasCompanyRequirement field
	Then the HasACompletionRequirement field should not be visible
	When I check the HasCompanyRequirement field
	Then I should see "No" in the HasACompletionRequirement dropdown
	
Scenario: user edits a maintenance plan cannot see auto cancel field if any completion requirements are true on initial edit page visit
    Given I am logged in as "user"
	And a maintenance plan "auto" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", local task description: "test local desc", has company requirement: "true"
    When I visit the Edit page for maintenance plan "auto"
	And I click the "Plan Information" tab
	Then the HasACompletionRequirement field should not be visible
	When I check the HasCompanyRequirement field
	Then I should see "No" in the HasACompletionRequirement dropdown

Scenario: user can search and view a maintenance plan
	Given I am logged in as "user"
	When I visit the Production/MaintenancePlan/Search page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press "Search"
	Then I should be at the Production/MaintenancePlan page
	And I should see a link to the Show page for maintenance plan "one"
	When I follow the Show link for maintenance plan "one"
	And I wait for the page to reload
	Then I should be at the Show page for maintenance plan "one"

Scenario: user can search and view a maintenance plan by Equipment Types Without Setting Facility
	Given I am logged in as "user"
	When I visit the Production/MaintenancePlan/Search page
	And I select state "one" from the State dropdown
	And I select equipment type "rtu"'s Description from the EquipmentTypes dropdown
	And I select equipment purpose "aerator" from the EquipmentPurposes dropdown
	And I press "Search"
	Then I should be at the Production/MaintenancePlan page
	And I should see a link to the Show page for maintenance plan "one"
	When I follow the Show link for maintenance plan "one"
	And I wait for the page to reload
	Then I should be at the Show page for maintenance plan "one"
	When I click the "Equipment Information" tab
	And I wait for the page to reload

Scenario: user can upload and view maintenance plan documents
    Given a document status "one" exists with description: "active"
	Given a production work order "one" exists with production work description: "maintenance plan", operating center: "nj7", planning plant: "one", facility: "one", equipment: "one", maintenance plan: "one"
	And I am logged in as "admin"
	When I visit the Show page for maintenance plan "one"
	And I click the "Documents" tab
	And I press "New Document"
    And I select "Document" from the #pnlNewDocument #DocumentType dropdown
	And I upload "..\..\mmsinc\MapCall.CommonTest\TestFile.bin"
	And I wait 2 second
	Then I should see "TestFile.bin" in the FileName field
	When I enter "asdf" into the FileName field
    And I press Save
    And I click the "Documents" tab
    Then I should see "asdf" in the table documentsTable's "File Name" column
    And I should see document type "document type"'s Name in the table documentsTable's "Document Type" column
	When I visit the Show page for production work order "one"
	And I click the "Maintenance Plan Documents" tab
	Then I should see "asdf" in the table maintenancePlanDocuments's "File Name" column
    And I should see document type "document type"'s Name in the table maintenancePlanDocuments's "Document Type" column

Scenario: user can add equipment to a maintenance plan
	Given an equipment "two" exists with identifier: "NJSB-1-EQID-2", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU2", s a p equipment id: 2
	And an equipment "three" exists with identifier: "NJSB-1-EQID-3", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU3", s a p equipment id: 3
	And an equipment "four" exists with identifier: "NJSB-1-EQID-4", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU4", s a p equipment id: 4
	And I am logged in as "user"
	When I visit the Show page for maintenance plan "one"
	And I click the "Equipment Information" tab
	And I press "Add Equipment"
	And I select equipment "two"'s Description from the Equipment dropdown
	And I select equipment "three"'s Description from the Equipment dropdown
	And I press "Add"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should see the following values in the EquipmentTable table
         | Equipment     |
         | RTU2 |
         | RTU3 |
	When I press "Add Equipment"
	And I press "Add All"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should see the following values in the EquipmentTable table
         | Equipment     |
         | RTU1 |
         | RTU2 |
         | RTU3 |
         | RTU4 |

Scenario: admin can add equipment to a maintenance plan
	Given an equipment "two" exists with identifier: "NJSB-1-EQID-2", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU2", s a p equipment id: 2
	And an equipment "three" exists with identifier: "NJSB-1-EQID-3", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU3", s a p equipment id: 3
	And an equipment "four" exists with identifier: "NJSB-1-EQID-4", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU4", s a p equipment id: 4
	And I am logged in as "admin"
	When I visit the Show page for maintenance plan "one"
	And I click the "Equipment Information" tab
	And I press "Add Equipment"
	And I select equipment "two"'s Description from the Equipment dropdown
	And I select equipment "three"'s Description from the Equipment dropdown
	And I press "Add"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should see the following values in the EquipmentTable table
         | Equipment     |
         | RTU2 |
         | RTU3 |
	When I press "Add Equipment"
	And I press "Add All"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should see the following values in the EquipmentTable table
         | Equipment     |
         | RTU1 |
         | RTU2 |
         | RTU3 |
         | RTU4 |

Scenario: user can remove equipment from a maintenance plan
	Given an equipment "two" exists with identifier: "NJSB-1-EQID-2", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU2", s a p equipment id: 2
	And an equipment "three" exists with identifier: "NJSB-1-EQID-3", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU3", s a p equipment id: 3
	And an equipment "four" exists with identifier: "NJSB-1-EQID-4", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU4", s a p equipment id: 4
	And I am logged in as "user"
	When I visit the Show page for maintenance plan "one"
	And I click the "Equipment Information" tab
	And I press "Add Equipment"
	And I press "Add All"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should see the following values in the EquipmentTable table
         | Equipment     |
         | RTU1 |
         | RTU2 |
         | RTU3 |
         | RTU4 |
	When I click the "Equipment" checkbox in the 1st row of EquipmentTable
	And I click ok in the dialog after pressing "Remove Selected"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should see the following values in the EquipmentTable table
         | Equipment     |
         | RTU2 |
         | RTU3 |
         | RTU4 |
	When I click ok in the dialog after pressing "Remove All"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should not see a link to the Show page for equipment "two"
	And I should not see a link to the Show page for equipment "three"
	And I should not see a link to the Show page for equipment "four"

Scenario: admin can remove equipment from a maintenance plan
	Given an equipment "two" exists with identifier: "NJSB-1-EQID-2", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU2", s a p equipment id: 2
	And an equipment "three" exists with identifier: "NJSB-1-EQID-3", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU3", s a p equipment id: 3
	And an equipment "four" exists with identifier: "NJSB-1-EQID-4", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU4", s a p equipment id: 4
	And I am logged in as "admin"
	When I visit the Show page for maintenance plan "one"
	And I click the "Equipment Information" tab
	And I press "Add Equipment"
	And I press "Add All"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should see the following values in the EquipmentTable table
         | Equipment     |
         | RTU1 |
         | RTU2 |
         | RTU3 |
         | RTU4 |
	When I click the "Equipment" checkbox in the 1st row of EquipmentTable
	And I click ok in the dialog after pressing "Remove Selected"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should see the following values in the EquipmentTable table
         | Equipment     |
         | RTU2 |
         | RTU3 |
         | RTU4 |
	When I click ok in the dialog after pressing "Remove All"
	And I wait for the page to reload
	And I click the "Equipment Information" tab
	Then I should not see a link to the Show page for equipment "two"
	And I should not see a link to the Show page for equipment "three"
	And I should not see a link to the Show page for equipment "four"
	
Scenario: User can create a production work order from a maintenance plan
	Given a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", production work description: "maintenance plan", is active: true, local task description: "What is your favorite color?"
	And equipment "one" exists in maintenance plan "two"
	And I am logged in as "user"
	When I visit the show page for maintenance plan "two"
	And I click the "Orders" tab
	And I press "Create Off-Scheduled Production Order"
	And I wait for the page to reload
	Then the currently shown production work order shall henceforth be known throughout the land as "FromAMaintenancePlan"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a link to the Show page for facility "one"
	And I should see a display for Priority with "Routine - Off Scheduled"
	And I should see a display for LocalTaskDescription with "What is your favorite color?"
	And I should see a display for ProductionWorkOrderFrequency with production work order frequency "daily"
	
Scenario: User should not be able create a production work order with tank inspections tab from a maintenance plan if task group is not T10
	Given a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "tnk-wpot", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", production work description: "maintenance plan", is active: true, local task description: "What is your favorite color?"
	And equipment "one" exists in maintenance plan "two"
	And I am logged in as "user"
	When I visit the show page for maintenance plan "two"
	And I click the "Orders" tab
	And I press "Create Off-Scheduled Production Order"
	And I wait for the page to reload
	Then the currently shown production work order shall henceforth be known throughout the land as "FromAMaintenancePlan"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a link to the Show page for facility "one"
	And I should see a display for Priority with "Routine - Off Scheduled"
	And I should see a display for LocalTaskDescription with "What is your favorite color?"
	And I should see a display for ProductionWorkOrderFrequency with production work order frequency "daily"
	Then I should not see the "Tank Inspections" tab

Scenario: User can create a production work order with tank inspections tab from a maintenance plan
	Given a task group "t10" exists with task group id: "T10", task group name: "This is group T10", task details: "Task details T10", task details summary: "Task summary T10", task group category: "one", equipment types: "tnk-wpot", task group categories: "one", maintenance plan task type: "one"
	And a maintenance plan "tnk" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "tnk-wpot", equipment purposes: "aerator", production work order frequency: "daily", equipment: "one", production work order priority: "one", task group: "t10", production work description: "maintenance plan", is active: true, local task description: "What is your favorite color?"	
	And equipment "one" exists in maintenance plan "tnk"
	And I am logged in as "user"
	When I visit the show page for maintenance plan "tnk"
	And I click the "Orders" tab
	And I press "Create Off-Scheduled Production Order"
	And I wait for the page to reload
	Then the currently shown production work order shall henceforth be known throughout the land as "FromAMaintenancePlan"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a link to the Show page for facility "one"
	And I should see a display for Priority with "Routine - Off Scheduled"
	And I should see a display for LocalTaskDescription with "What is your favorite color?"
	And I should see a display for ProductionWorkOrderFrequency with production work order frequency "daily"
	Then I should see the "Tank Inspections" tab

Scenario: User cannot create a production work order from an inactive maintenance plan
	Given a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", production work description: "maintenance plan", is active: false
	And equipment "one" exists in maintenance plan "two"
	And I am logged in as "user"
	When I visit the Show page for maintenance plan "two"
	And I click the "Orders" tab
	Then the CreateOffScheduledProductionOrder button should be disabled
	
Scenario: user can view the forecasted work orders of maintenance plan
	Given a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facilities: "one", equipment types: "rtu", equipment purposes: "aerator", task group category: "two", task group id: "one", production work order frequency: "daily", production work description: "maintenance plan", resources: "1.05", estimated hours: "23.75", contractor cost: "3", skill set: "one"
	And I am logged in as "user"
	When I visit the Show page for maintenance plan "two"
	And I click the "Scheduling Forecast" tab
	And I wait for the page to reload
	Then I should see the following values in the forecastWorkOrderTable table
         | Planned Date | Local Task Description       | Estimated Hours | Skill Set  | Resources |
         | today        | What is your favorite color? | 23.75           | Skill name | 1.05      |
	
Scenario: user can add and remove scheduled assignments from the forecast tab of a maintenance plan
	Given a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj8", planning plant: "one", facilities: "one", equipment types: "rtu", equipment purposes: "aerator", task group category: "two", task group id: "one", production work order frequency: "daily", production work description: "maintenance plan", resources: "1.05", estimated hours: "23.75", contractor cost: "3", skill set: "one"
	And an employee status "active" exists with description: "Active"
	And an employee "bob" exists with operating center: "nj7", status: "active", first name: "Test", last name: "Testerman"
	And an employee "carol" exists with operating center: "nj8", status: "active", first name: "Carol", last name: "Burnett"
	And I am logged in as "user"
	When I visit the Show page for maintenance plan "two"
	And I click the "Scheduling Forecast" tab
	And I wait for the page to reload
	And I press "Add Scheduled Assignments"
	Then I should not see employee "bob"'s Description in the AssignedTo dropdown
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select employee "bob"'s Description from the AssignedTo dropdown
	And I click the checkbox named ScheduledDates with the value "today's date"
	And I enter today's date into the AssignedFor field
	And I press Assign
	And I wait for the page to reload
	And I wait 1 second
	And I press "Add Scheduled Assignments"
	Then I should see the following values in the forecastAddAssignmentsTable table
         | Planned Date | Scheduled Assignments  | Local Task Description       | Estimated Hours | Skill Set  | Resources |
         | today        | *Test Testerman*       | What is your favorite color? | 23.75           | Skill name | 1.05      |
	When I press "Add Scheduled Assignments"
	And I press "Remove Scheduled Assignments"
	And I click the checkbox named SelectedAssignments with the value "1"
	And I click ok in the alert after pressing Remove
	And I wait for the page to reload
	And I click the "Scheduling Forecast" tab
	And I wait for the page to reload
	And I wait 1 second
	And I press "Remove Scheduled Assignments"
	Then the forecastRemoveAssignmentsTable table should be empty

Scenario: User can copy an existing maintenance plan for a new facility
	And a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", production work description: "maintenance plan", is active: true
	And I am logged in as "user"
	When I visit the show page for maintenance plan "two"
    Then I should see the button "Copy"
    When I click ok in the dialog after pressing "Copy"
	And I wait for the page to reload
	Then the currently shown maintenance plan shall henceforth be known throughout the land as "Carl"
	And I should be at the Edit page for maintenance plan "Carl"
	And I should see "Maintenance Plans : A Facility : This is group 1 : DAILY : Editing" in the bodyHeader element
	And I should see state "one" in the State dropdown
	And I should see operating center "nj7" in the OperatingCenter dropdown
	And I should see planning plant "one" in the PlanningPlant dropdown
	And I should see facility "one"'s Description in the Facility dropdown
	And I should see "Facility Areas"
	And I should see "The Zone - Things and junk and things and junk and stuff"

Scenario: User can view the number of assets associated with a maintenance plan on the index view
	Given I am logged in as "user"
	And an equipment "two" exists with identifier: "NJSB-1-EQID-2", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU2", s a p equipment id: 2
	And an equipment "three" exists with identifier: "NJSB-1-EQID-3", equipment status: "in service", facility: "one", equipment type: "rtu", description: "RTU3", s a p equipment id: 3
	And equipment "one" exists in maintenance plan "one"
	And a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", task group: "one", production work order frequency: "daily", equipment: "two", production work order priority: "one", local task description: "test local desc", equipment purposes: "aerator"
	And equipment "two" exists in maintenance plan "two"
	And equipment "three" exists in maintenance plan "two"
	When I visit the Production/MaintenancePlan/Search page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press "Search"
	Then I should be at the Production/MaintenancePlan page
	And I should see a link to the Show page for maintenance plan "two"
	And I should see the following values in the maintenancePlanTable table
         | # of Assets |
         | 1           |
         | 2           |

Scenario: user can search and view a maintenance plan for equipment purpose
	Given I am logged in as "user"
	When I visit the Production/MaintenancePlan/Search page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select equipment type "rtu"'s Description from the EquipmentTypes dropdown
	And I select equipment purpose "aerator" from the EquipmentPurposes dropdown
	And I press "Search"
	Then I should be at the Production/MaintenancePlan page
	And I should see a link to the Show page for maintenance plan "one"
	When I follow the Show link for maintenance plan "one"
	And I wait for the page to reload
	Then I should be at the Show page for maintenance plan "one"
	When I click the "Equipment Information" tab
	And I wait for the page to reload
	
Scenario: User can view orders tab initially sorted by date received descending
	Given a production work order "one" exists with date received: "1/1/2016", operating center: "nj7", planning plant: "one", facility: "one", equipment: "one", maintenance plan: "one"
	And a production work order "two" exists with date received: "2/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment: "one", maintenance plan: "one"
	And equipment "one" exists in maintenance plan "one"
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with operating center: "nj7", status: "active", employee id: "12345678", first name: "this", last name: "person"
	And a user "employee-one" exists with username: "employee-one", employee: "one"
	And an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "7/16/2020"
	And employee assignment: "one" exists in production work order: "one"
	And I am logged in as "user"
	When I visit the show page for maintenance plan "one"
	And I click the "Orders" tab
	Then I should see the following values in the productionWorkOrdersTable table
    | Id | Priority | Date Created | Date Completed | Local Task Description       | Task Description		 | Order Status        | Employees Assigned | Assigned On Date |
    | 2  | High     | *2/24/2020*  |                | What is your favorite color? | TTT : This is group 1   | Other               |                    |                  |
	| 1  | High     | *1/1/2016*   |                | What is your favorite color? | TTT : This is group 1   | ScheduledPreviously | this person        | 7/16/2020        |
	
Scenario: User can view orders tab with zero orders and create a production work order
	Given a maintenance plan "two" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", production work description: "maintenance plan", is active: true, local task description: "What is your favorite color?"
	And equipment "one" exists in maintenance plan "two"
	And I am logged in as "user"
	When I visit the show page for maintenance plan "two"
	And I click the "Orders" tab
	Then I should not see the table productionWorkOrdersTable
	When I press "Create Off-Scheduled Production Order"
	And I wait for the page to reload
	Then the currently shown production work order shall henceforth be known throughout the land as "FromAMaintenancePlan"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a link to the Show page for facility "one"
	And I should see a display for Priority with "Routine - Off Scheduled"
	And I should see a display for LocalTaskDescription with "What is your favorite color?"
	And I should see a display for ProductionWorkOrderFrequency with production work order frequency "daily"

Scenario: User edits maintenace plan sets to inactive which then requires a deactivation reason
	Given I am logged in as "user"
	And a maintenance plan "auto" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", local task description: "test local desc", has company requirement: "false"
    When I visit the Edit page for maintenance plan "auto"
	And I click the "Plan Information" tab
	And I select "No" from the IsActive dropdown
	And I press Save
	Then I should see a validation message for DeactivationReason with "The DeactivationReason field is required."	
	When I select "Yes" from the IsActive dropdown
	# The "i should not see a validation message for DeactivationReason" step seems to fail 
	# when this test is ran along side other tests, but is fine in isolation. If the browser
	# is visible, the validation message is not visible. So there's a weird timing issue happening
	# here.
	And I wait 1 second
	Then I should not see a validation message for DeactivationReason with "The DeactivationReason field is required."	
	
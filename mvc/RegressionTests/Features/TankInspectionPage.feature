Feature: tankinspections

Background:
	Given an employee status "active" exists with description: "Active"
	And an employee "one" exists with first name: "Bill", last name: "S. Preston", employee id: "1000001", status: "active"
	And an employee "two" exists with first name: "Johnny", last name: "Hotdog", employee id: "2000002", status: "active"
	And a user "user" exists with username: "user", employee: "one"
	And a user "other-user" exists with username: "other-user", employee: "two"
	And an admin user "admin" exists with username: "admin"
	And a state "nj" exists with abbreviation: "NJ"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "productionFacilities-useradmin" exists with action: "UserAdministrator", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And a role "ProductionEquipment-useradmin" exists with action: "UserAdministrator", module: "ProductionEquipment", user: "user", operating center: "nj7"
	And a role "productionFacilities-useradmin2" exists with action: "UserAdministrator", module: "ProductionFacilities", user: "other-user", operating center: "nj7"
	And a role "productionworkmanagement-useradmin" exists with action: "UserAdministrator", module: "ProductionWorkManagement", user: "user", operating center: "nj7"
	And a role "productionworkmanagement-useradmin2" exists with action: "UserAdministrator", module: "ProductionWorkManagement", user: "other-user", operating center: "nj7"
	And a role "productionFacilities-useradmin3" exists with action: "UserAdministrator", module: "ProductionPlannedWork", user: "user", operating center: "nj7"
	And production prerequisites exist
	And equipment types exist
	And equipment manufacturers exist
	And an equipment characteristic field type "string" exists with data type: "String"
    And an equipment characteristic field type "dropdown" exists with data type: "DropDown"
    And an equipment characteristic field type "currency" exists with data type: "Currency"
	And a production skill set "one" exists
	And an equipment model "one" exists with equipment manufacturer: "tnk", description: "tank"
	And order types exist
	And a coordinate "one" exists
	And a coordinate "two" exists with latitude: "35.04596702", longitude: "-90.14810180"
	And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg"
    And a street "one" exists with town: "nj7burg", full st name: "Easy St"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "PPF1"
	And a public water supply "one" exists
    And a public water supply pressure zone "one" exists
	And a facility "one" exists with operating center: "nj7", facility name: "NJ Facility", town: "nj7burg", planning plant: "one", address: "321 Streeeeeeet st", zip code: "12345", coordinate: "one", public water supply pressure zone: "one", public water supply: "one"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", equipment type: "tnk", operating center: "nj7", description: "TNK", s a p equipment id: "220", equipment manufacturer: "tnk", equipment model: "one"
	And an equipment "two" exists with identifier: "NJSB-1-EQID-1", facility: "one", equipment type: "tnk", operating center: "nj7", description: "TNK_CHEM", s a p equipment id: "221"
	And a production work description "one" exists with production skill set: "one", equipment type: "tnk", description: "one", order type: "corrective action"
	And production work order priorities exist
	And an equipment category "one" exists with description: "Cat"
	And an equipment subcategory "one" exists with description: "SubCat"
	And an equipment purpose "aerator" exists with description: "aerator", equipment type: "tnk-wpot", equipment category: "one", equipment subcategory: "one"
	And production work order frequencies exist
	And a maintenance plan task type "one" exists with description: "A good plan"
	And a task group category "one" exists with description: "This is the description", type: "chemical", abbreviation: "CHEM", is active: "true"
	And a task group "t10" exists with task group id: "T10", task group name: "This is group T10", task details: "Task details T10", task details summary: "Task summary T10", task group category: "one", equipment types: "tnk-wpot", task group categories: "one", maintenance plan task type: "one"
	And a maintenance plan "tnk" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "tnk-wpot", equipment purposes: "aerator", production work order frequency: "daily", equipment: "one", production work order priority: "one", task group: "t10", production work description: "one", is active: true, local task description: "What is your favorite color?"	
	And a production work order "one" exists with operating center: "nj7", planning plant: "one", facility: "one", equipment: "one", productionWorkDescription: "one", equipment type: "tnk", equipment is tank: "true", priority: "routine"
	And a production work order "two" exists with operating center: "nj7", planning plant: "one", facility: "one", equipment: "one", productionWorkDescription: "one", equipment type: "tnk", equipment is tank: "true", priority: "routine"
	And a production work order "three" exists with operating center: "nj7", planning plant: "one", facility: "one", equipment: "one", maintenance plan: "tnk", productionWorkDescription: "one", equipment type: "tnk-wpot", equipment is tank: "true", priority: "routine"
	And a tank inspection question group "one" exists with description: "Group 1"
	And a tank inspection question group "two" exists with description: "Group 2"
	And a tank inspection question group "three" exists with description: "Group 3"
	And a tank inspection question group "four" exists with description: "Group 4"
	And a tank inspection question group "five" exists with description: "Group 5"
	And a tank inspection question group "six" exists with description: "Group 6"
	And a tank inspection question type "one" exists with description: "Latches secured", tank inspection question group: "one"
	And a tank inspection question type "oneJr" exists with description: "Windows locked", tank inspection question group: "one"
	And a tank inspection question type "two" exists with description: "Steps secured", tank inspection question group: "two"
	And a tank inspection question type "three" exists with description: "ladder secured", tank inspection question group: "three"
	And a tank inspection question type "four" exists with description: "car secured", tank inspection question group: "four"
	And a tank inspection question type "five" exists with description: "barrel secured", tank inspection question group: "five"
	And a tank inspection question type "six" exists with description: "bucket secured", tank inspection question group: "six"
	And a tank inspection type "one" exists with description: "Routine"
	And a tank inspection type "two" exists with description: "Observation"
	And a tank inspection type "three" exists with description: "Not Routine"
	And a tank inspection type "four" exists with description: "Not Observation"
	And a tank inspection type "five" exists with description: "Drone"
	And a production work order production prerequisite "one" exists with production work order: "one", production prerequisite: "is confined space"
	And a tank inspection "one" exists with town: "nj7burg", tank inspection type: "one", coordinate: "one", tank observed by: "one", operating center: "nj7", facility: "one", Production work order: "one", Equipment: "one"
	And a tank inspection "five" exists with town: "nj7burg", tank inspection type: "one", coordinate: "one", tank observed by: "one", operating center: "nj7", facility: "one", Production work order: "two", Equipment: "one"
	And an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today", date started: "today"
	And an employee assignment "two" exists with production work order: "two", assigned to: "one", assigned for: "today"
	And an employee assignment "three" exists with production work order: "three", assigned to: "one", assigned for: "today"
    And a production pre job safety brief "one" exists with production work order: "one"
    And a production pre job safety brief worker exists with production pre job safety brief: "one", employee: "one"
	And a production pre job safety brief "two" exists with production work order: "two"
    And a production pre job safety brief worker exists with production pre job safety brief: "two", employee: "one"
	And a production pre job safety brief "three" exists with production work order: "three"
    And a production pre job safety brief worker exists with production pre job safety brief: "three", employee: "one"

	
Scenario: user can see a tank inspection from a production work order 
	Given I am logged in as "user"
	When I visit the Show page for production work order "one"
	And I click the "Tank Inspections" tab
	Then I should see the following values in the tankInspectionTable table
| Operating Center | Facility            | Equipment     |
| NJ7 - Shrewsbury | *NJ Facility - NJ7-* | *NJ7-1-ETTT-* |

Scenario: user can see a tank inspection from equipment 
	Given I am logged in as "user"
	When I visit the Show page for equipment "one"
	And I click the "Tank Inspections" tab
	Then I should see the following values in the tankInspectionTable table
| Operating Center | Facility            | Equipment     |
| NJ7 - Shrewsbury | *NJ Facility - NJ7-* | *NJ7-1-ETTT-* |

Scenario: User cannot create a tank inspection from a production work order until an employee is assigned
	Given I am logged in as "user"
	When I visit the Show page for production work order "two"
	And I click the "Tank Inspections" tab
	Then I should not see "New Tank Inspection"
	When I click the "Employee Assignments" tab
	And I click the "Start" button in the 1st row of employeeAssignmentsTable
	And I wait for the page to reload
	And I click the "Tank Inspections" tab
	And I follow "New Tank Inspection"

Scenario: employee assignment cannot end until tank inspection form is complete
	Given I am logged in as "user"
	When I visit the Show page for production work order "three"
	When I click the "Employee Assignments" tab
	And I click the "Start" button in the 1st row of employeeAssignmentsTable
	And I wait for the page to reload
	And I click the "Employee Assignments" tab
	Then I should not see the button "End"
	When I click the "Tank Inspections" tab
	And I follow "New Tank Inspection"
	When I enter coordinate "one"'s Id into the Coordinate field
	And I enter "1" into the TankCapacity field
	And I select tank inspection type "five" from the TankInspectionType dropdown
	And I enter "Comment 2" into the tankInspectionQuestions[0].ObservationAndComments field
	And I select "Yes" from the tankInspectionQuestions[0].RepairsNeeded dropdown
	And I select "Yes" from the tankInspectionQuestions[0].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[1].RepairsNeeded dropdown
	And I select "No" from the tankInspectionQuestions[1].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[2].RepairsNeeded dropdown
	And I select "No" from the tankInspectionQuestions[2].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[3].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[3].RepairsNeeded dropdown
	And I select "No" from the tankInspectionQuestions[4].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[4].RepairsNeeded dropdown
	And I select "No" from the tankInspectionQuestions[5].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[5].RepairsNeeded dropdown
	And I select "No" from the tankInspectionQuestions[6].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[6].RepairsNeeded dropdown
	And I press Save
	And I wait for the page to reload
	Then I should see a display for TankCapacity with "1"
	Given I am logged in as "admin"
	When I visit the Show page for production work order "three"
	When I click the "Employee Assignments" tab
	Then I should see the button "End"

Scenario: User can create a tank inspection from a production work order and receive validation messages
	Given I am logged in as "user"
	When I visit the Show page for production work order "one"
	And I click the "Tank Inspections" tab
	And I follow "New Tank Inspection"
	When I enter coordinate "one"'s Id into the Coordinate field
	And I enter "1" into the TankCapacity field
	And I select tank inspection type "five" from the TankInspectionType dropdown
	And I press Save
	And I enter "Comment 2" into the tankInspectionQuestions[0].ObservationAndComments field
	And I select "Yes" from the tankInspectionQuestions[0].RepairsNeeded dropdown
	And I select "Yes" from the tankInspectionQuestions[0].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[1].RepairsNeeded dropdown
	And I select "No" from the tankInspectionQuestions[1].TankInspectionAnswer dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[2].RepairsNeeded dropdown
	And I select "No" from the tankInspectionQuestions[2].TankInspectionAnswer dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[3].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[3].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[4].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[4].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[5].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[5].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[6].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[6].RepairsNeeded dropdown
	And I press Save
	And I wait for the page to reload
	Then I should see a display for TankCapacity with "1"
	And I should see a display for TankObservedBy with employee "one"
	And I should see a display for Town with town "nj7burg"
	Given I am logged in as "Admin"
	When I visit the Show page for production work order "one"
	And I click the "Tank Inspections" tab 
	Then I should see the following values in the tankInspectionTable table
| Operating Center | Facility            | Equipment     | Equipment Description |
| NJ7 - Shrewsbury | NJ Facility - NJ7-1 | *NJ7-1-ETTT-* | TNK                   |
| NJ7 - Shrewsbury | NJ Facility - NJ7-1 |               |                       |

Scenario: User can edit Inspection Questions And Changing Coordinate will change the coordinate value on the Facility Page
	Given a tank inspection "two" exists with town: "nj7burg", tank inspection type: "one", coordinate: "one", tank observed by: "one", operating center: "nj7", facility: "one", Production work order: "one", Equipment: "one"
	And I am logged in as "user"
	When I visit the Edit page for tank inspection "two"
	And I enter "0.04" into the TankCapacity field
	And I enter coordinate "two"'s Id into the Coordinate field
	And I select tank inspection type "five" from the TankInspectionType dropdown
	And I click the "Security & Vandalism" tab
	And I enter "Comment1" into the tankInspectionQuestions[0].ObservationAndComments field
	And I select "Yes" from the tankInspectionQuestions[0].RepairsNeeded dropdown 
	And I select "Yes" from the tankInspectionQuestions[0].TankInspectionAnswer dropdown
	And I select "Yes" from the tankInspectionQuestions[1].RepairsNeeded dropdown 
	And I select "Yes" from the tankInspectionQuestions[1].TankInspectionAnswer dropdown
	And I press Save
	Then I should see a validation message for tankInspectionQuestions[1].ObservationAndComments with "The ObservationAndComments field is required."
	When I enter "words" into the tankInspectionQuestions[1].ObservationAndComments field
	And I press Save
	And I select "No" from the tankInspectionQuestions[2].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[2].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[3].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[3].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[4].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[4].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[5].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[5].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[6].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[6].RepairsNeeded dropdown
	And I press Save
	Then I should see a display for TankCapacity with "0.04"
	When I click the "Security & Vandalism" tab
	Then I should see "Latches secured"
	Then I should see "Observations and Comments: Comment1"
	Then I should see "Repairs Needed: True"
	Then I should see "Windows locked"
	Then I should see "Observations and Comments: words"
	Then I should see "Repairs Needed: True"
	When I visit the Show page for facility "one"
	Then I should see a display for Coordinate with "35.04596702, -90.1481018"
	
Scenario: User can remove an inspection question
	Given a tank inspection "two" exists with town: "nj7burg", tank inspection type: "one", coordinate: "one", tank observed by: "one", operating center: "nj7", facility: "one", Production work order: "one", Equipment: "one"
	And I am logged in as "user"
	When I visit the Edit page for tank inspection "two"
	And I enter "3.05" into the TankCapacity field
	And I select tank inspection type "five" from the TankInspectionType dropdown
	When I click the "Security & Vandalism" tab
	And I select "Yes" from the tankInspectionQuestions[0].TankInspectionAnswer dropdown
	And I select "Yes" from the tankInspectionQuestions[0].RepairsNeeded dropdown
	And I enter "Comment" into the tankInspectionQuestions[0].ObservationAndComments field
	And I select "No" from the tankInspectionQuestions[1].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[1].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[2].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[2].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[3].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[3].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[4].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[4].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[5].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[5].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[6].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[6].RepairsNeeded dropdown
	And I press Save
	Then I should see a display for TankCapacity with "3.05"
	When I click the "Security & Vandalism" tab
	Then I should see "Latches secured"
	Then I should see "Observations and Comments: Comment"
	Then I should see "Repairs Needed: True"   
	When I visit the Edit page for tank inspection "two"
	And I click the "Security & Vandalism" tab
	And I select "No" from the tankInspectionQuestions[0].TankInspectionAnswer dropdown
	And I press Save
	When I click the "Security & Vandalism" tab
	Then I should see "Latches secured"
	Then I should see "Observations and Comments: "
	Then I should see "Repairs Needed: "  
	
Scenario: Tank Inspection question tabs 1 through 5 should populate questions even if not entered
	Given a tank inspection "seven" exists with town: "nj7burg", tank inspection type: "one", coordinate: "one", tank observed by: "one", operating center: "nj7", facility: "one", Production work order: "one", Equipment: "one"
	And I am logged in as "user"
	When I visit the Edit page for tank inspection "seven"
	And I enter "3.05" into the TankCapacity field
	When I click the "Security & Vandalism" tab
	And I select "Yes" from the tankInspectionQuestions[0].TankInspectionAnswer dropdown
	And I select "Yes" from the tankInspectionQuestions[0].RepairsNeeded dropdown
	And I enter "Comment" into the tankInspectionQuestions[0].ObservationAndComments field
	And I select "No" from the tankInspectionQuestions[1].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[1].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[2].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[2].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[3].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[3].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[4].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[4].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[5].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[5].RepairsNeeded dropdown
	And I press Save
	And I select "No" from the tankInspectionQuestions[6].TankInspectionAnswer dropdown
	And I select "No" from the tankInspectionQuestions[6].RepairsNeeded dropdown
	And I press Save
	When I click the "Security & Vandalism" tab
	Then I should see "Latches secured"
	When I click the "General Site Info" tab
	Then I should see "Steps secured"
	When I click the "Foundation" tab
	Then I should see "ladder secured"
	When I click the "Exterior Condition" tab
	Then I should see "car secured"
	When I click the "Exterior Roof" tab
	Then I should see "barrel secured"
	When I click the "Overflow Piping, Vaults, Accessory Bldgs. & Valves" tab
	Then I should see "bucket secured"
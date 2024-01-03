Feature: ValvePage

Background: data exists
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", is contracted operations: "false", sap enabled: "true", uses valve inspection frequency: "true", arc mobile map id: "01d4ebf78acc489695b930d5bc2850f3", largeValveInspFreq: "4", smallValveInspFreq: "2"
    And a user "user" exists with username: "user"
	And a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "sewer opening" exists with description: "sewer opening"
	And a valve control "flushing" exists with description: "BLOW OFF WITH FLUSHING"
	And a valve control "one" exists with description: "Foo"
	And a asset status "pending" exists with description: "PENDING"
	And a asset status "active" exists with description: "ACTIVE", is user admin only: "true"
	And a asset status "removed" exists with description: "REMOVED", is user admin only: "true"
	And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
	And a asset status "other adminonly" exists with description: "INACTIVE", is user admin only: "true"
	And a asset status "other" exists with description: "REQUEST RETIREMENT", is user admin only: "false"
	And a fire district abbreviation type "fire district" exists
	And a state "one" exists with abbreviation: "NJ"
	And a state "two" exists with name: "Pennsylvania", abbreviation: "PA"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
    And a town section "active" exists with town: "one"
    And a town section "inactive" exists with town: "one", active: false
	And a street "one" exists with town: "one", is active: true
	And a street "two" exists with town: "one", full st name: "Easy St", is active: true
	And a valve zone "one" exists
	And a valve zone "two" exists
	And a valve billing "municipal" exists with description: "Municipal"
	And a valve billing "oandm" exists with description: "O & M"
	And a valve billing "public" exists with description: "PUBLIC"
	And a valve billing "company" exists with description: "COMPANY"
    And a valve "one" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve billing: "public", status: "active", valve controls: "one", operating center: "nj7"
    And a valve "three" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve billing: "public", status: "active", valve controls: "one"
	And a coordinate "one" exists
	And a valve type "one" exists with description: "one"
	And a valve normal position "one" exists with description: "one"
	And a functional location "one" exists with town: "one", asset type: "valve"

Scenario: user can search for a valve
    Given I am logged in as "user"
    When I visit the FieldOperations/Valve/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should see a link to the Show page for valve: "one"
    When I follow the Show link for valve "one"
    Then I should be at the Show page for valve: "one"

Scenario: user can view a valve
    Given I am logged in as "user"
    When I visit the Show page for valve: "one"
    Then I should see a display for valve: "one"'s ValveNumber
	And I should see a display for valve: "one"'s Id

Scenario: user can add a valve with a main crossing
	Given a facility "one" exists with facility id: "NJSB-01", town: "one", operating center: "nj7", facility name: "argh"
	And a main crossing "one" exists with length of segment: "100.01", stream: "one", operating center: "nj7", town: "one", street: "one", closest cross street: "two"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And a valve size "one" exists
	And I am logged in as "user"
	When I visit the /FieldOperations/Valve/New page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I select asset status "other adminonly" from the Status dropdown
	And I select street "one"'s FullStName from the Street dropdown
	And I select valve zone "one" from the ValveZone dropdown
	And I enter "some critical notes" into the CriticalNotes field
	And I select valve billing "public" from the ValveBilling dropdown
	And I select facility "one" from the Facility dropdown
	And I select valve control "one" from the ValveControls dropdown
	And I select valve size "one" from the ValveSize dropdown
	And I enter "54321" into the WorkOrderNumber field
	And I select functional location "one" from the FunctionalLocation dropdown
	And I check the ControlsCrossing checkbox
	And I wait for ajax to finish loading
	And I select main crossing "one"'s Description from the MainCrossings multiselect
	When I press Save
	Then the currently shown valve shall henceforth be known throughout the land as "artemis"
	And I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
	And I should see a display for Town with town "one"
	When I click the "Main Crossings" tab
	Then I should see a link to the show page for main crossing "one"

Scenario: user can add a valve
	Given a facility "one" exists with facility id: "NJSB-01", town: "one", operating center: "nj7", facility name: "argh"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And a valve size "one" exists
    And I am logged in as "user"
	When I visit the /FieldOperations/Valve/New page
	And I select state "one" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for Status with "The Status field is required."
	And I should see a validation message for WorkOrderNumber with "The WBS # field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I check the Critical field
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."
	When I select town "one" from the Town dropdown
	And I press Save
	Then I should see a validation message for Street with "The Street Name field is required."
	When I select asset status "active" from the Status dropdown
	Then I should not see the DateRetired field
	When I select asset status "retired" from the Status dropdown
	Then I should see the DateRetired field
	When I select asset status "active" from the Status dropdown
	And I press Save
	Then I should see a validation message for Coordinate with "Coordinate is required for active valves."
	And I should see a validation message for ValveBilling with "The Billing Information field is required."
	And I should see a validation message for ValveZone with "The ValveZone field is required."
	And I should see a validation message for ValveControls with "The ValveControls field is required."
	And I should see a validation message for CriticalNotes with "The CriticalNotes field is required."
	When I select asset status "other adminonly" from the Status dropdown
	And I select street "one"'s FullStName from the Street dropdown
	And I select valve zone "one" from the ValveZone dropdown
	And I enter "some critical notes" into the CriticalNotes field
	And I select valve billing "public" from the ValveBilling dropdown
	And I select facility "one" from the Facility dropdown
	And I select valve control "one" from the ValveControls dropdown
	And I press Save
	Then I should see a validation message for FunctionalLocation with "The Functional Location field is required."
	And I should see a validation message for ValveSize with "The Valve Size (in) field is required."
	When I select functional location "one" from the FunctionalLocation dropdown
	And I select valve size "one" from the ValveSize dropdown
	And I enter "54321" into the WorkOrderNumber field
	And I press "Save"
	Then the currently shown valve shall henceforth be known throughout the land as "foo"
	And I should see a display for Critical with "Yes"
	And I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
	And I should see a display for Town with town "one"
	And I should see a display for CriticalNotes with "some critical notes"
	And I should see a display for Street with street "one"'s FullStName
	And I should see a display for ValveZone with valve zone "one"'s Description
	And I should see a display for ValveNumber with "VLA-4"
	And I should see a display for CreatedAt with today's date
	And I should see a display for Facility with facility "one"'s Description
	And I should see a display for WorkOrderNumber with "54321"
	And I should see a display for InspectionFrequency with "2"
	And I should see a display for InspectionFrequencyUnit with "Year"
	When I click the "Work Orders" tab
	Then I should not see "Create New Work Order"
	And I should see "This Asset is Currently Inactive."

Scenario: user cannot create a valve for an inactive town section
    Given I am logged in as "user"
	When I visit the /FieldOperations/Valve/New page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
	And I select town "one" from the Town dropdown
    And I wait for ajax to finish loading
    Then I should not see town section "inactive" in the TownSection dropdown

Scenario: user can add a found valve
    Given I am logged in as "user"
	And a valve size "one" exists
    When I visit the /FieldOperations/Valve/New page
	And I select state "one" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I check the Critical field
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."
	When I select town "one" from the Town dropdown
	And I press Save
	Then I should see a validation message for Street with "The Street Name field is required."
	And I should see a validation message for Status with "The Status field is required."
	When I select asset status "active" from the Status dropdown
	And I press Save
	Then I should see a validation message for Coordinate with "Coordinate is required for active valves."
	And I should see a validation message for ValveZone with "The ValveZone field is required."
	And I should see a validation message for CriticalNotes with "The CriticalNotes field is required."
	And I should see a validation message for ValveBilling with "The Billing Information field is required."
	And I should see a validation message for ValveControls with "The ValveControls field is required."
	When I select asset status "active" from the Status dropdown
	And I select valve control "one" from the ValveControls dropdown
	And I check the IsFoundValve field
	And I select street "one"'s FullStName from the Street dropdown
	And I select valve zone "one" from the ValveZone dropdown
	And I enter "some critical notes" into the CriticalNotes field
	And I enter "2" into the ValveSuffix field
	And I enter coordinate "one"'s Id into the Coordinate field
	And I select valve billing "public" from the ValveBilling dropdown
	And I select functional location "one" from the FunctionalLocation dropdown
	And I select valve size "one" from the ValveSize dropdown
	And I press Save
	Then I should see a validation message for DateInstalled with "The DateInstalled field is required."
	And I should see a validation message for NormalPosition with "Normal Position is required for active / installed valves."
	And I should see a validation message for ValveType with "Valve Type is required for active / installed valves."
	When I enter today's date into the DateInstalled field
	And I enter "3214" into the WorkOrderNumber field
	And I select valve type "one" from the ValveType dropdown
	And I select valve normal position "one" from the NormalPosition dropdown
	And I enter "4" into the Turns field
	And I press Save
	And I wait for the page to reload
	Then the currently shown valve shall henceforth be known throughout the land as "two"
	And I should see a display for Critical with "Yes"
	And I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
	And I should see a display for Town with town "one"
	And I should see a display for CriticalNotes with "some critical notes"
	And I should see a display for Street with street "one"'s FullStName
	And I should see a display for DateInstalled with today's date
	And I should see a display for ValveZone with valve zone "one"'s Description
	And I should see a display for ValveNumber with "VLA-2"
	And I should see a display for WorkOrderNumber with "3214"

Scenario: user receives proper validation and can edit the required fields for a valve
	Given a facility "two" exists with facility id: "NJSB-02", town: "one", operating center: "nj7"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
    And I am logged in as "user"
    When I visit the Edit page for valve: "one"
	And I select "-- Select --" from the Street dropdown
	And I select "-- Select --" from the ValveZone dropdown
	And I check the Critical field
	And I select "-- Select --" from the Status dropdown
	And I select "-- Select --" from the ValveBilling dropdown
	And I press "Save"
	Then I should see a validation message for Street with "The Street Name field is required."
	And I should see a validation message for ValveZone with "The ValveZone field is required."
	And I should see a validation message for CriticalNotes with "The CriticalNotes field is required."
	And I should see a validation message for Status with "The Status field is required."
	And I should see a validation message for ValveBilling with "The Billing Information field is required."
	When I select asset status "active" from the Status dropdown
	And I select valve billing "public" from the ValveBilling dropdown
	And I enter "" into the Coordinate field
	And I press "Save"
	Then I should see a validation message for Coordinate with "Coordinate is required for active valves."
	And I should not see a validation message for DateRetired with "DateRetired is required for retired / removed valves."
	When I enter "bar" into the ValveNumber field
	And I select asset status "retired" from the Status dropdown
	And I select street "two"'s FullStName from the Street dropdown
	And I select valve zone "two" from the ValveZone dropdown
	And I enter "some critical notes" into the CriticalNotes field
	And I select valve billing "public" from the ValveBilling dropdown
	And I select facility "two" from the Facility dropdown
	And I select valve control "one" from the ValveControls dropdown
	And I select functional location "one" from the FunctionalLocation dropdown
	And I press "Save"
	Then I should see a validation message for DateRetired with "DateRetired is required for retired / removed valves."
	When I enter today's date into the DateRetired field
	And I press "Save"
	Then I should see a validation message for ValveNumber with "Valve Number must contain the Valve Suffix."
	When I enter "VAR-1" into the ValveNumber field
	And I press "Save"
	Then I should see a display for Critical with "Yes"
	And I should see a display for CriticalNotes with "some critical notes"
	And I should see a display for Street with street "two"'s FullStName
	And I should see a display for ValveZone with valve zone "two"'s Description
    And I should see a display for ValveNumber with "VAR-1"
	And I should see a display for Facility with facility "two"'s Description
	And I should see a display for DateRetired with today's date
	When I follow "Edit"
	And I uncheck the Critical field
	# Validation doesn't run when CriticalNotes is disabled. Also value isn't posted back.
	Then the CriticalNotes field should be disabled

Scenario: user receives proper validation messages for new valves
	Given I am logged in as "user"
	When I visit the /FieldOperations/Valve/New page
	And I select state "one" from the State dropdown
	And I check the Critical field
	And I press Save
	Then I should see a validation message for ValveZone with "The ValveZone field is required."
	And I should see a validation message for CriticalNotes with "The CriticalNotes field is required."
	And I should see a validation message for Status with "The Status field is required."
	And I should see a validation message for ValveBilling with "The Billing Information field is required."
	When I select asset status "active" from the Status dropdown
	And I select valve control "one" from the ValveControls dropdown
	And I press Save
	Then I should see a validation message for Coordinate with "Coordinate is required for active valves."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."
	When I select town "one" from the Town dropdown
	And I press Save
	Then I should see a validation message for Street with "The Street Name field is required."
	When I uncheck the Critical field
	And I select street "one"'s FullStName from the Street dropdown
	And I select valve zone "one" from the ValveZone dropdown
	And I select asset status "retired" from the Status dropdown
	And I enter "some critical notes" into the CriticalNotes field
	And I select valve billing "public" from the ValveBilling dropdown
	And I select functional location "one" from the FunctionalLocation dropdown
	#And I press Save
	Then the CriticalNotes field should be disabled

Scenario Outline: When a user is adding or editing valves, the date retired field visibility and validation messages toggle given the asset status.
	Given I am logged in as "user"
	And I am at the FieldOperations/Valve/New page
	When I select asset status <status> from the Status dropdown
	Then I <should_see_field> see the DateRetired field
	When I press Save
	Then I <should_see_validation_message> see a validation message for DateRetired with "DateRetired is required for retired / removed valves."
	# Edit has the same When/Then expectations
	Given a valve "two" exists with operating center: "nj7", town: "one", street: "one", status: "active"
	And I am logged in as "user"
	When I visit the edit page for valve: "two" 
	And I select asset status <status> from the Status dropdown
	Then I <should_see_field> see the DateRetired field
	When I press Save
	Then I <should_see_validation_message> see a validation message for DateRetired with "DateRetired is required for retired / removed valves."

	Examples:
	| status               | should_see_field | should_see_validation_message |
	| "active"             | should not       | should not                    |
	| "removed"            | should           | should                        |
	| "retired"            | should           | should                        |

Scenario Outline: When a user is adding or editing valves, the validation messages toggle given the asset status.
	Given a valve "two" exists with operating center: "nj7", town: "one", street: "one", status: "active"
	And I am logged in as "user"
	And I am at the FieldOperations/Valve/New page
	When I select asset status <status> from the Status dropdown
	When I press Save
	Then I <should_see_validation_message> see a validation message for NormalPosition with "Normal Position is required for active / installed valves."
	Then I <should_see_validation_message> see a validation message for ValveType with "Valve Type is required for active / installed valves."
	# Edit has the same When/Then expectations
	When I visit the edit page for valve: "two" 
	And I select asset status <status> from the Status dropdown
	When I press Save
	Then I <should_see_validation_message> see a validation message for NormalPosition with "Normal Position is required for active / installed valves."
	Then I <should_see_validation_message> see a validation message for ValveType with "Valve Type is required for active / installed valves."
	
	Examples:
	| status      | should_see_validation_message |
	| "active"    | should                        |
	| "removed"   | should not                    |
	| "retired"   | should not                    |
	
Scenario: An edit role user can not edit certain fields that a useradmin role user can edit 
	Given a valve "two" exists with operating center: "nj7", town: "one", street: "one", status: "active"
    And I am logged in as "user"
	And a user "edituser" exists with username: "edituser"
	And a role "valve-edit" exists with action: "Edit", module: "FieldServicesAssets", user: "edituser", operating center: "nj7"
	When I log in as "user"
	And I visit the edit page for valve: "two" 
	Then I should see the BPUKPI field
	And I should see the ValveBilling field
	And I should see the ValveNumber field
	And I should see the ValveSuffix field
	And I should see the FunctionalLocation field
	And I should see asset status "active" in the Status dropdown
	And I should see asset status "retired" in the Status dropdown
	And I should see asset status "other" in the Status dropdown
	And I should see asset status "other adminonly" in the Status dropdown
	When I log in as "edituser"
	And I visit the edit page for valve: "two" 
	Then I should not see the BPUKPI field
	And I should not see the ValveBilling field
	And I should not see the ValveNumber field
	And I should not see the ValveSuffix field
	And I should not see the FunctionalLocation field
	# active is user-admin only, but it should be selectable still because it's the current value on the valve.
	And I should see asset status "active" in the Status dropdown
	# retired and other adminonly should not display at all
	And I should not see asset status "retired" in the Status dropdown
	And I should not see asset status "other adminonly" in the Status dropdown
	# other should be displayed because it's not limited to admins
	And I should see asset status "other" in the Status dropdown

Scenario: User should see some extra validation for FunctionalLocation
	Given an operating center "sap enabled" exists with opcode: "NJ7-sapenabled", name: "Shrewsbury", is contracted operations: "false", sap enabled: "true"
	And a town "la" exists with name: "Loch Arbour"
	And operating center: "sap enabled" exists in town: "la" with abbreviation: "LA"
	And a functional location "la" exists with town: "la", asset type: "valve"
	And an operating center "sap enabled contracted" exists with opcode: "NJ7-contracted", name: "Shrewsbury", is contracted operations: "true", sap enabled: "true"
	And a town "ah" exists with name: "Loch Arbour"
	And operating center: "sap enabled contracted" exists in town: "ah" with abbreviation: "LA"
	And a functional location "ah" exists with town: "ah", asset type: "valve"
	And an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
	And I am at the FieldOperations/Valve/New page
	When I select state "one" from the State dropdown
	And I press Save
	Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."
	When I select operating center "sap enabled" from the OperatingCenter dropdown
	And I select town "la" from the Town dropdown
	And I select valve control "one" from the ValveControls dropdown
	And I press Save
	Then I should see a validation message for FunctionalLocation with "The Functional Location field is required."
	When I select operating center "sap enabled contracted" from the OperatingCenter dropdown
	And I select town "ah" from the Town dropdown
	And I press Save
	Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."
	# Edit is done differently so it needs to be tested separately.
	Given a valve "notcontracted" exists with operating center: "sap enabled", town: "la"
	And I am at the edit page for valve: "notcontracted"
	When I select "-- Select --" from the FunctionalLocation dropdown
	And I press Save
	Then I should see a validation message for FunctionalLocation with "The Functional Location field is required."
	Given a valve "two" exists with operating center: "sap enabled contracted", town: "ah"
	And I am at the edit page for valve: "two"
	When I select "-- Select --" from the FunctionalLocation dropdown
	And I press Save
	Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."

Scenario: User can copy a valve
	Given a recurring frequency unit "year" exists with description: "Year"
	And a valve size "one" exists
    And a valve normal position "open" exists
    And a valve open direction "meh" exists
    And a town section "meh" exists with town: "one"
    And a main type "meh" exists		
	And a gradient "meh" exists with description: "blah"
    And a valve manufacturer "meh" exists
    And a valve type "meh" exists
    And a valve size "meh" exists
    And a water system "meh" exists
	And water system "meh" exists in operating center: "nj7"
	And a facility "one" exists with facility id: "NJSB-01", town: "one", operating center: "nj7", facility name: "argh"
    And a valve "two" exists with valve number: "VLA-2", b p u k p i: true, inspection frequency unit: "year", inspection frequency: 2, sketch number: "12345", street number: "123", street: "two", town: "one", work order number: "54321", valve billing: "municipal", gradient: "meh", cross street: "one", normal position: "open", operating center: "nj7", open direction: "meh", town section: "meh", main type: "meh", valve controls: "one", valve make: "meh", valve type: "meh", valve size: "meh", valve zone: "one", water system: "meh", functional location: "one", facility: "one", turns: 4
	And an admin user "admin" exists with username: "admin"
    And I am logged in as "admin"
    And I am at the Show page for valve: "two"
    Then I should see the button "Copy"
    When I click ok in the dialog after pressing "Copy"
    Then I should see a checkbox named BPUKPI with the value "true"
    And recurring frequency unit "year"'s ToString should be selected in the InspectionFrequencyUnit dropdown
	And I should see "2" in the InspectionFrequency field
    And I should see "12345" in the SketchNumber field
    And I should see "123" in the StreetNumber field
    And street "two"'s ToString should be selected in the Street dropdown
    And I should see a display for DisplayValve_Town with town "one"'s ToString
	And I should see "54321" in the WorkOrderNumber field
    And valve billing "municipal"'s Description should be selected in the ValveBilling dropdown
    And street "one"'s Description should be selected in the CrossStreet dropdown
    And valve normal position "open"'s Description should be selected in the NormalPosition dropdown
    And I should see a display for DisplayValve_OperatingCenter with operating center "nj7"'s ToString
	And valve open direction "meh"'s ToString should be selected in the OpenDirection dropdown
	And town section "meh"'s Description should be selected in the TownSection dropdown
    And main type "meh"'s Description should be selected in the MainType dropdown	
    And valve control "one"'s Description should be selected in the ValveControls dropdown
	And valve manufacturer "meh"'s Description should be selected in the ValveMake dropdown
    And valve type "meh"'s Description should be selected in the ValveType dropdown
    And valve size "meh"'s Description should be selected in the ValveSize dropdown
    And valve zone "one"'s Description should be selected in the ValveZone dropdown
    And water system "meh"'s ToString should be selected in the WaterSystem dropdown
    And functional location "one"'s Description should be selected in the FunctionalLocation dropdown
    And facility "one"'s Description should be selected in the Facility dropdown
	And I should not see "VLA-2" in the ValveNumber field 

Scenario: User can select an operating center without choosing a state
	Given I am logged in as "user"	
	And I am at the FieldOperations/Valve/Search page
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	When I select state "two" from the State dropdown
	Then I should not see operating center "nj7" in the OperatingCenter dropdown
	When I select state "one" from the State dropdown
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	
Scenario: user can see arc collector link
    Given I am logged in as "user"
    When I visit the Show page for valve: "one"
	Then I should see the link with href "https://fieldmaps.arcgis.app?itemID=01d4ebf78acc489695b930d5bc2850f3"

Scenario: user can see date range search fields on a valve search page
    Given I am logged in as "user"
    When I visit the FieldOperations/Valve/Search page
	Then I should see the DateInstalled_Operator field
	And I should see the CreatedAt_Operator field
	And I should see the LastInspectionDate_Operator field
	And I should see the DateRetired_Operator field

Scenario Outline: When a user is adding or editing valves, the work order number field validation messages toggle given the asset status.
	Given I am logged in as "user"
	And I am at the FieldOperations/Valve/New page
	When I select asset status <status> from the Status dropdown
	And I press Save
	Then I should see a validation message for WorkOrderNumber with "The WBS # field is required."
	# Edit has the same When/Then expectations
	Given a valve "two" exists with operating center: "nj7", town: "one", street: "one", status: "other", WorkOrderNumber: ""
	And I am logged in as "user"
	When I visit the edit page for valve: "two"
	When I select asset status <status> from the Status dropdown
	And I press Save
	Then I should see a validation message for WorkOrderNumber with "Work Order Number is required for retired, removed and active valves."
	When I enter "12345" into the WorkOrderNumber field
	And I press Save
	Then I should not see a validation message for WorkOrderNumber with "Work Order Number is required for retired, removed and active valves."

	Examples:
	| status               |
	| "removed"            |
	| "retired"            |
	| "active"             |

Scenario Outline: When a user is editing valves, the work order number field validation messages should not toggle given the asset status.
	Given a valve "two" exists with operating center: "nj7", town: "one", street: "one", status: <status>, WorkOrderNumber: ""
	And I am logged in as "user"
	When I visit the edit page for valve: "two"
	And I press Save
	Then I should not see a validation message for WorkOrderNumber with "Work Order Number is required for retired, removed and active valves."

	Examples:
	| status               |
	| "pending"            |
	| "removed"            |
	| "retired"            |
	| "active"             |
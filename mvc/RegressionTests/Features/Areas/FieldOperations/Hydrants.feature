Feature: Hydrants

Background: 
	Given a user "user" exists with username: "user"	
	And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "sewer opening" exists with description: "sewer opening"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a asset status "removed" exists with description: "REMOVED"
	And a asset status "other" exists with description: "REQUEST RETIREMENT"
	And a asset status "other adminonly" exists with description: "PENDING"
	And a hydrant billing "municipal" exists with description: "Municipal"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant billing "company" exists with description: "Company"
	And a hydrant billing "private" exists with description: "Private"
	And a hydrant inspection type "flush" exists with description: "FLUSH"
	And a hydrant tag status "tag" exists with description: "Tag!"		
	And a state "one" exists with abbreviation: "NJ"
	And a state "two" exists with name: "Pennsylvania", abbreviation: "PA"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "one", mapId: "01d4ebf78acc489695b930d5bc2850f3"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a town section "active" exists with town: "one"
    And a town section "inactive" exists with town: "one", active: false
	And a street "one" exists with town: "one", is active: true
	And a street "two" exists with town: "one", full st name: "Easy St", is active: true
	And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a fire district "one" exists with district name: "meh"
	And a fire district town "foo" exists with town: "one", fire district: "one"
	And a functional location "one" exists with town: "one", asset type: "hydrant"
	And a hydrant type "one" exists with Description: "Dry"
	And a hydrant type "two" exists with Description: "Wet"
	And a hydrant outlet configuration "one" exists with Description: "1 STEAMER"
	And a hydrant outlet configuration "two" exists with Description: "2 STEAMERS"

Scenario: User gets a validation error if they check the critical checkbox but do not enter any critical notes
	Given I am logged in as "user"
	And I am at the FieldOperations/Hydrant/New page
	When I check the Critical field
	And I press Save
	Then I should see the validation message "The CriticalNotes field is required."

Scenario: User should not see critical notes textbox if IsCritical checkbox is unchecked
	Given I am logged in as "user"
	And I am at the FieldOperations/Hydrant/New page
	When I check the Critical field
	Then I should see the CriticalNotes field
	When I uncheck the Critical field 
	Then I should not see the CriticalNotes field

Scenario Outline: When a user is adding or editing hydrants, the date retired field visibility and validation messages toggle given the asset status.
	Given I am logged in as "user"
	And I am at the FieldOperations/Hydrant/New page
	When I select asset status <status> from the Status dropdown
	Then I <should_see_field> see the DateRetired field
	When I press Save
	Then I <should_see_validation_message> see a validation message for DateRetired with "DateRetired is required for retired / removed hydrants."
	# Edit has the same When/Then expectations
	Given a hydrant "one" exists with operating center: "nj7" 
	And I am logged in as "user"
	And I am at the edit page for hydrant: "one"
	When I select asset status <status> from the Status dropdown
	Then I <should_see_field> see the DateRetired field
	When I press Save
	Then I <should_see_validation_message> see a validation message for DateRetired with "DateRetired is required for retired / removed hydrants."

	Examples:
	| status               | should_see_field | should_see_validation_message |
	| "active"             | should not       | should not                    |
	| "removed"            | should           | should                        |
	| "retired"            | should           | should                        |

Scenario Outline: When a user is adding or editing hydrants, the validation messages toggle given the asset status.
	Given I am logged in as "user"
	And a hydrant "one" exists with operating center: "nj7" 
	And I am at the FieldOperations/Hydrant/New page
	When I select asset status <status> from the Status dropdown
	When I press Save
	Then I <should_see_validation_message> see a validation message for HydrantMainSize with "Main Size is required for active / installed hydrants."
	# Edit has the same When/Then expectations
	When I visit the edit page for hydrant: "one"
	And I select asset status <status> from the Status dropdown
	And I select "-- Select --" from the HydrantMainSize dropdown
	When I press Save
	Then I <should_see_validation_message> see a validation message for HydrantMainSize with "Main Size is required for active / installed hydrants."
	
	Examples:
	| status      | should_see_validation_message |
	| "active"    | should                        |
	| "removed"   | should not                    |
	| "retired"   | should not                    |

Scenario: Hydrant suffix textbox is only visible for found hydrants on new page
	Given I am logged in as "user"
	And I am at the FieldOperations/Hydrant/New page
	Then I should not see the HydrantSuffix field
	When I check the IsFoundHydrant field
	Then I should see the HydrantSuffix field
	When I uncheck the IsFoundHydrant field
	Then I should not see the HydrantSuffix field

Scenario: Hydrant inspections tab should order inspections from most recent to earliest
	Given a hydrant "one" exists with operating center: "nj7" 
	And a hydrant inspection "one" exists with hydrant: "one", date inspected: "1/1/2015"
	And a hydrant inspection "two" exists with hydrant: "one", date inspected: "2/12/2015"
	And I am logged in as "user"
	And I am at the show page for hydrant: "one"
	When I click the "Inspections" tab
	Then I should see the following values in the inspections-table table
	| Date Inspected     |
	| 2/12/2015 12:00 AM |
	| 1/1/2015 12:00 AM  | 

Scenario: User should be able to add a hydrant inspection from the hydrants page and get back to the hydrant when complete
	Given a hydrant "one" exists with operating center: "nj7" 
	And I am logged in as "user"
	And I am at the show page for hydrant: "one"
	When I click the "Inspections" tab
	And I follow "New Inspection"
	Then I should see a link to the show page for hydrant "one"
	When I select hydrant inspection type "flush" from the HydrantInspectionType dropdown
	And I enter 2 into the GPM field
	And I enter 1 into the MinutesFlowed field
	And I enter 1 into the StaticPressure field
	And I select hydrant tag status "tag" from the HydrantTagStatus dropdown
	And I press Save 
	Then I should see a link to the show page for hydrant "one"

Scenario: User should see a lot of validation when creating a hydrant
	Given a facility "one" exists with facility id: "NJSB-01", town: "one", operating center: "nj7"	
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And I am logged in as "user"
	And I am at the FieldOperations/Hydrant/New page
	When I select state "one" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for HydrantBilling with "The Billing field is required."
	And I should see a validation message for Status with "The Status field is required."
	And I should see a validation message for WorkOrderNumber with "The WBS # field is required."
	When I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save 
	Then I should see a validation message for Town with "The Town field is required."
	When I select hydrant billing "public" from the HydrantBilling dropdown
	And I select town "one" from the Town dropdown
	And I press Save 
	Then I should see a validation message for FireDistrict with "Fire District is required for public hydrants."	
	When I select asset status "active" from the Status dropdown
	And I press Save 
	Then I should see a validation message for Coordinate with "Coordinate is required for active hydrants."
	And I should see "HLA"
	When I select asset status "retired" from the Status dropdown
	And I select hydrant billing "company" from the HydrantBilling dropdown
	And I press Save
	Then I should see a validation message for Street with "The Street field is required."
	When I select street "one" from the Street dropdown
	And I select hydrant billing "public" from the HydrantBilling dropdown
	And I select fire district "one" from the FireDistrict dropdown
	And I enter today's date into the DateRetired field
	And I select facility "one" from the Facility dropdown
	And I select asset status "active" from the Status dropdown
	And I enter "54321" into the WorkOrderNumber field
	Then I should not see the DateRetired field
	When I select asset status "retired" from the Status dropdown
	And I enter "1800" into the YearManufactured field
	Then I should see the DateRetired field
	When I press Save
	Then I should see a validation message for YearManufactured with "Year Manufactured should not be greater than the current year, and not before 1850."
	When I enter "1900" into the YearManufactured field
	And I select hydrant type "one" from the HydrantType dropdown
	And I select hydrant outlet configuration "two" from the HydrantOutletConfiguration dropdown
	And I press Save
	Then the currently shown hydrant shall henceforth be known throughout the land as "Rufus"
	And I should see a display for CreatedAt with today's date
	Then I should see a display for Facility with facility "one"'s Description
	And I should see a display for WorkOrderNumber with "54321"
	And I should see a display for HydrantType with "Dry"
	And I should see a display for HydrantOutletConfiguration with "2 STEAMERS"
	When I click the "Work Orders" tab
	Then I should not see "Create New Work Order"
	And I should see "This Asset is Currently Inactive."
	When I visit the edit page for hydrant: "Rufus"
	And I select hydrant type "two" from the HydrantType dropdown
	And I select hydrant outlet configuration "one" from the HydrantOutletConfiguration dropdown
	And I press Save
	Then I should see a display for HydrantType with "Wet"
	And I should see a display for HydrantOutletConfiguration with "1 STEAMER"

Scenario: User should not be able to create a hydrant for an inactive town section
	Given I am logged in as "user"
	And I am at the FieldOperations/Hydrant/New page	
	When I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown	
    And I wait for ajax to finish loading
    Then I should not see town section "inactive" in the TownSection dropdown

Scenario: User should see some extra validation for FunctionalLocation
	Given an operating center "sap enabled" exists with opcode: "NJ7-sapenabled", name: "Shrewsbury", is contracted operations: "false", sap enabled: "true"
	And a town "la" exists with name: "Loch Arbour"
	And operating center: "sap enabled" exists in town: "la" with abbreviation: "LA"
	And a functional location "la" exists with town: "la", asset type: "hydrant"
	And an operating center "sap enabled contracted" exists with opcode: "NJ7-contracted", name: "Shrewsbury", is contracted operations: "true", sap enabled: "true"	
	And a town "ah" exists with name: "Loch Arbour"
	And operating center: "sap enabled contracted" exists in town: "ah" with abbreviation: "LA"
	And a functional location "ah" exists with town: "ah", asset type: "hydrant"
	And an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
	And I am at the FieldOperations/Hydrant/New page
	When I press Save
	Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."
	When I select state "one" from the State dropdown
	And I select operating center "sap enabled" from the OperatingCenter dropdown
	And I select town "la" from the Town dropdown
	And I press Save
	Then I should see a validation message for FunctionalLocation with "The Functional Location field is required."
	When I select operating center "sap enabled contracted" from the OperatingCenter dropdown
	And I select town "ah" from the Town dropdown
	And I press Save
	Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."
	# Edit is done differently so it needs to be tested separately.
	Given a hydrant "one" exists with operating center: "sap enabled", town: "la"
	And I am at the edit page for hydrant: "one"
	When I select "-- Select --" from the FunctionalLocation dropdown
	And I press Save
	Then I should see a validation message for FunctionalLocation with "The Functional Location field is required."
	Given a hydrant "two" exists with operating center: "sap enabled contracted", town: "ah"
	And I am at the edit page for hydrant: "two"
	When I select "-- Select --" from the FunctionalLocation dropdown
	And I press Save
	Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."

Scenario: User marks a hydrant as out of service and back again
	Given a hydrant "one" exists with operating center: "nj7", town: "one", street: "one"
	And I am logged in as "user"
	And I am at the show page for hydrant: "one" 
	When I click the "Out of Service" tab
	And I press "Change service status"
	Then I should not see the BackInServiceDate field
	And I should see the OutOfServiceDate field
	When I enter "4/14/2015" into the OutOfServiceDate field
	And I click ok in the dialog after pressing "Update status"
	#And I press "Update status" 
	And I click the "Out of Service" tab
	Then I should see the following values in the out-of-service table
	| Out Of Service Date | Out Of Service By User |
	| 4/14/2015           | user: "user"           |
	When I press "Change service status"
	Then I should see the BackInServiceDate field
	And I should not see the OutOfServiceDate field
	When I enter "4/15/2015" into the BackInServiceDate field
	And I press "Update status"
	And I click the "Out of Service" tab
	Then I should see the following values in the out-of-service table
	| Out Of Service Date | Out Of Service By User | Back In Service Date | Back In Service By User |
	| 4/14/2015           | user: "user"           | 4/15/2015            | user: "user"            |

Scenario: An edit role user can not edit certain fields that a useradmin role user can edit 
	Given a hydrant "one" exists with operating center: "nj7", town: "one", street: "one", status: "active"
	And a facility "one" exists with facility id: "NJSB-01", town: "one", operating center: "nj7"
	And a user "edituser" exists with username: "edituser"
	And a role "hydrant-edit" exists with action: "Edit", module: "FieldServicesAssets", user: "edituser", operating center: "nj7"
	When I log in as "user"
	And I visit the edit page for hydrant: "one" 
	Then I should see the IsNonBPUKPI field
	And I should see the HydrantBilling field
	And I should see the HydrantNumber field
	And I should see the HydrantSuffix field
	And I should see the FireDistrict field
	And I should see the FunctionalLocation field
	And I should see the Facility field
	And I should see asset status "active" in the Status dropdown
	And I should see asset status "retired" in the Status dropdown
	And I should see asset status "other" in the Status dropdown
	And I should see asset status "other adminonly" in the Status dropdown
	When I log in as "edituser"
	And I visit the edit page for hydrant: "one" 
	Then I should not see the IsNonBPUKPI field
	And I should not see the HydrantBilling field
	And I should not see the HydrantNumber field
	And I should not see the HydrantSuffix field
	And I should not see the FireDistrict field
	And I should not see the FunctionalLocation field
	And I should not see the Coordinate field
	# active is user-admin only, but it should be selectable still because it's the current value on the hydrant.
	And I should see asset status "active" in the Status dropdown
	# retired and other adminonly should not display at all
	And I should not see asset status "retired" in the Status dropdown
	And I should not see asset status "other adminonly" in the Status dropdown
	# other should be displayed because it's not limited to admins
	And I should see asset status "other" in the Status dropdown

Scenario: User can copy a hydrant
	Given a recurring frequency unit "year" exists with description: "Year"
    And a lateral size "meh" exists
    And a hydrant direction "meh" exists with description: "vOv"
    And a hydrant size "meh" exists
    And a gradient "meh" exists with description: "blah"
    And a hydrant main size "meh" exists
    And a hydrant thread type "meh" exists
    And a main type "meh" exists
    And a town section "one" exists with town: "one"
    And a water system "meh" exists
	And water system "meh" exists in operating center: "nj7"
	And a facility "one" exists with facility id: "NJSB-01", town: "one", operating center: "nj7"
    And a hydrant "meh" exists with hydrant number: "HLA-6", is non b p u k p i: true, is dead end main: true, inspection frequency: 2, map page: "12345", street: "one", town: "one", work order number: "54321", fire district: "one", depth bury feet: 3, depth bury inches: 4, lateral size: "meh", cross street: "two", open direction: "meh", gradient: "meh", hydrant size: "meh", inspection frequency unit: "year", operating center: "nj7", hydrant main size: "meh", hydrant thread type: "meh", main type: "meh", town section: "one", hydrant billing: "company", water system: "meh", functional location: "one", facility: "one", BillingDate: "10/11/2018", DateInstalled: "09/10/2018"
	And an admin user "admin" exists with username: "admin"
    And I am logged in as "admin"
    And I am at the Show page for hydrant: "meh"
    Then I should see the button "Copy"
    When I click ok in the dialog after pressing "Copy"
    Then I should see a checkbox named IsNonBPUKPI with the value "true"
    And I should see a checkbox named IsDeadEndMain with the value "true"
    And I should see "2" in the InspectionFrequency field
	And I should see "12345" in the MapPage field
    And street "one"'s ToString should be selected in the Street dropdown
    And I should see a display for DisplayHydrant_Town with town "one"'s ToString
    And I should see "54321" in the WorkOrderNumber field
#    And fire district "one"'s ToString should be selected in the FireDistrict dropdown
    And I should see "3" in the DepthBuryFeet field
    And I should see "4" in the DepthBuryInches field
    And lateral size "meh"'s Description should be selected in the LateralSize dropdown
    And street "two"'s ToString should be selected in the CrossStreet dropdown
    And hydrant direction "meh"'s Description should be selected in the OpenDirection dropdown	
	And hydrant size "meh"'s Description should be selected in the HydrantSize dropdown
    And recurring frequency unit "year"'s ToString should be selected in the InspectionFrequencyUnit dropdown
    And I should see a display for DisplayHydrant_OperatingCenter with operating center "nj7"'s ToString
	And hydrant main size "meh"'s Description should be selected in the HydrantMainSize dropdown
	And hydrant thread type "meh"'s Description should be selected in the HydrantThreadType dropdown
	And main type "meh"'s Description should be selected in the MainType dropdown
	And town section "one"'s Description should be selected in the TownSection dropdown
	And hydrant billing "company"'s Description should be selected in the HydrantBilling dropdown
	And water system "meh"'s ToString should be selected in the WaterSystem dropdown
	And functional location "one"'s Description should be selected in the FunctionalLocation dropdown
	And facility "one"'s Description should be selected in the Facility dropdown
	And I should not see "HLA-6" in the HydrantNumber field
	And I should not see "10/11/2018" in the BillingDate field
	And I should not see "09/10/2018" in the DateInstalled field

Scenario: User can select an operating center without choosing a state
	Given I am logged in as "user"	
	And I am at the FieldOperations/Hydrant/Search page
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	When I select state "two" from the State dropdown
	Then I should not see operating center "nj7" in the OperatingCenter dropdown
	When I select state "one" from the State dropdown
	Then I should see operating center "nj7" in the OperatingCenter dropdown

# This is critical for GIS to be able to link from the Hydrants in the GIS Maps
# to mapcall. If they are required to include an operating center, they will need this data
# which they do not have available.
Scenario: User can search for a hydrant without choosing an operating center
	Given I am logged in as "user"	
	And a hydrant "one" exists with operating center: "nj7", hydrant outlet configuration: "one", hydrant type: "two"
	And I am at the FieldOperations/Hydrant/Search page
	When I enter hydrant "one"'s HydrantNumber into the HydrantNumber_Value field
	And I press "Search"
	Then I should see the following values in the hydrants table
	| Hydrant Outlet Configuration | Hydrant Type |
	| 1 STEAMER                    | Wet          |
	And I should see a link to the show page for hydrant: "one"

Scenario: user can see arc collector link
	Given I am logged in as "user"
	And an operating center "huh" exists with opcode: "NJ9", name: "Huh", state: "one", arc mobile map id: "01d4ebf78acc489695b930d5bc2850f3"
	And a role "hydrant-useradmin-huh" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "huh"
	And a hydrant "one" exists with operating center: "huh" 
	And I am at the Show page for hydrant "one"
	Then I should see the link with href "https://fieldmaps.arcgis.app?itemID=01d4ebf78acc489695b930d5bc2850f3"

Scenario: user can see date range search fields on a hydrant search page
    Given I am logged in as "user"
    When I visit the FieldOperations/Hydrant/Search page
	Then I should see the DateInstalled_Operator field
	And I should see the UpdatedAt_Operator field
	And I should see the CreatedAt_Operator field
	And I should see the LastInspectionDate_Operator field
	And I should see the DateRetired_Operator field

Scenario Outline: When a user is adding or editing hydrants, the work order number field validation messages toggle given the asset status.
	Given I am logged in as "user"
	And I am at the FieldOperations/Hydrant/New page
	When I select asset status <status> from the Status dropdown
	When I press Save
	Then I should see a validation message for WorkOrderNumber with "The WBS # field is required."
	 #Edit has the same When/Then expectations
	Given a hydrant "one" exists with operating center: "nj7", WorkOrderNumber: "", Id: 1, status: "other"
	And I am logged in as "user"
	And I am at the edit page for hydrant: "one"
	When I select asset status <status> from the Status dropdown
	When I press Save
	Then I should see a validation message for WorkOrderNumber with "Work Order Number is required for retired, removed and active hydrants."
	When I enter "12345" into the WorkOrderNumber field
	And I press Save
	Then I should not see a validation message for WorkOrderNumber with "Work Order Number is required for retired, removed and active hydrants."

	Examples:
	| status               |
	| "removed"            |
	| "retired"            |
	| "active"             |

Scenario: User can view a hydrant 
    Given a hydrant "one" exists with operating center: "nj7", town: "one", street: "one"
    And I am logged in as "user"
    And I am at the show page for hydrant: "one"
    Then I should see a display for hydrant: "one"'s Id

Scenario Outline: When a user is editing hydrants, the work order number field validation messages should not toggle given the asset status.
	Given a hydrant "one" exists with operating center: "nj7", WorkOrderNumber: "", Id: 1, status: <status>
	And I am logged in as "user"
	And I am at the edit page for hydrant: "one"
	When I press Save
	Then I should not see a validation message for WorkOrderNumber with "Work Order Number is required for retired, removed and active hydrants."

	Examples:
	| status               |
	| "other"              |
	| "removed"            |
	| "retired"            |
	| "active"             |

Scenario Outline: When a user is editing hydrants, the notification message toggle given the asset status.
	Given I am logged in as "user"
	And a hydrant "one" exists with operating center: "nj7" 
	When I visit the edit page for hydrant: "one"
	And I select asset status <status> from the Status dropdown
	Then I <should_see_notification_message> see the HydrantStatusAlert element
	
	Examples:
	| status      | should_see_notification_message |
	| "active"    | should not                      |
	| "removed"   | should                          |
	| "retired"   | should                          |

Scenario: User can paint a hydrant from the show page
    Given a hydrant "one" exists with operating center: "nj7"
    And I am logged in as "user"
    And I am at the show page for hydrant: "one"
    When I click the "Paint" tab
    And I wait for ajax to finish loading
    And I enter today's date into the PaintedAt field
    And I press "Add Record"
    Then I should be at the show page for hydrant "one"
    When I click the "Paint" tab
    And I wait for ajax to finish loading
    Then I should see today's date in the table paintingsTable's "Painted At" column

Scenario: User can paint a hydrant from the index table
    Given a hydrant "one" exists with operating center: "nj7"
    And I am logged in as "user"
    And I am at the FieldOperations/Hydrant/Search page
    When I enter hydrant "one"'s HydrantNumber into the HydrantNumber_Value field
    And I press "Search"
    Then I should be at the FieldOperations/Hydrant page
    When I click ok in the dialog after pressing "Painted Today"
    Then I should be at the FieldOperations/Hydrant page
    And I should not see the button "Painted Today"

Scenario: User can edit and delete hydrant painting records
    Given a hydrant "one" exists with operating center: "nj7"
    And a hydrant painting "one" exists with hydrant: "one", painted at: "today"
    And I am logged in as "user"
    And I am at the show page for hydrant: "one"
    When I click the "Paint" tab
    And I wait for ajax to finish loading
    And I click the "Edit" link in the 1st row of paintingsTable
    And I wait for ajax to finish loading
    And I enter tomorrow into the PaintedAt field
    And I press "Update Record"
    And I wait for ajax to finish loading
    Then I should be at the show page for hydrant "one"
    When I click the "Paint" tab
    And I wait for ajax to finish loading
    Then I should see tomorrow in the table paintingsTable's "Painted At" column
    When I click ok in the dialog after pressing "Delete"
    And I wait for ajax to finish loading
    Then I should be at the show page for hydrant "one"
    When I click the "Paint" tab
    And I wait for ajax to finish loading
    Then hydrant painting "one" should no longer exist
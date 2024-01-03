Feature: Sewer Opening Page

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", mapId: "01d4ebf78acc489695b930d5bc2850f3"
    And an operating center "ew4" exists with opcode: "EW4", name: "Edison", is active: "false"
    And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "sewer opening" exists with description: "sewer opening"
    And a town abbreviation type "town" exists
    And a town "one" exists with name: "Loch Arbour"
    And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
    And a waste water system "one" exists with id: 1, waste water system name: "Water System 1", operating center: "nj7"
    And waste water system: "one" exists in town: "one"
    And a town section "active" exists with town: "one"
    And a town section "inactive" exists with town: "one", active: false
    And a street "one" exists with town: "one", is active: true
    And a street "two" exists with town: "one", full st name: "Easy St", is active: true
    And a pipe material "one" exists with description: "Cast Iron"
    And a functional location "one" exists with town: "one", asset type: "sewer opening"
    And a sewer opening "one" exists with opening number: "MAD-1", town: "one", street: "one", operating center: "nj7", functional location: "one"
    And a sewer opening "two" exists with opening number: "MAD-2", town: "one", street: "two", operating center: "nj7", functional location: "one"
    And a sewer opening "three" exists with opening number: "MAD-3", town: "one", street: "two", operating center: "nj7", functional location: "one", critical: true
    And a sewer opening "four" exists with opening number: "MAD-3", town: "one", street: "two", operating center: "ew4", functional location: "one"
    And a asset status "cancelled" exists with description: "CANCELLED"
    And a asset status "pending" exists with description: "PENDING"
    And a asset status "active" exists with description: "ACTIVE"
    And a asset status "retired" exists with description: "RETIRED"
    And a asset status "removed" exists with description: "REMOVED"
    And a coordinate "one" exists
    And a smart cover alert application description "one" exists with description: "SmartFLOE(tm)"
    And a smart cover alert "one" exists with smart cover alert application description: "one", sewer opening: "one"
    And I am logged in as "admin"
    And a sewer opening type "catch basin" exists with description: "catch basin"
    And a sewer opening type "clean out" exists with description: "clean out"
    And a sewer opening type "lamphole" exists with description: "lamphole"
    And a sewer opening type "manhole" exists with description: "manhole"
    And a sewer opening type "outfall" exists with description: "outfall"
    And a sewer opening type "npdes regulator" exists with description: "npdes regulator"

Scenario: user can see waste water system dropdown description
    When I visit the FieldOperations/SewerOpening/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I wait for ajax to finish loading
    Then I should see "NJ7WW0001 - Water System 1" in the WasteWaterSystem dropdown

Scenario: user can search for a sewer opening
    Given a sewer opening "twobad" exists with opening number: "MAD-2", town: "one", street: "two", operating center: "nj7", functional location: "one", TaskNumber: "", status: "cancelled"
    When I visit the FieldOperations/SewerOpening/Search page
    And I press Search
    Then I should see a link to the Show page for sewer opening: "one"
    And I should not see a link to the FieldOperations/SewerOpeningInspection/New page for sewer opening: "twobad"
    When I follow the Show link for sewer opening "one"
    Then I should be at the Show page for sewer opening: "one"
    When I visit the FieldOperations/SewerOpening/Search page
    And I select "Yes" from the Critical dropdown 
    And I press Search 
    Then I should see a link to the Show page for sewer opening: "three"
    When I follow the Show link for sewer opening "three"
    Then I should be at the Show page for sewer opening: "three"

Scenario: user can view a sewer opening
    When I visit the Show page for sewer opening: "one"
    Then I should see a display for sewer opening: "one"'s OpeningNumber
    And I should see a display for sewer opening: "one"'s Id

Scenario: user can link two sewer openings through a connection
    When I visit the Show page for sewer opening: "one"
    And I click the "Connections" tab
    And I press "Add New Connection"
    And I enter sewer opening "two"'s OpeningNumber and select sewer opening "two"'s OpeningNumber from the ConnectedOpening combobox
    And I press "Add Connection"
    And I click the "Connections" tab
    Then I should be at the Show page for sewer opening: "one"
    And I should see the following values in the connections-table table
    | Is Inlet | Upstream Opening | Downstream Opening | Sewer Pipe Material |
    | No       | MAD-2            | MAD-1              |                     |
    When I press "Add New Connection"
    And I enter sewer opening "two"'s OpeningNumber and select sewer opening "two"'s OpeningNumber from the ConnectedOpening combobox
    And I press "Add Connection"
    And I click the "Connections" tab
    Then I should be at the Show page for sewer opening: "one"
    And I should see the following values in the connections-table table
    | Is Inlet | Upstream Opening | Downstream Opening | Sewer Pipe Material |
    | No       | MAD-2            | MAD-1              |                     |
    When I press "Add New Connection"
    And I enter sewer opening "one"'s OpeningNumber and select sewer opening "one"'s OpeningNumber from the ConnectedOpening combobox
    And I press "Add Connection"
    And I click the "Connections" tab
    Then I should be at the Show page for sewer opening: "one"
    And I should see the following values in the connections-table table
    | Is Inlet | Upstream Opening | Downstream Opening | Sewer Pipe Material |
    | No       | MAD-2            | MAD-1              |                     |
    When I click the "Edit" link in the 1st row of connections-table
    And I wait for the page to reload
    And I select pipe material "one" from the SewerPipeMaterial dropdown
    And I press "Save"
    And I click the "Connections" tab
    Then I should be at the Show page for sewer opening: "one"
    And I should see the following values in the connections-table table
    | Is Inlet | Upstream Opening | Downstream Opening | Sewer Pipe Material |
    | No       | MAD-2            | MAD-1              | Cast Iron           |
    When I click the "Delete" button in the 1st row of connections-table and then click ok in the confirmation dialog
    Then I should be at the Show page for sewer opening: "one"
    And I should not see "MAD-2"
    And I should not see "Cast Iron"
   
Scenario: user can add a sewer opening
    Given a sewer opening type "one" exists with description: "catch basin"
    When I visit the FieldOperations/SewerOpening/New page
    And I press Save
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    And I should see a validation message for Coordinate with "The Coordinate field is required."
    And I should see a validation message for TaskNumber with "The WBS # field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Save
    Then I should see a validation message for Town with "The Town field is required."
    When I select town "one" from the Town dropdown
    And I press Save
    Then I should see a validation message for Street with "The Street field is required."
    And I should see "NJ7WW0001 - Water System 1" in the WasteWaterSystem dropdown
    And I should see a validation message for Status with "The Opening Status field is required."
    When I enter coordinate "one"'s Id into the Coordinate field
    And I select street "one" from the Street dropdown
    When I select asset status "cancelled" from the Status dropdown
    Then I should not see the DateRetired field
    When I select asset status "retired" from the Status dropdown
    Then I should see the DateRetired field
    When I select "NPDES REGULATOR" from the SewerOpeningType dropdown
    And I enter "54321" into the TaskNumber field
    And I enter "12/11/2020" into the DateRetired field
    And I press Save
    Then I should see a validation message for OutfallNumber with "The OutfallNumber field is required."
    When I enter "65432" into the OutfallNumber field
    And I select "Blue Water" from the BodyOfWater dropdown
    When I enter "Sesame Street" into the LocationDescription field
    And I press Save
    Then the currently shown sewer opening will now be referred to as "Bernard"
    And I should see a display for TaskNumber with "54321"
    And I should see a display for DateRetired with "12/11/2020 12:00:00 AM"
    When I click the "Work Orders" tab
    Then I should not see "Create New Work Order"
    And I should see "This Asset is Currently Inactive."

Scenario: user cannot create a seweropening for an inactive town section
    When I visit the FieldOperations/SewerOpening/New page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I wait for ajax to finish loading
    Then I should not see town section "inactive" in the TownSection dropdown
    
Scenario: user can edit a sewer opening
    Given a sewer opening "twobad" exists with opening number: "MAD-2", town: "one", street: "two", operating center: "nj7", functional location: "one"
    #Given a functional location "one" exists with town: "one"
    When I visit the Edit page for sewer opening: "one"
    Then I should see "NJ7WW0001 - Water System 1" in the WasteWaterSystem dropdown
    When I select functional location "one" from the FunctionalLocation dropdown
    And I enter "MAD-2" into the OpeningNumber field
    And I press Save
    Then I should see a validation message for OpeningNumber with "The generated opening number 'MAD-2' is not unique to the operating center 'NJ7 - Shrewsbury'"
    When I enter "MAD-3" into the OpeningNumber field
    And I press Save
    Then I should see a validation message for OpeningNumber with "The generated opening number 'MAD-3' is not unique to the operating center 'NJ7 - Shrewsbury'"
    When I enter "MAD-5" into the OpeningNumber field
    And I enter "1" into the DepthToInvert field
    And I enter "2" into the RimElevation field
    And I press Save
    Then I should be at the Show page for sewer opening: "one"
    And I should see a display for OpeningNumber with "MAD-5"
    And I should see a display for DepthToInvert with "1"
    And I should see a display for RimElevation with "2"

Scenario: user can see SAPEquipmentId
    Given a sewer opening "twobad" exists with opening number: "MAD-2", town: "one", street: "two", operating center: "nj7", functional location: "one"
    When I visit the Edit page for sewer opening: "one"
    Then I should see a display for SAPEquipmentId with "123456"

Scenario: User should see some extra validation for FunctionalLocation
    Given an operating center "sap enabled" exists with opcode: "NJ7-sapenabled", name: "Shrewsbury", is contracted operations: "false", sap enabled: "true"
    And a town "la" exists with name: "Loch Arbour"
    And operating center: "sap enabled" exists in town: "la" with abbreviation: "LA"
    And a sewer opening functional location "la" exists with town: "la"
    And an operating center "sap enabled contracted" exists with opcode: "NJ7-contracted", name: "Shrewsbury", is contracted operations: "true", sap enabled: "true"
    And a town "ah" exists with name: "Loch Arbour"
    And operating center: "sap enabled contracted" exists in town: "ah" with abbreviation: "LA"
    And a sewer opening functional location "ah" exists with town: "ah"
    And I am logged in as "admin"
    And I am at the FieldOperations/SewerOpening/New page
    When I press Save
    Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."
    When I select operating center "sap enabled" from the OperatingCenter dropdown
    And I select town "la" from the Town dropdown
    And I press Save
    Then I should see a validation message for FunctionalLocation with "The Functional Location field is required."
    When I select operating center "sap enabled contracted" from the OperatingCenter dropdown
    And I select town "ah" from the Town dropdown
    And I press Save
    Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."
    # Edit is done differently so it needs to be tested separately.
    Given a sewer opening "notcontracted" exists with operating center: "sap enabled", town: "la"
    And I am at the edit page for sewer opening: "notcontracted"
    When I select "-- Select --" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a validation message for FunctionalLocation with "The Functional Location field is required."
    Given a sewer opening "grrrr" exists with operating center: "sap enabled contracted", town: "ah"
    And I am at the edit page for sewer opening: "two"
    When I select "-- Select --" from the FunctionalLocation dropdown
    And I press Save
    Then I should not see a validation message for FunctionalLocation with "The Functional Location field is required."

Scenario: User gets a validation error if they check the critical checkbox but do not enter any critical notes
    Given I am at the FieldOperations/SewerOpening/New page
    When I check the Critical field
    And I press Save
    Then I should see the validation message "The CriticalNotes field is required."

Scenario: User should not see critical notes textbox if IsCritical checkbox is unchecked
    Given I am at the FieldOperations/SewerOpening/New page
    When I check the Critical field
    Then I should see the CriticalNotes field
    When I uncheck the Critical field 
    Then I should not see the CriticalNotes field

Scenario: User can copy a sewer opening
    Given a town section "meh" exists with town: "one"
    And a sewer opening "meh" exists with opening number: "MAD-4", operating center: "nj7", town: "one", street: "two", intersecting street: "one", task number: "12345", is epoxy coated: true, is doghouse opening: true, town section: "meh", functional location: "one"
    And I am at the Show page for sewer opening: "meh"
    Then I should see the button "Copy"
    When I click ok in the dialog after pressing "Copy"
    Then I should see a display for DisplaySewerOpening_OperatingCenter with operating center "nj7"'s ToString
    And I should see a display for DisplaySewerOpening_Town with town "one"'s ToString
    And street "two"'s ToString should be selected in the Street dropdown
    And street "one"'s ToString should be selected in the IntersectingStreet dropdown
    And I should see "12345" in the TaskNumber field
    And I should see a checkbox named IsEpoxyCoated with the value "true"
    And I should see a checkbox named IsDoghouseOpening with the value "true"
    And town section "meh"'s ToString should be selected in the TownSection dropdown
    And functional location "one"'s Description should be selected in the FunctionalLocation dropdown
    And I should not see "MAD-4" in the OpeningNumber field

Scenario: User can create sewer main cleaning record from SewerOpening show page
    When I visit the FieldOperations/SewerOpening/Search page
    And I press Search
    Then I should see a link to the Show page for sewer opening: "one"
    When I follow the Show link for sewer opening "one"
    Then I should be at the Show page for sewer opening: "one"
    When I click the "Main Inspections / Cleaning" tab
    Then I should see a link to the FieldOperations/SewerMainCleaning/NewFromSewerOpening page for sewer opening: "one"

Scenario: User can't add inspection if operating center is inactive
    Given I am logged in as "admin"
    When I visit the Show page for sewer opening: "four"
    And I click the "Inspections" tab
    Then I should not see "New Inspection"

Scenario: user cant add inspection from index if operating center is inactive
    Given I am logged in as "admin"
    When I visit the FieldOperations/SewerOpening/Search page
    And I press Search
    Then I should see a link to the Show page for sewer opening: "four"
    And I should not see a link to the FieldOperations/SewerOpeningInspection/New page for sewer opening: "four"

Scenario: User can't add main cleaning if operating center is inactive
    Given I am logged in as "admin"
    When I visit the Show page for sewer opening: "four"
    And I click the "Main Inspections / Cleaning" tab
    Then I should not see a link to the FieldOperations/SewerMainCleaning/NewFromSewerOpening page for sewer opening: "four"

Scenario: user can see date range search fields on sewer opening search page
    Given I am logged in as "user"
    When I visit the FieldOperations/SewerOpening/Search page
    Then I should see the DateInstalled_Operator field
    And I should see the CreatedAt_Operator field
    And I should see the DateRetired_Operator field

Scenario: user can see arc collector link
    And an operating center "huh" exists with opcode: "NJ9", name: "Huh", arc mobile map id: "01d4ebf78acc489695b930d5bc2850f3"
    And a sewer opening "huh" exists with operating center: "huh" 
    And I am at the Show page for sewer opening "huh"
    Then I should see the link with href "https://fieldmaps.arcgis.app?itemID=01d4ebf78acc489695b930d5bc2850f3"

Scenario Outline: When a user is adding or editing sewer openings, the date retired field visibility and validation messages toggle given the asset status.
    Given I am logged in as "user"
    And I am at the FieldOperations/SewerOpening/New page
    When I select asset status <status> from the Status dropdown
    Then I <should_see_field> see the DateRetired field
    When I press Save
    Then I <should_see_validation_message> see a validation message for DateRetired with "DateRetired is required for retired / removed sewer openings."
    # Edit has the same When/Then expectations
    Given a sewer opening "twobad" exists with opening number: "MAD-2", town: "one", street: "two", operating center: "nj7", functional location: "one"
    When I visit the Edit page for sewer opening: "one"
    And I select asset status <status> from the Status dropdown
    Then I <should_see_field> see the DateRetired field
    When I press Save
    Then I <should_see_validation_message> see a validation message for DateRetired with "DateRetired is required for retired / removed sewer openings."

    Examples:
    | status               | should_see_field | should_see_validation_message |
    | "active"             | should not       | should not                    |
    | "removed"            | should           | should                        |
    | "retired"            | should           | should                        |

Scenario Outline: When a user is adding or editing sewer openings, the work order number field validation messages toggle given the asset status.
    Given I am logged in as "user"
    And I am at the FieldOperations/SewerOpening/New page
    When I select asset status <status> from the Status dropdown
    When I press Save
    Then I should see a validation message for TaskNumber with "The WBS # field is required."
    # Edit has the same When/Then expectations
    Given a sewer opening "twobad" exists with opening number: "MAD-2", town: "one", street: "two", operating center: "nj7", functional location: "one", TaskNumber: "", status: "cancelled"
    When I visit the Edit page for sewer opening: "twobad"
    When I select asset status <status> from the Status dropdown
    And I press Save
    Then I should see a validation message for TaskNumber with "Work Order Number is required for retired, removed and active sewer openings."
    When I enter "12345" into the TaskNumber field
    And I press Save
    Then I should not see a validation message for TaskNumber with "Work Order Number is required for retired, removed and active sewer openings."

    Examples:
    | status               |
    | "removed"            |
    | "retired"            |
    | "active"             |

Scenario Outline: When a user is editing sewer openings, the work order number field validation messages should not toggle given the asset status.
    Given a sewer opening "twobad" exists with opening number: "MAD-2", town: "one", street: "two", operating center: "nj7", functional location: "one", TaskNumber: "", status: <status>
    When I visit the Edit page for sewer opening: "twobad"
    And I press Save
    Then I should not see a validation message for TaskNumber with "Work Order Number is required for retired, removed and active sewer openings."

    Examples:
    | status               |
    | "pending"            |
    | "removed"            |
    | "retired"            |
    | "active"             |

Scenario: User can copy a Pending sewer opening
    Given a sewer opening "pen" exists with opening number: "MAD-4", operating center: "nj7", town: "one", street: "two", functional location: "one", TaskNumber: "", status: "pending"
    When I visit the Show page for sewer opening: "pen"
    Then I should see the button "Copy"

Scenario: user can view linked smart cover alerts
    When I visit the Show page for sewer opening: "one"
    And I click the "Smart Cover Alerts" tab
    Then I should see a link to the Show page for smart cover alert: "one"
    And I should see the following values in the smartCoverAlerts table
  | Application Description | Acknowledged |
  | SmartFLOE(tm)           | No           |

Scenario: User cannot create a sewer main cleaning record and an inspection for InActive SewerOpening
    Given a sewer opening "twobad" exists with opening number: "MAD-2", town: "one", street: "two", operating center: "nj7", functional location: "one", TaskNumber: "", status: "cancelled"
    When I visit the Show page for sewer opening: "twobad"
    And I click the "Main Inspections / Cleaning" tab
    Then I should not see a link to the FieldOperations/SewerMainCleaning/NewFromSewerOpening page for sewer opening: "one"
    When I click the "Inspections" tab
    Then I should not see "New Inspection"

Scenario: User can create a npdes regulator inspection from the sewer opening index page
    Given a sewer opening "regulator" exists with opening number: "MSC-2", town: "one", operating center: "nj7", sewer opening type: "npdes regulator", status: "active"
    When I visit the FieldOperations/SewerOpening/Search page
    And I select "NPDES REGULATOR" from the SewerOpeningType dropdown
    And I press Search
    Then I should see a link to the Show page for sewer opening: "regulator"
    When I follow the show link for sewer opening "regulator"
    When I click the "Inspections" tab
    Then I should see "New NPDES Regulator Inspection"
    When I follow "New NPDES Regulator Inspection"
    Then I should be at the FieldOperations/NpdesRegulatorInspection/New page

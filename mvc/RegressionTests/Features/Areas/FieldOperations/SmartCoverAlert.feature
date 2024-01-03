Feature: SmartCoverAlert

Background:
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town "one" exists with name: "King's Head"
	And a town section "one" exists with name: "A section", town: "one"
	And operating center: "nj7" exists in town: "one"
	And a street "one" exists with town: "one", is active: true
	And a street "two" exists with town: "one", is active: true
	And a sewer opening "one" exists with opening number: "MAD-1", town: "one", town section: "one", street number: "228", street: "one", operating center: "nj7", intersecting street: "two"
	And an asset type "sewer opening" exists with description: "sewer opening"
	And operating center "nj7" has asset type "sewer opening"
	And work order requesters exist
	And work order purposes exist
	And work order priorities exist
	And work descriptions exist
    And markout requirements exist
	And acoustic monitoring exist
	And a user "user" exists with username: "user"
	And an admin user "admin" exists with username: "admin"
	And a role "role-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "role-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "role-read-assets" exists with action: "Read", module: "FieldServicesAssets", user: "user"
	And a role "role-edit-assets" exists with action: "Edit", module: "FieldServicesAssets", user: "user"
	And a smart cover alert application description "one" exists with description: "SmartFLOE(tm)"
	And a smart cover alert "one" exists with smart cover alert application description: "one", sewer opening: "one"
	
Scenario: User can search for a smart cover alert and acknowledge it
	Given I am logged in as "user"
	And I am at the FieldOperations/SmartCoverAlert/Search page
	When I press Search
	Then I should see a link to the Show page for smart cover alert: "one"
	And I should see the following values in the smartCoverAlertsTable table
	| State | Operating Center | Town        | Sewer Opening Number | Date Received         | Acknowledged | Work Order Number | Application Description |
	| NJ    | NJ7 - Shrewsbury | King's Head | 67890                | 1/9/2022 12:00:00 AM  | No           |                   | SmartFLOE(tm)           |
	When I press Acknowledge
	Then I should see a display for User with "user"
	When I enter "01/10/2022" into the AcknowledgedOn field	
	And I press Save
	Then I should be at the FieldOperations/SmartCoverAlert page
	And I should see a link to the Show page for smart cover alert: "one"
	And I should see the following values in the smartCoverAlertsTable table
	| State | Operating Center | Town        | Sewer Opening Number | Acknowledged | Work Order Number | Application Description |
	| NJ    | NJ7 - Shrewsbury | King's Head | 67890                | Yes          |                   | SmartFLOE(tm)           |

Scenario: User can view a smart cover alert
    Given I am logged in as "user"
    When I visit the Show page for smart cover alert: "one"
	Then I should see a display for AlertId with "12345"
    And I should see a display for SewerOpening_State with "NJ"
    And I should see a display for SewerOpening_OperatingCenter with "NJ7 - Shrewsbury"
    And I should see a display for SewerOpening_Town with "King's Head"
    And I should see a display for SewerOpening_TownSection with "A section"
    And I should see a display for ApplicationDescription with "SmartFLOE(tm)"
    And I should see a display for Latitude with "39.9"
    And I should see a display for Longitude with "-75.04"
    And I should see a display for SensorToBottom with "20"
    And I should see a display for ManholeDepth with "160"
    And I should see a display for DateReceived with "1/9/2022 12:00:00 AM"
    And I should see a display for Acknowledged with "No"
    And I should see a display for PowerPackVoltage with "3.44"
    And I should see a display for WaterLevelAboveBottom with "-58.9"
    And I should see a display for Temperature with "55.4"
    And I should see a display for SignalStrength with "4"
    And I should see a display for SignalQuality with "14"
	And I should see a link to the Show page for sewer opening: "one"
	When I follow "Create Work Order"
	Then I should be at the FieldOperations/WorkOrder/New page

Scenario: User can search for a smart cover alert and create work order for it
	Given I am logged in as "user"
	And a work order "one" exists with operating center: "nj7", work description: "hydrant installation", markout requirement: "none", street: "one", nearest cross street: "two", smart cover alert: "one"
    And a crew "one" exists with description: "inactive", operating center: "nj7", active: false
	And a crew assignment "ca" exists with work order: "one", crew: "one", assigned for: "1/1/2000 03:00:00 AM", assigned on: "1/1/2000", date started: "05/15/2018 10:10:00 AM"	
	And I am at the FieldOperations/SmartCoverAlert/Search page
	When I press Search
	Then I should see a link to the Show page for smart cover alert: "one"
	And I should see the following values in the smartCoverAlertsTable table
	| State | Operating Center | Town        | Sewer Opening Number | Acknowledged | Work Order Number | Application Description |
	| NJ    | NJ7 - Shrewsbury | King's Head | 67890                | No           |      1            | SmartFLOE(tm)           |
	When I follow "Create Work Order"
	Then I should be at the FieldOperations/WorkOrder/New page
	And operating center "nj7"'s ToString should be selected in the OperatingCenter dropdown
	And town "one"'s ToString should be selected in the Town dropdown
	And town section "one"'s ToString should be selected in the TownSection dropdown
	And I should see sewer opening "one"'s StreetNumber in the StreetNumber field
	And I should see street "one"'s ToString in the Street_AutoComplete field
	And I should see street "two"'s ToString in the NearestCrossStreet_AutoComplete field
	And asset type "sewer opening"'s ToString should be selected in the AssetType dropdown
	And work order requester "acoustic monitoring"'s ToString should be selected in the RequestedBy dropdown
	And work order priority "emergency"'s ToString should be selected in the Priority dropdown
	When I select work description "sewer opening repair" from the WorkDescription dropdown
	And I select markout requirement "none" from the MarkoutRequirement dropdown
	And I select acoustic monitoring "smart cover" from the AcousticMonitoringType dropdown
	And I select work order purpose "equip reliability" from the Purpose dropdown
	And I press Save
	Then the currently shown work order will now be referred to as "new"
    And I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "new"
	When I visit the Show page for smart cover alert: "one"
	And I click the "Work Orders" tab
	Then I should see the following values in the workOrdersTable table
	| Work Order Number | Description of Job   | Date Started             |
	|      1            | HYDRANT INSTALLATION |  5/15/2018 10:10:00 AM   |
	|      2            | SEWER OPENING REPAIR |                          |
	
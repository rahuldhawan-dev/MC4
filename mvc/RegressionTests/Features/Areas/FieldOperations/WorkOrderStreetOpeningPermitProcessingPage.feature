Feature: WorkOrderStreetOpeningPermitProcessingPage

Background:
    Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "false", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", is contracted operations: "true"
    And an operating center "nj6" exists with opcode: "NJ6", name: "Short Hills", sap enabled: "true", sap work orders enabled: "true", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e"
    And a town "nj7burg" exists with name: "TOWN"
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg", name: "Tucson"
    And a town section "inactive" exists with town: "nj7burg", active: false
    And a street "one" exists with town: "nj7burg", full st name: "EASY STREET", is active: true
    And a street "two" exists with town: "nj7burg", is active: true, full st name: "HIGH STREET"
    And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "main" exists with description: "main"
    And an asset type "service" exists with description: "service"
    And an asset type "sewer opening" exists with description: "sewer opening"
    And an asset type "sewer lateral" exists with description: "sewer lateral"
    And an asset type "sewer main" exists with description: "sewer main"
    And an asset type "storm catch" exists with description: "storm catch"
    And an asset type "equipment" exists with description: "equipment"
    And an asset type "facility" exists with description: "facility"
    And an asset type "main crossing" exists with description: "main crossing"
    And operating center: "nj7" has asset type "valve"
    And operating center: "nj7" has asset type "hydrant"
    And operating center: "nj7" has asset type "main"
    And operating center: "nj7" has asset type "service"
    And operating center: "nj7" has asset type "sewer opening"
    And operating center: "nj7" has asset type "sewer lateral"
    And operating center: "nj7" has asset type "sewer main"
    And operating center: "nj7" has asset type "storm catch"
    And operating center: "nj7" has asset type "equipment"
    And operating center: "nj7" has asset type "facility"
    And operating center: "nj7" has asset type "main crossing"
    And a hydrant "one" exists with street: "one", town: "nj7burg", operating center: "nj7"
    And a valve "one" exists with operating center: "nj7", town: "nj7burg", street: "one"
    And a sewer opening "one" exists with street: "one"
    And a facility "one" exists with town: "nj7burg", operating center: "nj7"
    And an equipment "one" exists with facility: "one"
    And a main crossing status "active" exists with description: "Active"
    And a main crossing "one" exists with length of segment: "100.01", stream: "one", town: "nj7burg", operating center: "nj7", main crossing status: "active"
    And work order requesters exist
    And work order purposes exist
    And work order priorities exist
    And work descriptions exist
    And markout requirements exist
    And a coordinate "one" exists
    And a user "user" exists with username: "user", full name: "Roy"
    And an admin user "admin" exists with username: "admin"
    And a contractor "one" exists with name: "Sunil", is active: "true", awr: "true"
    And a user "readonly-user" exists with username: "readonly-user", default operating center: "nj7", full name: "Smith"
    And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-edit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-read-readonly-user" exists with action: "Read", module: "FieldServicesWorkManagement", user: "readonly-user", operating center: "nj7"
    And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a role "asset-add" exists with action: "Add", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a work order "one" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "one", work order priority: "emergency"
    And a work order "two" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "valve repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", date completed: "03/22/2023", work order priority: "emergency", distance from cross street: "12", alert issued: true, lost water: 5
    And a work order "three" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "sewer main", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "sewer main overflow", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "none", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", work order priority: "emergency"
    And a work order "cancelled" exists with operating center: "nj7", cancelled at: "11/11/2016"
    And a role "markoutviolation-add" exists with action: "Add", module: "BPUGeneral", user: "user", operating center: "nj7"

# Search/Index
Scenario: User can search for work orders
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Search page
    Then I should not see operating center "nj6"'s Description in the OperatingCenter dropdown
    And I should see operating center "nj7"'s Description in the OperatingCenter dropdown
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "cancelled"
    And I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "one"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "two"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "three"

Scenario: Admin user can search for work orders
    Given a work order "four" exists with operating center: "nj6", asset type: "hydrant", work description: "hydrant flushing"
    And a work order "five" exists with operating center: "nj6", asset type: "valve", work description: "valve replacement", sap work order number: "1234567890", work order priority: "emergency", s o p required: "true"
    And I am logged in as "admin"
    And I am at the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Search page
    Then I should see operating center "nj6"'s Description in the OperatingCenter dropdown
    And I should see operating center "nj7"'s Description in the OperatingCenter dropdown
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "cancelled"
    And I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "one"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "two"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "three"
    When I go to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Search page
    And I select operating center "nj6" from the OperatingCenter dropdown
    And I press Search
    Then I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "four"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "five"

Scenario: User can search multiple asset types and multiple work descriptions
    Given a work order "four" exists with operating center: "nj7", asset type: "hydrant", work description: "hydrant flushing", sap work order number: "1234567890", work order priority: "emergency", s o p required: "true"
    And a work order "five" exists with operating center: "nj7", asset type: "valve", work description: "valve replacement", sap work order number: "1234567890", work order priority: "emergency", s o p required: "true"
    And a work order "six" exists with operating center: "nj7", asset type: "hydrant", work description: "hydrant flushing", sap work order number: "1234567890", work order priority: "emergency", s o p required: "true"
    And a work order "seven" exists with operating center: "nj7", asset type: "valve", work description: "valve replacement", sap work order number: "1234567890", work order priority: "emergency", s o p required: "true"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Search page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "hydrant" from the AssetType dropdown
    And I select asset type "valve" from the AssetType dropdown
    And I select work description "hydrant flushing" from the WorkDescription dropdown
    And I select work description "valve replacement" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "four"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "five"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "six"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "seven"
    When I go to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "hydrant" from the AssetType dropdown
    And I select work description "hydrant flushing" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "four"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "six"
    And I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "five"
    And I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "seven"
    When I go to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "valve" from the AssetType dropdown
    And I select work description "valve replacement" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "five"
    And I should see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "seven"
    And I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "four"
    And I should not see a link to the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order "six"

#Initial Details
Scenario: User can view a work order
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "two"
    Then I should only see work order "two"'s Id in the WorkOrderId element
    And I should only see "TOWN" in the Town element
    And I should only see "Tucson" in the TownSection element
    And I should only see "Blah Blah Blah" in the CriticalMainBreakNotes element
    And I should only see "1234" in the StreetNumber element
    And I should only see street "one"'s FullStName in the Street element
    And I should only see "Testing Additional Apartment" in the ApartmentAddtl element
    And I should only see street "two"'s FullStName in the NearestCrossStreet element
    And I should only see "85023" in the ZipCode element
    And I should only see "Valve" in the AssetType element
    And I should see a link to the show page for valve "one"
    And I should only see "Customer" in the RequestedByDescription element
    And I should only see "Smith" in the RequestedBy element
    And I should only see "867-5309" in the PhoneNumber element
    And I should only see "123-456-7890" in the SecondaryPhoneNumber element
    And I should only see "Revenue >1000" in the Purpose element
    And I should only see "Emergency" in the Priority element
    And I should only see "VALVE REPAIR" in the WorkDescription element
    And I should only see "0-50" in the EstimatedCustomerImpact element
    And I should only see "4-6" in the AnticipatedRepairTime element
    And I should only see "Yes" in the AlertIssuedDisplay element
    And I should only see "Yes" in the SignificantTrafficImpact element
    And I should only see "Routine" in the MarkoutRequirement element
    And I should only see "123456789" in the AccountCharged element
    And the TrafficControlRequired field should be disabled
    And the TrafficControlRequired field should be unchecked
    And the StreetOpeningPermitRequired field should be disabled
    And the StreetOpeningPermitRequired field should be checked
    And the DigitalAsBuiltRequired field should be disabled
    And the DigitalAsBuiltRequired field should be unchecked
    And I should only see "hey this is a note" in the Notes element
    And I should only see "Roy" in the CreatedBy element
    And I should only see "Roy" in the CompletedBy element
    And I should only see "123456789" in the SAPNotificationNumber element
    And I should only see " " in the ApprovedOn element
    And I should only see "987654321" in the SAPWorkOrderNumber element
    And I should only see "3/15/2023 12:00:00 AM (EST)" in the MaterialsApprovedOn element
    And I should only see "Success" in the SAPErrorCode element
    And I should only see "3/18/2023 12:00:00 AM (EST)" in the MaterialPlanningCompletedOn element
    And I should not see a link to the FieldOperations/Service/LinkOrNew?workOrderId=2 page
    And I should not see a link to the FieldOperations/CrewAssignment/ShowCalendar?Crew=2 page

# Restoration
Scenario: Users can view restorations for a work order
    Given a restoration type "one" exists with description: "ASPHALT - ALLEY"
    And a restoration type "two" exists with description: "BRICK - STREET"
    And a restoration response priority "one" exists with description: "Emergency 5 day"
    And a restoration response priority "two" exists with description: "Priority (10 days)"
    And a restoration "one" exists with RestorationType: "two", PavingSquareFootage: "28.00", LinearFeetOfCurb: "6.00", WorkOrder: "two", PartialRestorationDate: "2001-11-16", FinalRestorationDate: "2000-12-31", ResponsePriority: "one", AssignedContractor: "one"
    And a restoration "two" exists with RestorationType: "one", PavingSquareFootage: "25.00", LinearFeetOfCurb: "7.00", WorkOrder: "two", PartialRestorationDate: "2000-12-31", FinalRestorationDate: "2003-09-12", ResponsePriority: "two", AssignedContractor: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "two"
    When I click the "Restoration" tab
    Then I should see the following values in the restorations table
    | Type of Restoration | Paving Square Footage | Linear Feet Of Curb | Initial Date     | Final Date    | Response Priority  | Assigned Contractor |
    | BRICK - STREET      | 28.00                 | 6.00                | 11/16/2001       | 12/31/2000    | Emergency 5 day    | Sunil               |
    | ASPHALT - ALLEY     | 25.00                 | 7.00                | 12/31/2000       | 9/12/2003     | Priority (10 days) | Sunil               |
    And I should see a link to the show page for restoration "one"
    And I should see a link to the edit page for restoration "one"
    And I should see a link to the show page for restoration "two"
    And I should see a link to the edit page for restoration "two"

# Additional
Scenario: Users can view additional tab for a work order
    Given a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
    And a crew assignment "one" exists with work order: "two", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "2"
    And a crew assignment "two" exists with work order: "two", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "3"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "two"
    When I click the "Additional" tab
    Then I should see a link to "Content/LeakageChart.pdf"
    And I should see a display for TotalManHours with "3"
    And I should see a display for LostWater with "5"
    And I should see a display for Notes with "hey this is a note"
    And I should see a display for DistanceFromCrossStreet with "12"
    And I should see the link "Finalization"
    And I should see the link "General"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Edit page for work order: "two"

# Street Opening Permit
Scenario: User should be not able to see street opening permit links for a work order
    Given a work order "five" exists with operating center: "nj6", town: "nj7burg", street: "one", asset type: "service", work description: "service line installation", work order priority: "emergency", sap work order number: "987654321", s o p required: "true"
    And a role "workorder-edit-nj6" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj6"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderStreetOpeningPermitProcessing/Show page for work order: "five"
    When I click the "Street Opening Permit" tab
    Then I should see "Add Street Opening Permit"
    And I should see "Submit New Permit"
    When I follow "Add Street Opening Permit"
    And I wait for the dialog to open
    And I press "Save Permit"
    Then I should see a validation message for StreetOpeningPermitNumber with "The Permit # field is required."
    And I should see a validation message for DateRequested with "The DateRequested field is required."
    When I type "1234567890" into the StreetOpeningPermitNumber field
    And I enter today's date into the DateRequested field
    And I press "Save Permit"
    And I wait for ajax to finish loading
    Then I should see the following values in the streetOpeningPermitsTable table
      | Permit #   | Date Requested |
      | 1234567890 | today          |
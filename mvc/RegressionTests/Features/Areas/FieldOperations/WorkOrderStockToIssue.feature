Feature: WorkOrderStockToIssue

Background: 
    # Disable SAP for operating centers for the test. The SAP integration is not going to work in the tests since we do
    # not have any ability to make test data in the SAP system. 
	Given an operating center "sap-disabled" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "false", sap work orders enabled: "false"
	And an operating center "sap-enabled" exists with opcode: "SAP", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And a town "nj7burg" exists with name: "TOWN"
    And an asset type "hydrant" exists with description: "hydrant"
    And operating center: "sap-enabled" has asset type "hydrant"
    And operating center: "sap-disabled" exists in town: "nj7burg"
    And a street "one" exists with town: "nj7burg", full st name: "EASY STREET", is active: true
    And a street "two" exists with town: "nj7burg", is active: true, full st name: "HIGH STREET"
    And an asset type "valve" exists with description: "valve"
    And operating center: "sap-disabled" has asset type "valve"
    And a valve "one" exists with operating center: "sap-disabled", town: "nj7burg", street: "one", turns: 15, date installed: "yesterday"
    And work order requesters exist
    And work order purposes exist
    And work order priorities exist
    And work descriptions exist
    And markout requirements exist
    And an asset type "sewer opening" exists with description: "sewer opening"
    And operating center: "sap-enabled" has asset type "sewer opening"
    And a user "user-admin" exists with username: "user-admin", full name: "Roy"
   	And a wildcard role exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user-admin"
    And a role exists with action: "Read", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-disabled"
    And a role exists with action: "Add", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-disabled"
    And a role exists with action: "Read", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-enabled"
    And a role exists with action: "Add", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-enabled"
    And a stock location "one" exists with SAPStockLocation: "2600", Description: "H&M"
    And a work order "one" exists with approved by: "user-admin", approved on: "today", operating center: "sap-disabled", town: "nj7burg", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "water main break repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user-admin", completed by: "user-admin", sap notification number: "123456789", sap work order number: "987654321", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", business unit: "Liberty", date completed: "03/22/2023", lost water: 5
    And a material "one" exists with PartNumber: "123456789", Description: "Plastic"
    And a material used "one" exists with material: "one", work order: "one", stock location: "one", quantity: 2

#Initial Details
Scenario: User admin can view a work order's initial details
    Given I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderStockToIssue/Show page for work order: "one"
    Then I should only see work order "one"'s Id in the WorkOrderId element
    And I should only see "TOWN" in the Town element
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
    And I should only see "Routine" in the Priority element
    And I should only see "WATER MAIN BREAK REPAIR" in the WorkDescription element
    And I should only see "0-50" in the EstimatedCustomerImpact element
    And I should only see "4-6" in the AnticipatedRepairTime element
    And I should only see "n/a" in the AlertIssuedDisplay element
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
    And I should only see "987654321" in the SAPWorkOrderNumber element
    And I should see "" in the MaterialsApprovedOn element
    And I should only see "Success" in the SAPErrorCode element
    And I should only see "3/18/2023 12:00:00 AM (EST)" in the MaterialPlanningCompletedOn element

Scenario: User admin cannot edit a work order or see various links
    Given I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderStockToIssue/Show page for work order: "one"
    Then I should not see a link to the edit page for work order: "one"
    And I should not see the link "SAP Notifications"
    And I should not see the button "Cancel Order"
    And I should not see the button "Complete Material Planning"
    And I should not see the link "Create Service"

# Materials
Scenario: User admin can view materials for a work order
    Given a material "two" exists with PartNumber: "987654321", Description: "Copper"
    And a stock location "two" exists with SAPStockLocation: "1300", Description: "STYD"
    And a material used "two" exists with Material: "two", WorkOrder: "one", StockLocation: "two", Quantity: 5
    And a material used "three" exists with WorkOrder: "one", StockLocation: "two", Quantity: 7, NonStockDescription: "Test"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderStockToIssue/Show page for work order: "one"
    When I click the "Materials" tab
    Then I should see the following values in the materials table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |
    | 987654321   | STYD           | 1300               | Copper      | 5        |
    | N/A         | STYD           | 1300               |  Test       | 7        |
    And I should see a display for MaterialsDocID with "122333444"
    And I should see a display for MaterialPostingDate with ""

Scenario: User admin can approve materials for a work order
    Given I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderStockToIssue/Show page for work order: "one"
    When I click the "Materials" tab
    Then I should see the following values in the materials table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |
    And I should see a display for MaterialsDocID with "122333444"
    When I enter "7/3/2023" into the MaterialPostingDate field
    And I press approve-stock-to-issue-button
    And I click the "Materials" tab
    Then I should see a display for MaterialPostingDate with "7/3/2023"
    And I should see a display for MaterialsApprovedBy with "Roy"

Scenario: User admin should not see approve button if the work order already has materials approved
    Given a work order "two" exists with materials approved by: "user-admin", materials approved on: "today", approved by: "user-admin", approved on: "today", operating center: "sap-disabled", town: "nj7burg", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "water main break repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user-admin", completed by: "user-admin", sap notification number: "123456789", sap work order number: "987654321", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", business unit: "Liberty", date completed: "03/22/2023"
    And a material used "two" exists with material: "one", work order: "two", stock location: "one", quantity: 2
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderStockToIssue/Show page for work order: "two"
    When I click the "Materials" tab
    Then I should not see the approve-stock-to-issue-button element

# Additional
Scenario: User admin can view additional tab for a work order
    Given a crew "one" exists with description: "Sunil", operating center: "sap-disabled", active: true
    And a crew "two" exists with description: "Sai", operating center: "sap-disabled", active: true
    And a crew assignment "one" exists with work order: "one", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "2"
    And a crew assignment "two" exists with work order: "one", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "3"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderStockToIssue/Show page for work order: "one"
    When I click the "Additional" tab
    Then I should see a link to "Content/LeakageChart.pdf"
    And I should see a display for TotalManHours with "3"
    And I should see a display for LostWater with "5"
    And I should see a display for Notes with "hey this is a note"
    And I should see a display for DistanceFromCrossStreet with ""
    And I should see the link "Finalization"
    And I should see the link "General"
    # This is an insanely fragile test that relies on us knowing the id value of the work order being tested.
    # I'm not gonna try to work around this and make it right when that effort will just get removed when 271 goes away.
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "one"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Edit page for work order: "one"

Scenario: user can view and not edit hydrant details
    Given a hydrant "hydrant" exists with street: "one", town: "nj7burg", operating center: "sap-enabled", hydrant suffix: "42" 
    And a work order "hydrant" exists with operating center: "sap-enabled", town: "nj7burg", street: "one", asset type: "hydrant", hydrant: "hydrant"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "hydrant"
    When I click the "Hydrant" tab
    Then I should not see the hydrantEditButton element 
    When I switch to the hydrantFrame frame
    Then I should see a display for HydrantSuffix with "42"

Scenario: user can view and edit hydrant details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-enabled"
    And a hydrant "hydrant" exists with street: "one", town: "nj7burg", operating center: "sap-enabled", hydrant suffix: "42", hydrant number: "HAB-42" 
    And a work order "hydrant" exists with operating center: "sap-enabled", town: "nj7burg", street: "one", asset type: "hydrant", hydrant: "hydrant"
    And a functional location "one" exists with town: "nj7burg", asset type: "hydrant"
    And a fire district "one" exists with district name: "meh"
    And a fire district town "foo" exists with town: "nj7burg", fire district: "one"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "hydrant"
    When I click the "Hydrant" tab
    When I press "hydrantEditButton"
    And I switch to the hydrantFrame frame
    And I select fire district "one" from the FireDistrict dropdown
    And I select functional location "one" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a display for FunctionalLocation with functional location "one"

Scenario: user can view and not edit valve details
    Given I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderStockToIssue/Show page for work order: "one"
    When I click the "Valve" tab
    Then I should not see the valveEditButton element 
    When I switch to the valveFrame frame
    Then I should see a display for ValveType with ""

Scenario: user can view and edit valve details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-disabled"
    And a valve type "one" exists with description: "GATE"
    And a valve normal position "one" exists with description: "CLOSED"
    And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderStockToIssue/Show page for work order: "one"
    When I click the "Valve" tab
    When I press "valveEditButton"
    And I switch to the valveFrame frame
    And I press Save
    Then I should see a validation message for ValveType with "Valve Type is required for active / installed valves." 
    When I select valve type "one" from the ValveType dropdown
    And I select valve normal position "one" from the NormalPosition dropdown
    And I enter "208" into the ValveSuffix field
    And I enter "VAB-208" into the ValveNumber field
    And I press Save
    Then I should see a display for ValveType with valve type "one"
    And I should see a display for NormalPosition with valve normal position "one"

Scenario: user can view and not edit sewer opening details
    Given a sewer opening "opening" exists with operating center: "sap-enabled", town: "nj7burg", street: "one", opening number: "MAD-42"
    And a work order "opening" exists with operating center: "sap-enabled", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Sewer Opening" tab
    Then I should not see the sewerOpeningEditButton element 
    When I switch to the sewerOpeningFrame frame
    Then I should see a display for OpeningNumber with "MAD-42"

Scenario: user can view and edit sewer opening details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-enabled"
    And a body of water "one" exists with name: "crystal lake", operating center: "sap-enabled"
    And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
    And a sewer opening "opening" exists with operating center: "sap-enabled", town: "nj7burg", street: "one", opening number: "MAD-42", body of water: "one"
    And a work order "opening" exists with operating center: "sap-enabled", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening"
    And a functional location "one" exists with town: "nj7burg", asset type: "sewer opening"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Sewer Opening" tab
    When I press "sewerOpeningEditButton"
    And I switch to the sewerOpeningFrame frame
    And I select functional location "one" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a display for FunctionalLocation with functional location "one"

Scenario: UserAdmin can view and not edit service details
    Given a sewer opening "opening" exists with operating center: "sap-enabled", town: "nj7burg", street: "one", opening number: "MAD-42"
	And a service category "one" exists with description: "Neato"
    And a service "unique" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one"
    And a work order "opening" exists with operating center: "sap-enabled", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening", service: "unique"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Service" tab
    Then I should not see the serviceEditButton element 
    When I switch to the serviceFrame frame
    Then I should see a display for ServiceType with service "unique"'s ServiceType

Scenario: UserAdmin can view and edit service details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-disabled"
    And a sewer opening "opening" exists with operating center: "sap-disabled", town: "nj7burg", street: "one", opening number: "MAD-42"
    And a service category "one" exists with description: "Neato"
    And a service "unique" exists with service number: "123456", date installed: "4/24/1984", service category: "one"
    And a work order "opening" exists with operating center: "sap-disabled", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening", service: "unique"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Service" tab
    When I press "serviceEditButton"
    When I switch to the serviceFrame frame
    And I select town "nj7burg" from the Town dropdown
    And I follow "Cancel"
    Then I should see a display for ServiceNumber with "123456"
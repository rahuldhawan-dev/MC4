Feature: GeneralWorkOrderPage

Background: 
    # The name of the state *needs* to be New Jersey for the permits stuff.
    # StateFactory checks for uniqueness based on the abbreviation and will return
    # an existing State instead, which usually has a number appended to the default
    # state name.
    Given a state "nj" exists with name: "New Jersey", abbreviation: "NJ_DO_NOT_CHANGE!"
    And a county "monmouth" exists with name: "MONMOUTH", state: "nj"
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e"
    And an operating center "nj6" exists with opcode: "NJ6", name: "Short Hills"
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
    And a user "readonly-user" exists with username: "readonly-user", default operating center: "nj7", full name: "Smith"
	And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-edit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-read-readonly-user" exists with action: "Read", module: "FieldServicesWorkManagement", user: "readonly-user", operating center: "nj7"
    And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a role "asset-add" exists with action: "Add", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a work order "one" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "one"
    And a work order "two" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "valve leaking", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", approved on: "03/12/2023", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", approved by: "user", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", date completed: "03/22/2023", alert issued: true, lost water: 5
    And a work order "three" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "sewer main", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "sewer main overflow", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "none", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", approved on: "03/12/2023", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", approved by: "user", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user"
    And a work order "workorder-pitcher-filter-delivered" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "sewer main overflow", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "none", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", approved on: "03/12/2023", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", approved by: "user", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", has pitcher filter been provided to customer: true, date pitcher filter delivered to customer: "8/1/2023"
    And a work order "cancelled" exists with operating center: "nj7", cancelled at: "11/11/2016"
    And a role "markoutviolation-add" exists with action: "Add", module: "BPUGeneral", user: "user", operating center: "nj7"

# Search/Index
Scenario: User can search for work orders
	Given I am logged in as "user"
	And I am at the FieldOperations/GeneralWorkOrder/Search page
    Then I should not see operating center "nj6"'s Description in the OperatingCenter dropdown
    And I should see operating center "nj7"'s Description in the OperatingCenter dropdown
	When I press Search
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "cancelled"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "one"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "three"

Scenario: User can search for a work order where pitcher filter has been delivered
    Given I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Search page
    When I select "Yes" from the HasPitcherFilterBeenProvidedToCustomer dropdown
    And I press Search
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "workorder-pitcher-filter-delivered"
    When I visit the FieldOperations/GeneralWorkOrder/Search page
    And I enter "8/1/2023" into the DatePitcherFilterDeliveredToCustomer_Start field
    And I enter "8/1/2023" into the DatePitcherFilterDeliveredToCustomer_End field
    And I press Search
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "workorder-pitcher-filter-delivered"

Scenario: User sees SAP Error Code on show page
    Given a work order "sap error" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "one", s a p error code: "RETRY::UPDATE FAILURE Initial Import"
    And I am logged in as "user"
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "sap error"
    Then I should see the error message "RETRY::UPDATE FAILURE Initial Import"

Scenario: User can search multiple asset types and multiple work descriptions
    Given a work order "four" exists with operating center: "nj7", asset type: "hydrant", work description: "hydrant flushing"
    And a work order "five" exists with operating center: "nj7", asset type: "valve", work description: "valve replacement"
    And a work order "six" exists with operating center: "nj7", asset type: "hydrant", work description: "hydrant flushing"
    And a work order "seven" exists with operating center: "nj7", asset type: "valve", work description: "valve replacement"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Search page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "hydrant" from the AssetType dropdown
    And I select asset type "valve" from the AssetType dropdown
    And I select work description "hydrant flushing" from the WorkDescription dropdown
    And I select work description "valve replacement" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "four"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "five"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "six"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "seven"
    When I go to the FieldOperations/GeneralWorkOrder/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "hydrant" from the AssetType dropdown
    And I select work description "hydrant flushing" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "four"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "six"
    And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "five"
    And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "seven"
    When I go to the FieldOperations/GeneralWorkOrder/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "valve" from the AssetType dropdown
    And I select work description "valve replacement" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "five"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "seven"
    And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "four"
    And I should not see a link to the FieldOperations/GeneralWorkOrder/Show page for work order "six"

Scenario: Site admin can edit sap fields for work orders
	Given I am logged in as "admin"
	And I am at the FieldOperations/GeneralWorkOrder/Search page
	When I press Search
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "cancelled"
	When I click the "(edit)" link in the 5th row of workOrdersTable
	And I wait for the page to reload
	And I enter "123" into the SAPNotificationNumber field
	And I enter "4321" into the SAPWorkOrderNumber field
	And I press Save 	
	Then I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "cancelled"

Scenario: Site admin can clear SupervisorApprovedOn and PreviouslyApprovedBy from a work order
	Given I am logged in as "admin"
	And I am at the FieldOperations/GeneralWorkOrder/Search page
	When I press Search
    Then I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "three"
	When I click the "(edit)" link in the 3rd row of workOrdersTable
	And I wait for the page to reload
    And I select "-- Select --" from the ApprovedBy dropdown
    And I enter "" into the ApprovedOn field
	And I press Save	
	Then I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "three"

#Initial Details
Scenario: User can view a work order
    Given I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
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
    And I should only see "Routine" in the Priority element
    And I should only see "VALVE LEAKING" in the WorkDescription element
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
    And the DigitalAsBuiltCompletedCheckbox field should be disabled
    And the DigitalAsBuiltCompletedCheckbox field should be unchecked
    And I should only see "hey this is a note" in the Notes element
    And I should only see "Roy" in the CreatedBy element
    And I should only see "Roy" in the CompletedBy element
    And I should only see "123456789" in the SAPNotificationNumber element
    And I should only see "3/12/2023 12:00:00 AM (EST)" in the ApprovedOn element
    And I should only see "987654321" in the SAPWorkOrderNumber element
    And I should only see "3/15/2023 12:00:00 AM (EST)" in the MaterialsApprovedOn element
    And I should only see "Success" in the SAPErrorCode element
    And I should only see "3/18/2023 12:00:00 AM (EST)" in the MaterialPlanningCompletedOn element
    And I should see the link "SAP Notifications"
    And I should not see the button "Cancel Order"
    And I should not see the button "Complete Material Planning"
    And I should not see a link to the FieldOperations/Service/LinkOrNew?workOrderId=2 page
    When I follow "Edit"
    Then I should be at the /FieldOperations/GeneralWorkOrder/Edit page for work order: "two"

Scenario: User cannot edit a work order
    Given I am logged in as "readonly-user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    Then I should not see a link to the edit page for work order: "two"

# Materials
Scenario: Users can view materials for a work order
    Given a material "one" exists with PartNumber: "123456789", Description: "Plastic"
    And a material "two" exists with PartNumber: "987654321", Description: "Copper"
    And a stock location "one" exists with SAPStockLocation: "2600", Description: "H&M"
    And a stock location "two" exists with SAPStockLocation: "1300", Description: "STYD"
    And a material used "one" exists with Material: "one", WorkOrder: "two", StockLocation: "one", Quantity: 2
    And a material used "two" exists with Material: "two", WorkOrder: "two", StockLocation: "two", Quantity: 5
    And a material used "three" exists with WorkOrder: "two", StockLocation: "two", Quantity: 7, NonStockDescription: "Test"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Materials" tab
    Then I should see the following values in the materials table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |
    | 987654321   | STYD           | 1300               | Copper      | 5        |
    | N/A         | STYD           | 1300               |  Test       | 7        |
    And I should see a display for MaterialsDocID with "122333444"
    And I should see a display for WorkOrderApprovedBy with "Roy"
    And I should see a display for MaterialPostingDate with "3/10/2023"
    And I should see a display for MaterialsApprovedBy with "Roy"

# Spoils
Scenario: Users can view spoils for a work order
    Given a spoil storage location "one" exists with Name: "Newman Springs Yard"
    And a spoil storage location "two" exists with Name: "Union Beach Yard"
    And a spoil "one" exists with SpoilStorageLocation: "two", WorkOrder: "two", Quantity: 1.25
    And a spoil "two" exists with SpoilStorageLocation: "one", WorkOrder: "two", Quantity: 2.75
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Spoils" tab
    Then I should see the following values in the spoils table
    | Quantity (CY) | Spoil Storage Location |
    | 1.25          | Union Beach Yard       |
    | 2.75          | Newman Springs Yard    |

# Markouts
Scenario: Users can view markouts for a work order
    Given a markout type "one" exists with description: "C TO C"
    And a markout type "two" exists with description: "None"
    And a markout "one" exists with MarkoutType: "two", WorkOrder: "two", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with MarkoutType: "one", WorkOrder: "two", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Markouts" tab
    Then I should see the following values in the markoutsTable table
    | Markout Type    | Date Of Request    | Ready Date           | Expiration Date       |
    | NOT LISTED      | 2/20/2023          | 2/21/2023 12:00 AM   |  2/28/2023 12:00 AM   |
    | C TO C          | 3/10/2023          | 3/12/2023 12:00 AM   |  3/19/2023 12:00 AM   |
    And I should not see "Edit" in the table markoutsTable's "Action" column
    And I should not see "Delete" in the table markoutsTable's "Action" column

Scenario: Users can delete markouts for a work order which is not approved
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "valve leaking", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", date completed: "03/22/2023", alert issued: true, lost water: 5
    And a markout type "one" exists with description: "C TO C"
    And a markout type "two" exists with description: "None"
    And a markout "one" exists with MarkoutType: "two", WorkOrder: "ten", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with MarkoutType: "one", WorkOrder: "ten", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "ten"
    When I click the "Markouts" tab
    Then I should see the following values in the markoutsTable table
    | Markout Type    | Date Of Request    | Ready Date           | Expiration Date       |
    | NOT LISTED      | 2/20/2023          | 2/21/2023 12:00 AM   |  2/28/2023 12:00 AM   |
    | C TO C          | 3/10/2023          | 3/12/2023 12:00 AM   |  3/19/2023 12:00 AM   |
    When I click the "Delete" link in the 1st row of markoutsTable and then click ok in the confirmation dialog
    And I wait for ajax to finish loading
    Then I should see the following values in the markoutsTable table
    | Markout Type    | Date Of Request    | Ready Date           | Expiration Date       |
    | C TO C          | 3/10/2023          | 3/12/2023 12:00 AM   |  3/19/2023 12:00 AM   |

Scenario: Users can create new markouts for a work order
    Given a markout type "one" exists with description: "C TO C"
    And a markout type "two" exists with description: "None"
    And a markout "one" exists with MarkoutType: "two", WorkOrder: "two", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with MarkoutType: "one", WorkOrder: "two", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Markouts" tab
    And I follow "Add New Markout"
    And I wait for the dialog to open
    And I enter "9876543210" into the MarkoutNumber field
    And I select markout type "two" from the MarkoutType dropdown
    And I press "Save Markout"
    Then I should see a validation message for Note with "Required."
    And I should see a validation message for MarkoutNumber with "Must be 9 or more characters."
    When I enter "987654321" into the MarkoutNumber field
    And I enter "123456789" into the Note field
    And I press "Save Markout"
    Then I should see a validation message for Note with "Please enter valid notes for the Markout Type."
    When I enter "1234567890" into the Note field
    And I press "Save Markout"
    Then I should see today's date in the table markoutsTable's "Date Of Request" column 
    And I should see the following values in the markoutsTable table
    | Markout Type | Date Of Request | Note       |
    | NOT LISTED   | 2/20/2023       |            |
    | C TO C       | 3/10/2023       |            |
    | NOT LISTED   | today           | 1234567890 |

Scenario: Users can create new markouts for a work order with markout editable operating center
    Given an operating center "ca30" exists with opcode: "CA30", name: "San Diego", sap enabled: "true", sap work orders enabled: "true", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", MarkoutsEditable: true
    And a work order "new" exists with operating center: "ca30", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "water main break repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", approved on: "03/12/2023", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", approved by: "user", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", date completed: "03/22/2023"
    And a role "workorder-read-ca30" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "ca30"
	And a role "workorder-add-ca30" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "ca30"
	And a markout type "one" exists with description: "C TO C"
    And a markout type "two" exists with description: "None"
    And a markout "one" exists with MarkoutType: "two", WorkOrder: "new", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with MarkoutType: "one", WorkOrder: "new", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "new"
    When I click the "Markouts" tab
    And I follow "Add New Markout"
    And I wait for the dialog to open
    And I enter "987654321" into the MarkoutNumber field
    And I select markout type "two" from the MarkoutType dropdown
    And I press "Save Markout"
    Then I should see a validation message for Note with "Required."
    When I enter "123456789" into the Note field
    And I press "Save Markout"
    Then I should see a validation message for ReadyDate with "The ReadyDate field is required."
    And I should see a validation message for ExpirationDate with "The ExpirationDate field is required."
    When I enter "05/11/2023" into the ReadyDate field
    And I enter "05/21/2023" into the ExpirationDate field
    And I press "Save Markout"
    Then I should see a validation message for Note with "Please enter valid notes for the Markout Type."
    When I enter "1234567890" into the Note field
    And I press "Save Markout"
    Then I should see today's date in the table markoutsTable's "Date Of Request" column
    And I should see "5/11/2023 12:00 AM" in the table markoutsTable's "Ready Date" column
    And I should see "5/21/2023 12:00 AM" in the table markoutsTable's "Expiration Date" column
    And I should see the following values in the markoutsTable table
    | Markout Type | Date Of Request | Note       |
    | NOT LISTED   | 2/20/2023       |            |
    | C TO C       | 3/10/2023       |            |
    | NOT LISTED   | today           | 1234567890 |
    

# Markout Violations
Scenario: Users can view markout violations for a work order
    Given a markout violation "one" exists with DateOfViolationNotice: "2019-02-05 00:00:00.000", MarkoutViolationStatus: "Pending review - Jeff Bowlby", MarkoutRequestNumber: "162022169", WorkOrder: "two"
    And a markout violation "two" exists with DateOfViolationNotice: "2018-06-28 00:00:00.000", MarkoutViolationStatus: "Completed", MarkoutRequestNumber: "190740395", WorkOrder: "two"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Markout Violations" tab
    Then I should see the following values in the markoutViolations table
    | Date Of Violation Notice    | Markout Violation Status       | Markout Request Number          |
    | 2/5/2019                    | Pending review - Jeff Bowlby   |   162022169                     |
    | 6/28/2018                   | Completed                      |   190740395                     |
    When I follow "Create Markout Violation"
    Then I should be at the BPU/MarkoutViolation/New page

# Markout Damages
Scenario: Users can view markout damages for a work order
    Given a sewer markout damage utility damage type "sewer" exists
    And a water markout damage utility damage type "water" exists
    And a gas markout damage utility damage type "gas" exists 
    And a markout damage "one" exists with DamageOn: "2019-02-05", RequestNumber: "123456789", Excavator: "PSE&G", WorkOrder: "two"
    And a markout damage "two" exists with DamageOn: "2018-06-28", RequestNumber: "987654321", Excavator: "J F KIELY", WorkOrder: "two"
    And markout damage "one" has sewer markout damage utility damage type "sewer"
    And markout damage "two" has water markout damage utility damage type "water"
    And markout damage "two" has gas markout damage utility damage type "gas"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Markout Damages" tab
    Then I should see the following values in the markoutDamages table
    | Request Number | Damaged On Date    | Excavator | Utilities Damaged |
    | 123456789      | 2/5/2019 12:00 AM  | PSE&G     | Sewer             |
    | 987654321      | 6/28/2018 12:00 AM | J F KIELY | Water, Gas        |
    And I should see a link to the /FieldOperations/MarkoutDamage/New/2 page    

# Sewer Overflows
Scenario: Users can view sewer overflows for a work order
    Given a sewer clearing method "one" exists with description: "Power Snake"
    And a sewer clearing method "two" exists with description: "Water Jet"
    And a sewer overflow area "one" exists with description: "Manhole"
    And a sewer overflow area "two" exists with description: "Vac Truck"
    And a zone type "one" exists with description: "Business"
    And a zone type "two" exists with description: "Residential"
    And a sewer overflow reason "one" exists with description: "Grease"
    And a sewer overflow reason "two" exists with description: "Roots"
    And a sewer overflow "one" exists with SewerClearingMethod: "one", AreaCleanedUpTo: "one", ZoneType: "one", EnforcingAgencyCaseNumber: "10-25314", IncidentDate: "2022-04-07", WorkOrder: "three"
    And a sewer overflow "two" exists with SewerClearingMethod: "two", AreaCleanedUpTo: "two", ZoneType: "two", EnforcingAgencyCaseNumber: "22-02-13-1314-16", IncidentDate: "2022-03-31", WorkOrder: "three"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    Then I should not see the "Sewer Overflows" tab
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "three"
    Then I should see the "Sewer Overflows" tab
    When I click the "Sewer Overflows" tab
    Then I should see a link to the show page for sewer overflow "one"
    And I should see a link to the show page for sewer overflow "two"
    And I should see a link to the edit page for sewer overflow "one"
    And I should see a link to the edit page for sewer overflow "two"
	And I should see the following values in the sewerOverflows table
    | Sewer Clearing Method | Area Cleaned Up To | Zone Type   | Enforcing Agency Case Number | Incident Date          |
    | Power Snake           | Manhole            | Business    | 10-25314                     | 4/7/2022 12:00:00 AM   |
    | Water Jet             | Vac Truck          | Residential | 22-02-13-1314-16             | 3/31/2022 12:00:00 AM  |

# Traffic Control
Scenario: Users can view traffic control tickets for a work order
    Given a billing party "one" exists with Description: "ATLANTIC County"
    And a billing party "two" exists with Description: "ALLEGHENY County"
    And a traffic control ticket "one" exists with WorkStartDate: "2023-02-10", TotalHours: "1.85", NumberOfOfficers: "4", WorkOrder: "two", BillingParty: "two"
    And a traffic control ticket "two" exists with WorkStartDate: "2023-03-28", TotalHours: "3.15", NumberOfOfficers: "9", WorkOrder: "two", BillingParty: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Traffic Control" tab
    Then I should see a display for TrafficControlRequired with "No"
    And I should see a display for Notes with "hey this is a note"
    And I should see the following values in the trafficeControlTickets table
    | Work Start Date    | Total Combined Hours | Number of Officers or Employees | Billing Party      |
    | 2/10/2023          | 1.85                 | 4                               | ALLEGHENY County   |
    | 3/28/2023          | 3.15                 | 9                               | ATLANTIC County    |
    And I should see a link to the /FieldOperations/TrafficControlTicket/New?workOrderId=2 page

# Restoration
Scenario: Users can view restorations for a work order
    Given a restoration type "one" exists with description: "ASPHALT - ALLEY"
    And a restoration type "two" exists with description: "BRICK - STREET"
    And a restoration response priority "one" exists with description: "Emergency 5 day"
    And a restoration response priority "two" exists with description: "Priority (10 days)"
    And a contractor "one" exists with name: "Sunil", is active: "true", awr: "true"
    And a restoration "one" exists with RestorationType: "two", PavingSquareFootage: "28.00", LinearFeetOfCurb: "6.00", WorkOrder: "two", PartialRestorationDate: "2001-11-16", FinalRestorationDate: "2000-12-31", ResponsePriority: "one", AssignedContractor: "one"
    And a restoration "two" exists with RestorationType: "one", PavingSquareFootage: "25.00", LinearFeetOfCurb: "7.00", WorkOrder: "two", PartialRestorationDate: "2000-12-31", FinalRestorationDate: "2003-09-12", ResponsePriority: "two", AssignedContractor: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Restoration" tab
    Then I should see the following values in the restorations table
    | Type of Restoration | Paving Square Footage | Linear Feet Of Curb | Initial Date     | Final Date    | Response Priority  | Assigned Contractor |
    | BRICK - STREET      | 28.00                 | 6.00                | 11/16/2001       | 12/31/2000    | Emergency 5 day    | Sunil               |
    | ASPHALT - ALLEY     | 25.00                 | 7.00                | 12/31/2000       | 9/12/2003     | Priority (10 days) | Sunil               |
    And I should see a link to the show page for restoration "one"
    And I should see a link to the edit page for restoration "one"
    And I should see a link to the show page for restoration "two"
    And I should see a link to the edit page for restoration "two"
    
# Crew Assignments
Scenario: Users can view crew assignments for a work order
	Given a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
	And a crew assignment "one" exists with work order: "two", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "42"
    And a crew assignment "two" exists with work order: "two", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "12"
    And I am logged in as "user"
	And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
	When I click the "Crew Assignments" tab
	Then I should see the following values in the assignmentsTable table
    | Assigned On             | Assigned For             | Assigned To  | Start Time                  | End Time                    | Employees on Crew|
    | 1/11/2023 1:59 PM (EST) | 1/29/2023 3:00 AM (EST)  | Sunil        | 4/10/2022 2:22:00 PM (EST)  | 4/10/2022 3:07:00 PM (EST)  | 42               |
    | 3/16/2023 2:06 PM (EST) | 3/29/2023 12:00 AM (EST) | Sai          | 4/8/2022 11:38:00 PM (EST)  | 4/9/2022 12:49:00 AM (EST)  | 12               |

# Additional
Scenario: Users can view additional tab for a work order
    Given a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
    And a crew assignment "one" exists with work order: "two", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "2"
    And a crew assignment "two" exists with work order: "two", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "3"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Additional" tab
    Then I should see a link to "Content/LeakageChart.pdf"
    And I should see a display for TotalManHours with "3"
    And I should see a display for LostWater with "5"
    And I should see a display for Notes with "hey this is a note"
    And I should see a display for DistanceFromCrossStreet with ""
    And I should see the link "Finalization"
    And I should see the link "General"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Edit page for work order: "two"

# Account
Scenario: Users can view account tab for a work order
    Given I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "one"
    Then I should not see the "Account" tab
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    And I click the "Account" tab
    Then I should see a display for AccountCharged with "123456789"
    And I should see a display for ApprovedBy_FullName with user "user"'s FullName

Scenario: User can view special instruction on work order show page
    Given I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "one"
    Then I should only see work order "one"'s SpecialInstructions in the WorkOrderSpecialInstructions element

Scenario: User can view special instruction on work order Edit page
    Given I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "two"
    Then I should only see work order "two"'s SpecialInstructions in the WorkOrderSpecialInstructions element

# Schedule of Values
Scenario: User can view schedule of values for a work order
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", sap enabled: "true", sap work orders enabled: "true", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a town section "belmar" exists with town: "wall", name: "West Belmar"
    And a street "ace" exists with town: "wall", full st name: "ACE DRIVE", is active: true
    And a work order "four" exists with operating center: "nj4", town: "wall", street: "ace", asset type: "valve", valve: "one"
    And a schedule of value category "one" exists with description: "Saw Cut"
    And a schedule of value category "two" exists with description: "Curb Box"
    And a unit of measure "one" exists with description: "Gallons"
    And a unit of measure "two" exists with description: "HR"
    And a schedule of value "one" exists with ScheduleOfValueCategory: "one", Description: "Cold Patch", UnitOfMeasure: "one"
    And a schedule of value "two" exists with ScheduleOfValueCategory: "two", Description: "Partial Excavation", UnitOfMeasure: "two"
    And a work order schedule of value "one" exists with WorkOrder: "four", ScheduleOfValue: "one", OtherDescription: "Chem pump parts", IsOvertime: "true"
    And a work order schedule of value "two" exists with WorkOrder: "four", ScheduleOfValue: "two", OtherDescription: "Total cost of repair by contractor", IsOvertime: "false"
    And a role "workorder-read-nj4" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    Then I should not see the "Schedule Of Values" tab
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "four"
    Then I should see the "Schedule Of Values" tab
    When I click the "Schedule Of Values" tab
    Then I should see the following values in the scheduleOfValues table
    | Schedule Of Value Category | Schedule Of Value  | Other Description                  | Unit Of Measure | Is Overtime |
    | Saw Cut                    | Cold Patch         | Chem pump parts                    | Gallons         | Yes         |
    | Curb Box                   | Partial Excavation | Total cost of repair by contractor | HR              | No          |

# Requisitions
Scenario: Users can view requisitions for a work order
    Given a requisition type "one" exists with description: "Paving"
    And a requisition type "two" exists with description: "Spoils"
    And a requisition "one" exists with work order: "two", SAPRequisitionNumber: "what is this", RequisitionType: "one"
    And a requisition "two" exists with work order: "two", SAPRequisitionNumber: "123456", RequisitionType: "two"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Purchase Orders(PO)" tab
    Then I should see the following values in the requisitions table
    | Purchase Order (PO) Type | SAP Purchase Order(PO) # |
    | Paving                   | what is this             |
    | Spoils                   | 123456                   |

# Job Site Check Lists
Scenario: Users can view job site check lists for a work order
    Given a job site check list "one" exists with map call work order: "two", CheckListDate: "01/22/2023", CreatedBy: "Sunil"
    And a job site check list "two" exists with map call work order: "two", CheckListDate: "03/21/2023", CreatedBy: "Sai"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Job Site Check Lists" tab
    Then I should see the following values in the jobSiteCheckLists table
    | Check List Date | Created By |
    | 1/22/2023       | Sunil      |
    | 3/21/2023       | Sai        |
    And I should see a link to the show page for job site check list "one"
    And I should see a link to the show page for job site check list "two"

# Job Observations
Scenario: Users can view job observations for a work order
    Given I am logged in as "user"
    And a overall safety rating "one" exists with description: "Satisfactory"
    And a overall safety rating "two" exists with description: "Unsatisfactory"
    And a overall quality rating "one" exists with description: "Satisfactory"
    And a overall quality rating "two" exists with description: "Unsatisfactory"
    And a job observation "one" exists with work order: "two", ObservationDate: "01/22/2023", OverallSafetyRating: "one", OverallQualityRating: "two"
	And a job observation "two" exists with work order: "two", ObservationDate: "03/21/2023", OverallSafetyRating: "two", OverallQualityRating: "one"
	And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    When I click the "Job Observations" tab
    Then I should see a link to the /HealthAndSafety/JobObservation/New/2 page
    And I should see the following values in the jobObservations table
    | Observation Date  | Overall Safety Rating | Overall Quality Rating |
    | 1/22/2023         | Satisfactory          |    Unsatisfactory      |
    | 3/21/2023         | Unsatisfactory        |     Satisfactory       |
    And I should see a link to the show page for job observation "one"
    And I should see a link to the show page for job observation "two"
    
# Invoices
Scenario: User can view invoices for a work order
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", sap enabled: "true", sap work orders enabled: "true", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a town section "belmar" exists with town: "wall", name: "West Belmar"
    And a street "ace" exists with town: "wall", full st name: "ACE DRIVE", is active: true
    And a work order "four" exists with operating center: "nj4", town: "wall", street: "ace", asset type: "valve", valve: "one"
    And a work order invoice "one" exists with WorkOrder: "four", InvoiceDate: "02/15/2023", SubmittedDate: "02/12/2023", CanceledDate: "02/16/2023", IncludeMaterials: true
    And a work order invoice "two" exists with WorkOrder: "four", InvoiceDate: "03/15/2023", SubmittedDate: "03/12/2023", CanceledDate: "03/16/2023", IncludeMaterials: false
    And a role "workorder-read-nj4" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "two"
    Then I should not see the "Invoices" tab
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "four"
    Then I should see the "Invoices" tab
    When I click the "Invoices" tab
    Then I should see the following values in the invoices table
    | Invoice Date | Submitted Date | Canceled Date | Include Materials |
    | 2/15/2023    | 2/12/2023      |  2/16/2023    | Yes               |
    | 3/15/2023    | 3/12/2023      |  3/16/2023    | No                |

# Meters
Scenario: User can view meters for a work order
    Given a work order "five" exists with operating center: "nj7", asset type: "service", work description: "install meter"
    And a service installation "one" exists with work order: "five", MeterManufacturerSerialNumber: "1234567890", MeterLocationInformation: "9876543210"
    And a service installation "two" exists with work order: "five", MeterManufacturerSerialNumber: "9876543210", MeterLocationInformation: "1234567890"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "five"
    Then I should see the "Set Meter" tab
    When I click the "Set Meter" tab
    Then I should see the following values in the meters table
    | Id | Meter Manufacturer Serial Number | Meter Location Information |
    | 1  | 1234567890                       |  9876543210                |
    | 2  | 9876543210                       |  1234567890                |
    And I should see a link to the show page for service installation "one"
    And I should see a link to the show page for service installation "two"

Scenario: User can view links for a work order
    Given a work order cancellation reason "one" exists with description: "Customer Request"
    And a work order cancellation reason "two" exists with description: "Company Error"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "one"
    Then I should see the button "Complete Material Planning"
    And I should see the button "btnCancelOrder"
    When I press "btnCancelOrder"
    And I select work order cancellation reason "one" from the WorkOrderCancellationReason dropdown
    And I click ok in the alert after pressing btnSubmit    
    Then I should be at the FieldOperations/GeneralWorkOrder/Show page for work order "one"

Scenario: User can view links for service work order
    Given a service "linked" exists with premise number: "000000000", operating center: "nj7", town: "nj7burg"
    And a work order "five" exists with operating center: "nj7", asset type: "service", town: "nj7burg", premise number: "000000000"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "five"
    Then I should see "Create Service"
    When I follow "Create Service"
    Then I should be at the FieldOperations/Service/LinkOrNew page

Scenario: User sees cancellation information on show page
    Given a user "the chancellor of cancellation" exists
    And a work order cancellation reason "felt like it" exists with description: "Felt like it"
    And a work order "cancelled 2" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "one", cancelled at: "today", cancelled by: "the chancellor of cancellation", cancellation reason: "felt like it"
    And I am logged in as "user"
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "cancelled 2"
    Then I should at least see user "the chancellor of cancellation"'s UserName in the cancellationInfo element
    And I should at least see "today's date" in the cancellationInfo element
    And I should at least see work order cancellation reason "felt like it"'s Description in the cancellationInfo element
        
Scenario: User cannot create a markout when the work order markout requirement is none
    Given a work order "markout none" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", asset type: "hydrant", hydrant: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "water main break repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "none", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", approved on: "03/12/2023", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", approved by: "user", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", date completed: "03/22/2023"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "markout none"
    When I click the "Markouts" tab
    Then I should not see the button "Add New Markout"

# Street Opening Permit
Scenario: User should be able to see street opening permit links for a work order
    Given a work order "five" exists with operating center: "nj7", asset type: "service", town: "nj7burg", premise number: "000000000", s o p required: "true"
    And a street opening permit "one" exists with StreetOpeningPermitNumber: "1234", DateRequested: "01/01/2021", DateIssued: "01/02/2021", WorkOrder: "five"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "five"
    When I click the "Street Opening Permit" tab
    Then I should see "Add Street Opening Permit"
    And I should see "Submit New Permit"
    And I should see the following values in the streetOpeningPermitsTable table
    | Permit #   | Date Requested | Date Issued |
    | 1234       | 1/1/2021       | 1/2/2021    |
    And I should not see "Edit Delete" in the table streetOpeningPermitsTable's "Action" column
    When I visit the FieldOperations/GeneralWorkOrder/Edit page for work order: "five"
    And I click the "Street Opening Permit" tab
    Then I should see "Edit Delete" in the table streetOpeningPermitsTable's "Action" column
    When I click the "Edit" link in the 1st row of streetOpeningPermitsTable
    And I enter "05/21/2023" into the DateRequested field
    And I enter "05/23/2023" into the DateIssued field
    And I press "Save Permit"
    Then I should see "Edit Delete" in the table streetOpeningPermitsTable's "Action" column
    And I should see the following values in the streetOpeningPermitsTable table
    | Permit #   | Date Requested | Date Issued |
    | 1234       | 5/21/2023      | 5/23/2023   |
    
Scenario: User admin should be able to submit a new permit through the permits API 
    Given a town "testtown" exists with name: "TEST TOWN", county: "monmouth"
    # This permits user name is specifically created on the permits.mapcall.info site for this test.
    # The API is also handling payment, rather than us dealing with authorize.net directly. 
    # Because of this, we *can not* functional test the payment aspect without it being a prone
    # to flukes if multiple people are running the test at the same time. We can't create a payment
    # profile for this test like we can for traffic tickets. The profile will never end up attached
    # to the user account on the Permits API.
    And an operating center "sop" exists with opcode: "SOP", name: "SOP Op Center", permitsOmUserName: "ross.dickinson@amwater.com"
    And operating center: "sop" exists in town: "testtown"
    And a work order "sop" exists with operating center: "sop", asset type: "service", town: "testtown", s o p required: "true"
	And a user "sopuser" exists with username: "sopuser"
	And a role "workorder-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "sopuser", operating center: "sop"
    And I am logged in as "sopuser"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "sop"
    When I click the "Street Opening Permit" tab
    And I press "Submit New Permit" 
    And I switch to the submit-permit-frame frame
    And I press "Begin permit for municipality: TEST TOWN"
    # ArbitraryIdentifier displays as WorkOrderID to the client.
    Then I should see work order "sop"'s Id in the ArbitraryIdentifier field
    # There's gonna need to be a wait here, I think, or a switch to the frame and waiting for the navigation to complete inside of that.
    # The buttons will need ids to press.
    # The permit for TEST TOWN is specifically made for easy testing.
    # It also tests that the controller is able to find state/county/town permit forms, as they need
    # to be searched for in that order. Without a state, no county/town will be found. Without a county, no town can be found. 
    When I press Save
    And I upload "itsatiff.tiff"
    And I follow "Continue to Payment"
    # The payment html is from the payments api, we can't control it.
    # There's also no specific element id to look for, so we need to look through the whole text.
    # This step should be quick though as it's in a frame with very little content.
    Then I should see "Payment Account:"
    # See comment at top of test about why we don't submit payment in this test.

# Edit Crew Assignments
Scenario: Users can edit crew assignments for a work order
	Given a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
	And a crew assignment "one" exists with work order: "two", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "42"
    And a crew assignment "two" exists with work order: "two", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "12"
    And I am logged in as "user"
	And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "two"
	When I click the "Crew Assignments" tab
	Then I should see the following values in the assignmentsTable table
    | Assigned On             | Assigned For             | Assigned To  | Start Time                  | End Time                    | Employees on Crew|
    | 1/11/2023 1:59 PM (EST) | 1/29/2023 3:00 AM (EST)  | Sunil        | 4/10/2022 2:22:00 PM (EST)  | 4/10/2022 3:07:00 PM (EST)  | 42               |
    | 3/16/2023 2:06 PM (EST) | 3/29/2023 12:00 AM (EST) | Sai          | 4/8/2022 11:38:00 PM (EST)  | 4/9/2022 12:49:00 AM (EST)  | 12               |
    When I click the "Edit" link in the 1st row of assignmentsTable
    And I wait for the dialog to open
    And I enter 8 into the EmployeesOnJob field
    And I enter "10/19/2023" into the DateEnded field
    And I enter "10/10/2023" into the DateStarted field
    And I press "Save Crew Assignment"
    And I click the "Crew Assignments" tab
    Then I should see the following values in the assignmentsTable table
    | Assigned On             | Assigned For             | Assigned To  | Start Time                    | End Time                      | Employees on Crew|
    | 1/11/2023 1:59 PM (EST) | 1/29/2023 3:00 AM (EST)  | Sunil        | 10/10/2023 12:00:00 AM (EST)  | 10/19/2023 12:00:00 AM (EST)  | 8                |
    | 3/16/2023 2:06 PM (EST) | 3/29/2023 12:00 AM (EST) | Sai          | 4/8/2022 11:38:00 PM (EST)    | 4/9/2022 12:49:00 AM (EST)    | 12               |

# Edit Traffic Control
Scenario: Users can edit fields on traffic control tab for a work order
    Given a billing party "one" exists with Description: "ATLANTIC County"
    And a billing party "two" exists with Description: "ALLEGHENY County"
    And a traffic control ticket "one" exists with WorkStartDate: "2023-02-10", TotalHours: "1.85", NumberOfOfficers: "4", WorkOrder: "two", BillingParty: "two"
    And a traffic control ticket "two" exists with WorkStartDate: "2023-03-28", TotalHours: "3.15", NumberOfOfficers: "9", WorkOrder: "two", BillingParty: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "two"
    When I click the "Traffic Control" tab
    Then I should see a display for WorkOrder_TrafficControlRequired with "No"
    And I should see a display for WorkOrder_Notes with "hey this is a note"
    And I should see the following values in the trafficeControlTickets table
    | Work Start Date    | Total Combined Hours | Number of Officers or Employees | Billing Party      |
    | 2/10/2023          | 1.85                 | 4                               | ALLEGHENY County   |
    | 3/28/2023          | 3.15                 | 9                               | ATLANTIC County    |
    And I should see a link to the /FieldOperations/TrafficControlTicket/New?workOrderId=2 page
    When I enter "Testing Notes" into the AppendNotes field
    And I enter "12" into the NumberOfOfficersRequired field
    And I press SaveTrafficControl
    Then I should see "Testing Notes"
    And I should see "12" in the NumberOfOfficersRequired field
    
# Edit Initial
Scenario: Users can edit fields on Initial tab for a work order
    Given the test flag "sap returns a valid functional location" exists
    Given a billing party "one" exists with Description: "ATLANTIC County"
    And a billing party "two" exists with Description: "ALLEGHENY County"
    And a traffic control ticket "one" exists with WorkStartDate: "2023-02-10", TotalHours: "1.85", NumberOfOfficers: "4", WorkOrder: "two", BillingParty: "two"
    And a traffic control ticket "two" exists with WorkStartDate: "2023-03-28", TotalHours: "3.15", NumberOfOfficers: "9", WorkOrder: "two", BillingParty: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "two"
    When I enter "Testing special instructions" into the SpecialInstructions field
    And I press SaveInitialInfo
    And I wait for ajax to finish loading
    Then I should see "Testing special instructions"

# Edit Additional
Scenario: Users can update additional tab for a work order of valve asset type
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "valve leaking", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", approved by: "user", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", date completed: "03/22/2023", alert issued: true, lost water: 5
    And a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
    And a crew assignment "one" exists with work order: "ten", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "2"
    And a crew assignment "two" exists with work order: "ten", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "3"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "ten"
    When I click the "Additional" tab
    Then I should see a link to "Content/LeakageChart.pdf"
    And I should see a display for WorkOrder_TotalManHours with "3"
    And I should see work description "valve leaking" in the FinalWorkDescription dropdown
    And I should see "5" in the LostWater field
    And I should see a display for WorkOrder_Notes with "hey this is a note"
    And I should see "" in the DistanceFromCrossStreet field
    And I should see the link "Finalization"
    And I should see the link "General"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "ten"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show page for work order: "ten"
    When I enter "Testing Notes" into the AppendNotes field
    And I enter "7" into the LostWater field
    And I enter "12" into the DistanceFromCrossStreet field
    And I select work description "valve repair" from the FinalWorkDescription dropdown
    And I press Update
    Then I should be at the FieldOperations/GeneralWorkOrder/Edit page for work order: "ten"
    And I should see "7" in the LostWater field
    And I should see "12" in the DistanceFromCrossStreet field
    And I should see work description "valve repair" in the FinalWorkDescription dropdown

Scenario: Users can update additional tab for a work order of main asset type
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "main", work description: "water main installation", work order priority: "emergency"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "ten"    
    When I click the "Additional" tab
    And I select work description "water main break repair" from the FinalWorkDescription dropdown
    And I press Update
    Then I should see a validation message for LostWater with "The Total Gallons Lost field is required."
    And I should see a validation message for TrafficImpact with "The Significant Traffic Impact field is required."
    And I should see a validation message for DistanceFromCrossStreet with "The Distance From Cross Street (feet) field is required."
    When I enter "7" into the LostWater field
    And I enter "12" into the DistanceFromCrossStreet field
    And I select "Yes" from the TrafficImpact dropdown
    And I press Update
    Then I should be at the FieldOperations/GeneralWorkOrder/Edit page for work order: "ten"
    And I should see work description "water main break repair" in the FinalWorkDescription dropdown
    And I should see "7" in the LostWater field
    And I should see "12" in the DistanceFromCrossStreet field
    And I should see "Yes" in the TrafficImpact dropdown

Scenario: User can update additional tab for a work order of service asset type
    Given a work order "five" exists with operating center: "nj7", asset type: "service", work description: "install meter"
    And a service material "one" exists with description: "Copper", is edit enabled: true
    And a service material "two" exists with description: "Plastic", is edit enabled: true
    And a service material "three" exists with description: "Iron", is edit enabled: true
    And a service size "one" exists with service size description: "1/2", service: true
    And a service size "two" exists with service size description: "1", service: true
    And a service size "three" exists with service size description: "2", service: true
    And a pitcher filter customer delivery method "one" exists with description: "Handed to customer"
    And a pitcher filter customer delivery method "two" exists with description: "Left on porch\doorstep"
    And a pitcher filter customer delivery method "three" exists with description: "Other"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "five"
    When I click the "Additional" tab
    And I select work description "service line renewal" from the FinalWorkDescription dropdown
    And I press Update
    Then I should see a validation message for PreviousServiceLineMaterial with "The Previous Service Company Material field is required."
    And I should see a validation message for PreviousServiceLineSize with "The Previous Service Company Size field is required."
    And I should see a validation message for CompanyServiceLineMaterial with "The Service Company Material field is required."
    And I should see a validation message for CompanyServiceLineSize with "The Service Company Size field is required."
    And I should see a validation message for CustomerServiceLineSize with "The CustomerServiceLineSize field is required."
    And I should see a validation message for DoorNoticeLeftDate with "The DoorNoticeLeftDate field is required."
    And I should see a validation message for InitialServiceLineFlushTime with "The InitialServiceLineFlushTime field is required."
    And I should see a validation message for HasPitcherFilterBeenProvidedToCustomer with "The Pitcher filter provided to customer? field is required."
    When I select service material "one" from the PreviousServiceLineMaterial dropdown
    And I select service material "two" from the CompanyServiceLineMaterial dropdown
    And I select service material "three" from the CustomerServiceLineMaterial dropdown
    And I select service size "one" from the PreviousServiceLineSize dropdown
    And I select service size "two" from the CompanyServiceLineSize dropdown
    And I select service size "three" from the CustomerServiceLineSize dropdown
    And I enter "6/20/2023" into the DoorNoticeLeftDate field
    And I enter "35" into the InitialServiceLineFlushTime field
    And I select "Yes" from the HasPitcherFilterBeenProvidedToCustomer dropdown
    And I enter "6/20/2023" into the DatePitcherFilterDeliveredToCustomer field
    And I select pitcher filter customer delivery method "three" from the PitcherFilterCustomerDeliveryMethod dropdown
    And I enter "Testing Other" into the PitcherFilterCustomerDeliveryOtherMethod field
    And I enter "6/21/2023" into the DateCustomerProvidedAWStateLeadInformation field
    And I press Update
    Then I should be at the FieldOperations/GeneralWorkOrder/Edit page for work order: "five"
    And I should not see a validation message for PreviousServiceLineMaterial with "The Previous Service Company Material field is required."
    And I should not see a validation message for PreviousServiceLineSize with "The Previous Service Company Size field is required."
    And I should not see a validation message for CompanyServiceLineMaterial with "The Service Company Material field is required."
    And I should not see a validation message for CompanyServiceLineSize with "The Service Company Size field is required."
    And I should not see a validation message for CustomerServiceLineSize with "The CustomerServiceLineSize field is required."
    And I should not see a validation message for DoorNoticeLeftDate with "The DoorNoticeLeftDate field is required."
    And I should not see a validation message for InitialServiceLineFlushTime with "The InitialServiceLineFlushTime field is required."
    And I should not see a validation message for HasPitcherFilterBeenProvidedToCustomer with "The Pitcher filter provided to customer? field is required."
    And I should see work description "service line renewal" in the FinalWorkDescription dropdown
    And I should see service material "one" in the PreviousServiceLineMaterial dropdown
    And I should see service material "two" in the CompanyServiceLineMaterial dropdown
    And I should see service material "three" in the CustomerServiceLineMaterial dropdown
    And I should see service size "one" in the PreviousServiceLineSize dropdown
    And I should see service size "two" in the CompanyServiceLineSize dropdown
    And I should see service size "three" in the CustomerServiceLineSize dropdown
    And I should see "Yes" in the HasPitcherFilterBeenProvidedToCustomer dropdown
    And I should see pitcher filter customer delivery method "three" in the PitcherFilterCustomerDeliveryMethod dropdown
  
Scenario: User cannot select a revisit description for a non-revisit order
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", hydrant: "one", asset type: "hydrant", work description: "hydrant leaking", coordinate: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "ten"
    When I click the "Additional" tab
    Then I should not see work description "hydrant landscaping" in the FinalWorkDescription dropdown
    And I should see work description "hydrant paint" in the FinalWorkDescription dropdown
    When I click the "Initial Information" tab
    Then I should not see work description "hydrant landscaping" in the WorkDescription dropdown
    And I should see work description "hydrant paint" in the WorkDescription dropdown

Scenario: User cannot select a non-revisit description for a revisit order
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", hydrant: "one", asset type: "hydrant", work description: "hydrant landscaping", coordinate: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Edit page for work order: "ten"
    When I click the "Additional" tab
    Then I should not see work description "hydrant leaking" in the FinalWorkDescription dropdown
    And I should see work description "hydrant restoration repair" in the FinalWorkDescription dropdown
    When I click the "Initial Information" tab
    Then I should not see work description "hydrant leaking" in the WorkDescription dropdown
    And I should see work description "hydrant restoration repair" in the WorkDescription dropdown
    
Scenario: user can create a service for a work order linked to a sewer lateral
    Given a work order "sewer lateral" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer lateral"
    And I am logged in as "user"
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "sewer lateral"
    And I follow "Create Service"
    Then I should be at the FieldOperations/Service/LinkOrNew page
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "sewer lateral"
    And I follow "Edit"
    And I wait for the page to reload
    And I follow "Create Service"
    Then I should be at the FieldOperations/Service/LinkOrNew page

Scenario: User should be able to remove assigned contractor
    Given a contractor "one" exists with name: "Sunil", is active: "true", awr: "true"
    And a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", hydrant: "one", asset type: "hydrant", work description: "hydrant leaking", coordinate: "one", assigned contractor: "one", assigned to contractor on: "11/10/2023"
    And I am logged in as "user"
    When I visit the FieldOperations/GeneralWorkOrder/Show page for work order: "ten"
    Then I should only see "Sunil On 11/10/2023 12:00:00 AM" in the AssignedContractor element
    When I follow "Edit"
    Then I should be at the FieldOperations/GeneralWorkOrder/Edit page for work order: "ten"
    And I should only see "Sunil On 11/10/2023 12:00:00 AM (click to remove assigned contractor)" in the AssignedContractor element
    When I press "(click to remove assigned contractor)"
    And I wait for the page to reload
    Then I should be at the FieldOperations/GeneralWorkOrder/Edit page for work order: "ten"
    And I should not see "Sunil On 11/10/2023 12:00:00 AM (click to remove assigned contractor)"
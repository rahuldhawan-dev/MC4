Feature: WorkOrderSupervisorApproval

Background: 
    # Disable operating centers for the test. The SAP integration is not going to work in the tests since we do
    # not have any ability to make test data in the SAP system. 
	Given an operating center "sap-disabled" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "false", sap work orders enabled: "false"
	And an operating center "sap-enabled" exists with opcode: "SAP", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And a town "nj7burg" exists with name: "TOWN"
    And operating center: "sap-disabled" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg", name: "Tucson"
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
    And operating center: "sap-disabled" has asset type "valve"
    And operating center: "sap-disabled" has asset type "hydrant"
    And operating center: "sap-disabled" has asset type "main"
    And operating center: "sap-disabled" has asset type "service"
    And operating center: "sap-disabled" has asset type "sewer opening"
    And operating center: "sap-disabled" has asset type "sewer lateral"
    And operating center: "sap-disabled" has asset type "sewer main"
    And operating center: "sap-disabled" has asset type "storm catch"
    And operating center: "sap-disabled" has asset type "equipment"
    And operating center: "sap-disabled" has asset type "facility"
    And operating center: "sap-disabled" has asset type "main crossing"
    And a valve "one" exists with operating center: "sap-disabled", town: "nj7burg", street: "one"
    And work order requesters exist
    And work order purposes exist
    And work order priorities exist
    And work descriptions exist
    And markout requirements exist
    And a user "user-admin" exists with username: "user-admin", full name: "Roy"
	And a wildcard role exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user-admin"
    And a role exists with action: "Read", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-disabled"
    And a role exists with action: "Add", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-disabled"
    And a role exists with action: "Read", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-enabled"
    And a role exists with action: "Add", module: "FieldServicesAssets", user: "user-admin", operating center: "sap-enabled"
    And a work order "one" exists with operating center: "sap-disabled", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "water main break repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user-admin", completed by: "user-admin", sap notification number: "123456789", sap work order number: "987654321", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user-admin", business unit: "Liberty", date completed: "03/22/2023", lost water: 5
    And a role "markoutviolation-add" exists with action: "Add", module: "BPUGeneral", user: "user-admin", operating center: "sap-disabled"

#
# Accounts tab
#

Scenario: User admin should not be able to approve a work order that has an asset type matched with a work description that does not have the same asset type
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "hydrant flushing", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    Then I should see the notification message "Unable to approve a work order when its asset type does not match its work description's asset type."

Scenario: User admin should not be able to approve a work order that has service approval issues 
    # Don't set the service record on the work order, that allows for this message to show up.
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "service", work description: "service line renewal", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    Then I should see the notification message "This work order is for a service but either no service asset record is linked to this work order, or the linked service does not have an installed date. Please ensure that this work order is linked to a service with a valid installed date. Services"

Scenario: User admin should not be able to approve a work order that is already approved
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "valve box repair", approved by: "user-admin", approved on: "4/24/1984", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    Then I should not see the "approve-workorder-button" element

Scenario: User admin should not be able to approve a work order that has an investigative work description
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "valve investigation", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    Then I should see the notification message "Unable to approve a work order with an investigative work order description."

Scenario: User admin should not be able to approve a work order that SAP has not released
    Given a work order "wo" exists with operating center: "sap-enabled", s a p error code: "NOT RELEASED", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    Then I should see the notification message "Unable to approve a work order when SAP has not released or has rejected the release."

Scenario: User admin should be able to edit the requires invoice field if the operating center has work order invoices
    Given an operating center "invoicing" exists with opcode: "NJ4", name: "Howell", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "invoicing" exists in town: "wall"
    And a work order "wo" exists with operating center: "invoicing", town: "wall", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "123456"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    Then I should see the RequiresInvoice field

Scenario: User admin should not be able to edit the requires invoice field if the operating center does not have work order invoices
    Given an operating center "no-invoicing" exists with opcode: "NJ4", name: "Howell", hasWorkOrderInvoicing: false
    And a town "wall" exists with name: "WALL"
    And operating center: "no-invoicing" exists in town: "wall"
    And a work order "wo" exists with operating center: "no-invoicing", town: "wall", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "123456"
    And a wildcard role exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user-admin"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    Then I should not see the RequiresInvoice field

Scenario: User admin can approve a work order
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    And I press "Approve"
    And I click the "Account" tab
    Then I should see a display for ApprovedBy_FullName with user "user-admin"'s FullName

Scenario: User admin can approve a work order with service line installation partial work description and a revisit work order with service line installation complete partial work description is automatically created
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "service", valve: "one", work description: "service line installation partial", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And a SAPWorkOrderStep "one" exists with Description: "Create"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    And I press "Approve"
    Then I should be at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order "wo"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Show/3 page

Scenario: User admin can reject a work order that can be approved
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    And I press "Reject"
    And I enter "I dunno I just don't like it" into the RejectionReason field
    And I press reject-workorder-button
    Then I should be at the FieldOperations/GeneralWorkOrder/Show page for work order: "wo"
    And I should at least see "user-admin: I dunno I just don't like it" in the Notes element

# This test only needs to exist as long as the Reject button is existing in two places
Scenario: User admin can reject a work order that can not be approved
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "hydrant flushing", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    And I press "Reject"
    And I enter "I dunno I just don't like it" into the RejectionReason field
    And I press reject-workorder-button
    Then I should be at the FieldOperations/GeneralWorkOrder/Show page for work order: "wo"
    And I should at least see "user-admin: I dunno I just don't like it" in the Notes element

Scenario: User admin can not reject a work order that was already approved
	Given a work order "wo" exists with approved by: "user-admin", approved on: "4/24/1984", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Account" tab
    Then I should not see the approve-workorder-button element

# Requisition/Purchase Orders(PO)

Scenario: User admin can view requisitions for a work order
    Given a requisition type "one" exists with description: "Paving"
    And a requisition type "two" exists with description: "Spoils"
    And a requisition "one" exists with work order: "one", SAPRequisitionNumber: "what is this", RequisitionType: "one"
    And a requisition "two" exists with work order: "one", SAPRequisitionNumber: "123456", RequisitionType: "two"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Purchase Orders(PO)" tab
    Then I should see the following values in the requisitions-table table
    | Purchase Order (PO) Type | SAP Purchase Order(PO) # |
    | Paving                   | what is this             |
    | Spoils                   | 123456                   |

Scenario: User admin can add a requisition
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And a requisition type "one" exists with description: "some req type"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Purchase Orders(PO)" tab
    And I follow "Add Purchase Order"
    And I select requisition type "one" from the RequisitionType dropdown
    And I enter "whatever" into the SAPRequisitionNumber field
    And I press save-requisition
    Then I should see the following values in the requisitions-table table
    | Purchase Order (PO) Type | SAP Purchase Order(PO) # |
    | some req type            | whatever                 |

Scenario: User admin can edit a requisition
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And a requisition type "one" exists with description: "some req type"
    And a requisition "one" exists with work order: "wo", requisition type: "one", s a p requisition number: "12346"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Purchase Orders(PO)" tab
    And I follow the "Edit" link to the Edit page for requisition "one"
    And I enter "some value" into the SAPRequisitionNumber field
    And I press save-requisition 
    Then I should see the following values in the requisitions-table table
    | Purchase Order (PO) Type | SAP Purchase Order(PO) # |
    | some req type            | some value               |

Scenario: User admin can delete a requisition
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "valve box repair", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And a requisition type "one" exists with description: "some req type"
    And a requisition "one" exists with work order: "wo", requisition type: "one", s a p requisition number: "12346"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Purchase Orders(PO)" tab
    And I click ok in the dialog after following "Delete"
    Then I should not see a link to the Edit page for requisition: "one"



#
# Everything below here is a test that should be the same as the general work order page test
#

# Search/Index
Scenario: User admin can search for work orders they have role for
    Given a operating center "no-role" exists
    And a user "user-admin-no-wildcard-role" exists with username: "user-admin-no-wildcard-role", full name: "Roy"
	And a role exists with operating center: "sap-disabled", action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user-admin-no-wildcard-role"
    And a work order "cancelled" exists with operating center: "sap-disabled", cancelled at: "11/11/2016"
	And I am logged in as "user-admin-no-wildcard-role"
	And I am at the FieldOperations/WorkOrderSupervisorApproval/Search page
    Then I should see operating center "sap-disabled"'s Description in the OperatingCenter dropdown
    And I should not see operating center "no-role"'s Description in the OperatingCenter dropdown
    When I select operating center "sap-disabled" from the OperatingCenter dropdown
	And I press Search
    Then I should not see a link to the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "cancelled"
    And I should see a link to the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"

#Initial Details
Scenario: User admin can view a work order
    Given I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    Then I should only see work order "one"'s Id in the WorkOrderId element
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
    And I should only see "3/15/2023 12:00:00 AM (EST)" in the MaterialsApprovedOn element
    And I should only see "Success" in the SAPErrorCode element
    And I should only see "3/18/2023 12:00:00 AM (EST)" in the MaterialPlanningCompletedOn element

Scenario: User admin cannot edit a work order or see various links
    Given I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    Then I should not see a link to the edit page for work order: "one"
    And I should not see the link "SAP Notifications"
    And I should not see the button "Cancel Order"
    And I should not see the button "Complete Material Planning"
    And I should not see the link "Create Service"

# Materials
Scenario: User admin can view materials for a work order
    Given a material "one" exists with PartNumber: "123456789", Description: "Plastic"
    And a material "two" exists with PartNumber: "987654321", Description: "Copper"
    And a stock location "one" exists with SAPStockLocation: "2600", Description: "H&M"
    And a stock location "two" exists with SAPStockLocation: "1300", Description: "STYD"
    And a material used "one" exists with Material: "one", WorkOrder: "one", StockLocation: "one", Quantity: 2
    And a material used "two" exists with Material: "two", WorkOrder: "one", StockLocation: "two", Quantity: 5
    And a material used "three" exists with WorkOrder: "one", StockLocation: "two", Quantity: 7, NonStockDescription: "Test"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Materials" tab
    Then I should see the following values in the materials table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |
    | 987654321   | STYD           | 1300               | Copper      | 5        |
    | N/A         | STYD           | 1300               |  Test       | 7        |
    And I should see a display for MaterialsDocID with "122333444"
    And I should see a display for MaterialPostingDate with "3/10/2023"
    And I should see a display for MaterialsApprovedBy with "Roy"

# Spoils
Scenario: User admin can view spoils for a work order
    Given a spoil storage location "one" exists with Name: "Newman Springs Yard"
    And a spoil storage location "two" exists with Name: "Union Beach Yard"
    And a spoil "one" exists with SpoilStorageLocation: "two", WorkOrder: "one", Quantity: 1.25
    And a spoil "two" exists with SpoilStorageLocation: "one", WorkOrder: "one", Quantity: 2.75
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Spoils" tab
    Then I should see the following values in the spoils table
    | Quantity (CY) | Spoil Storage Location |
    | 1.25          | Union Beach Yard       |
    | 2.75          | Newman Springs Yard    |

# Markouts
Scenario: User admin can view markouts for a work order
    Given a markout type "one" exists with description: "C TO C"
    And a markout type "two" exists with description: "None"
    And a markout "one" exists with MarkoutType: "two", WorkOrder: "one", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with MarkoutType: "one", WorkOrder: "one", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Markouts" tab
    Then I should see the following values in the markoutsTable table
    | Markout Type    | Date Of Request    | Ready Date           | Expiration Date       |
    | NOT LISTED      | 2/20/2023          | 2/21/2023 12:00 AM   |  2/28/2023 12:00 AM   |
    | C TO C          | 3/10/2023          | 3/12/2023 12:00 AM   |  3/19/2023 12:00 AM   |

Scenario: User admin can delete markouts for a work order
    Given a markout type "one" exists with description: "C TO C"
    And a markout type "two" exists with description: "None"
    And a markout "one" exists with MarkoutType: "two", WorkOrder: "one", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with MarkoutType: "one", WorkOrder: "one", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Markouts" tab
    Then I should see the following values in the markoutsTable table
    | Markout Type    | Date Of Request    | Ready Date           | Expiration Date       |
    | NOT LISTED      | 2/20/2023          | 2/21/2023 12:00 AM   |  2/28/2023 12:00 AM   |
    | C TO C          | 3/10/2023          | 3/12/2023 12:00 AM   |  3/19/2023 12:00 AM   |
    And I should see the "Action" column in the "markoutsTable" table

Scenario: User admin can create new markouts for a work order
    Given a markout type "one" exists with description: "C TO C"
    And a markout type "two" exists with description: "None"
    And a markout "one" exists with MarkoutType: "two", WorkOrder: "one", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with MarkoutType: "one", WorkOrder: "one", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
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

Scenario: User admin can create new markouts for a work order with markout editable operating center
    Given an operating center "ca30" exists with opcode: "CA30", name: "San Diego", sap enabled: "true", sap work orders enabled: "true", MarkoutsEditable: true
    And a work order "new" exists with operating center: "ca30", town: "nj7burg", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "water main break repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user-admin", completed by: "user-admin", sap notification number: "123456789", sap work order number: "987654321", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user-admin", business unit: "Liberty", date completed: "03/22/2023"
	And a markout type "one" exists with description: "C TO C"
    And a markout type "two" exists with description: "None"
    And a markout "one" exists with MarkoutType: "two", WorkOrder: "new", DateOfRequest: "02/20/2023", ReadyDate: "02/21/2023", ExpirationDate: "02/28/2023"
    And a markout "two" exists with MarkoutType: "one", WorkOrder: "new", DateOfRequest: "03/10/2023", ReadyDate: "03/12/2023", ExpirationDate: "03/19/2023"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "new"
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
Scenario: User admin can view markout violations for a work order
    Given a markout violation "one" exists with DateOfViolationNotice: "2019-02-05 00:00:00.000", MarkoutViolationStatus: "Pending review - Jeff Bowlby", MarkoutRequestNumber: "162022169", WorkOrder: "one"
    And a markout violation "two" exists with DateOfViolationNotice: "2018-06-28 00:00:00.000", MarkoutViolationStatus: "Completed", MarkoutRequestNumber: "190740395", WorkOrder: "one"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Markout Violations" tab
    Then I should see the following values in the markoutViolations table
    | Date Of Violation Notice    | Markout Violation Status       | Markout Request Number          |
    | 2/5/2019                    | Pending review - Jeff Bowlby   |   162022169                     |
    | 6/28/2018                   | Completed                      |   190740395                     |
    When I follow "Create Markout Violation"
    Then I should be at the BPU/MarkoutViolation/New page

# Markout Damages
Scenario: User admin can view markout damages for a work order
    Given a sewer markout damage utility damage type "sewer" exists
    And a water markout damage utility damage type "water" exists
    And a gas markout damage utility damage type "gas" exists 
    And a markout damage "one" exists with DamageOn: "2019-02-05", RequestNumber: "123456789", Excavator: "PSE&G", WorkOrder: "one"
    And a markout damage "two" exists with DamageOn: "2018-06-28", RequestNumber: "987654321", Excavator: "J F KIELY", WorkOrder: "one"
    And markout damage "one" has sewer markout damage utility damage type "sewer"
    And markout damage "two" has water markout damage utility damage type "water"
    And markout damage "two" has gas markout damage utility damage type "gas"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Markout Damages" tab
    Then I should see the following values in the markoutDamages table
    | Request Number | Damaged On Date    | Excavator | Utilities Damaged |
    | 123456789      | 2/5/2019 12:00 AM  | PSE&G     | Sewer             |
    | 987654321      | 6/28/2018 12:00 AM | J F KIELY | Water, Gas        |
    And I should see a link to the /FieldOperations/MarkoutDamage/New/1 page   

# This is tested more thoroughly in markoutdamage.feature.
Scenario: User admin can create markout damages for a work order
    Given I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Markout Damages" tab
    And I follow "Create Markout Damage"
    Then I should see work order "one"'s Id in the WorkOrder field

# Sewer Overflows
Scenario: User admin can view sewer overflows for a work order
    Given a sewer clearing method "one" exists with description: "Power Snake"
    And a sewer clearing method "two" exists with description: "Water Jet"
    And a sewer overflow area "one" exists with description: "Manhole"
    And a sewer overflow area "two" exists with description: "Vac Truck"
    And a zone type "one" exists with description: "Business"
    And a zone type "two" exists with description: "Residential"
    And a sewer overflow reason "one" exists with description: "Grease"
    And a sewer overflow reason "two" exists with description: "Roots"
    And a work order "for-sewers" exists with operating center: "sap-disabled", town: "nj7burg", work description: "sewer main overflow", completed by: "user-admin", date completed: "4/24/1984"
    And a sewer overflow "one" exists with SewerClearingMethod: "one", AreaCleanedUpTo: "one", ZoneType: "one", EnforcingAgencyCaseNumber: "10-25314", IncidentDate: "2022-04-07", WorkOrder: "for-sewers"
    And a sewer overflow "two" exists with SewerClearingMethod: "two", AreaCleanedUpTo: "two", ZoneType: "two", EnforcingAgencyCaseNumber: "22-02-13-1314-16", IncidentDate: "2022-03-31", WorkOrder: "for-sewers"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    Then I should not see the "Sewer Overflows" tab
    When I visit the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "for-sewers"
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
Scenario: User admin can view traffic control tickets for a work order
    Given a billing party "one" exists with Description: "ATLANTIC County"
    And a billing party "two" exists with Description: "ALLEGHENY County"
    And a traffic control ticket "one" exists with WorkStartDate: "2023-02-10", TotalHours: "1.85", NumberOfOfficers: "4", WorkOrder: "one", BillingParty: "two"
    And a traffic control ticket "two" exists with WorkStartDate: "2023-03-28", TotalHours: "3.15", NumberOfOfficers: "9", WorkOrder: "one", BillingParty: "one"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Traffic Control" tab
    Then I should see a display for TrafficControlRequired with "No"
    And I should see a display for Notes with "hey this is a note"
    And I should see the following values in the trafficeControlTickets table
    | Work Start Date    | Total Combined Hours | Number of Officers or Employees | Billing Party      |
    | 2/10/2023          | 1.85                 | 4                               | ALLEGHENY County   |
    | 3/28/2023          | 3.15                 | 9                               | ATLANTIC County    |

# This is tested more thoroughly in the traffic control feature
Scenario: User admin can add a traffic control ticket to a work order
    Given I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Traffic Control" tab
    And I follow "Create Traffic Control Ticket"
    Then I should see work order "one"'s Id in the WorkOrder field

# Restoration
Scenario: User admin can view restorations for a work order
    Given a restoration type "one" exists with description: "ASPHALT - ALLEY"
    And a restoration type "two" exists with description: "BRICK - STREET"
    And a restoration response priority "one" exists with description: "Emergency 5 day"
    And a restoration response priority "two" exists with description: "Priority (10 days)"
    And a contractor "one" exists with name: "Sunil", is active: "true", awr: "true"
    And a restoration "one" exists with RestorationType: "two", PavingSquareFootage: "28.00", LinearFeetOfCurb: "6.00", WorkOrder: "one", PartialRestorationDate: "2001-11-16", FinalRestorationDate: "2000-12-31", ResponsePriority: "one", AssignedContractor: "one"
    And a restoration "two" exists with RestorationType: "one", PavingSquareFootage: "25.00", LinearFeetOfCurb: "7.00", WorkOrder: "one", PartialRestorationDate: "2000-12-31", FinalRestorationDate: "2003-09-12", ResponsePriority: "two", AssignedContractor: "one"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
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
Scenario: User admin can view crew assignments for a work order
	Given a crew "one" exists with description: "Sunil", operating center: "sap-disabled", active: true
    And a crew "two" exists with description: "Sai", operating center: "sap-disabled", active: true
	And a crew assignment "one" exists with work order: "one", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "42"
    And a crew assignment "two" exists with work order: "one", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "12"
    And I am logged in as "user-admin"
	And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
	When I click the "Crew Assignments" tab
	Then I should see the following values in the assignmentsTable table
    | Assigned On             | Assigned For             | Assigned To  | Start Time                  | End Time                    | Employees on Crew|
    | 1/11/2023 1:59 PM (EST) | 1/29/2023 3:00 AM (EST)  | Sunil        | 4/10/2022 2:22:00 PM (EST)  | 4/10/2022 3:07:00 PM (EST)  | 42               |
    | 3/16/2023 2:06 PM (EST) | 3/29/2023 12:00 AM (EST) | Sai          | 4/8/2022 11:38:00 PM (EST)  | 4/9/2022 12:49:00 AM (EST)  | 12               |

# Additional
Scenario: User admin can view additional tab for a work order
    Given a crew "one" exists with description: "Sunil", operating center: "sap-disabled", active: true
    And a crew "two" exists with description: "Sai", operating center: "sap-disabled", active: true
    And a crew assignment "one" exists with work order: "one", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "2"
    And a crew assignment "two" exists with work order: "one", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "3"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Additional" tab
    Then I should see a link to "Content/LeakageChart.pdf"
    And I should see a display for TotalManHours with "3"
    And I should see a display for LostWater with "5"
    And I should see a display for Notes with "hey this is a note"
    And I should see a display for DistanceFromCrossStreet with ""
    And I should see a link to the FieldOperations/GeneralWorkOrder/Edit page for work order: "one"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "one"

# Schedule of Values
Scenario: User admin can view schedule of values for a work order when the work order has invoicing
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a work order "four" exists with operating center: "nj4", town: "wall", asset type: "valve", valve: "one", sap work order number: "1", completed by: "user-admin", date completed: "4/24/1984"
    And a schedule of value category "one" exists with description: "Saw Cut"
    And a schedule of value category "two" exists with description: "Curb Box"
    And a unit of measure "one" exists with description: "Gallons"
    And a unit of measure "two" exists with description: "HR"
    And a schedule of value "one" exists with ScheduleOfValueCategory: "one", Description: "Cold Patch", UnitOfMeasure: "one"
    And a schedule of value "two" exists with ScheduleOfValueCategory: "two", Description: "Partial Excavation", UnitOfMeasure: "two"
    And a work order schedule of value "one" exists with WorkOrder: "four", ScheduleOfValue: "one", OtherDescription: "Chem pump parts", IsOvertime: "true"
    And a work order schedule of value "two" exists with WorkOrder: "four", ScheduleOfValue: "two", OtherDescription: "Total cost of repair by contractor", IsOvertime: "false"
    And I am logged in as "user-admin"
    When I visit the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "four"
    And I click the "Schedule Of Values" tab
    Then I should see the following values in the scheduleOfValuesTable table
    | Schedule Of Value Category | Schedule Of Value  | Other Description                  | Unit Of Measure | Is Overtime |
    | Saw Cut                    | Cold Patch         | Chem pump parts                    | Gallons         | True        |
    | Curb Box                   | Partial Excavation | Total cost of repair by contractor | HR              | False       |

Scenario: User admin can not view schedule of values if operating center does not have invoicing
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a street "ace" exists with town: "wall", full st name: "ACE DRIVE", is active: true
    And a work order "four" exists with operating center: "nj4", town: "wall", street: "ace", asset type: "valve", valve: "one"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    Then I should not see the "Schedule Of Values" tab

Scenario: User can delete schedule of values for a work order
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a work order "wo" exists with operating center: "nj4", town: "wall", asset type: "valve", valve: "one", sap work order number: "1", completed by: "user-admin", date completed: "4/24/1984"
    And a schedule of value category "one" exists with description: "Saw Cut"
    And a schedule of value category "two" exists with description: "Curb Box"
    And a unit of measure "one" exists with description: "Gallons"
    And a unit of measure "two" exists with description: "HR"
    And a schedule of value "one" exists with ScheduleOfValueCategory: "one", Description: "Cold Patch", UnitOfMeasure: "one"
    And a schedule of value "two" exists with ScheduleOfValueCategory: "two", Description: "Partial Excavation", UnitOfMeasure: "two"
    And a work order schedule of value "one" exists with WorkOrder: "wo", ScheduleOfValue: "one", OtherDescription: "Chem pump parts", IsOvertime: "true"
    And a work order schedule of value "two" exists with WorkOrder: "wo", ScheduleOfValue: "two", OtherDescription: "Total cost of repair by contractor", IsOvertime: "false"
    And I am logged in as "user-admin"
    When I visit the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    Then I should see the "Schedule Of Values" tab
    When I click the "Schedule Of Values" tab
    Then I should see the following values in the scheduleOfValuesTable table
    | Schedule Of Value Category | Schedule Of Value  | Other Description                  | Unit Of Measure | Is Overtime |
    | Saw Cut                    | Cold Patch         | Chem pump parts                    | Gallons         | True        |
    | Curb Box                   | Partial Excavation | Total cost of repair by contractor | HR              | False       |
    When I click the "Delete" link in the 2nd row of scheduleOfValuesTable and then click ok in the confirmation dialog
    And I wait for ajax to finish loading
    Then I should see the following values in the scheduleOfValuesTable table
    | Schedule Of Value Category | Schedule Of Value  | Other Description                  | Unit Of Measure | Is Overtime |
    | Saw Cut                    | Cold Patch         | Chem pump parts                    | Gallons         | True        |

Scenario: User can add schedule of values for a work order
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", sap enabled: "true", sap work orders enabled: "true", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a work order "wo" exists with operating center: "nj4", town: "wall", asset type: "valve", valve: "one", sap work order number: "1", completed by: "user-admin", date completed: "4/24/1984"
    And a schedule of value category "one" exists with description: "Saw Cut"
    And a schedule of value category "two" exists with description: "Curb Box"
    And a unit of measure "one" exists with description: "Gallons"
    And a unit of measure "two" exists with description: "HR"
    And a schedule of value "one" exists with ScheduleOfValueCategory: "one", Description: "Cold Patch", UnitOfMeasure: "one"
    And a schedule of value "two" exists with ScheduleOfValueCategory: "two", Description: "Partial Excavation", UnitOfMeasure: "two"
    And a work order schedule of value "one" exists with WorkOrder: "wo", ScheduleOfValue: "one", OtherDescription: "Chem pump parts", IsOvertime: "true"
    And a work order schedule of value "two" exists with WorkOrder: "wo", ScheduleOfValue: "two", OtherDescription: "Total cost of repair by contractor", IsOvertime: "false"
    And I am logged in as "user-admin"
    When I visit the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    When I click the "Schedule Of Values" tab
    Then I should see the following values in the scheduleOfValuesTable table
    | Schedule Of Value Category | Schedule Of Value  | Other Description                  | Unit Of Measure | Is Overtime |
    | Saw Cut                    | Cold Patch         | Chem pump parts                    | Gallons         | True        |
    | Curb Box                   | Partial Excavation | Total cost of repair by contractor | HR              | False       |
    When I follow "Add Schedule Of Value"
    And I wait for the dialog to open
    Then the OtherDescription field should be disabled
    And the IsOvertime field should be disabled
    When I select schedule of value category "one" from the ScheduleOfValueCategory dropdown
    And I select schedule of value "one" from the ScheduleOfValue dropdown
    And I enter 8 into the Total field
    And I press "Save Schedule of Value"
    Then I should see the following values in the scheduleOfValuesTable table
    | Schedule Of Value Category | Schedule Of Value  | Other Description                  | Unit Of Measure | Is Overtime |
    | Saw Cut                    | Cold Patch         | Chem pump parts                    | Gallons         | True        |
    | Curb Box                   | Partial Excavation | Total cost of repair by contractor | HR              | False       |
    | Saw Cut                    | Cold Patch         |                                    | Gallons         | False       |

# Job Site Check Lists
Scenario: User admin can view job site check lists for a work order
    Given a job site check list "one" exists with map call work order: "one", CheckListDate: "01/22/2023", CreatedBy: "Sunil"
    And a job site check list "two" exists with map call work order: "one", CheckListDate: "03/21/2023", CreatedBy: "Sai"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Job Site Check Lists" tab
    Then I should see the following values in the jobSiteCheckLists table
    | Check List Date | Created By |
    | 1/22/2023       | Sunil      |
    | 3/21/2023       | Sai        |
    And I should see a link to the show page for job site check list "one"
    And I should see a link to the show page for job site check list "two"

# Job Observations
Scenario: User admin can view job observations for a work order
    Given I am logged in as "user-admin"
    And a overall safety rating "one" exists with description: "Satisfactory"
    And a overall safety rating "two" exists with description: "Unsatisfactory"
    And a overall quality rating "one" exists with description: "Satisfactory"
    And a overall quality rating "two" exists with description: "Unsatisfactory"
    And a job observation "one" exists with work order: "one", ObservationDate: "01/22/2023", OverallSafetyRating: "one", OverallQualityRating: "two"
	And a job observation "two" exists with work order: "one", ObservationDate: "03/21/2023", OverallSafetyRating: "two", OverallQualityRating: "one"
	And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Job Observations" tab
    Then I should see the following values in the jobObservations table
    | Observation Date  | Overall Safety Rating | Overall Quality Rating |
    | 1/22/2023         | Satisfactory          |    Unsatisfactory      |
    | 3/21/2023         | Unsatisfactory        |     Satisfactory       |
    And I should see a link to the show page for job observation "one"
    And I should see a link to the show page for job observation "two"

# This is tested more thoroughly in the JobObservationPage.feature
Scenario: User admin can create a job observation for a work order
    Given a role exists with action: "UserAdministrator", module: "OperationsHealthAndSafety", user: "user-admin", operating center: "sap-disabled"
    And I am logged in as "user-admin"
    And a overall safety rating "one" exists with description: "Satisfactory"
    And a overall safety rating "two" exists with description: "Unsatisfactory"
    And a overall quality rating "one" exists with description: "Satisfactory"
    And a overall quality rating "two" exists with description: "Unsatisfactory"
    And a job observation "one" exists with work order: "one", ObservationDate: "01/22/2023", OverallSafetyRating: "one", OverallQualityRating: "two"
	And a job observation "two" exists with work order: "one", ObservationDate: "03/21/2023", OverallSafetyRating: "two", OverallQualityRating: "one"
	And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    When I click the "Job Observations" tab
    And I follow "Create Job Observation"
    Then I should see work order "one"'s Id in the WorkOrder field
    
# Invoices
Scenario: User admin can view invoices for a work order
    Given an operating center "invoicing" exists with opcode: "NJ4", name: "Howell", sap enabled: "true", sap work orders enabled: "true", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "invoicing" exists in town: "wall"
    And a work order "four" exists with operating center: "invoicing", town: "wall", asset type: "valve", valve: "one", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "123456"
    And a work order invoice "one" exists with WorkOrder: "four", InvoiceDate: "02/15/2023", SubmittedDate: "02/12/2023", CanceledDate: "02/16/2023", IncludeMaterials: true
    And a work order invoice "two" exists with WorkOrder: "four", InvoiceDate: "03/15/2023", SubmittedDate: "03/12/2023", CanceledDate: "03/16/2023", IncludeMaterials: false
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "one"
    Then I should not see the "Invoices" tab
    When I visit the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "four"
    Then I should see the "Invoices" tab
    When I click the "Invoices" tab
    Then I should see the following values in the invoices table
    | Invoice Date | Submitted Date | Canceled Date | Include Materials |
    | 2/15/2023    | 2/12/2023      |  2/16/2023    | Yes               |
    | 3/15/2023    | 3/12/2023      |  3/16/2023    | No                |

# Meters
Scenario: User admin can view meters for a work order when the operating center is SAP enabled
    Given a work order "five" exists with operating center: "sap-enabled", asset type: "service", work description: "install meter", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "123456"
    And a service installation "one" exists with work order: "five", MeterManufacturerSerialNumber: "1234567890", MeterLocationInformation: "9876543210"
    And a service installation "two" exists with work order: "five", MeterManufacturerSerialNumber: "9876543210", MeterLocationInformation: "1234567890"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "five"
    When I click the "Set Meter" tab
    Then I should see the following values in the meters table
    | Id | Meter Manufacturer Serial Number | Meter Location Information |
    | 1  | 1234567890                       |  9876543210                |
    | 2  | 9876543210                       |  1234567890                |
    And I should see a link to the show page for service installation "one"
    And I should see a link to the show page for service installation "two"

Scenario: user can view and not edit valve details
    Given a user "user-valve" exists with username: "user-valve", full name: "Roy" 
    And an operating center "nj4" exists with opcode: "NJ4", name: "Howell", hasWorkOrderInvoicing: true
    And a role exists with action: "Read", module: "FieldServicesAssets", user: "user-valve", operating center: "nj4"
    And a role exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user-valve", operating center: "nj4"
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a valve "canview" exists with operating center: "nj4", town: "wall"
    And a work order "canview" exists with operating center: "nj4", town: "wall", asset type: "valve", valve: "canview", sap work order number: "1", completed by: "user-admin", date completed: "4/24/1984"
    And I am logged in as "user-valve"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "canview"
    When I click the "Valve" tab
    Then I should not see the valveEditButton element 
    When I switch to the valveFrame frame
    Then I should see a display for ValveType with ""

Scenario: user can view and edit valve details
    Given a user "user-valve" exists with username: "user-valve", full name: "Roy" 
    And an operating center "nj4" exists with opcode: "NJ4", name: "Howell", hasWorkOrderInvoicing: true
    And a role exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user-valve", operating center: "nj4"
    And a role exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user-valve", operating center: "nj4"
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a street "canedit" exists with town: "wall", full st name: "gravy street"
    And a valve "canedit" exists with operating center: "nj4", town: "wall", date installed: "yesterday", street: "canedit", turns: 5
    And a valve type "one" exists with description: "GATE"
    And a valve normal position "one" exists with description: "CLOSED"
    And a functional location "one" exists with town: "wall", asset type: "valve"
    And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
    And a work order "canedit" exists with operating center: "nj4", town: "wall", asset type: "valve", valve: "canedit", sap work order number: "1", completed by: "user-admin", date completed: "4/24/1984"
    And I am logged in as "user-valve"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "canedit"
    When I click the "Valve" tab
    When I press "valveEditButton"
    And I switch to the valveFrame frame
    And I press Save
    Then I should see a validation message for ValveType with "Valve Type is required for active / installed valves." 
    When I select valve type "one" from the ValveType dropdown
    And I select valve normal position "one" from the NormalPosition dropdown
    And I select functional location "one" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a display for ValveType with valve type "one"
    And I should see a display for NormalPosition with valve normal position "one"

Scenario: User can view special instruction on work order show page
    Given a work order "wo" exists with operating center: "sap-disabled", asset type: "valve", valve: "one", work description: "hydrant flushing", completed by: "user-admin", date completed: "4/24/1984", sap work order number: "12345"
    And I am logged in as "user-admin"
    And I am at the FieldOperations/WorkOrderSupervisorApproval/Show page for work order: "wo"
    Then I should only see work order "wo"'s SpecialInstructions in the WorkOrderSpecialInstructions element

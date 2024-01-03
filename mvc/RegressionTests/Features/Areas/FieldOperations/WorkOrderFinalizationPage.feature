Feature: WorkOrderFinalizationPage

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
    And a work order "two" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "valve", valve: "one", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "valve repair", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "routine", created by: "user", sap notification number: "123456789", sap work order number: "987654321", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", business unit: "Liberty", work order priority: "emergency", distance from cross street: "12", alert issued: true, lost water: 5
    And a work order "three" exists with operating center: "nj7", town: "nj7burg", town section: "one", street: "one", apartment addtl: "Testing Additional Apartment", nearest cross street: "two", asset type: "sewer main", zip code: "85023", work order requester: "customer", secondary phone number: "123-456-7890", work description: "sewer main overflow", customer name: "Smith", significant traffic impact: "true", alert id: "4567", s o p required: "true", markout requirement: "none", created by: "user", completed by: "user", sap notification number: "123456789", sap work order number: "987654321", materials approved on: "03/15/2023", material planning completed on: "03/18/2023", s a p error code: "Success", materials doc id: "122333444", material posting date: "03/10/2023 10:10:25 AM", materials approved by: "user", work order priority: "emergency"
    And a work order "cancelled" exists with operating center: "nj7", cancelled at: "11/11/2016"
    And a role "markoutviolation-add" exists with action: "Add", module: "BPUGeneral", user: "user", operating center: "nj7"
    And an meter location "one" exists with description: "one", code: "c1"
    And an meter location "two" exists with description: "Unknown", code: "c2"

# Search/Index
Scenario: User can search for work orders
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Search page
    Then I should not see operating center "nj6"'s Description in the OperatingCenter dropdown
    And I should see operating center "nj7"'s Description in the OperatingCenter dropdown
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should not see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "cancelled"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "one"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "three"

Scenario: Admin user can search for work orders
    Given a work order "four" exists with operating center: "nj6", asset type: "hydrant", work description: "hydrant flushing"
    And a work order "five" exists with operating center: "nj6", asset type: "valve", work description: "valve replacement", sap work order number: "1234567890", work order priority: "emergency"
    And I am logged in as "admin"
    And I am at the FieldOperations/WorkOrderFinalization/Search page
    Then I should see operating center "nj6"'s Description in the OperatingCenter dropdown
    And I should see operating center "nj7"'s Description in the OperatingCenter dropdown
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should not see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "cancelled"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "one"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "three"
    When I go to the FieldOperations/WorkOrderFinalization/Search page
    And I select operating center "nj6" from the OperatingCenter dropdown
    And I press Search
    Then I should not see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "four"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"

Scenario: User can search multiple asset types and multiple work descriptions
    Given a work order "four" exists with operating center: "nj7", asset type: "hydrant", work description: "hydrant flushing", sap work order number: "1234567890", work order priority: "emergency"
    And a work order "five" exists with operating center: "nj7", asset type: "valve", work description: "valve replacement", sap work order number: "1234567890", work order priority: "emergency"
    And a work order "six" exists with operating center: "nj7", asset type: "hydrant", work description: "hydrant flushing", sap work order number: "1234567890", work order priority: "emergency"
    And a work order "seven" exists with operating center: "nj7", asset type: "valve", work description: "valve replacement", sap work order number: "1234567890", work order priority: "emergency"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Search page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "hydrant" from the AssetType dropdown
    And I select asset type "valve" from the AssetType dropdown
    And I select work description "hydrant flushing" from the WorkDescription dropdown
    And I select work description "valve replacement" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "four"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "five"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "six"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "seven"
    When I go to the FieldOperations/WorkOrderFinalization/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "hydrant" from the AssetType dropdown
    And I select work description "hydrant flushing" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "four"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "six"
    And I should not see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "five"
    And I should not see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "seven"
    When I go to the FieldOperations/WorkOrderFinalization/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select asset type "valve" from the AssetType dropdown
    And I select work description "valve replacement" from the WorkDescription dropdown
    When I press "Search"
    Then I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "five"
    And I should see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "seven"
    And I should not see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "four"
    And I should not see a link to the FieldOperations/WorkOrderFinalization/Edit page for work order "six"

#Initial Details
Scenario: User can view a work order
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
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
    And I should only see "123456789" in the SAPNotificationNumber element
    And I should only see " " in the ApprovedOn element
    And I should only see "987654321" in the SAPWorkOrderNumber element
    And I should only see "3/15/2023 12:00:00 AM (EST)" in the MaterialsApprovedOn element
    And I should only see "Success" in the SAPErrorCode element
    And I should only see "3/18/2023 12:00:00 AM (EST)" in the MaterialPlanningCompletedOn element
    And I should not see a link to the FieldOperations/Service/LinkOrNew?workOrderId=2 page
    And I should not see a link to the FieldOperations/CrewAssignment/ShowCalendar?Crew=2 page

Scenario: User can finalize a work order of valve asset type
    Given a work order flushing notice type "one" exists with description: "Standard Flushing Notice left"
    And a work order flushing notice type "two" exists with description: "Extended Flushing Notice left"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I select work order flushing notice type "one"'s Description from the FlushingNoticeType dropdown
    And I select "Yes" from the DigitalAsBuiltCompleted dropdown
    And I enter "4/14/2015" into the CompletedDate field
    And I press Finalize
    Then I should be at the FieldOperations/WorkOrderFinalization/Show page for work order "two"
    And I should only see "Standard Flushing Notice left" in the FlushingNoticeTypeInit element
    And I should only see "4/14/2015" in the DateCompleted element
    And the DigitalAsBuiltCompletedCheckbox field should be disabled
    And the DigitalAsBuiltCompletedCheckbox field should be checked

Scenario: User can finalize a work order with service line renewal work description
    Given a work order flushing notice type "one" exists with description: "Standard Flushing Notice left"
    And a work order flushing notice type "two" exists with description: "Extended Flushing Notice left"
    And a service material "one" exists with description: "Copper", is edit enabled: true
    And a service material "two" exists with description: "Plastic", is edit enabled: true
    And a service material "three" exists with description: "Iron", is edit enabled: true
    And a service size "one" exists with service size description: "1/2"
    And a service size "two" exists with service size description: "1"
    And a service size "three" exists with service size description: "2"
    And a pitcher filter customer delivery method "one" exists with description: "Handed to customer"
    And a pitcher filter customer delivery method "two" exists with description: "Left on porch\doorstep"
    And a pitcher filter customer delivery method "three" exists with description: "Other"
    And a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "service", work description: "install meter", work order priority: "emergency"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "ten"    
    When I click the "Additional" tab
    And I press Finalize
    Then I should see a validation message for CompletedDate with "The Date Completed field is required."
    Then I should see a validation message for MeterLocation with "The MeterLocation field is required."
    When I enter "6/24/2023" into the CompletedDate field
    And I select "one" from the MeterLocation dropdown
    And I select work description "service line renewal" from the FinalWorkDescription dropdown  
    And I enter 23 into the InitialServiceLineFlushTime field
    And I select "Yes" from the HasPitcherFilterBeenProvidedToCustomer dropdown
    And I enter "6/20/2023" into the DatePitcherFilterDeliveredToCustomer field
    And I select pitcher filter customer delivery method "three" from the PitcherFilterCustomerDeliveryMethod dropdown
    And I enter "Testing Other" into the PitcherFilterCustomerDeliveryOtherMethod field
    And I enter "6/21/2023" into the DateCustomerProvidedAWStateLeadInformation field
    And I select service material "one" from the PreviousServiceLineMaterial dropdown
    And I select service material "two" from the CompanyServiceLineMaterial dropdown
    And I select service material "three" from the CustomerServiceLineMaterial dropdown
    And I select service size "one" from the PreviousServiceLineSize dropdown
    And I select service size "two" from the CompanyServiceLineSize dropdown
    And I select service size "three" from the CustomerServiceLineSize dropdown
    And I enter "6/22/2023" into the DoorNoticeLeftDate field
    And I enter "7" into the LostWater field
    And I enter "15" into the DistanceFromCrossStreet field
    And I enter "additional notes" into the AppendNotes field
    And I select work order flushing notice type "two"'s Description from the FlushingNoticeType dropdown
    And I select "Yes" from the DigitalAsBuiltCompleted dropdown
    And I enter "6/24/2023" into the CompletedDate field
    And I press Finalize
    Then I should be at the FieldOperations/WorkOrderFinalization/Show page for work order "ten"
    And I should only see "one" in the MeterLocationInit element
    And I should only see "SERVICE LINE RENEWAL" in the WorkDescription element
    And I should only see "Extended Flushing Notice left" in the FlushingNoticeTypeInit element
    And I should only see "6/24/2023" in the DateCompleted element
    When I click the "Additional" tab
    Then I should see a display for LostWater with "7"
    And I should see a display for DistanceFromCrossStreet with "15"
    And I should see "additional notes"    
    And I should see a display for PreviousServiceLineMaterial with "Copper"
    And I should see a display for CompanyServiceLineMaterial with "Plastic"
    And I should see a display for CustomerServiceLineMaterial with "Iron"
    And I should see a display for PreviousServiceLineSize with "1/2"
    And I should see a display for CompanyServiceLineSize with "1"
    And I should see a display for CustomerServiceLineSize with "2"
    And I should see a display for DoorNoticeLeftDate with "6/22/2023"
    And I should see a display for InitialServiceLineFlushTime with "23"
    And I should see a display for InitialFlushTimeEnteredBy with "Roy"
    And I should see a display for InitialFlushTimeEnteredAt with a date time close to now
    And I should see a display for HasPitcherFilterBeenProvidedToCustomer with "Yes"    
    And I should see a display for PitcherFilterCustomerDeliveryMethod with "Other"
    And I should see a display for PitcherFilterCustomerDeliveryOtherMethod with "Testing Other"
    And I should see a display for DatePitcherFilterDeliveredToCustomer with "6/20/2023"
    And I should see a display for DateCustomerProvidedAWStateLeadInformation with "6/21/2023"

Scenario: User can finalize a work order with main break repair work description
    Given a work order flushing notice type "one" exists with description: "Standard Flushing Notice left"
    And a work order flushing notice type "two" exists with description: "Extended Flushing Notice left"
    And a customer impact range "one" exists with description: "101-200", Id: "5"
    And a customer impact range "two" exists with description: "51-100", Id: "6"
    And a repair time range "one" exists with description: "4-6"
    And a repair time range "two" exists with description: "8-10"
    And a service size "one" exists with service size description: "Size 1", size: "1"
    And a main break material "one" exists with description: "Cement"
    And a main condition "one" exists with description: "Good"
    And a main failure type "one" exists with description: "Pinhole"
    And a main break soil condition "one" exists with description: "Clay"
    And a main break disinfection method "one" exists with description: "Chlorination"
    And a main break flush method "one" exists with description: "Main"
    And a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "main", work description: "water main installation", work order priority: "emergency"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "ten"    
    When I click the "Additional" tab
    And I enter "7" into the LostWater field
    And I enter "15" into the DistanceFromCrossStreet field
    And I enter "additional notes" into the AppendNotes field
    And I select work description "water main break repair" from the FinalWorkDescription dropdown
    Then I should see the "Main Break" tab
    And I should see "You must enter the main break info before continuing."
    When I select work description "water main bleeders" from the FinalWorkDescription dropdown
    Then I should not see the "Main Break" tab
    And I should not see "You must enter the main break info before continuing."
    When I select work description "water main break repair" from the FinalWorkDescription dropdown
    And I select customer impact range "two" from the CustomerImpact dropdown
    And I select repair time range "one" from the RepairTime dropdown
    And I select "Yes" from the AlertIssued dropdown
    And I select "Yes" from the TrafficImpact dropdown
    And I select work order flushing notice type "two"'s Description from the FlushingNoticeType dropdown
    And I select "Yes" from the DigitalAsBuiltCompleted dropdown
    And I enter "6/24/2023" into the CompletedDate field  
    When I click the "Main Break" tab
    And I follow "Add Main Break"
    And I wait for the dialog to open
    And I select main break material "one" from the MainBreakMaterial dropdown
    And I select main condition "one" from the MainCondition dropdown
    And I select main failure type "one" from the MainFailureType dropdown
    And I select main break soil condition "one" from the MainBreakSoilCondition dropdown
    And I select main break disinfection method "one" from the MainBreakDisinfectionMethod dropdown
    And I select main break flush method "one" from the MainBreakFlushMethod dropdown
    And I select service size "one" from the ServiceSize dropdown
    And I enter 8.75 into the Depth field
    And I enter 21 into the CustomersAffected field
    And I enter 4.5 into the ShutdownTime field
    And I enter 3.25 into the ChlorineResidual field
    And I press "Save Main Break"
    Then I should not see "You must enter the main break info before continuing."
    When I press Finalize
    Then I should be at the FieldOperations/WorkOrderFinalization/Show page for work order "ten"
    And I should only see "WATER MAIN BREAK REPAIR" in the WorkDescription element
    And I should only see "Extended Flushing Notice left" in the FlushingNoticeTypeInit element
    And I should only see "6/24/2023" in the DateCompleted element
    And I should only see "51-100" in the EstimatedCustomerImpact element
    And I should only see "4-6" in the AnticipatedRepairTime element
    And I should only see "Yes" in the SignificantTrafficImpact element
    And I should only see "Yes" in the AlertIssuedDisplay element
    When I click the "Additional" tab
    Then I should see a display for LostWater with "7"
    And I should see a display for DistanceFromCrossStreet with "15"
    And I should see "additional notes"

Scenario: User can finalize a work order with service line retire work description
    Given a work order "five" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "service", work description: "install meter", work order priority: "emergency", sap work order number: "987654321"
    And a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
    And a crew assignment "one" exists with work order: "five", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "2"
    And a crew assignment "two" exists with work order: "five", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "3"
    And a service material "one" exists with description: "Copper", is edit enabled: true
    And a service size "one" exists with service size description: "1/2"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"
    When I click the "Additional" tab
    Then I should see a link to "Content/LeakageChart.pdf"
    And I should see a display for WorkOrder_TotalManHours with "3"
    And I should see a display for WorkOrder_Notes with "hey this is a note"    
    And I should see the link "Crew Assignments"
    And I should see the link "General"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Edit page for work order: "five"    
    And I should see "" in the AppendNotes field
    When I enter "7" into the LostWater field
    And I enter "15" into the DistanceFromCrossStreet field
    And I enter "additional notes" into the AppendNotes field
    And I select work description "service line retire" from the FinalWorkDescription dropdown
    And I select "-- Select --" from the CompanyServiceLineMaterial dropdown
    And I select "one" from the MeterLocation dropdown
    And I press Finalize
    Then I should not see a validation message for CompanyServiceLineMaterial with "The Service Company Material field is required."
    And I should not see a validation message for CompanyServiceLineSize with "The Service Company Size field is required."
    When I enter "6/24/2023" into the CompletedDate field
    And I press Finalize
    Then I should be at the FieldOperations/WorkOrderFinalization/Show page for work order: "five"
    When I click the "Additional" tab
    Then I should see a display for LostWater with "7"
    And I should see a display for DistanceFromCrossStreet with "15"
    And I should see "additional notes"
    And I should only see "6/24/2023" in the DateCompleted element

Scenario: User can view links for a work order
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "service", work description: "install meter", work order priority: "emergency"
    And a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
	And a crew assignment "one" exists with work order: "ten", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "42"
    And a crew assignment "two" exists with work order: "ten", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "12"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "ten"
    Then I should see "Create Service"
    And I should not see a link to the FieldOperations/CrewAssignment/ShowCalendar page
    When I follow "Create Service"
    Then I should be at the FieldOperations/Service/LinkOrNew page

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
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Materials" tab
    Then I should see the following values in the materialsUsedTable table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |
    | 987654321   | STYD           | 1300               | Copper      | 5        |
    | N/A         | STYD           | 1300               |  Test       | 7        |

Scenario: Users can delete materials for a work order
    Given a material "one" exists with PartNumber: "123456789", Description: "Plastic"
    And a material "two" exists with PartNumber: "987654321", Description: "Copper"
    And a stock location "one" exists with SAPStockLocation: "2600", Description: "H&M"
    And a stock location "two" exists with SAPStockLocation: "1300", Description: "STYD"
    And a material used "one" exists with Material: "one", WorkOrder: "two", StockLocation: "one", Quantity: 2
    And a material used "two" exists with Material: "two", WorkOrder: "two", StockLocation: "two", Quantity: 5
    And a material used "three" exists with WorkOrder: "two", StockLocation: "two", Quantity: 7, NonStockDescription: "Test"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Materials" tab
    Then I should see the following values in the materialsUsedTable table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |
    | 987654321   | STYD           | 1300               | Copper      | 5        |
    | N/A         | STYD           | 1300               |  Test       | 7        |
    When I click the "Delete" link in the 2nd row of materialsUsedTable and then click ok in the confirmation dialog
    And I wait for ajax to finish loading
    Then I should see the following values in the materialsUsedTable table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |
    | N/A         | STYD           | 1300               |  Test       | 7        |

Scenario: Users can add materials for a work order
    Given a material "one" exists with PartNumber: "123456789", Description: "Plastic"
    And a material "two" exists with PartNumber: "987654321", Description: "Copper"
    And a operating center stocked material "one" exists with OperatingCenter: "nj7", Material: "two"
    And a stock location "one" exists with SAPStockLocation: "2600", Description: "H&M"
    And a stock location "two" exists with SAPStockLocation: "1300", Description: "STYD"
    And a material used "one" exists with Material: "one", WorkOrder: "two", StockLocation: "one", Quantity: 2    
    And a material used "two" exists with WorkOrder: "two", StockLocation: "two", Quantity: 7, NonStockDescription: "Test"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Materials" tab
    Then I should see the following values in the materialsUsedTable table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |
    | N/A         | STYD           | 1300               |  Test       | 7        |
    When I follow "Add New Material"
    And I wait for the dialog to open
    And I enter 5 into the Quantity field
    And I select material "two" from the Material dropdown
    And I select stock location "two" from the StockLocation dropdown
    And I press "Save Material"
    Then I should see the following values in the materialsUsedTable table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |    
    | N/A         | STYD           | 1300               |  Test       | 7        |
    | 987654321   | STYD           | 1300               | Copper      | 5        |
    When I click the "Edit" link in the 3rd row of materialsUsedTable
    And I wait for the dialog to open
    And I enter 8 into the Quantity field
    And I press "Save Material"
    Then I should see the following values in the materialsUsedTable table
    | Part Number | Stock Location | SAP Stock Location | Description | Quantity |
    | 123456789   | H&M            | 2600               | Plastic     | 2        |    
    | N/A         | STYD           | 1300               |  Test       | 7        |
    | 987654321   | STYD           | 1300               | Copper      | 8        |

#Spoils
Scenario: Users can view spoils for a work order
    Given a spoil storage location "one" exists with Name: "Newman Springs Yard"
    And a spoil storage location "two" exists with Name: "Union Beach Yard"
    And a spoil "one" exists with SpoilStorageLocation: "two", WorkOrder: "two", Quantity: 1.25
    And a spoil "two" exists with SpoilStorageLocation: "one", WorkOrder: "two", Quantity: 2.75
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Spoils" tab
    Then I should see the following values in the spoilsTable table
    | Quantity (CY) | Spoil Storage Location |
    | 1.25          | Union Beach Yard       |
    | 2.75          | Newman Springs Yard    |

Scenario: User can delete spoils for a work order
    Given a spoil storage location "one" exists with Name: "Newman Springs Yard"
    And a spoil storage location "two" exists with Name: "Union Beach Yard"
    And a spoil "one" exists with SpoilStorageLocation: "two", WorkOrder: "two", Quantity: 1.25
    And a spoil "two" exists with SpoilStorageLocation: "one", WorkOrder: "two", Quantity: 2.75
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Spoils" tab
    Then I should see the following values in the spoilsTable table
    | Quantity (CY) | Spoil Storage Location |
    | 1.25          | Union Beach Yard       |
    | 2.75          | Newman Springs Yard    |
    When I click the "Delete" link in the 2nd row of spoilsTable and then click ok in the confirmation dialog
    And I wait for ajax to finish loading
    Then I should see the following values in the spoilsTable table
    | Quantity (CY) | Spoil Storage Location |
    | 1.25          | Union Beach Yard       |

Scenario: User can add spoils for a work order
    Given a spoil storage location "one" exists with Name: "Newman Springs Yard", OperatingCenter: "nj7", Active: "true"
    And a spoil storage location "two" exists with Name: "Union Beach Yard", OperatingCenter: "nj6", Active: "true"
    And a spoil "one" exists with SpoilStorageLocation: "two", WorkOrder: "two", Quantity: 1.25
    And a spoil "two" exists with SpoilStorageLocation: "one", WorkOrder: "two", Quantity: 2.75
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Spoils" tab
    Then I should see the following values in the spoilsTable table
    | Quantity (CY) | Spoil Storage Location |
    | 1.25          | Union Beach Yard       |
    | 2.75          | Newman Springs Yard    |
    When I follow "Add Spoil"
    And I wait for the dialog to open
    Then I should not see spoil storage location "two"'s Name in the SpoilStorageLocation dropdown
    When I enter 8 into the Quantity field    
    And I select spoil storage location "one" from the SpoilStorageLocation dropdown
    And I press "Save Spoil"
    Then I should see the following values in the spoilsTable table
    | Quantity (CY) | Spoil Storage Location |
    | 1.25          | Union Beach Yard       |
    | 2.75          | Newman Springs Yard    |
    | 8             | Newman Springs Yard    |
    When I click the "Edit" link in the 1st row of spoilsTable
    And I wait for the dialog to open
    And I enter 5 into the Quantity field
    And I select spoil storage location "one" from the SpoilStorageLocation dropdown
    And I press "Save Spoil"
    Then I should see the following values in the spoilsTable table
    | Quantity (CY) | Spoil Storage Location |
    | 5             | Newman Springs Yard    |
    | 2.75          | Newman Springs Yard    |
    | 8             | Newman Springs Yard    |

# Markout Violations
Scenario: Users can view markout violations for a work order
    Given a markout violation "one" exists with DateOfViolationNotice: "2019-02-05 00:00:00.000", MarkoutViolationStatus: "Pending review - Jeff Bowlby", MarkoutRequestNumber: "162022169", WorkOrder: "two"
    And a markout violation "two" exists with DateOfViolationNotice: "2018-06-28 00:00:00.000", MarkoutViolationStatus: "Completed", MarkoutRequestNumber: "190740395", WorkOrder: "two"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Markout Violations" tab
    Then I should see the following values in the markoutViolations table
    | Date Of Violation Notice    | Markout Violation Status       | Markout Request Number          |
    | 2/5/2019                    | Pending review - Jeff Bowlby   |   162022169                     |
    | 6/28/2018                   | Completed                      |   190740395                     |
    And I should see a link to the Show page for markout violation "one"
    And I should see a link to the Show page for markout violation "two"
    And I should see a link to the Edit page for markout violation "one"
    And I should see a link to the Edit page for markout violation "two"
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
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Markout Damages" tab
    Then I should see the following values in the markoutDamages table
    | Request Number | Damaged On Date    | Excavator | Utilities Damaged |
    | 123456789      | 2/5/2019 12:00 AM  | PSE&G     | Sewer             |
    | 987654321      | 6/28/2018 12:00 AM | J F KIELY | Water, Gas        |
    And I should see a link to the /FieldOperations/MarkoutDamage/New/2 page
    And I should see a link to the Show page for markout damage "one"
    And I should see a link to the Show page for markout damage "two"

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
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    Then I should not see the "Sewer Overflows" tab
    When I visit the FieldOperations/WorkOrderFinalization/Edit page for work order: "three"
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
    When I follow "Create Sewer Overflow"
    And I close the first browser tab
    Then I should be at the FieldOperations/SewerOverflow/New page
    And I should see operating center "nj7"'s Description in the OperatingCenter dropdown
    And I should see town "nj7burg" in the Town dropdown
    And I should see street "one"'s Description in the Street dropdown
    And I should see street "two"'s Description in the CrossStreet dropdown

# Traffic Control
Scenario: Users can view traffic control tickets for a work order
    Given a billing party "one" exists with Description: "ATLANTIC County"
    And a billing party "two" exists with Description: "ALLEGHENY County"
    And a traffic control ticket "one" exists with WorkStartDate: "2023-02-10", TotalHours: "1.85", NumberOfOfficers: "4", WorkOrder: "two", BillingParty: "two"
    And a traffic control ticket "two" exists with WorkStartDate: "2023-03-28", TotalHours: "3.15", NumberOfOfficers: "9", WorkOrder: "two", BillingParty: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Traffic Control" tab
    Then I should see a display for TrafficControlRequired with "No"
    And I should see a display for Notes with "hey this is a note"
    And I should see the following values in the trafficeControlTickets table
    | Work Start Date    | Total Combined Hours | Number of Officers or Employees | Billing Party      |
    | 2/10/2023          | 1.85                 | 4                               | ALLEGHENY County   |
    | 3/28/2023          | 3.15                 | 9                               | ATLANTIC County    |
    And I should see a link to the /FieldOperations/TrafficControlTicket/New?workOrderId=2 page
    And I should see a link to the Show page for traffic control ticket "one"
    And I should see a link to the Show page for traffic control ticket "two"

# Restoration
Scenario: Users can view restorations for a work order
    Given a restoration type "one" exists with description: "ASPHALT - ALLEY"
    And a restoration type "two" exists with description: "BRICK - STREET"
    And a restoration response priority "one" exists with description: "Emergency 5 day"
    And a restoration response priority "two" exists with description: "Priority (10 days)"
    And a restoration "one" exists with RestorationType: "two", PavingSquareFootage: "28.00", LinearFeetOfCurb: "6.00", WorkOrder: "two", PartialRestorationDate: "2001-11-16", FinalRestorationDate: "2000-12-31", ResponsePriority: "one", AssignedContractor: "one"
    And a restoration "two" exists with RestorationType: "one", PavingSquareFootage: "25.00", LinearFeetOfCurb: "7.00", WorkOrder: "two", PartialRestorationDate: "2000-12-31", FinalRestorationDate: "2003-09-12", ResponsePriority: "two", AssignedContractor: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Restoration" tab
    Then I should see the following values in the restorations table
    | Type of Restoration | Paving Square Footage | Linear Feet Of Curb | Initial Date     | Final Date    | Response Priority  | Assigned Contractor |
    | BRICK - STREET      | 28.00                 | 6.00                | 11/16/2001       | 12/31/2000    | Emergency 5 day    | Sunil               |
    | ASPHALT - ALLEY     | 25.00                 | 7.00                | 12/31/2000       | 9/12/2003     | Priority (10 days) | Sunil               |
    And I should see a link to the show page for restoration "one"
    And I should see a link to the edit page for restoration "one"
    And I should see a link to the show page for restoration "two"
    And I should see a link to the edit page for restoration "two"
    And I should see a link to the /FieldOperations/Restoration/New/2 page
    
# Crew Assignments
Scenario: Users can view crew assignments for a work order
    Given a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
    And a crew assignment "one" exists with work order: "two", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "42"
    And a crew assignment "two" exists with work order: "two", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "12"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Crew Assignments" tab
    Then I should see the following values in the assignmentsTable table
    | Assigned On             | Assigned For             | Assigned To  | Start Time                  | End Time                    | Employees on Crew|
    | 1/11/2023 1:59 PM (EST) | 1/29/2023 3:00 AM (EST)  | Sunil        | 4/10/2022 2:22:00 PM (EST)  | 4/10/2022 3:07:00 PM (EST)  | 42               |
    | 3/16/2023 2:06 PM (EST) | 3/29/2023 12:00 AM (EST) | Sai          | 4/8/2022 11:38:00 PM (EST)  | 4/9/2022 12:49:00 AM (EST)  | 12               |

Scenario: Users can end crew assignments for a work order
    Given a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
    And a crew assignment "one" exists with work order: "two", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Crew Assignments" tab
    And I enter "42" into the EmployeesOnJob field 
	And I press the end assignment link for crew assignment: "one"
    Then I should be at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    And I should see the following values in the assignmentsTable table
    | Assigned On             | Assigned For             | Assigned To  | Start Time                  | Employees on Crew|
    | 1/11/2023 1:59 PM (EST) | 1/29/2023 3:00 AM (EST)  | Sunil        | 4/10/2022 2:22:00 PM (EST)  | 42               |

# Additional
Scenario: Users can view and update fields on additional tab for a work order
    Given a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
    And a crew assignment "one" exists with work order: "two", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "2"
    And a crew assignment "two" exists with work order: "two", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "3"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Additional" tab
    Then I should see a link to "Content/LeakageChart.pdf"
    And I should see a display for WorkOrder_TotalManHours with "3"
    And I should see a display for WorkOrder_Notes with "hey this is a note"    
    And I should see the link "Crew Assignments"
    And I should see the link "General"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Edit page for work order: "two"
    And I should see "5" in the LostWater field
    And I should see "12" in the DistanceFromCrossStreet field
    And I should see "" in the AppendNotes field
    When I enter "7" into the LostWater field
    And I enter "15" into the DistanceFromCrossStreet field
    And I enter "additional notes" into the AppendNotes field
    And I press Update
    Then I should be at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Additional" tab
    Then I should see "7" in the LostWater field
    And I should see "15" in the DistanceFromCrossStreet field
    And I should see "" in the AppendNotes field
    And I should see "additional notes"
    When I press CrewAssignments
    Then I should be at the FieldOperations/CrewAssignment/ShowCalendar page

# Schedule of Values
Scenario: User can view schedule of values for a work order
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", sap enabled: "true", sap work orders enabled: "false", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a town section "belmar" exists with town: "wall", name: "West Belmar"
    And a street "ace" exists with town: "wall", full st name: "ACE DRIVE", is active: true
    And a work order "four" exists with operating center: "nj4", town: "wall", street: "ace", asset type: "valve", valve: "one", work order priority: "emergency"
    And a schedule of value category "one" exists with description: "Saw Cut"
    And a schedule of value category "two" exists with description: "Curb Box"
    And a unit of measure "one" exists with description: "Gallons"
    And a unit of measure "two" exists with description: "HR"
    And a schedule of value "one" exists with ScheduleOfValueCategory: "one", Description: "Cold Patch", UnitOfMeasure: "one"
    And a schedule of value "two" exists with ScheduleOfValueCategory: "two", Description: "Partial Excavation", UnitOfMeasure: "two"
    And a work order schedule of value "one" exists with WorkOrder: "four", ScheduleOfValue: "one", OtherDescription: "Chem pump parts", IsOvertime: "true"
    And a work order schedule of value "two" exists with WorkOrder: "four", ScheduleOfValue: "two", OtherDescription: "Total cost of repair by contractor", IsOvertime: "false"
    And a role "workorder-read-nj4" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
    And a role "workorder-edit-nj4" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    Then I should not see the "Schedule Of Values" tab
    When I visit the FieldOperations/WorkOrderFinalization/Edit page for work order: "four"
    Then I should see the "Schedule Of Values" tab
    When I click the "Schedule Of Values" tab
    Then I should see the following values in the scheduleOfValuesTable table
    | Schedule Of Value Category | Schedule Of Value  | Other Description                  | Unit Of Measure | Is Overtime |
    | Saw Cut                    | Cold Patch         | Chem pump parts                    | Gallons         | True        |
    | Curb Box                   | Partial Excavation | Total cost of repair by contractor | HR              | False       |

Scenario: User can delete schedule of values for a work order
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", sap enabled: "true", sap work orders enabled: "false", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a town section "belmar" exists with town: "wall", name: "West Belmar"
    And a street "ace" exists with town: "wall", full st name: "ACE DRIVE", is active: true
    And a work order "four" exists with operating center: "nj4", town: "wall", street: "ace", asset type: "valve", valve: "one", work order priority: "emergency"
    And a schedule of value category "one" exists with description: "Saw Cut"
    And a schedule of value category "two" exists with description: "Curb Box"
    And a unit of measure "one" exists with description: "Gallons"
    And a unit of measure "two" exists with description: "HR"
    And a schedule of value "one" exists with ScheduleOfValueCategory: "one", Description: "Cold Patch", UnitOfMeasure: "one"
    And a schedule of value "two" exists with ScheduleOfValueCategory: "two", Description: "Partial Excavation", UnitOfMeasure: "two"
    And a work order schedule of value "one" exists with WorkOrder: "four", ScheduleOfValue: "one", OtherDescription: "Chem pump parts", IsOvertime: "true"
    And a work order schedule of value "two" exists with WorkOrder: "four", ScheduleOfValue: "two", OtherDescription: "Total cost of repair by contractor", IsOvertime: "false"
    And a role "workorder-read-nj4" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
    And a role "workorder-edit-nj4" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    Then I should not see the "Schedule Of Values" tab
    When I visit the FieldOperations/WorkOrderFinalization/Edit page for work order: "four"
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
    Given an operating center "nj4" exists with opcode: "NJ4", name: "Howell", sap enabled: "true", sap work orders enabled: "false", arc mobile map id: "15fdc279b4234fcb85f455ee70897a9e", hasWorkOrderInvoicing: true
    And a town "wall" exists with name: "WALL"
    And operating center: "nj4" exists in town: "wall"
    And a town section "belmar" exists with town: "wall", name: "West Belmar"
    And a street "ace" exists with town: "wall", full st name: "ACE DRIVE", is active: true
    And a work order "four" exists with operating center: "nj4", town: "wall", street: "ace", asset type: "valve", valve: "one", work order priority: "emergency"
    And a schedule of value category "one" exists with description: "Saw Cut"
    And a schedule of value category "two" exists with description: "Curb Box"
    And a unit of measure "one" exists with description: "Gallons"
    And a unit of measure "two" exists with description: "HR"
    And a schedule of value "one" exists with ScheduleOfValueCategory: "one", Description: "Cold Patch", UnitOfMeasure: "one"
    And a schedule of value "two" exists with ScheduleOfValueCategory: "two", Description: "Partial Excavation", UnitOfMeasure: "two"
    And a work order schedule of value "one" exists with WorkOrder: "four", ScheduleOfValue: "one", OtherDescription: "Chem pump parts", IsOvertime: "true"
    And a work order schedule of value "two" exists with WorkOrder: "four", ScheduleOfValue: "two", OtherDescription: "Total cost of repair by contractor", IsOvertime: "false"
    And a role "workorder-read-nj4" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
    And a role "workorder-edit-nj4" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj4"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    Then I should not see the "Schedule Of Values" tab
    When I visit the FieldOperations/WorkOrderFinalization/Edit page for work order: "four"
    Then I should see the "Schedule Of Values" tab
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
Scenario: Users can view job site check lists for a work order
    Given a job site check list "one" exists with map call work order: "two", CheckListDate: "01/22/2023", CreatedBy: "Sunil"
    And a job site check list "two" exists with map call work order: "two", CheckListDate: "03/21/2023", CreatedBy: "Sai"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    When I click the "Job Site Check Lists" tab
    Then I should see the following values in the jobSiteCheckLists table
    | Check List Date | Created By |
    | 1/22/2023       | Sunil      |
    | 3/21/2023       | Sai        |
    And I should see a link to the show page for job site check list "one"
    And I should see a link to the show page for job site check list "two"

# Meters
Scenario: User can view meters for a work order
    Given a work order "five" exists with operating center: "nj6", town: "nj7burg", street: "one", asset type: "service", work description: "install meter", work order priority: "emergency", sap work order number: "987654321"
    And a service installation "one" exists with work order: "five", MeterManufacturerSerialNumber: "1234567890", MeterLocationInformation: "9876543210"
    And a service installation "two" exists with work order: "five", MeterManufacturerSerialNumber: "9876543210", MeterLocationInformation: "1234567890"
    And a role "workorder-edit-nj6" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj6"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"
    Then I should see the "Set Meter" tab
    When I click the "Set Meter" tab
    Then I should see the following values in the meters table
    | Id | Meter Manufacturer Serial Number | Meter Location Information |
    | 1  | 1234567890                       |  9876543210                |
    | 2  | 9876543210                       |  1234567890                |
    And I should see a link to the show page for service installation "one"
    And I should see a link to the show page for service installation "two"
    And I should not see a link to the /FieldOperations/ServiceInstallation/New/5 page

Scenario: User should be able to create meters for a work order
    Given a work order "five" exists with operating center: "nj6", town: "nj7burg", street: "one", asset type: "service", work description: "service line installation", work order priority: "emergency", sap work order number: "987654321"
    And a role "workorder-edit-nj6" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj6"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"
    Then I should see the "Set Meter" tab
    When I click the "Set Meter" tab
    Then I should see a link to the /FieldOperations/ServiceInstallation/New/5 page

# Main Breaks
Scenario: User can add main break for a work order
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "main", work description: "water main break repair", work order priority: "emergency"
    And a service size "one" exists with service size description: "Size 1", size: "1"
    And a main break material "one" exists with description: "Cement"
    And a main break material "two" exists with description: "Cast Iron"
    And a main condition "one" exists with description: "Good"
    And a main failure type "one" exists with description: "Pinhole"
    And a main break soil condition "one" exists with description: "Clay"
    And a main break disinfection method "one" exists with description: "Chlorination"
    And a main break flush method "one" exists with description: "Main"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "ten"
    When I click the "Main Break" tab
    And I follow "Add Main Break"
    And I wait for the dialog to open
    Then the FootageReplaced field should be disabled
    And the ReplacedWith field should be disabled
    When I press "Save Main Break"
    Then I should see a validation message for MainBreakMaterial with "The Existing Material field is required."
    And I should see a validation message for ServiceSize with "The Size field is required."
    And I should see a validation message for MainCondition with "The MainCondition field is required."
    And I should see a validation message for MainFailureType with "The Failure Type field is required."
    And I should see a validation message for MainBreakSoilCondition with "The Soil Condition field is required."
    And I should see a validation message for MainBreakDisinfectionMethod with "The Disinfection Method field is required."
    And I should see a validation message for MainBreakFlushMethod with "The Flush Method field is required."
    And I should see a validation message for Depth with "The Depth(in.) field is required."
    And I should see a validation message for CustomersAffected with "The CustomersAffected field is required."
    And I should see a validation message for ShutdownTime with "The Shut Down Time(Hrs) field is required."
    And I should see a validation message for ChlorineResidual with "The ChlorineResidual field is required."
    When I select main break material "one" from the MainBreakMaterial dropdown
    And I select main condition "one" from the MainCondition dropdown
    And I select main failure type "one" from the MainFailureType dropdown
    And I select main break soil condition "one" from the MainBreakSoilCondition dropdown
    And I select main break disinfection method "one" from the MainBreakDisinfectionMethod dropdown
    And I select main break flush method "one" from the MainBreakFlushMethod dropdown
    And I select service size "one" from the ServiceSize dropdown
    And I enter 8.75 into the Depth field
    And I enter 21 into the CustomersAffected field
    And I enter 4.5 into the ShutdownTime field
    And I enter 3.25 into the ChlorineResidual field
    And I press "Save Main Break"
    Then I should see the following values in the mainBreaksTable table
    | Size   | Existing Material | Main Condition | Failure Type | Depth(in.) | Soil Condition | Customers Affected | Shut Down Time(Hrs) | Disinfection Method | Flush Method | Chlorine Residual | Boil Alert Issued |
    | Size 1 | Cement            | Good           | Pinhole      | 8.75       | Clay           | 21                 | 4.5                 | Chlorination        | Main         | 3.25              | False             |

Scenario: User can edit main breaks for a work order
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "main", work description: "water main break repair", work order priority: "emergency"
    And a service size "one" exists with service size description: "Size 1", size: "1"
    And a main break material "one" exists with description: "Cement"
    And a main break material "two" exists with description: "Cast Iron"
    And a main condition "one" exists with description: "Good"
    And a main failure type "one" exists with description: "Pinhole"
    And a main break soil condition "one" exists with description: "Clay"
    And a main break disinfection method "one" exists with description: "Chlorination"
    And a main break flush method "one" exists with description: "Main"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "ten"
    When I click the "Main Break" tab
    And I follow "Add Main Break"
    And I wait for the dialog to open
    Then the FootageReplaced field should be disabled
    And the ReplacedWith field should be disabled
    When I select main break material "one" from the MainBreakMaterial dropdown
    And I select main condition "one" from the MainCondition dropdown
    And I select main failure type "one" from the MainFailureType dropdown
    And I select main break soil condition "one" from the MainBreakSoilCondition dropdown
    And I select main break disinfection method "one" from the MainBreakDisinfectionMethod dropdown
    And I select main break flush method "one" from the MainBreakFlushMethod dropdown
    And I select service size "one" from the ServiceSize dropdown
    And I enter 8.75 into the Depth field
    And I enter 21 into the CustomersAffected field
    And I enter 4.5 into the ShutdownTime field
    And I enter 3.25 into the ChlorineResidual field
    And I press "Save Main Break"
    Then I should see the following values in the mainBreaksTable table
    | Size   | Existing Material | Main Condition | Failure Type | Depth(in.) | Soil Condition | Customers Affected | Shut Down Time(Hrs) | Disinfection Method | Flush Method | Chlorine Residual | Boil Alert Issued |
    | Size 1 | Cement            | Good           | Pinhole      | 8.75       | Clay           | 21                 | 4.5                 | Chlorination        | Main         | 3.25              | False             |
    When I click the "Edit" link in the 1st row of mainBreaksTable
    And I wait for the dialog to open
    And I select main break material "two" from the MainBreakMaterial dropdown
    And I press "Save Main Break"
    And I wait for ajax to finish loading
    Then I should see the following values in the mainBreaksTable table
    | Size   | Existing Material | Main Condition | Failure Type | Depth(in.) | Soil Condition | Customers Affected | Shut Down Time(Hrs) | Disinfection Method | Flush Method | Chlorine Residual | Boil Alert Issued |
    | Size 1 | Cast Iron         | Good           | Pinhole      | 8.75       | Clay           | 21                 | 4.5                 | Chlorination        | Main         | 3.25              | False             |

Scenario: User can delete main break for a work order
    Given a work order "ten" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "main", work description: "water main break repair", work order priority: "emergency"
    And a main break "one" exists with work order: "ten", CustomersAffected: "24", ShutdownTime: "4.5"
    And a main break "two" exists with work order: "ten", CustomersAffected: "13", ShutdownTime: "6.0"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "ten"
    When I click the "Main Break" tab
    Then I should see the following values in the mainBreaksTable table
    | Customers Affected | Shut Down Time(Hrs) |
    | 24                 | 4.5                 |
    | 13                 | 6                   |
    When I click the "Delete" link in the 1st row of mainBreaksTable and then click ok in the confirmation dialog
    And I wait for ajax to finish loading
    Then I should see the following values in the mainBreaksTable table
    | Customers Affected | Shut Down Time(Hrs) |
    | 13                 | 6                   |

Scenario: user can view and not edit hydrant details
    Given a hydrant "hydrant" exists with street: "one", town: "nj7burg", operating center: "nj7", hydrant suffix: "42" 
    And a work order "hydrant" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "hydrant", hydrant: "hydrant"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "hydrant"
    When I click the "Hydrant" tab
    Then I should not see the hydrantEditButton element 
    When I switch to the hydrantFrame frame
    Then I should see a display for HydrantSuffix with "42"

Scenario: user can view and edit hydrant details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a hydrant "hydrant" exists with street: "one", town: "nj7burg", operating center: "nj7", hydrant suffix: "42", hydrant number: "HAB-42"
    And a work order "hydrant" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "hydrant", hydrant: "hydrant"
    And a functional location "one" exists with town: "nj7burg", asset type: "hydrant"
    And a fire district "one" exists with district name: "meh"
    And a fire district town "foo" exists with town: "nj7burg", fire district: "one"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "hydrant"
    When I click the "Hydrant" tab
    When I press "hydrantEditButton"
    And I switch to the hydrantFrame frame
    And I select fire district "one" from the FireDistrict dropdown
    And I select functional location "one" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a display for FunctionalLocation with functional location "one"
    
Scenario: user can view and not edit valve details
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "one"
    When I click the "Valve" tab
    Then I should not see the valveEditButton element 
    When I switch to the valveFrame frame
    Then I should see a display for ValveType with ""

Scenario: user can view and edit valve details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a valve type "one" exists with description: "GATE"
    And a valve normal position "one" exists with description: "CLOSED"
    And a functional location "one" exists with town: "nj7burg", asset type: "valve"
    And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
    And a valve "details" exists with operating center: "nj7", town: "nj7burg", street: "one", valve number: "VAB-200", valve suffix: 200, date installed: "yesterday", turns: 4
    And a work order "details" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "valve", valve: "details", work order priority: "emergency"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "details"
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

Scenario: user can view and not edit sewer opening details
    Given a sewer opening "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", opening number: "MAD-42"
    And a work order "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Sewer Opening" tab
    Then I should not see the sewerOpeningEditButton element 
    When I switch to the sewerOpeningFrame frame
    Then I should see a display for OpeningNumber with "MAD-42"

Scenario: user can view and edit sewer opening details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
    And a sewer opening "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", opening number: "MAD-42"
    And a work order "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening"
    And a functional location "one" exists with town: "nj7burg", asset type: "sewer opening"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Sewer Opening" tab
    When I press "sewerOpeningEditButton"
    And I switch to the sewerOpeningFrame frame
    And I select functional location "one" from the FunctionalLocation dropdown
    And I press Save
    Then I should see a display for FunctionalLocation with functional location "one"

# Street Opening Permit
Scenario: User should be not able to see street opening permit links for a work order
    Given a work order "five" exists with operating center: "nj6", town: "nj7burg", street: "one", asset type: "service", work description: "service line installation", work order priority: "emergency", sap work order number: "987654321", s o p required: "true"
    And a role "workorder-edit-nj6" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj6"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"
    When I click the "Street Opening Permit" tab
    Then I should not see "Add Street Opening Permit"
    And I should not see "Submit New Permit"

Scenario: User should be able to update additional data when service line renewal description is selected 
    Given a work order "five" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "service", work description: "install meter", work order priority: "emergency", sap work order number: "987654321"
    And a crew "one" exists with description: "Sunil", operating center: "nj7", active: true
    And a crew "two" exists with description: "Sai", operating center: "nj7", active: true
    And a crew assignment "one" exists with work order: "five", crew: "one", assigned for: "2023-01-29 03:00:00", assigned on: "2023-01-11 13:59:00", date started: "2022-04-10 14:22:00", date ended: "2022-04-10 15:07:00", employees on job: "2"
    And a crew assignment "two" exists with work order: "five", crew: "two", assigned for: "2023-03-29 00:00:00", assigned on: "2023-03-16 14:06:00", date started: "2022-04-08 23:38:00", date ended: "2022-04-09 00:49:00", employees on job: "3"
    And a service material "one" exists with description: "Copper"
    And a service size "one" exists with service size description: "1/2"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"
    When I click the "Additional" tab
    Then I should see a link to "Content/LeakageChart.pdf"
    And I should see a display for WorkOrder_TotalManHours with "3"
    And I should see a display for WorkOrder_Notes with "hey this is a note"    
    And I should see the link "Crew Assignments"
    And I should see the link "General"
    And I should see a link to the FieldOperations/GeneralWorkOrder/Edit page for work order: "five"    
    And I should see "" in the AppendNotes field
    When I enter "7" into the LostWater field
    And I enter "15" into the DistanceFromCrossStreet field
    And I enter "additional notes" into the AppendNotes field
    And I select work description "service line renewal" from the FinalWorkDescription dropdown
    And I press Update
    Then I should be at the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"
    When I click the "Additional" tab
    Then I should see "7" in the LostWater field
    And I should see "15" in the DistanceFromCrossStreet field
    And I should see "" in the AppendNotes field
    And I should see "additional notes"

Scenario: User cannot select a revisit description for a non-revisit order
    Given a work order "five" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "service", work description: "install meter", work order priority: "emergency", sap work order number: "987654321"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"
    When I click the "Additional" tab
    Then I should not see work description "service landscaping" in the FinalWorkDescription dropdown
    And I should see work description "service relocation" in the FinalWorkDescription dropdown

Scenario: User cannot select a non-revisit description for a revisit order
    Given a work order "five" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "service", work description: "service landscaping", work order priority: "emergency", sap work order number: "987654321"
    And I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "five"
    When I click the "Additional" tab
    Then I should not see work description "service relocation" in the FinalWorkDescription dropdown
    And I should see work description "service restoration repair" in the FinalWorkDescription dropdown

Scenario: User can view special instruction on work order finalization page
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrderFinalization/Edit page for work order: "two"
    Then I should at least see work order "two"'s Id in the WorkOrderId element
    And I should only see work order "two"'s SpecialInstructions in the WorkOrderSpecialInstructions element

Scenario: user can view and not edit service details
    Given a sewer opening "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", opening number: "MAD-42"
	And a service category "one" exists with description: "Neato"
    And a service "unique" exists with service number: "123456", premise number: "9876543", date installed: "4/24/1984", service category: "one"
    And a work order "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening", service: "unique"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Service" tab
    Then I should not see the serviceEditButton element 
    When I switch to the serviceFrame frame
    Then I should see a display for ServiceType with service "unique"'s ServiceType

Scenario: UserAdmin can view and edit service details
    Given a role "asset-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a sewer opening "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", opening number: "MAD-42"
    And a service category "one" exists with description: "Neato"
    And a service "unique" exists with service number: "123456", date installed: "4/24/1984", service category: "one"
    And a work order "opening" exists with operating center: "nj7", town: "nj7burg", street: "one", asset type: "sewer opening", sewer opening: "opening", service: "unique"
    And I am logged in as "user"
    And I am at the FieldOperations/GeneralWorkOrder/Show page for work order: "opening"
    When I click the "Service" tab
    When I press "serviceEditButton"
    When I switch to the serviceFrame frame
    And I select town "nj7burg" from the Town dropdown
    And I follow "Cancel"
    Then I should see a display for ServiceNumber with "123456"

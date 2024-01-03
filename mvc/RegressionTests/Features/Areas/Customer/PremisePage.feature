Feature: Premise Page

Background: 
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
	And a town "nj7burg" exists with name: "Loch Arbour"
	And a coordinate "one" exists
	And a state "one" exists with abbreviation: "NJ" 
	And a state "two" exists with name: "Pennsylvania", abbreviation: "PA"
	And operating center: "nj7" exists in town: "nj7burg"
	And work order requesters exist
	And work order purposes exist
	And work order priorities exist
	And work descriptions exist
	And service utility types exist
    And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "main" exists with description: "main"
    And an asset type "service" exists with description: "service"
    And an asset type "sewer opening" exists with description: "sewer opening"
	And an asset type "sewer lateral" exists with description: "sewer lateral"
	And operating center: "nj7" has asset type "valve"
    And operating center: "nj7" has asset type "hydrant"
    And operating center: "nj7" has asset type "main"
    And operating center: "nj7" has asset type "service"
    And operating center: "nj7" has asset type "sewer opening"
    And operating center: "nj7" has asset type "sewer lateral"
    And a town section "one" exists with town: "nj7burg"
    And a street "one" exists with town: "nj7burg", full st name: "Easy St", is active: true
    And a user "user" exists with username: "user"
	And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesImages", user: "user", operating center: "nj7"
	And a role "roleAdd" exists with action: "Add", module: "FieldServicesImages", user: "user", operating center: "nj7"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesImages", user: "user", operating center: "nj7"
	And a role "roleReadAssets" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "roleAddAssets" exists with action: "Add", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "roleEditAssets" exists with action: "Edit", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "roleReadShortCycleWorkOrders" exists with action: "Read", module: "FieldServicesShortCycle", user: "user", operating center: "nj7"
	And a role "roleAddShortCycleWorkOrders" exists with action: "Add", module: "FieldServicesShortCycle", user: "user", operating center: "nj7"
	And a role "roleEditShortCycleWorkOrders" exists with action: "Edit", module: "FieldServicesShortCycle", user: "user", operating center: "nj7"
	And an e p a code "lead" exists with description: "LEAD"
	And an e p a code "not lead" exists with description: "NOT LEAD"			
	And an e p a code "lead status unknown" exists with description: "LEAD STATUS UNKNOWN"
	And a service size "one" exists with service size description: "Size 1", size: "1"
	And a service material "one" exists with description: "I am a material", customer e p a code: "lead", company e p a code: "lead"
	And a service size "two" exists with service size description: "Size 2", size: "2"
	And a service material "two" exists with description: "I am a material2", customer e p a code: "not lead", company e p a code: "not lead"
	And a service size "three" exists with service size description: "Size 3", size: "3"
	And a service material "three" exists with description: "I am a material3", customer e p a code: "lead status unknown", company e p a code: "lead status unknown"
	And a service category "one" exists with description: "Fire Retire Service Only"
	And a service category "two" exists with description: "Fire Service Installation"
	And a service category "three" exists with description: "Fire Service Renewal"
	And a service "one" exists with service number: "123", operating center: "nj7", town: "nj7burg", street: "one", cross street: "one", town section: "one", premise number: "5000206788", installation: "12345"
	And a tap image "one" exists with service number: "123456", apartment number: "Garbage1", PremiseNumber: "5000206788", service: "one", OperatingCenter: "nj7", DateCompleted: "06/30/2021", LengthOfService: "2", ServiceMaterial: "one", PreviousServiceMaterial: "one", CustomerSideMaterial: "one", ServiceSize: "one", PreviousServiceSize: "one", CustomerSideSize: "one", ServiceType: "servicetype1", IsDefaultImageForService: "true", OfficeReviewRequired: "true"
	And a tap image "two" exists with service number: "234567", apartment number: "Garbage2", PremiseNumber: "5000206788", OperatingCenter: "nj7", DateCompleted: "06/30/2021", LengthOfService: "2", ServiceMaterial: "two", PreviousServiceMaterial: "two", CustomerSideMaterial: "two", ServiceSize: "two", PreviousServiceSize: "two", CustomerSideSize: "two", ServiceType: "servicetype2", IsDefaultImageForService: "true", OfficeReviewRequired: "false"
	And a tap image "three" exists with service number: "345678", apartment number: "Garbage3", PremiseNumber: "5000206788", OperatingCenter: "nj7", DateCompleted: "06/30/2021", LengthOfService: "2", ServiceMaterial: "three", PreviousServiceMaterial: "three", CustomerSideMaterial: "three", ServiceSize: "three", PreviousServiceSize: "three", CustomerSideSize: "three", ServiceType: "servicetype3", IsDefaultImageForService: "false", OfficeReviewRequired: "false"
	And a region code "one" exists with s a p code: "1300", description: "nj7burg"
	And a premise "one" exists with premise number: "5000206788", is major account: "true", service address house number: "7", service address apartment: "garbage", service address street: "EaSy St", service address fraction: "1/2", equipment: "123", meter serial number: "123", operating center: "nj7", region code: "one", device location: "1234", device category: "00000000091111111", installation: "12345", coordinate: "one", service city: "nj7burg", connection object: "test", service zip: "85023", service utility type: "irrigation"
	And a premise "two" exists with premise number: "123456780", service address house number: "7", service address street: "EaSy St", service address fraction: "1/2", operating center: "nj7", region code: "one", device location: "1234", is major account: "false"
	And a service "two" exists with state: "one", operating center: "nj7", town: "nj7burg", service number: 15, premise: "one", premise number: "5000206788", date installed: "05/15/1994", service category: "two", service material: "two", service size: "two", previous service material: "two", previous service size: "two", date retired: "08/10/1994"
	And a service "three" exists with state: "one", operating center: "nj7", town: "nj7burg", service number: 16, premise: "one", premise number: "3254769801", date installed: "05/04/1994", service category: "three", service material: "three", service size: "three", previous service material: "one", previous service size: "one", date retired: "08/11/1994"
	And a service "four" exists with service number: "123", operating center: "nj7", town: "nj7burg", street: "one", cross street: "one", town section: "one", premise number: "5000206788", installation: "12345", service category: "one", service material: "one", service size: "one", previous service material: "three", previous service size: "three", premise: "one"
	And a work order "one" exists with operating center: "nj7", work description: "hydrant flushing", date completed: "07/12/2021", premise number: "5000206788", date received: "10/20/2020", work order purpose: "bpu", work order priority: "routine", work order requester: "customer", account charged: "123456789", service: "two"
    And a work order "two" exists with operating center: "nj7", work description: "valve replacement", premise number: "5000206788", date completed: "03/22/2021", date received: "11/12/2020", work order purpose: "seasonal", work order priority: "emergency", work order requester: "employee", account charged: "987654321", service: "two"
	And a work order "three" exists with operating center: "nj7", town: "nj7burg", service: "three", premise number: ""
	And a service material "four" exists with description: "copper", customer e p a code: "not lead", company e p a code: "not lead"
	And a service material "five" exists with description: "lead", customer e p a code: "lead", company e p a code: "lead"
	And a service material "six" exists with description: "plastic", customer e p a code: "not lead", company e p a code: "not lead"
	And a meter direction "one" exists with description: "Front"
	And a meter direction "two" exists with description: "Left Side"
	And a meter direction "three" exists with description: "Rear"
	And a small meter location "one" exists with description: "Front1"
	And a small meter location "two" exists with description: "Left Side1"
	And a small meter location "three" exists with description: "Rear1"
	And a short cycle customer material "one" exists with short cycle work order number: 123456, premise: "one", assignment start: "6/29/2018 11:11:11", CustomerSideMaterial: "four", ReadingDeviceDirectionalLocation: "one", ReadingDevicePositionalLocation: "one", TechnicalInspectedOn: "12/10/2019" 
	And a short cycle customer material "two" exists with short cycle work order number: 456789, premise: "one", assignment start: "12/29/2018 11:11:11", CustomerSideMaterial: "five", ServiceLineSize: "3", ReadingDeviceDirectionalLocation: "two", ReadingDevicePositionalLocation: "two", TechnicalInspectedOn: "12/10/2020" 
	And a short cycle customer material "three" exists with short cycle work order number: 789123, premise: "one", assignment start: "6/29/2019 11:11:11", CustomerSideMaterial: "six", ReadingDeviceDirectionalLocation: "three", ReadingDevicePositionalLocation: "three", TechnicalInspectedOn: "12/10/2021"
	And a service "five" exists with service number: "123", operating center: "nj7", town: "nj7burg", street: "one", cross street: "one", town section: "one", premise number: "5000206788", installation: "12345", service category: "two", service material: "one", service size: "one", previous service material: "three", previous service size: "three", premise: "one", date installed: "05/04/2021", last updated: "08/24/2021", meter setting size: "three", customer side material: "two", customer side size: "two"
	And a service "six" exists with state: "one", operating center: "nj7", town: "nj7burg", service number: 15, premise: "two", premise number: "5000206788", date installed: "05/15/1994", service category: "two", service material: "two", service size: "two", previous service material: "two", previous service size: "two", date retired: "08/10/1994", last updated: "08/24/2021", meter setting size: "three", customer side material: "three", customer side size: "three"	
	
Scenario: User can search for a premise
	Given I am logged in as "user"
	When I visit the Customer/Premise/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I press Search
	Then I should see a link to the Show page for premise "one"
	When I follow the Show link for premise "one"
	Then I should be at the Show page for premise "one"
	When I visit the Customer/Premise/Search page
	And I select "Yes" from the HasMeter dropdown
	And I press Search
	Then I should see a link to the show page for premise "one"
	And I should not see a link to the show page for premise "two"
	When I visit the Customer/Premise/Search page
	And I select "No" from the HasMeter dropdown
	And I press Search
	Then I should see a link to the show page for premise "two"
	And I should not see a link to the show page for premise "one"
	When I visit the Customer/Premise/Search page
	And I enter "Garbage" into the ServiceAddressApartment_Value field
	And I press Search
	Then I should see a link to the show page for premise "one"
	And I should not see a link to the show page for premise "two"
	When I visit the Customer/Premise/Search page
	And I select "No" from the IsMajorAccount dropdown
	And I press Search
	Then I should see a link to the show page for premise "two"
	And I should not see a link to the show page for premise "one"

Scenario: User can see service utility types in the premise lookup
	Given I am logged in as "user"
	And a service utility type "one" exists with description: "argh", type: "argh"
	And I am at the Customer/Premise/Find?partialView=false page
	Then I should see "argh" in the ServiceUtilityType dropdown

Scenario: User can select an operating center without choosing a state
	Given I am logged in as "user"	
	And I am at the Customer/Premise/Search page
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	When I select state "two" from the State dropdown
	Then I should not see operating center "nj7" in the OperatingCenter dropdown
	When I select state "one" from the State dropdown
	Then I should see operating center "nj7" in the OperatingCenter dropdown

Scenario: user can view a premise and see some of the values
	Given I am logged in as "user"
	When I visit the Show page for premise: "one"
	Then I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
	And I should see a display for PremiseNumber with "5000206788"
	And I should see a display for ServiceAddressApartment with "garbage"
	And I should see a display for ServiceAddressFraction with "1/2"
	And I should see a display for ServiceAddressHouseNumber with "7"
	And I should see a display for ServiceAddressStreet with "EaSy St"
	And I should see a display for ServiceZip with "85023"
	And I should see a display for Coordinate with "40.32246702, -74.1481018"
	And I should see a display for ConnectionObject with "test"
	And I should see a display for DeviceCategory with "91111111"
	And I should see a display for DeviceLocation with "1234"
	And I should see a display for Installation with "12345"

Scenario: User can click on the service id on the Current Material Size tab to view to service used
	Given I am logged in as "user"
	When I visit the Show page for premise: "two"
	And I click the "Current Material/Size" tab
	Then I should see a link to the show page for service: "six"
	When I follow "6"
	Then I should be at the Show page for service "six"

Scenario: User can search for a premise and see work orders
	Given I am logged in as "user"
	When I visit the Customer/Premise/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I press Search
	Then I should see a link to the Show page for premise "one"
	When I follow the Show link for premise "one"
	Then I should be at the Show page for premise "one"
	And I should see a display for PremiseNumber with "5000206788"
	When I click the "Work Orders" tab
	Then I should see the following values in the workOrders table
	| Work Order General | Work Order Finalization | Description of Job | Date Received | Date Completed | Priority  | Purpose  | Requested By | Accounting |
	| 2					 | 2					   | VALVE REPLACEMENT  | *11/12/2020*  | 3/22/2021      | Emergency | Seasonal | Employee     | 987654321  |
	| 1					 | 1					   | HYDRANT FLUSHING   | *10/20/2020*  | 7/12/2021      | Routine   | BPU      | Customer     | 123456789  |
	And I should see "Records found: 2"

Scenario: User can search for a premise and see service history
	Given I am logged in as "user"
	When I visit the Customer/Premise/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I press Search
	Then I should see a link to the Show page for premise "one"
	When I follow the Show link for premise "one"
	Then I should be at the Show page for premise "one"
	And I should see a display for PremiseNumber with "5000206788"
	When I click the "Service History" tab
	Then I should see the following values in the serviceHistoryTable table
	| Service Number | Category of Service       | Installed Date | Retired Date | Service Company Material | Service Company Size | Previous Service Company Material | Previous Service Company Size | Work Order |
	| 123            | Fire Service Installation | 5/4/2021       |              | I am a material          | Size 1               | I am a material3                  | Size 3                        |            |
	| 15             | Fire Service Installation | 5/15/1994      | 8/10/1994    | I am a material2         | Size 2               | I am a material2                  | Size 2                        | 2          |
	| 16             | Fire Service Renewal      | 5/4/1994       | 8/11/1994    | I am a material3         | Size 3               | I am a material                   | Size 1                        | 3          |
	| 123            | Fire Retire Service Only  |                |              | I am a material          | Size 1               | I am a material3                  | Size 3                        |            |
	And I should see a link to the FieldOperations/Service/Show page for service "two"																		  
	And I should see a link to the FieldOperations/Service/Show page for service "three"
	And I should see a link to the FieldOperations/Service/Show page for service "four"

Scenario: User can search for a premise and see tap images and related tap images
	Given I am logged in as "user"
	When I visit the Customer/Premise/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I press Search
	Then I should see a link to the Show page for premise "one"
	When I follow the Show link for premise "one"
	Then I should be at the Show page for premise "one"
	And I should see a display for PremiseNumber with "5000206788"
	When I click the "Tap Images" tab
	And I follow "View"
	Then I should be at the FieldOperations/TapImage/Show page for tap image "one"
	When I visit the Customer/Premise/Show page for premise "one"
	And I click the "Related Tap Images" tab
	Then I should see "Records found: 2"
	And I should see a link to the FieldOperations/TapImage/Show page for tap image "two"
	And I should see a link to the FieldOperations/TapImage/Show page for tap image "three"
	And I should see a link to the FieldOperations/TapImage/Search page

Scenario: User can search for a premise and verify tap images data and related tap images data
	Given I am logged in as "user"
	When I visit the Customer/Premise/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I press Search
	Then I should see a link to the Show page for premise "one"
	When I follow the Show link for premise "one"
	Then I should be at the Show page for premise "one"
	And I should see a display for PremiseNumber with "5000206788"
	When I click the "Tap Images" tab
	Then I should see the following values in the tapImages table
	| Operating Center | Service Number | Premise Number | Date Completed | Created At  | Length Of Service | Service Company Material | Service Company Size | Previous Service Company Material | Previous Service Company Size | Customer Side Material | Customer Side Size | Service Type | Is Default Image for Tap | Office Review Required |
	| NJ7 - Shrewsbury | 123456         | 5000206788     | 6/30/2021      | today       | 2                 | I am a material          | Size 1               | I am a material                   | Size 1                        | I am a material        | Size 1             | servicetype1 | Yes                      | Yes                    |
	When I visit the Customer/Premise/Show page for premise "one"
	And I click the "Related Tap Images" tab
	Then I should see the following values in the tapImages table
	| Operating Center | Service Number | Premise Number | Date Completed | Created At  | Length Of Service | Service Company Material | Service Company Size | Previous Service Company Material | Previous Service Company Size | Customer Side Material | Customer Side Size | Service Type | Is Default Image for Tap | Office Review Required |
	| NJ7 - Shrewsbury | 234567         | 5000206788     | 6/30/2021      | today       | 2                 | I am a material2         | Size 2               | I am a material2                  | Size 2                        | I am a material2        | Size 2             | servicetype2 | Yes                      | No                     |
	| NJ7 - Shrewsbury | 345678         | 5000206788     | 6/30/2021      | today       | 2                 | I am a material3         | Size 3               | I am a material3                  | Size 3                        | I am a material3        | Size 3             | servicetype3 | No                       | No                     |
	And I should see "Records found: 2"
	And I should see a link to the FieldOperations/TapImage/Show page for tap image "two"
	And I should see a link to the FieldOperations/TapImage/Show page for tap image "three"
	And I should see a link to the FieldOperations/TapImage/Search page

Scenario: User can search for a premise and see W1V Customer History
    Given I am logged in as "user"
	When I visit the Customer/Premise/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I press Search
	Then I should see a link to the Show page for premise "one"
	When I follow the Show link for premise "one"
	Then I should be at the Show page for premise "one"
	When I click the "W1V Customer History" tab
	Then I should see the following values in the shortCycleCustomerMaterialsTable table
	| Work Order Number | Customer Side Material       | Location       | Assignment Date             |
	| 789123            | plastic                      |  Rear1         | 6/29/2019 11:11:11 AM       |
	| 456789            | lead                         |  Left Side1    | 12/29/2018 11:11:11 AM      |
	| 123456            | copper                       |  Front1        | 6/29/2018 11:11:11 AM       |	

Scenario: User can search for a premise and see current material/size
    Given I am logged in as "user"    
	And a consolidated customer side material exists with consolidated e p a code: "lead", customer side e p a code: "lead", customer side external e p a code: "lead"
	And a consolidated customer side material exists with consolidated e p a code: "lead status unknown", customer side e p a code: "lead status unknown", customer side external e p a code: ""
	And a consolidated customer side material exists with consolidated e p a code: "not lead", customer side e p a code: "not lead", customer side external e p a code: "not lead"
	When I visit the Customer/Premise/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I press Search
	Then I should see a link to the Show page for premise "one"
	When I follow the Show link for premise "one"
	Then I should be at the Show page for premise "one"
	When I click the "Current Material/Size" tab
	Then I should see a display for MostRecentService_MeterSettingSize with "Size 3"
	And I should see a display for MostRecentService_ServiceMaterial with "I am a material"
	And I should see a display for MostRecentService_ServiceSize with "Size 1"
	And I should see a display for MostRecentServiceCompanyMaterialEPACode with "LEAD"
	And I should see a display for MostRecentService_CustomerSideMaterial with "I am a material2"
	And I should see a display for MostRecentService_CustomerSideSize with "Size 2"
	And I should see a display for MostRecentServiceCustomerMaterialEPACode with "NOT LEAD"
	And I should see a display for MostRecentCustomerMaterial_AssignmentStart with "6/29/2019 11:11:11 AM"
	And I should see a display for MostRecentCustomerMaterial_CustomerSideMaterial with "plastic"
	And I should see a display for MostRecentCustomerMaterial_ServiceLineSize with ""
	And I should see a display for MostRecentCustomerMaterial_ReadingDevicePositionalLocation with "Rear1"
	And I should see a display for MostRecentCustomerMaterialEPACode with "NOT LEAD"
	And I should see a display for ConsolidatedCustomerSideMaterial with "NOT LEAD"
	When I visit the Show page for premise: "two"
	And I click the "Current Material/Size" tab
	Then I should see a display for MostRecentService_MeterSettingSize with "Size 3"
	And I should see a display for MostRecentService_ServiceMaterial with "I am a material2"
	And I should see a display for MostRecentService_ServiceSize with "Size 2"
	And I should see a display for MostRecentService_CustomerSideMaterial with "I am a material3"
	And I should see a display for MostRecentService_CustomerSideSize with "Size 3"
	And I should see a display for MostRecentServiceCustomerMaterialEPACode with "LEAD STATUS UNKNOWN"
	And I should see a display for MostRecentCustomerMaterial_AssignmentStart with ""
	And I should see a display for MostRecentCustomerMaterial_CustomerSideMaterial with ""
	And I should see a display for MostRecentCustomerMaterial_ServiceLineSize with ""
	And I should see a display for MostRecentCustomerMaterial_ReadingDevicePositionalLocation with ""
	And I should see a display for MostRecentCustomerMaterialEPACode with ""
	And I should see a display for ConsolidatedCustomerSideMaterial with "LEAD STATUS UNKNOWN"

Scenario: User can search for a premise and create work order
    Given I am logged in as "user"
	When I visit the Customer/Premise/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	When I press Search
	Then I should see a link to the Show page for premise "one"
	When I follow the Show link for premise "one"
	Then I should be at the Show page for premise "one"
	When I follow "Create T&D Work Order"
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	And I should see coordinate "one"'s Id in the CoordinateId field
	And I should see "garbage" in the ApartmentAddtl field
	And I should see "85023" in the ZipCode field
	And asset type "service"'s ToString should be selected in the AssetType dropdown
	And I should see "5000206788" in the PremiseNumber field
	And I should see "1234" in the DeviceLocation field
	And I should see "12345" in the Installation field
	And I should see "123" in the SAPEquipmentNumber field
	And I should see "123" in the MeterSerialNumber field
	And town "nj7burg"'s ToString should be selected in the Town dropdown

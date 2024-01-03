Feature: ServicesPage

Background: data exists
    Given a w b s number "one" exists with description: "B18-02-0001"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", w b s number: "one"
    And a contractor "one" exists with name: "one", operating center: "nj7"
    And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
    And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
    And a customer side s l replacement offer status "one" exists with description: "N/A"
    And a customer side s l replacement offer status "two" exists with description: "Offered-Accepted"
    And a customer side s l replacer "one" exists with description: "sl replacer"
    And a contractor "nj7" exists with framework operating center: "nj7", operating center: "nj7"	
    And a town "one" exists with name: "Loch Arbour"
    And a town "two" exists with name: "Allenhurst"
    And a state "one" exists with Abbreviation: "NJ"
    And operating center: "nj7" exists in town: "one" 
    And operating center: "nj7" exists in town: "two" 
    And a town section "one" exists with name: "A section", town: "one"
    And a town section "two" exists with name: "A section", town: "two"
    And a town section "inactive" exists with town: "one", active: false
    And a street "one" exists with town: "one"
    And a street "two" exists with town: "two"
    And a street "three" exists with town: "one"
    And a service category "one" exists with description: "Fire Retire Service Only"
    And a service category "two" exists with description: "Fire Service Installation"
    And a service category "three" exists with description: "Fire Service Renewal"
    And a service category "four" exists with description: "Install Meter Set"
    And a service category "five" exists with description: "Sewer Service New"
    And a service type "one" exists with operating center: "nj7", service category: "one", description: "Water NJ7"
    And a coordinate "one" exists
    And a premise "one" exists with service address house number: "7", service address street: "EaSy St", service address fraction: "1/2", operating center: "nj7", device location: "1234", is major account: "false", premise number: "7328675309"
    And a service "one" exists with state: "one", operating center: "nj7", town: "one", service category: "one", service number: 1, contact date: "2018-05-31"
    And a service "two" exists with state: "one", operating center: "nj7", town: "two", service category: "one", service number: 2
    And a service "three" exists with state: "one", premise: "one", operating center: "nj7", town: "one", town section: "one", service category: "one", service number: 13, premise number: "7328675309", street number: "123", street: "one", cross street: "three", zip: "12345", block: "106", lot: "10", coordinate: "one"
    And a service "four" exists with state: "one", premise: "one", operating center: "nj7", town: "one", town section: "one", service category: "five", service number: 12, premise number: "7328675309", street number: "123", street: "one", cross street: "three", zip: "12345", block: "106", lot: "10", coordinate: "one"
    And a premise type "one" exists with description: "premise type 1", abbreviation: "pt1"
    And a service size "one" exists with size: .5, service size description: "1/2", main: true, service: true, sort order: 1
    And a service size "two" exists with size: .25, service size description: "1/4", main: true, service: true, sort order: 1
    And a service size "three" exists with size: .126, service size description: "1/8", main: true, service: true, meter: true, sort order: 1
    And a service size "four" exists with size: .0625, service size description: "1/16", main: true, service: true, sort order: 1
    And a service material "one" exists with description: "copper"
    And operating center: "nj7" exists in service material: "one"
    And a main type "one" exists with description: "asbestos"
    And a service installation purpose "one" exists with description: "basdfas"
    And a backflow device "one" exists with description: "other"
    And a permit type "one" exists with description: "State Permit"
    And a service status "one" exists with description: "active"
    And a street material "one" exists with description: "concrete"
    And a service material "two" exists with description: "carbon fiber"
    And operating center: "nj7" exists in service material: "two"
    And a service priority "one" exists with description: "important"
    And a service restoration contractor "one" exists with contractor: "Foo", operating center: "nj7", final restoration: true, partial restoration: true
    And an asset type "service" exists with description: "service"
    And an asset type "sewer lateral" exists with description: "sewer lateral"
    And operating center "nj7" has asset type "service"
    And operating center "nj7" has asset type "sewer lateral"
    And a data type "data type" exists with table name: "Services"
    And a document type "document type" exists with data type: "data type", name: "Service Document"
    And a work description "one" exists with description: "service line installation"
    And a finalization work order "one" exists with service: "one", contractor: "one", work description: "one"
    And a work order "two" exists with service: "two"
    And a flushing of customer plumbing instructions "one" exists with description: "instructions"

Scenario: user can search for services
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the Service/Search page	
    Then I should see "Service Number"
    And I should see "Premise Number"
    And I should see "Operating Center"
    And I should see "Town"
    And I should see "Town Section"
    And I should see "Development"
    And I should see "Street Number"
    And I should see "Apartment Number"
    And I should see "Street"
    And I should see "Cross Street"
    And I should see "Block"
    And I should see "Lot"
    And I should see "Customer Name"
    And I should see "Phone Number"
    And I should see "Category of Service"
    And I should see "Service Company Material"
    And I should see "Service Company Size"

Scenario: user can view a service
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the Show page for service: "one"
    Then I should see a display for ContactDate with "5/31/2018"
    And I should see a display for "Name" with service: "one"'s Name
    And I should see a display for "StreetNumber" with service: "one"'s StreetNumber
    And I should see a display for "Street" with service: "one"'s Street
    And I should see a display for "Town" with service: "one"'s Town
    And I should see a display for "TownSection" with service: "one"'s TownSection
    And I should see a display for "Block" with service: "one"'s Block
    And I should see a display for "Lot" with service: "one"'s Lot
    And I should see a display for "Coordinate" with service: "one"'s Coordinate
    And I should see a display for "PhoneNumber" with service: "one"'s PhoneNumber
    And I should see a display for "ApartmentNumber" with service: "one"'s ApartmentNumber
    And I should see a display for "CrossStreet" with service: "one"'s CrossStreet
    And I should see a display for "State" with service: "one"'s State
    And I should see a display for "Zip" with service: "one"'s Zip
    And I should see a display for "Development" with service: "one"'s Development
    And I should see a display for "ObjectId" with service: "one"'s ObjectId
    And I should see a display for CleanedCoordinates with "No"
    And I should see a display for "ServiceCategory" with service: "one"'s ServiceCategory
    And I should see a display for "ServiceInstallationPurpose" with service: "one"'s ServiceInstallationPurpose
    And I should see a display for "TaskNumber1" with service: "one"'s TaskNumber1
    And I should see a display for "SAPNotificationNumber" with service: "one"'s SAPNotificationNumber
    And I should see a display for "SAPWorkOrderNumber" with service: "one"'s SAPWorkOrderNumber
    And I should see a display for DeveloperServicesDriven with ""
    And I should see a display for Agreement with ""
    And I should see a display for "MainType" with service: "one"'s MainType
    And I should see a display for "MainSize" with service: "one"'s MainSize
    And I should see a display for BureauOfSafeDrinkingWaterPermitRequired with ""
    And I should see a display for "ParentTaskNumber" with service: "one"'s ParentTaskNumber
    And I should see a display for "TaskNumber2" with service: "one"'s TaskNumber2
    And I should see a display for MeterSettingRequirement with ""
    And I should see a display for "MeterSettingSize" with service: "one"'s MeterSettingSize
    And I should see a display for "ServiceMaterial" with service: "one"'s ServiceMaterial
    And I should see a display for "ServiceSize" with service: "one"'s ServiceSize
    And I should see a display for "ServiceSideType" with service: "one"'s ServiceSideType
    And I should see a display for PitInstalled with ""
    And I should see a display for "CustomerSideSLReplacement" with service: "one"'s CustomerSideSLReplacement
    And I should see a display for "FlushingOfCustomerPlumbing" with service: "one"'s FlushingOfCustomerPlumbing
    And I should see a display for "CustomerSideSLReplacedBy" with service: "one"'s CustomerSideSLReplacedBy
    And I should see a display for "CustomerSideSLReplacementContractor" with service: "one"'s CustomerSideSLReplacementContractor
    And I should see a display for "LengthOfCustomerSideSLReplaced" with service: "one"'s LengthOfCustomerSideSLReplaced
    And I should see a display for "CustomerSideSLReplacementCost" with service: "one"'s CustomerSideSLReplacementCost
    And I should see a display for "CustomerSideReplacementDate" with service: "one"'s CustomerSideReplacementDate
    And I should see a display for "CustomerSideSLWarrantyExpiration" with service: "one"'s CustomerSideSLWarrantyExpiration
    And I should see a display for "CustomerSideReplacementWBSNumber" with service: "one"'s CustomerSideReplacementWBSNumber
    When I click the "Work Orders" tab
    Then I should see the following values in the workOrdersTable table
	| WORK ORDER GENERAL | Description of Job        |
	| 1                  | SERVICE LINE INSTALLATION |
    And I should see a link to the edit page for finalization work order "one"

Scenario: user can add a tap image to a service
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the Show page for service: "three"
    Then I should not see a link to the edit page for service: "three"
    When I click the "Tap Images" tab
    And I follow "New Tap Image"
    And I wait for ajax to finish loading
    Then I should be at the TapImage/New page
    And service "three"'s OperatingCenter should be selected in the OperatingCenter dropdown
    And service "three"'s Town should be selected in the Town dropdown
    And I should see service "three"'s StreetNumber in the StreetNumber field
    And service "three"'s Street should be selected in the StreetIdentifyingInteger dropdown
    And service "three"'s CrossStreet should be selected in the CrossStreetIdentifyingInteger dropdown
    And I should see service "three"'s PremiseNumber in the PremiseNumber field
    And I should see service "three"'s ServiceNumber in the ServiceNumber field
    And service "three" should be selected in the Service dropdown
    And I should see service "three"'s Block in the Block field
    And I should see service "three"'s Lot in the Lot field
    And I should see service "three"'s DateInstalled in the DateCompleted field
    And I should see service "three"'s LengthOfService in the LengthOfService field
    When I press Save
    Then I should see the validation message "The FileUpload field is required."
    When I upload "itsatiff.tiff"
    And I press Save
    Then I should be at the Show page for service: "three"
    When I click the "Tap Images" tab
    And I follow "View"
	Then the currently shown tap image shall henceforth be known throughout the land as "zoink"	
	And I should delete tap image "zoink"'s file to clean up after this test

Scenario: user can add a document to a service
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the Show page for service: "one"
    When I click the "Documents" tab
	And I press "New Document"
    And I select "Service Document" from the #pnlNewDocument #DocumentType dropdown
	And I upload "..\..\mmsinc\MapCall.CommonTest\TestFile.bin"
	And I wait 2 second
	Then I should see "TestFile.bin" in the FileName field
	When I enter "asdf" into the FileName field
    And I press Save
    And I click the "Documents" tab
    Then I should see "asdf" in the table documentsTable's "File Name" column
    And I should see "Service Document" in the table documentsTable's "Document Type" column

Scenario: user can add a note to a service
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the Show page for service: "one"
    When I click the "Notes" tab
	And I press "New Note"
    And I enter "this is not a note" into the Text field
    And I press "Add Note"
    And I click the "Notes" tab
    Then I should see "this is not a note" in the table notesTable's "Note" column

Scenario: user can install a service
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the Show page for service: "one"
    When I follow "Edit"
    Then I should be at the edit page for service: "one"
    When I select "Yes" from the MeterSettingRequirement dropdown
    And I enter "8/17/2020" into the DateInstalled field
    And I press "Save"
    Then I should see the validation message "The Size of Main field is required."
    And I should see the validation message "The CustomerSideMaterial field is required."
    And I should see the validation message "The CustomerSideSize field is required."
    And I should see the validation message "The MeterSettingSize field is required."
    And I should see the validation message "The Service Company Material field is required."
    And I should see the validation message "The Service Company Size field is required."
    And I should see the validation message "The DateIssuedToField field is required."
    And I should see the validation message "The WorkIssuedTo field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The LengthOfService field is required."
    When I select service size "one" from the MainSize dropdown
    And I select service material "one" from the CustomerSideMaterial dropdown
    And I select service size "one" from the CustomerSideSize dropdown
    And I select service size "one" from the MeterSettingSize dropdown
    And I select service material "one" from the ServiceMaterial dropdown
    And I select service size "one" from the ServiceSize dropdown
    And I enter "8/17/2020" into the DateIssuedToField field
    And I select service restoration contractor "one" from the WorkIssuedTo dropdown
    And I select service priority "one" from the ServicePriority dropdown
    And I enter "123.45" into the LengthOfService field
    And I enter "5" into the DepthMainFeet field
    And I enter "8" into the DepthMainInches field
    And I press "Save"
    Then I should see a display for MainSize with service size "one"
    And I should see a display for CustomerSideMaterial with service material "one"
    And I should see a display for CustomerSideSize with service size "one"
    And I should see a display for MeterSettingSize with service size "one"
    And I should see a display for ServiceMaterial with service material "one"
    And I should see a display for ServiceSize with service size "one"
    And I should see a display for DateIssuedToField with "8/17/2020"
    And I should see a display for WorkIssuedTo with service restoration contractor "one"
    And I should see a display for ServicePriority with service priority "one"
    And I should see a display for LengthOfService with "123.45"
    And I should see a display for DepthMain with escaped text "5' 8\""

Scenario: user can edit and retire a service
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the Show page for service: "one"
    When I follow "Edit"
    Then I should be at the edit page for service: "one"
    When I enter "8/17/2020" into the RetiredDate field
    And I press "Save"
    Then I should see the validation message "The DateIssuedToField field is required."
    And I should see the validation message "The WorkIssuedTo field is required."
    And I should see the validation message "The Priority field is required."
    And I should see the validation message "The Installed Date field is required."
    And I should see the validation message "The Previous Service Company Material field is required."
    And I should see the validation message "The OriginalInstallationDate field is required."
    And I should see the validation message "The Previous Service Company Size field is required."
    When I enter "8/18/2020" into the DateIssuedToField field
    And I enter "8/19/2020" into the DateInstalled field
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "one" from the MainSize dropdown
    And I select service material "one" from the CustomerSideMaterial dropdown
    And I select service size "one" from the CustomerSideSize dropdown
    And I select service size "one" from the MeterSettingSize dropdown
    And I select service material "one" from the ServiceMaterial dropdown
    And I select service size "one" from the ServiceSize dropdown
    And I select "Yes" from the CompanyOwned dropdown
    And I select service restoration contractor "one" from the WorkIssuedTo dropdown
    And I select service priority "one" from the ServicePriority dropdown
    And I enter "123.45" into the LengthOfService field
    And I select service material "one" from the PreviousServiceMaterial dropdown
    And I enter "8/1/2020" into the OriginalInstallationDate field
    And I select service size "one" from the PreviousServiceSize dropdown
    And I select customer side s l replacement offer status "one" from the CustomerSideSLReplacement dropdown
    And I select flushing of customer plumbing instructions "one" from the FlushingOfCustomerPlumbing dropdown
    And I select customer side s l replacer "one" from the CustomerSideSLReplacedBy dropdown
    And I enter "8" into the LengthOfCustomerSideSLReplaced field
    And I enter "8/21/2020 10:00:00 AM" into the CustomerSideReplacementDate field
    And I select contractor "nj7" from the CustomerSideSLReplacementContractor dropdown
    #first one here gets the focus off DateIssuedToField's date picker popup
    And I press "Save"
    #And I press "Save"
    And I wait for the page to reload
    Then I should see a display for DateIssuedToField with "8/18/2020" 
    And I should see a display for DateInstalled with "8/19/2020" 
    And I should see a display for MainSize with service size "one"
    And I should see a display for CustomerSideMaterial with service material "one"
    And I should see a display for CustomerSideSize with service size "one"
    And I should see a display for MeterSettingSize with service size "one"
    And I should see a display for ServiceMaterial with service material "one"
    And I should see a display for ServiceSize with service size "one"
    And I should see a display for CompanyOwned with "Yes"
    And I should see a display for WorkIssuedTo with service restoration contractor "one"
    And I should see a display for ServicePriority with service priority "one"
    And I should see a display for LengthOfService with "123.45"
    And I should see a display for PreviousServiceMaterial with service material "one"
    And I should see a display for OriginalInstallationDate with "8/1/2020"
    And I should see a display for PreviousServiceSize with service size "one"
    And I should see a display for CustomerSideSLReplacement with customer side s l replacement offer status "one"
    And I should see a display for FlushingOfCustomerPlumbing with flushing of customer plumbing instructions "one"
    And I should see a display for CustomerSideSLReplacedBy with customer side s l replacer "one" 
    And I should see a display for LengthOfCustomerSideSLReplaced with "8"
    And I should see a display for CustomerSideReplacementDate with "8/21/2020 10:00:00 AM"
    And I should see a display for CustomerSideSLReplacementContractor with contractor "nj7"

Scenario: user can view a service but cannot edit
    Given I am logged in as "user@site.com", password: "testpassword#1"
    And I am at the Show page for service: "two"
    Then I should not see a link to the edit page for service: "two"

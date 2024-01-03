Feature: ServicePage

    Background: data exists
        Given a w b s number "one" exists with description: "B18-02-0001"
        And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", w b s number: "one"
        And an admin user "admin" exists with username: "admin", default operating center: "nj7"
        And a user "testuser" exists with username: "stuff", default operating center: "nj7", full name: "Smith"
        And a customer side s l replacement offer status "one" exists with description: "N/A"
        And a customer side s l replacement offer status "two" exists with description: "Offered-Accepted"
        And a contractor "nj7" exists with framework operating center: "nj7", operating center: "nj7", name: "Kevin"
        And a contractor user "user" exists with email: "user@site.com", contractor: "nj7"
        And a town "one" exists with name: "Loch Arbour"
        And a town "two" exists with name: "Allenhurst"
        And a state "one" exists with abbreviation: "NJ"
        And a state "two" exists with name: "Pennsylvania", abbreviation: "PA"
        And operating center: "nj7" exists in town: "one"
        And operating center: "nj7" exists in town: "two"
        And a town section "one" exists with name: "A section", town: "one"
        And a town section "two" exists with name: "A section", town: "two"
        And a town section "inactive" exists with town: "one", active: false
        And a street "one" exists with town: "one", is active: true
        And a street "two" exists with town: "two", is active: true
        And a street "three" exists with town: "one", is active: true
        And a premise unavailable reason "one" exists with description: "Killed Premise"
        And a inside shutoff service termination point "one" exists
        And an other service termination point "other" exists
        And service categories exist
        And a service type "one" exists with operating center: "nj7", service category: "fire retire service only", description: "Water NJ7"
        And a premise "one" exists with service address house number: "7", service address street: "EaSy St", service address fraction: "1/2", operating center: "nj7", device location: "1234", is major account: "false", premise number: "7328675309"
        And a service "one" exists with state: "one", operating center: "nj7", town: "one", service category: "fire retire service only", service number: 1
        And a service "two" exists with state: "one", operating center: "nj7", service category: "fire retire service only", service number: 2, premise number unavailable: true, premise unavailable reason: "one"
        And a service "three" exists with state: "one", operating center: "nj7", town: "one", town section: "one", service category: "fire retire service only", service number: 13, premise: "one", premise number: "7328675309", street number: "123", street: "one", cross street: "three", zip: "12345", block: "106", lot: "10", coordinate: "one"
        And a service "four" exists with state: "one", operating center: "nj7", town: "one", town section: "one", service category: "sewer service new", service number: 12, premise: "one", premise number: "7328675309", street number: "123", street: "one", cross street: "three", zip: "12345", block: "106", lot: "10", coordinate: "one"
        And a service "five" exists with state: "one", operating center: "nj7", town: "one", service category: "fire retire service only", service number: 15, premise: "one", premise number: "7328675309", s a p work order number: "1234567", s a p notification number: "9876543"
        And a coordinate "one" exists
        And a premise type "one" exists with description: "premise type 1"
        And a service size "one" exists with size: .5, service size description: "1/2", main: true, service: true, sort order: 1
        And a service size "two" exists with size: .25, service size description: "1/4", main: true, service: true, sort order: 1
        And a service size "three" exists with size: .126, service size description: "1/8", main: true, service: true, meter: true, sort order: 1
        And a service size "four" exists with size: .0625, service size description: "1/16", main: true, service: true, sort order: 1
        # A Lead service material needs to exist for create/edit
        And a service material "one" exists with description: "Lead"
        And operating center: "nj7" exists in service material: "one"
        And a main type "one" exists with description: "asbestos"
        #And a service installation purpose "one" exists with description: "basdfas"
        And service installation purposes exist
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

    Scenario: Admin can search for a service
        Given I am logged in as "admin"
        When I visit the FieldOperations/Service/Search page
        And I select operating center "nj7" from the OperatingCenter dropdown
        And I press Search
        Then I should see a link to the Show page for service: "one"
        When I follow the Show link for service "one"
        Then I should be at the Show page for service: "one"

    Scenario: Admin can view a service
        Given I am logged in as "admin"
        When I visit the Show page for service: "one"
        Then I should see a display for service: "one"'s Id
        And I should not see the PremiseUnavailableReason field

Scenario: Admin can add a service and see the correct validation
    Given I am logged in as "admin"
    When I visit the FieldOperations/Service/New page
    And I select state "one" from the State dropdown
    And I press Save
    Then I should see a validation message for PremiseNumber with "The Premise Number field is required."
    And I should see a validation message for PremiseNumberUnavailable with "Please confirm that there is no premise number available or enter the premise number below."
    And I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    And I should see a validation message for Block with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for Lot with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for StreetNumber with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for ServiceCategory with "The Category of Service field is required."
    #And I should see a validation message for LengthOfService with "The field LengthOfService is invalid."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Save
    Then I should see a validation message for Town with "The Town field is required."
    When I select town "one" from the Town dropdown
    And I press Save
    Then I should see a validation message for Street with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    When I select street "one" from the Street dropdown
    And I select service category "install meter set" from the ServiceCategory dropdown
    #Then I should not see a validation message for LengthOfService with "The field LengthOfService is invalid."
    When I select service category "fire retire service only" from the ServiceCategory dropdown
    And I enter "0000000000" into the PremiseNumber field
    And I press Save
    And I wait 1 second
    Then I should not see a validation message for PremiseNumberUnavailable with "Please confirm that there is no premise number available or enter the premise number below."
    When I enter "1234567890" into the PremiseNumber field
    And I enter "123456789" into the Installation field
    And I enter "123456789" into the DeviceLocation field
    And I enter "7" into the LengthOfService field
    And I enter "10" into the Block field
    And I enter "101" into the Lot field
    And I enter "124124124124124124124" into the LeadServiceReplacementWbs field
    And I enter "124124124124124124124" into the LeadServiceRetirementWbs field
    And I select service category "fire retire service only" from the ServiceCategory dropdown
    And I check the LeadAndCopperCommunicationProvided field
    And I select contractor "nj7" from the CustomerSideSLReplacementContractor dropdown
    And I select customer side s l replacement offer status "two" from the CustomerSideSLReplacement dropdown
    And I select "Yes" from the CompanyOwned dropdown
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then the currently shown service will now be referred to as "vincent"
    And I should be at the show page for service: "vincent"
    And I should see a display for PremiseNumber with "1234567890"
    And I should see a display for DeviceLocation with "123456789"
    And I should see a display for CustomerSideReplacementWBSNumber with w b s number "one"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for ServiceNumber with service "vincent"'s ServiceNumber
    And I should see a display for Street with street "one"
    And I should see a display for LengthOfService with "7"
    And I should see a display for Block with "10"
    And I should see a display for Lot with "101"
    And I should see a display for LeadServiceReplacementWbs with "124124124124124124124"
    And I should see a display for LeadServiceRetirementWbs with "124124124124124124124"
    And I should see a display for ServiceCategory with service category "fire retire service only"
    And I should see a display for LeadAndCopperCommunicationProvided with "Yes"
    And I should see a display for CustomerSideSLReplacementContractor with contractor "nj7"
    And I should see a display for CustomerSideSLReplacement with customer side s l replacement offer status "two"
    And I should see a display for CompanyOwned with "Yes"
    And I should see a display for MeterSettingRequirement with "Yes"
    And I should see a display for MeterSettingSize with service size "three"

    Scenario: Admin cannot add a service for an inactive town section
        Given I am logged in as "admin"
        When I visit the FieldOperations/Service/New page
        And I select state "one" from the State dropdown
        And I select operating center "nj7" from the OperatingCenter dropdown
        And I wait for ajax to finish loading
        And I select town "one" from the Town dropdown
        And I wait for ajax to finish loading
        Then I should not see town section "inactive" in the TownSection dropdown

    Scenario: user cannot add a service for an operating center they do not have access to
        Given a user "nj4" exists with username: "nj4"
        And an operating center "nj4" exists
        And a role "nj4Add" exists with action: "Add", module: "FieldServicesAssets", user: "nj4", operating center: "nj4"
        And I am logged in as "nj4"
        When I visit the FieldOperations/Service/New page
        And I select state "one" from the State dropdown
        Then I should not see operating center "nj7" in the OperatingCenter dropdown
        And I should see operating center "nj4" in the OperatingCenter dropdown

Scenario: sizes and materials are defaulted to previous matching premise values for installation and operating center
    Given a premise "new" exists with premise number: "0987654321", installation: "1234567890", operating center: "nj7"
    And a service "new" exists with premise number: "0987654321", operating center: "nj7", premise: "new", date installed: "yesterday", service material: "one", service size: "one", customer side material: "two", customer side size: "two"
    And I am logged in as "admin"
    When I visit the FieldOperations/Service/New page
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I enter premise "new"'s Installation into the Installation text box
    And I wait for ajax to finish loading
    Then service material "one" should be selected in the ServiceMaterial dropdown
    And service size "one" should be selected in the ServiceSize dropdown
    And service material "two" should be selected in the CustomerSideMaterial dropdown
    And service size "two" should be selected in the CustomerSideSize dropdown
    
Scenario: user can add an existing service and see the correct validation
    Given I am logged in as "admin"
    When I visit the FieldOperations/Service/New page
    And I select state "one" from the State dropdown
    And I press Save
    Then I should see a validation message for PremiseNumber with "The Premise Number field is required."
    And I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    And I should see a validation message for Block with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for Lot with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for StreetNumber with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for ServiceCategory with "The Category of Service field is required."
    #And I should see a validation message for LengthOfService with "The field LengthOfService is invalid."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Save
    Then I should see a validation message for Town with "The Town field is required."
    When I select town "one" from the Town dropdown
    And I press Save
    Then I should see a validation message for Street with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    When I select street "one" from the Street dropdown
    And I select service category "install meter set" from the ServiceCategory dropdown
    And I press Save
    #Then I should not see a validation message for LengthOfService with "The field LengthOfService is invalid."
    When I check the IsExistingOrRenewal field
    And I press Save
    Then I should not see a validation message for ServiceNumber with "The field ServiceNumber is invalid."
    When I enter "411" into the ServiceNumber field
    And I select service category "fire retire service only" from the ServiceCategory dropdown
    And I enter "1234567890" into the PremiseNumber field
    And I enter "123456789" into the Installation field
    And I enter "7" into the LengthOfService field
    And I enter "10" into the Block field
    And I enter "101" into the Lot field
    And I select service category "fire retire service only" from the ServiceCategory dropdown
    And I check the DeviceLocationUnavailable field
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then the currently shown service will now be referred to as "vincent"
    And I should be at the show page for service: "vincent"
    And I should see a display for PremiseNumber with "1234567890"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for ServiceNumber with "411"
    And I should see a display for IsActive with "Yes"
    And I should see a display for Street with street "one"
    And I should see a display for LengthOfService with "7"
    And I should see a display for Block with "10"
    And I should see a display for Lot with "101"
    And I should see a display for ServiceCategory with service category "fire retire service only"
    And I should see a display for LeadAndCopperCommunicationProvided with "No"

    Scenario: Admin can do premise lookup when creating a service
        Given the test flag "fake sap technical master account data" exists
        And a service utility type "one" exists with description: "Water Service"
        And I am logged in as "admin"
        When I visit the FieldOperations/Service/New page
        And I follow "Click here to Lookup and Verify Technical Master Data"
        Then I should see "Find SAP Technical Master Account"
        When I press Search
        Then I should see the following values in the installationResults table
            | Installation Type | Device Location | Installation |
            | Water Service     | 67890           | 12345        |
        When I click the 1st row of installationResults
        And I press selectInstallation
        Then I should see "12345" in the Installation field
        And I should see "67890" in the DeviceLocation field

    Scenario: Admin can do premise lookup when editing a service
        Given the test flag "fake sap technical master account data" exists
        And a service utility type "one" exists with description: "Water Service"
        And I am logged in as "admin"
        When I visit the Edit page for service: "one"
        And I follow "Click here to Lookup and Verify Technical Master Data"
        Then I should see "Find SAP Technical Master Account"
        When I press Search
        Then I should see the following values in the installationResults table
            | Installation Type | Device Location | Installation |
            | Water Service     | 67890           | 12345        |
        When I click the 1st row of installationResults
        And I press selectInstallation
        Then I should see "12345" in the Installation field
        And I should see "67890" in the DeviceLocation field

    #@ignore(Alex said to ignore this 11/16/2016)
    #Scenario: Admin can edit a service
    #    Given I am logged in as "admin"
    #    When I visit the Edit page for service: "one"
    #    And I enter "" into the PremiseNumber field
    #    And I enter "" into the ServiceNumber field
    #    And I enter "" into the LengthOfService field
    #    And I select "-- Select --" from the Town dropdown
    #    And I select "-- Select --" from the ServiceCategory dropdown
    #    And I press Save
    #    Then I should see a validation message for PremiseNumber with "The PremiseNumber field is required."
    #    And I should see a validation message for ServiceNumber with "The ServiceNumber field is required."
    #    #And I should see a validation message for LengthOfService with "The field LengthOfService is invalid."
    #    And I should see a validation message for Block with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    #    And I should see a validation message for Lot with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    #    And I should see a validation message for ServiceCategory with "The Category of Service field is required."
    #    And I should see a validation message for Town with "The Town field is required."
    #    When I select town "two" from the Town dropdown
    #    And I press Save
    #    Then I should see a validation message for Street with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    #    And I should see a validation message for StreetNumber with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    #    When I enter "1" into the ServiceNumber field
    #    And I enter "7" into the LengthOfService field
    #    And I enter "1234567890" into the PremiseNumber field
    #    And I select "Yes" from the IsActive dropdown
    #    And I enter today's date into the ContactDate field
    #    And I enter "A Service" into the Name field
    #    And I enter "2" into the StreetNumber field
    #    And I select street "two" from the Street dropdown
    #    And I select town section "two" from the TownSection dropdown
    #    And I enter "3" into the Block field
    #    And I enter "4" into the Lot field
    #    And I enter coordinate "one"'s Id into the Coordinate field
    #    And I enter "5" into the PhoneNumber field
    #    And I enter "6" into the ApartmentNumber field
    #    And I select street "two" from the CrossStreet dropdown
    #    And I select state "one" from the State dropdown
    #    And I enter "7" into the Zip field
    #    And I enter "8" into the Development field
    #    And I enter "9" into the ObjectId field
    #    And I select "No" from the CleanedCoordinates dropdown
    #    And I enter "10" into the NSINumber field
    #    And I select premise type "one" from the PremiseType dropdown
    #    And I enter "11" into the BusinessPartner field
    #    And I enter "12" into the MailPhoneNumber field
    #    And I enter "13" into the MailStreetNumber field
    #    And I enter "14" into the MailTown field
    #    And I enter "15" into the MailZip field
    #    And I enter "16" into the Fax field
    #    And I enter "17" into the MailStreetName field
    #    And I enter "18" into the MailState field
    #    And I enter "19" into the Email field
    #    And I enter "21" into the RoadOpeningFee field
    #    And I enter "22" into the AmountReceived field
    #    And I enter "23" into the ServiceInstallationFee field
    #    And I enter "24" into the PaymentReferenceNumber field
    #    And I select service category "one" from the ServiceCategory dropdown
    #    And I select service installation purpose "one" from the ServiceInstallationPurpose dropdown
    #    And I enter "25" into the TaskNumber1 field
    #    And I select "Yes" from the DeveloperServicesDriven dropdown
    #    And I select "Yes" from the MeterSettingRequirement dropdown
    #    And I select service size "one" from the ServiceSize dropdown
    #    And I select service size "two" from the MainSize dropdown
    #    And I select "Yes" from the BureauOfSafeDrinkingWaterPermitRequired dropdown
    #    And I enter "26" into the ParentTaskNumber field
    #    And I enter "27" into the TaskNumber2 field
    #    And I select "Yes" from the Agreement dropdown
    #    And I select service size "three" from the MeterSettingSize dropdown
    #    And I select service material "one" from the ServiceMaterial dropdown
    #    And I select main type "one" from the MainType dropdown
    #    And I enter "10000000" into the SAPNotificationNumber field
    #    And I enter "9999999999" into the SAPWorkOrderNumber field
    #    And I select service material "one" from the CustomerSideMaterial dropdown
    #    And I select service size "one" from the CustomerSideSize dropdown
    #    #5a
    #    And I enter today's date into the QuestionaireSentDate field
    #    And I enter today's date into the QuestionaireReceivedDate field
    #    And I select backflow device "one" from the BackflowDevice dropdown
    #    #6
    #    And I select permit type "one" from the PermitType dropdown
    #    And I enter today's date into the PermitSentDate field
    #    And I enter today's date into the PermitExpirationDate field
    #    And I enter "28" into the PermitNumber field
    #    And I enter today's date into the PermitReceivedDate field
    #    #7
    #    And I enter today's date into the ApplicationSentOn field
    #    And I enter today's date into the ApplicationReceivedOn field
    #    And I enter today's date into the ApplicationApprovedOn field
    #    #8
    #    And I enter today's date into the InspectionDate field
    #    And I select service status "one" from the ServiceStatus dropdown
    #    #9
    #    And I enter today's date into the DateIssuedToField field
    #    And I select service restoration contractor "one" from the WorkIssuedTo dropdown
    #    And I select service priority "one" from the ServicePriority dropdown
    #    And I enter today's date into the DateInstalled field
    #    And I enter "30" into the LengthOfService field
    #    And I enter "31" into the DepthMainFeet field
    #    And I enter "32" into the DepthMainInches field
    #    #10
    #    And I enter "33" into the RetiredAccountNumber field
    #    And I enter "34" into the RetireMeterSet field
    #    And I select service material "two" from the PreviousServiceMaterial dropdown
    #    And I select street material "one" from the StreetMaterial dropdown
    #    And I enter today's date into the OriginalInstallationDate field
    #    And I enter today's date into the RetiredDate field
    #    And I select service size "four" from the PreviousServiceSize dropdown
    #    #11
    #    And I enter "35" into the InstallationInvoiceNumber field
    #    And I enter "36" into the PurchaseOrderNumber field
    #    And I enter today's date into the InstallationInvoiceDate field
    #    And I enter "37" into the InstallationCost field
    #    And I select contractor "nj7" from the CustomerSideSLReplacementContractor dropdown
    #    And I select customer side s l replacement offer status "two" from the CustomerSideSLReplacement dropdown
    #    And I press Save
    #    Then I should see a display for OperatingCenter with operating center "nj7"
    #    And I should see a display for ServiceNumber with "1"
    #    And I should see a display for PremiseNumber with "1234567890"
    #    And I should see a display for IsActive with "Yes"
    #    And I should see a display for ContactDate with today's date
    #    And I should see a display for Name with "A Service"
    #    And I should see a display for StreetNumber with "2"
    #    And I should see a display for Street with street "two"
    #    And I should see a display for TownSection with town section "two"
    #    And I should see a display for Block with "3"
    #    And I should see a display for Lot with "4"
    #    And I should see a display for Coordinate with "40.32246702, -74.1481018"
    #    And I should see a display for PhoneNumber with "5"
    #    And I should see a display for ApartmentNumber with "6"
    #    And I should see a display for CrossStreet with street "two"
    #    And I should see a display for State with state "one"
    #    And I should see a display for Zip with "7"
    #    And I should see a display for Development with "8"
    #    And I should see a display for ObjectId with "9"
    #    And I should see a display for CleanedCoordinates with "No"
    #    And I should see a display for NSINumber with "10"
    #    And I should see a display for PremiseType with premise type "one"
    #    And I should see a display for BusinessPartner with "11"
    #    And I should see a display for MailPhoneNumber with "12"
    #    And I should see a display for MailStreetNumber with "13"
    #    And I should see a display for MailTown with "14"
    #    And I should see a display for MailZip with "15"
    #    And I should see a display for Fax with "16"
    #    And I should see a display for MailStreetName with "17"
    #    And I should see a display for MailState with "18"
    #    And I should see a display for Email with "19"
    #    And I should see a display for RoadOpeningFee with "$21.00"
    #    And I should see a display for AmountReceived with "$22.00"
    #    And I should see a display for ServiceInstallationFee with "$23.00"
    #    And I should see a display for TotalFee with "$44.00"
    #    And I should see a display for PaymentReferenceNumber with "24"
    #    And I should see a display for ServiceCategory with service category "one"
    #    And I should see a display for ServiceInstallationPurpose with service installation purpose "one"
    #    And I should see a display for TaskNumber1 with "25"
    #    And I should see a display for DeveloperServicesDriven with "Yes"
    #    And I should see a display for MeterSettingRequirement with "Yes"
    #    And I should see a display for ServiceSize with service size "one"
    #    And I should see a display for MainSize with service size "two"
    #    And I should see a display for BureauOfSafeDrinkingWaterPermitRequired with "Yes"
    #    And I should see a display for ParentTaskNumber with "26"
    #    And I should see a display for TaskNumber2 with "27"
    #    And I should see a display for Agreement with "Yes"
    #    And I should see a display for MeterSettingSize with service size "three"
    #    And I should see a display for ServiceMaterial with service material "one"
    #    And I should see a display for MainType with main type "one"
    #    And I should see a display for SAPNotificationNumber with "10000000"
    #    And I should see a display for SAPWorkOrderNumber with "9999999999"
    #    #5a
    #    And I should see a display for QuestionaireSentDate with today's date
    #    And I should see a display for QuestionaireReceivedDate with today's date
    #    And I should see a display for BackflowDevice with backflow device "one"
    #    #6
    #    And I should see a display for PermitType with permit type "one"
    #    And I should see a display for PermitSentDate with today's date
    #    And I should see a display for PermitExpirationDate with today's date
    #    And I should see a display for PermitNumber with "28"
    #    And I should see a display for PermitReceivedDate with today's date
    #    #7
    #    And I should see a display for ApplicationSentOn with today's date
    #    And I should see a display for ApplicationReceivedOn with today's date
    #    And I should see a display for ApplicationApprovedOn with today's date
    #    #8
    #    And I should see a display for InspectionDate with today's date
    #    And I should see a display for ServiceStatus with service status "one"
    #    #9
    #    And I should see a display for DateIssuedToField with today's date
    #    And I should see a display for WorkIssuedTo with service restoration contractor "one"
    #    And I should see a display for ServicePriority with service priority "one"
    #    And I should see a display for DateInstalled with today's date
    #    And I should see a display for LengthOfService with "30"
    #    And I should see a display for DepthMain with escaped text "31' 32\""
    #    #10
    #    And I should see a display for RetiredAccountNumber with "33"
    #    And I should see a display for RetireMeterSet with "34"
    #    And I should see a display for PreviousServiceMaterial with service material "two"
    #    And I should see a display for StreetMaterial with street material "one"
    #    And I should see a display for OriginalInstallationDate with today's date
    #    And I should see a display for RetiredDate with today's date
    #    And I should see a display for PreviousServiceSize with service size "four"
    #    #11
    #    And I should see a display for InstallationInvoiceNumber with "35"
    #    And I should see a display for PurchaseOrderNumber with "36"
    #    And I should see a display for InstallationInvoiceDate with today's date
    #    And I should see a display for InstallationCost with "37"
    #    And I should see a display for CustomerSideSLReplacementContractor with contractor "nj7"
    #    And I should see a display for CustomerSideSLReplacement with customer side s l replacement offer status "two"
    #    And I should see a display for CustomerSideReplacementWBSNumber with w b s number "one"

Scenario: additional validation checks for date retired entered
    Given I am logged in as "admin"
    When I visit the Edit page for service: "one"
    And I enter today's date into the RetiredDate field
    And I select "-- Select --" from the MeterSettingRequirement dropdown
    And I select "-- Select --" from the WorkIssuedTo dropdown
    And I press Save
    Then I should see a validation message for CrossStreet with "The CrossStreet field is required."
    And I should see a validation message for ServiceInstallationPurpose with "The Purpose of Installation field is required."
    And I should see a validation message for DateIssuedToField with "The DateIssuedToField field is required."
    And I should see a validation message for WorkIssuedTo with "The WorkIssuedTo field is required."
    #And I should see a validation message for LengthOfService with "The LengthOfService field is required."
    And I should see a validation message for ServicePriority with "The Priority field is required."
    And I should see a validation message for DateInstalled with "The Installed Date field is required."
    And I should see a validation message for PreviousServiceMaterial with "The Previous Service Company Material field is required."
    And I should see a validation message for Zip with "The Zip field is required."
    And I should see a validation message for MeterSettingRequirement with "The Meter Setting Requirement field is required."
    When I select "Yes" from the MeterSettingRequirement dropdown
    And I press Save
    Then I should not see a validation message for MeterSettingRequirement with "The Meter Setting Requirement field is required."    

Scenario: additional validation checks for date installed entered
    Given I am logged in as "admin"
    When I visit the Edit page for service: "one"
    And I enter today's date into the DateInstalled field
    And I enter "" into the ServiceNumber field
    And I select "-- Select --" from the MeterSettingRequirement dropdown
    And I select "-- Select --" from the ServiceCategory dropdown
    And I select "-- Select --" from the DeveloperServicesDriven dropdown
    And I select "-- Select --" from the ServiceMaterial dropdown
    And I select "-- Select --" from the ServiceSize dropdown
    And I select "-- Select --" from the WorkIssuedTo dropdown
    And I select "-- Select --" from the PreviousServiceSize dropdown
    And I press Save
    Then I should see a validation message for ServiceCategory with "The Category of Service field is required."
    And I should see a validation message for DateIssuedToField with "The DateIssuedToField field is required."
    And I should see a validation message for DeveloperServicesDriven with "The DeveloperServicesDriven field is required."
    And I should see a validation message for LengthOfService with "The LengthOfService field is required."
    And I should see a validation message for MeterSettingRequirement with "The Meter Setting Requirement field is required."
    And I should see a validation message for ServicePriority with "The Priority field is required."
    And I should see a validation message for ServiceInstallationPurpose with "The Purpose of Installation field is required."
    And I should see a validation message for ServiceMaterial with "The Service Company Material field is required."
    And I should see a validation message for MainSize with "The Size of Main field is required."
    And I should see a validation message for ServiceSize with "The Service Company Size field is required."
    And I should see a validation message for MeterSettingSize with "The Meter Setting Size field is required."
    And I should see a validation message for State with "The State field is required."
    And I should see a validation message for Street with "The Street field is required."
    And I should see a validation message for StreetNumber with "The StreetNumber field is required."
    #And I should see a validation message for TaskNumber1 with "The WBS # field is required."
    And I should see a validation message for WorkIssuedTo with "The WorkIssuedTo field is required."
    And I should see a validation message for Zip with "The Zip field is required."
    When I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then I should not see a validation message for MeterSettingRequirement with "The Meter Setting Requirement field is required."
    And I should not see a validation message for MeterSettingSize with "The Meter Setting Size field is required."

Scenario: skip additional validation checks for date installed entered when water measurement only and material verification
    Given I am logged in as "admin"
    When I visit the Edit page for service: "one"
    And I enter today's date into the DateInstalled field
    And I select service category "water measurement only" from the ServiceCategory dropdown
    And I select service installation purpose "material verification" from the ServiceInstallationPurpose dropdown
    And I enter "" into the ServiceNumber field
    And I select "-- Select --" from the MeterSettingRequirement dropdown
    And I select "-- Select --" from the DeveloperServicesDriven dropdown
    And I select "-- Select --" from the ServiceMaterial dropdown
    And I select "-- Select --" from the ServiceSize dropdown
    And I select "-- Select --" from the WorkIssuedTo dropdown
    And I select "-- Select --" from the PreviousServiceSize dropdown
    And I press Save
    Then I should not see a validation message for DateIssuedToField with "The DateIssuedToField field is required."
    And I should not see a validation message for DeveloperServicesDriven with "The DeveloperServicesDriven field is required."
    And I should not see a validation message for LengthOfService with "The LengthOfService field is required."
    And I should not see a validation message for ServicePriority with "The Priority field is required."
    And I should not see a validation message for MainSize with "The Size of Main field is required."
    And I should not see a validation message for MeterSettingSize with "The Meter Setting Size field is required."
    And I should not see a validation message for TaskNumber1 with "The WBS # field is required."
    And I should not see a validation message for WorkIssuedTo with "The WorkIssuedTo field is required."
    And I should not see a validation message for Zip with "The Zip field is required."
    And I should not see a validation message for CustomerSideSize with "The CustomerSideSize field is required."

Scenario: other validation checks
    Given I am logged in as "admin"
    When I visit the Edit page for service: "one"
    And I select service category "fire service renewal" from the ServiceCategory dropdown
    And I enter today's date into the DateInstalled field
    And I select "-- Select --" from the PreviousServiceMaterial dropdown
    And I select "-- Select --" from the PreviousServiceSize dropdown
    And I press Save
    Then I should see a validation message for PreviousServiceMaterial with "The Previous Service Material field is required."
    And I should see a validation message for PreviousServiceSize with "The Previous Service Size field is required."
    And I should see a validation message for OriginalInstallationDate with "The Original Installation Date field is required.    "
    When I select service category "fire retire service only" from the ServiceCategory dropdown
    And I press Save
    Then I should not see a validation message for PreviousServiceMaterial with "The Previous Service Material field is required."
    And I should not see a validation message for PreviousServiceSize with "The Previous Service Size field is required."
    And I should not see a validation message for OriginalInstallationDate with "The Original Installation Date field is required.    "
    When I enter today's date into the RetiredDate field
    And I press Save
    Then I should not see a validation message for PreviousServiceMaterial with "The Previous Service Material field is required."
    And I should not see a validation message for PreviousServiceSize with "The Previous Service Size field is required."
    And I should not see a validation message for OriginalInstallationDate with "The Original Installation Date field is required.    "

Scenario: Admin can create a renewal from an existing service
    Given I am logged in as "admin"
    When I visit the Show page for service: "one"
    And I click the "Renewals" tab
    And I follow "Add a Renewal"
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I select street "one" from the Street dropdown
    And I select service category "fire retire service only" from the ServiceCategory dropdown
    And I enter "1234567890" into the PremiseNumber field
    And I enter "123456789" into the Installation field
    And I enter "7" into the LengthOfService field
    And I enter "10" into the Block field
    And I enter "101" into the Lot field    
    And I check the DeviceLocationUnavailable field
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then the currently shown service will now be referred to as "eugene"
    And I should be at the show page for service: "eugene"
    And I should see a display for PremiseNumber with "1234567890"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for DeviceLocationUnavailable with "Yes"
    # The service number from the original record should be copied to the new service record
    And I should see a display for ServiceNumber with service "one"'s ServiceNumber
    And I should see a display for IsActive with "Yes"
    And I should see a display for Street with street "one"
    And I should see a display for LengthOfService with "7"
    And I should see a display for Block with "10"
    And I should see a display for Lot with "101"
    And I should see a display for ServiceCategory with service category "fire retire service only"

Scenario: Admin can copy a service and service number isnt copied
    Given I am logged in as "admin"
    When I visit the Show page for service: "three"
    And I follow "Copy"
    And I press Save
    Then I should see a validation message for PremiseNumber with "The Premise Number field is required."
    When I enter "0000000042" into the PremiseNumber field
    And I enter "123456789" into the Installation field
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I check the DeviceLocationUnavailable field
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then the currently shown service will now be referred to as "kristen"
    And I should see a display for ServiceNumber with service "kristen"'s ServiceNumber
    And I should see a display for PremiseNumber with "0000000042"

Scenario: Admin can copy a service with service number and category is null
    Given I am logged in as "admin"
    When I visit the Show page for service: "three"
    And I follow "Copy W/Service Number"
    When I select service category "fire service installation" from the ServiceCategory dropdown
    And I enter "123456789" into the Installation field
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I check the DeviceLocationUnavailable field
    And I check the PremiseNumberUnavailable field
    And I select premise unavailable reason "one" from the PremiseUnavailableReason dropdown        
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then the currently shown service will now be referred to as "kevin"
    And I should see a display for ServiceNumber with service "three"'s ServiceNumber

Scenario: Admin can copy a service as a sewer service and a bunch of stuff is null
    Given I am logged in as "admin"
    When I visit the Show page for service: "three"
    And I follow "Create Sewer Service"
    When I select service category "fire service installation" from the ServiceCategory dropdown
    And I enter "123456789" into the Installation field
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "one" from the Town dropdown
    And I check the DeviceLocationUnavailable field
    And I check the PremiseNumberUnavailable field
    And I select premise unavailable reason "one" from the PremiseUnavailableReason dropdown        
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then the currently shown service will now be referred to as "koral"
    And I should see a display for ServiceNumber with service "three"'s ServiceNumber
    
Scenario: Admin can select an operating center without choosing a state
    Given I am logged in as "admin"    
    And I am at the FieldOperations/Service/Search page
    Then I should see operating center "nj7" in the OperatingCenter dropdown
    When I select state "two" from the State dropdown
    Then I should not see operating center "nj7" in the OperatingCenter dropdown
    When I select state "one" from the State dropdown
    Then I should see operating center "nj7" in the OperatingCenter dropdown

    Scenario: user without user admin role can not add, edit, or remove a premise contact
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role "fieldserviceassets-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service premise contact "one" exists with service: "one"
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Account Contacts" tab
        Then I should not see "Add Premise Contact"
        And I should not see a link to the edit page for service premise contact: "one"
        And I should not see "Remove"

    Scenario: user with add role cannot edit or remove a premise contact or flush
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role "fieldserviceassets-read" exists with action: "Add", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service premise contact "one" exists with service: "one"
        And a service flush "one" exists with service: "one"
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Account Contacts" tab
        Then I should see "Add Account Contact"
        And I should not see a link to the edit page for service premise contact: "one"
        And I should not see "Remove"
        When I click the "Water Quality Samples" tab
        Then I should see "Add Water Quality Sample"
        And I should not see a link to the edit page for service flush: "one"
        And I should not see "Remove"

    Scenario: user with edit role cannot add a premise contact or flush
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role "fieldserviceassets-read" exists with action: "Edit", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service premise contact "one" exists with service: "one"
        And a service flush "one" exists with service: "one"
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Account Contacts" tab
        Then I should not see "Add Premise Contact"
        And I should see a link to the edit page for service premise contact: "one"
        And I should see "Remove"
        When I click the "Water Quality Samples" tab
        Then I should not see "Add Water Quality Sample"
        And I should see a link to the edit page for service flush: "one"
        And I should see "Remove"

    Scenario: user with user admin role can add a premise contact
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service premise contact method "one" exists
        And a service premise contact type "one" exists
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Account Contacts" tab
        And I press "Add Account Contact"
        And I enter "4/24/1984" into the ViewModel_ContactDate field
        And I select service premise contact method "one" from the ViewModel_ContactMethod dropdown
        And I select service premise contact type "one" from the ViewModel_ContactType dropdown
        And I select "Yes" from the ViewModel_NotifiedCustomerServiceCenter dropdown
        And I select "No" from the ViewModel_CertifiedLetterSent dropdown
        And I enter "Blah" into the ViewModel_ContactInformation field
        And I press save-customer-contact
        And I click the "Account Contacts" tab
        Then I should see the following values in the customer-contacts-table table
            | Contact Date | Contact Method                        | Contact Type                        | Notified Customer Service Center | Certified Letter Sent | Contact Information |
            | 4/24/1984    | service premise contact method: "one" | service premise contact type: "one" | Yes                              | No                    | Blah                |

    Scenario: user with user admin role can edit a premise contact
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service premise contact "one" exists with service: "one", contact date: "4/24/1984", notified customer service center: "true", certified letter sent: "false", contact information: "Blah"
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Account Contacts" tab
        Then I should see the following values in the customer-contacts-table table
            | Contact Date | Notified Customer Service Center | Certified Letter Sent | Contact Information |
            | 4/24/1984    | Yes                              | No                    | Blah                |
        When I follow the Edit link for service premise contact "one"
        And I enter "Something else" into the ContactInformation field
        And I press Save
        And I click the "Account Contacts" tab
        Then I should see the following values in the customer-contacts-table table
            | Contact Date | Notified Customer Service Center | Certified Letter Sent | Contact Information |
            | 4/24/1984    | Yes                              | No                    | Something else      |

    Scenario: user with user admin role can remove a premise contact
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service premise contact "one" exists with service: "one", contact date: "4/24/1984", notified customer service center: "true", certified letter sent: "false", contact information: "Blah"
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Account Contacts" tab
        Then I should see the following values in the customer-contacts-table table
            | Contact Date | Notified Customer Service Center | Certified Letter Sent | Contact Information |
            | 4/24/1984    | Yes                              | No                    | Blah                |
        When I click ok in the dialog after pressing "Remove"
        Then I should be at the Show page for service: "one"
        And the customer-contacts-table table should be empty

    Scenario: user without user admin role can not add, edit, or remove a flush
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role "fieldserviceassets-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service flush "one" exists with service: "one"
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Water Quality Samples" tab
        Then I should not see "Add Flush"
        And I should not see a link to the edit page for service flush: "one"
        And I should not see "Remove"

    Scenario: user with user admin role can add a flush
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service flush premise contact method "one" exists
        And a service flush flush type "one" exists
        And a service flush sample type "one" exists
        And a service flush sample status "one" exists
        And a service flush sample taken by type "one" exists
        And a service flush replacement type "one" exists
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Water Quality Samples" tab
        And I press "Add Water Quality Sample"
        And I enter "4/24/1984" into the ViewModel_PremiseContactDate field
        And I enter "4/24/1984" into the ViewModel_SampleDate field
        And I select service flush premise contact method "one" from the ViewModel_FlushContactMethod dropdown
        And I select service flush flush type "one" from the ViewModel_FlushType dropdown
        And I select service flush sample type "one" from the ViewModel_SampleType dropdown
        And I select service flush sample status "one" from the ViewModel_SampleStatus dropdown
        And I select service flush sample taken by type "one" from the ViewModel_TakenBy dropdown
        And I select service flush replacement type "one" from the ViewModel_ReplacementType dropdown
        And I select "Yes" from the ViewModel_FlushNotifiedCustomerServiceCenter dropdown
        And I select "Passed" from the ViewModel_SampleResultPassed dropdown
        And I enter "12345" into the ViewModel_SampleId field
        And I press save-flush
        And I click the "Water Quality Samples" tab
        Then I should see the following values in the flushes-table table
            | Flush Type                      | Sample Type                      | Sample Result | Premise Contact Date | Sample Id |
            | service flush flush type: "one" | service flush sample type: "one" | Passed        | 4/24/1984            | 12345     |

    Scenario: user with user admin role can edit a flush
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service flush "one" exists with service: "one", premise contact date: "4/24/1984", notified customer service center: "true", sample date: "5/25/1985"
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Water Quality Samples" tab
        Then I should see the following values in the flushes-table table
            | Premise Contact Date | Notified Customer Service Center |
            | 4/24/1984            | Yes                              |
        When I follow the Edit link for service flush "one"
        And I select "No" from the FlushNotifiedCustomerServiceCenter dropdown
        And I press Save
        And I click the "Water Quality Samples" tab
        Then I should see the following values in the flushes-table table
            | Premise Contact Date | Notified Customer Service Center |
            | 4/24/1984            | No                               |

    Scenario: user with user admin role can remove a flush
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And a service flush "one" exists with service: "one", premise contact date: "4/24/1984", notified customer service center: "true", sample date: "5/25/1985"
        And I am logged in as "user"
        And I am at the Show page for service: "one"
        When I click the "Water Quality Samples" tab
        Then I should see the following values in the flushes-table table
            | Premise Contact Date | Notified Customer Service Center |
            | 4/24/1984            | Yes                              |
        When I click ok in the dialog after pressing "Remove"
        Then I should be at the Show page for service: "one"
        And the flushes-table table should be empty

    Scenario: user should see other point field visibility toggle based on termination point
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role exists with action: "Add", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And I am logged in as "user"
        And I am at the FieldOperations/Service/New page
        # By default it shouldn't be visible
        Then I should not see the OtherPoint field
        When I select inside shutoff service termination point "one" from the TerminationPoint dropdown
        Then I should not see the OtherPoint field
        When I select other service termination point "other" from the TerminationPoint dropdown
        Then I should see the OtherPoint field

    Scenario: user can create a new service from an existing service linked to a work order
        Given I am logged in as "admin"
        And a service "linked" exists with premise number: "000000000", operating center: "nj7", town: "one"
        And a work order "one" exists with premise number: "000000000", operating center: "nj7", town: "one"
        #assumption is the background above isn't creating any orders
        When I visit the FieldOperations/Service/LinkOrNew?workOrderId=1 page
        Then I should not see "There are no existing services that match."
        When I press "Link Service"
        And I wait for the page to reload
        Then I should see a display for OperatingCenter with operating center "nj7"

Scenario: user can add a service and see the validation
    Given I am logged in as "admin"
    When I visit the FieldOperations/Service/New page
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select town "one" from the Town dropdown
    And I select street "one" from the Street dropdown
    And I select service category "fire retire service only" from the ServiceCategory dropdown
    And I enter "7" into the LengthOfService field
    And I enter "10" into the Block field
    And I enter "101" into the Lot field
    And I enter "124124124124124124124" into the LeadServiceReplacementWbs field
    And I enter "124124124124124124124" into the LeadServiceRetirementWbs field
    And I select service category "fire retire service only" from the ServiceCategory dropdown
    And I check the LeadAndCopperCommunicationProvided field
    And I select contractor "nj7" from the CustomerSideSLReplacementContractor dropdown
    And I select customer side s l replacement offer status "two" from the CustomerSideSLReplacement dropdown
    And I select "Yes" from the CompanyOwned dropdown
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then I should see a validation message for PremiseNumber with "The Premise Number field is required."
    And I should see a validation message for PremiseNumberUnavailable with "Please confirm that there is no premise number available or enter the premise number below."
    And I should see a validation message for Installation with "The Installation Number field is required."
    And I should not see the PremiseUnavailableReason field 
    When I check the PremiseNumberUnavailable field
    And I press Save
    Then I should see a validation message for PremiseUnavailableReason with "The Premise Number Unavailable Reason is required."
    When I select premise unavailable reason "one" from the PremiseUnavailableReason dropdown
    And I press Save
    Then I should not see a validation message for PremiseNumber with "The Premise Number field is required."
    And I should not see a validation message for PremiseNumberUnavailable with "Please confirm that there is no premise number available or enter the premise number below."
    And I should not see a validation message for Installation with "The Installation Number is required."
    And I should see a display for PremiseNumberUnavailable with "Yes"
    And I should see a display for PremiseUnavailableReason with "Killed Premise"
    And I should see a display for DeviceLocationUnavailable with "Yes"
    And I should see a display for CustomerSideReplacementWBSNumber with w b s number "one"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for Street with street "one"
    And I should see a display for LengthOfService with "7"
    And I should see a display for Block with "10"
    And I should see a display for Lot with "101"
    And I should see a display for LeadServiceReplacementWbs with "124124124124124124124"
    And I should see a display for LeadServiceRetirementWbs with "124124124124124124124"
    And I should see a display for ServiceCategory with service category "fire retire service only"
    And I should see a display for LeadAndCopperCommunicationProvided with "Yes"
    And I should see a display for CustomerSideSLReplacementContractor with contractor "nj7"
    And I should see a display for CustomerSideSLReplacement with customer side s l replacement offer status "two"
    And I should see a display for CompanyOwned with "Yes"

    Scenario: user can search for a service using premise unavailable reason
        Given I am logged in as "admin"
        When I visit the FieldOperations/Service/Search page
        And I select premise unavailable reason "one" from the PremiseUnavailableReason dropdown
        And I press Search
        Then I should see a link to the Show page for service: "two"
        When I follow the Show link for service "two"
        Then I should be at the Show page for service: "two"
        And I should see a display for PremiseUnavailableReason with "Killed Premise"

    Scenario: user can see a link for premise number
        Given I am logged in as "admin"
        When I visit the Show page for service: "five"
        Then I should see a link to the Show page for premise "one"
        When I follow the Show link for premise "one"
        Then I should be at the Show page for premise "one"

Scenario: user can create a new service and see the correct validations
    Given I am logged in as "admin"
    When I visit the FieldOperations/Service/New page
    And I select state "one" from the State dropdown
    And I press Save
    Then I should see a validation message for PremiseNumber with "The Premise Number field is required."
    And I should see a validation message for PremiseNumberUnavailable with "Please confirm that there is no premise number available or enter the premise number below."
    And I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    And I should see a validation message for Block with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for Lot with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for StreetNumber with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for ServiceCategory with "The Category of Service field is required."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I press Save
    Then I should see a validation message for Town with "The Town field is required."
    When I select town "one" from the Town dropdown
    And I press Save
    Then I should see a validation message for Street with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    When I select street "one" from the Street dropdown
    And I select service category "install meter set" from the ServiceCategory dropdown
    Then I should not see the CustomerSideSLReplacement field
    And I should not see the FlushingOfCustomerPlumbing field
    And I should not see the CustomerSideSLReplacedBy field
    And I should not see the CustomerSideSLReplacementContractor field
    And I should not see the LengthOfCustomerSideSLReplaced field
    And I should not see the CustomerSideSLReplacementCost field
    And I should not see the CustomerSideReplacementDate field
    And I should not see the OfferedAgreement field
    And I should not see the OfferedAgreementDate field
    And I should not see the ServiceRegroundingPremiseType field
    When I press Save
    And I select service category "fire retire service only" from the ServiceCategory dropdown
    And I enter "0000000000" into the PremiseNumber field
    And I press Save
    And I wait 1 second
    Then I should not see a validation message for PremiseNumberUnavailable with "Please confirm that there is no premise number available or enter the premise number below."
    And I should see a validation message for PremiseNumber with "Invalid Premise Number. Please check PremiseNumberUnavailable if you don't have a valid premise number."
    When I check the PremiseNumberUnavailable field
    And I press Save
    Then I should not see a validation message for PremiseNumber with "Invalid Premise Number. Please check PremiseNumberUnavailable if you don't have a valid premise number."
    When I select premise unavailable reason "one" from the PremiseUnavailableReason dropdown
    And I enter "10" into the Block field
    And I enter "101" into the Lot field
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then the currently shown service will now be referred to as "vincent"
    And I should be at the show page for service: "vincent"

    Scenario: User can see contractor log results
        Given an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 1", timestamp: "8/16/2118 12:59:00 PM", ContractorUser: "user", User: null
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 2", timestamp: "8/16/2118 12:58:00 PM", ContractorUser: "user", User: null
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 3", timestamp: "8/16/2118 12:57:00 PM", User: "testuser"
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 4", timestamp: "8/16/2118 12:56:00 PM", ContractorUser: "user", User: null
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 5", timestamp: "8/16/2118 12:55:00 PM", ContractorUser: "user", User: null
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 6", timestamp: "8/16/2118 12:54:00 PM", User: "testuser"
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 7", timestamp: "8/16/2118 12:53:00 PM", ContractorUser: "user", User: null
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 8", timestamp: "8/16/2118 12:52:00 PM", ContractorUser: "user", User: null
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 9", timestamp: "8/16/2118 12:51:00 PM", User: "testuser"
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 10", timestamp: "8/16/2118 12:50:00 PM", ContractorUser: "user", User: null
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 11", timestamp: "8/16/2118 12:49:00 PM", User: "testuser"
        And an audit log entry exists with entity name: "Service", entity id: "1", field name: "Entry 12", timestamp: "8/16/2118 12:48:00 PM", ContractorUser: "user", User: null
        And I am logged in as "admin"
        When I visit the Show page for service: "one"
        And I click the "Log" tab
        And I wait for ajax to finish loading
        Then I should see the following values in the auditLogEntryTable table
            | Entity Name | Field Name | Time Stamp (EST)            | Contractor            | User  |
            | Service     | Entry 1    | 8/16/2118 12:59:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 2    | 8/16/2118 12:58:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 3    | 8/16/2118 12:57:00 PM (EST) |                       | Smith |
            | Service     | Entry 4    | 8/16/2118 12:56:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 5    | 8/16/2118 12:55:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 6    | 8/16/2118 12:54:00 PM (EST) |                       | Smith |
            | Service     | Entry 7    | 8/16/2118 12:53:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 8    | 8/16/2118 12:52:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 9    | 8/16/2118 12:51:00 PM (EST) |                       | Smith |
            | Service     | Entry 10   | 8/16/2118 12:50:00 PM (EST) | Kevin - user@site.com |       |
        When I follow "2"
        And I wait for ajax to finish loading
        Then I should see the following values in the auditLogEntryTable table
            | Entity Name | Field Name | Time Stamp (EST)            | Contractor            | User  |
            | Service     | Entry 11   | 8/16/2118 12:49:00 PM (EST) |                       | Smith |
            | Service     | Entry 12   | 8/16/2118 12:48:00 PM (EST) | Kevin - user@site.com |       |
        When I follow "<<"
        And I wait for ajax to finish loading
        Then I should see the following values in the auditLogEntryTable table
            | Entity Name | Field Name | Time Stamp (EST)            | Contractor            | User  |
            | Service     | Entry 1    | 8/16/2118 12:59:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 2    | 8/16/2118 12:58:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 3    | 8/16/2118 12:57:00 PM (EST) |                       | Smith |
            | Service     | Entry 4    | 8/16/2118 12:56:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 5    | 8/16/2118 12:55:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 6    | 8/16/2118 12:54:00 PM (EST) |                       | Smith |
            | Service     | Entry 7    | 8/16/2118 12:53:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 8    | 8/16/2118 12:52:00 PM (EST) | Kevin - user@site.com |       |
            | Service     | Entry 9    | 8/16/2118 12:51:00 PM (EST) |                       | Smith |
            | Service     | Entry 10   | 8/16/2118 12:50:00 PM (EST) | Kevin - user@site.com |       |

    Scenario: user can see the correct validation when creating a new service
        Given I am logged in as "admin"
        When I visit the FieldOperations/Service/New page
        And I select state "one" from the State dropdown
        And I select operating center "nj7" from the OperatingCenter dropdown
        And I press Save
        Then I should not see a validation message for OriginalInstallationDate with "The Original Installation Date field is required."
        And I should not see a validation message for PreviousServiceMaterial with "The Previous Service Material field is required."
        And I should not see a validation message for PreviousServiceSize with "The Previous Service Size field is required."
        When I select service category "water retire service only" from the ServiceCategory dropdown
        And I press Save
        Then I should not see a validation message for OriginalInstallationDate with "The Original Installation Date field is required."
        And I should not see a validation message for PreviousServiceMaterial with "The Previous Service Material field is required."
        And I should not see a validation message for PreviousServiceSize with "The Previous Service Size field is required."
        When I enter today's date into the DateInstalled field
        And I press Save
        Then I should see a validation message for OriginalInstallationDate with "The Original Installation Date field is required."
        And I should see a validation message for PreviousServiceMaterial with "The Previous Service Material field is required."
        And I should see a validation message for PreviousServiceSize with "The Previous Service Size field is required."

Scenario: Admin can add a service with water service customer side service category
    Given I am logged in as "admin"
    When I visit the FieldOperations/Service/New page
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select town "one" from the Town dropdown
    And I select street "one" from the Street dropdown
    And I select service category "water service renewal cust side" from the ServiceCategory dropdown
    And I enter "1234567890" into the PremiseNumber field
    And I enter "123456789" into the Installation field
    And I enter "7" into the LengthOfService field
    And I enter "10" into the Block field
    And I enter "101" into the Lot field
    And I enter "124124124124124124124" into the LeadServiceReplacementWbs field
    And I enter "124124124124124124124" into the LeadServiceRetirementWbs field
    And I check the LeadAndCopperCommunicationProvided field
    And I select contractor "nj7" from the CustomerSideSLReplacementContractor dropdown
    And I select customer side s l replacement offer status "two" from the CustomerSideSLReplacement dropdown
    And I select "Yes" from the CompanyOwned dropdown
    And I select service material "two" from the PreviousServiceCustomerMaterial dropdown
    And I select service size "four" from the PreviousServiceCustomerSize dropdown
    And I check the DeviceLocationUnavailable field
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then the currently shown service will now be referred to as "vincent"
    And I should be at the show page for service: "vincent"
    And I should see a display for PremiseNumber with "1234567890"
    And I should see a display for CustomerSideReplacementWBSNumber with w b s number "one"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for ServiceNumber with service "vincent"'s ServiceNumber
    And I should see a display for Street with street "one"
    And I should see a display for LengthOfService with "7"
    And I should see a display for Block with "10"
    And I should see a display for Lot with "101"
    And I should see a display for LeadServiceReplacementWbs with "124124124124124124124"
    And I should see a display for LeadServiceRetirementWbs with "124124124124124124124"
    And I should see a display for ServiceCategory with service category "water service renewal cust side"
    And I should see a display for LeadAndCopperCommunicationProvided with "Yes"
    And I should see a display for CustomerSideSLReplacementContractor with contractor "nj7"
    And I should see a display for CustomerSideSLReplacement with customer side s l replacement offer status "two"
    And I should see a display for CompanyOwned with "Yes"
    And I should see a display for PreviousServiceCustomerMaterial with service material "two"
    And I should see a display for PreviousServiceCustomerSize with service size "four"

    Scenario: user without user admin role can not edit sap notification number, sap work order number
        Given a user "user" exists with username: "user", default operating center: "nj7"
        And a role "fieldserviceassets-edit" exists with action: "Edit", module: "FieldServicesAssets", user: "user", operating center: "nj7"
        And I am logged in as "user"
        When I visit the Edit page for service: "five"
        Then I should see a display for DisplayService_SAPNotificationNumber with "9876543"
        And I should see a display for DisplayService_SAPWorkOrderNumber with "1234567"

Scenario: user with user admin role can edit sap notification number, sap work order number
    Given I am logged in as "admin"    
    And I am at the Show page for service: "five"
    Then I should see a display for SAPNotificationNumber with "9876543"
    And I should see a display for SAPWorkOrderNumber with "1234567"
    When I visit the Edit page for service: "five"
    And I select street "one" from the Street dropdown
    And I enter "10" into the Block field
    And I enter "101" into the Lot field
    And I enter "1234567890" into the PremiseNumber field
    And I enter "123456789" into the Installation field
    And I enter "10000000" into the SAPNotificationNumber field
    And I enter "9999999999" into the SAPWorkOrderNumber field
    And I check the DeviceLocationUnavailable field
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    And I press Save
    Then I should see a display for SAPNotificationNumber with "10000000"
    And I should see a display for SAPWorkOrderNumber with "9999999999"

Scenario: user can create a new service without customer side lead service line replacement status
    Given I am logged in as "admin"
    When I visit the FieldOperations/Service/New page
    And I select state "one" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select town "one" from the Town dropdown
    And I select street "one" from the Street dropdown
    And I select service category "install meter set" from the ServiceCategory dropdown
    And I check the PremiseNumberUnavailable field
    And I select premise unavailable reason "one" from the PremiseUnavailableReason dropdown
    And I enter "10" into the Block field
    And I enter "101" into the Lot field
    And I select "Yes" from the MeterSettingRequirement dropdown
    And I select service size "three" from the MeterSettingSize dropdown
    Then I should not see the CustomerSideSLReplacement field
    And I should not see the FlushingOfCustomerPlumbing field
    And I should not see the CustomerSideSLReplacedBy field
    And I should not see the CustomerSideSLReplacementContractor field
    And I should not see the LengthOfCustomerSideSLReplaced field
    And I should not see the CustomerSideSLReplacementCost field
    And I should not see the CustomerSideReplacementDate field
    And I should not see the OfferedAgreement field
    And I should not see the OfferedAgreementDate field
    And I should not see the ServiceRegroundingPremiseType field
    When I press Save
    Then the currently shown service will now be referred to as "vincent"
    And I should be at the show page for service: "vincent"    

Scenario: Admin can add a service with sewer service category 
    Given I am logged in as "admin"
    When I visit the FieldOperations/Service/New page
    And I select state "one" from the State dropdown
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select town "one" from the Town dropdown
    And I select street "one" from the Street dropdown
    And I press Save
    Then I should see a validation message for PremiseNumber with "The Premise Number field is required."
    And I should see a validation message for PremiseNumberUnavailable with "Please confirm that there is no premise number available or enter the premise number below."
    And I should see a validation message for Block with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for Lot with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for StreetNumber with "You must enter either a 'Street Number and Name' or a 'Block and Lot'."
    And I should see a validation message for ServiceCategory with "The Category of Service field is required."
    And I should not see a validation message for MeterSettingRequirement with "The Meter Setting Requirement field is required."
    And I should not see a validation message for MeterSettingSize with "The Meter Setting Size field is required."
    When I select service category "sewer reconnect" from the ServiceCategory dropdown
    And I enter "0000000000" into the PremiseNumber field
    And I press Save
    And I wait 1 second
    Then I should not see a validation message for PremiseNumberUnavailable with "Please confirm that there is no premise number available or enter the premise number below."
    And I should not see a validation message for MeterSettingRequirement with "The Meter Setting Requirement field is required."
    And I should not see a validation message for MeterSettingSize with "The Meter Setting Size field is required."
    When I enter "1234567890" into the PremiseNumber field
    And I enter "123456789" into the Installation field
    And I enter "123456789" into the DeviceLocation field
    And I enter "7" into the LengthOfService field
    And I enter "10" into the Block field
    And I enter "101" into the Lot field
    And I enter "124124124124124124124" into the LeadServiceReplacementWbs field
    And I enter "124124124124124124124" into the LeadServiceRetirementWbs field
    And I select service category "fire retire service only" from the ServiceCategory dropdown
    And I check the LeadAndCopperCommunicationProvided field
    And I select contractor "nj7" from the CustomerSideSLReplacementContractor dropdown
    And I select customer side s l replacement offer status "two" from the CustomerSideSLReplacement dropdown
    And I select "Yes" from the CompanyOwned dropdown
    And I press Save
    Then the currently shown service will now be referred to as "vincent"
    And I should be at the show page for service: "vincent"
    And I should see a display for PremiseNumber with "1234567890"
    And I should see a display for DeviceLocation with "123456789"
    And I should see a display for CustomerSideReplacementWBSNumber with w b s number "one"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for ServiceNumber with service "vincent"'s ServiceNumber
    And I should see a display for Street with street "one"
    And I should see a display for LengthOfService with "7"
    And I should see a display for Block with "10"
    And I should see a display for Lot with "101"
    And I should see a display for LeadServiceReplacementWbs with "124124124124124124124"
    And I should see a display for LeadServiceRetirementWbs with "124124124124124124124"
    And I should see a display for ServiceCategory with service category "fire retire service only"
    And I should see a display for LeadAndCopperCommunicationProvided with "Yes"
    And I should see a display for CustomerSideSLReplacementContractor with contractor "nj7"
    And I should see a display for CustomerSideSLReplacement with customer side s l replacement offer status "two"
    And I should see a display for CompanyOwned with "Yes"
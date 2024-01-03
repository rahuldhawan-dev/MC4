Feature: ServiceInstallationPage

Background: 
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg"
    And a street "one" exists with town: "nj7burg", full st name: "Easy St", is active: true
    And a street "two" exists with town: "nj7burg", is active: true
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
    And work order requesters exist
    And work order purposes exist
    And work order priorities exist
    And work descriptions exist
    And markout requirements exist
	And meter supplemental locations exist
	And miu install reason codes exist
	And small meter locations exist
	And meter directions exist
	And a service installation read type "one" exists with description: "AMI - Badger"
	And a service installation read type "two" exists with description: "AMI - Sensus"
	And a service installation first activity "one" exists with description: "activity 1-1"
	And a service installation first activity "two" exists with description: "activity 1-2"
	And a service installation position "one" exists with description: "position 1"
	And a service installation position "two" exists with description: "position 2"
	And a service installation purpose "one" exists with description: "purpose 1"
	And a service installation purpose "two" exists with description: "purpose 2"
	And a service installation reason "one" exists with description: "reason 1"
	And a service installation reason "two" exists with description: "reason 2"
    And a coordinate "one" exists	
    And a user "user" exists with username: "user"
	And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-edit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-delete" exists with action: "Delete", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a work order "one" exists with operating center: "nj7", device location: 6002164522
	And meter supplemental location: "inside" exists in small meter location: "curb"
    And an meter location "one" exists with description: "one", code: "c1"

Scenario: User can search and view a service installation
	Given a service installation "one" exists with work order: "one"
	And I am logged in as "user"
	When I visit the FieldOperations/ServiceInstallation/Search page
	And I press "Search"
	Then I should see a link to the Show page for service installation: "one"
	When I follow the Show link for service installation "one"
	Then I should be at the Show page for service installation: "one"

Scenario: User can click save even if invalid data is entered for MeterManufacturerSerialNumber
	Given I do not currently function
	And I am logged in as "user"
	And a work order "two" exists with operating center: "nj7", device location: 6003973582
	When I visit the FieldOperations/ServiceInstallation/New page
	And I enter work order "two"'s Id into the WorkOrder field
	And I enter "Test" into the MeterManufacturerSerialNumber field
	Then the NSISave button should be disabled
	When I press Verify
	And I wait for ajax to finish loading
	Then I should see "Enter Valid Serial No/Equipment number" in the ajaxError element
	And the NSISave button should be enabled
	And the Manufacturer field should be enabled
	And the MeterSize field should be enabled
	And the ServiceType field should be enabled
	And the MeterSerialNumber field should be enabled
	And the Register1Dials field should be enabled
	And the Register1UnitOfMeasure field should be enabled
	And the Register1Size field should be enabled
	And the RegisterTwoDials field should be enabled
	And the Register2UnitOfMeasure field should be enabled
	And the Register2Size field should be enabled

# Alex said to ignore this because the test will never be able to pass because of SAP. -Ross 6/14/2017
Scenario: User can almost create a service installation if it weren't for SAP
	Given I do not currently function
	And I am logged in as "user"
	When I visit the FieldOperations/ServiceInstallation/New page
	And I enter work order "one"'s Id into the WorkOrder field
	And I enter "1234" into the MeterManufacturerSerialNumber field
	And I press Verify
	And I wait for ajax to finish loading
	# This test fails here because the Verify button isn't returning a successful result, which 
	# means the rest of the fields are still hidden. The test can't continue until these are visible.
	And I press Save
	#Then I should see a validation message for MeterManufacturerSerialNumber with "The MeterManufacturerSerialNumber field is required."
	Then I should see a validation message for MeterLocation with "The MeterLocation field is required."
	And I should see a validation message for MeterDirectionalLocation with "The MeterDirectionalLocation field is required."
	And I should see a validation message for ReadingDeviceSupplemental with "The Reading Device Location field is required."
	And I should see a validation message for ReadingDeviceDirectionalInformation with "The Reading Device Directional Location field is required."
	And I should see a validation message for Register1ReadType with "The Register1ReadType field is required."
	And I should see a validation message for Register1RFMIU with "The Register 1 RF/MIU field is required."
	And I should see a validation message for Register1CurrentRead with "The Register1CurrentRead field is required."
	And I should see a validation message for Activity1 with "The Activity1 field is required."
	And I should see a validation message for ServiceFound with "The ServiceFound field is required."
	And I should see a validation message for ServiceLeft with "The ServiceLeft field is required."
	And I should see a validation message for OperatedPointOfControl with "The OperatedPointOfControl field is required."
	And I should see a validation message for ServiceInstallationReason with "The Reason for Install/Replace/Remove field is required."
	When I select meter supplemental location "inside" from the MeterLocation dropdown
	And I select meter supplemental location "inside" from the ReadingDeviceSupplemental dropdown
	And I select miu install reason code "new install ami" from the MiuInstallReason dropdown
	And I press Save	
	Then I should see a validation message for ReadingDevicePosition with "The Reading Positional Location field is required."
	Then I should see a validation message for MeterLocationInformation with "The MeterLocationInformation field is required."
	When I enter "123456" into the MeterManufacturerSerialNumber field 
	And I select meter direction "left side" from the MeterDirectionalLocation dropdown
	And I select meter supplemental location "inside" from the ReadingDeviceSupplemental dropdown
	And I select meter direction "left side" from the ReadingDeviceDirectionalInformation dropdown
	And I select service installation read type "one" from the Register1ReadType dropdown
	And I select service installation first activity "one" from the Activity1 dropdown
	And I select service installation position "one" from the ServiceFound dropdown
	And I select service installation position "one" from the ServiceLeft dropdown
	And I select service installation reason "one" from the ServiceInstallationReason dropdown
	And I enter "321321" into the Register1RFMIU field
	And I enter "000001" into the Register1CurrentRead field
	And I enter "some notes about the install" into the MeterLocationInformation field
	When I press Save
	Then I should see a validation message for MeterPositionalLocation with "The MeterPositionalLocation field is required."
	And I should see a validation message for ReadingDevicePosition with "The Reading Positional Location field is required."
	When I enter "1234" into the RegisterTwoDials field
	And I press Save
	Then I should see a validation message for Register2ReadType with "The Register2ReadType field is required."
	And I should see a validation message for Register2RFMIU with "The Register 2 RF/MIU field is required."
	And I should see a validation message for Register2CurrentRead with "Please enter a numeric reading with leading zeros that matches the number of dials."
	When I enter "" into the RegisterTwoDials field
	And I select small meter location "curb" from the MeterPositionalLocation dropdown
	And I select small meter location "curb" from the ReadingDevicePosition dropdown
	And I select "Yes" from the OperatedPointOfControl dropdown
	And I press Save
	Then I should not see a validation message for OperatedPointOfControl with "The OperatedPointOfControl field is required."
	#Can't continue without sap returning a value
	#Then the currently shown service installation shall henceforth be known throughout the land as "the install"
    #And I should see a display for Register1CurrentRead with "000001"
	
Scenario: User can edit a service installation
	Given a service installation "one" exists with work order: "one", register 1 dials: "6"
	And I am logged in as "user"
	When I visit the Edit page for service installation: "one"
	And I select meter supplemental location "inside" from the ReadingDeviceSupplemental dropdown
	And I select meter supplemental location "inside" from the MeterLocation dropdown
	And I select small meter location "curb" from the MeterPositionalLocation dropdown
	And I select small meter location "curb" from the ReadingDevicePosition dropdown
	And I select miu install reason code "new install ami" from the MiuInstallReason dropdown
	And I press Save
	Then I should be at the Show page for service installation: "one"
	And I should see a display for MiuInstallReason with "new install ami"

Scenario: User can destroy a service installation
	Given a service installation "one" exists
	And I am logged in as "user"
	When I visit the Show page for service installation: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the FieldOperations/ServiceInstallation/Search page
	When I try to access the Show page for service installation: "one" expecting an error
	Then I should see a 404 error message

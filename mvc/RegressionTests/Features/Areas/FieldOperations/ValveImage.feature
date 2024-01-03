Feature: ValveImage

Background: users exist
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "FieldServicesImages", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesImages", user: "user"
	And a user "noroles" exists with username: "noroles"
	And an asset type "valve" exists with description: "valve"
	And a valve open direction "right" exists with description: "Right"
	And a valve size "22" exists with size: 22
	And a valve normal position "one" exists with description: "NORMALLY OPEN"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a state "one" exists with name: "New Jersey Again", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour", county: "one", state: "one"
	And operating center: "nj4" exists in town: "one" 
	And a town section "one" exists with name: "A section", town: "one"
	And a role "roleValvesRead" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj4"
	And a street "one" exists with town: "one", is active: true
	And a street "two" exists with town: "one", full st name: "Easy St", is active: true
	And an admin user "admin" exists with username: "admin"

Scenario: User should see a lot of validation messages when creating a valve image
	Given I am logged in as "user"
	When I visit the /FieldOperations/ValveImage/New page
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for CrossStreet with "The CrossStreet field is required."
	And I should see a validation message for IsDefaultImageForValve with "The Is Default Image for Valve field is required."
	And I should see a validation message for ValveSize with "The ValveSize field is required."
	And I should see a validation message for FileUpload.Key with "The FileUpload field is required."
	When I select operating center "nj4" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."

Scenario: User can autopopulate values from an existing valve
	Given a valve "one" exists with valve number: "VAB-100", valve suffix: "100", operating center: "nj4", town: "one", street: "one", valve location: "Some location", valve size: "22", normal position: "one", open direction: "right", cross street: "two", town section: "one"
	And I am logged in as "user"
	When I visit the Show page for valve: "one"
	And I wait 1 seconds
	When I visit the /FieldOperations/ValveImage/New page
	And I enter "VAB-100" into the ValveNumberSearch field
	And I select operating center "nj4" from the OperatingCenterIdentifier dropdown
	And I press "Look Up"
	And I wait for ajax to finish loading
	#A lot of cascading happens here, so give it a second 
	And I wait 1 seconds 
	Then operating center "nj4"'s ToString should be selected in the OperatingCenter dropdown
	And town "one"'s ToString should be selected in the Town dropdown
	And street "one"'s ToString should be selected in the StreetIdentifyingInteger dropdown
	And valve "one"'s ToString should be selected in the Valve dropdown
	And valve normal position "one"'s Description should be selected in the NormalPosition dropdown 
	And valve open direction "right"'s Description should be selected in the OpenDirection dropdown
	And I should see "VAB-100" in the ValveNumber field
	And I should see "Some location" in the Location field
	And I should see "22" in the ValveSize field
	And I should see "A section" in the TownSection field
	And I should see street "two"'s FullStName in the CrossStreet field

Scenario: User can create a valve image 
	Given I am logged in as "user"
	When I visit the /FieldOperations/ValveImage/New page
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I enter "Some Section" into the TownSection field
	And I enter "N" into the StreetPrefix field
	And I enter "10th" into the Street field
	And I enter "St" into the StreetSuffix field
	And I enter "S" into the CrossStreetPrefix field
	And I enter "9th" into the CrossStreet field
	And I enter "Ave" into the CrossStreetSuffix field
	And I enter "12" into the ValveSize field
	And I enter "12345" into the ValveNumber field
	And I select "Yes" from the IsDefaultImageForValve dropdown
	And I upload "itsatiff.tiff"
	And I press Save 
	Then the currently shown valve image shall henceforth be known throughout the land as "zoink"
	And I should delete valve image "zoink"'s file to clean up after this test
	And I should see a display for Town_County_State with state "one"'s Abbreviation
	And I should see a display for Town_County with county "one"
	And I should see a display for Town with town "one"
	And I should see a display for TownSection with "Some Section"
	And I should see a display for OperatingCenter with operating center "nj4"
	And I should see a display for FullStreetName with "N 10th St"
	And I should see a display for FullCrossStreetName with "S 9th Ave"
	And I should see a display for ValveNumber with "12345"
	And I should see a display for ValveSize with "12"
	And I should see a display for IsDefaultImageForValve with "Yes"

Scenario: User can copy a valve image and retain some of the details from the original record
	Given a valve image "one" exists with valve number: "123456", operating center: "nj4", street number: "6"
	And I am logged in as "user"
	When I visit the /FieldOperations/ValveImage/Show page for valve image: "one"
	And I follow "Copy"
	Then I should see "123456" in the ValveNumber field
	And I should see "6" in the StreetNumber field
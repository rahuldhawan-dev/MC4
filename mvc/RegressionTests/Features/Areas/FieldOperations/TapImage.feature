Feature: TapImage

Background: users exist
	Given a user "user" exists with username: "user"
	And an operating center "opc" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesImages", user: "user", operating center: "opc"
	And a role "roleReadAssets" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "opc"
	And a role "roleAdd" exists with action: "Add", module: "FieldServicesImages", user: "user", operating center: "opc"
	And a role "roleAddAssets" exists with action: "Add", module: "FieldServicesAssets", user: "user", operating center: "opc"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesImages", user: "user", operating center: "opc"
	And a role "roleEditAssets" exists with action: "Edit", module: "FieldServicesAssets", user: "user", operating center: "opc"
	And a user "noroles" exists with username: "noroles"
	And a state "one" exists with name: "New Jersey Again", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour", county: "one", state: "one"
	And operating center: "opc" exists in town: "one" 	
	And a service size "one" exists with service size description: "Size 1", size: "1"
	And a service material "one" exists with description: "I am a material"

	
Scenario: User should see a lot of validation messages when creating a valve image
	Given I am logged in as "user"
	When I visit the /FieldOperations/TapImage/New page
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for ServiceType with "The ServiceType field is required."
	And I should see a validation message for IsDefaultImageForService with "The Is Default Image for Tap field is required."
	And I should see a validation message for FileUpload.Key with "The FileUpload field is required."
	When I select operating center "opc" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."

Scenario: User can autopopulate values from an existing service
	Given a street "one" exists with town: "one", is active: true
	And a street "two" exists with town: "one", is active: true
	And a service category "one" exists with description: "Water cats"
	And a town section "one" exists with name: "A section", town: "one"
	And a service "one" exists with service number: "123", operating center: "opc", town: "one", street: "one", cross street: "two", town section: "one", service material: "one", premise number: "456", length of service: "4", service size: "one", service category: "one", previous service material: "one", previous service size: "one"
	And I am logged in as "user"
	When I visit the /FieldOperations/TapImage/New page
	And I enter "123" into the ServiceNumberSearch field
	And I enter "456" into the PremiseNumberSearch field
	And I press "Look Up"
	And I wait for ajax to finish loading
	#A lot of cascading happens here, so give it a second 
	And I wait 1 seconds 
	Then operating center "opc"'s ToString should be selected in the OperatingCenter dropdown
	And town "one"'s ToString should be selected in the Town dropdown
	And street "one"'s ToString should be selected in the StreetIdentifyingInteger dropdown
	And street "two"'s ToString should be selected in the CrossStreetIdentifyingInteger dropdown
	And service "one"'s ToString should be selected in the Service dropdown
	And I should see "A section" in the TownSection field
	And I should see "123" in the ServiceNumber field
	And I should see "456" in the PremiseNumber field
	And I should see "WATER" in the ServiceType field
	And I should see "4" in the LengthOfService field
	And I should see "Size 1" in the ServiceSize dropdown
	And I should see "I am a material" in the ServiceMaterial dropdown
	And I should see service material "one" in the PreviousServiceMaterial dropdown
	And I should see service size "one" in the PreviousServiceSize dropdown

Scenario: User can autopopulate values from an existing service and they receive the most recent service that has been created because hey why not
	Given a street "one" exists with town: "one", is active: true
	And a service category "one" exists with description: "Water cats"
	And a town section "one" exists with name: "A section", town: "one"
	And a town section "two" exists with name: "A section of the most recent service", town: "one"
	And a service "one" exists with service number: "123", operating center: "opc", town: "one", street: "one", town section: "one", service material: "one", premise number: "456", length of service: "4", service size: "one", service category: "one"
	And a service "two" exists with service number: "123", operating center: "opc", town: "one", street: "one", town section: "two", service material: "one", premise number: "456", length of service: "4", service size: "one", service category: "one"
	And I am logged in as "user"
	When I visit the /FieldOperations/TapImage/New page
	And I enter "123" into the ServiceNumberSearch field
	And I enter "456" into the PremiseNumberSearch field
	And I press "Look Up"
	And I wait for ajax to finish loading
	#A lot of cascading happens here, so give it a second 
	And I wait 1 seconds 
	Then operating center "opc"'s ToString should be selected in the OperatingCenter dropdown
	And town "one"'s ToString should be selected in the Town dropdown
	And street "one"'s ToString should be selected in the StreetIdentifyingInteger dropdown
	And service "two"'s ToString should be selected in the Service dropdown
	And I should see "A section of the most recent service" in the TownSection field
	And I should see "123" in the ServiceNumber field
	And I should see "456" in the PremiseNumber field
	And I should see "WATER" in the ServiceType field
	And I should see "4" in the LengthOfService field
	And I should see "Size 1" in the ServiceSize dropdown
	And I should see "I am a material" in the ServiceMaterial dropdown

Scenario: User can create and edit a tap image
	Given a street "one" exists with town: "one", is active: true
	And a service category "one" exists with description: "Water cats"
	And a town section "one" exists with name: "A section", town: "one"		
	And a service "one" exists with service number: "123", operating center: "opc", town: "one", street: "one", town section: "one", premise number: "456", length of service: "4", service size: "one", service material: "one", service category: "one", apartment number: "16", lot: "12", block: "9"
	And I am logged in as "user"
	When I visit the /FieldOperations/TapImage/New page
	And I enter "123" into the ServiceNumberSearch field
	And I enter "456" into the PremiseNumberSearch field		
	And I press "Look Up"
	And I wait for ajax to finish loading
	#A lot of cascading happens here, so give it a second 
	And I wait 1 seconds 
	#And I press Save
	And I upload "itsatiff.tiff"	
	And I select "Yes" from the IsDefaultImageForService dropdown
	And I enter "8" into the StreetNumber field	
	And I press Save
	Then the currently shown tap image shall henceforth be known throughout the land as "zoink"	
	And I should delete tap image "zoink"'s file to clean up after this test
	And I should see a display for Town_County_State with state "one"'s Abbreviation
	And I should see a display for Town_County with county "one"
	And I should see a display for Town with town "one"
	And I should see a display for TownSection with "A section"
	And I should see a display for OperatingCenter with operating center "opc"
	And I should see a display for StreetNumber with "8" 
	And I should see a display for FullStreetName with street "one"'s FullStName
	#And I should see a display for PremiseNumber with "456"
	And I should see a display for ServiceNumber with "123"
	And I should see a display for Service with service "one"'s Description
	And I should see a display for ApartmentNumber with "16"
	And I should see a display for Lot with "12"
	And I should see a display for Block with "9"
	And I should see a display for ServiceType with "WATER"
	And I should see a display for IsDefaultImageForService with "Yes"	
	And I should see a display for ServiceSize with "Size 1"
	And I should see a display for ServiceMaterial with "I am a material"

	When I follow "Edit"
	Then operating center "opc"'s ToString should be selected in the OperatingCenter dropdown
	And town "one"'s ToString should be selected in the Town dropdown
	And street "one"'s ToString should be selected in the StreetIdentifyingInteger dropdown
	And service "one"'s ToString should be selected in the Service dropdown
	
Scenario: User can do wildcard searches on service number for tap images
	Given a tap image "one" exists with service number: "123456", apartment number: "Garbage"
	And a tap image "two" exists with service number: "3456789"
	And a tap image "nope" exists with service number: "never find me"
	And I am logged in as "user"
	When I visit the /FieldOperations/TapImage/Search page
	And I enter "3456" into the ServiceNumber_Value field
	And I select "Wildcard" from the ServiceNumber_MatchType dropdown
	And I press Search
	Then I should see "Records found: 2"
	And I should see the following values in the search-results-table table
	| Service Number |
	| 123456         |
	| 3456789        |
	When I visit the /FieldOperations/TapImage/Search page
	And I enter "*3456*" into the ServiceNumber_Value field
	And I select "Wildcard" from the ServiceNumber_MatchType dropdown
	And I press Search
	Then I should see "Records found: 2"
	And I should see the following values in the search-results-table table
	| Service Number |
	| 123456         |
	| 3456789        |
	When I visit the /FieldOperations/TapImage/Search page
	And I enter "*Garbage*" into the ApartmentNumber_Value field
	And I select "Wildcard" from the ApartmentNumber_MatchType dropdown
	And I press Search
	Then I should see "Records found: 1"
	And I should see the following values in the search-results-table table
	| Service Number |
	| 123456         |

Scenario: User can do exact searches on service number for tap images
	Given a tap image "one" exists with service number: "123456"
	And a tap image "two" exists with service number: "123456"
	And a tap image "three" exists with service number: "345678"
	And I am logged in as "user"
	When I visit the /FieldOperations/TapImage/Search page
	And I enter "123456" into the ServiceNumber_Value field
	And I select "Exact" from the ServiceNumber_MatchType dropdown
	And I press Search
	Then I should see "Records found: 2"
	And I should see the following values in the search-results-table table
	| Service Number |
	| 123456         |
	| 123456         |

Scenario: User can copy an image and retain some of the details from the original record
	Given a tap image "one" exists with service number: "123456", operating center: "opc", premise number: "4444", apartment number: "5", street number: "6"
	And I am logged in as "user"
	When I visit the /FieldOperations/TapImage/Show page for tap image: "one"
	And I follow "Copy"
	Then I should see "123456" in the ServiceNumber field
	And I should see "4444" in the PremiseNumber field
	And I should see "5" in the ApartmentNumber field
	And I should see "6" in the StreetNumber field

Scenario: User cannot see add link on search tap images page
	Given I am logged in as "user"
	When I visit the /FieldOperations/TapImage/Search page
	Then I should not see a link to the /FieldOperations/TapImage/New page
	And I should see a link to the /FieldOperations/TapImage/Search page
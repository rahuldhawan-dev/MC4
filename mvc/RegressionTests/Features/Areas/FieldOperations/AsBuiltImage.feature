Feature: AsBuiltImage

Background: users exist
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "FieldServicesImages", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesImages", user: "user"
	And a user "noroles" exists with username: "noroles"
	And an operating center "opc" exists with opcode: "CA50", name: "Los Angeles", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a state "one" exists with name: "California", abbreviation: "CA", scada table: "cascada"
	And a county "one" exists with name: "LOS ANGELES", state: "one"
	And a town "one" exists with name: "ARCADIA", county: "one", state: "one"
	And operating center: "opc" exists in town: "one"
	And a town section "one" exists with name: "EAST PASADENA", town: "one"
	And a role "roleRead-opc" exists with action: "Read", module: "FieldServicesImages", user: "user", operating center: "opc"

Scenario: user receives a functional search page with cascading dropdowns
	Given I am logged in as "user"
	When I visit the /FieldOperations/AsBuiltImage/Search page 
	And I select state "one" from the State dropdown
	And I select county "one" from the County dropdown
	And I select town "one" from the Town dropdown
	Then I should see town section "one"'s Name in the TownSection dropdown
	When I press Search
	Then I should not see "an error"

Scenario: user receives friendly error message when no results 
	Given I am logged in as "user"
	When I visit the /FieldOperations/AsBuiltImage/Search page 
	And I select state "one" from the State dropdown
	And I select county "one" from the County dropdown
	And I press Search
    Then I should see the error message No results matched your query.

Scenario: user should be able to search for results in an operating center
	Given an as built image "one" exists with operating center: "opc", town: "one", coordinates modified on: "01/01/2013 12:00:00", file name: "foo.pdf"
	And an as built image "two" exists with operating center: "opc", town: "one", coordinates modified on: "01/02/2013 12:00:00"
	And an as built image "three" exists with coordinates modified on: "01/03/2013 12:00:00"
	And I am logged in as "user"
	When I visit the /FieldOperations/AsBuiltImage/Search page 
	And I select operating center "opc" from the OperatingCenter dropdown
	And I press Search
    Then I should see the table-caption "Records found: 2"
	And I should see as built image "one"'s CoordinatesModifiedOn in the "Coordinates Modified On" column
	And I should see as built image "one"'s FileName in the "File Name" column
	And I should see as built image "two"'s CoordinatesModifiedOn in the "Coordinates Modified On" column
	And I should not see as built image "three"'s CoordinatesModifiedOn in the "Coordinates Modified On" column
	 
Scenario: User should see a lot of validation messages when creating an as built image
	Given I am logged in as "user"
	When I visit the /FieldOperations/AsBuiltImage/New page
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for DateInstalled with "The DateInstalled field is required."
	And I should see a validation message for FileUpload.Key with "The FileUpload field is required."
	When I select operating center "opc" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."

Scenario: User can create a new as built image record
	Given I am logged in as "user"
	And I clear out the image upload root directory folder
	And a coordinate "one" exists 
	When I visit the /FieldOperations/AsBuiltImage/New page
	And I select operating center "opc" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I select town section "one" from the TownSection dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "Some Project" into the ProjectName field
	And I enter "N" into the StreetPrefix field
	And I enter "10th" into the Street field
	And I enter "St" into the StreetSuffix field
	And I enter "S" into the CrossStreetPrefix field
	And I enter "9th" into the CrossStreet field
	And I enter "Ave" into the CrossStreetSuffix field
	And I enter "42" into the MapPage field
	And I enter "4/24/1984" into the DateInstalled field
	And I enter "4/24/1984" into the PhysicalInService field
	And I enter "12345" into the TaskNumber field
	And I enter "Stuff and stuff" into the Comments field
	And I upload "itsatiff.tiff"
	# TC keeps choking on this every other time it runsfor some reason, 
	# testing to see if it's just a race condition with file uploads 
	#And I wait 5 seconds 
	And I press Save 
	Then the currently shown as built image shall henceforth be known throughout the land as "zoink"
	And I should delete as built image "zoink"'s file to clean up after this test
	And I should see a display for Town_County_State with state "one"'s Abbreviation
	And I should see a display for Town_County with county "one"
	And I should see a display for Town with town "one"
	And I should see a display for TownSection with town section "one"
	And I should see a display for ProjectName with "Some Project"
	And I should see a display for OperatingCenter with operating center "opc"
	And I should see a display for FullStreet with "N 10th St"
	And I should see a display for FullCrossStreet with "S 9th Ave"
	And I should see a display for MapPage with "42"
	And I should see a display for TaskNumber with "12345"
	And I should see a display for DateInstalled with "4/24/1984"
	And I should see a display for PhysicalInService with "4/24/1984"
	And I should see a display for Comments with "Stuff and stuff"
	And I should see a display for FileName with "itsatiff.tiff"


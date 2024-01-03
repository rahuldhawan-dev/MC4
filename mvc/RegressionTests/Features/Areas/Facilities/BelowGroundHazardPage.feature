Feature: BelowGroundHazard

Background: users and supporting data exist
	Given a user "user" exists with username: "user"
	And a state "nj" exists
	And an operating center "nj7" exists with opcode: "nj7", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "nj"
	And an admin user "admin" exists with username: "admin"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "roleAdd" exists with action: "Add", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "roleDelete" exists with action: "Delete", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "workorder-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a coordinate "one" exists
	And a town "one" exists with state: "nj"
	And a town section "one" exists with town: "one"
	And operating center: "nj7" exists in town: "one"
	And a town section "active" exists with town: "one"
	And a town section "inactive" exists with town: "one", name: "Inactive Section", active: false
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a street "one" exists with town: "one", is active: true
	And a hazard type "one" exists with description: "Rumbling from below"
	And an asset status "one" exists with description: "ACTIVE"
	And a hazard approach recommended type "one" exists with description: "Dig Dig Dig"
	And a work order "one" exists with operating center: "nj7"

Scenario: User can search and view below ground hazards
	Given an below ground hazard "one" exists with coordinate: "one", operating center: "nj7", work order: "one", town: "one", town section: "one", street: "one", hazard type: "one", hazard area: "100", asset status: "one"
	And I am logged in as "user"
	When I visit the Facilities/BelowGroundHazard/Search page
	And I press Search
	And I wait for the page to reload
	And I follow the Show link for below ground hazard "one"
	Then I should be at the Show page for below ground hazard "one"

#Scenario: User can delete a below ground hazard
#	Given an below ground hazard "one" exists with coordinate: "one", operating center: "nj7", work order: "one", town: "one", town section: "one", street: "one", hazard area: "100", hazard type: "one", asset status: "one"
#	And I am logged in as "user"
#	When I visit the Show page for below ground hazard "one"
#	And I click ok in the dialog after pressing "Delete"
#	Then I should be at the Facilities/BelowGroundHazard/Search page
#	When I try to access the Show page for below ground hazard: "one" expecting an error
#	Then I should see a 404 error message

Scenario: User can create a below ground hazard and gets the proper validation
	Given I am logged in as "user"
	And a coordinate "two" exists
	And a role "roleFacilityReadnj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	When I visit the Facilities/BelowGroundHazard/New page
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	Then I should see a validation message for HazardArea with "The Hazard Area (Feet) field is required."
	Then I should see a validation message for HazardType with "The HazardType field is required."
	Then I should see a validation message for AssetStatus with "The Status field is required."
	Then I should see a validation message for HazardDescription with "The HazardDescription field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "1000" into the HazardArea field 
	And I enter "1000" into the DepthOfHazard field
	And I enter "1000" into the ProximityToAmWaterAsset field
	And I enter "100" into the StreetNumber field
	And I enter work order "one"'s Id into the WorkOrder field
	And I select hazard type "one"'s Description from the HazardType dropdown
	And I select asset status "one"'s Description from the AssetStatus dropdown
	And I enter "stagnant magma under the tank, seismic tremors, fractures in the ground, gas pockets squeezing out" into the HazardDescription field
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."
	And I should see a validation message for HazardArea with "The field Hazard Area (Feet) must be between 1 and 500."
	And I should see a validation message for DepthOfHazard with "The field Depth Of Hazard (Inches) must be between 0 and 144."
	When I select town "one" from the Town dropdown
	And I select street "one" from the Street dropdown
	And I enter coordinate "two"'s Id into the Coordinate field
	And I enter "100" into the HazardArea field 
	And I enter "100" into the DepthOfHazard field
	And I press Save
	And I wait for the page to reload
	Then the currently shown below ground hazard shall henceforth be known throughout the land as "meatloaf"
	And I should be at the Show page for below ground hazard "meatloaf"
	And I should see a display for OperatingCenter with operating center "nj7" 
	And I should see a display for Town with town "one"
	And I should see a display for StreetNumber with "100"
	And I should see a display for Street with street "one"
	And I should see a display for HazardArea with "100"
	And I should see a display for HazardType with "Rumbling from below"
	And I should see a display for DepthOfHazard with "100"
	And I should see a display for AssetStatus with asset status "one"
	And I should see a display for HazardDescription with "stagnant magma under the tank, seismic tremors, fractures in the ground, gas pockets squeezing out"
	And I should see a display for ProximityToAmWaterAsset with "1000"

Scenario: User can edit a below ground hazard
	Given an below ground hazard "one" exists with coordinate: "one", operating center: "nj7", work order: "one", town: "one", town section: "one", street: "one", hazard area: "100", hazard type: "one", asset status: "one"
	And I am logged in as "user"
	When I visit the Show page for below ground hazard "one"
	And I follow "Edit"
	Then I should be at the Edit page for below ground hazard "one"
	When I enter "100" into the HazardArea field 
	And I enter "100" into the DepthOfHazard field
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for below ground hazard "one"
	And I should see a display for OperatingCenter with operating center "nj7" 
	And I should see a display for Town with town "one"
	And I should see a display for DepthOfHazard with "100"
	And I should see a display for Street with street "one"
	And I should see a display for HazardArea with "100" 
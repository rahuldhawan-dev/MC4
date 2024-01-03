Feature: EasementPage

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
	And a coordinate "one" exists with latitude: "-50", longitude: "-100"
	And a town "one" exists with state: "nj"
	And a town section "one" exists with town: "one"
	And operating center: "nj7" exists in town: "one"
	And a town section "active" exists with town: "one"
	And a town section "inactive" exists with town: "one", name: "Inactive Section", active: false
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a street "one" exists with town: "one", is active: true
	And a easement status "one" exists with description: "Status One"
	And a easement category "one" exists with description: "Category One"
	And a easement type "one" exists with description: "Type One"
	And a grantor type "one" exists with description: "Grantor Type One"
	And an easement "one" exists with state: "nj", operating center: "nj7", town: "one", town section: "one", street number: "1234", street: "one", cross street: "one", coordinate: "one", RecordNumber: "5678", Wbs: "4567", EasementDescription: "Testing Easement", DateRecorded: "05/15/2022", DeedBook: "Testing Deed Book", DeedPage: "Testing Deed Page", BlockLot: "", OwnerName: "Smith", OwnerAddress: "lkjhgfs", OwnerPhone: "9848032919"

Scenario: User can search and view easements	
	Given I am logged in as "user"
	When I visit the Facilities/Easement/Search page
	And I press Search
	And I wait for the page to reload
	And I follow the Show link for easement "one"
	Then I should be at the Show page for easement "one"

Scenario: User can create an easement and gets the proper validation
	Given I am logged in as "user"
	When I visit the Facilities/Easement/New page
	And I press Save
	Then I should see a validation message for State with "The State field is required."
	When I select state "nj" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."
	Then I should see a validation message for Coordinate with "The Coordinate field is required."
	Then I should see a validation message for RecordNumber with "The RecordNumber field is required."
	Then I should see a validation message for Status with "The Status field is required."
	Then I should see a validation message for Category with "The Asset Category field is required."
	Then I should see a validation message for EasementDescription with "The Description field is required."
	Then I should see a validation message for Type with "The Easement Type field is required."
	Then I should see a validation message for DateRecorded with "The Recorded Date field is required."
	Then I should see a validation message for GrantorType with "The GrantorType field is required."
	When I select town "one" from the Town dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "1234" into the RecordNumber field 
	And I enter "testing" into the EasementDescription field
	And I enter today's date into the DateRecorded field
	And I select easement status "one" from the Status dropdown
	And I select easement category "one" from the Category dropdown
	And I select easement type "one" from the Type dropdown
	And I select grantor type "one" from the GrantorType dropdown
    And I press Save
	And I wait for the page to reload
	Then the currently shown easement shall henceforth be known throughout the land as "two"
	And I should be at the Show page for easement "two"
	And I should see a display for State with state "nj"
	And I should see a display for OperatingCenter with operating center "nj7" 
	And I should see a display for Town with town "one"
	And I should see a display for Coordinate_Latitude with "-50"
	And I should see a display for Coordinate_Longitude with "-100"
	And I should see a display for RecordNumber with "1234"
	And I should see a display for EasementDescription with "testing"
	And I should see a display for DateRecorded with today's date
	And I should see a display for Status with easement status "one"
	And I should see a display for Category with easement category "one"
	And I should see a display for Type with easement type "one"
	And I should see a display for GrantorType with grantor type "one"

Scenario: User can edit an easement
	Given I am logged in as "user"
	When I visit the Show page for easement "one"
	And I follow "Edit"
	Then I should be at the Edit page for easement "one"
	When I select easement status "one" from the Status dropdown
	And I select easement category "one" from the Category dropdown
	And I select easement type "one" from the Type dropdown
	And I select grantor type "one" from the GrantorType dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for easement "one"
	And I should see a display for Status with easement status "one"
	And I should see a display for Category with easement category "one"
	And I should see a display for Type with easement type "one"
	And I should see a display for GrantorType with grantor type "one"
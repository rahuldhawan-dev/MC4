Feature: FunctionalLocationPage
	In order to manage functional locations
	As a user
	I want to be able to manage functional locations through the web site

Background: users and supporting data exists
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesDataLookups", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesDataLookups", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "FieldServicesDataLookups", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "FieldServicesDataLookups", user: "user"
	And an operating center "one" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "one" exists in town: "one"
	#And an valve asset type "valve" exists with description: "Valve"
	
#user can view
Scenario: user can view a functional location
	Given a functional location "one" exists with description: "NJAC-AB-VALVE-0001", town: "one"
	And I am logged in as "user"
	When I visit the Show page for functional location: "one"
	Then I should see a display for functional location: "one"'s Description
	Then I should see a display for "Town_ShortName" with town: "one"'s ShortName

#user can add/edit/destroy
Scenario: user can add edit and destroy a functional location
	Given I am logged in as "user"
	When I visit the FieldOperations/FunctionalLocation/New page
	And I press Save
	Then I should see the validation message "The Description field is required."
	When I enter "NJAC-AB-VALVE-0001" into the Description field
	And I select operating center "one" from the OperatingCenter dropdown
	And I wait for ajax to finish loading
	And I select town "one"'s ShortName from the Town dropdown
	And I select "Yes" from the IsActive dropdown
	And I press Save
	Then the currently shown functional location shall henceforth be known throughout the land as "one"
	And I should see a display for functional location: "one"'s Description
	And I should see a display for "Town_ShortName" with town: "one"
	When I follow "Edit"
	And I enter "" into the Description field
	And I press Save
	Then I should see the validation message "The Description field is required."
	When I enter "NJAC-AB-VALVE-0002" into the Description field
	And I press Save
	Then I should see a display for Description with "NJAC-AB-VALVE-0002"
	When I click ok in the dialog after pressing "Delete"
	Then I should be at the FieldOperations/FunctionalLocation/Search page
	When I try to access the Show page for functional location: "one" expecting an error
	Then I should see a 404 error message
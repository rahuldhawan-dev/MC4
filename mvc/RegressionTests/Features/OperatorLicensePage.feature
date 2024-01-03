Feature: OperatorLicensePage
	
Background: AllYourSetupBelongsToMe
	Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a state "two" exists with name: "Philadelphia", abbreviation: "PA"
	And an operating center "nj7" exists with opcode: "nj7", state: "one"
	And an operating center "nj4" exists with opcode: "nj4", state: "one"
	And an employee status "active" exists with description: "Active"
	And an employee status "inactive" exists with description: "Inactive"
	And an employee "D:" exists with operating center: "nj7", status: "active", employee id: "123"
	And an employee ":(" exists with operating center: "nj4", status: "active", employee id: "987"	
	And an employee "I:" exists with operating center: "nj4", status: "inactive", employee id: "456"
	And a public water supply "one" exists with Identifier: "123", system: "System"
	And a waste water system "one" exists with waste water system name: "Water System 1"
	And a operator license type "one" exists with description: "The best license type"
	And a operator license type "two" exists with description: "The worst license type"
	And a operator license "one" exists with employee: "D:", operator license type: "one", "operating center: "nj7", expiration date: "yesterday", validation date: "today"
	And a operator license "two" exists with employee: ":(", operator license type: "two", "operating center: "nj4", expiration date: "yesterday", validation date: "yesterday", state: "one", license sub level: "15"
	And a operator license "three" exists with employee: ":(", operator license type: "two", licensed operator of record: "true", "operating center: "nj4", expiration date: "yesterday", validation date: "yesterday", state: "one"
	And a operator license "four" exists with employee: "I:", operator license type: "one", "operating center: "nj7", state: "two", expiration date: "yesterday", validation date: "today"

Scenario: Operator License can be created
	Given I am logged in as "admin"
	When I visit the OperatorLicense/New page
	And I press Save
	Then I should be at the OperatorLicense/New page
	And I should see the validation message "The OperatorLicenseType field is required."
	And I should see the validation message "The State field is required."
	And I should see the validation message "The License Level/Class field is required."
	And I should see the validation message "The LicenseNumber field is required."
	And I should see the validation message "The ValidationDate field is required."
	And I should see the validation message "The ExpirationDate field is required."
	And I should see the validation message "The Employee Status field is required."
	When I select state "one" from the State dropdown
	And I press "Save"
	Then I should see the validation message "The OperatingCenter field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should be at the OperatorLicense/New page
	And I should see the validation message "The Employee Status field is required."
	When I select "Active" from the Status dropdown
	And I press Save
	Then I should see the validation message "The Employee field is required."	
	When I select employee "D:"'s Description from the Employee dropdown
	And I select operator license type "one" from the OperatorLicenseType dropdown
	And I check the LicensedOperatorOfRecord field
	And I select state "one" from the State dropdown
	And I enter "123" into the LicenseLevel field
	And I enter "1, 3, 4" into the LicenseSubLevel field
	And I enter "123123" into the LicenseNumber field
	And I enter today's date into the ValidationDate field
	And I enter today's date into the ExpirationDate field
	And I press Save
	Then the currently shown operator license will now be referred to as "new"
	And I should be at the Show page for operator license "new"
	And I should see a display for Employee_OperatingCenter with operating center "nj7"
	And I should see a display for State with state "one"
	And I should see a display for OperatorLicenseType with operator license type "one"
	And I should see a display for LicensedOperatorOfRecord with "Yes"
	And I should see a display for Employee with employee "D:"
	And I should see a display for LicenseLevel with "123"
	And I should see a display for LicenseSubLevel with "1, 3, 4"
	And I should see a display for LicenseNumber with "123123"
	And I should see a display for Employee_EmployeeId with "123"
	And I should see a display for ValidationDate with today's date
	And I should see a display for ExpirationDate with today's date
	
Scenario: Operator License can be searched
	Given I am logged in as "admin"
	When I search for OperatorLicenses with no conditions chosen
	Then I should be at the OperatorLicense page
	And I should see a link to the show page for operator license "one"
	And I should see a link to the show page for operator license "two"
	When I visit the /OperatorLicense/Search page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press "Search"
	Then I should be at the OperatorLicense page
	And I should see a link to the show page for operator license "one"
	And I should not see a link to the show page for operator license "two"
	When I search for OperatorLicenses with operator license type: "two" chosen
	Then I should be at the OperatorLicense page
	And I should not see a link to the show page for operator license "one"
	And I should see a link to the show page for operator license "two"
	When I visit the /OperatorLicense/Search page
	And I check the LicensedOperatorOfRecord field
	And I press "Search"
	Then I should be at the OperatorLicense page
	#you'll see "Active" because Employee Status column was added
	Then I should see the following values in the operatingLicenseTable table
	| Employee Status   |
	| Active			|
	And I should see a link to the show page for operator license "three"
	When I visit the /OperatorLicense/Search page
	And I select "Inactive" from the Status dropdown
	And I press "Search"
	Then I should be at the OperatorLicense page
	#you'll see "Active" because Employee Status column was added
	Then I should see the following values in the operatingLicenseTable table
	| Employee Status   |
	| Inactive			|
	And I should see a link to the show page for operator license "four"
	
Scenario: Operator License show page displays employee's operating center	
	Given I am logged in as "admin"
	And I am at the Show page for operator license: "four"
	When I click the "Details" tab
	Then I should see a display for Employee_OperatingCenter with employee "I:"'s OperatingCenter 

Scenario: Operator License search find operators by employee's operating center	and state
	Given I am logged in as "admin"
	When I visit the /OperatorLicense/Search page
	And I select state "one" from the State dropdown
	And I select employee "I:"'s OperatingCenter from the OperatingCenter dropdown
	And I press "Search"
	Then I should see the following values in the operatingLicenseTable table
	  | Operating Center | Employee                  |
	  | nj4 -            | employee: "I:"'s FullName | 

Scenario: Operator License can be edited
	Given I am logged in as "admin"
	And I am at the Show page for operator license: "two"
	When I follow "Edit"
	And I check the LicensedOperatorOfRecord field
	And I enter today's date into the ValidationDate field
	And I enter today's date into the ExpirationDate field 
	And I press Save
	Then I should be at the Show page for operator license: "two"
	And I should see a display for LicensedOperatorOfRecord with "Yes"
	And I should see a display for ValidationDate with today's date
	And I should see a display for ExpirationDate with today's date
	And I should see a display for Employee_EmployeeId with "987"
	And I should see a display for LicenseSubLevel with "15"

Scenario: user should only be able to search within operating centers they have access to and not view edit others
	Given a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesEmployeeLimited", operating center: "nj7"
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And I am logged in as "read-only-nj7"
	When I visit the /OperatorLicense/Search page
	And I select state "one" from the State dropdown
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	And I should not see operating center "nj4" in the OperatingCenter dropdown
	When I visit the Show page for operator license: "one"
	Then I should see a display for Employee_OperatingCenter with operating center "nj7"
	When I try to visit the Show page for operator license: "two" expecting an error
	Then I should see a 404 error message
	When I try to visit the Edit page for operator license: "two" expecting an error
	Then I should see "You do not have the roles necessary to access this resource."
	When I try to visit the /Employee/Search page expecting an error
	Then I should see "You do not have the roles necessary to access this resource."

Scenario: user can add/remove PWSID's from OperatorLicense record
	Given I am logged in as "admin"
	And I am at the Show page for operator license: "two"
	When I click the "PWSID" tab 
	And I press "Add New Public Water Supply"
	And I select public water supply "one" from the PublicWaterSupply dropdown 
	And I press "Add Public Water Supply"
	Then I should be at the Show page for operator license: "two"
	When I click the "PWSID" tab 
	Then I should see public water supply "one"'s Description in the table publicWaterSupplyTable's "Public Water Supply" column
	When I click ok in the alert after pressing id
	And I wait for the page to reload
	Then I should be at the Show page for operator license: "two"
	When I click the "PWSID" tab 
	Then I should not see public water supply "one"'s Description in the table publicWaterSupplyTable's "Public Water Supply" column

Scenario: user can add/remove WWSIDs to/from OperatorLicense record
	Given I am logged in as "admin"
	And I am at the Show page for operator license: "two"
	When I click the "WWSID" tab 
	And I press "Add New Waste Water System"
	And I select waste water system "one" from the WasteWaterSystem dropdown 
	And I press "Add Waste Water System"
	Then I should be at the Show page for operator license: "two"
	When I click the "WWSID" tab 
	Then I should see waste water system "one"'s Description in the table wasteWaterSystemTable's "Waste Water System" column
	When I click ok in the dialog after pressing "Remove Waste Water System"
	And I wait for the page to reload
	Then I should be at the Show page for operator license: "two"
	When I click the "WWSID" tab 
	Then I should not see waste water system "one"'s Description in the table wasteWaterSystemTable's "Waste Water System" column
	
Scenario: user can access all the logs for a specific operator License from log tab under OperatorLicense record
    Given the test flag "allow audits" exists
	And I am logged in as "admin"
	And I am at the Show page for operator license: "two"
	When I click the "Log" tab
	Then I should see "Records found" 
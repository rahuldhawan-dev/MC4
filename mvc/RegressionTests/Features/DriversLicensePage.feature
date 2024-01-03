Feature: Drivers License Page

Background: stuff (and junk)
	Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "nj7"
	And an operating center "nj4" exists with opcode: "nj4"
	And an employee status "active" exists with description: "Active"
	And an employee "D:" exists with operating center: "nj7", status: "active"
	And an employee "vOv" exists with operating center: "nj4", status: "active"
	And a drivers license class "one" exists
	And a drivers license class "two" exists
	And a drivers license "one" exists with employee: "D:", drivers license class: "one", renewal date: "tomorrow", issued date: "yesterday"
	And a drivers license "two" exists with employee: "vOv", drivers license class: "two", renewal date: "yesterday", issued date: "yesterday"
	And a state "one" exists

Scenario: there's a search page, and it's usable
	Given I am logged in as "admin"
	When I search for DriversLicenses with no conditions chosen
	Then I should be at the DriversLicense page
	And I should see a link to the Show page for drivers license "one"
	And I should see a link to the Show page for drivers license "two"
	When I search for DriversLicenses with operating center: "nj7" chosen
	Then I should be at the DriversLicense page
	And I should see a link to the Show page for drivers license "one"
	And I should not see a link to the Show page for drivers license "two"
	When I visit the DriversLicense/Search page
	And I select drivers license class "two" from the DriversLicenseClass dropdown
	And I press Search
	Then I should be at the DriversLicense page
	And I should not see a link to the Show page for drivers license "one"
	And I should see a link to the Show page for drivers license "two"

Scenario: drivers licenses can be created
	Given an employee "|>" exists with operating center: "nj7", status: "active"
	And I am logged in as "admin"
	When I visit the DriversLicense/New page
	And I press Save
	Then I should be at the DriversLicense/New page
	And I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The DriversLicenseClass field is required."
	And I should see the validation message "The State field is required."
	And I should see the validation message "The LicenseNumber field is required."
	And I should see the validation message "The IssuedDate field is required."
	And I should see the validation message "The RenewalDate field is required."
	# TODO: endorsements and restrictions	
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should be at the DriversLicense/New page
	And I should see the validation message "The Employee field is required."
	# TODO: endorsements and restrictions	
	When I select employee "|>"'s Description from the Employee dropdown
	And I select drivers license class "one" from the DriversLicenseClass dropdown
	And I select state "one" from the State dropdown
	And I enter "12345 12345 12345" into the LicenseNumber field
	And I enter today's date into the IssuedDate field
	And I enter today's date into the RenewalDate field
	And I press Save
	Then the currently shown drivers license will now be referred to as "new"
	And I should be at the Show page for drivers license: "new"
	And I should see a display for Employee_OperatingCenter with operating center "nj7"
	And I should see a display for State with state "one"
	And I should see a display for DriversLicenseClass with drivers license class "one"
	And I should see a display for Employee with employee "|>"
	And I should see a display for IssuedDate with today's date
	And I should see a display for RenewalDate with today's date
	And I should see a display for LicenseNumber with "12345 12345 12345"

Scenario: drivers licenses can be edited
	Given I am logged in as "admin"
	And I am at the Show page for drivers license: "two"
	When I follow "Edit"
	And I enter today's date into the RenewalDate field
	And I enter today's date into the IssuedDate field
	And I press Save
	Then I should be at the Show page for drivers license: "two"
	And I should see a display for RenewalDate with today's date
	And I should see a display for IssuedDate with today's date

Scenario: user should only be able to search within operating centers they have access to and not view edit others
	Given a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesEmployeeLimited", operating center: "nj7"
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And I am logged in as "read-only-nj7"
	When I visit the /DriversLicense/Search page
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	And I should not see operating center "nj4" in the OperatingCenter dropdown
	When I visit the Show page for drivers license: "one"
	Then I should see a display for Employee_OperatingCenter with operating center "nj7"
	When I try to visit the Show page for drivers license: "two" expecting an error
	Then I should see a 404 error message
	When I try to visit the Edit page for drivers license: "two" expecting an error
	Then I should see "You do not have the roles necessary to access this resource."
	When I try to visit the /Employee/Search page expecting an error
	Then I should see "You do not have the roles necessary to access this resource."
	
Scenario: user can renew a license and everything except the date fields are populated with existing data
	Given I am logged in as "admin"
	And I am at the Show page for drivers license: "two"
	When I follow "Renew"
	Then I should be at the Renew page for drivers license: "two"
	And operating center "nj4"'s ToString should be selected in the OperatingCenter dropdown
    And employee "vOv"'s Description should be selected in the Employee dropdown
	And drivers license class "two"'s ToString should be selected in the DriversLicenseClass dropdown
	And I should see drivers license "two"'s LicenseNumber in the LicenseNumber field
	And I should see "" in the IssuedDate field
	And I should see "" in the RenewalDate field 
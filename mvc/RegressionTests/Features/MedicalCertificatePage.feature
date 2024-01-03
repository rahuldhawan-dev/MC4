Feature: MedicalCertificatePage
	
Background: data exists to perform these tests
	Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "nj7"
	And an operating center "nj4" exists with opcode: "nj4"
	And an employee status "active" exists with description: "Active"
	And an employee "x" exists with operating center: "nj7", status: "active"
	And an employee "y" exists with operating center: "nj4", status: "active"
	And a medical certificate "one" exists with employee: "x", certification date: "yesterday", expiration date: "yesterday"
	And a medical certificate "two" exists with employee: "y"

Scenario: there is a search page and it may be used
	Given I am logged in as "admin"
	When I search for MedicalCertificates with no conditions chosen
	Then I should be at the MedicalCertificate page
	And I should see a link to the Show page for medical certificate "one"
	And I should see a link to the Show page for medical certificate "two"
	When I search for MedicalCertificates with operating center: "nj7" chosen
	Then I should be at the MedicalCertificate page
	And I should see a link to the Show page for medical certificate "one"
	And I should not see a link to the Show page for medical certificate "two"

Scenario: medical certificates can be created
	Given an employee "z" exists with operating center: "nj7", status: "active"
	And I am logged in as "admin"
	When I visit the MedicalCertificate/New page
	And I press Save
	Then I should be at the MedicalCertificate/New page
	And I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The CertificationDate field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should see the validation message "The Employee field is required."
	When I select employee "z"'s Description from the Employee dropdown
	And I enter today's date into the CertificationDate field
	And I enter today's date into the ExpirationDate field
	And I press Save
	Then the currently shown medical certificate shall henceforth be known throughout the land as "Simon"
	And I should be at the show page for medical certificate: "Simon"
	And I should see a display for Employee_OperatingCenter with operating center "nj7"
	And I should see a display for Employee with employee "z"
	And I should see a display for CertificationDate with today's date
	And I should see a display for ExpirationDate with today's date

Scenario: medical certificates can be edited
	Given I am logged in as "admin"
	And I am at the Show page for medical certificate: "one"
	When I follow "Edit"
	And I enter today's date into the CertificationDate field
	And I enter today's date into the ExpirationDate field
	And I press Save
	Then I should be at the Show page for medical certificate: "one"
	And I should see a display for CertificationDate with today's date
	And I should see a display for ExpirationDate with today's date

Scenario: user should only be able to search within operating centers they have access to and not view edit others
	Given a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesEmployeeLimited", operating center: "nj7"
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And I am logged in as "read-only-nj7"
	When I visit the /MedicalCertificate/Search page
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	And I should not see operating center "nj4" in the OperatingCenter dropdown
	When I visit the Show page for medical certificate: "one"
	Then I should see a display for Employee_OperatingCenter with operating center "nj7"
	When I try to visit the Show page for medical certificate: "two" expecting an error
	Then I should see a 404 error message
	When I try to visit the Edit page for medical certificate: "two" expecting an error
	Then I should see "You do not have the roles necessary to access this resource."
	When I try to visit the /Employee/Search page expecting an error
	Then I should see "You do not have the roles necessary to access this resource."

Scenario: user can renew a medical certificate 
	Given I am logged in as "admin"
	And I am at the Show page for medical certificate: "one"
	When I follow "Renew"
	Then I should be at the Renew page for medical certificate: "one"
	And operating center "nj7"'s ToString should be selected in the OperatingCenter dropdown
 	And employee "x"'s Description should be selected in the Employee dropdown
	And I should see "" in the Comments field
	And I should see "" in the CertificationDate field
	And I should see "" in the ExpirationDate field 
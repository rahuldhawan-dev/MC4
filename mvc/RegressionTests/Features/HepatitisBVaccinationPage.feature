Feature: HepatitisBVaccinationPage

Background: data exists to perform these tests
	Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "nj7"
	And an operating center "nj4" exists with opcode: "nj4"
	And an employee status "active" exists with description: "Active"
	And an employee "x" exists with operating center: "nj7", status: "active"
	And an employee "y" exists with operating center: "nj4", status: "active"
	And a hepatitis b vaccine status "one" exists with description: "offered"
	And a hepatitis b vaccination "one" exists with employee: "x", response date: "yesterday", hepatitis b vaccine status: "one"
	And a hepatitis b vaccination "two" exists with employee: "y", hepatitis b vaccine status: "one"

Scenario: there is a search page and it may be used
	Given I am logged in as "admin"
	When I search for HepatitisBVaccinations with no conditions chosen
	Then I should be at the HepatitisBVaccination page
	And I should see a link to the Show page for hepatitis b vaccination "one"
	And I should see a link to the Show page for hepatitis b vaccination "two"
	When I search for HepatitisBVaccinations with operating center: "nj7" chosen
	Then I should be at the HepatitisBVaccination page
	And I should see a link to the Show page for hepatitis b vaccination "one"
	And I should not see a link to the Show page for hepatitis b vaccination "two"

Scenario: hepatitis b vaccinations can be created
	Given an employee "z" exists with operating center: "nj7", status: "active"
	And I am logged in as "admin"
	When I visit the HepatitisBVaccination/New page
	And I press Save
	Then I should be at the HepatitisBVaccination/New page
	And I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The ResponseDate field is required."
	And I should see the validation message "The HepatitisBVaccineStatus field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should see the validation message "The Employee field is required."
	When I select employee "z"'s Description from the Employee dropdown
	And I enter today's date into the ResponseDate field
	And I select hepatitis b vaccine status "one" from the HepatitisBVaccineStatus dropdown
	And I press Save
	Then the currently shown hepatitis b vaccination shall henceforth be known throughout the land as "Simon"
	And I should be at the show page for hepatitis b vaccination: "Simon"
	And I should see a display for Employee_OperatingCenter with operating center "nj7"
	And I should see a display for Employee with employee "z"
	And I should see a display for ResponseDate with today's date

Scenario: hepatitis b vaccinations can be edited
	Given I am logged in as "admin"
	And I am at the Show page for hepatitis b vaccination: "one"
	When I follow "Edit"
	And I enter today's date into the ResponseDate field
	And I press Save
	Then I should be at the Show page for hepatitis b vaccination: "one"
	And I should see a display for ResponseDate with today's date

Scenario: user without the role cannot access the hepatitis b vaccination page
	Given a user "invalid" exists with username: "invalid"
	And I am logged in as "invalid"
	When I visit the /HepatitisBVaccination/Search page
	Then I should see the missing role error

Scenario: user should only be able to search within operating centers they have access to and not view edit others
	Given a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesEmployeeLimited", operating center: "nj7"
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And I am logged in as "read-only-nj7"
	When I visit the /HepatitisBVaccination/Search page
	Then I should see operating center "nj7" in the OperatingCenter dropdown
	And I should not see operating center "nj4" in the OperatingCenter dropdown
	When I visit the Show page for hepatitis b vaccination: "one"
	Then I should see a display for Employee_OperatingCenter with operating center "nj7"
	When I try to visit the Show page for hepatitis b vaccination: "two" expecting an error
	Then I should see a 404 error message
	When I try to visit the Edit page for hepatitis b vaccination: "two" expecting an error
	Then I should see "You do not have the roles necessary to access this resource."
	When I try to visit the /Employee/Search page expecting an error
	Then I should see "You do not have the roles necessary to access this resource."
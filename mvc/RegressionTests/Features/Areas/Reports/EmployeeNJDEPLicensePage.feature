Feature: EmployeNJDEPLicensePage

Background: 
	Given a state "nj" exists
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "nj"
    And a user "user" exists with username: "user"
	And an employee status "active" exists with description: "Active"
	And an employee "D:" exists with operating center: "nj7", status: "active", license water treatment: "w", license water distribution: "t", license sewer collection: "c", license sewer treatment: "s", license industrial discharge: "n", first name: "Bill", last name: "S. Preston Esq."
	And a role "workorder-read" exists with action: "Read", module: "HumanResourcesEmployee", user: "user", operating center: "nj7"
		
Scenario: User can search for work orders
	Given I am logged in as "user"
	And I am at the Reports/EmployeeNJDEPLicense/Search page
	When I select state "nj" from the State dropdown
	And I press Search
	Then I should see the following values in the employeeNJDEPLicenseTable table
	| Operating Center | Name                 | License Water Treatment | License Water Distribution | License Sewer Collection | License Sewer Treatment | License Industrial Discharge |
	| *NJ7*            | Bill S. Preston Esq. | w                       | t                          | c                        | s                       | n                            |
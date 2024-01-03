Feature: Grievance Page

Background: here's some stuff ok
    Given an operating center "nj7" exists with opcode: "NJ7"
    And an operating center "nj4" exists with opcode: "NJ4"

	And a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesUnion", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "HumanResourcesUnion", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "HumanResourcesUnion", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "HumanResourcesUnion", operating center: "nj7"

	And a role "roleReadNj4" exists with action: "Read", module: "HumanResourcesUnion", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "HumanResourcesUnion", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "HumanResourcesUnion", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "HumanResourcesUnion", operating center: "nj4"

    And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
	And a user "invalid" exists with username: "invalid"
    And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
    And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
    And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a union "union1" exists with bargaining unit: The International Brotherhood of Boilermakers%2c Iron Ship Builders%2c Blacksmiths%2c Forgers and Helpers
    And a union "union2" exists with bargaining unit: International Pirates%2c Haberdashers%2c Flaneurs%2c Printer’s Devils%2c Farriers%2c Alchemists%2c Coopers%2c Canal Puddlers%2c Fishmongers%2c Cellarmen%2c Wicket Goblins and Part-time Popinjays Union
	And a local "local1" exists with union: "union1", name: "123", description: "Monmouth County", operating center: "nj7"
	And a local "local2" exists with union: "union2", name: "123", description: "Ocean County", operating center: "nj4"
	And a grievance category "one" exists with description: things
	And a grievance categorization "gc1" exists with description: this is a thing, grievance category: "one"
	And a grievance categorization "gc2" exists with description: this is also a thing, grievance category: "one"
	And a grievance categorization "gc3" exists with description: this is the third thing, grievance category: "one"
    And a grievance status "gs1" exists with description: this is a different sort of thing
    And a grievance status "gs2" exists with description: ceci n'est pas une thing
    And a grievance status "gs3" exists with description: are you thinging what i'm thinging?
    And a union contract "uc1" exists with local: "local1", operating center: "nj7"
    And a union contract "uc2" exists with local: "local2", operating center: "nj4"
    And a data type "data type" exists with table name: "UnionGrievances", name: "Grievance"
	And an employee status "active" exists with description: "Active"
	And an employee "ted" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "nj7", status: "active"
	And an employee "bill" exists with first name: "Bill", last name: "Preston", employee id: "11111117", operating center: "nj7", status: "active"
	
Scenario: a user without the role cannot access the grievance page
	Given I am logged in as "invalid"
	When I visit the /Grievance/Search page
    Then I should see the missing role error

Scenario: admin can search by categorization
    Given a grievance "grievance1" exists with categorization: "gc1", grievance category: "one"
    And a grievance "grievance2" exists with categorization: "gc2", grievance category: "one"
    And a grievance "grievance3" exists with categorization: "gc3", grievance category: "one"
    And I am logged in as "admin"
    When I visit the /Grievance/Search page
    And I select grievance category "one"'s Description from the GrievanceCategory dropdown
    And I select grievance categorization "gc1"'s Description from the Categorization dropdown
    And I select grievance categorization "gc3"'s Description from the Categorization dropdown
    And I press Search
    And I wait for the page to reload
	Then I should see grievance categorization "gc1"'s Description in the "Grievance SubCategory" column
	And I should see grievance categorization "gc3"'s Description in the "Grievance SubCategory" column
	And I should not see grievance categorization "gc2"'s Description in the "Grievance SubCategory" column

Scenario: admin can search by status
    Given a grievance "grievance1" exists with status: "gs1"
    And a grievance "grievance2" exists with status: "gs2"
    And a grievance "grievance3" exists with status: "gs3"
    And I am logged in as "admin"
    When I visit the /Grievance/Search page
    And I select grievance status "gs1"'s Description from the Status dropdown
    And I select grievance status "gs3"'s Description from the Status dropdown
    And I press Search
    And I wait for the page to reload
	Then I should see grievance status "gs1"'s Description in the "Status" column
	And I should see grievance status "gs3"'s Description in the "Status" column
	And I should not see grievance status "gs2"'s Description in the "Status" column

Scenario: admin can edit grievance
    Given a grievance "grievance1" exists with status: "gs1", categorization: "gc1", contract: "uc1", operating center: "nj7"
    And I am logged in as "admin"
    When I visit the Show page for grievance: "grievance1"
    And I follow "Edit"
    Then I should be at the Edit page for grievance: "grievance1"
    When I select operating center "nj4"'s Name from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select union contract "uc2"'s Description from the Contract dropdown
    And I select grievance category "one"'s Description from the GrievanceCategory dropdown
    And I select grievance categorization "gc2"'s Description from the Categorization dropdown
    And I select grievance status "gs2"'s Description from the Status dropdown
    And I enter "12345" into the Number field
    And I enter today's date into the DateReceived field	
    And I enter "1.234" into the EstimatedImpactValue field
    And I enter today's date into the IncidentDate field
    And I enter "Some bad things happened" into the Description field
    And I enter "But now everyone is ok with it" into the DescriptionOfOutcome field
	And I enter today's date into the UnionDueDate field
	And I enter today's date into the ManagementDueDate field
    And I press Save
    Then I should be at the Show page for grievance: "grievance1"

Scenario: user should only be able to search within operating centers they have access to
    Given I am logged in as "read-only-nj7"
    When I visit the /Grievance/Search page
    Then I should see operating center "nj7"'s Name in the OperatingCenter dropdown
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    Then I should see union contract "uc1"'s Description in the Contract dropdown
    And I should not see operating center "nj4"'s Name in the OperatingCenter dropdown
    And I should not see union contract "uc2"'s Description in the Contract dropdown
    Given I am logged in as "read-only-nj4"
    When I visit the /Grievance/Search page
    Then I should see operating center "nj4"'s Name in the OperatingCenter dropdown
    When I select operating center "nj4" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    Then I should see union contract "uc2"'s Description in the Contract dropdown
    And I should not see operating center "nj7"'s Name in the OperatingCenter dropdown
    And I should not see union contract "uc1"'s Description in the Contract dropdown
	Given I am logged in as "user-admin-both"
    When I visit the /Grievance/Search page
    Then I should see operating center "nj4"'s Name in the OperatingCenter dropdown
    And I should see operating center "nj7"'s Name in the OperatingCenter dropdown

Scenario: user with edit rights can edit grievance
    Given a grievance "grievance1" exists with status: "gs1", categorization: "gc1", contract: "uc1", operating center: "nj7", grievance category: "one"
    And I am logged in as "user-admin-nj7"
    When I visit the Show page for grievance: "grievance1"
    And I follow "Edit"
    Then I should be at the Edit page for grievance: "grievance1"
    And I should not see operating center "nj4"'s Name in the OperatingCenter dropdown
    And I should not see union contract "uc2"'s Description in the Contract dropdown
    When I select grievance category "one"'s Description from the GrievanceCategory dropdown
    And I select grievance categorization "gc2"'s Description from the Categorization dropdown
    And I select grievance status "gs2"'s Description from the Status dropdown
    And I enter "12345" into the Number field
    And I enter today's date into the DateReceived field	
    And I enter "1.234" into the EstimatedImpactValue field
    And I enter today's date into the IncidentDate field
    And I enter "Some bad things happened" into the Description field
    And I enter "But now everyone is ok with it" into the DescriptionOfOutcome field
	And I enter today's date into the UnionDueDate field
	And I enter today's date into the ManagementDueDate field
    And I press Save
    Then I should be at the Show page for grievance: "grievance1"

Scenario: user with nj7 edit and nj4 read rights cannot edit an nj4 grievance
	Given a grievance "grievance1" exists with status: "gs1", categorization: "gc1", contract: "uc1", operating center: "nj4"
	And a user "user-nj7-all-nj4-read" exists with username: "user-nj7-all-nj4-read", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7;roleReadNj4
	And I am logged in as "user-nj7-all-nj4-read"
	When I visit the Show page for grievance: "grievance1"
	Then I should not see a link to the Edit page for grievance: "grievance1"
	And I should not see the button "Delete"

Scenario: user with edit rights can add employees to and delete employees from grievances
    Given a grievance "grievance1" exists with status: "gs1", categorization: "gc1", contract: "uc1", operating center: "nj7"
    And I am logged in as "user-admin-nj7"
    When I visit the Show page for grievance: "grievance1"
    And I click the "Employees" tab
    And I press "Link Employee"
	And I check employee "bill"'s Description in the EmployeeIds checkbox list
    And I press "Link Employees"
    Then I should be at the Show page for grievance: "grievance1" on the Employees tab
    And I should see employee "bill"'s EmployeeId in the table employeesTable's "Employee ID" column
	When I click the "Employees" tab
    And I click the "Remove" button in the 1st row of employeesTable and then click ok in the confirmation dialog
    Then I should be at the Show page for grievance: "grievance1" on the Employees tab
    And I should not see employee "bill"'s EmployeeId in the table employeesTable's "Employee ID" column

Scenario: read-only user cannot add or remove employees
    Given a grievance "grievance1" exists with status: "gs1", categorization: "gc1", contract: "uc1", operating center: "nj7"
    And I am logged in as "read-only-nj7"	
    When I visit the Show page for grievance: "grievance1"
    And I click the "Employees" tab
    Then I should not see the button "Link Employee"
    And I should not see the button "Remove"

#Scenario: user cannot add employees from operating centers they don't have access to
#    Given a grievance "grievance1" exists with status: "gs1", categorization: "gc1", contract: "uc1", operating center: "nj7"
#    And an employee "bill" exists with operating center: "nj7"
#    And an employee "bob" exists with operating center: "nj4"
#    And I am logged in as "user-admin-nj7"
#    When I visit the Show page for grievance: "grievance1"
#    And I click the "Employees" tab
#    And I press "Link Employee"
#    Then I should see a checkbox named EmployeeIds with employee "bill"'s Id
#	And I should not see a checkbox named EmployeeIds with employee "bob"'s Id

Scenario: user with add rights can add a new grievance
    Given I am logged in as "user-admin-nj7"
    When I visit the /Grievance/New page
    And I enter today's date into the DateReceived field
	And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I enter "the" and select "Theodore Logan" from the LaborRelationsBusinessPartner autocomplete field
    And I press Save
	Then the currently shown grievance will now be referred to as "new grievance"
    And I should be at the Show page for grievance: "new grievance"   
    And I should see operating center "nj7"'s Name in the OperatingCenter dropdown
    And I should see a display for LaborRelationsBusinessPartner with "Theodore Logan"

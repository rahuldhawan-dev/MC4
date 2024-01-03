Feature: ContractorOverrideLaborCost
	In order to write text in italics
	As a developer
	I must type here

Background: is a dark color on my screen
    Given an operating center "nj7" exists with opcode: "NJ7", name: "Bob"
    And an operating center "nj4" exists with opcode: "NJ4", name: "David"
	And a role "roleReadNj7" exists with action: "Read", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleEditNj4" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj4"
    And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a contractor labor cost "one" exists with operating centers: nj4,nj7
	And a contractor "one" exists

Scenario: User sees validation when creating a record
	Given I am logged in as "user"
	And I am at the /ProjectManagement/ContractorOverrideLaborCost/New page
	When I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for Contractor with "The Contractor field is required."
	And I should see a validation message for EffectiveDate with "The EffectiveDate field is required."
	When I enter "f" into the Cost field
	And I press Save
	Then I should see a validation message for Cost with "The field Cost must be a number."
	When I enter "-1" into the Cost field
	And I press Save
	Then I should see a validation message for Cost with "Cost must be greater than or equal to zero."
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
	Then I should see a validation message for ContractorLaborCost with "The ContractorLaborCost field is required."

Scenario: User can create a new record
	Given I am logged in as "user"
	And I am at the /ProjectManagement/ContractorOverrideLaborCost/New page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select contractor "one" from the Contractor dropdown
    And I wait for ajax to finish loading
	And I select contractor labor cost "one" from the ContractorLaborCost dropdown
	And I enter "42" into the Cost field
	And I enter "4/24/1984" into the EffectiveDate field
	And I press Save
	Then I should see a display for "OperatingCenter" with operating center: "nj7"
	And I should see a display for "Contractor" with contractor: "one"
	And I should see a display for "ContractorLaborCost" with contractor labor cost: "one"
	And I should see a display for Cost with "$42.00"
	And I should see a display for EffectiveDate with "4/24/1984"

Scenario: user can edit a record
    Given a contractor labor cost "first" exists with operating centers: nj4,nj7
	And a contractor labor cost "second" exists with operating centers: nj4,nj7
	And a contractor "two" exists
	And a contractor override labor cost "first" exists with operating center: "nj7", contractor labor cost: "first", contractor: "one", cost: 5.55, effective date: "4/24/1984"
    And a user "nj4-user" exists with username: "nj4-user", roles: roleEditNj4;roleEditNj7
	And I am logged in as "nj4-user"
	And I am at the edit page for contractor override labor cost: "first"
	When I select operating center "nj4" from the OperatingCenter dropdown
	And I select contractor "two" from the Contractor dropdown
    And I wait for ajax to finish loading
	And I select contractor labor cost "one" from the ContractorLaborCost dropdown
	And I enter "42" into the Cost field
	And I enter "5/25/1985" into the EffectiveDate field
	And I press Save
	Then I should see a display for "OperatingCenter" with operating center: "nj4"
	And I should see a display for "Contractor" with contractor: "two"
	And I should see a display for "ContractorLaborCost" with contractor labor cost: "one"
	And I should see a display for Cost with "$42.00"
	And I should see a display for EffectiveDate with "5/25/1985"

Scenario: User can search
	Given a contractor labor cost "first" exists
	And a contractor labor cost "second" exists
	And a contractor "two" exists
	And a contractor override labor cost "first" exists with operating center: "nj7", contractor labor cost: "first", contractor: "one", cost: 5.55, effective date: "4/24/1984"
	And a contractor override labor cost "second" exists with operating center: "nj4", contractor labor cost: "second", contractor: "two", cost: 4.44, effective date: "5/25/1985"
	And I am logged in as "user"
	When I visit the /ProjectManagement/ContractorOverrideLaborCost/Search page
    And I press "Search"
    Then I should be at the ProjectManagement/ContractorOverrideLaborCost page
	Then I should see the following values in the results table
	| Operating Center        | Contractor        | Contractor Labor Cost           | Cost  | Effective Date |
	| operating center: "nj7" | contractor: "one" | contractor labor cost: "first"  | $5.55 | 4/24/1984      |
	| operating center: "nj4" | contractor: "two" | contractor labor cost: "second" | $4.44 | 5/25/1985      |

Scenario: User can view ze index
	Given a contractor labor cost "first" exists
	And a contractor labor cost "second" exists
	And a contractor "two" exists
	And a contractor override labor cost "first" exists with operating center: "nj7", contractor labor cost: "first", contractor: "one", cost: 5.55, effective date: "4/24/1984"
	And a contractor override labor cost "second" exists with operating center: "nj4", contractor labor cost: "second", contractor: "two", cost: 4.44, effective date: "5/25/1985"
	And I am logged in as "user"
	When I visit the /ProjectManagement/ContractorOverrideLaborCost/Index page
	Then I should see the following values in the results table
	| Operating Center        | Contractor        | Contractor Labor Cost           | Cost  | Effective Date |
	| operating center: "nj7" | contractor: "one" | contractor labor cost: "first"  | $5.55 | 4/24/1984      |
	| operating center: "nj4" | contractor: "two" | contractor labor cost: "second" | $4.44 | 5/25/1985      |
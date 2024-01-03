Feature: ContractorLaborCost

Background: 
    Given an operating center "nj7" exists with opcode: "NJ7", name: "Bob"
    And an operating center "nj4" exists with opcode: "NJ4", name: "David"
	# Additional DataLookups roles are needed because the ContractorLaborCostController inherits role requirements from a base class
	And a role "roleReadNj7Data" exists with action: "Read", module: "FieldServicesDataLookups", operating center: "nj7"
	And a role "roleReadNj7Edit" exists with action: "Edit", module: "FieldServicesDataLookups", operating center: "nj7"
	And a role "roleReadNj7" exists with action: "Read", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleEditNj4" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj4"
    And a user "user" exists with username: "user", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7;roleReadNj7Data;roleReadNj7Edit
    And a contractor labor cost "one" exists with operating centers: nj7
	And a contractor "one" exists

Scenario: User can add an operating center to a contractor labor cost
	Given I am logged in as "user"
	And I am at the show page for contractor labor cost "one"
	When I click the "Operating Centers" tab
	And I press "Add New Operating Center"
	And I select operating center "nj4" from the OperatingCenterId dropdown
	And I press "Add Operating Center"
	And I click the "Operating Centers" tab
	Then I should see the following values in the operatingCentersTable table 
	| Op Code | Operating Center Name |
	| NJ7     | Bob                   |
	| NJ4     | David                 |

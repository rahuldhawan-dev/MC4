Feature: UnionContractProposalPage

Background: Hey, let's build a sports arena in the Meadowlands!
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
	And a local "local2" exists with union: "union2", name: "321", description: "Ocean County", operating center: "nj4"
    And a union contract "uc1" exists with local: "local1", operating center: "nj7"
    And a union contract "uc2" exists with local: "local2", operating center: "nj4"
    And a union contract proposal "ucp1" exists with contract: "uc1"
    And a union contract proposal "ucp2" exists with contract: "uc2"
    And a data type "data type" exists with table name: "UnionContracts"

Scenario: a user without the role cannot access the union contract proposal page
	Given I am logged in as "invalid"
	When I visit the /UnionContractProposal/Search page
	Then I should see the missing role error

Scenario: user should only be able to search within operating centers they have access to
    Given I am logged in as "read-only-nj7"
    When I visit the /UnionContractProposal/Search page
    Then I should see operating center "nj7"'s Name in the OperatingCenter dropdown
    And I should see local "local1"'s Name in the Local dropdown	
    And I should see union contract "uc1"'s Description in the Contract dropdown	
    And I should not see operating center "nj4"'s Name in the OperatingCenter dropdown
    And I should not see local "local2"'s Name in the Local dropdown	
    And I should not see union contract "uc2"'s Description in the Contract dropdown	
    When I try to visit the Show page for union contract proposal: "ucp2" expecting an error
    Then I should see a 404 error message
    Given I am logged in as "read-only-nj4"
    When I visit the /UnionContractProposal/Search page
    Then I should see operating center "nj4"'s Name in the OperatingCenter dropdown
    And I should see local "local2"'s Name in the Local dropdown	
    And I should see union contract "uc2"'s Description in the Contract dropdown
    And I should not see operating center "nj7"'s Name in the OperatingCenter dropdown
    And I should not see local "local1"'s Name in the Local dropdown	
    And I should not see union contract "uc1"'s Description in the Contract dropdown	
    When I try to visit the Show page for union contract proposal: "ucp1" expecting an error
    Then I should see a 404 error message
	Given I am logged in as "user-admin-both"
    When I visit the /UnionContractProposal/Search page
    Then I should see operating center "nj4"'s Name in the OperatingCenter dropdown
    And I should see operating center "nj7"'s Name in the OperatingCenter dropdown
    And I should see local "local1"'s Name in the Local dropdown	
    And I should see local "local2"'s Name in the Local dropdown	
    And I should see union contract "uc1"'s Description in the Contract dropdown	
    And I should see union contract "uc2"'s Description in the Contract dropdown	

Scenario: admin can search by a selection of search fields
    Given I am logged in as "admin"
    When I visit the /UnionContractProposal/Search page
    And I select union contract "uc1"'s Description from the Contract dropdown
    And I press Search
    And I wait for the page to reload
    Then I should see a link to the Show page for union contract proposal "ucp1"
    And I should not see a link to the Show page for union contract proposal "ucp2"
    When I visit the /UnionContractProposal/Search page
    And I select operating center "nj4"'s Name from the OperatingCenter dropdown
    And I press Search
    And I wait for the page to reload
    Then I should see a link to the Show page for union contract proposal "ucp2"
    And I should not see a link to the Show page for union contract proposal "ucp1"
	When I visit the /UnionContractProposal/Search page
    And I select local "local1"'s Name from the Local dropdown
    And I press Search
    And I wait for the page to reload
    Then I should see a link to the Show page for union contract proposal "ucp1"
    And I should not see a link to the Show page for union contract proposal "ucp2"
    When I visit the /UnionContractProposal/Search page
    And I select union "union2"'s BargainingUnit from the Union dropdown
    And I press Search
    And I wait for the page to reload
    Then I should see a link to the Show page for union contract proposal "ucp2"
    And I should not see a link to the Show page for union contract proposal "ucp1"
    When I visit the /UnionContractProposal/Search page
    And I enter union contract proposal "ucp1"'s Id into the EntityId field
    And I press Search
    And I wait for the page to reload
    Then I should see a link to the Show page for union contract proposal "ucp1"
    And I should not see a link to the Show page for union contract proposal "ucp2"

Scenario: user with edit rights can edit union contract proposal
    Given I am logged in as "user-admin-nj7"
    When I visit the show page for union contract proposal: "ucp1"
    And I follow "Edit"
    Then I should be at the Edit page for union contract proposal: "ucp1"
    And I should see union contract "uc1"'s Description in the Contract dropdown
    And I should not see union contract "uc2"'s Description in the Contract dropdown
	When I enter "There is some value" into the ValuationAssumptions field
	And I press Save
    Then I should be at the Show page for union contract proposal: "ucp1"
    And I should see a display for ValuationAssumptions with "There is some value"

Scenario: user with nj7 edit and nj4 read rights cannot edit an nj4 union contract proposal
	Given a union contract proposal "nj4" exists with contract: "uc2"
	And a user "user-nj7-all-nj4-read" exists with username: "user-nj7-all-nj4-read", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7;roleReadNj4
	And I am logged in as "user-nj7-all-nj4-read"
	When I visit the Show page for union contract proposal: "nj4"
	Then I should not see a link to the Edit page for union contract proposal: "nj4"
	And I should not see the button "Delete"

Scenario: user with add rights can add union contract proposal
    Given I am logged in as "user-admin-nj7"
    When I visit the /UnionContractProposal/New page
    And I select union contract "uc1"'s Description from the Contract dropdown
    And I press Save
    Then the currently shown union contract proposal will now be referred to as "meh"
    And I should be at the Show page for union contract proposal: "meh"

Scenario: user with delete rights can delete union contract proposal
    Given a union contract proposal "ucp3" exists with contract: "uc1"
    And I am logged in as "user-admin-nj7"
    When I visit the show page for union contract proposal: "ucp3"
    And I click ok in the dialog after pressing "Delete"
    Then I should be at the unioncontractproposal/Search page
    When I press Search
    Then I should be at the UnionContractProposal page
    And I should see a link to the Show page for union contract proposal: "ucp1"
    And I should not see a link to the Show page for union contract proposal: "ucp3"
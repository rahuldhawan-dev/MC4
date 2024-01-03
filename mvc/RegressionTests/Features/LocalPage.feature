Feature: Local Page

Background: Users, Unions, Locals
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
    And a user "user-admin-both" exists with username: "hr-user-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And a user "user-nj7-all-nj4-read" exists with username: "user-nj7-all-nj4-read", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7;roleReadNj4
    And a union "union1" exists with bargaining unit: The International Brotherhood of Boilermakers%2c Iron Ship Builders%2c Blacksmiths%2c Forgers and Helpers
    And a union "union2" exists with bargaining unit: International Pirates%2c Haberdashers%2c Flaneurs%2c Printer’s Devils%2c Farriers%2c Alchemists%2c Coopers%2c Canal Puddlers%2c Fishmongers%2c Cellarmen%2c Wicket Goblins and Part-time Popinjays Union
	And a local "local1" exists with union: "union1", name: "123", description: "Monmouth County", operating center: "nj7"
	And a local "local2" exists with union: "union2", name: "123", description: "Ocean County", operating center: "nj4"
	And a state "one" exists with abbreviation: "NJ"
	And a division "one" exists with description: "foo", state: "one"

Scenario: a user without the role cannot access the local page
	Given I am logged in as "invalid"
	When I visit the /Local/Search page
	Then I should see the missing role error

Scenario: admin can search for and edit locals
    Given a coordinate "one" exists 
	And I am logged in as "admin"
    When I visit the Local/Search page
    And I select union "union1"'s BargainingUnit from the Union dropdown
    And I press Search
    And I wait for the page to reload
    Then I should be at the Show page for local: "local1"
	When I follow "Edit"
    And I wait for the page to reload
    And I select union "union2"'s BargainingUnit from the Union dropdown
    And I enter "321" into the Name field
    And I enter "Ocean County" into the Description field
	And I enter coordinate "one"'s Id into the Coordinate field
    And I select operating center "nj4" from the OperatingCenter dropdown
    And I press Save
    Then I should be at the Show page for local: "local1"
    And I should see a display for Union_BargainingUnit with union "union2"'s BargainingUnit
    And I should see a display for Name with "321"
    And I should see a display for Description with "Ocean County"
    And I should see a display for OperatingCenter_OperatingCenterCode with operating center "nj4"'s OperatingCenterCode

Scenario: nj7 user can search for and edit locals
    Given a coordinate "one" exists 
	And I am logged in as "user-admin-nj7"
    When I visit the Local/Search page
    And I select union "union1"'s BargainingUnit from the Union dropdown
    And I press Search
    And I wait for the page to reload
    Then I should be at the Show page for local: "local1"
	When I follow "Edit"
    And I wait for the page to reload
    Then I should not see operating center "nj4" in the OperatingCenter dropdown
    And I should see operating center "nj7" in the OperatingCenter dropdown
    When I select union "union2"'s BargainingUnit from the Union dropdown
    And I enter "321" into the Name field
    And I enter "Ocean County" into the Description field
	And I enter coordinate "one"'s Id into the Coordinate field
    And I press Save
    Then I should be at the Show page for local: "local1"
    And I should see a display for Union_BargainingUnit with union "union2"'s BargainingUnit
    And I should see a display for Name with "321"
    And I should see a display for Description with "Ocean County"

Scenario: nj7 user cannot search for, view, or edit nj4 locals
    Given I am logged in as "user-admin-nj7"
    When I visit the Local/Search page
    And I select union "union2"'s BargainingUnit from the Union dropdown
    And I press Search
    And I wait for the page to reload
    Then I should be at the Local/Search page
    And I should see the error message No results matched your query.
    When I try to access the Show page for local: "local2" expecting an error
    Then I should see a 404 error message
    When I try to access the Edit page for local: "local2" expecting an error
    Then I should see a 404 error message

Scenario: nj7 user and nj4 reader cannot edit nj4 locals
    Given I am logged in as "user-nj7-all-nj4-read"
    When I visit the Local/Search page
    And I press Search
    And I wait for the page to reload
    Then I should be at the Local page
    And I should not see the error message No results matched your query.
	When I follow the Show link for local "local2"
    Then I should be at the Show page for local: "local2"
	And I should not see a link to the Edit page for local: "local2"
	And I should not see the button "Delete"
    
Scenario: read-only user can search and view, but cannot edit or manipulate notes/documents
    Given I am logged in as "read-only-nj7"
    When I visit the Local/Search page
    And I select union "union1"'s BargainingUnit from the Union dropdown
    And I press Search
    And I wait for the page to reload
    Then I should be at the Show page for local: "local1"
    And I should not see a link to the edit page for local: "local1"
    When I click the "Notes" tab
    Then I should not see the button "New Note"
    When I click the "Documents" tab
    Then I should not see the button "Link Existing Document"
    And I should not see the button "New Document"
    When I try to access the Edit page for local: "local1" expecting an error
    Then I should see the missing role error

Scenario: user with add rights can add a new local
    Given a coordinate "one" exists 
	And I am logged in as "user-admin-nj7"
    When I visit the /Local/new page
    And I enter "foo" into the Name field
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select union "union1"'s BargainingUnit from the Union dropdown
	And I check the IsActive field
	And I enter coordinate "one"'s Id into the Coordinate field
	And I select state "one"'s Abbreviation from the State dropdown
	And I select division "one"'s Description from the Division dropdown
	And I enter "foo" into the SAPUnionDescription field
    And I press Save
    Then the currently shown local will now be referred to as "new local"
    And I should be at the Show page for local: "new local"
	And I should see a display for IsActive with "Yes"
	And I should see a display for "Division" with division: "one"'s Description
	And I should see a display for "SAPUnionDescription" with local: "new local"'s SAPUnionDescription

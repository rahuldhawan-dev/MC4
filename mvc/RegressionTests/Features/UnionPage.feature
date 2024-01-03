Feature: Union Page

Background: In the beginning Jason created some Users and Unions
	Given a role "roleRead" exists with action: "Read", module: "HumanResourcesUnion"
	And a role "roleEdit" exists with action: "Edit", module: "HumanResourcesUnion"
	And a role "roleAdd" exists with action: "Add", module: "HumanResourcesUnion"
	And a role "roleDelete" exists with action: "Delete", module: "HumanResourcesUnion"
    And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
	And a user "invalid" exists with username: "invalid"
    And a user "read-only" exists with username: "read-only", roles: roleRead
    And a user "hr-user" exists with username: "hr-user", roles: roleRead;roleEdit;roleAdd;roleDelete
    And a union "union1" exists with bargaining unit: The International Brotherhood of Boilermakers%2c Iron Ship Builders%2c Blacksmiths%2c Forgers and Helpers

Scenario: a user without the role cannot access the union page
	Given I am logged in as "invalid"
	When I visit the /Union/Search page
	Then I should see the missing role error

Scenario: admin can search for and edit unions
    Given I am logged in as "admin"
    When I visit the Union/Search page
	And I select union "union1"'s BargainingUnit from the EntityId dropdown
    And I press Search
    And I wait for the page to reload
    Then I should be at the Show page for union: "union1"
	When I follow "Edit"
    And I wait for the page to reload
    And I enter "Teamasters" into the BargainingUnit field
    And I press Save
    Then I should be at the Show page for union: "union1"
    And I should see "Teamasters"

Scenario: user should not have access
    Given I am logged in as "user"
    When I visit the Union/Search page
    Then I should see "You do not have the roles necessary to access this resource."
    When I visit the Show page for union: "union1"
    Then I should see "You do not have the roles necessary to access this resource."

Scenario: user with union role can search for and edit unions
    Given I am logged in as "hr-user"
    When I visit the Union/Search page
    And I select union "union1"'s BargainingUnit from the EntityId dropdown
    And I press Search
    And I wait for the page to reload
    Then I should be at the Show page for union: "union1"
	When I follow "Edit"
    And I wait for the page to reload
    And I enter "Teamasters" into the BargainingUnit field
    And I press Save
    Then I should be at the Show page for union: "union1"
    And I should see "Teamasters"

Scenario: user with add rights can add unions
    Given I am logged in as "hr-user"
    When I visit the /Union/New page
    And I enter "Teamsters" into the BargainingUnit field
    And I press Save
    Then the currently shown union will now be referred to as "new union"
    And I should be at the Show page for union: "new union"

Scenario: user with only read role should only be able to view unions
    Given I am logged in as "read-only"
    When I visit the Union/Search page
    And I select union "union1"'s BargainingUnit from the EntityId dropdown
    And I press Search
    And I wait for the page to reload
    Then I should be at the Show page for union: "union1"
    And I should not see "You do not have the roles necessary to access this resource."
    And I should not see a link to the Edit page for union: "union1"
    When I visit the Edit page for union: "union1"
    Then I should see "You do not have the roles necessary to access this resource."

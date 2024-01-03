Feature: ChemicalUnitCostPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a chemical "one" exists
    And a chemical unit cost "one" exists with chemical: "one"
    And a chemical unit cost "two" exists with chemical: "one"
    And I am logged in as "admin"

Scenario: user can search for a chemical unit cost
    When I visit the Environmental/ChemicalUnitCost/Search page
    And I press Search
    Then I should see a link to the Show page for chemical unit cost: "one"
    When I follow the Show link for chemical unit cost "one"
    Then I should be at the Show page for chemical unit cost: "one"

Scenario: user can view a chemical unit cost
    When I visit the Show page for chemical unit cost: "one"
    Then I should see a display for chemical unit cost: "one"'s ChemicalOrderingProcess

Scenario: user can add a chemical unit cost
    When I visit the Environmental/ChemicalUnitCost/New page
    And I enter "foo" into the ChemicalOrderingProcess field
	And I select chemical "one" from the Chemical dropdown
    And I press Save
    Then the currently shown chemical unit cost will now be referred to as "new"
    And I should see a display for ChemicalOrderingProcess with "foo"

Scenario: user can edit a chemical unit cost
    When I visit the Edit page for chemical unit cost: "one"
    And I enter "bar" into the ChemicalOrderingProcess field
    And I press Save
    Then I should be at the Show page for chemical unit cost: "one"
    And I should see a display for ChemicalOrderingProcess with "bar"
	
Feature: ChemicalInventoryTransactionPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a chemical storage "one" exists
    And a chemical inventory transaction "one" exists with storage: "one"
    And a chemical inventory transaction "two" exists with storage: "one"
    And I am logged in as "admin"

Scenario: user can search for a chemical inventory transaction
    When I visit the Environmental/ChemicalInventoryTransaction/Search page
    And I press Search
    Then I should see a link to the Show page for chemical inventory transaction: "one"
    When I follow the Show link for chemical inventory transaction "one"
    Then I should be at the Show page for chemical inventory transaction: "one"

Scenario: user can view a chemical inventory transaction
    When I visit the Show page for chemical inventory transaction: "one" with inventory record type: "foo"
    Then I should see a display for chemical inventory transaction: "one"'s InventoryRecordType

Scenario: user can add a chemical inventory transaction
    When I visit the Environmental/ChemicalInventoryTransaction/New page
	And I select chemical storage "one" from the Storage dropdown
    And I enter "foo" into the InventoryRecordType field
    And I press Save
    Then the currently shown chemical inventory transaction will now be referred to as "new"
    And I should see a display for InventoryRecordType with "foo"

Scenario: user can edit a chemical inventory transaction
    When I visit the Edit page for chemical inventory transaction: "one"
    And I enter "bar" into the InventoryRecordType field
    And I press Save
    Then I should be at the Show page for chemical inventory transaction: "one"
    And I should see a display for InventoryRecordType with "bar"

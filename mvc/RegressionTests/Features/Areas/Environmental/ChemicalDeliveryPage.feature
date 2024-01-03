Feature: ChemicalDeliveryPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a chemical "one" exists
	And a chemical storage "one" exists with chemical: "one"
    And a chemical delivery "one" exists with chemical: "one", storage: "one"
    And a chemical delivery "two" exists with chemical: "one", storage: "one"
    And I am logged in as "admin"

Scenario: user can search for a chemical delivery
    When I visit the Environmental/ChemicalDelivery/Search page
    And I press Search
    Then I should see a link to the Show page for chemical delivery: "one"
    When I follow the Show link for chemical delivery "one"
    Then I should be at the Show page for chemical delivery: "one"

Scenario: user can view a chemical delivery
    When I visit the Show page for chemical delivery: "one"
    Then I should see a display for chemical delivery: "one"'s ConfirmationInformation

Scenario: user can add a chemical delivery
    When I visit the Environmental/ChemicalDelivery/New page
	And I select chemical "one" from the Chemical dropdown
	And I select chemical storage "one" from the Storage dropdown
    And I enter "foo" into the ConfirmationInformation field
    And I press Save
    Then the currently shown chemical delivery will now be referred to as "new"
    And I should see a display for ConfirmationInformation with "foo"

Scenario: user can edit a chemical delivery
    When I visit the Edit page for chemical delivery: "one"
    And I enter "bar" into the ConfirmationInformation field
    And I press Save
    Then I should be at the Show page for chemical delivery: "one"
    And I should see a display for ConfirmationInformation with "bar"

Feature: BillingPartyPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a billing party "one" exists
    And a billing party "two" exists

Scenario: user can search for a billing party
    Given I am logged in as "admin"
    When I visit the FieldOperations/BillingParty/Search page
    And I press Search
    Then I should see a link to the Show page for billing party: "one"
    When I follow the Show link for billing party "one"
    Then I should be at the Show page for billing party: "one"

Scenario: user can view a billing party
    Given I am logged in as "admin"
    When I visit the Show page for billing party: "one"
    Then I should see a display for billing party: "one"'s Description

Scenario: user can add a billing party
    Given I am logged in as "admin"
    When I visit the FieldOperations/BillingParty/New page
    And I enter "foo" into the Description field
    And I press Save
    Then the currently shown billing party will now be referred to as "new"
    And I should see a display for Description with "foo"

Scenario: user can edit a billing party
    Given I am logged in as "admin"
    When I visit the Edit page for billing party: "one"
    And I enter "bar" into the Description field
    And I press Save
    Then I should be at the Show page for billing party: "one"
    And I should see a display for Description with "bar"

Scenario: user can add a contact
	Given a user "user" exists with username: "user"
	And a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And an operating center "one" exists with opcode: "QQ1", name: "Wawa"
	And a role "editrole" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "one"
	And a contact type "ct" exists with description: "yowzah!"
	And a billing party contact type "x" exists with contact type: "ct"
	And I am logged in as "user"
	When I visit the Show page for billing party: "one"
	And I click the "Contacts" tab
	And I press "Add Contact for Billing Party"
	And I enter "Tina" and select "Belcher, Tina" from the Contact autocomplete field
	And I select contact type "ct"'s Description from the ContactType dropdown
	And I press saveBillingPartyContactButton
	And I wait for the page to reload
	Then I should be at the Show page for billing party: "one"
	When I click the "Contacts" tab
	Then I should see "Belcher, Tina"

Scenario: admin can delete a contact
	Given a user "user" exists with username: "user"
	And an operating center "one" exists with opcode: "QQ1", name: "Wawa"
	And a role "editrole" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "one"
	And a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com"
	And a contact type "ct" exists with description: "yowzah!"
	And a billing party contact "tc" exists with contact: "contact", billing party: "one", contact type: "ct"
	And I am logged in as "admin"
	When I visit the Show page for billing party: "one"
	And I click the "Contacts" tab
    And I click the "Remove" button in the 1st row of contactsTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Contacts" tab
	Then I should not see "Belcher, Tina"
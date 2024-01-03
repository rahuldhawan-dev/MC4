Feature: ChemicalVendorPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a chemical vendor "one" exists
    And a chemical vendor "two" exists
    And I am logged in as "admin"

Scenario: user can search for a chemical vendor
    When I visit the Environmental/ChemicalVendor/Search page
    And I press Search
    Then I should see a link to the Show page for chemical vendor: "one"
    When I follow the Show link for chemical vendor "one"
    Then I should be at the Show page for chemical vendor: "one"

Scenario: user can view a chemical vendor
    When I visit the Show page for chemical vendor: "one"
    Then I should see a display for chemical vendor: "one"'s Vendor

Scenario: user can add a chemical vendor
    When I visit the Environmental/ChemicalVendor/New page
    And I enter "foo" into the Vendor field
    And I press Save
    Then the currently shown chemical vendor will now be referred to as "new"
    And I should see a display for Vendor with "foo"

Scenario: user can edit a chemical vendor
    When I visit the Edit page for chemical vendor: "one"
    And I enter "bar" into the Vendor field
    And I press Save
    Then I should be at the Show page for chemical vendor: "one"
    And I should see a display for Vendor with "bar"
	
Feature: Regulations Page

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a regulation "one" exists
    And a regulation status "active" exists with description: "Active"
    And a regulation status "inactive" exists with description: "Inactive"
    And a regulation agency "meh" exists with description: "Federal Government"

Scenario: admin can create regulations
    Given I am logged in as "admin"
    When I visit the Regulation/New page
    And I press Save
    Then I should see the validation message "The Status field is required."
	And I should see the validation message "The Agency field is required."
    When I select regulation status "active" from the Status dropdown
    And I select regulation agency "meh" from the Agency dropdown
    And I press Save
	Then the currently shown regulation will now be referred to as "new"
    And I should be at the Show page for regulation: "new"

Scenario: admin can edit regulations
    Given I am logged in as "admin"
    When I visit the Show page for regulation: "one"
    And I follow "Edit"
    Then I should be at the Edit page for regulation: "one"
    When I select regulation status "inactive" from the Status dropdown
    And I press Save
    Then I should be at the Show page for regulation: "one"
    And I should see a display for Status with regulation status "inactive"

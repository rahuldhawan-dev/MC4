Feature: FireDistrictPage
	Page for managing Fire Districts

Background:
    Given a user "user" exists with username: "user"
    And a role "roleRead" exists with action: "Read", module: "FieldServicesDataLookups", user: "user"
    And a role "roleEdit" exists with action: "Edit", module: "FieldServicesDataLookups", user: "user"
    And a role "roleAdd" exists with action: "Add", module: "FieldServicesDataLookups", user: "user"
    And a role "roleDelete" exists with action: "Delete", module: "FieldServicesDataLookups", user: "user"
    And a state "nj" exists with abbreviation: "NJ"
    And a state "ny" exists with abbreviation: "NY"

Scenario: User can create a fire district
    Given I am logged in as "user"
    And I am at the FireDistrict/New page
    When I press Save
    Then I should see a validation message for DistrictName with "The DistrictName field is required."
    And I should see a validation message for State with "The State field is required."
    When I enter "Some District" into the DistrictName field
    And I select state "nj" from the State dropdown
    And I press Save
    Then the currently shown fire district shall henceforth be known throughout the land as "one"
    And I should be at the Show page for fire district "one"
    And I should see a display for DistrictName with "Some District"
    And I should see a display for State with state "nj"

Scenario: User can edit a fire district
    Given a fire district "one" exists with district name: "Some District", state: "nj"
    And I am logged in as "user"
    And I am at the Edit page for fire district "one"
    When I enter "Some Other District" into the DistrictName field
    And I select state "ny" from the State dropdown
    And I press Save
    Then I should be at the Show page for fire district "one"
    And I should see a display for DistrictName with "Some Other District"
    And I should see a display for State with state "ny"

Scenario: User can search for and view a fire district
    Given a fire district "one" exists with district name: "Some District", state: "nj"
    And a fire district "two" exists with district name: "Some Other District", state: "ny"
    And I am logged in as "user"
    And I am at the FireDistrict/Search page
    When I select state "nj" from the State dropdown
    And I press Search
    Then I should be at the FireDistrict page
    And I should see a link to the show page for fire district "one"
    And I should not see a link to the show page for fire district "two"

Scenario: User can delete a fire district
    Given a fire district "one" exists with district name: "Some District", state: "nj"
    And I am logged in as "user"
    When I visit the Show page for fire district "one"
    And I click ok in the dialog after pressing "Delete"
    Then I should be at the FireDistrict/Search page
    When I try to access the Show page for fire district: "one" expecting an error
    Then I should see a 404 error message

Scenario: User cannot delete fire districts which are linked to things
    Given a fire district "one" exists with district name: "Some District", state: "nj"
    And a town "one" exists with state: "nj"
    And a fire district town "one" exists with fire district: "one", town: "one"
    And I am logged in as "user"
    When I visit the Show page for fire district "one"
    Then I should not see the "Delete" button in the action bar

Feature: TrafficControlTicketCheckPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a traffic control ticket check "one" exists
    And a traffic control ticket check "two" exists
    And I am logged in as "admin"

Scenario: user can search for a traffic control ticket check
    When I visit the FieldOperations/TrafficControlTicketCheck/Search page
    And I press Search
    Then I should see a link to the Show page for traffic control ticket check: "one"
    When I follow the Show link for traffic control ticket check "one"
    Then I should be at the Show page for traffic control ticket check: "one"

Scenario: user can view a traffic control ticket check
    When I visit the Show page for traffic control ticket check: "one"
    Then I should see a display for traffic control ticket check: "one"'s Memo

Scenario: user can edit a traffic control ticket check
    When I visit the Edit page for traffic control ticket check: "one"
    And I enter "bar" into the Memo field
    And I press Save
    Then I should be at the Show page for traffic control ticket check: "one"
    And I should see a display for Memo with "bar"

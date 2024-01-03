Feature: OneCallMarkoutTicketPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a one call markout message type "routine" exists with description: "routine"
	And a one call markout message type "update" exists with description: "update"
    And a one call markout ticket "one" exists with message type: "routine", request number: "12345", c d c code: "abc"
    And a one call markout ticket "two" exists with message type: "routine", request number: "12346", related request number: "12345", c d c code: "abc"
    And I am logged in as "admin"

Scenario: user can search for a one call markout ticket
    When I visit the FieldOperations/OneCallMarkoutTicket/Search page
    And I press Search
    Then I should see a link to the Show page for one call markout ticket: "one"
    When I follow the "View" link to the Show page for one call markout ticket "one"
    Then I should be at the Show page for one call markout ticket: "one"

Scenario: user can view a one call markout ticket
    When I visit the Show page for one call markout ticket: "two"
    Then I should see a display for one call markout ticket: "two"'s RequestNumber
	And I should see a link to the Show page for one call markout ticket "one"
	When I follow the Show link for one call markout ticket "one"
	Then I should see a link to the Show page for one call markout ticket "two"
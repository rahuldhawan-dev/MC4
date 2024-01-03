Feature: OneCallMarkoutAuditPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a one call markout audit "one" exists with date received: "today"
    And a one call markout audit "two" exists with date received: "today"
    And I am logged in as "admin"

Scenario: user can search for a one call markout audit
    When I visit the FieldOperations/OneCallMarkoutAudit/Search page
    And I press Search
    Then I should see a link to the Show page for one call markout audit: "one"
    When I follow the Show link for one call markout audit "one"
    Then I should be at the Show page for one call markout audit: "one"

Scenario: user can view a one call markout audit
    When I visit the Show page for one call markout audit: "one"
    Then I should see a display for one call markout audit: "one"'s DateReceived
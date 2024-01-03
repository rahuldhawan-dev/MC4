Feature: OneCallMarkoutResponsePage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And a one call markout message type "routine" exists with description: "routine"
	And a one call markout message type "update" exists with description: "update"
    And a one call markout ticket "one" exists with message type: "routine", request number: "12345"
    And a one call markout ticket "two" exists with message type: "routine", request number: "12346", related request number: "12345"
	And a one call markout response status "one" exists with description: "unable to locate"
	And a one call markout response technique "one" exists with description: "all"
	And I am logged in as "admin"

Scenario: user can add a one call markout response
    When I visit the Show page for one call markout ticket: "one"
    And I follow "Respond"
	And I wait for the page to reload
	And I select one call markout response status "one" from the OneCallMarkoutResponseStatus dropdown
	And I select one call markout response technique "one" from the OneCallMarkoutResponseTechnique dropdown
	And I enter "arhgasdfasd" into the Comments field
	And I press Save
    Then the currently shown one call markout response will now be referred to as "hannibal"
    And I should see a display for Comments with "arhgasdfasd"
	And I should see a link to the Show page for one call markout ticket "one"

Scenario: user can view a one call markout response
	Given a one call markout response "one" exists with one call markout ticket: "one", completed by: "user"
    When I visit the Show page for one call markout ticket: "one"
	And I click the "Responses" tab
    Then I should see one call markout response "one"'s CompletedAt in the table responseTable's "Completed At" column
	When I follow the Show link for one call markout response "one"
	Then I should be at the Show page for one call markout response "one"

Scenario: user can edit a one call markout response
    Given a one call markout response "one" exists with one call markout ticket: "one", completed by: "user", completed at: "yesterday"
	When I visit the Edit page for one call markout response: "one"
    And I enter "bar" into the Comments field
    And I press Save
    Then I should be at the Show page for one call markout response: "one"
    And I should see a display for Comments with "bar"

Scenario: special rules required when m/o performed has been selected
    Given a one call markout response status "performed" exists with description: "M/O Performed"
    When I visit the Show page for one call markout ticket: "one"
	And I follow "Respond"
    And I wait for the page to reload
	And I select one call markout response status "performed" from the OneCallMarkoutResponseStatus dropdown
	And I press Save
    Then I should be at the FieldOperations/OneCallMarkoutResponse/Create page
    And I should see the validation message "A technique and one of 'Paint', 'Flag', or 'Stake' must be selected when markout has been performed."
	When I select "No" from the Paint dropdown
	And I select "No" from the Flag dropdown
	And I select "No" from the Stake dropdown
	And I press Save
    Then I should be at the FieldOperations/OneCallMarkoutResponse/Create page
    And I should see the validation message "A technique and one of 'Paint', 'Flag', or 'Stake' must be selected when markout has been performed."
    When I select "Yes" from the Flag dropdown
	And I press Save
    Then I should be at the FieldOperations/OneCallMarkoutResponse/Create page
    And I should see the validation message "A technique and one of 'Paint', 'Flag', or 'Stake' must be selected when markout has been performed."
	When I select one call markout response technique "one" from the OneCallMarkoutResponseTechnique dropdown
	And I press Save
    Then the currently shown one call markout response will now be referred to as "new"
	And I should see a link to the Show page for one call markout ticket "one"

Scenario: certain statuses cause certain fields to be hidden
    Given a one call markout response status "no facilities" exists with description: "No Facilities"
    And a one call markout response status "cancelled" exists with description: "Cancelled"
    And a one call markout response status "co not required" exists with description: "Co. Not Required"
    And a one call markout response status "unable to locate" exists with description: "Unable to Locate"
    When I visit the Show page for one call markout ticket: "one"
	And I follow "Respond"
    And I wait for the page to reload
	And I select one call markout response status "no facilities" from the OneCallMarkoutResponseStatus dropdown
    Then I should not see the OneCallMarkoutResponseTechnique field
    And I should not see the Paint field
    And I should not see the Flag field
    And I should not see the Stake field
	When I select one call markout response status "cancelled" from the OneCallMarkoutResponseStatus dropdown
    Then I should not see the OneCallMarkoutResponseTechnique field
    And I should not see the Paint field
    And I should not see the Flag field
    And I should not see the Stake field
	When I select one call markout response status "co not required" from the OneCallMarkoutResponseStatus dropdown
    Then I should not see the OneCallMarkoutResponseTechnique field
    And I should not see the Paint field
    And I should not see the Flag field
    And I should not see the Stake field
	When I select one call markout response status "unable to locate" from the OneCallMarkoutResponseStatus dropdown
    Then I should not see the OneCallMarkoutResponseTechnique field
    And I should not see the Paint field
    And I should not see the Flag field
    And I should not see the Stake field
	When I select one call markout response status "one" from the OneCallMarkoutResponseStatus dropdown
    Then I should see the OneCallMarkoutResponseTechnique field
    And I should see the Paint field
    And I should see the Flag field
    And I should see the Stake field

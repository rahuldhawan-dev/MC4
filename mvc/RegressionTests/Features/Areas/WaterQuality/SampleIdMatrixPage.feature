Feature: SampleIdMatrixPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "NJ7"
	And a town "one" exists with name: "Townie"
    And public water supply statuses exist
	And a public water supply "one" exists with op code: "NJ7", status: "active"
	And a sample site status "active" exists with description: "Active"
	And a sample site "one" exists with town: "one", sample site name: "Grateful Deli", public water supply: "one", sample site status: "active", bacti site: "true"
    And a sample id matrix "one" exists with sample site: "one"
    And a sample id matrix "two" exists with sample site: "one"
    And I am logged in as "admin"

Scenario: user can search for a sample id matrix
    When I visit the WaterQuality/SampleIdMatrix/Search page
    And I press Search
    Then I should see a link to the Show page for sample id matrix: "one"
    When I follow the Show link for sample id matrix "one"
    Then I should be at the Show page for sample id matrix: "one"

Scenario: user can view a sample id matrix
    When I visit the Show page for sample id matrix: "one"
    Then I should see a display for sample id matrix: "one"'s Parameter

Scenario: user can add a sample id matrix
    When I visit the WaterQuality/SampleIdMatrix/New page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select sample site "one"'s ToString from the SampleSite dropdown
    And I enter "foo" into the Parameter field
    And I press Save
    Then the currently shown sample id matrix will now be referred to as "davros"
    And I should see a display for Parameter with "foo"

Scenario: user can edit a sample id matrix
    When I visit the Edit page for sample id matrix: "one"
    And I enter "bar" into the Parameter field
    And I press Save
    Then I should be at the Show page for sample id matrix: "one"
    And I should see a display for Parameter with "bar"

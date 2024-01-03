Feature: DevelopmentProjectPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And an operating center "nj7" exists with operating center code: "nj7"
    And an operating center "nj4" exists with operating center code: "nj4"
    And a development project category "one" exists
    And a development project category "two" exists
    And a business unit "one" exists with operating center: "nj7"
    And a business unit "two" exists with operating center: "nj4" 
    And public water supply statuses exist
    And a public water supply "one" exists
    And a public water supply "two" exists
	And a development project "one" exists with operating center: "nj7", category: "one", business unit: "one"
    And a development project "two" exists with operating center: "nj4", category: "two", business unit: "two"
    And I am logged in as "admin"

Scenario: user can search for a development project
    When I visit the ProjectManagement/DevelopmentProject/Search page
    And I press Search
    Then I should see a link to the Show page for development project: "one"
    When I follow the Show link for development project "one"
    Then I should be at the Show page for development project: "one"

Scenario: user can view a development project
    When I visit the Show page for development project: "one"
    Then I should see a display for development project: "one"'s ProjectDescription

Scenario: user can add a development project
    When I visit the ProjectManagement/DevelopmentProject/New page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select development project category "one" from the Category dropdown
    And I select business unit "one" from the BusinessUnit dropdown
    And I select public water supply "one" from the PublicWaterSupply dropdown
    And I enter "12345" into the WBSNumber field
    And I enter 1 into the DomesticWaterServices field
    And I enter 2 into the FireServices field
    And I enter 3 into the DomesticSanitaryServices field
    And I enter "foo" into the ProjectDescription field
    And I press Save
	Then I should see a validation message for ForecastedInServiceDate with "The ForecastedInServiceDate field is required."
	When I enter today's date into the ForecastedInServiceDate field
	And I press Save
    Then the currently shown development project will now be referred to as "new"
    And I should see a display for ProjectDescription with "foo"

Scenario: user can edit a development project
    When I visit the Edit page for development project: "one"
    And I enter "bar" into the ProjectDescription field
    And I press Save
    Then I should be at the Show page for development project: "one"
    And I should see a display for ProjectDescription with "bar"

Feature: WaterConstituentPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a water constituent "one" exists
    And a water constituent "two" exists
    And a state "one" exists with Abbreviation: "NJ"
    And a state "two" exists with Abbreviation: "NY"
    And a unit of water sample measure "one" exists	
    And a drinking water contaminant category "one" exists with Description: "in buckets"
    And a waste water contaminant category "one" exists with Description: "in a balloon"
    And I am logged in as "admin"

Scenario: user can search for a water constituent
    When I visit the WaterQuality/WaterConstituent/Search page
    And I press Search
    Then I should see a link to the Show page for water constituent: "one"
    When I follow the Show link for water constituent "one"
    Then I should be at the Show page for water constituent: "one"

Scenario: user can view a water constituent
    When I visit the Show page for water constituent: "one"
    Then I should see a display for water constituent: "one"'s Description

Scenario: user can add a water constituent
    When I visit the WaterQuality/WaterConstituent/New page
    And I enter "foo" into the Description field
    And I select drinking water contaminant category "one" from the DrinkingWaterContaminantCategory dropdown
    And I select waste water contaminant category "one" from the WasteWaterContaminantCategory dropdown
    And I press Save
    Then the currently shown water constituent will now be referred to as "rick"
    And I should see a display for Description with "foo"
    And I should see a display for DrinkingWaterContaminantCategory with "in buckets"
    And I should see a display for WasteWaterContaminantCategory with "in a balloon"

Scenario: user can edit a water constituent
    When I visit the Edit page for water constituent: "one"
    And I enter "bar" into the Description field
    And I press Save
    Then I should be at the Show page for water constituent: "one"
    And I should see a display for Description with "bar"

Scenario: user can add and remove a state limit to a water constituent
    When I visit the Show page for water constituent: "one"
    And I click the "State Limits" tab
	And I press "Add New State Limit"
    And I select state "one" from the State dropdown
    And I select unit of water sample measure "one" from the UnitOfMeasure dropdown
    And I press "Save State Limit"
    Then I should be at the Show page for water constituent: "one"
    When I click the "State Limits" tab
    Then I should see state "one"'s Abbreviation in the table stateLimits's "State" column
    And I should see unit of water sample measure "one"'s Description in the table stateLimits's "Unit Of Measure" column
    And I should not see state "one"'s Abbreviation in the State dropdown
    When I click the "State Limits" tab
    And I click ok in the dialog after pressing "Remove State Limit"
    Then I should be at the Show page for water constituent: "one"
    When I click the "State Limits" tab
    Then the stateLimits table should be empty

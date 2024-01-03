Feature: WaterSamplePage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "NJ7"
	And a town "one" exists with name: "Townie"
    And public water supply statuses exist
	And a public water supply "one" exists with op code: "NJ7", status: "active"
	And a sample site status "active" exists with description: "Active"
	And a sample site "one" exists with town: "one", sample site name: "Grateful Deli", public water supply: "one", sample site status: "active", bacti site: "true"
    And a unit of water sample measure "one" exists with description: "foo"
    And a water constituent "one" exists with unit of measure: "one"
	And a sample id matrix "one" exists with sample site: "one", water constituent: "one"
    And a water sample "one" exists with sample id matrix: "one", sample date: "12/12/12"
    And a water sample "two" exists with sample id matrix: "one", sample date: "12/12/13"
    And I am logged in as "admin"

Scenario: user can search for a water sample
    When I visit the WaterQuality/WaterSample/Search page
    And I press Search
    Then I should see a link to the Show page for water sample: "one"
    When I follow the Show link for water sample "one"
    Then I should be at the Show page for water sample: "one"

Scenario: user can view a water sample
    When I visit the Show page for water sample: "one"
    Then I should see a display for water sample: "one"'s AnalysisPerformedBy

Scenario: user can add a water sample
    When I visit the WaterQuality/WaterSample/New page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select sample site "one"'s ToString from the SampleSite dropdown
    And I enter "foo" into the AnalysisPerformedBy field
	And I enter today's date into the SampleDate field
	And I select sample id matrix "one" from the SampleIdMatrix dropdown
    And I wait for ajax to finish loading
    And I select unit of water sample measure "one" from the UnitOfMeasure dropdown
	And I enter "1.08" into the SampleValue field
    And I press Save
    Then the currently shown water sample will now be referred to as "edward"
    And I should see a display for AnalysisPerformedBy with "foo"
	And I should see a display for SampleValue with "1.08"

Scenario: user can edit a water sample
    When I visit the Edit page for water sample: "one"
    And I enter "bar" into the AnalysisPerformedBy field
	And I enter "1.098" into the SampleValue field
    And I press Save
    Then I should be at the Show page for water sample: "one"
    And I should see a display for AnalysisPerformedBy with "bar"
	And I should see a display for SampleValue with "1.098"
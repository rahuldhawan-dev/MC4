Feature: MeasurementPointEquipmentTypePage

Background: things exist
    Given an operating center "nj4" exists with opcode: "NJ4", name: "or the right of the people peaceably to assemble"
    And an admin user "admin" exists with username: "admin"
	And equipment manufacturers exist
    And equipment types exist
	And a town "nj4ton" exists
	And a planning plant "one" exists with operating center: "nj4", code: "D214", description: "Hgra"
	And a facility "one" exists with operating center: "nj4", facility id: "NJSB-3", facility name: "A Nother Facility", planning plant: "one", has confined space requirement: "True", functional location: "North"
	And a facility area "one" exists with description: "facilityLab"
	And a facility sub area "one" exists with description: wet, area: "one"
	And a facility facility area "one" exists with facility: "one", facilityArea: "one", facilitySubArea: "one"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", facility facility area: "one", equipment type: "generator", description: "RTU1", s a p equipment id: 1
    And a unit of measure "one" exists with description: "inches"
	And a measurement point equipment type "unused" exists with description: "Measurement", category: "A", equipment type: "generator", min: "1.0", max: "5.0", unit of measure: "one", position: "1"
	And a measurement point equipment type "used" exists with description: "Measurement 2", category: "A", equipment type: "generator", min: "1.0", max: "5.0", unit of measure: "one", position: "1"
	And order types exist
	And a production work description "one" exists with description: "GENERAL REPAIR", order type: "corrective action"
	And a production work order "one" exists with production work description: "one", operating center: "nj4", planning plant: "one", facility: "one", equipment: "one", estimated completion hours: "1", local task description: "description one"
	And a production work order measurement point value exists with production work order: "one", equipment: "one", value: "2", measurement point equipment type: "used", measurement doc id: "123"

Scenario: admin can edit all fields of a measurement point if it is not currently in use
	Given I am logged in as "admin"
	When I visit the Edit page for measurement point equipment type "unused"
	And I enter "B" into the Category field
	And I enter "Measurement X" into the Description field
	And I enter "101" into the Position field
	And I select unit of measure "one"'s Description from the UnitOfMeasure dropdown
	And I enter "1.0" into the Min field
	And I enter "10.0" into the Max field
	And I select "Yes" from the IsActive dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for equipment type: "generator"
	When I click the "Measurement Points" tab
	And I wait for the page to reload
	Then I should see "Measurement X" in the table measurementPointsTable's "Description" column
	
Scenario: admin can only edit certain fields of a measurement point if it is currently in use
	Given I am logged in as "admin"
	When I visit the Edit page for measurement point equipment type "used"
	Then the PositionDisplay field should be disabled
	And the CategoryDisplay field should be disabled
	And the MinDisplay field should be disabled
	And the MaxDisplay field should be disabled
	And the UnitOfMeasureDisplay field should be disabled
	When I enter "Measurement X" into the Description field
	And I select "Yes" from the IsActive dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for equipment type: "generator"
	When I click the "Measurement Points" tab
	And I wait for the page to reload
	Then I should see "Measurement X" in the table measurementPointsTable's "Description" column
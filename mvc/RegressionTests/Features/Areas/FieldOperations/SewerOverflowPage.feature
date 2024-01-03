Feature: SewerOverflowPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a state "nj" exists with name: "New Jersey", abbreviation: "NJ" 
	And a state "pa" exists with name: "Pennsylvania", abbreviation: "PA"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", maximum overflow gallons: "10", state: "nj"
	And a town abbreviation type "town" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a sewer stoppage type "one" exists with description: "stoppage1"
	And a sewer stoppage type "two" exists with description: "stoppage2"
	And a sewer clearing method "one" exists with description: "clearing"
	And a sewer overflow area "one" exists with description: "area"
	And a zone type "one" exists with description: "safe zone"
	And a coordinate "one" exists 
    And a waste water system "one" exists with id: 1, waste water system name: "Water System 1", operating center: "nj7"
	And waste water system: "one" exists in town: "one"
	And a sewer overflow "one" exists with operating center: "nj7", town: "one", street: "one", coordinate: "one", waste water system: "one"
    And a sewer overflow "two" exists with operating center: "nj7", town: "one", street: "one", coordinate: "one"
	And a body of water "one" exists with name: "crystal lake", operating center: "nj7"
    And sewer overflow discharge locations exist
	And sewer overflow types exist
	And sewer overflow causes exist
	And discharge weather related types exist
	And I am logged in as "admin"

Scenario: user can search for a sewer overflow
    When I visit the FieldOperations/SewerOverflow/Search page
    And I press Search
    Then I should see a link to the Show page for sewer overflow: "one"
    When I follow the Show link for sewer overflow "one"
    Then I should be at the Show page for sewer overflow: "one"

Scenario: user can view a sewer overflow
    When I visit the Show page for sewer overflow: "one"
    Then I should see a display for sewer overflow: "one"'s OperatingCenter

Scenario: user can add a sewer overflow
    When I visit the FieldOperations/SewerOverflow/New page
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I select sewer overflow discharge location "body of water" from the DischargeLocation dropdown
	And I select body of water "one"'s Name from the BodyOfWater dropdown
	And I enter "3" into the GallonsFlowedIntoBodyOfWater field
	And I select waste water system "one" from the WasteWaterSystem dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I enter today's date into the IncidentDate field
	And I enter "Ron Swanson" into the TalkedTo field
	And I enter "420" into the StreetNumber field
	And I select street "one" from the Street dropdown
	And I select street "two" from the CrossStreet dropdown
	And I enter "1" into the GallonsOverflowedEstimated field
	And I enter "456" into the EnforcingAgencyCaseNumber field
	And I enter today's date into the CallReceived field
	And I enter today's date into the CrewArrivedOnSite field
	And I enter today's date into the SewageContained field
	And I enter today's date into the StoppageCleared field
	And I enter today's date into the WorkCompleted field
	And I enter today's date into the IncidentDate field
	And I enter "1" into the SewageRecoveredGallons field
	And I select sewer overflow type "sso" from the OverflowType dropdown
	And I select sewer overflow cause "grease" from the OverflowCause dropdown
	And I select discharge weather related type "wet" from the WeatherType dropdown
	And I enter "storm catch" into the LocationOfStoppage field
	And I select sewer clearing method "one" from the SewerClearingMethod dropdown
	And I select "Yes" from the OverflowCustomers dropdown
	And I select sewer overflow area "one" from the AreaCleanedUpTo dropdown
	And I select zone type "one" from the ZoneType dropdown
	And I press Save
    And I wait for the page to reload
	Then I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for Town with town "one"
	And I should see a display for Coordinate with "40.32246702, -74.1481018"
	And I should see a display for CreatedBy with "admin"
	And I should see a display for CreatedAt with today's date
	And I should see a display for IncidentDate with today's date
	And I should see a display for StreetNumber with "420"
	And I should see a display for Street with street "one"
	And I should see a display for CrossStreet with street "two"
	And I should see a display for GallonsOverflowedEstimated with "1"
	And I should see a display for GallonsFlowedIntoBodyOfWater with "3"
	And I should see a display for EnforcingAgencyCaseNumber with "456"
	And I should see a display for CallReceived with today's date
	And I should see a display for CrewArrivedOnSite with today's date
	And I should see a display for SewageContained with today's date
	And I should see a display for StoppageCleared with today's date
	And I should see a display for WorkCompleted with today's date
	And I should see a display for BodyOfWater with body of water "one"
	And I should see a display for LocationOfStoppage with "storm catch"
	And I should see a display for SewerClearingMethod with sewer clearing method "one"
	And I should see a display for OverflowCustomers with "Yes"
	And I should see a display for AreaCleanedUpTo with sewer overflow area "one"
	And I should see a display for ZoneType with zone type "one"

Scenario: user can edit a sewer overflow
    When I visit the Edit page for sewer overflow: "one"
	And I enter today's date into the IncidentDate field
	And I enter "Ron Swanson" into the TalkedTo field
	And I enter "420" into the StreetNumber field
	And I select street "one" from the Street dropdown
	And I select street "two" from the CrossStreet dropdown
	And I enter "1" into the GallonsOverflowedEstimated field
	And I enter "456" into the EnforcingAgencyCaseNumber field
	And I enter today's date into the CallReceived field
	And I enter today's date into the CrewArrivedOnSite field
	And I enter today's date into the SewageContained field
	And I enter today's date into the StoppageCleared field
	And I enter today's date into the WorkCompleted field
	And I enter today's date into the IncidentDate field
	And I enter "1" into the SewageRecoveredGallons field
	And I select sewer overflow discharge location "body of water" from the DischargeLocation dropdown
	And I select body of water "one"'s Name from the BodyOfWater dropdown
	And I select sewer overflow type "sso" from the OverflowType dropdown
	And I select sewer overflow cause "grease" from the OverflowCause dropdown
	And I select discharge weather related type "wet" from the WeatherType dropdown
	And I enter "storm catch" into the LocationOfStoppage field
	And I select sewer clearing method "one" from the SewerClearingMethod dropdown
	And I select "Yes" from the OverflowCustomers dropdown
	And I select sewer overflow area "one" from the AreaCleanedUpTo dropdown
	And I select zone type "one" from the ZoneType dropdown
	And I enter "3" into the GallonsFlowedIntoBodyOfWater field
    And I press Save
    Then I should be at the Show page for sewer overflow: "one"
    And I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
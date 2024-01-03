Feature: Npdes Regulator Inspections

Background:
	Given the test flag "document page size 5" exists
	And an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
    And an asset type "sewer opening" exists with description: "sewer opening"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a state "nj" exists
	And a town "one" exists with name: "Loch Arbour", state: "nj"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a waste water system "one" exists with id: 1, waste water system name: "Water System 1", operating center: "nj7"
    And waste water system: "one" exists in town: "one"
	And a role "regulator-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "regulator-inspections-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a sewer opening type "regulator" exists with description: "Npdes Regulator"
	And a sewer opening "one" exists with operating center: "nj7", town: "one", street: "one", opening suffix: "123", sewer opening type: "regulator", waste water system: "one"
	And a sewer opening "two" exists with operating center: "nj7", town: "one", street: "two", opening suffix: "456", sewer opening type: "regulator"
	And a sewer opening "three" exists with operating center: "nj7", town: "one", street: "two", opening suffix: "789", sewer opening type: "regulator"
	And npdes regulator inspection form answer types exist
	And a npdes regulator inspection "one" exists with sewer opening: "one", arrival date time: "1/1/2023", departure date time: "1/1/2023"
	And a npdes regulator inspection "two" exists with sewer opening: "one", arrival date time: "2/1/2023", departure date time: "2/1/2023", is discharge present: "true"
	And a npdes regulator inspection "three" exists with sewer opening: "one", arrival date time: "3/1/2023", departure date time: "3/1/2023"
	And a npdes regulator inspection "four" exists with sewer opening: "two", arrival date time: "4/1/2023", departure date time: "4/1/2023"
	And a npdes regulator inspection "five" exists with sewer opening: "one", arrival date time: "5/1/2023", departure date time: "5/1/2023"
	And a npdes regulator inspection "six" exists with sewer opening: "one", arrival date time: "6/1/2023", departure date time: "6/1/2023"
	And a npdes regulator inspection "seven" exists with sewer opening: "one", arrival date time: "7/1/2023", departure date time: "7/1/2023"

Scenario: user can view an npdes regulator inspection
	Given I am logged in as "user"
    When I visit the Show page for npdes regulator inspection "one"
    Then I should see a display for npdes regulator inspection: "one"'s DepartureDateTime
    And I should see a display for npdes regulator inspection: "one"'s NpdesRegulatorInspectionType

Scenario: user can edit an npdes regulator inspection
	Given I am logged in as "user"
	When I visit the Show page for npdes regulator inspection "one"
	And I follow "Edit"
	Then I should be at the Edit page for npdes regulator inspection "one"
	When I press Save
	And I wait for the page to reload
	Then I should be at the Show page for npdes regulator inspection "one"

Scenario: add should not be visible in action bar 
	Given I am logged in as "admin"
	And I am at the FieldOperations/NpdesRegulatorInspection/Search page
	Then I should not see the "new" button in the action bar

Scenario: delete should not be visible in action bar 
	Given I am logged in as "admin"
	And I am at the Show page for npdes regulator inspection "one"
	Then I should not see "Delete"

Scenario: user should see a bunch of stuff on the Npdes Regulator Inspections Show page
	Given I am logged in as "user"
	When I visit the Show page for npdes regulator inspection "one"
	Then I should see a link to the Show page for sewer opening "one"
	And I should see a display for SewerOpening_LocationDescription with "description of location" 
	And I should see a display for SewerOpening_OutfallNumber with "007"
	And I should see a display for ArrivalDateTime with "1/1/2023 12:00:00 AM"
	And I should see a display for DepartureDateTime with "1/1/2023 12:00:00 AM"
	And I should see a display for NpdesRegulatorInspectionType with "STANDARD"
	And I should see a display for HasInfiltration with "No"
	And I should see a display for OutfallCondition with "Outfall Condition Description"
	And I should see a display for GateStatusAnswerType with "Yes"
	And I should see a display for BlockCondition with "OUT"
	And I should see a display for IsDischargePresent with "No"
	And I should see a display for HasFlowMeterMaintenanceBeenPerformed with "No"
	And I should see a display for Remarks with "Looks like a remark to me"
	When I follow "Edit"
	And I select npdes regulator inspection form answer type "n/a" from the GateStatusAnswerType dropdown
	And I press Save
	Then I should see a display for GateStatusAnswerType with "n/a"

Scenario: User can search for an existing Npdes regulator inspection
    Given I am logged in as "user"
    When I visit the FieldOperations/NpdesRegulatorInspection/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should see a link to the Show page for npdes regulator inspection "one"

Scenario: User can search for Npdes regulator inspections based on operating center and is discharge present
    Given I am logged in as "user"
    When I visit the FieldOperations/NpdesRegulatorInspection/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select "Yes" from the IsDischargePresent dropdown
    And I press Search
    Then I should see a link to the Show page for npdes regulator inspection "two"

Scenario: User can search for an existing Npdes regulator inspection based on DepartureDateTime
    Given I am logged in as "user"
    When I visit the FieldOperations/NpdesRegulatorInspection/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select ">" from the DepartureDateTime_Operator dropdown
	And I enter "01/01/2023" into the DepartureDateTime_End field
    And I press Search
    Then I should see a link to the Show page for npdes regulator inspection "two"
    And I should not see a link to the Show page for npdes regulator inspection "one"

Scenario: user can access and view the Npdes Regulator Inspections report
	Given I am logged in as "user"
	And I am at the Reports/NpdesRegulatorInspection/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I select "2023" from the Year dropdown
	And I press ViewReport
	Then I should be at the Reports/NpdesRegulatorInspection page
	And I should see the following values in the results table
	| Inspection Id | Town			| WWSID				         | Permit Number | Location Description	    | Outfall Number | Departure Date Inspected | Inspected By | Inspection Type | Block Condition | Discharge Present | Weather Related           | Rainfall Estimate | Body Of Water | Discharge Flow | Discharge Cause				| Discharge Duration | Remarks						|
	| 1				| Loch Arbour   | NJ7WW0001 - Water System 1 | *Permit*		 | description of location  | 007            | 1/1/2023 12:00:00 AM     | *user*       | STANDARD	     | OUT			   | No				   | *WeatherConditionFactory* | 0				   | Blue Water	   | 0              | Discharge Cause Description	| 0					 | Looks like a remark to me	|

Scenario: user can create a new work order
	Given I am logged in as "admin"
	When I visit the New page for npdes regulator inspection "one"
	Then I should be at the New page for npdes regulator inspection "one"
	And I should see "Create New Work Order"
	When I follow "Create New Work Order"
	And I wait for the page to reload
	Then I should be at the New page for npdes regulator inspection "one"

Scenario: user can edit the arrival and departure date time and enter the twilight zone
	Given I am logged in as "user"
	When I go to the Show page for npdes regulator inspection "one"
	And I follow "Edit"
	Then I should be at the Edit page for npdes regulator inspection "one"
	And I should see the ArrivalDateTime field
	When I enter "1/1/2023 7:00:00 AM" into the ArrivalDateTime field
	And I enter "1/1/2023 10:00:00 AM" into the DepartureDateTime field
	And I press Save
	Then I should be at the Show page for npdes regulator inspection "one"
	And I should see a display for ArrivalDateTime with "1/1/2023 7:00:00 AM"
	And I should see a display for DepartureDateTime with "1/1/2023 10:00:00 AM"

Scenario: user can export all records of the Npdes Regulator Inspections report
	Given I am logged in as "user"
	And I am at the FieldOperations/NpdesRegulatorInspection/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I press Search
	Then I should be at the FieldOperations/NpdesRegulatorInspection page
	Then I should see the following values in the results table
	|      |         | Sewer Opening | Operating Center | Town        | Inspected By  | Departure Date/Time  | Sewer Opening Type | Inspection Type | Discharge Present | Outfall Number | Body Of Water |
	| View | Inspect | MSC-6231      | NJ7 - Shrewsbury | Loch Arbour | *user*        | 1/1/2023 12:00:00 AM | NPDES REGULATOR    | STANDARD        | No                | 007            | Blue Water    |
	| View | Inspect | MSC-6231      | NJ7 - Shrewsbury | Loch Arbour | *user*        | 2/1/2023 12:00:00 AM | NPDES REGULATOR    | STANDARD        | Yes               | 007            | Blue Water    |
	| View | Inspect | MSC-6231      | NJ7 - Shrewsbury | Loch Arbour | *user*        | 3/1/2023 12:00:00 AM | NPDES REGULATOR    | STANDARD        | No                | 007            | Blue Water    |
	| View | Inspect | MSC-6231      | NJ7 - Shrewsbury | Loch Arbour | *user*        | 4/1/2023 12:00:00 AM | NPDES REGULATOR    | STANDARD        | No                | 007            | Blue Water    |
	| View | Inspect | MSC-6231      | NJ7 - Shrewsbury | Loch Arbour | *user*        | 5/1/2023 12:00:00 AM | NPDES REGULATOR    | STANDARD        | No                | 007            | Blue Water    |
	When I follow "2"
	Then I should see the following values in the results table
	|      |         | Sewer Opening | Operating Center | Town        | Inspected By | Departure Date/Time  | Sewer Opening Type | Inspection Type | Discharge Present | Outfall Number | Body Of Water |
	| View | Inspect | MSC-6231		 | NJ7 - Shrewsbury | Loch Arbour | *user*       | 6/1/2023 12:00:00 AM | NPDES REGULATOR | STANDARD | No | 007 | Blue Water |
	| View | Inspect | MSC-6231      | NJ7 - Shrewsbury | Loch Arbour | *user*       | 7/1/2023 12:00:00 AM | NPDES REGULATOR | STANDARD | No | 007 | Blue Water |

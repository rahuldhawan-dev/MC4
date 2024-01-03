Feature: NpdesRegulatorsDueInspectionReport

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "regulatorsdueread" exists with action: "Read", module: "ProductionPlannedWork"
	And a role "hydrantinspection-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a town "one" exists with name: "Loch Arbour"
	And a town "two" exists with name: "Ann Arbor"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And an asset status "active" exists with description: "ACTIVE"
	# it needs sewer opening type of NPDES Regulator, but that's the default
	And a sewer opening "one" exists with operating center: "nj7", town: "one", status: "active"
	And a sewer opening "two" exists with operating center: "nj7", town: "two", status: "active"
	And an npdes regulator inspection "inRange" exists with sewer opening: "one", arrival date time: "1/1/2023", departure date time: "1/1/2023"
	And an npdes regulator inspection "outOfRange" exists with sewer opening: "one", arrival date time: "2/1/2023", departure date time: "2/1/2023"
	
	# only regulators with NO npdes regulator inspections in date range are taken
Scenario: User can search npdes regulators due inspection by operating center and inspection date 
	Given I am logged in as "user"
	And I am at the Reports/NpdesRegulatorsDueInspectionReport/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "12/30/2022" into the DepartureDateTime_Start field
	And I enter "1/3/2023" into the DepartureDateTime_End field
	When I press Search
	Then I should see the following values in the results table 
	| Operating Center | Town      | Status | Count |
	| NJ7              | Ann Arbor | ACTIVE | 1     |
	
Scenario: User clicks on the town and goes to the index listing the npdes regulators in the count 
	Given I am logged in as "user"
	And I am at the Reports/NpdesRegulatorsDueInspectionReport/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "6/30/2022" into the DepartureDateTime_Start field
	And I enter "7/3/2022" into the DepartureDateTime_End field
	When I press Search
	And I click the 2nd row of results
	Then I should be at the FieldOperations/NpdesRegulatorsDueInspection page
	And I should see the following values in the results table 
	|      |         | Opening Number       | Town        | NPDES Permit Number | Outfall Number | Body Of Water | Location Description    | Opening Status |
	| View | Inspect | MSC-6231             | Loch Arbour | *Permit*            | 007            | Blue Water    | description of location | ACTIVE |
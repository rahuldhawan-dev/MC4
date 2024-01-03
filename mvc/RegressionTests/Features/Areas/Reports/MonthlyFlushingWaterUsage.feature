Feature: MonthlyFlushingWaterUsage

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "hydrantinspection-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a town "two" exists with name: "Super Town"
	And operating center: "nj7" exists in town: "two" with abbreviation: "ST"
	And a asset status "active" exists with description: "ACTIVE"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant billing "private" exists with description: "Private"
	And a hydrant "one" exists with operating center: "nj7", hydrant billing: "public", town: "one"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public", town: "two"
	And a hydrant exists with operating center: "nj7", hydrant billing: "private", town: "two"
	And a hydrant inspection exists with hydrant: "one", date inspected: "1/3/2015", minutes flowed: 10, GPM: 10
	And a hydrant inspection exists with hydrant: "one", date inspected: "2/1/2015", minutes flowed: 20, GPM: 20

Scenario: User can search and see how many gallons were flushed each month for a year
	Given I am logged in as "user" 
	And I am at the Reports/MonthlyFlushingWaterUsage/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I enter 2015 into the Year field
	And I press Search 
	Then I should see "Results for NJ7 - Shrewsbury in 2015" in the resultsTitle element
	And I should see the following values in the results table
	| Month | Total Gallons |
	| Jan   | 100           |
	| Feb   | 400           |
	| Mar   | 0             |
	| Apr   | 0             |
	| May   | 0             |
	| Jun   | 0             |
	| Jul   | 0             |
	| Aug   | 0             |
	| Sep   | 0             |
	| Oct   | 0             |
	| Nov   | 0             |
	| Dec   | 0             |

Scenario: User should see validation when searching
	Given I am logged in as "user" 
	And I am at the Reports/MonthlyFlushingWaterUsage/Search page
	When I press Search 
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."

Scenario: Year should be populated with the current year by default
	Given I am logged in as "user" 
	And I am at the Reports/MonthlyFlushingWaterUsage/Search page
	Then I should see "this year" in the Year field 

Feature: HydrantMonthlyReportsPage
In order to view monthly hydrant reports
	As a user
	I want to be able to view them without error

Background:
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", zone start year: "2015"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", zone start year: "2015"
	And a user "user" exists with username: "user"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a town "two" exists with name: "Lakewood"
	And operating center: "nj4" exists in town: "two" with abbreviation: "ST"
	#And a street "one" exists with town: "one"
	#And a street "two" exists with town: "one", full st name: "Easy St"
	And a asset status "active" exists with description: "ACTIVE"
	And a recurring frequency unit "year" exists with description: "Year"
	And a hydrant billing "muni" exists with description: "Municipal"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant billing "private" exists with description: "Private"
	And a hydrant inspection type "flush" exists with description: "FLUSH"
	And a hydrant inspection type "flushinspect" exists with description: "INSPECT/FLUSH"
	And a hydrant inspection type "inspect" exists with description: "INSPECT"
	And a hydrant "one" exists with operating center: "nj7", hydrant billing: "public", town: "one", status: "active", zone: "1", date installed: "1/1/2000", inspection frequency unit: "year", inspection frequency: "1"
	And a hydrant "two" exists with operating center: "nj7", hydrant billing: "public", town: "one", status: "active", zone: "1", date installed: "1/1/2000", inspection frequency unit: "year", inspection frequency: "1"
	And a hydrant "three" exists with operating center: "nj7", hydrant billing: "private", town: "one", status: "active", zone: "1", date installed: "1/1/2000", inspection frequency unit: "year", inspection frequency: "1"
	And a hydrant "four" exists with operating center: "nj4", hydrant billing: "public", town: "two", status: "active", zone: "1", date installed: "1/1/2000", inspection frequency unit: "year", inspection frequency: "1"
	And a hydrant inspection "one" exists with hydrant: "one", date inspected: "1/3/2015", minutes flowed: 10, GPM: 10, hydrant inspection type: "flush"
	And a hydrant inspection "two" exists with hydrant: "two", date inspected: "2/3/2015", minutes flowed: 10, GPM: 10, hydrant inspection type: "flushinspect"
	And a hydrant inspection "three" exists with hydrant: "three", date inspected: "3/3/2015", minutes flowed: 10, GPM: 10, hydrant inspection type: "inspect"
	And a hydrant inspection "four" exists with hydrant: "four", date inspected: "6/3/2015", minutes flowed: 10, GPM: 10, hydrant inspection type: "flush"

Scenario: user can view the hydrant inspections by month report
	Given I am logged in as "user"
	When I visit the Reports/KPIHydrantInspectionByMonth/Search page
	And I press Search
	Then I should see the validation message "The Year field is required."
	When I select "2015" from the Year dropdown
	And I press Search
	Then I should see the following values in the hydrant-inspections-table table
	| Operating Center | Work Description | Year | Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec | Total Required Inspections | Total Required | % Complete |
	| NJ4              | FLUSH            | 2015 | 0   | 0   | 0   | 0   | 0   | 1   | 0   | 0   | 0   | 0   | 0   | 0   | 1                          |                |            |
	| NJ4              | Total            | 2015 | 0   | 0   | 0   | 0   | 0   | 1   | 0   | 0   | 0   | 0   | 0   | 0   | 1                          | 1              | 100.00 %   |
	| NJ7              | FLUSH            | 2015 | 1   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1                          |                |            |
	| NJ7              | INSPECT          | 2015 | 0   | 0   | 1   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1                          |                |            |
	| NJ7              | INSPECT/FLUSH    | 2015 | 0   | 1   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1                          |                |            |
	| NJ7              | Total            | 2015 | 1   | 1   | 1   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 3                          | 3              | 100.00 %   |
	| Total            |                  | 2015 | 1   | 1   | 1   | 0   | 0   | 1   | 0   | 0   | 0   | 0   | 0   | 0   | 4                          | 4              | 100.00 %   |

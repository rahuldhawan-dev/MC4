Feature: SewerMainCleaningFootagePage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town "one" exists with name: "Loch Arbour"
    And a street "one" exists with town: "one", is active: true
	And a street "two" exists with town: "one", is active: true
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
    And sewer main inspection types exist
	And a sewer main cleaning "one" exists with operating center: "nj7", town: "one", street: "one", inspection type: "acoustic", date: "12/7/2023 11:10 PM"
    And a sewer main cleaning "two" exists with operating center: "nj7", town: "one", street: "two", inspection type: "smoke test", date: "12/8/2023 11:13 PM"
	And a sewer main cleaning "three" exists with operating center: "nj7", town: "one", street: "one", inspection type: "acoustic", date: "11/18/2023 11:12 PM"
    And a sewer main cleaning "four" exists with operating center: "nj7", town: "one", street: "two", inspection type: "smoke test", date: "10/25/2023 11:11 PM"

Scenario: admin can search for a sewer main cleaning footage report
    Given I am logged in as "admin"
    When I visit the Reports/SewerMainCleaningFootage/Search page
    And I press Search
	Then I should see the validation message "The Year field is required."
	When I select "2023" from the Year dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I select sewer main inspection type "smoke test" from the InspectionType dropdown
	And I press Search
	Then I should see the following values in the results table
	| Year | Operating Center | Town        | Inspection Type | Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec   | Total |
	| 2023 | NJ7              | Loch Arbour | SMOKE TEST      | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 250 | 0   | 250	| 500	|
    When I visit the Reports/SewerMainCleaningFootage/Search page
	And I select "2023" from the Year dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I select sewer main inspection type "acoustic" from the InspectionType dropdown
	And I press Search
	Then I should see the following values in the results table
	| Year | Operating Center | Town        | Inspection Type | Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec   | Total |
	| 2023 | NJ7              | Loch Arbour | ACOUSTIC        | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 250 | 250	| 500	|

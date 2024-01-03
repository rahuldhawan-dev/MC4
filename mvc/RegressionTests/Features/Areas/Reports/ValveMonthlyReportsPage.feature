Feature: ValveMonthlyReportsPage
	In order to view monthly valve reports
	As a user
	I want to be able to view them without error

Background:
	Given a user "user" exists with username: "user"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a valve size "large" exists with size: 12
	And a valve size "small" exists with size: 1
	And a valve billing "public" exists with description: "Public"
	And a valve billing "oandm" exists with description: "O & M"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a valve zone "one" exists
	And a valve zone "two" exists
	And a valve zone "three" exists
	And a valve zone "four" exists
	And a valve zone "five" exists
	And a valve zone "six" exists
	And a valve zone "seven" exists
    And a valve "one" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve size: "small", status: "active", valve billing: "public", date installed: "1/1/2001"
	And a valve "two" exists with valve zone: "five", town: "one", street: "one", valve number: "VLA-2", valve suffix: 2, valve size: "small", status: "active", valve billing: "public", date installed: "1/1/2001"
    And a valve "three" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve size: "large", status: "active", valve billing: "public", date installed: "1/1/2001"
	And a valve "four" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve size: "small", status: "active", valve billing: "public", date installed: "1/1/2001"
	And a valve "five" exists with valve zone: "two", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve size: "small", status: "active", valve billing: "public", date installed: "1/1/2001"
	And a valve inspection "one" exists with valve: "one", minimum required turns: 4, date inspected: "12/8/2011 11:07 PM", inspected: "true", inspected by: "user"
	And a valve inspection "two" exists with valve: "two", minimum required turns: 4, date inspected: "12/8/2011 11:07 PM", inspected: "true", inspected by: "user"
	And a valve inspection "three" exists with valve: "three", minimum required turns: 4, date inspected: "12/8/2011 11:07 PM", inspected: "true", inspected by: "user"
	And a valve inspection "five" exists with valve: "five", minimum required turns: 4, date inspected: "12/8/2011 11:07 PM", inspected: "true", inspected by: "user"
	And a valve inspection "oneA" exists with valve: "one", minimum required turns: 4, date inspected: "12/7/2011 11:07 PM", inspected: "false", inspected by: "user"
	And a valve inspection "threeA" exists with valve: "three", minimum required turns: 4, date inspected: "12/1/2011 11:07 PM", inspected: "false", inspected by: "user"

Scenario: user can view the valve inspections by month report
	Given I am logged in as "user"
	When I visit the Reports/ValveInspectionsByMonth/Search page
	And I press Search
	Then I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The Year field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select "2011" from the Year dropdown
	And I press Search
	Then I should see the following values in the valve-inspections-table table
	| Size Range | Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec | Total Performed | Total Required | Total %  |
	| < 12       | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 2   | 2               | 3              | 66.67 %  |
	| >= 12      | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1   | 1               | 1              | 100.00 % |
	| Total      | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 3   | 3               | 4              | 75.00 %  |

Scenario: user can view the valves operated by month report
	Given I am logged in as "user"
	When I visit the Reports/ValvesOperatedByMonth/Search page
	And I press Search
	Then I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The Year field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select "2011" from the Year dropdown
	And I press Search
	Then I should see the following values in the valves-operated-table table
	| Size Range | Inspected| Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec | Total |
	| < 12       | YES      | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 3   | 3     |
	| < 12       | NO       | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1   | 1     |
	| < 12       | Total    | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 4   | 4     |
	| >= 12      | YES      | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1   | 1     |
	| >= 12      | NO       | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1   | 1     |
	| >= 12      | Total    | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 2   | 2     |
	| Total      |          | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 6   | 6     |

Scenario: user can view the required valves operated by month report
	Given I am logged in as "user"
	When I visit the Reports/RequiredValvesOperatedByMonth/Search page
	And I press Search
	Then I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The Year field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select "2011" from the Year dropdown
	And I press Search
	Then I should see the following values in the valves-required-table table
	| Size Range | Inspected| Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec | Total |
	| < 12       | YES      | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 2   | 2     |
	| < 12       | NO       | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1   | 1     |
	| < 12       | Total    | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 3   | 3     |
	| >= 12      | YES      | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1   | 1     |
	| >= 12      | NO       | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 1   | 1     |
	| >= 12      | Total    | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 2   | 2     |
	| Total      |          | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 5   | 5     |

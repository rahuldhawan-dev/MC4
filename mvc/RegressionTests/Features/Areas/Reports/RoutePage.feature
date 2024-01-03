Feature: RoutePage
	In order to view the routes in an operating center
	As a user
	I want to be told search and view the routes in an operating center

Background: 
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"

Scenario: User can search and view the valve report
	Given a valve zone "one" exists with description: "1"
	And a valve zone "two" exists with description: "2"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a valve size "one" exists with size: 12
	And a valve size "two" exists with size: 1
	And a valve billing "public" exists with description: "Public"
	And a valve billing "O & M" exists with description: "O & M"
    And a valve "one" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve size: "one", status: "active", valve billing: "public", operating center: "nj7", route: "1"
	And a valve "two" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-2", valve suffix: 2, valve size: "two", status: "retired", valve billing: "O & M", operating center: "nj7", route: "1"
    And a valve "three" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve size: "two", status: "retired", valve billing: "O & M", operating center: "nj7", route: "2"
	And I am logged in as "user"
	And I am at the Reports/ValveRoute/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
	And I press Search
	Then I should not see "No results matched your query."
	And I should see the following values in the results table
	| Operating Center | Town        | Route | Valve Status | Total |
	| NJ7 - Shrewsbury | Loch Arbour | 1     | ACTIVE       | 1     |
	| NJ7 - Shrewsbury | Loch Arbour | 1     | RETIRED      | 1     |
	| NJ7 - Shrewsbury | Loch Arbour | 2     | RETIRED      | 1     |

Scenario: User can search and view the hydrant report
	Given a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a hydrant billing "one" exists with description: "Public"
	And a hydrant billing "two" exists with description: "Municipal"
	And a hydrant "one" exists with town: "one", street: "one", hydrant number: "HLA-1", hydrant suffix: 1, status: "active", hydrant billing: "one", operating center: "nj7", route: "1"
	And a hydrant "two" exists with town: "one", street: "one", hydrant number: "HLA-2", hydrant suffix: 2, status: "retired", hydrant billing: "two", operating center: "nj7", route: "1"
    And a hydrant "three" exists with town: "one", street: "one", hydrant number: "HLA-3", hydrant suffix: 3, status: "retired", hydrant billing: "two", operating center: "nj7", route: "2"
	And I am logged in as "admin"
    And I am at the Reports/HydrantRoute/Search page
	When I press Search
	Then I should not see "No results matched your query."
	And I should see the following values in the results table
	| Operating Center | Town        | Route | Hydrant Status | Total |
	| NJ7 - Shrewsbury | Loch Arbour | 1     | ACTIVE       | 1     |
	| NJ7 - Shrewsbury | Loch Arbour | 1     | RETIRED      | 1     |
	| NJ7 - Shrewsbury | Loch Arbour | 2     | RETIRED      | 1     |

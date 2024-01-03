Feature: MunicipalValveZonePage

Background: 
	Given a user "user" exists with username: "user"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a valve size "one" exists with size: 12
	And a valve size "two" exists with size: 1
	And a valve billing "public" exists with description: "Public"
	And a valve billing "O & M" exists with description: "O & M"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a valve zone "one" exists with description: "1"
	And a valve zone "two" exists with description: "2"
    And a valve "one" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve size: "one", status: "active", valve billing: "public", operating center: "nj7"
	And a valve "two" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-2", valve suffix: 2, valve size: "two", status: "retired", valve billing: "O & M", operating center: "nj7"
    And a valve "three" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve size: "two", status: "retired", valve billing: "O & M", operating center: "nj7"
	And a municipal valve zone "one" exists with operating center: "nj7", town: "one", small valve zone: "one", large valve zone: "two"

Scenario: User can search and view the report
	Given I am logged in as "user"
	And I am at the Reports/MunicipalValveZone/Search page
	When I press Search
	Then I should not see "No results matched your query."
	And I should see the following values in the results table
	| Operating Center | Town        | Small Valves | Large Valves | Small Valve Zone | Large Valve Zone |
	| NJ7              | Loch Arbour | 1            | 0            | 1                | 2                |

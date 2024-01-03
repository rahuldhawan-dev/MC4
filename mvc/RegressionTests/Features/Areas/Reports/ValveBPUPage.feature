﻿Feature: ValveBPUPage

Background: 
	Given a user "user" exists with username: "user"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
	And a valve size "one" exists with size: 12
	And a valve size "two" exists with size: 1
	And a valve billing "one" exists with description: "Public"
	And a valve billing "two" exists with description: "O & M"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a valve zone "one" exists
	And a valve zone "two" exists
    And a valve "one" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve size: "one", status: "active", valve billing: "one"
	And a valve "two" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-2", valve suffix: 2, valve size: "two", status: "retired", valve billing: "two"
    And a valve "three" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve size: "two", status: "retired", valve billing: "two"

Scenario: User can search and view break down of active hydrants and their operating center and billing info
	Given I am logged in as "user"
	And I am at the Reports/ValveBPU/Search page
	When I press Search
	Then I should not see "No results matched your query."
	And I should see the following values in the results table
	| Operating Center | Valve Billing | Valve Status | Size Range  | Total |
	| NJ7              | O & M         | RETIRED      | < 12        | 2     |
	| NJ7              | Public        | ACTIVE       | >= 12       | 1     |

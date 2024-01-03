Feature: ValveImageLink

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a valve "one" exists with operating center: "nj7", valve number: "VAB-100", s a p equipment id: "1234512345"
	And a valve "two" exists with operating center: "nj7", valve number: "VAB-101", s a p equipment id: "1234512346"
	And a valve "three" exists
	And a valve image "one" exists with valve: "one"
	And a valve image "two" exists with valve: "two"

Scenario: user can access and view the report like a boss
	Given I am logged in as "user"
	And I am at the Reports/ValveImageLink/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should be at the Reports/ValveImageLink page
	And I should see the following values in the results table
	| Operating Center | Valve   | SAP Equipment Number | Address                                                                   |
	| NJ7 - Shrewsbury | VAB-100 | 1234512345           | https://mapcall.amwater.com/modules/mvc/FieldOperations/ValveImage/Show/1.pdf |
	| NJ7 - Shrewsbury | VAB-101 | 1234512346           | https://mapcall.amwater.com/modules/mvc/FieldOperations/ValveImage/Show/2.pdf |

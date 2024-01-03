Feature: TapImageLink

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a tap image "one" exists with premise number: "1234512345", operating center: "nj7"
	And a tap image "two" exists with premise number: "1234512346", operating center: "nj7"
	And a tap image "three" exists with premise number: "12345", operating center: "nj7"

Scenario: user can access and view the report like a boss
	Given I am logged in as "user"
	And I am at the Reports/TapImageLink/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should be at the Reports/TapImageLink page
	And I should see the following values in the results table
	| Operating Center | Premise Number | Address                                                                 |
	| NJ7 - Shrewsbury | 1234512345     | https://mapcall.amwater.com/modules/mvc/FieldOperations/TapImage/Show/1.pdf |
	| NJ7 - Shrewsbury | 1234512346     | https://mapcall.amwater.com/modules/mvc/FieldOperations/TapImage/Show/2.pdf |

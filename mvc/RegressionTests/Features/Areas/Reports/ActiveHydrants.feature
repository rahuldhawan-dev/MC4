Feature: Active Hydrants

Background: 
	Given a user "user" exists with username: "user"
	And a asset status "active" exists with description: "ACTIVE"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant billing "private" exists with description: "Private"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public"
	And a hydrant exists with operating center: "nj7", hydrant billing: "private"

Scenario: User can search and view break down of active hydrants and their operating center and billing info
	Given I am logged in as "user"
	And I am at the Reports/ActiveHydrant/Search page
	When I press Search
	Then I should see the following values in the results table
	| Operating Center | Hydrant Billing | Count |
	| NJ7              | Private         | 1     |
	| NJ7              | Public          | 2     |

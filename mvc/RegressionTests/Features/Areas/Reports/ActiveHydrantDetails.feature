Feature: ActiveHydrantDetails

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a town "two" exists with name: "Super Town"
	And operating center: "nj7" exists in town: "two" with abbreviation: "ST"
	And a asset status "active" exists with description: "ACTIVE"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant billing "private" exists with description: "Private"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public", town: "one"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public", town: "two"
	And a hydrant exists with operating center: "nj7", hydrant billing: "private", town: "two"

Scenario: User can search and view break down of active hydrants and their operating center and billing info
	Given I am logged in as "user"
	And I am at the Reports/ActiveHydrantDetail/Search page
	When I press Search
	Then I should see the following values in the results table
	| Operating Center | Town        | Hydrant Billing | Count |
	| NJ7              | Loch Arbour | Public          | 1     |
	| NJ7              | Super Town  | Private         | 1     |
	| NJ7              | Super Town  | Public          | 1     |

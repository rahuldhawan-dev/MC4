Feature: HydrantsDueInspection

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "hydrantinspection-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a town "two" exists with name: "Super Town"
	And operating center: "nj7" exists in town: "two" with abbreviation: "ST"
	And a asset status "active" exists with description: "ACTIVE"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant billing "municipal" exists with description: "Municipal"
	And a hydrant "one" exists with operating center: "nj7", hydrant billing: "public", town: "one"
	And a hydrant "two" exists with operating center: "nj7", hydrant billing: "public", town: "two"
	And a hydrant "three" exists with operating center: "nj7", hydrant billing: "municipal", town: "two"
	# NOTE If you add a hydrant inspection here, the factory creates a hydrant with a generic town that
	#      you will see in the results table. This makes things confusing because it adds unexpected data
	#      that gets parsed.

Scenario: User can search hydrants due inspection by operating center 
	Given I am logged in as "user"
	And I am at the Reports/HydrantsDueInspection/Search page
	When I press Search
	Then I should see the following values in the results table 
	| Operating Center | Town        | Count |
	| NJ7              | Loch Arbour | 1     |
	| NJ7              | Super Town  | 1     | 
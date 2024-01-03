Feature: HydrantWorkOrdersByDescriptionPage

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a role "wo-useradmin" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a town "two" exists with name: "Super Town"
	And operating center: "nj7" exists in town: "two" with abbreviation: "ST"
	And a asset status "active" exists with description: "ACTIVE"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant billing "private" exists with description: "Private"
	And a hydrant "one" exists with operating center: "nj7", hydrant billing: "public", town: "one", hydrant suffix: "123", hydrant number: "QQ-123"
	And a hydrant "two" exists with operating center: "nj7", hydrant billing: "public", town: "two", hydrant suffix: "321", hydrant number: "QQ-321"
	And an asset type "valve" exists with description: "valve"
	And a work description "hydrant" exists with description: "hydrant repair"
	And a work order exists with hydrant: "one", work description: "hydrant"
	And a work order exists with hydrant: "two", work description: "hydrant", cancelled at: "11/11/2016"

Scenario: user can search for a hydrant with a work order
	Given I am logged in as "user"
	And I am at the Reports/HydrantWorkOrdersByDescription/Search page
	When I select work description "hydrant" from the HasWorkOrderWithWorkDescriptions dropdown
	And I press Search
	Then I should see the following values in the results table 
	| Hydrant Number | Operating Center | Town        | Hydrant Suffix |
	| QQ-123         | NJ7 - Shrewsbury | Loch Arbour | 123            |
	And I should see a link to the show page for hydrant: "one"
	And I should not see a link to the show page for hydrant: "two"
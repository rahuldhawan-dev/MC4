Feature: PublicHydrantTotalsReport

Background: 
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "hydrants-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a town "two" exists with name: "Super Town"
	And operating center: "nj7" exists in town: "two" with abbreviation: "ST"
	And a fire district "one" exists with premise number: "12345"
	And a fire district town "foo" exists with town: "one", fire district: "one"
	And a asset status "active" exists with description: "ACTIVE"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant billing "private" exists with description: "Private"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public", town: "one", hydrant suffix: "123", hydrant number: "LA-123", fire district: "one"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public", town: "one", hydrant suffix: "123", hydrant number: "LA-133", fire district: "one"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public", town: "two", hydrant suffix: "123", hydrant number: "ST-111"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public", town: "two", hydrant suffix: "123", hydrant number: "ST-112"
	And a hydrant exists with operating center: "nj7", hydrant billing: "public", town: "two", hydrant suffix: "123", hydrant number: "ST-113"
	# This one should never actually get counted since it's not public
	And a hydrant "notgonnacount" exists with operating center: "nj7", hydrant billing: "private", town: "one", hydrant suffix: "123", hydrant number: "QQ-124"

Scenario: User can search and see results
	Given I am logged in as "user"
	And I am at the Reports/PublicHydrantCount/Search page
	When I press Search
	Then I should see the following values in the results table 
	| Operating Center | Town        | Fire District         | Status | Premise Number | Total |
	| NJ7              | Loch Arbour | default district name | ACTIVE | 12345          | 2     |
	| NJ7              | Super Town  | default district name | ACTIVE | 987654321      | 3     |
	
	#| Operating Center | Town        | Fire District | Status | Premise Number | Total |
	#| NJ7              | Loch Arbour |               | ACTIVE | 12345          | 2     |
	#| NJ7              | Super Town  |               | ACTIVE |                | 3     |

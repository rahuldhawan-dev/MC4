Feature: AgedPendingAssetReport

Background: 
	Given a user "user" exists with username: "user"
	And a asset status "active" exists with description: "ACTIVE", is user admin only: "true"
	And a asset status "retired" exists with description: "RETIRED", is user admin only: "true"
	And a asset status "pending" exists with description: "PENDING", is user admin only: "true"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant inspection type "flush" exists with description: "FLUSH"
	And a hydrant tag status "tag" exists with description: "Tag!"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a valve zone "one" exists
	And a valve zone "two" exists
	And a valve billing "public" exists with description: "PUBLIC"
	And a hydrant "one" exists with town: "one", street: "one", status: "pending", created at: "9 days ago"
	And a hydrant "two" exists with town: "one", street: "one", status: "pending", created at: "90 days ago"
	And a hydrant "three1" exists with town: "one", street: "one", status: "pending", created at: "180 days ago"
	And a hydrant "three2" exists with town: "one", street: "one", status: "pending", created at: "360 days ago"
	And a hydrant "four" exists with town: "one", street: "one", status: "pending", created at: "361 days ago"
    And a valve "one" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve billing: "public", status: "pending", created at: "9 days ago"
	And a valve "two" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, valve billing: "public", status: "pending", created at: "90 days ago"
    And a valve "three" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve billing: "public", status: "pending", created at: "179 days ago"
	And a valve "four" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve billing: "public", status: "pending", created at: "180 days ago"
	And a valve "five" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve billing: "public", status: "pending", created at: "360 days ago"
	And a valve "six" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, valve billing: "public", status: "pending", created at: "361 days ago"
	And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"

Scenario: user can access and view the report like a boss
	Given I am logged in as "user"
	And I am at the Reports/AgedPendingAsset/Search page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should be at the Reports/AgedPendingAsset page
	And I should see the following values in the results table
	| Operating Center | Asset Type | 0 - 90 Days | 91 - 180 Days | 181 - 360 Days | > 360 Days | Total |
	| NJ7 - Shrewsbury | Valve      | 2           | 2             | 1              | 1          | 6     |
	| NJ7 - Shrewsbury | Hydrant    | 2           | 1             | 1              | 1          | 5     |

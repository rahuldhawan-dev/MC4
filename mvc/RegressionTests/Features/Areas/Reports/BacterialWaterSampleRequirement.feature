Feature: BacterialWaterSampleRequirement
	In order to view BactiSample Chlorine Highs and Lows
	As a user
	I want to be able to search for them through the site

Background: users exist
	Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with name: "New Jersey Again", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour"
	And a town "two" exists with name: "Allenhurst"
	And an operating center "nj7" exists with opcode: "NJ7"
	And a sample site status "active" exists with description: "Active"
	And public water supply statuses exist
	And a public water supply "one" exists with Identifier: "1234", status: "active", operating area: "north", january required bacterial water samples: "12", march required bacterial water samples: "21", december required bacterial water samples: "211"
	And a bacterial sample type "process control" exists with description: "Process Control"
	And a bacterial sample type "new main" exists with description: "New Main"
	And a sample site "one" exists with town: "one", sample site name: "Grateful Deli", public water supply: "one", operating center: "nj7", january required bacterial water samples: 12, march required bacterial water samples: 21, december required bacterial water samples: 211
	And a sample site "two" exists with town: "two", sample site name: "Cravings", public water supply: "one", operating center: "nj7"
	And a bacterial water sample "one" exists with sample site: "one", cl2 free: "0.16", cl2 total: "0.48", sample collection d t m: "01/01/2014", bacterial sample type: "process control"
	And a bacterial water sample "two" exists with sample site: "two", cl2 free: "0.26", cl2 total: "0.88", sample collection d t m: "02/01/2014", bacterial sample type: "new main"

Scenario: user should be able to search and get results
	Given I am logged in as "admin"
	When I visit the /Reports/BacterialWaterSampleRequirement/Search page
	And I select "2014" from the Year dropdown
	And I press Search
	Then I should see the following values in the results table
	| PWSID          | Type            | Year | Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec |
	| *1234 - north* | Process Control | 2014 | 1   |     |     |     |     |     |     |     |     |     |     |     |
	| *1234 - north* | New Main        | 2014 |     | 1   |     |     |     |     |     |     |     |     |     |     |
	| *1234 - north* | Requirement     | 2014 | 12  | 0   | 21  | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 0   | 211 |
	# The above Requirement line comes from voodoo magic in a repository method. It is not an actual bacterial sample type.
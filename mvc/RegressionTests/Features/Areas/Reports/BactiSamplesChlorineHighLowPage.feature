Feature: BactiSamplesChlorineHighLowPage
	In order to view BactiSample Chlorine Highs and Lows
	As a user
	I want to be able to search for them through the site

Background: users exist
	Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with name: "New Jersey Again", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour", county: "one", state: "one"
	And a town "two" exists with name: "Allenhurst", county: "one", state: "one"
	And public water supply statuses exist
	And a public water supply "one" exists with Identifier: "1234", operating area: "north", status: "active"
	And a sample site status "active" exists with description: "Active"
	And a sample site "one" exists with town: "one", sample site name: "Grateful Deli", public water supply: "one"
	And a sample site "two" exists with town: "two", sample site name: "Cravings", public water supply: "one"
	And a bacterial water sample "one" exists with sample site: "one", cl2 free: "0.16", cl2 total: "0.48", sample collection d t m: "01/01/2014"
	And a bacterial water sample "two" exists with sample site: "two", cl2 free: "0.26", cl2 total: "0.88", sample collection d t m: "02/01/2014"

Scenario: user should be able to search and get results
	Given I am logged in as "admin"
	When I visit the /Reports/BactiSamplesChlorineHighLow/Search page
	And I press Search
	Then I should see bacterial water sample "one"'s Cl2Free in the "Jan" column
	And I should see bacterial water sample "two"'s Cl2Free in the "Feb" column


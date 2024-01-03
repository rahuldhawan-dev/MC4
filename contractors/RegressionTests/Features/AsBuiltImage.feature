Feature: AsBuiltImage

Background: users exist
    Given an operating center "one" exists with opcntr: "NJX", opcntrname: "Shrewsburgh"
    And a contractor "one" exists with name: "one", operating center: "one"
    And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
	And a state "one" exists with name: "New Jersey Again", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour", county: "one", state: "one"
	And operating center: "one" exists in town: "one" 

Scenario: user receives a functional search page with cascading dropdowns
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	When I visit the /AsBuiltImage/Search page 
	And I select state "one" from the State dropdown
	And I select county "one" from the County dropdown
	Then I should see town "one"'s ShortName in the Town dropdown
	When I press Search
	Then I should not see "an error"

Scenario: user receives friendly error message when no results 
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	When I visit the /AsBuiltImage/Search page 
	And I select state "one" from the State dropdown
	And I select county "one" from the County dropdown
	And I press Search
    Then I should see the error message No results matched your query.

Scenario: user should be able to search for results in an operating center
	Given an as built image "one" exists with operating center: "one", town: "one", coordinates modified on: "01/01/2013 12:00:00"
	And an as built image "two" exists with operating center: "one", town: "one", coordinates modified on: "01/02/2013 12:00:00"
	And an as built image "three" exists with coordinates modified on: "01/03/2013 12:00:00"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	When I visit the /AsBuiltImage/Search page 
	And I select operating center "one" from the OperatingCenter dropdown
	And I press Search
    Then I should see the table-caption "Records found: 2"
	And I should see as built image "one"'s CoordinatesModifiedOn in the "Coordinates Modified On" column
	And I should see as built image "two"'s CoordinatesModifiedOn in the "Coordinates Modified On" column
	And I should not see as built image "three"'s CoordinatesModifiedOn in the "Coordinates Modified On" column



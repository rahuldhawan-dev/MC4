Feature: ValveImage

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
	When I visit the /ValveImage/Search page 
	And I select state "one" from the State dropdown
	And I select county "one" from the County dropdown
	Then I should see town "one"'s ShortName in the Town dropdown
	When I press Search
	Then I should not see "an error"

Scenario: user receives friendly error message when no results 
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	When I visit the /ValveImage/Search page 
	And I select state "one" from the State dropdown
	And I select county "one" from the County dropdown
	And I press Search
    Then I should see the error message No results matched your query.

Scenario: user should be able to search for results in an operating center
	Given an valve image "one" exists with operating center: "one", town: "one"
	And an valve image "two" exists with operating center: "one", town: "one"
	And an valve image "three" exists 
	And I am logged in as "admin@site.com", password: "testpassword#1"
	When I visit the /ValveImage/Search page 
	And I select operating center "one" from the OperatingCenter dropdown
	And I press Search
    Then I should see the table-caption "Records found: 2"
	And I should see a link to the Show page for valve image: "one" 
	And I should see a link to the Show page for valve image: "two" 
	And I shouldn't see a link to the Show page for valve image: "three" 



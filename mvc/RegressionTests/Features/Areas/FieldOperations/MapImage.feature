Feature: MapImage

Background: 
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "FieldServicesImages", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "FieldServicesImages", user: "user"
	And a state "one" exists with name: "New Jersey Again", abbreviation: "NJ", scada table: "njscada"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour", county: "one", state: "one"

Scenario: user receives a functional search page with cascading dropdowns
	Given I am logged in as "user"
	When I visit the /FieldOperations/MapImage/Search page 
	And I select state "one" from the State dropdown
	And I select county "one" from the County dropdown
	Then I should see town "one"'s ShortName in the Town dropdown
	When I press Search
	Then I should not see "an error"

Scenario: user receives friendly error message when no results 
	Given I am logged in as "user"
	When I visit the /FieldOperations/MapImage/Search page 
	And I select state "one" from the State dropdown
	And I select county "one" from the County dropdown
	And I press Search
    Then I should see the error message No results matched your query.

Scenario: User should be able to view a map image
	Given a map image "one" exists with town: "one", town section: "A section of town", map page: "123", gradient: "what", north: "some north", south: "some south", east: "some east", west: "some west", date revised: "that's not a date!"
	And I am logged in as "user"
	When I visit the show page for map image: "one"
	Then I should see a display for Town_County_State with "NJ"
	And I should see a display for Town_County with "Monmouth"
	And I should see a display for Town with "Loch Arbour"
	And I should see a display for TownSection with "A section of town"
	And I should see a display for MapPage with "123"
	And I should see a display for Gradient with "what"
	And I should see a display for North with "some north"
	And I should see a display for South with "some south"
	And I should see a display for East with "some east"
	And I should see a display for West with "some west"
	And I should see a display for DateRevised with "that's not a date!"

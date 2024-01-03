Feature: FacilityAreaPage

Background:
	Given a role "ProductionFacilityAreaManagement-read" exists with action: "Read", module: "ProductionFacilityAreaManagement"
	And a role "ProductionFacilityAreaManagement-add" exists with action: "Add", module: "ProductionFacilityAreaManagement"
	And a role "ProductionFacilityAreaManagement-edit" exists with action: "Edit", module: "ProductionFacilityAreaManagement"
	And a user "user" exists with username: "user", roles: ProductionFacilityAreaManagement-read;ProductionFacilityAreaManagement-add;ProductionFacilityAreaManagement-edit
	And a user "notauthroized" exists with username: "notauthorized"
	And a facility area "one" exists 

Scenario: User can create a new facility area
	Given I am logged in as "user" 
	When I visit the Facilities/FacilityArea/New page
	And I enter "Testing123" into the Description field 
	And I press "Save"
	Then the currently shown facility area shall henceforth be known throughout the land as "Phillipe"
	And I should see a display for Description with "Testing123"

Scenario: User can view facility area index
	Given I am logged in as "user" 
	When I visit the Facilities/FacilityArea page
	Then I should see a link to the Show page for facility area "one"

Scenario: User can edit a facility area
	Given I am logged in as "user"
	When I visit the Edit page for facility area "one"
	And I enter "Things and stuff" into the Description field
	And I press "Save"
	Then I should be at the Show page for facility area "one"
	And I should see a display for Description with "Things and stuff"

Scenario: User without role can not view Facility area:
	Given I am logged in as "notauthroized"
	When I visit the Facilities/FacilityArea page
	Then I should see a 404 error message

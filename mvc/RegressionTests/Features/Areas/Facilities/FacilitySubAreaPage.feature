Feature: FacilitySubAreaPage

Background:
	Given a role "ProductionFacilityAreaManagement-read" exists with action: "Read", module: "ProductionFacilityAreaManagement"
	And a role "ProductionFacilityAreaManagement-add" exists with action: "Add", module: "ProductionFacilityAreaManagement"
	And a role "ProductionFacilityAreaManagement-edit" exists with action: "Edit", module: "ProductionFacilityAreaManagement"
	And a user "user" exists with username: "user", roles: ProductionFacilityAreaManagement-read;ProductionFacilityAreaManagement-add;ProductionFacilityAreaManagement-edit
	And a user "notauthroized" exists with username: "notauthorized"
	And a facility area "one" exists 
	And a facility area "two" exists 
	And a facility sub area "one" exists with description: wet, area: "one"

Scenario: User can create a new facility sub area
	Given I am logged in as "user" 
	When I visit the Facilities/FacilitySubArea/New page
	And I enter "Testing123" into the Description field 
	And I select facility area "one" from the Area dropdown
	And I press "Save"
	Then the currently shown facility sub area shall henceforth be known throughout the land as "Phillipe"
	And I should see a display for Description with "Testing123"
	And I should see a display for Area with facility area "one"'s Description

Scenario: User can view facility sub area index
	Given I am logged in as "user" 
	When I visit the Facilities/FacilitySubArea page
	Then I should see a link to the Show page for facility sub area "one"

Scenario: User can edit a facility sub area
	Given I am logged in as "user"
	When I visit the Edit page for facility sub area "one"
	And I enter "Things and stuff" into the Description field
	And I select facility area "two" from the Area dropdown
	And I press "Save"
	Then I should be at the Show page for facility sub area "one"
	And I should see a display for Description with "Things and stuff"
	And I should see a display for Area with facility area "two"'s Description

Scenario: User without role can not view Facility sub area:
	Given I am logged in as "notauthroized"
	When I visit the Facilities/FacilitySubArea page
	Then I should see a 404 error message
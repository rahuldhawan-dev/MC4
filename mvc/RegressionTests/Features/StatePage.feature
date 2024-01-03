Feature: State Page
	In order to manage states
	As a user
	I want to be able to view states

Background: users exist
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And a role "role" exists with action: "Read", module: "FieldServicesDataLookups", user: "user"
	And a user "noroles" exists with username: "noroles"
	And a state "one" exists with name: "Q State", abbreviation: "QQ", scada table: "njscada"
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"

# AUTHORIZATION
Scenario: user with read access can view states
	Given I am logged in as "user"
	When I visit the State page
	Then I should at least see "States" in the bodyHeader element
	And I should see a link to the Show page for state: "one"
	When I follow the show link for state "one"
	Then I should be at the Show page for state: "one"
	And I should see a display for state: "one"'s Name
	And I should not see a link to the Edit page for state "one"
	When I try to access the Edit page for state: "one"
	Then I should be at the Error/Forbidden screen

Scenario: user without proper role should see missing role error for all state pages
	Given I am logged in as "noroles"
	When I visit the State page
	Then I should see the missing role error
	When I visit the Show page for state: "one"
	Then I should see the missing role error

Scenario: user should see forbidden screen when attemptingj to edit a stat page because they are not an admin
	Given I am logged in as "noroles"
	When I try to visit the Edit page for state: "one" 
	Then I should be at the Error/Forbidden screen

Scenario: site admin can access all state pages
	Given I am logged in as "admin"
	When I visit the State page
	Then I should at least see "States" in the bodyHeader element
	And I should see a link to the Show page for state: "one"
	When I visit the Show page for state: "one"
	Then I should see a display for state: "one"'s Name

# CONTENT
Scenario: user should see state fields in the State Show page
	Given I am logged in as "user"
	When I visit the Show page for state: "one"
	Then I should see a display for Abbreviation with "QQ"
	And I should see a display for Name with "Q State"
	And I should see a display for ScadaTable with "njscada"

Scenario: site admin should be able to edit a state
	Given I am logged in as "admin"
	And I am at the Show page for state: "one"
	When I follow the edit link for state "one"
	And I enter "New York" into the Name field
	And I enter "NY" into the Abbreviation field
	And I enter "scada yeah" into the ScadaTable field
	And I press Save
	Then I should see a display for Name with "New York"
	And I should see a display for Abbreviation with "NY"
	And I should see a display for ScadaTable with "scada yeah"
	
Scenario: admin user should see the correct validators when editing
	Given I am logged in as "admin"
	And I am at the Edit page for state: "one"
	When I enter "" into the Name field
	And I press Save
	Then I should see the validation message The Name field is required.
	When I enter "New Jersey" into the Name field
	And I enter "" into the Abbreviation field
	And I press Save
	Then I should see the validation message The Abbreviation field is required.

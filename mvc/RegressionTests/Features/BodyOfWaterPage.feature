Feature: BodyOfWaterPage
	In order to manage bodies of water
	As a user
	I want to be able to manage bodies of water.

Background: 
    Given an admin user "admin" exists with username: "admin"
	And a role "bowread" exists with action: "Read", module: "FieldServicesAssets"
	And a role "bowedit" exists with action: "Edit", module: "FieldServicesAssets"
	And a role "bowadd" exists with action: "Add", module: "FieldServicesAssets"
	And a role "bowdelete" exists with action: "Delete", module: "FieldServicesAssets"
	And a user "user" exists with username: "user", roles: bowread;bowedit;bowadd;bowdelete
	And a state "NJ" exists with abbreviation: "NJ"
	And a operating center "one" exists with State: "NJ", opcode: "one"
	And a body of water "one" exists with name: "Big Scary Lake", operating center: "one"

Scenario: Admin can view a body of water
	Given I am logged in as "admin"
	And I am at the BodyOfWater/Show page for body of water: "one"
	Then I should see a display for Name with "Big Scary Lake"
	And I should see a display for OperatingCenter with operating center "one"'s Description

Scenario: Admin can search a body of water
	Given I am logged in as "admin"
	And I am at the BodyOfWater/Search page
	When I press Search
	Then I should be at the BodyOfWater page
	And I should see a link to the show page for body of water: "one"

Scenario: Admin can create a body of water
	Given I am logged in as "admin"
    When I visit the BodyOfWater/New page
	And I select state "NJ" from the State dropdown
	And I press "Save"
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "one" from the OperatingCenter dropdown
	And I press "Save"
	Then I should see a validation message for Name with "The Name field is required."
	When I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Name field
	And I press "Save"
	Then I should see a validation message for Name with "The field Name must be a string with a maximum length of 50."
	When I enter "description here" into the Name field 
	And I press Save
	Then I should see a display for Name with "description here"

Scenario: Admin can edit a body of water
	Given I am logged in as "admin"
	When I visit the Edit page for body of water: "one"
	And I enter "" into the Name field
	And I press "Save"
	Then I should see a validation message for Name with "The Name field is required."
	When I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Name field
	And I press "Save"
	Then I should see a validation message for Name with "The field Name must be a string with a maximum length of 50."
	When I enter "New description here" into the Name field
	And I select "-- Select --" from the OperatingCenter dropdown
	And I press "Save"
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "one" from the OperatingCenter dropdown
	And I press Save
    Then I should be at the Show page for body of water: "one"
    And I should see a display for Name with "New description here"

Scenario: User can view a body of water
	Given I am logged in as "user"
	And I am at the BodyOfWater/Show page for body of water: "one"
	Then I should see a display for Name with "Big Scary Lake"
	And I should see a display for OperatingCenter with operating center "one"'s Description

Scenario: User can search a body of water
	Given I am logged in as "user"
	And I am at the BodyOfWater/Search page
	When I press Search
	Then I should be at the BodyOfWater page
	And I should see a link to the show page for body of water: "one"

Scenario: User can create a body of water
	Given I am logged in as "user"
    When I visit the BodyOfWater/New page
	And I select state "NJ" from the State dropdown
	And I press "Save"
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "one" from the OperatingCenter dropdown
	And I press "Save"
	Then I should see a validation message for Name with "The Name field is required."
	When I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Name field
	And I press "Save"
	Then I should see a validation message for Name with "The field Name must be a string with a maximum length of 50."
	When I enter "description here" into the Name field 
	And I press Save
	Then I should see a display for Name with "description here"

Scenario: User can edit a body of water
	Given I am logged in as "user"
	When I visit the Edit page for body of water: "one"
	And I enter "" into the Name field
	And I press "Save"
	Then I should see a validation message for Name with "The Name field is required."
	When I enter "This is a really long description really long really long really long really long really long I'm tired of copying and pasting text" into the Name field
	And I press "Save"
	Then I should see a validation message for Name with "The field Name must be a string with a maximum length of 50."
	When I enter "New description here" into the Name field
	And I select "-- Select --" from the OperatingCenter dropdown
	And I press "Save"
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "one" from the OperatingCenter dropdown
	And I press Save
    Then I should be at the Show page for body of water: "one"
    And I should see a display for Name with "New description here"
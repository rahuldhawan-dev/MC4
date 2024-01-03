Feature: PersonnelAreaPage

Background:
	Given an operating center "nj7" exists with opcode: "NJ7"
	And a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesEmployee", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "HumanResourcesEmployee", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "HumanResourcesEmployee", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "HumanResourcesEmployee", operating center: "nj7"
    And a user "user" exists with username: "user", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7

Scenario: User can view a personnel area
	Given an s a p company code "one" exists 
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a personnel area "one" exists with personnel area id: 9999, description: "Description of sorts", operating center: "nj7"
	And I am logged in as "user"
	When I visit the Show page for personnel area: "one"
	Then I should see a display for PersonnelAreaId with "9999"
	And I should see a display for Description with "Description of sorts"
	And I should see a display for OperatingCenter with operating center "nj7"'s Description

Scenario: User should see validation messages when creating a new personnel area
	Given I am logged in as "user"
	And I am at the PersonnelArea/New page
	When I press Save
	Then I should see a validation message for PersonnelAreaId with "The Personnel Area Id field is required."
	And I should see a validation message for Description with "The Description field is required."
	And I should see a validation message for OperatingCenter with "The OperatingCenter field is required."

Scenario: User can create a personnel area
	Given I am logged in as "user"
	And I am at the PersonnelArea/New page 
	When I enter "4444" into the PersonnelAreaId field
	And I enter "Some Description" into the Description field
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save 
	Then I should see a display for PersonnelAreaId with "4444"
	And I should see a display for Description with "Some Description"
	And I should see a display for OperatingCenter with operating center "nj7"'s Description

Scenario: User can edit a personnel area
	Given an operating center "nj4" exists 
	And a personnel area "one" exists with personnel area id: "5555", description: "Desc", operating center: "nj7"
	And I am logged in as "user"
	When I visit the Edit page for personnel area: "one"
	And I enter "6666" into the PersonnelAreaId field
	And I enter "Some Description" into the Description field
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I press Save 
	Then I should be at the show page for personnel area: "one"
	Then I should see a display for PersonnelAreaId with "6666"
	And I should see a display for Description with "Some Description"
	And I should see a display for OperatingCenter with operating center "nj4"'s Description
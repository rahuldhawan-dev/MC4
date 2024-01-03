Feature: PositionGroupPage
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background:
	Given an operating center "nj7" exists with opcode: "NJ7"
	And a role "roleReadNj7" exists with action: "Read", module: "HumanResourcesPositions", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "HumanResourcesPositions", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "HumanResourcesPositions", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "HumanResourcesPositions", operating center: "nj7"
    And a user "user" exists with username: "user", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And a position group common name "one" exists with Description: "PGCN"

Scenario: User can view a position group
	Given an s a p company code "one" exists 
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a position group "one" exists with group: "A12", position description: "PD", business unit: "BU", business unit description: "BUD", s a p company code: "one", state: "one", common name: "one", s a p position group key: "12345678"
	And I am logged in as "user"
	When I visit the Show page for position group: "one"
	Then I should see a display for Group with "A12"
	And I should see a display for PositionDescription with "PD"
	And I should see a display for BusinessUnit with "BU"
	And I should see a display for BusinessUnitDescription with "BUD"
	And I should see a display for SAPCompanyCode with s a p company code "one"'s Description
	And I should see a display for State with state "one"'s Abbreviation
	And I should see a display for CommonName with position group common name "one"'s Description
	And I should see a display for SAPPositionGroupKey with "12345678"

Scenario: User should see validation messages when creating a new position group
	Given I am logged in as "user"
	And I am at the PositionGroup/New page 
	When I press Save
	Then I should see a validation message for Group with "The Group field is required."
	And I should see a validation message for PositionDescription with "The PositionDescription field is required."
	And I should see a validation message for BusinessUnit with "The BusinessUnit field is required."
	And I should see a validation message for BusinessUnitDescription with "The BusinessUnitDescription field is required."
	And I should see a validation message for SAPCompanyCode with "The SAPCompanyCode field is required."
	And I should see a validation message for CommonName with "The CommonName field is required."
	And I should see a validation message for SAPPositionGroupKey with "The SAPPositionGroupKey field is required."

Scenario: User can create a position group
	Given an s a p company code "one" exists 
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And I am logged in as "user"
	And I am at the PositionGroup/New page 
	When I enter "QL1" into the Group field
	And I enter "Some Description" into the PositionDescription field
	And I enter "999999" into the BusinessUnit field
	And I enter "Some Business Unit" into the BusinessUnitDescription field
	And I select s a p company code "one" from the SAPCompanyCode dropdown
	And I select state "one" from the State dropdown
	And I select position group common name "one" from the CommonName dropdown
	And I enter "Words" into the SAPPositionGroupKey field
	And I press Save 
	Then I should see a display for Group with "QL1"
	And I should see a display for PositionDescription with "Some Description"
	And I should see a display for BusinessUnit with "999999"
	And I should see a display for BusinessUnitDescription with "Some Business Unit"
	And I should see a display for SAPCompanyCode with s a p company code "one"'s Description
	And I should see a display for State with state "one"'s Abbreviation
	And I should see a display for CommonName with position group common name "one"'s Description
	And I should see a display for SAPPositionGroupKey with "Words"

Scenario: User can edit a position group
	Given an s a p company code "one" exists 
	And an s a p company code "two" exists 
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a state "two" exists with name: "West Jersey", abbreviation: "WJ"
	And a position group common name "two" exists with description: "WHAAAAAAT"
	And a position group "one" exists with group: "A12", position description: "PD", business unit: "BU", business unit description: "BUD", s a p company code: "one", state: "one"
	And I am logged in as "user"
	When I visit the Edit page for position group: "one"
	And I enter "QL1" into the Group field
	And I enter "Some Description" into the PositionDescription field
	And I enter "999999" into the BusinessUnit field
	And I enter "Some Business Unit" into the BusinessUnitDescription field
	And I select s a p company code "two" from the SAPCompanyCode dropdown
	And I select state "two" from the State dropdown
	And I select position group common name "two" from the CommonName dropdown
	And I enter "Words" into the SAPPositionGroupKey field
	And I press Save 
	Then I should be at the show page for position group: "one"
	Then I should see a display for Group with "QL1"
	And I should see a display for PositionDescription with "Some Description"
	And I should see a display for BusinessUnit with "999999"
	And I should see a display for BusinessUnitDescription with "Some Business Unit"
	And I should see a display for SAPCompanyCode with s a p company code "two"'s Description
	And I should see a display for State with state "two"'s Abbreviation
	And I should see a display for CommonName with position group common name "two"'s Description
	And I should see a display for SAPPositionGroupKey with "Words"
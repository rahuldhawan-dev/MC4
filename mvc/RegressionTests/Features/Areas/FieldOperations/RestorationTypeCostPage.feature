Feature: RestorationTypeCostPage

Background: stuff exists
    Given an admin user "admin" exists with username: "admin"
    And an operating center "nj7" exists with opcode: "nj7"
	And an operating center "nj4" exists with opcode: "nj4"
	And a restoration type "one" exists with description: "ASPHALT - ALLEY"
	And a restoration type "two" exists with description: "PRIMAL - CONCRETE - SLEDGE"

Scenario: admin can search for and view restoration type costs
	Given a restoration type cost "one" exists with restoration type: "one", operating center: "nj7"
	And a restoration type cost "two" exists with restoration type: "two", operating center: "nj4"
	And I am logged in as "admin"
	When I visit the FieldOperations/RestorationTypeCost/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
    Then I should be at the FieldOperations/RestorationTypeCost page
	And I should see a link to the show page for restoration type cost "one"
	And I should not see a link to the show page for restoration type cost "two"
    When I follow the show link for restoration type cost "one"
	Then I should be at the show page for restoration type cost "one"
	And I should see a display for RestorationType with restoration type "one"
	And I should see a display for OperatingCenter with operating center "nj7"

Scenario: admin can edit restoration type costs
	Given a restoration type cost "one" exists with restoration type: "one", operating center: "nj7"
	And I am logged in as "admin"
	When I visit the show page for restoration type cost "one"
	And I follow "Edit"
	Then I should be at the edit page for restoration type cost "one"
    And I should see a display for DisplayRestorationTypeCost_RestorationType with restoration type "one"
	And I should see a display for DisplayRestorationTypeCost_OperatingCenter with operating center "nj7"
	When I enter "8" into the Cost field
	And I enter "17" into the FinalCost field
	And I press Save
	Then I should be at the show page for restoration type cost "one"
	And I should see a display for Cost with "8"
	And I should see a display for FinalCost with "17"

Scenario: admin can create new restoration type costs
	Given I am logged in as "admin"
	When I visit the FieldOperations/RestorationTypeCost/New page
	And I press Save
	Then I should be at the FieldOperations/RestorationTypeCost/New page
	And I should see a validation message for RestorationType with "The RestorationType field is required."
	And I should see a validation message for Cost with "The Cost field is required."
	And I should see a validation message for FinalCost with "The FinalCost field is required."
	And I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select restoration type "one" from the RestorationType dropdown
	And I enter "8" into the Cost field
	And I enter "17" into the FinalCost field
	And I select operating center "nj7" from the OperatingCenter dropdown
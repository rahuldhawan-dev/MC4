Feature: LargeServiceProjectPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town abbreviation type "town" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a town "two" exists with name: "Allenhurst"
	And operating center: "nj7" exists in town: "two" with abbreviation: "AH"
    And a large service project "one" exists
    And a large service project "two" exists
    And I am logged in as "admin"

Scenario: user can search for a large service project
    When I visit the ProjectManagement/LargeServiceProject/Search page
    And I press Search
    Then I should see a link to the Show page for large service project: "one"
    When I follow the Show link for large service project "one"
    Then I should be at the Show page for large service project: "one"

Scenario: user can view a large service project
    When I visit the Show page for large service project: "one"
    Then I should see a display for large service project: "one"'s ProjectTitle

Scenario: user can add a large service project
    When I visit the ProjectManagement/LargeServiceProject/New page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
    And I enter "foo" into the ProjectTitle field
	And I enter "123 Main" into the ProjectAddress field
    And I press Save
    Then the currently shown large service project will now be referred to as "Bernard Simmons III"
    And I should see a display for ProjectTitle with "foo"

Scenario: user can edit a large service project
    When I visit the Edit page for large service project: "one"
	And I select town "two"'s ShortName from the Town dropdown
	And I enter "project title 2" into the ProjectTitle field
	And I enter "123 Easy St." into the ProjectAddress field
    And I press Save
    Then I should be at the Show page for large service project: "one"
    And I should see a display for ProjectTitle with "project title 2"
	And I should see a display for ProjectAddress with "123 Easy St."
	And I should see a display for Town with town "two"
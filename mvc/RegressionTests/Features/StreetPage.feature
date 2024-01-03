Feature: StreetPage

Background: stuff exists
    Given a state "nj" exists with abbreviation: "NJ"
	And a state "ny" exists with abbreviation: "NY"
	And a county "njnmouth" exists with state: "nj"
	And a county "nycean" exists with state: "ny"
	And a town "njton" exists with county: "njnmouth", state: "nj"
	And a town "nyberg" exists with county: "nycean", state: "ny"
	And a street prefix "one" exists
	And a street prefix "two" exists
	And a street suffix "one" exists
	And a street suffix "two" exists
	And a street "nj way" exists with town: "njton", prefix: "one", suffix: "one"
	And a street "ny st" exists with town: "nyberg", prefix: "two", suffix: "two"
	And an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"

Scenario: admin can search for and view streets
    Given I am logged in as "admin"
	When I visit the /Street/Search page
	And I select state "nj" from the State dropdown
	And I press "Search"
	Then I should be at the Street page
	And I should see a link to the Show page for street "nj way"
    When I visit the show page for street "nj way"
	Then I should see a display for Prefix with street prefix "one"
	And I should see a display for Name with street "nj way"'s Name
	And I should see a display for Suffix with street suffix "one"
	And I should see a display for Town with town "njton"
	And I should see a display for Town_County with county "njnmouth"
	And I should see a display for Town_State with state "nj"

Scenario: admin can edit streets
    Given I am logged in as "admin"
	When I visit the Edit page for street "nj way"
	Then I should see a display for Display_Town_State with state "nj"
	And I should see a display for Display_Town_County with county "njnmouth"
	And I should see a display for Display_Town with town "njton"
	When I enter "foo" into the Name field
	And I uncheck the IsActive field
	And I press Save
	Then I should be at the Show page for street "nj way"
	And I should see a display for Name with "foo"
	And I should see a display for IsActive with "No"

Scenario: admin can create streets
    Given I am logged in as "admin"
	When I visit the /Street/New page
	And I press Save
    Then I should see a validation message for State with "The State field is required."
	And I should see a validation message for Name with "The Name field is required."
	When I select state "nj" from the State dropdown
	And I wait for ajax to finish loading
	And I press Save
	Then I should see a validation message for County with "The County field is required."
	When I select county "njnmouth" from the County dropdown
	And I wait for ajax to finish loading
	And I press Save
	Then I should see a validation message for Town with "The Town field is required."
	When I select town "njton" from the Town dropdown
	And I wait for ajax to finish loading
	And I select street prefix "one" from the Prefix dropdown
	And I select street suffix "two" from the Suffix dropdown
	And I enter "foo" into the Name field
	And I press "Save"
	Then I should see a display for Town_State with state "nj"
	And I should see a display for IsActive with "Yes"
	And I should see a display for Town_County with county "njnmouth"
	And I should see a display for Town with town "njton"
	And I should see a display for Prefix with street prefix "one"
	And I should see a display for Name with "foo"
	And I should see a display for Suffix with street suffix "two"

Scenario: admin can destroy a street
	Given I am logged in as "admin"
	When I visit the show page for street "nj way"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Street/Search page
	When I try to access the Show page for street: "nj way" expecting an error
	Then I should see a 404 error message

Scenario: user cannot destroy a street
	Given I am logged in as "user"
	When I visit the show page for street "nj way"
	Then I should not see the "delete" button in the action bar
Feature: RecurringProjectPage
	In order to view Recurring projects
	As a user
	I want to be able to search for them through the site

Background: users exists
	Given a user "user" exists with username: "user"
	And a role "role" exists with action: "Read", module: "FieldServicesProjects", user: "user"
	
Scenario: user receives a functional search page with stuff and receives friendly error message when no results
	Given I am logged in as "user"
	When I visit the /Reports/RecurringProjectlist/Search page 
	Then I should see "Recurring Project List"
	When I select "Yes" from the RequiresScoring dropdown
	And I press Search
	Then I should see "No results matched your query."

Scenario: user should be able to search and get results
	Given a recurring project "one" exists with project title: "test 1"
	And a coordinate "cooooooord" exists with latitude: "-50", longitude: "-100" 
	And a recurring project "two" exists with project title: "test 2", coordinate: "cooooooord"
	And I am logged in as "user"
	When I visit the /Reports/RecurringProjectList/Search page 
	And I press Search
	Then I should see the table-caption "Records found: 2"
	And I should see recurring project "one"'s ProjectTitle in the "Project Title" column
	And I should see recurring project "two"'s ProjectTitle in the "Project Title" column
	And I should see coordinate "cooooooord"'s Latitude in the "Latitude" column
	And I should see coordinate "cooooooord"'s Longitude in the "Longitude" column
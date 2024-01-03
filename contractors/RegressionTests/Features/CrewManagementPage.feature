Feature: Crew Management Page
    In order to manage crews
	As a user
	I want to be able to add/edit/delete my company's crews

Background: admin and non-admin users exist
	Given a contractor "one" exists with name: "one"
	And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
	And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"

Scenario: user cannot access the crew management pages
	Given a crew "one" exists with description: "one", contractor: "one"
	And I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the crew management page
	Then I should be at the forbidden screen
	When I try to access the show page for crew: "one"
	Then I should be at the forbidden screen
	When I try to access the edit page for crew: "one"
	Then I should be at the forbidden screen

Scenario: admin can access the crew management page and see their crews
	Given a contractor "two" exists with name: "two"
	And a crew "Test Crew 1" exists with description: "Test Crew 1", contractor: "one"
	And a crew "Test Crew 2" exists with description: "Test Crew 2", contractor: "one"
	And a crew exists with description: "2 Live Crew", contractor: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the crew management page
	Then I should see "Manage Crews"
	And I should see "Test Crew 1" 
	And I should see "Test Crew 2"
	And I should not see "2 Live Crew"

Scenario: admin can edit an existing crew
	Given a crew "Test Crew 1" exists with description: "Test Crew 1", contractor: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the crew management page
	When I follow "Select" 
	Then I should be at the show page for crew: "Test Crew 1"
	And I should see "Test Crew 1"
	And I should see "Edit"
	When I follow "Edit"
	Then I should see "Test Crew 1" in the Description field
	When I enter "Test Crew Foo" into the Description field
	And I press Submit
	Then I should see "Test Crew Foo"

Scenario: admin adds a new crew
	Given I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the crew management page
	When I follow "New Crew"
	And I enter "Test Crew Foo" into the Description field
	And I enter "5.50" into the Availability field
	When I press Submit
	Then I should see "5.50"
	And I should see "Test Crew Foo"

 Scenario: admin should see open crew assignments
	Given a crew "one" exists with description: "Test Crew 1", contractor: "one"
	And a work order "one" exists with street number: "911"
	And a crew assignment "ca" exists with work order: "one", crew: "one"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	And I am at the show page for crew: "one"
	Then I should not see "No Open Crew Assignments Exist for this Crew."
	And I should see "Order #"
	And I should see "Date Received"
	And I should see "911"
	
Scenario: admin should not be able to view another contractors crew
	Given a contractor "two" exists with name: "two"
	And a crew "one" exists with description: "Test Crew 1", contractor: "one"
	And a crew "two" exists with description: "Test Crew 2", contractor: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	When I try to access the show page for crew: "two"
    Then I should see "Crews with id"
	And I should see "was not found."

 Scenario: admin should not be able to edit another contractors crew
	Given a contractor "two" exists with name: "two"
	And a crew "one" exists with description: "Test Crew 1", contractor: "one"
	And a crew "two" exists with description: "Test Crew 2", contractor: "two"
	And I am logged in as "admin@site.com", password: "testpassword#1"
	When I try to access the edit page for crew: "two" expecting an error
    Then I should see "Crews with id"
	And I should see "was not found."

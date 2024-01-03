Feature: Home Page
    In order to view and manipulate data
    As a user
	I want to be told the sum of two numbers

Background: users exist
    Given a contractor "one" exists with name: "one"
    And an admin user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
	And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"

Scenario: admin logs in and sees menu
    Given I am logged in as "admin@site.com", password: "testpassword#1"
    Then I should see the link "Planning"
    And I should see the link "Scheduling"
    And I should see the link "Crew Assignments"
    And I should see the link "Crew Management"
    And I should see the link "Finalization"
    And I should see the link "General Search"
	And I should see "Logged in as admin@site.com" in the logindisplay element

Scenario: user logs in and sees menu
    Given I am logged in as "user@site.com", password: "testpassword#1"
    Then I should see the link "Crew Assignments"
    And I should see the link "Finalization"
    And I should not see the link "Scheduling"
    And I should not see the link "Crew Management"
    And I should see the link "General Search"
    And I should not see the link "Users"
	And I should see "Logged in as user@site.com" in the logindisplay element

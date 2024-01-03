Feature: Login Page
    In order to prevent unautorized access to data
	As a user
	I want to be required to login

Background: admin user exists
    Given a contractor "one" exists with name: "one"
    Given a user exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"

Scenario: user logs in
	Given I am at the login page
    And I have entered "admin@site.com" into the Email field
    And I have entered "testpassword#1" into the Password field
	When I press Login
	Then I should be at the home screen

Scenario: unknown user tries to log in
    Given I am at the login page
    And I have entered "notadmin@site.com" into the Email field
    And I have entered "testpassword#1" into the Password field
	When I press Login
    Then I should see the validation message "User does not exist or password is incorrect."

Scenario: user enters the wrong password
    Given I am at the login page
    And I have entered "admin@site.com" into the Email field
    And I have entered "nottestpassword" into the Password field
	When I press Login
	Then I should see the validation message "User does not exist or password is incorrect."

Scenario: user enters an invalid email address
	Given I am at the login page
    And I have entered "gobbledygook" into the Email field
    When I press Login
	Then I should see the validation message "The Email field must have a valid email address."

Scenario: user does not enter an email
	Given I am at the login page
    And I have entered "testpassword#1" into the Password field
	When I press Login
	Then I should see the validation message "The Email field is required."

Scenario: user does not enter a password
	Given I am at the login page
    And I have entered "admin@site.com" into the Email field
	When I press Login
	Then I should see the validation message "The Password field is required."

Scenario: user does not enter email or password
    Given I am at the login page
    When I press Login
    Then I should see the validation message "The Email field is required."
    And I should see the validation message "The Password field is required."

Scenario: user with disabled contractor logs in
    Given a contractor "two" exists with name: "one", contractors access: "false"
    And a user exists with email: "disabled@site.com", password: "testpassword#1", contractor: "two"
	And I am at the login page
    And I have entered "disabled@site.com" into the Email field
    And I have entered "testpassword#1" into the Password field
	When I press Login
    Then I should see the validation message "Access is not enabled."

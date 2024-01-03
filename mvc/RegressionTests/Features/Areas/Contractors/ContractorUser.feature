Feature: Contractor Users Page

Background:
	Given a user "user" exists with username: "user"
	And a role "contractorsgeneral-useradmin" exists with action: "UserAdministrator", module: "ContractorsGeneral", user: "user"
    And a contractor "one" exists with name: "one"
	And a contractor "cool guy" exists with name: "cool guy"
	And a contractor user "user" exists with email: "user@site.com", contractor: "one"
	And a contractor user "otheruser" exists with email: "different@user.com", password: "other testpassword"

Scenario: admin user visits the user page to see users they administrate
    Given I am logged in as "user"
	And I am at the Contractors/ContractorUser/Index page
	Then I should see "user@site.com"

Scenario: admin user adds a new user
	Given I am logged in as "user"
	And I am at the Contractors/ContractorUser/New page
	And I have selected contractor "one" from the Contractor dropdown
	And I have entered "newuser@site.com" into the Email field
	And I have entered "What is the meaning of life?" into the PasswordQuestion field
	And I have entered "42" into the PasswordAnswer field
	And I have entered "new testpassword#1" into the Password field
	And I have entered "new testpassword#1" into the ConfirmPassword field
	And I have selected "Yes" from the IsActive dropdown
	And I have selected "No" from the IsAdmin dropdown
	When I press Save
	Then I should see a display for Contractor with "one"
	And I should see a display for Email with "newuser@site.com"
	And I should see a display for IsAdmin with "No"
	And I should see a display for IsActive with "Yes"

Scenario: admin user can change a contractor user's password without knowing the contractor user's current password
	Given a contractor user "one" exists 
	And I am logged in as "user"
	And I am at the edit page for contractor user: "one"
	And I have entered "user's newpassword#1" into the Password field
	And I have entered "user's newpassword#1" into the ConfirmPassword field
	When I press Save
	Then I should be at the show page for contractor user: "one"

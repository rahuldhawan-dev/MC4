Feature: Users Page
	In order to let other employees use the site
	As an admin
	I want to be able to manipulate other users

Background: admin user exists
    Given a contractor "one" exists with name: "one"
	And an admin user "admin" exists with email: "admin@site.com", password: "testpassword#1", contractor: "one"
	And a user "user" exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
	And a contractor "cool guy" exists with name: "cool guy"
	And a user "otheruser" exists with email: "different@user.com", password: "other testpassword", contractor: "cool guy"

Scenario: user visits their own user page
    Given I am logged in as "user@site.com", password: "testpassword#1"
	When I follow "user@site.com"
	Then I should be at the show page for user: "user"
	And I should see "Your Account"

Scenario: a regular user should not be able to see the show page for another user
    Given a user "other user" exists with email: "other.user@site.com", password: "other password" contractor: "one"
    And I am logged in as "user@site.com", password: "testpassword#1"
    When I try to access the show page for user: "other user"
	Then I should see a display for Email with "user@site.com"

Scenario: an admin should not be able to see the show page for a user that's not under their control
    Given I am logged in as "admin@site.com", password: "testpassword#1"
	When I try to access the show page for user: "otheruser"
	Then I should see a display for Email with "admin@site.com"

Scenario: User successfully changes password
	Given I am logged in as "user@site.com", password: "testpassword#1"
	And I have followed "user@site.com"
	And I have followed "Change Password"
	And I have entered "testpassword#1" into the CurrentPassword field
	And I have entered "newpass#1" into the NewPassword field
	And I have entered "newpass#1" into the ConfirmNewPassword field
	When I press Save
	Then I should be at the User/Show page
	And I should see "Password has been changed successfully!"

Scenario: User changes password but the password does not meet security requirements
	Given I am logged in as "user@site.com", password: "testpassword#1"
	And I have followed "user@site.com"
	And I have followed "Change Password"
	And I have entered "testpassword#1" into the CurrentPassword field
	And I have entered "newpass" into the NewPassword field
	And I have entered "newpass" into the ConfirmNewPassword field
	When I press Save
	Then I should see "Password does not meet security requirements."

Scenario: User cancels changing password
	Given I am logged in as "user@site.com", password: "testpassword#1" 
	And I have followed "user@site.com"
	And I have followed "Change Password"
	And I have entered "testpassword#1" into the CurrentPassword field
	And I have entered "newpass" into the NewPassword field
	And I have entered "newpass" into the ConfirmNewPassword field
	When I follow "Cancel"
	Then I should be at the show page for user: "user"
	And I should not see "Your password has been changed successfully!"

Scenario: User tries to change password but enters wrong current password
	Given I am logged in as "user@site.com", password: "testpassword#1" 
	And I have followed "user@site.com"
	And I have followed "Change Password"
	And I have entered "user's wrong testpassword" into the CurrentPassword field
	And I have entered "newpass#1" into the NewPassword field
	And I have entered "newpass#1" into the ConfirmNewPassword field
	When I press Save
	Then I should see the error message "Incorrect password."

Scenario: User successfully changes password question and answer
	Given I am logged in as "user@site.com", password: "testpassword#1" 
	And I have followed "user@site.com"
	And I have followed "Change Password Question and Answer"
	And I have entered "testpassword#1" into the Password field
	And I have entered "some question" into the PasswordQuestion field
	And I have entered "some answer" into the PasswordAnswer field
	When I press Save
	Then I should be at the User/Show page
	And I should see "Password question and answer have been changed successfully!"
	And I should see "some question"

Scenario: User cancels changes password question and answer
	Given I am logged in as "user@site.com", password: "testpassword#1" 
	And I have followed "user@site.com"
	And I have followed "Change Password Question and Answer"
	And I have entered "testpassword#1" into the Password field
	And I have entered "some question" into the PasswordQuestion field
	And I have entered "some answer" into the PasswordAnswer field
	When I follow "Cancel"
	Then I should be at the User/Show page
	And I should not see "Your password question and answer have been changed successfully!"
	And I should not see "some question"
	And I should see a display for PasswordQuestion with "What's the meaning of life, the universe, and everything?"

Scenario: User tries to access the page without being logged in
	Given I am not logged in
    When I try to access the show page for user: "user"
	Then I should be at the login page

Scenario: User accesses the page while logged in
    Given I am logged in as "admin@site.com", password: "testpassword#1"
	When I follow "admin@site.com"
	Then I should be at the show page for admin user: "admin"
	And I should see "Your Account"

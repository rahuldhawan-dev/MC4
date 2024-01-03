Feature: CutoffSawQuestionnairePage
	Boys are from mars
	Girls are from Venus
	I've got a yum yum

Background: user exists
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user"

Scenario: user can view a cutoff saw questionnaire
	Given an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111118"
	And an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "11111117"
	And a cutoff saw questionnaire "one" exists with lead person: "one", saw operator: "two"
	And I am logged in as "user"
	When I visit the Show page for cutoff saw questionnaire: "one"
	Then I should see a display for LeadPerson_FullName with "Theodore Logan"
	And I should see a display for SawOperator_FullName with "Bill Preston"

Scenario: User can download a cutoff saw questionnaire's pdf
    Given an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111118"
	And an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "11111117"
	And a cutoff saw questionnaire "one" exists with lead person: "one", saw operator: "two"
	And I am logged in as "user"
	Then I should be able to download cutoff saw questionnaire "one"'s pdf

Scenario: user can search for a cutoff saw questionnaire
	Given an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111118"
	And an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "11111117"
	And a cutoff saw questionnaire "one" exists with lead person: "one", saw operator: "two"
	And I am logged in as "user"
	When I visit the Search page for cutoff saw questionnaire: "one"
	And I press Search
	And I follow the Show link for cutoff saw questionnaire "one"
	Then I should be at the Show page for cutoff saw questionnaire: "one"

Scenario: user can add a cutoff saw questionnaire
	Given an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111118"
	And an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "11111117"
	And I am logged in as "user"
	When I visit the CutoffSawQuestionnaire/New page
	And I press Save
	Then I should see the validation message The Lead Person field is required.
	And I should see the validation message The Saw Operator field is required.
	And I should see the validation message You must agree to the terms and conditions.
	When I check the Agree field
	And I select employee "one"'s Description from the LeadPerson dropdown
	And I select employee "two"'s Description from the SawOperator dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown cutoff saw questionnaire shall henceforth be known throughout the land as "one"
	And I should be at the Show page for cutoff saw questionnaire: "one"
	# ensure only current active questions are added

Scenario: admin can destroy a cutoff saw questionnaire
	Given an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111118"
	And an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "11111117"
	And a cutoff saw question "one" exists with question: "Hi%2C How are you?"
	And a cutoff saw questionnaire "one" exists with lead person: "one", saw operator: "two"
	And I am logged in as "admin"
	When I visit the Show page for cutoff saw questionnaire: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the CutoffSawQuestionnaire/Search page
	When I try to access the Show page for cutoff saw questionnaire: "one" expecting an error
    Then I should see a 404 error message
	
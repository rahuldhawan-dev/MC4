Feature: UserRole

Background:
	Given a user "user" exists with username: "user"
	And a user "other" exists with username: "other"
	And an admin user "admin" exists with username: "admin"
	And a role "userrole" exists with action: "Read", module: "HumanResourcesEmployee", user: "user"
	And a role "otherrole" exists with action: "UserAdministrator", module: "OperationsIncidents", user: "user"
	And an operating center "one" exists

Scenario: Searching shows results
	Given I am logged in as "admin"
	And I am at the Reports/UserRole/Search page
	When I select "HumanResources - HumanResourcesEmployee" from the Module dropdown
	And I press Search
	Then I should see the following values in the results-table table
    | User Name |
    | user      |

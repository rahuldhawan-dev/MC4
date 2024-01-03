Feature: MeterChangeOuts

Background:
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a role "role-admin" exists with action: "UserAdministrator", module: "FieldServicesMeterChangeOuts", user: "user", operating center: "nj7"
	And a meter change out status "scheduled" exists with description: "Scheduled"
	And a meter change out status "any" exists with description: "Thing"
	And a meter change out status "every" exists with description: "other Thing"

Scenario: User should see message when new serial number is not unique
	Given I am logged in as "user"
	And a meter change out "one" exists with new serial number: "12345"
	And a meter change out "two" exists
	And I am at the FieldOperations/MeterChangeOut/Edit page for meter change out: "two" 
	When I type "12345" into the NewSerialNumber field
	And I press Save
	Then I should see "A meter change out record already uses this serial number."
	When I type "123456" into the NewSerialNumber field
	And I press Save
	Then I should not see "A meter change out record already uses this serial number."

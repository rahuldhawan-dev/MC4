Feature: MeterChangeOuts

Background:
    Given a contractor "one" exists with name: "one"
	And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a meter change out status "scheduled" exists with description: "Scheduled"
	And a meter change out status "any" exists with description: "Thing"
	And a meter change out status "every" exists with description: "other Thing"

Scenario: User should see message when
    Given I am logged in as "user@site.com", password: "testpassword#1"
	And a meter change out contract "one" exists with contractor: "one"
	And a meter change out "one" exists with new serial number: "12345", contract: "one"
	And a meter change out "two" exists with contract: "one"
	And I am at the MeterChangeOut/Edit page for meter change out: "two" 
	When I type "12345" into the NewSerialNumber field
	And I press Save
	Then I should see "A meter change out record already uses this serial number."
	When I type "123456" into the NewSerialNumber field
	And I press Save
	Then I should not see "A meter change out record already uses this serial number."

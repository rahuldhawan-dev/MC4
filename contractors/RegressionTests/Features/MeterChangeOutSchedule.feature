Feature: MeterChangeOutSchedule

Background:
	Given a contractor "one" exists with name: "one"
	And a user exists with email: "user@site.com", password: "testpassword#1", contractor: "one"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a meter change out status "scheduled" exists with description: "Scheduled"
	And a meter schedule time "time-am1" exists with description: "AM @ 6:00", a m: "true"
	And a meter schedule time "time-am2" exists with description: "AM @ 6:30", a m: "true"
	And a meter schedule time "time-am3" exists with description: "AM @ 7:00", a m: "true"
	And a meter schedule time "time-pm1" exists with description: "PM @ 3:00", a m: "false"
	And a meter schedule time "time-pm2" exists with description: "PM @ 4:00", a m: "false"
	And a meter schedule time "time-pm3" exists with description: "PM @ 5:00", a m: "false"
	And a contractor meter crew "one" exists with description: "Some Contractor", contractor: "one"
	And a contractor meter crew "two" exists with description: "Different Contractor", contractor: "one"
	And a meter change out contract "one" exists with contractor: "one"
	And a meter change out "one" exists with meter change out status: "scheduled", date scheduled: "today", meter schedule time: "time-am1", assigned contractor meter crew: "one", contract: "one"
	And a meter change out "two" exists with meter change out status: "scheduled", date scheduled: "today", meter schedule time: "time-am2", assigned contractor meter crew: "two", contract: "one"
	And a meter change out "three" exists with meter change out status: "scheduled", date scheduled: "today", meter schedule time: "time-am3", assigned contractor meter crew: "one", contract: "one"
	And a meter change out "four" exists with meter change out status: "scheduled", date scheduled: "today", meter schedule time: "time-pm2", assigned contractor meter crew: "two", contract: "one"
	And a meter change out "five" exists with meter change out status: "scheduled", date scheduled: "today", meter schedule time: "time-pm2", assigned contractor meter crew: "two", contract: "one"

Scenario: User should see a report and that's it really
    Given I am logged in as "user@site.com", password: "testpassword#1"
	And I am at the MeterChangeOutSchedule/Index page 
	Then I should see the following values in the table-0 table
	| Scheduled Time | Assigned Contractor Meter Crew |
	| AM @ 6:30      | Different Contractor           |
	| PM @ 4:00      | Different Contractor           |
	| PM @ 4:00      | Different Contractor           |
	| AM @ 6:00      | Some Contractor                |
	| AM @ 7:00      | Some Contractor                |
	And I should see "Total: 5"
	And I should see "Some Contractor: (2/0)"
	And I should see "Different Contractor: (1/2)"

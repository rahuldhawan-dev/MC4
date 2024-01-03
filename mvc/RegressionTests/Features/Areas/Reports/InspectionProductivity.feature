Feature: InspectionProductivity

Background: 
	Given a user "user" exists with username: "user", full name: "J-Lo"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an operating center "qq9" exists with opcode: "QQ9", name: "Qbury"
	And a hydrant "nj7-one" exists with operating center: "nj7"
	And a hydrant "nj7-two" exists with operating center: "nj7"
	And a hydrant "qq9-one" exists with operating center: "qq9"
	And a hydrant "qq9-two" exists with operating center: "qq9"
	And a hydrant inspection type "inspect" exists with description: "INSPECT"
	And a hydrant inspection type "inspectflush" exists with description: "INSPECT / FLUSH"
	And a hydrant inspection exists with hydrant: "nj7-one", date inspected: "1/1/2015", minutes flowed: 10, GPM: 10, hydrant inspection type: "inspect", inspected by: "user"

Scenario: User should only see seven days from the starting date by default
	Given I am logged in as "user"
	And I am at the Reports/InspectionProductivity/Search page
	When I enter "1/1/2015" into the StartDate field
	And I press Search 
	Then I should see "1/1/2015"
	And I should see "1/2/2015"
	And I should see "1/3/2015"
	And I should see "1/4/2015"
	And I should see "1/5/2015"
	And I should see "1/6/2015"
	And I should see "1/7/2015"
	And I should not see "1/8/2015"
	# This test can't actually test anything because the "and I should see the following rows" step 
	# will not work with the crazy table this page displays
Feature: AbsenceNotificationPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a operating center "nj7" exists with opcode: "NJ7"
	And a employee status "active" exists with description: "Active"	
	And an employee "bill" exists with first name: "Bill", last name: "Preston", employee id: "11111117", operating center: "nj7", status: "active"
	And a employee f m l a notification "one" exists with description: "Other"
    And a absence notification "one" exists with employee f m l a notification: "one"
    And a absence notification "two" exists with employee f m l a notification: "one"
	And a progressive discipline "one" exists with description: "Counseled"
	And an absence status "one" exists with description: "sick"
    And I am logged in as "admin"

Scenario: user can search for a absence notification
    When I visit the Operations/AbsenceNotification/Search page
    And I press Search
    Then I should see a link to the Show page for absence notification: "one"
    When I follow the Show link for absence notification "one"
    Then I should be at the Show page for absence notification: "one"

Scenario: user can view a absence notification
    When I visit the Show page for absence notification: "one"
    Then I should see a display for absence notification: "one"'s SupervisorNotes

Scenario: user can add a absence notification
    When I visit the Operations/AbsenceNotification/New page	
    And I enter "foo" into the SupervisorNotes field	
	And I select operating center "nj7" from the OperatingCenter dropdown		
	And I select employee "bill"'s Description from the Employee dropdown
	And I select employee f m l a notification "one"'s Description from the EmployeeFMLANotification dropdown
    And I press Save
    Then the currently shown absence notification will now be referred to as "Johnny"
    And I should see a display for SupervisorNotes with "foo"
	And I should see a display for EmployeeFMLANotification with employee f m l a notification "one"'s Description

Scenario: user can edit a absence notification
    When I visit the Edit page for absence notification: "one"
    And I enter "bar" into the SupervisorNotes field
	And I select progressive discipline "one"'s Description from the ProgressiveDiscipline dropdown
	And I select absence status "one"'s Description from the AbsenceStatus dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown		
	And I select employee "bill"'s Description from the Employee dropdown
    And I press Save
    Then I should be at the Show page for absence notification: "one"
    And I should see a display for SupervisorNotes with "bar"
	And I should see a display for ProgressiveDiscipline with progressive discipline "one"'s Description
	And I should see a display for AbsenceStatus with absence status "one"'s Description

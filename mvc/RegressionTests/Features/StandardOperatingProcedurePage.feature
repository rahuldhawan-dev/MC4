Feature: StandardOperatingProcedurePage

Background: things exist
    Given an admin user "admin" exists with username: "admin"
    And an operating center "nj7" exists with opcode: "nj7"
    And an operating center "nj4" exists with opcode: "nj4"
    And an s o p section "one" exists with description: "a"
    And an s o p section "two" exists with description: "b"
    And an s o p sub section "one" exists with description: "c"
    And an s o p sub section "two" exists with description: "d"
    And an operating center "one" exists with opcode: "one"
    And an operating center "two" exists with opcode: "two"
    And a functional area "one" exists
    And a functional area "two" exists
    And an s o p status "one" exists with description: "f"
    And an s o p status "two" exists with description: "g"
    And an s o p category "one" exists with description: "h"
    And an s o p category "two" exists with description: "i"
    And an s o p system "one" exists with description: "lying"
    And an s o p system "two" exists with description: "cheating"
	And a policy practice "one" exists with description: "e"
	And a facility "facility" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility"
    And a standard operating procedure "one" exists with section: "one", sub section: "one", operating center: "one", functional area: "one", status: "one", category: "one", system: "one"
    And a standard operating procedure "two" exists with section: "two", sub section: "two", operating center: "two", functional area: "two", status: "two", category: "two", system: "two"

Scenario: admin can search for standard operating procedures
    Given I am logged in as "admin"
    When I search for StandardOperatingProcedures with no conditions chosen
    Then I should be at the StandardOperatingProcedure page
	And I should see a link to the Show page for standard operating procedure "one"
	And I should see a link to the Show page for standard operating procedure "two"
    When I search for StandardOperatingProcedures with section: "one" chosen
    Then I should be at the StandardOperatingProcedure page
    And I should see a link to the Show page for standard operating procedure "one"
    And I should not see a link to the Show page for standard operating procedure "two"
    When I search for StandardOperatingProcedures with sub section: "two" chosen
    Then I should be at the StandardOperatingProcedure page
    And I should not see a link to the Show page for standard operating procedure "one"
    And I should see a link to the Show page for standard operating procedure "two"
	When I search for StandardOperatingProcedures with operating center: "one" chosen
    Then I should be at the StandardOperatingProcedure page
    And I should see a link to the Show page for standard operating procedure "one"
    And I should not see a link to the Show page for standard operating procedure "two"
    When I search for StandardOperatingProcedures with functional area: "two" chosen
    Then I should be at the StandardOperatingProcedure page
    And I should not see a link to the Show page for standard operating procedure "one"
    And I should see a link to the Show page for standard operating procedure "two"
	When I search for StandardOperatingProcedures with status: "one" chosen
    Then I should be at the StandardOperatingProcedure page
    And I should see a link to the Show page for standard operating procedure "one"
    And I should not see a link to the Show page for standard operating procedure "two"
    When I search for StandardOperatingProcedures with category: "two" chosen
    Then I should be at the StandardOperatingProcedure page
    And I should not see a link to the Show page for standard operating procedure "one"
    And I should see a link to the Show page for standard operating procedure "two"
	When I search for StandardOperatingProcedures with system: "one" chosen
    Then I should be at the StandardOperatingProcedure page
    And I should see a link to the Show page for standard operating procedure "one"
    And I should not see a link to the Show page for standard operating procedure "two"

Scenario: admin can add a standard operating procedure
    Given I am logged in as "admin"
    When I visit the /StandardOperatingProcedure/New page
	And I select s o p section "one" from the Section dropdown
	And I select s o p sub section "one" from the SubSection dropdown
	And I select policy practice "one" from the PolicyPractice dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select functional area "one" from the FunctionalArea dropdown
	And I select s o p status "one" from the Status dropdown
	And I select s o p category "one" from the Category dropdown
	And I select s o p system "one" from the System dropdown
	And I select facility "facility" from the Facility dropdown
    And I press Save
    Then the currently shown standard operating procedure will now be referred to as "new sop"
	And I should be at the Show page for standard operating procedure: "new sop"
	And I should see a display for Section with s o p section "one"'s Description
	And I should see a display for SubSection with s o p sub section "one"'s Description
	And I should see a display for PolicyPractice with policy practice "one"'s Description
	And I should see a display for OperatingCenter with operating center "nj4"
	And I should see a display for FunctionalArea with functional area "one"
	And I should see a display for Status with s o p status "one"
	And I should see a display for Category with s o p category "one"
	And I should see a display for System with s o p system "one"
	And I should see a display for Facility with facility "facility"

Scenario: admin can edit a standard operating procedure
    Given I am logged in as "admin"
    When I visit the Edit page for standard operating procedure: "one"
	And I enter "foo bar" into the Description field
    And I press Save
    Then I should be at the Show page for standard operating procedure: "one"
    And I should see a display for Description with "foo bar"

Scenario: Regular user should not see the questions tab
	Given a user "user" exists with username: "user"
	And a role "role" exists with action: "Read", module: "ManagementGeneral", user: "user"
	And I am logged in as "user"
	When I visit the Show page for standard operating procedure: "one"
	Then I should not see the "Questions" tab

Scenario: A user administrator should see the questions tab
	Given a user "user" exists with username: "user"
	And a role "role" exists with action: "UserAdministrator", module: "ManagementGeneral", user: "user"
	And I am logged in as "user"
	When I visit the Show page for standard operating procedure: "one"
	Then I should see the "Questions" tab
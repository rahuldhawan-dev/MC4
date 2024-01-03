Feature: FamilyMedicalLeaveActCasePage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a family medical leave act case "one" exists
    And a family medical leave act case "two" exists
	And an operating center "nj7" exists with opcode: "NJ7"
	And an employee status "active" exists with description: "Active"
	And an employee "bill" exists with first name: "Bill", last name: "Preston", employee id: "11111117", operating center: "nj7", status: "active"
	And a company absence certification "one" exists with description: "fmla-sick"
    And I am logged in as "admin"

Scenario: user can search for a family medical leave act case
    When I visit the Operations/FamilyMedicalLeaveActCase/Search page
    And I press Search
    Then I should see a link to the Show page for family medical leave act case: "one"
    When I follow the Show link for family medical leave act case "one"
    Then I should be at the Show page for family medical leave act case: "one"

Scenario: user can view a family medical leave act case
    When I visit the Show page for family medical leave act case: "one"
    Then I should see a display for family medical leave act case: "one"'s FrequencyDays

Scenario: user can add a family medical leave act case
    When I visit the Operations/FamilyMedicalLeaveActCase/New page
    And I press Save
	Then I should see the validation message "The Employee field is required."
	When I select employee "bill"'s Description from the Employee dropdown
	And I enter "12-AbC" into the FrequencyDays field
	And I select company absence certification "one"'s Description from the CompanyAbsenceCertification dropdown
	And I press Save
    Then the currently shown family medical leave act case will now be referred to as "clarence"
    And I should see a display for FrequencyDays with "12-AbC"
	And I should see a display for "Employee" with employee: "bill"

Scenario: user can edit a family medical leave act case
    When I visit the Edit page for family medical leave act case: "one"
    And I enter "bar" into the FrequencyDays field
    And I press Save
    Then I should be at the Show page for family medical leave act case: "one"
    And I should see a display for FrequencyDays with "bar"

Feature: BusinessUnit page

Background:
    Given a user "user" exists with username: "user"
    And a role "roleUserAdmin" exists with action: "UserAdministrator", module: "FieldServicesProjects", user: "user"
    And an operating center "nj7" exists with opcode: "NJ7"
    And a business unit area "one" exists
    And a department "one" exists
    And an employee "empy" exists
   
Scenario: User can create a business unit record
    Given I am logged in as "user"
    And I am at the BusinessUnit/New page
    When I press Save
    Then I should see a validation message for BU with "The BU field is required."
    And I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    And I should see a validation message for Department with "The Department field is required."
    And I should see a validation message for Order with "The Order field is required."
    And I should see a validation message for Is271Visible with "The Is 271 Visible field is required."
    When I enter "123456" into the BU field
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select department "one" from the Department dropdown
    And I enter "1" into the Order field
    And I enter "some description" into the Description field
    And I select "Yes" from the Is271Visible dropdown
    And I select employee "empy"'s Description from the EmployeeResponsible dropdown
    And I enter 1 into the AuthorizedStaffingLevelTotal field
    And I enter 2 into the AuthorizedStaffingLevelManagement field
    And I enter 3 into the AuthorizedStaffingLevelNonBargainingUnit field
    And I enter 4 into the AuthorizedStaffingLevelBargainingUnit field
    And I select "Yes" from the IsActive dropdown
    And I press Save
    Then I should see a display for BU with "123456"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for Department with department "one"
    And I should see a display for Order with "1"
    And I should see a display for Description with "some description"
    And I should see a display for Is271Visible with "Yes"
    And I should see a display for EmployeeResponsible with employee "empy"
    And I should see a display for AuthorizedStaffingLevelTotal with "1"
    And I should see a display for AuthorizedStaffingLevelManagement with "2"
    And I should see a display for AuthorizedStaffingLevelNonBargainingUnit with "3"
    And I should see a display for AuthorizedStaffingLevelBargainingUnit with "4"
    And I should see a display for IsActive with "Yes"

Scenario: User can edit an business unit record
    Given a business unit "one" exists
    And I am logged in as "user"
    And I am at the Edit page for business unit "one"
    When I enter "123456" into the BU field
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select department "one" from the Department dropdown
    And I enter "1" into the Order field
    And I enter "some description" into the Description field
    And I select "Yes" from the Is271Visible dropdown
    And I select employee "empy"'s Description from the EmployeeResponsible dropdown
    And I enter 1 into the AuthorizedStaffingLevelTotal field
    And I enter 2 into the AuthorizedStaffingLevelManagement field
    And I enter 3 into the AuthorizedStaffingLevelNonBargainingUnit field
    And I enter 4 into the AuthorizedStaffingLevelBargainingUnit field
    And I press Save
    Then I should see a display for BU with "123456"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for Department with department "one"
    And I should see a display for Order with "1"
    And I should see a display for Description with "some description"
    And I should see a display for Is271Visible with "Yes"
    And I should see a display for EmployeeResponsible with employee "empy"
    And I should see a display for AuthorizedStaffingLevelTotal with "1"
    And I should see a display for AuthorizedStaffingLevelManagement with "2"
    And I should see a display for AuthorizedStaffingLevelNonBargainingUnit with "3"
    And I should see a display for AuthorizedStaffingLevelBargainingUnit with "4"

Scenario: User can search for business unit records
    Given a business unit "one" exists
    And I am logged in as "user"
    And I am at the BusinessUnit/Search page
    When I press Search
    Then I should see a link to the Show page for business unit "one"

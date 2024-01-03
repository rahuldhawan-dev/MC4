Feature: EmployeeHeadCount

Background:
    Given a user "user" exists with username: "user"
    And a role "roleRead" exists with action: "Read", module: "HumanResourcesEmployee", user: "user"
    And a role "roleEdit" exists with action: "Edit", module: "HumanResourcesEmployee", user: "user"
    And a role "roleAdd" exists with action: "Add", module: "HumanResourcesEmployee", user: "user"
    And a role "roleDelete" exists with action: "Delete", module: "HumanResourcesEmployee", user: "user"
    And a state "nj" exists with abbreviation: "NJ"
    And an operating center "nj7" exists with opcode: "NJ7", state: "nj"
    And a business unit "one" exists with operating center: "nj7"

Scenario: User can create an employee head count record
    Given I am logged in as "user"
    And I am at the HumanResources/EmployeeHeadCount/New page
    When I press Save
    Then I should see a validation message for NonUnionCount with "The Non Union field is required."
    And I should see a validation message for UnionCount with "The Union field is required."
    And I should see a validation message for OtherCount with "The Other field is required."
    When I select state "nj" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select business unit "one" from the BusinessUnit dropdown
    And I enter "2023" into the Year field
    And I enter "4/24/1984" into the StartDate field
    And I enter "5/25/1984" into the EndDate field
    And I enter 1 into the NonUnionCount field
    And I enter 2 into the UnionCount field
    And I enter 3 into the OtherCount field
    And I enter "some notes" into the MiscNotes field
    And I press Save
    Then I should see a display for OperatingCenter_State with state "nj"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for BusinessUnit with business unit "one"
    And I should see a display for BusinessUnit_Department with business unit "one"'s Department
    And I should see a display for BusinessUnit_Area with business unit "one"'s Area
    And I should see a display for Year with "2023"
    And I should see a display for StartDate with "4/24/1984"
    And I should see a display for EndDate with "5/25/1984"
    And I should see a display for NonUnionCount with "1"
    And I should see a display for UnionCount with "2"
    And I should see a display for OtherCount with "3"
    And I should see a display for TotalCount with "6"
    And I should see a display for MiscNotes with "some notes"

Scenario: User should see the Total value change as they modify related fields
    Given I am logged in as "user"
    And I am at the HumanResources/EmployeeHeadCount/New page
    # NOTE: Must use type instead of enter so that the input event fires on the client.
    When I type "1" into the NonUnionCount field
    And I type "2" into the UnionCount field
    And I type "3" into the OtherCount field 
    Then I should see "6" in the TotalCount element
    # need to type a space to trigger the event. Just an empty string will not work.
    When I type " " into the OtherCount field
    Then I should see "3" in the TotalCount element
    
Scenario: User can edit an employee head count record
    Given a employee head count "one" exists
    And I am logged in as "user"
    And I am at the Edit page for employee head count "one"
    When I select state "nj" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select business unit "one" from the BusinessUnit dropdown
    And I enter "2023" into the Year field
    And I enter "4/24/1984" into the StartDate field
    And I enter "5/25/1984" into the EndDate field
    And I enter 1 into the NonUnionCount field
    And I enter 2 into the UnionCount field
    And I enter 3 into the OtherCount field
    And I enter "some notes" into the MiscNotes field
    And I press Save
    Then I should be at the Show page for employee head count "one"
    And I should see a display for OperatingCenter_State with state "nj"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for BusinessUnit with business unit "one"
    And I should see a display for BusinessUnit_Department with business unit "one"'s Department
    And I should see a display for BusinessUnit_Area with business unit "one"'s Area
    And I should see a display for Year with "2023"
    And I should see a display for StartDate with "4/24/1984"
    And I should see a display for EndDate with "5/25/1984"
    And I should see a display for NonUnionCount with "1"
    And I should see a display for UnionCount with "2"
    And I should see a display for OtherCount with "3"
    And I should see a display for TotalCount with "6"
    And I should see a display for MiscNotes with "some notes"

Scenario: User can search for employee head count records
    Given a employee head count "one" exists
    And I am logged in as "user"
    And I am at the HumanResources/EmployeeHeadCount/Search page
    When I press Search
    Then I should see a link to the Show page for employee head count "one"

Scenario: User can delete an employee head count record
    Given a employee head count "one" exists
    And I am logged in as "user"
    When I visit the Show page for employee head count "one"
    And I click ok in the dialog after pressing "Delete"
    Then I should be at the HumanResources/EmployeeHeadCount/Search page
    When I try to access the Show page for employee head count: "one" expecting an error
    Then I should see a 404 error message
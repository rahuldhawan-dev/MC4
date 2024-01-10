Feature: Report Use Report

Background:
    Given a state "nj" exists with name: "New Jersey", abbreviation: "NJ" 
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "nj"
    And a user "user" exists with username: "user", default operating center: "nj7", full name: "Bob Belcher" 
    And a role "read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a report viewing "one" exists with user: "user", date viewed: "12/19/2023 8:46:00 AM", report name: "Foo"
 
Scenario: user can search for report use
    Given I am logged in as "user"
    And I am at the Reports/ReportUse/Search page
    When I select state "nj" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    And I wait for the page to reload
    Then I should see the following values in the reportUseTable table
    | Operating Center | User        | Report Name | Date Viewed            |
    | NJ7 - Shrewsbury | Bob Belcher | Foo         | 12/19/2023 8:46:00 AM |
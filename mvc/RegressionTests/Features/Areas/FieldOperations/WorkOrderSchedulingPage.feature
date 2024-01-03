Feature: WorkOrderSchedulingPage

Background: 
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And a town section "one" exists with town: "nj7burg"
    And a town section "inactive" exists with town: "nj7burg", active: false
    And a street "one" exists with town: "nj7burg", full st name: "Easy St"
    And a street "two" exists with town: "nj7burg"
    And work order requesters exist
    And work order purposes exist
    And work order priorities exist
    And work descriptions exist
    And markout requirements exist
    And a service material "one" exists with description: "copper"
	And a service material "two" exists with description: "lead"
	And a service material "three" exists with description: "plastic"
    And a crew "one" exists with description: "one", availability: "8", operating center: "nj7", active: true
    And a crew "two" exists with description: "two", availability: "8", operating center: "nj7", active: true
    And a crew "no operating center" exists with description: "no operating center", active: true
    And a crew "inactive" exists with description: "inactive", operating center: "nj7", active: false
    And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
    And a role "workorder-read" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-add" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "workorder-edit" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "nj7"
    And a role "asset-read" exists with action: "Read", module: "FieldServicesAssets", user: "user", operating center: "nj7"
    And a work order "one" exists with operating center: "nj7", sap notification number: "222222", sap work order number: "111111", date received: "12/01/2019", town: "nj7burg", work description: "hydrant flushing", work order priority: "emergency", work order purpose: "customer", markout requirement: "routine", town section: "one", markout called date: "12/16/2022"  
    And a work order "two" exists with operating center: "nj7", sap notification number: "333333", sap work order number: "222222", date received: "12/02/2020", town: "nj7burg", work description: "hydrant repair", work order priority: "high priority", work order purpose: "compliance", markout requirement: "none", town section: "one", markout called date: "12/17/2022", company service material: "one", customer service material: "two"
    And a work order "three" exists with operating center: "nj7", sap notification number: "444444", sap work order number: "333333", date received: "12/03/2021", town: "nj7burg", work description: "hydrant replacement", work order priority: "routine", work order purpose: "safety", markout requirement: "emergency", town section: "one", markout called date: "12/18/2022", company service material: "two", customer service material: "three"
    And a work order "four" exists with operating center: "nj7", sap notification number: "555555", sap work order number: "444444", date received: "12/04/2022", town: "nj7burg", work description: "leak survey", work order priority: "emergency", work order purpose: "seasonal", date completed: "12/01/2022", town section: "one", markout called date: "12/19/2022"
    And a crew assignment "one" exists with work order: "one", crew: "one", assigned for: "12/21/2022 03:00:00 AM", assigned on: "12/06/2022"
    And a crew assignment "two" exists with work order: "two", crew: "two", assigned for: "12/11/2022 03:00:00 AM", assigned on: "12/06/2022"

Scenario: user can search for and assign orders to crew
    Given I am logged in as "user"
    And I am at the FieldOperations/WorkOrderScheduling/Search page
    When I select operating center "nj7"'s Description from the OperatingCenter dropdown
    And I press "Search"
    Then I should be at the FieldOperations/WorkOrderScheduling page
    And I should not see a link to the show page for work order "one"
    And I should see a link to the show page for work order "two"
    And I should see a link to the show page for work order "three"
    And I should not see a link to the show page for work order "four"
    And I should see the following values in the workOrdersTable table
    | Order #▾ | SAP Notification # | SAP Work Order # | Date Received | Street Number | Asset Type | Service Company Material | Customer Service Line Material | Asset   | Description of Job  | Estimated TTC (hours) | Priority      | Purpose    | Markout Requirement | Markout Ready Date | Markout Expiration Date | Assigned Date | Assigned To |
    | 2        | 333333             | 222222           | 12/2/2020     | 1234          | Valve      |      copper              |          lead                  | VAB-100 | HYDRANT REPAIR      | 0                     | High Priority | Compliance | None                |                    |                         | 12/11/2022    | two         |
    | 3        | 444444             | 333333           | 12/3/2021     | 1234          | Valve      |      lead                |          plastic               | VAB-100 | HYDRANT REPLACEMENT | 0                     | Routine       | Safety     | Emergency           |                    |                         |               |             |
    When I press Assign
    Then I should see a validation message for Crew with "The Crew field is required."
    And I should see a validation message for AssignFor with "The AssignFor field is required."
    When I click the checkbox named WorkOrderIDs with work order "three"'s Id
    And I select crew "one"'s Description from the Crew dropdown
    And I enter "12/31/2022" into the AssignFor field
    And I press Assign
    Then I should be at the FieldOperations/CrewAssignment/ShowCalendar page
    When I visit the FieldOperations/WorkOrderScheduling/Search page
    And I select operating center "nj7"'s Description from the OperatingCenter dropdown
    And I press "Search"
    Then I should be at the FieldOperations/WorkOrderScheduling page
    And I should see the following values in the workOrdersTable table
    | Order #▾ | Assigned Date | Assigned To |
    | 2        |  12/11/2022   |   two       |
    | 3        |  12/31/2022   |   one       |   

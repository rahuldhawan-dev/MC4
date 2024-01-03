Feature: WaterLossPage

Background: 
    Given an admin user "admin" exists with username: "admin"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
    And an asset type "valve" exists with id: 1
    And an asset type "service" exists with id: 4
    And a role "waterlossread" exists with action: "Read", module: "FieldServicesWorkManagement"
    And a user "user" exists with username: "user", roles: waterlossread
    And a work description "workDescSumpPump" exists with description: "sump pump"
    And a work description "workDescValveRepair" exists with description: "valve repair"
    And a work order "workOrderTooEarly" exists with operating center: "nj7", work description: "workDescSumpPump", date completed: "6/1/2023", asset type = "service", business unit: "123456", lost water: "10"
    And a work order "workOrderJuneSump1" exists with operating center: "nj7", work description: "workDescSumpPump", date completed: "6/20/2023", asset type = "service", business unit: "123456", lost water: "10"
    And a work order "workOrderJuneSump2" exists with operating center: "nj7", work description: "workDescSumpPump", date completed: "6/25/2023", asset type = "service", business unit: "567890", lost water: "15"
    And a work order "workOrderJuneSump3" exists with operating center: "nj7", work description: "workDescSumpPump", date completed: "6/30/2023", asset type = "service", business unit: "567890", lost water: "70"
    And a work order "workOrderJuneValve" exists with operating center: "nj7", work description: "workDescValveRepair", date completed: "6/20/2023", asset type = "valve", business unit: "123456", lost water: "23"
    And a work order "workOrderJulySump" exists with operating center: "nj7", work description: "workDescSumpPump", date completed: "7/4/2023", asset type = "service", business unit: "123456", lost water: "7"
    And a work order "workOrderTooLate" exists with operating center: "nj7", work description: "workDescSumpPump", date completed: "7/29/2023", asset type = "service", business unit: "123456", lost water: "14"
    
Scenario: User can search for water losses
    Given I am logged in as "user"
    And I am at the Reports/WaterLossManagement/Search page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I enter "6/17/2023" into the Date_Start field
    And I enter "7/25/2023" into the Date_End field
    And I press Search
    Then I should be at the Reports/WaterLossManagement page
    And I should see the following values in the results table
    | Operating Center | Month | Year | Work Description | Work Order Count | Total Gallons |
    | NJ7 - Shrewsbury | Jun   | 2023 | SUMP PUMP        | 1                | 10            |
    | NJ7 - Shrewsbury | Jun   | 2023 | SUMP PUMP        | 2                | 85            |
    | NJ7 - Shrewsbury | Jun   | 2023 | VALVE REPAIR     | 1                | 23            |
    | NJ7 - Shrewsbury | Jul   | 2023 | SUMP PUMP        | 1                | 7             |
    And I should see "118" in the WaterLossSubTotal6 element
    And I should see "7" in the WaterLossSubTotal7 element
    And I should see "125" in the WaterLossGrandTotal element

Scenario: User can search for water losses without a start date
    Given I am logged in as "user"
    And I am at the Reports/WaterLossManagement/Search page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I select "<" from the Date_Operator dropdown
    And I enter "7/25/2023" into the Date_End field
    And I press Search
    Then I should be at the Reports/WaterLossManagement page
    And I should see the following values in the results table
      | Operating Center | Month | Year | Work Description | Work Order Count | Total Gallons |
      | NJ7 - Shrewsbury | Jun   | 2023 | SUMP PUMP        | 2                | 20            |
      | NJ7 - Shrewsbury | Jun   | 2023 | SUMP PUMP        | 2                | 85            |
      | NJ7 - Shrewsbury | Jun   | 2023 | VALVE REPAIR     | 1                | 23            |
      | NJ7 - Shrewsbury | Jul   | 2023 | SUMP PUMP        | 1                | 7             |
    And I should see "128" in the WaterLossSubTotal6 element
    And I should see "7" in the WaterLossSubTotal7 element
    And I should see "135" in the WaterLossGrandTotal element

Scenario: User can view work orders associated with water losses
    Given I am logged in as "user"
    And I am at the Reports/WaterLossManagement/Search page
    When I select operating center "nj7" from the OperatingCenter dropdown
    And I enter "6/17/2023" into the Date_Start field
    And I enter "7/25/2023" into the Date_End field
    And I press Search
    Then I should be at the Reports/WaterLossManagement page
    When I click the "2" link in the 2nd row of results
    Then I should be at the FieldOperations/GeneralWorkOrder page
    And I should see the link "3"
    And I should see the link "4"
    
Feature: OperatingCenterSpoilRemovalCost

Background: stuff exists
    Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ", scada table: "njscada"   
	And an operating center "opc" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"	
    And a Operating Center Spoil Removal Cost "one" exists with operating center: "opc", cost: "1500"
    And I am logged in as "admin"

Scenario: admin can search for OperatingCenterSpoilRemovalCost
    When I visit the FieldOperations/OperatingCenterSpoilRemovalCost/Search page
    And I select state "one" from the State dropdown
    And I wait for ajax to finish loading
    And I select operating center "opc" from the OperatingCenter dropdown
    And I press "Search"
    Then I should be at the FieldOperations/OperatingCenterSpoilRemovalCost page
    And I should see a link to the Show page for Operating Center Spoil Removal Cost "one"    

Scenario: admin can view OperatingCenterSpoilRemovalCost
    When I visit the Show page for Operating Center Spoil Removal Cost "one"
    Then I should see a display for OperatingCenter with operating center "opc"
    And I should see a display for OperatingCenter_State with state "one"

Scenario: admin can create OperatingCenterSpoilRemovalCost
    When I visit the FieldOperations/OperatingCenterSpoilRemovalCost/New page
    And I enter "testDemo" into the Cost field
    And I press Save
    Then I should see a validation message for State with "The State field is required."
    Then I should see a validation message for Cost with "The field Cost must be a number."
    When I select state "one" from the State dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    When I select operating center "opc" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see a validation message for Cost with "The field Cost must be a number."
    When I enter "1500" into the Cost field
    And I press Save
    Then I should be at the FieldOperations/OperatingCenterSpoilRemovalCost/Show/2 page

Scenario: admin can edit OperatingCenterSpoilRemovalCost
    When I visit the Edit page for Operating Center Spoil Removal Cost "one"
    And I enter "200" into the Cost field
    And I press Save
    Then I should be at the Show page for Operating Center Spoil Removal Cost "one"
    And I should see a display for Cost with "200"
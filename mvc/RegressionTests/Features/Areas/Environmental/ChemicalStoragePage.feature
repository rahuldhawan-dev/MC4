Feature: ChemicalStoragePage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a chemical "one" exists with part number: "1111", sds hyperlink: "http://chemicals-are-cool.com"
    And a chemical "two" exists with part number: "2", sds hyperlink: "http://chemicals-are-cool.com"
    And a state "nj" exists with abbreviation: "NJ", town: "one"
    And a state "pa" exists with abbreviation: "PA", town: "three"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
    And an operating center "nj8" exists with opcode: "NJ8", name: "Burlington"
    And an operating center "pa4" exists with opcode: "PA4", name: "Shrewsburg", state: "pa"
    And a town "one" exists with state: "nj"
    And a town "two" exists with state: "nj"
    And a town "three" exists with state: "pa"
    And a facility "one" exists with operating center: "nj7", town: "one"
    And a facility "two" exists with operating center: "nj8", town: "two"
    And a facility "three" exists with operating center: "pa4", town: "three"
    And a chemical warehouse number "one" exists with operating center: "nj7"
    And a chemical warehouse number "two" exists with operating center: "nj8", warehouse number: "Warehouse 2"
    And a chemical warehouse number "paOne" exists with operating center: "pa4"
    And I am logged in as "admin"
    And a chemical storage "one" exists with chemical: "one", facility: "one"
    And a chemical storage "two" exists with chemical: "one", facility: "two"
    And a chemical storage "two-for-two" exists with chemical: "two", facility: "two"
    And a chemical storage "paChem" exists with chemical: "one", facility: "three"

Scenario: user can search for a chemical storage
    When I visit the Environmental/ChemicalStorage/Search page
    And I select state "pa" from the State dropdown  
    And I press Search
    Then I should see a link to the Show page for chemical storage: "paChem"
    Then I should not see a link to the Show page for chemical storage: "one"
    Then I should not see a link to the Show page for chemical storage: "two"
    And I should see the following values in the chemical-storage table
	| Chemical  | SDS Hyperlink   | 
	| *Chemical*       |http://chemicals-are-cool.com    |
    When I follow the Show link for chemical storage "paChem"
    Then I should be at the Show page for chemical storage: "paChem"

Scenario: user can view a chemical storage
    When I visit the Show page for chemical storage: "one"
    Then I should see a display for chemical storage: "one"'s DeliveryInstructions
    And I should not see the Chemical_SdsHyperlink field
    When I visit the Show page for chemical storage: "two-for-two"
    Then I should see a display for Chemical_SdsHyperlink with "http://chemicals-are-cool.com"

Scenario: user can add a chemical storage
    When I visit the Environmental/ChemicalStorage/New page
    And I enter "foo" into the DeliveryInstructions field
    And I select chemical "one" from the Chemical dropdown
    And I enter "container" into the ContainerType field
    And I enter "maximum" into the MaximumDailyInventory field
    And I enter "average" into the AverageDailyInventory field
    And I enter "817" into the DaysOnSite field
    And I enter "storage pressure" into the StoragePressure field
    And I enter "storage temp" into the StorageTemperature field
    And I check the IsActive field
    And I press Save
    Then I should see the validation message The State field is required. 
    When I select state "nj" from the State dropdown  
    And I select operating center "nj7" from the OperatingCenter dropdown  
    And I select facility "one" from the Facility dropdown  
    And I select chemical warehouse number "one" from the WarehouseNumber dropdown   
    And I press Save  
    Then the currently shown chemical storage will now be referred to as "new" 
    And I should see a display for Facility_Town_State with "NJ"
    And I should see a display for Facility_OperatingCenter with "NJ7 - Shrewsbury"
    And I should see a display for Facility with "Facility 0 - NJ7-1" 
    And I should see a display for WarehouseNumber with chemical warehouse number "one" 
    And I should see a display for Chemical with chemical "one" 
    And I should see a display for DeliveryInstructions with "foo" 
    And I should see a display for ContainerType with "container"
    And I should see a display for MaximumDailyInventory with "maximum"
    And I should see a display for AverageDailyInventory with "average"
    And I should see a display for DaysOnSite with "817"
    And I should see a display for StoragePressure with "storage pressure"
    And I should see a display for StorageTemperature with "storage temp"
    And I should see a display for IsActive with "Yes"

# NOTE: This test is incredibly fragile for some reason. It may start to fail but only
# when another one of the tests in this feature runs first. It seems like a factory somewhere
# isn't creating something correctly, so the edit model isn't mapped as expected. This results in
# the facility and operating center dropdowns having the wrong selected values. 
# The operating center value is set from the chemical warehouse number, or the facility if it can't
# be retrieved otherwise. I don't quite know why this loads up with it selected for NJ7. Everything
# about this test appears to be configured for NJ8. -Ross 10/4/2023
Scenario: user can edit a chemical storage
    Given a chemical "three" exists with name: "Chemical 3", part number: "2222"
    And a chemical storage "three" exists with chemical: "three", facility: "two", IsActive: true
    And a chemical warehouse number "three" exists with operating center: "nj8", warehouse number: "Warehouse 3"
    When I visit the Edit page for chemical storage: "three"
    And I select operating center "nj8" from the OperatingCenter dropdown
    And I enter "bar" into the DeliveryInstructions field
    And I enter "container" into the ContainerType field
    And I enter "maximum" into the MaximumDailyInventory field
    And I enter "average" into the AverageDailyInventory field
    And I enter "817" into the DaysOnSite field
    And I enter "storage pressure" into the StoragePressure field
    And I enter "storage temp" into the StorageTemperature field
    And I uncheck the IsActive field
    And I select chemical warehouse number "three" from the WarehouseNumber dropdown   
    And I select facility "two" from the Facility dropdown
    And I press Save  
    Then I should be at the Show page for chemical storage: "three"
    And I should see a display for Facility_Town_State with "NJ"
    And I should see a display for Facility_OperatingCenter with operating center "nj8"
    And I should see a display for Facility with facility "two"
    And I should see a display for WarehouseNumber with chemical warehouse number "three"
    And I should see a display for Chemical with chemical "three"
    And I should see a display for DeliveryInstructions with "bar"	
    And I should see a display for ContainerType with "container"
    And I should see a display for MaximumDailyInventory with "maximum"
    And I should see a display for AverageDailyInventory with "average"
    And I should see a display for DaysOnSite with "817"
    And I should see a display for StoragePressure with "storage pressure"
    And I should see a display for StorageTemperature with "storage temp"
    And I should see a display for IsActive with "No"

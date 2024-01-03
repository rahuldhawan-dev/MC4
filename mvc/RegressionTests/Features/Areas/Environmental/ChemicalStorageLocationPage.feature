Feature: ChemicalStorageLocationPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a state "nj" exists with abbreviation: "NJ", town: "one"
    And a state "pa" exists with abbreviation: "PA", town: "three"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
    And an operating center "nj8" exists with opcode: "NJ8", name: "Burlington"
    And an operating center "pa4" exists with opcode: "PA4", name: "Shrewsburg", state: "pa"
    And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Jets"
    And a planning plant "two" exists with operating center: "pa4", code: "P420", description: "Eagles"
    And a chemical warehouse number "one" exists with operating center: "nj7"
    And a chemical warehouse number "two" exists with operating center: "nj8", warehouse number: "Warehouse 2"
    And a chemical warehouse number "paOne" exists with operating center: "pa4"
    And I am logged in as "admin"
    And a chemical storage location "one" exists with state: "nj", operating center: "nj7", planning plant: "one", chemical warehouse number: "one", storage location number: "SL1", storage location description: "location 1"
    And a chemical storage location "two" exists with state: "pa", operating center: "pa4", planning plant: "two", chemical warehouse number: "paOne", storage location number: "SL2", storage location description: "location 2"
    
Scenario: user can search for a chemical storage location
    When I visit the Environmental/ChemicalStorageLocation/Search page
    And I select state "pa" from the State dropdown
    And I select operating center "pa4" from the OperatingCenter dropdown
    And I press Search
    Then I should see a link to the Show page for chemical storage location: "two"
    Then I should not see a link to the Show page for chemical storage location: "one"
    
Scenario: user can view a chemical storage location
    When I visit the Show page for chemical storage location: "one"
    Then I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for PlanningPlant with planning plant "one"
    
Scenario: user can add a chemical storage location
    When I visit the Environmental/ChemicalStorageLocation/New page
    And I select state "nj" from the State dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I select planning plant "one" from the PlanningPlant dropdown
    And I select chemical warehouse number "one" from the ChemicalWarehouseNumber dropdown
    And I enter "312" into the StorageLocationNumber field
    And I enter "Location Description" into the StorageLocationDescription field
    And I select "Yes" from the IsActive dropdown
    And I press Save
    Then the currently shown chemical storage location will now be referred to as "gary"
    And I should see a display for State with "NJ"
    And I should see a display for OperatingCenter with operating center "nj7"
    And I should see a display for PlanningPlant with planning plant "one"
    And I should see a display for ChemicalWarehouseNumber with chemical warehouse number "one"
    And I should see a display for StorageLocationNumber with "312" 
    And I should see a display for StorageLocationDescription with "Location Description"
    And I should see a display for IsActive with "Yes"
    
Scenario: user can edit a chemical storage location
    Given a chemical storage location "three" exists with state: "nj", operating center: "nj7", planning plant: "one", chemical warehouse number: "one", storage location number: "SL3", storage location description: "location 3"
    When I visit the Edit page for chemical storage location: "three"
    And I select operating center "nj8" from the OperatingCenter dropdown
    And I select chemical warehouse number "two" from the ChemicalWarehouseNumber dropdown
    And I enter "new nbr" into the StorageLocationNumber field
    And I enter "new location desc" into the StorageLocationDescription field 
    And I select "No" from the IsActive dropdown
    And I press Save  
    Then I should be at the Show page for chemical storage location: "three"
    And I should see a display for OperatingCenter with operating center "nj8"
    And I should see a display for ChemicalWarehouseNumber with chemical warehouse number "two"
    And I should see a display for StorageLocationNumber with "new nbr"
    And I should see a display for StorageLocationDescription with "new location desc"
    And I should see a display for IsActive with "No"
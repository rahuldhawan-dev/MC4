Feature: WellTestPage

Background: data exists
    Given a state "NJ" exists with abbreviation: "NJ"
    And a town "Swedesboro" exists with state: "NJ"
    And an operating center "oc-01" exists with opcode: "NJ7", name: "Shrewsbury", state: "NJ"
    And a public water supply "pws-01" exists with identifier: "NJ1345001", system: "Coastal North Monmouth/Lakewood"
    And operating center: "oc-01" exists in town: "Swedesboro"
    And operating center: "oc-01" exists in public water supply: "pws-01"
    And a company subsidiary "cs-01" exists with description: "ACME"
    And a facility "f-01" exists with operating center: "oc-01", town: "Swedesboro", facility name: "House of Panda", company subsidiary: "cs-01", public water supply: "pws-01"
    And a production work order "pwo-01" exists with operating center: "oc-01", facility: "f-01"
    And an employee "e-01" exists with operating center: "oc-01" 
    And an equipment type "well" exists with description: "well"
    And an equipment "eq-01" exists with identifier: "NJSB-1-EQID-1", facility: "f-01", equipment type: "well", description: "well"
    And an equipment "eq-with-invalid-characteristics" exists with identifier: "NJSB-1-EQID-1", facility: "f-01", equipment type: "well", description: "invalid-well-characteristics"
    And an equipment "eq-with-valid-characteristics" exists with identifier: "NJSB-1-EQID-1", facility: "f-01", equipment type: "well", description: "valid-well-characteristics"
    
    And an equipment characteristic field type "string" exists with data type: "String"
    And an equipment characteristic field type "number" exists with data type: "Number"
    And an equipment characteristic field "ecf-diameter-top" exists with equipment type: "well", field type: "number", field name: "DIAMETERTOP", required: "true"
    And an equipment characteristic field "ecf-diameter-bottom" exists with equipment type: "well", field type: "number", field name: "DIAMETERBOTTOM", required: "true"
    And an equipment characteristic field "ecf-pump-depth" exists with equipment type: "well", field type: "number", field name: "PUMPDEPTH", required: "true"
    And an equipment characteristic field "ecf-well-depth" exists with equipment type: "well", field type: "number", field name: "WELLDEPTH", required: "true"
    And an equipment characteristic field "ecf-method-of-measurement" exists with equipment type: "well", field type: "string", field name: "METHOD_OF_MEASUREMENT", required: "true"
    And an equipment characteristic field "ecf-is-well-vaulted" exists with equipment type: "well", field type: "string", field name: "WELL_VAULTED", required: "true"
    And an equipment characteristic field "ecf-well-type" exists with equipment type: "well", field type: "string", field name: "WELL_TYPE", required: "true"
    And an equipment characteristic field "ecf-well-capacity-rating" exists with equipment type: "well", field type: "number", field name: "WELL_CAPACITY_RATING", required: "true"
    
    And an equipment characteristic "ec-diameter-top" exists with equipment: "eq-01", field: "ecf-diameter-top", value: "10"
    And an equipment characteristic "ec-diameter-bottom" exists with equipment: "eq-01", field: "ecf-diameter-bottom", value: "10"
    And an equipment characteristic "ec-pump-depth" exists with equipment: "eq-01", field: "ecf-pump-depth", value: "180"
    And an equipment characteristic "ec-well-depth" exists with equipment: "eq-01", field: "ecf-well-depth", value: "200"
    And an equipment characteristic "ec-method-of-measurement" exists with equipment: "eq-01", field: "ecf-method-of-measurement", value: "testing-method-of-measurement"
    And an equipment characteristic "ec-is-well-vaulted" exists with equipment: "eq-01", field: "ecf-is-well-vaulted", value: "testing-is-well-vaulted"
    And an equipment characteristic "ec-well-type" exists with equipment: "eq-01", field: "ecf-well-type", value: "testing-well-type"
    And an equipment characteristic "ec-well-capacity-rating" exists with equipment: "eq-01", field: "ecf-well-capacity-rating", value: "10.50"

    And an equipment characteristic "ec-pump-depth-invalid" exists with equipment: "eq-with-invalid-characteristics", field: "ecf-pump-depth", value: ""
    And an equipment characteristic "ec-well-depth-invalid" exists with equipment: "eq-with-invalid-characteristics", field: "ecf-well-depth", value: ""
    And an equipment characteristic "ec-well-capacity-rating-invalid" exists with equipment: "eq-with-invalid-characteristics", field: "ecf-well-capacity-rating", value: ""

    And an equipment characteristic "ec-pump-depth-valid" exists with equipment: "eq-with-valid-characteristics", field: "ecf-pump-depth", value: "42"
    And an equipment characteristic "ec-well-depth-valid" exists with equipment: "eq-with-valid-characteristics", field: "ecf-well-depth", value: "42"
    And an equipment characteristic "ec-well-capacity-rating-valid" exists with equipment: "eq-with-valid-characteristics", field: "ecf-well-capacity-rating", value: "10.50"
    
    And a user "user-unauthorized" exists with username: "user-unauthorized"
    And a user "user-admin" exists with username: "user-admin", employee: "e-01"
    And a role "role-admin" exists with action: "UserAdministrator", module: "ProductionWorkManagement", user: "user-admin", operating center: "oc-01"
    And a role "role-production-equipment" exists with action: "UserAdministrator", module: "ProductionEquipment", user: "user-admin", operating center: "oc-01"

    And a well test grade type "ag" exists with description: "Above Grade"
    And a well test grade type "bg" exists with description: "Below Grade"

    And a well test "wt-01" exists with equipment: "eq-01", employee: "e-01", production work order: "pwo-01", pumping rate: "402", measurement point: "20.50", static water level: "500.50", pumping water level: "600.60", date of test: "3/31/2021", grade type: "ag"
    And a well test "wt-invalid-characteristics" exists with equipment: "eq-with-invalid-characteristics", employee: "e-01", production work order: "pwo-01", pumping rate: "402", measurement point: "20.50", static water level: "500.50", pumping water level: "600.60", date of test: "3/31/2021", grade type: "ag"
    And a well test "wt-valid-characteristics" exists with equipment: "eq-with-valid-characteristics", employee: "e-01", production work order: "pwo-01", pumping rate: "402", measurement point: "20.50", static water level: "500.50", pumping water level: "600.60", date of test: "3/31/2021", grade type: "ag"

Scenario: Unauthorized users cannot access any pages
    Given I am logged in as "user-unauthorized"
    And I am at the Production/WellTest/Search page
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Show page for well test: "wt-01"
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Edit page for well test: "wt-01"
    Then I should see "You do not have the roles necessary to access this resource."

Scenario: User sees 404 Not Found response when visiting New without required parameters
    Given I am logged in as "user-admin"
    When I visit the Production/WellTest/New page
    Then I should see a 404 error message

Scenario: User can add a new well test
    Given I am logged in as "user-admin"
    When I go to the new page for a well test with production work order: "pwo-01", operating center: "oc-01", equipment: "eq-01"
    Then I should see "today's date" in the DateOfTest field
    And employee "e-01"'s Description should be selected in the Employee dropdown
    And I should see a display for ProductionWorkOrderDisplay_ProductionWorkDescription with production work order "pwo-01"'s ProductionWorkDescription
    And I should see a display for ProductionWorkOrderDisplay_OperatingCenter_State with operating center "oc-01"'s State
    And I should see a display for ProductionWorkOrderDisplay_OperatingCenter with operating center "oc-01"
    And I should see a display for ProductionWorkOrderDisplay_Facility with facility "f-01"'s Description
    And I should see a display for EquipmentDisplay_Description with equipment "eq-01"'s Description
    And I should see a display for CompanySubsidiary with company subsidiary "cs-01"'s Description
    And I should see a display for WaterSystem with public water supply "pws-01"'s Description
    And I should see a display for WellName with equipment "eq-01"'s Description
    And I should see a display for WellDiameterTop with equipment characteristic "ec-diameter-top"'s Value
    And I should see a display for WellDiameterBottom with equipment characteristic "ec-diameter-bottom"'s Value
    And I should see a display for WellDepth with equipment characteristic "ec-well-depth"'s Value
    And I should see a display for PumpDepth with equipment characteristic "ec-pump-depth"'s Value
    And I should see a display for MethodOfMeasurement with equipment characteristic "ec-method-of-measurement"'s Value
    And I should see a display for IsWellVaulted with equipment characteristic "ec-is-well-vaulted"'s Value
    And I should see a display for WellType with equipment characteristic "ec-well-type"'s Value
    And I should see a display for WellCapacityRating with equipment characteristic "ec-well-capacity-rating"'s Value
    
    # Test Required Fields
    When I press Save
    Then I should see a validation message for PumpingRate with "The Pumping Rate After 1 Hour (gpm) field is required."
    And I should see a validation message for MeasurementPoint with "The Measurement Point (ft-above/below surface grade) field is required."
    And I should see a validation message for StaticWaterLevel with "The Static Water Level (ft-bmp) field is required."
    And I should see a validation message for PumpingWaterLevel with "The Pumping Water Level After 1 Hour (ft-bmp) field is required."
    And I should see a validation message for GradeType with "The Above or Below Grade field is required."
    When I enter "" into the DateOfTest field
    And I press Save
    Then I should see a validation message for DateOfTest with "The DateOfTest field is required."
    
    # Test min ranges
    When I enter "today's date" into the DateOfTest field
    And I enter "-13" into the PumpingRate field
    And I enter "-100.2" into the MeasurementPoint field
    And I enter "-200.3" into the StaticWaterLevel field
    And I enter "-300.4" into the PumpingWaterLevel field
    And I press Save
    Then I should see a validation message for PumpingRate with "The field Pumping Rate After 1 Hour (gpm) must be between 10 and 10000."
    And I should see a validation message for MeasurementPoint with "The field Measurement Point (ft-above/below surface grade) must be between 1 and 30."
    And I should see a validation message for StaticWaterLevel with "Static Water Level must be between -100.00 and 1000.00, and to the nearest 0.01 foot."
    And I should see a validation message for PumpingWaterLevel with "Pumping Water Level must be between 0.00 and 1000.00, and to the nearest 0.01 foot."
    
    # Test max ranges
    When I enter "100033" into the PumpingRate field
    And I enter "45" into the MeasurementPoint field
    And I enter "4324" into the StaticWaterLevel field
    And I enter "4323" into the PumpingWaterLevel field
    And I press Save
    Then I should see a validation message for PumpingRate with "The field Pumping Rate After 1 Hour (gpm) must be between 10 and 10000."
    And I should see a validation message for MeasurementPoint with "The field Measurement Point (ft-above/below surface grade) must be between 1 and 30."
    And I should see a validation message for StaticWaterLevel with "Static Water Level must be between -100.00 and 1000.00, and to the nearest 0.01 foot."
    And I should see a validation message for PumpingWaterLevel with "Pumping Water Level must be between 0.00 and 1000.00, and to the nearest 0.01 foot."

    # Test Static Water Level > Pumping Water Level
    When I enter "500.01" into the StaticWaterLevel field
    And I enter "500.00" into the PumpingWaterLevel field
    And I press Save
    Then I should see a validation message for StaticWaterLevel with "Static Water Level is greater than Pumping Water Level; please check your entries for accuracy."
    
    # Test valid information
    When I enter "342" into the PumpingRate field
    And I enter "25.01" into the MeasurementPoint field
    And I enter "600.09" into the StaticWaterLevel field
    And I enter "750.15" into the PumpingWaterLevel field
    And I select "Above Grade" from the GradeType dropdown
    And I press Save
    And I wait for the page to reload
    Then I should see a display for Employee with employee "e-01"
    And I should see a display for DateOfTest with "today's date"
    And I should see a display for PumpingRate with "342"
    And I should see a display for MeasurementPoint with "25.01"
    And I should see a display for GradeType with "Above Grade"
    And I should see a display for StaticWaterLevel with "600.09"
    And I should see a display for PumpingWaterLevel with "750.15"
    And I should see a display for SpecificCapacity with "2.28"
    And I should see a display for DrawDown with "150.06"

Scenario: User can edit an existing well test
    Given I am logged in as "user-admin"
    When I visit the Edit page for well test: "wt-01"
    Then I should see "3/31/2021" in the DateOfTest field
    And employee "e-01"'s Description should be selected in the Employee dropdown
    And I should see a display for ProductionWorkOrderDisplay_ProductionWorkDescription with production work order "pwo-01"'s ProductionWorkDescription
    And I should see a display for ProductionWorkOrderDisplay_OperatingCenter_State with operating center "oc-01"'s State
    And I should see a display for ProductionWorkOrderDisplay_OperatingCenter with operating center "oc-01"
    And I should see a display for ProductionWorkOrderDisplay_Facility with facility "f-01"'s Description
    And I should see a display for EquipmentDisplay_Description with equipment "eq-01"'s Description
    And I should see a display for CompanySubsidiary with company subsidiary "cs-01"'s Description
    And I should see a display for WaterSystem with public water supply "pws-01"'s Description
    And I should see a display for WellName with equipment "eq-01"'s Description
    And I should see a display for WellDiameterTop with equipment characteristic "ec-diameter-top"'s Value
    And I should see a display for WellDiameterBottom with equipment characteristic "ec-diameter-bottom"'s Value
    And I should see a display for WellDepth with equipment characteristic "ec-well-depth"'s Value
    And I should see a display for PumpDepth with equipment characteristic "ec-pump-depth"'s Value
    And I should see a display for MethodOfMeasurement with equipment characteristic "ec-method-of-measurement"'s Value
    And I should see a display for IsWellVaulted with equipment characteristic "ec-is-well-vaulted"'s Value
    And I should see a display for WellType with equipment characteristic "ec-well-type"'s Value
    And I should see a display for WellCapacityRating with equipment characteristic "ec-well-capacity-rating"'s Value
    And I should see "402" in the PumpingRate field
    And I should see "20.50" in the MeasurementPoint field 
    And I should see "500.50" in the StaticWaterLevel field 
    And I should see "600.60" in the PumpingWaterLevel field 

    # Test Required Fields
    When I enter "" into the DateOfTest field
    And I enter "" into the PumpingRate field
    And I enter "" into the MeasurementPoint field
    And I enter "" into the StaticWaterLevel field
    And I enter "" into the PumpingWaterLevel field
    When I press Save
    Then I should see a validation message for PumpingRate with "The Pumping Rate After 1 Hour (gpm) field is required."
    And I should see a validation message for MeasurementPoint with "The Measurement Point (ft-above/below surface grade) field is required."
    And I should see a validation message for StaticWaterLevel with "The Static Water Level (ft-bmp) field is required."
    And I should see a validation message for PumpingWaterLevel with "The Pumping Water Level After 1 Hour (ft-bmp) field is required."
    And I should see a validation message for DateOfTest with "The DateOfTest field is required."
    
    When I enter "today's date" into the DateOfTest field
    # Test min ranges
    And I enter "-13" into the PumpingRate field
    And I enter "-100.2" into the MeasurementPoint field
    And I enter "-200.3" into the StaticWaterLevel field
    And I enter "-300.4" into the PumpingWaterLevel field
    And I press Save
    Then I should see a validation message for PumpingRate with "The field Pumping Rate After 1 Hour (gpm) must be between 10 and 10000."
    And I should see a validation message for MeasurementPoint with "The field Measurement Point (ft-above/below surface grade) must be between 1 and 30."
    And I should see a validation message for StaticWaterLevel with "Static Water Level must be between -100.00 and 1000.00, and to the nearest 0.01 foot."
    And I should see a validation message for PumpingWaterLevel with "Pumping Water Level must be between 0.00 and 1000.00, and to the nearest 0.01 foot."
    
    # Test max ranges
    When I enter "100033" into the PumpingRate field
    And I enter "45" into the MeasurementPoint field
    And I enter "4324" into the StaticWaterLevel field
    And I enter "4323" into the PumpingWaterLevel field
    And I press Save
    Then I should see a validation message for PumpingRate with "The field Pumping Rate After 1 Hour (gpm) must be between 10 and 10000."
    And I should see a validation message for MeasurementPoint with "The field Measurement Point (ft-above/below surface grade) must be between 1 and 30."
    And I should see a validation message for StaticWaterLevel with "Static Water Level must be between -100.00 and 1000.00, and to the nearest 0.01 foot."
    And I should see a validation message for PumpingWaterLevel with "Pumping Water Level must be between 0.00 and 1000.00, and to the nearest 0.01 foot."

    # Test Static Water Level > Pumping Water Level
    When I enter "500.01" into the StaticWaterLevel field
    And I enter "500.00" into the PumpingWaterLevel field
    And I press Save
    Then I should see a validation message for StaticWaterLevel with "Static Water Level is greater than Pumping Water Level; please check your entries for accuracy."
    
    # Test valid information
    When I enter "342" into the PumpingRate field
    And I enter "25.01" into the MeasurementPoint field
    And I enter "600.09" into the StaticWaterLevel field
    And I enter "750.15" into the PumpingWaterLevel field
    And I press Save
    And I wait for the page to reload
    Then I should see a display for Employee with employee "e-01"
    And I should see a display for DateOfTest with "today's date"
    And I should see a display for PumpingRate with "342"
    And I should see a display for MeasurementPoint with "25.01"
    And I should see a display for StaticWaterLevel with "600.09"
    And I should see a display for PumpingWaterLevel with "750.15"

Scenario: User can view an existing well test
    Given I am logged in as "user-admin"
    When I visit the show page for well test: "wt-01"
    Then I should see a link to the Show page for production work order "pwo-01"
    And I should see a display for ProductionWorkOrder_OperatingCenter_State with operating center "oc-01"'s State
    And I should see a display for ProductionWorkOrder_OperatingCenter with operating center "oc-01"
    And I should see a display for ProductionWorkOrder_Facility with facility "f-01"'s Description
    And I should see a display for Equipment_Description with equipment "eq-01"'s Description
    And I should see the link "View Equipment Well Test History" ends with "Equipment/Show/1#WellTestResultsTab"
    And I should see a display for CompanySubsidiary with company subsidiary "cs-01"'s Description
    And I should see a display for WaterSystem with public water supply "pws-01"'s Description
    And I should see a display for WellName with equipment "eq-01"'s Description
    And I should see a display for WellDiameterTop with equipment characteristic "ec-diameter-top"'s Value
    And I should see a display for WellDiameterBottom with equipment characteristic "ec-diameter-bottom"'s Value
    And I should see a display for WellDepth with equipment characteristic "ec-well-depth"'s Value
    And I should see a display for PumpDepth with equipment characteristic "ec-pump-depth"'s Value
    And I should see a display for MethodOfMeasurement with equipment characteristic "ec-method-of-measurement"'s Value
    And I should see a display for IsWellVaulted with equipment characteristic "ec-is-well-vaulted"'s Value
    And I should see a display for WellType with equipment characteristic "ec-well-type"'s Value
    And I should see a display for Employee with employee "e-01"
    And I should see a display for DateOfTest with "3/31/2021"
    And I should see a display for PumpingRate with "402"
    And I should see a display for MeasurementPoint with "20.50"
    And I should see a display for GradeType with "Above Grade"
    And I should see a display for StaticWaterLevel with "500.50"
    And I should see a display for PumpingWaterLevel with "600.60"
    And I should see a display for DrawDown with "100.10"    
    And I should see a display for SpecificCapacity with "4.02"

Scenario: User should not see delete in action bar 
	Given I am logged in as "user-admin"
	And I am at the show page for well test: "wt-01"
	Then I should not see "Delete"

Scenario: User can search for an existing well test
    Given I am logged in as "user-admin"
    When I visit the Production/WellTest/Search page
    And I press Search
    Then I should see a link to the Show page for well test: "wt-01"
    When I follow the Show link for well test "wt-01"
    Then I should be at the Show page for well test "wt-01"

Scenario: User should see invalid equipment characteristic messages
    Given I am logged in as "user-admin"
    When I visit the show page for well test: "wt-invalid-characteristics"
    Then I should see "Pump Depth is invalid; please check Equipment Characteristics for accuracy."
    And I should see "Well Depth is invalid; please check Equipment Characteristics for accuracy."
    And I should see "Well Capacity Rating is invalid; please check Equipment Characteristics for accuracy."

Scenario: User should see equipment characteristic validation error messages
    Given I am logged in as "user-admin"
    When I visit the show page for well test: "wt-valid-characteristics"
    Then I should see "Static Water Level is greater than Pump Depth; please check your entries and Equipment Characteristics for accuracy."
    And I should see "Static Water Level is greater than Well Depth; please check your entries and Equipment Characteristics for accuracy."
    And I should see "Pumping Water Level is greater than Pump Depth; please check your entries and Equipment Characteristics for accuracy."
    And I should see "Pumping Water Level is greater than Well Depth; please check your entries and Equipment Characteristics for accuracy."
    And I should see "Pumping Rate is greater than Well Capacity Rating; please check your entries and Equipment Characteristics for accuracy."

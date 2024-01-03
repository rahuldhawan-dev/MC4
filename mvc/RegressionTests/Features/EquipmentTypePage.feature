Feature: EquipmentTypePage

Background: things exist
    Given an operating center "nj7" exists with opcode: "NJ7", name: "or abridging the freedom of speech, or of the press"
    And an operating center "nj4" exists with opcode: "NJ4", name: "or the right of the people peaceably to assemble"
	And a role "roleReadNj7" exists with action: "Read", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleReadNj4" exists with action: "Read", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "FieldServicesEstimatingProjects", operating center: "nj4"
    And an admin user "admin" exists with username: "admin"
	And a user "invalid" exists with username: "invalid"
    And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
    And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
    And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And equipment manufacturers exist
    And equipment types exist
	And a town "nj4ton" exists
	And a planning plant "one" exists with operating center: "nj4", code: "D214", description: "Hgra"
	And a facility "one" exists with operating center: "nj4", facility id: "NJSB-3", facility name: "A Nother Facility", planning plant: "one", has confined space requirement: "True", functional location: "North"
	And a facility area "one" exists with description: "facilityLab"
	And a facility sub area "one" exists with description: wet, area: "one"
	And a facility facility area "one" exists with facility: "one", facilityArea: "one", facilitySubArea: "one"
    And an equipment characteristic field type "string" exists with data type: "String"
    And an equipment characteristic field type "dropdown" exists with data type: "DropDown"
    And an equipment characteristic field type "currency" exists with data type: "Currency"
	And a generator equipment lifespan "generator" exists 
	And an engine equipment lifespan "engine" exists
	#And an equipment manufacturer "generator" exists with description: "generator manufacturer", equipment lifespan: "generator"
	#And an equipment manufacturer "generator" exists with description: "generator manufacturer", generator equipment lifespan: "generator"
	And an equipment model "generator" exists with generator equipment manufacturer: "generator", description: "generator model"
	#And an equipment manufacturer "engine" exists with description: "engine manufacturer", equipment lifespan: "engine"
	And an equipment model "engine" exists with engine equipment manufacturer: "engine", description: "engine model"
	And an equipment category "one" exists with description: "Cat"
	And an equipment category "two" exists with description: "cat 2"
	And an equipment subcategory "one" exists with description: "SubCat"
	And an equipment subcategory "two" exists with description: "sub cat 2"
	And an equipment purpose "one" exists with equipment category: "one", equipment subcategory: "one", description: "eq type one", abbreviation: "abba", generator equipment lifespan: "generator", equipment type: "generator"
    And an equipment purpose "two" exists with engine equipment lifespan: "engine", description: "some other kinda equipment", equipment type: "engine"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", facility facility area: "one", equipment type: "generator", description: "RTU1", s a p equipment id: 1
    And a unit of measure "one" exists with description: "inches"
	And a measurement point equipment type "unused" exists with description: "Height", category: "A", equipment type: "generator", min: "1.0", max: "5.0", unit of measure: "one", position: "1"
	And a measurement point equipment type "used" exists with description: "Width", category: "A", equipment type: "generator", min: "1.0", max: "5.0", unit of measure: "one", position: "1"
	And order types exist
	And a production work description "one" exists with description: "GENERAL REPAIR", order type: "corrective action"
	And a production work order "one" exists with production work description: "one", operating center: "nj4", planning plant: "one", facility: "one", equipment: "one", estimated completion hours: "1", local task description: "description one"
	And a production work order measurement point value exists with production work order: "one", equipment: "one", value: "2", measurement point equipment type: "used", measurement doc id: "123"

Scenario: admin can add fields to Equipment Type
    Given I am logged in as "admin"
    And I am at the Show page for equipment type: "generator"
	When I click the "Characteristic Fields" tab
	And I press "Add Field"
    And I press Add
	Then I should be at the Show page for equipment type: "generator"
    And I should see the validation message "The FieldName field is required."
    And I should see the validation message "The FieldType field is required."
    And I should see the validation message "The Required field is required."
    And I should see the validation message "The IsSAPCharacteristic field is required."
	When I enter "blah blah field name" into the FieldName field
    And I select equipment characteristic field type "string" from the FieldType dropdown
    And I select "Yes" from the Required dropdown
    And I select "No" from the IsSAPCharacteristic dropdown
    And I press Add
    And I wait for the page to reload
    Then I should be at the Show page for equipment type: "generator"

Scenario: Admin can add fields with drop down values to Equipment Type
    Given I am logged in as "admin"
    And I am at the Show page for equipment type: "generator"
	When I click the "Characteristic Fields" tab
	And I press "Add Field"
	When I enter "blah blah field name" into the FieldName field
    And I select equipment characteristic field type "dropdown" from the FieldType dropdown
    And I select "Yes" from the Required dropdown
    And I select "No" from the IsSAPCharacteristic dropdown
    And I press Add
    Then I should be at the Show page for equipment type: "generator"
    And I should see the validation message "Fields with type 'DropDown' must have at least one drop down value."
    When I enter "asdf" into the DropDownValuesInput field
    And I press +
    And I enter "fdsa" into the DropDownValuesInput field
    And I press +
    And I press Add
    And I wait for the page to reload
    Then I should be at the Show page for equipment type: "generator"

Scenario: Admin can remove fields from Equipment Type
    Given an equipment characteristic field "one" exists with equipment type: "generator", field type: "string", field name: "blah blah field name", required: "true"
    And I am logged in as "admin"
    And I am at the Show page for equipment type: "generator"
    Then I should see "blah blah field name" in the table characteristicFieldTable's "Name" column
    And I should see equipment characteristic field type "string"'s DataType in the table characteristicFieldTable's "Type" column
    And I should see "Yes" in the table characteristicFieldTable's "Required" column
	When I click the "Characteristic Fields" tab
    And I click ok in the dialog after pressing "Remove Field"
    And I wait for the page to reload
    Then I should be at the Show page for equipment type: "generator"
    And I should not see "blah blah field name" in the table characteristicFieldTable's "Name" column
    And I should not see equipment characteristic field type "string"'s DataType in the table characteristicFieldTable's "Type" column
    And I should not see "Yes" in the table characteristicFieldTable's "Required" column

Scenario: cannot remove fields that have been used
    Given an equipment characteristic field "one" exists with equipment type: "generator", field type: "string", field name: "blah blah field name", required: "true"
    And an equipment characteristic "one" exists with equipment: "one", field: "one", value: "well isn't that special"
    And I am logged in as "admin"
    When I visit the Show page for equipment type: "generator"
    And I click the "Characteristic Fields" tab
    Then I should see "blah blah field name" in the table characteristicFieldTable's "Name" column
    And I should not see the button "Remove Field"
    
Scenario: admin can add measurement points to an Equipment Type 
	Given I am logged in as "admin"
	When I visit the Show page for equipment type: "generator"
	And I click the "Measurement Points" tab
	And I wait for the page to reload
	And I press "Add Measurement Point"
	And I enter "B" into the Category field
	And I enter "Measurement 3" into the MeasurementPointDescription field
	And I enter "101" into the Position field
	And I select unit of measure "one"'s Description from the UnitOfMeasure dropdown
	And I enter "1.0" into the Min field
	And I enter "10.0" into the Max field
	And I select "Yes" from the MeasurementPointIsActive dropdown
	And I press AddMeasurementPoint
	And I wait for the page to reload
	Then I should see "Measurement 3" in the table measurementPointsTable's "Description" column
	
Scenario: admin can only remove measurement points that are not in use
	Given I am logged in as "admin"
	When I visit the Show page for equipment type: "generator"
	And I click the "Measurement Points" tab
	And I wait for the page to reload
	And I click ok in the dialog after pressing "Delete"
	And I wait for the page to reload
	Then I should not see "Height" in the table measurementPointsTable's "Description" column
	And I should not see the button "Delete"
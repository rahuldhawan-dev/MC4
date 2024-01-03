Feature: EquipmentCharacteristicsFieldPage

Background: users and supporting data exists
	Given a user "user" exists with username: "user", fullname: "SpongeBob Squarepants"
	And a user "other" exists with username: "other"
	And an admin user "admin" exists with username: "admin"
	And a state "one" exists with abbreviation: "NJ"
	And a town "lazytown" exists
	And an operating center "opc" exists with state "one", opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "replace" exists with opcode: "NJ3", name: "Fire Road", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", sap enabled: "true"
	And a role "roleRead" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "opc"
	And a role "roleReadFacility" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "opc"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionEquipment", user: "user", operating center: "opc"
	And a role "roleEditFacility" exists with action: "Edit", module: "ProductionFacilities", user: "user", operating center: "opc"
	And a role "roleAdd" exists with action: "Add", module: "ProductionEquipment", user: "user", operating center: "opc"
	And a role "roleAddFacility" exists with action: "Add", module: "ProductionFacilities", user: "user", operating center: "opc"
	And a role "roleRead2" exists with action: "Read", module: "ProductionEquipment", user: "user"
	And a role "roleEdit2" exists with action: "Edit", module: "ProductionEquipment", user: "user"
	And a role "roleEdit3" exists with action: "Read", module: "ProductionPlannedWork", user: "user"
	And a role "roleAdd2" exists with action: "Add", module: "ProductionEquipment", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionEquipment", user: "user"
	And a role "roleDeleteFacilities" exists with action: "Delete", module: "ProductionFacilities", user: "user"
	And a role "roleReadFacilitiesEam" exists with action: "Read", module: "ProductionFacilities", user: "other"
	And a role "roleReadEquipmentEam" exists with action: "Read", module: "ProductionEquipment", user: "other"
	And a role "roleEditEquipmentEam" exists with action: "Edit", module: "ProductionEquipment", user: "other"
	And a role "roleAddEquipmentEam" exists with action: "Add", module: "ProductionEquipment", user: "other"
	And a role "roleEditEamEam" exists with action: "Edit", module: "EngineeringEAMAssetManagement", user: "other"
	And a role "roleAddEamEam" exists with action: "Add", module: "EngineeringEAMAssetManagement", user: "other"
	And operating center: "opc" exists in town: "lazytown"
    And an equipment type "generator" exists with description: "Generator"
    And an equipment type "engine" exists with description: "Engine"
	And an equipment type "aerator" exists with description: "Aerator"
	And an equipment type "fire suppression" exists with description: "Fire Suppression"
	And an equipment type "well" exists with description: "Well"
	And an equipment type "tnk" exists with description: "TNK"
	And an equipment purpose "aerator" exists with description: "aerator", equipment type: "aerator"
    And equipment manufacturers exist
	And an equipment type "filter" exists with description: "Filter"
	And a generator equipment lifespan "generator" exists 
	And an engine equipment lifespan "engine" exists
	And an filter equipment lifespan "filter" exists
	And an equipment model "generator" exists with equipment manufacturer: "generator", description: "generator model"
	And an equipment model "engine" exists with equipment manufacturer: "engine", description: "engine model"
	And an equipment model "filter" exists with equipment manufacturer: "filter", description: "filter model"
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And an asset type "equipment" exists with description: "equipment"
    And an equipment characteristic field type "string" exists with data type: "String"
    And an equipment characteristic field type "dropdown" exists with data type: "DropDown"
    And an equipment characteristic field type "currency" exists with data type: "Currency"
    And an abc indicator "one" exists
	And an equipment category "one" exists with description: "Cat"
	And an equipment category "two" exists with description: "cat 2"
	And a facility "one" exists with facility id: "NJSB-01", town: "lazytown", operating center: "opc"
	And a facility "two" exists with facility id: "NJSB-01", town: "lazytown", operating center: "opc"
	And a facility area "one" exists with description: "facilityLab"
	And a facility area "two" exists with description: "chemical"
	And a facility sub area "one" exists with description: wet, area: "one"
	And a facility sub area "two" exists with description: Lime, area: "two"
	And a facility facility area "one" exists with facility: "one", facilityArea: "one", facilitySubArea: "one"
	And a facility facility area "two" exists with facility: "one", facilityArea: "two", facilitySubArea: "two"
	And equipment statuses exist
	And an equipment subcategory "one" exists with description: "SubCat"
	And an equipment subcategory "two" exists with description: "sub cat 2"
	And an equipment purpose "generator" exists with equipment category: "one", equipment subcategory: "one", description: "eq type one", abbreviation: "abba", generator equipment lifespan: "generator", equipment type: "generator"
    And an equipment purpose "engine" exists with engine equipment lifespan: "engine", description: "some other kinda equipment", equipment type: "engine"
    And an equipment purpose "fire suppression" exists with equipment category: "one", equipment subcategory: "one", abbreviation: "fs", description: "fire suppression equipment", equipment type: "fire suppression"
    And an equipment purpose "filter" exists with filter equipment lifespan: "filter", description: "filter", equipment type: "filter"
	And an employee status "active" exists with description: "Active"
    And an employee "D:" exists with status: "active"
    And equipment risk characteristics exist
	And an operating center "opc2" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", s a p enabled: "true", IsContractedOperations: "false"
	And a unit of measure "one" exists with description: "gallons"
	And a measurement point equipment type "one" exists with equipment type: "generator", unit of measure: "one", description: "test", min: "0.0", max: "100.0", category: "g", position: "100"
	And a role "rolePWORead" exists with action: "Read", module: "ProductionWorkManagement", user: "user", operating center: "opc2"
	And a role "rolePWOEdit" exists with action: "Edit", module: "ProductionWorkManagement", user: "user", operating center: "opc2"
	And a lockout device color "blue" exists with description: "blue"
	And a lockout device "one" exists with person: "user", lockout device color: "blue", description: "one", serial number: "123"
	And production prerequisites exist
	And an equipment "shoe" exists with equipment status: "in service", equipment type: "generator", facility: "one", equipment purpose: "filter"
	And a planning plant "shoe" exists with operating center: "opc", code: "D217", description: "Argh"
	And a production work description "shoe" exists with equipment type: "generator"
	And a production work order "shoe" exists with productionWorkDescription: "shoe", operating center: "opc", planning plant: "shoe", facility: "one", equipment: "shoe"
	And an equipment characteristic field "field" exists with field name: "test", description: "test", field type: "dropdown", required: "false", IsSAPCharacteristic: "false", is active: "true"
	And an equipment characteristic field "sap" exists with field name: "sap", description: "sap", field type: "dropdown", required: "false", IsSAPCharacteristic: "true", is active: "true"

Scenario: admin can edit drop down values of non-SAP characteristic fields
    Given I am logged in as "admin"
    And I am at the Show page for equipment type: "generator"
	When I click the "Characteristic Fields" tab
	And I click the "Edit" link in the 1st row of characteristicFieldTable
	And I wait for the page to reload
	And I enter "asdf" into the DropDownValuesInput field
	And I press +
	And I enter "fdsa" into the DropDownValuesInput field
	And I press +
	And I press Update
	And I wait for the page to reload
	Then I should be at the Show page for equipment type: "generator"
	When I click the "Characteristic Fields" tab
	And I click the "Edit" link in the 1st row of characteristicFieldTable
	And I wait for the page to reload
		Then I should see the following values in the dropdownValuesTable table 
		  | Drop Down Value  | |
		  | asdf             | Delete |
		  | fdsa             | Delete |
    
Scenario: admin can not edit drop down values of SAP characteristic fields
	Given I am logged in as "admin"
	And I am at the Show page for equipment type: "generator"
	When I click the "Characteristic Fields" tab
	And I click the "Edit" link in the 2nd row of characteristicFieldTable
	And I wait for the page to reload
	Then I should not see the DropDownValuesInput field
	
Scenario: admin can delete drop down values that are not being used
	Given I am logged in as "admin"
	And I am at the Show page for equipment type: "generator"
	When I click the "Characteristic Fields" tab
	And I click the "Edit" link in the 1st row of characteristicFieldTable
	And I wait for the page to reload
	And I enter "asdf" into the DropDownValuesInput field
	And I press +
	And I enter "fdsa" into the DropDownValuesInput field
	And I press +
	And I press Update
	And I wait for the page to reload
	Then I should be at the Show page for equipment type: "generator"
    When I go to the Edit page for equipment: "shoe"
    And I click the "Attributes" tab
    And I select "asdf" from the test dropdown
    And I select employee "D:"'s Description from the RequestedBy dropdown
	And I press Save
    Then I should be at the Show page for equipment: "shoe"
    When I go to the Show page for equipment type: "generator"
	And I click the "Characteristic Fields" tab
	And I click the "Edit" link in the 1st row of characteristicFieldTable
	And I wait for the page to reload
	Then I should see the following values in the dropdownValuesTable table 
	  | Drop Down Value |        |
	  | asdf            |        |
	  | fdsa            | Delete |
    When I click the "Delete" button in the 2nd row of dropdownValuesTable and then click ok in the confirmation dialog
    Then I should be at the Edit page for equipment characteristic field: "field"
	And I should see the following values in the dropdownValuesTable table 
	  | Drop Down Value |        |
	  | asdf            |        |
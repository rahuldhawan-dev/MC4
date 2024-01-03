Feature: Equipment Page
	In order to manage and view equipment and search
	As a user and believer
	I want to be able to view and manage said eqipment

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
	And an equipment "shoe" exists with equipment status: "in service", equipment type: "generator", facility: "one"
	And a planning plant "shoe" exists with operating center: "opc", code: "D217", description: "Argh"
	And a production work description "shoe" exists with equipment type: "generator"
	And a production work order "shoe" exists with productionWorkDescription: "shoe", operating center: "opc", planning plant: "shoe", facility: "one", equipment: "shoe"

Scenario: admin can replace equipment
    Given I do not currently function
    # this test is flaky as of 2021-02-03
	#Given a role "roleReadReplace" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "replace"	
	#And a role "roleAddReplace" exists with action: "Add", module: "ProductionEquipment", user: "user", operating center: "replace"
	#And a role "roleEditReplace" exists with action: "Edit", module: "ProductionEquipment", user: "user", operating center: "replace"
	#And a facility "replace" exists with operating center: "replace"
	#And a role "roleFacilityReadNj4" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "replace"
	#And a role "roleEquipmentReadNj4" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "replace"
	#And an equipment "replace" exists with facility: "replace", identifier: "NJSB-1-EQID-1", equipment status: "in service", s a p equipment id: "12345"
	#And production prerequisites exist
	#And I am logged in as "user"
	#When I visit the Show page for equipment: "replace"
	#And I follow "Replace"	
    #And I select equipment type "engine"'s Description from the EquipmentType dropdown
    #And I wait for ajax to finish loading	
	#And I select equipment purpose "engine"'s ToString from the EquipmentPurpose dropdown
    #And I wait for ajax to finish loading
	#Then production prerequisite "has lockout requirement" should be checked in the Prerequisites checkbox list
	## ross wants me to tell you that "BALDOOR" comes from SAP QA
	## so if it's not being found you're having SAP connection issues	 	
	#When I select "BALDOR" from the EquipmentManufacturer dropdown	
	#And I select employee "D:"'s Description from the RequestedBy dropdown
	#And I select abc indicator "one" from the ABCIndicator dropdown
	#And I select equipment status "pending" from the EquipmentStatus dropdown
	#And I press Save
	#Then the currently shown equipment shall henceforth be known throughout the land as "two" 
	#And equipment status "pending"'s Description should be selected in the EquipmentStatus dropdown
	#When I visit the Show page for equipment: "replace"
	#Then I should see a display for EquipmentStatus with equipment status "pending retirement"'s Description

Scenario: admin can copy equipment
	Given an equipment "one" exists with equipment status: "in service", equipment type: "generator"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Show page for equipment: "one"
    Then I should see the button "Copy"	
    When I click ok in the dialog after pressing "Copy"
	Then the currently shown equipment shall henceforth be known throughout the land as "tim"	
	And I should be at the edit page for equipment "tim"	
	Then I should see "-- Select --" in the EquipmentPurpose dropdown
	When I select equipment type "engine"'s Description from the EquipmentType dropdown
    And I wait for ajax to finish loading
	And I select equipment purpose "engine"'s ToString from the EquipmentPurpose dropdown
    And I wait for ajax to finish loading
    When I select "ENGINE" from the EquipmentManufacturer dropdown	
	And I select employee "D:"'s Description from the RequestedBy dropdown
	And I select equipment status "pending"'s ToString from the EquipmentStatus dropdown
	And I press Save
	Then I should be at the show page for equipment "tim"
	And I should see a display for EquipmentStatus with equipment status "pending"'s Description

Scenario: admin can replace equipment without legacy id
	Given a facility "replace" exists with operating center: "replace"
	And an equipment "replace" exists with equipment status: "cancelled", equipment type: "generator", s a p equipment id: "12345678", facility: "replace"
	And order types exist
	And a production work description "one" exists with description: "GENERAL REPAIR", order type: "corrective action"
    And a production work order "one" exists with productionWorkDescription: "one", equipment: "replace"
	And a production work order equipment "one" exists with production work order: "one", equipment: "replace"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Show page for equipment: "replace"
	Then I should see a link to the Replace page for equipment "replace"	
	When I follow the Replace link for equipment "replace"
	Then I should be at the Replace page for equipment "replace"
	When I select equipment purpose "generator"'s ToString from the EquipmentPurpose dropdown
 	And I select equipment manufacturer "generator"'s ToString from the EquipmentManufacturer dropdown
	And I select production work order "one" from the ReplacementProductionWorkOrder dropdown
    And I wait for ajax to finish loading
	And I enter "1234" into the Legacy field
	And I select employee "D:"'s Description from the RequestedBy dropdown
	When I select employee "D:"'s Description from the AssetControlSignOffBy dropdown
	And I enter "1/1/1999" into the AssetControlSignOffDate field
	And I press Save
	Then the currently shown equipment shall henceforth be known throughout the land as "tim"	
	And I should be at the edit page for equipment "tim"

Scenario: admin can replace equipment with legacy id
	Given a facility "replace" exists with operating center: "replace"
	And an equipment "replace" exists with equipment status: "cancelled", equipment type: "generator", equipment purpose: "generator", s a p equipment id: "12345678", facility: "replace", legacy: "1234"
	And order types exist
	And a production work description "one" exists with description: "GENERAL REPAIR", order type: "corrective action"
    And a production work order "one" exists with productionWorkDescription: "one", equipment: "replace"
	And a production work order equipment "one" exists with production work order: "one", equipment: "replace"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Show page for equipment: "replace"
	Then I should see a link to the Replace page for equipment "replace"	
	When I follow the Replace link for equipment "replace"
	Then I should be at the Replace page for equipment "replace"
	When I select equipment purpose "generator"'s ToString from the EquipmentPurpose dropdown
 	And I select equipment manufacturer "generator"'s ToString from the EquipmentManufacturer dropdown
	And I select production work order "one" from the ReplacementProductionWorkOrder dropdown
    And I wait for ajax to finish loading
	And I select employee "D:"'s Description from the RequestedBy dropdown
	And I press Save
	Then the currently shown equipment shall henceforth be known throughout the land as "tim"	
	And I should be at the edit page for equipment "tim"
	When I visit the show page for equipment "tim"
	And I click the "Linked Equipment" tab
	Then I should see a link to the show page for equipment "replace"

Scenario: copy creates a copy with prerequisites included
	Given an operating center "copy" exists
	And a facility "copy" exists with operating center: "copy"
	And a equipment "one" exists with facility: "copy", identifier: "NJSB-1-EQID-1", equipment status: "in service", equipment type: "generator"
	And a production prerequisite exists with facility: "copy", equipment: "one", description: "has lockout requirement"
	And a production prerequisite exists with facility: "copy", equipment: "one", description: "is confined space"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Edit page for equipment: "one"
	And I check production prerequisite "has lockout requirement" in the Prerequisites checkbox list
	And I check production prerequisite "is confined space" in the Prerequisites checkbox list
	And I check production prerequisite "hot work" in the Prerequisites checkbox list
	And I select equipment purpose "generator"'s ToString from the EquipmentPurpose dropdown
	And I select employee "D:"'s Description from the RequestedBy dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for equipment "one"
	Then I should see the following values in the productionPrerequisitesTable table
			 |                         |
			 | Has Lockout Requirement |
			 | Hot Work				   |
			 | Is Confined Space       |
	Then I should see the button "Copy"	
	When I click ok in the dialog after pressing "Copy"		
	Then the currently shown equipment shall henceforth be known throughout the land as "tim"	
	And I should be at the edit page for equipment "tim"	
	Then I should see "-- Select --" in the EquipmentPurpose dropdown
	And production prerequisite "is confined space" should be checked in the Prerequisites checkbox list
	And production prerequisite "has lockout requirement" should be checked in the Prerequisites checkbox list
	And production prerequisite "hot work" should be checked in the Prerequisites checkbox list
	When I select equipment type "engine"'s Description from the EquipmentType dropdown
	And I wait for ajax to finish loading
	And I select equipment purpose "engine"'s ToString from the EquipmentPurpose dropdown
	And I wait for ajax to finish loading
	When I select "ENGINE" from the EquipmentManufacturer dropdown	
	And I select employee "D:"'s Description from the RequestedBy dropdown
	And I select equipment status "pending"'s ToString from the EquipmentStatus dropdown
	And I press Save
	Then I should be at the show page for equipment "tim"
	And I should see a display for EquipmentStatus with equipment status "pending"'s Description
	Then I should see the following values in the productionPrerequisitesTable table
			 |                         |
			 | Has Lockout Requirement |
			 | Hot Work				   |
			 | Is Confined Space       |
	
Scenario: user can add equipment with Status Retired
	Given I am logged in as "user"
	And production prerequisites exist
	When I visit the Equipment/New page
	And I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I enter "" into the Description field
	And I press Save
	Then I should see the validation message The Description field is required.
	And I should see the validation message The Facility field is required.
	When I enter "description here" into the Description field
	And I select facility "one"'s Description from the Facility dropdown
	And I select facility facility area "one"'s Display from the FacilityFacilityArea dropdown
	And I select equipment type "aerator"'s Description from the EquipmentType dropdown
	And I select equipment status "out of service" from the EquipmentStatus dropdown
	Given I can not see the DateRetired field
	When I select equipment status "retired" from the EquipmentStatus dropdown
	Then I should see the DateRetired field
	When I press Save
	Then I should see the validation message DateRetired is required when a piece of equipment is retired.
	Given I can not see the ManufacturerOther field
	When I select equipment manufacturer "other" from the EquipmentManufacturer dropdown
	Then I should see the ManufacturerOther field 
	When I select equipment manufacturer "lowry" from the EquipmentManufacturer dropdown
	And I select equipment purpose "aerator"'s ToString from the EquipmentPurpose dropdown
	And I enter "serial 1" into the SerialNumber field
	And I enter "1/1/1999" into the DateInstalled field
	When I enter "11/1/2023" into the DateRetired field
	And I select employee "D:"'s Description from the RequestedBy dropdown
	And I select abc indicator "one" from the ABCIndicator dropdown
	And I click the "Risk Characteristics" tab
	And I select equipment condition "poor" from the Condition dropdown
	And I select equipment performance rating "poor" from the Performance dropdown
	And I select equipment static dynamic type "static" from the StaticDynamicType dropdown
	And I select equipment consequences of failure rating "low" from the ConsequenceOfFailure dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown equipment shall henceforth be known throughout the land as "one"
	And I should be at the Edit page for equipment "one"
	When I visit the Show page for equipment: "one"
	Then I should see a display for EquipmentStatus with equipment status "retired"'s Description
	

Scenario: user can add equipment
	Given I am logged in as "user"
	And production prerequisites exist
	When I visit the Equipment/New page
	And I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I enter "" into the Description field
	And I press Save
	Then I should see the validation message The Description field is required.
	And I should see the validation message The Facility field is required.
	When I enter "description here" into the Description field
	And I select facility "one"'s Description from the Facility dropdown
	And I select facility facility area "one"'s Display from the FacilityFacilityArea dropdown
	And I select equipment type "aerator"'s Description from the EquipmentType dropdown
	And I select equipment status "out of service" from the EquipmentStatus dropdown
	Given I can not see the ManufacturerOther field
	When I select equipment manufacturer "other" from the EquipmentManufacturer dropdown
	Then I should see the ManufacturerOther field 
	When I select equipment manufacturer "lowry" from the EquipmentManufacturer dropdown
	And I select equipment purpose "aerator"'s ToString from the EquipmentPurpose dropdown
	And I enter "serial 1" into the SerialNumber field
	And I enter "1/1/1999" into the DateInstalled field
	And I enter "safety dance" into the SafetyNotes field
	And I enter "take on me" into the MaintenanceNotes field
	And I enter "time after time" into the OperationNotes field
    And I select employee "D:"'s Description from the RequestedBy dropdown
	And I select abc indicator "one" from the ABCIndicator dropdown
	And I click the "Risk Characteristics" tab
	And I select equipment condition "poor" from the Condition dropdown
	And I select equipment performance rating "poor" from the Performance dropdown
	And I select equipment static dynamic type "static" from the StaticDynamicType dropdown
	And I select equipment consequences of failure rating "low" from the ConsequenceOfFailure dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown equipment shall henceforth be known throughout the land as "one"
	And I should be at the Edit page for equipment "one"
	When I visit the Show page for equipment: "one"
	Then I should see a display for Identifier with "NJ4-1-ETTT-2"
	And I should see a display for Description with "description here"
	And I should see a link to the Show page for facility: "one"
	And I should see a display for EquipmentPurpose with equipment purpose "aerator"'s ToString
	And I should see a display for EquipmentStatus with equipment status "out of service"'s Description
	And I should see a display for EquipmentManufacturer with equipment manufacturer "lowry"
	And I should see a display for SerialNumber with "serial 1"
	And I should see a display for Number with "1"
	And I should see a display for DateInstalled with "1/1/1999"
	And I should see a display for SafetyNotes with "safety dance"
	And I should see a display for MaintenanceNotes with "take on me"
	And I should see a display for OperationNotes with "time after time"
    And I should see a display for CreatedAt with today's date
	When I click the "Risk Characteristics" tab
    Then I should see a display for Condition with equipment condition "poor"'s Description
	And I should see a display for Performance with equipment performance rating "poor"'s Description
	And I should see a display for StaticDynamicType with equipment static dynamic type "static"'s Description
	And I should see a display for ConsequenceOfFailure with equipment consequences of failure rating "low"'s Description
	And I should see a display for RiskCharacteristicsLastUpdatedBy_FullName with user "user"'s FullName
	And I should see a display for RiskCharacteristicsLastUpdatedOn with today's date

Scenario: User should see the has lockout requirement prerequisite checked by default
	Given production prerequisites exist 
	And I am logged in as "user"
	When I visit the Equipment/New page	
	And I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I enter "description here" into the Description field
	And I uncheck production prerequisite "has lockout requirement" in the Prerequisites checkbox list 
	And I select facility "one"'s Description from the Facility dropdown
	And I select equipment status "out of service" from the EquipmentStatus dropdown
	And I select equipment type "generator"'s Description from the EquipmentType dropdown
	Then production prerequisite "has lockout requirement" should be checked in the Prerequisites checkbox list
	And production prerequisite "has lockout requirement" should not be enabled in the Prerequisites checkbox list
	When I select "-- Select --" from the EquipmentType dropdown
	And I uncheck production prerequisite "has lockout requirement" in the Prerequisites checkbox list 
	And I select equipment type "engine"'s Description from the EquipmentType dropdown
	Then production prerequisite "has lockout requirement" should be checked in the Prerequisites checkbox list
	And production prerequisite "has lockout requirement" should not be enabled in the Prerequisites checkbox list
	When I uncheck production prerequisite "has lockout requirement" in the Prerequisites checkbox list 
	And I select "-- Select --" from the EquipmentType dropdown
	And I select equipment type "aerator"'s Description from the EquipmentType dropdown
	Then production prerequisite "has lockout requirement" should be checked in the Prerequisites checkbox list
	
Scenario: user can edit equipment
	Given an equipment "one" exists with identifier: "NJSB-1-EQID-1", equipment status: "in service", equipment type: "generator"
	And production prerequisites exist
	And I am logged in as "user"
	When I visit the Edit page for equipment: "one"
	And I enter "" into the Description field
	And I press Save
	Then I should see the validation message The Description field is required.
	When I enter "description here" into the Description field
	And I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select facility facility area "two"'s Display from the FacilityFacilityArea dropdown
    And I select equipment type "aerator"'s Description from the EquipmentType dropdown
	And I select equipment purpose "aerator" from the EquipmentPurpose dropdown
    Then I should see equipment manufacturer "lowry" in the EquipmentManufacturer dropdown
    When I select equipment manufacturer "lowry" from the EquipmentManufacturer dropdown	
    And I select abc indicator "one"'s ToString from the ABCIndicator dropdown	
	And I select equipment status "out of service" from the EquipmentStatus dropdown
	And I enter "serial 1" into the SerialNumber field
	And I enter "1/1/1908" into the DateInstalled field
	And I enter "safety dance" into the SafetyNotes field
	And I enter "take on me" into the MaintenanceNotes field
	And I enter "time after time" into the OperationNotes field
	And I enter "4321" into the SAPEquipmentId field
    And I select employee "D:"'s Description from the RequestedBy dropdown
	Given I can not see the ManufacturerOther field
	When I select equipment manufacturer "other" from the EquipmentManufacturer dropdown
	Then I should see the ManufacturerOther field 
	When I enter "other things" into the ManufacturerOther field
	And I click the "Risk Characteristics" tab
	And I select equipment condition "poor" from the Condition dropdown
	And I select equipment performance rating "poor" from the Performance dropdown
	And I select equipment static dynamic type "static" from the StaticDynamicType dropdown
	And I select equipment consequences of failure rating "low" from the ConsequenceOfFailure dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for equipment "one"
	And I should see a display for Identifier with "NJ4-1-ETTT-2"
	And I should see a display for Description with "description here"
    And I should see a display for Facility_Department with facility "one"'s Department
	And I should see a link to the Show page for facility: "one"
    And I should see a display for EquipmentManufacturer with equipment manufacturer "other"
	And I should see a display for ManufacturerOther with "other things"
    And I should see a display for ABCIndicator with abc indicator "one"'s Description	
	And I should see a display for EquipmentStatus with equipment status "out of service"'s Description
	And I should see a display for SerialNumber with "serial 1"
	And I should see a display for Number with "1"
	And I should see a display for DateInstalled with "1/1/1908"
	And I should see a display for SafetyNotes with "safety dance"
	And I should see a display for MaintenanceNotes with "take on me"
	And I should see a display for OperationNotes with "time after time"
	And I should see a display for FunctionalLocation with "Oz"
	And I should see a display for SAPEquipmentId with "4321"
	When I click the "Risk Characteristics" tab
    Then I should see a display for Condition with equipment condition "poor"'s Description
	And I should see a display for Performance with equipment performance rating "poor"'s Description
	And I should see a display for StaticDynamicType with equipment static dynamic type "static"'s Description
	And I should see a display for ConsequenceOfFailure with equipment consequences of failure rating "low"'s Description
	And I should see a display for RiskCharacteristicsLastUpdatedBy_FullName with user "user"'s FullName
	And I should see a display for RiskCharacteristicsLastUpdatedOn with today's date
	
Scenario: User should not see manufacturer other if the manufacturer is not other
	Given an equipment "one" exists with identifier: "NJSB-1-EQID-1", equipment status: "in service", equipment manufacturer: "generator", equipment type: "generator", description: "Generator"
	And an equipment "other" exists with identifier: "NJSB-1-EQID-2", equipment status: "in service", equipment manufacturer: "other", manufacturer other: "neat", equipment type: "generator", description: "other"
	And I am logged in as "user"
	When I visit the Show page for equipment: "one"
	Then I should not see "Manufacturer Other"
	When I visit the Show page for equipment: "other"
	Then I should see a display for ManufacturerOther with "neat"

Scenario: user with readonly access should not be able to add/edit/delete documents
    Given a user "readonly" exists with username: "readonly", roles: roleRead
    And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", equipment status: "in service"
	And I am logged in as "readonly"
	When I visit the Show page for equipment: "one"
    And I click the "Notes" tab
    Then I should not see the toggleNewNote element
    When I click the "Documents" tab
    Then I should not see the toggleLinkDocument element
    And I should not see the toggleNewDocument element

Scenario: user can view an equipment record
	Given an equipment "one" exists with identifier: "NJSB-1-EQID-1", equipment status: "in service", date installed: "1/1/2020"
	And a data type "equipment" exists with table name: "Equipment", name: "Equipment"
	And a document type "sop" exists with data type: "equipment", name: "sop"
	And a document "sop" exists with document type: "sop", file name: "some file name"
	And a coordinate "one" exists
	And an equipment document link "doculinky" exists with document: "sop", equipment: "one", data type: "equipment", document type: "sop"
	And I am logged in as "user"
	When I visit the Show page for equipment: "one"
	Then I should see "EquipmentID"
	And I should see a display for equipment: "one"'s Identifier

Scenario: user can view list of equipment
	Given an equipment "in service" exists with identifier: "NJSB-1-EQID-1", equipment status: "in service"
	And an equipment "retired" exists with identifier: "NJSB-1-EQID-2", equipment status: "retired"
	And an equipment "cancelled" exists with identifier: "NJSB-1-EQID-2", equipment status: "cancelled"
	And I am logged in as "user"
	When I visit the Equipment page
	Then I should see a link to the Show page for equipment: "in service"
	And I should see a link to the Show page for equipment: "retired"
	And I should see a link to the Show page for equipment: "cancelled"
	And the td elements in the 3rd row of the "equipment-table" table should have a "background-color" value of "#ffbdbd"
	And the td elements in the 4th row of the "equipment-table" table should have a "background-color" value of "#ffe08d"
	
Scenario: admin can edit equipment identifier and number
	Given an equipment "one" exists with identifier: "NJSB-1-EQID-1", equipment type: "generator", equipment purpose: "generator", requested by: "D:", equipment status: "in service"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Show page for equipment: "one"
	And I follow the edit link for equipment "one"
	Then I should be at the Edit page for equipment: "one"
	When I enter "4321" into the Number field
	And I select abc indicator "one" from the ABCIndicator dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for equipment: "one"
	And I should see a display for Identifier with "NJ7-4-abba-2"

Scenario: facility field is not blank on the equipment edit page when the facility has a status of "Pending Requirement"
	Given facility statuses exist
	And a facility "fac" exists with facility name: "Swimming Brook", operating center: "opc", facility status: "pending_retirement"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", equipment status: "in service", facility: "fac", equipment type: "generator"
	And production prerequisites exist
	And I am logged in as "user"
	When I visit the Edit page for equipment: "one"
	And I press Save
	Then I should not see the validation message The Facility field is required.
    When I select equipment type "aerator"'s Description from the EquipmentType dropdown
    And I select employee "D:"'s Description from the RequestedBy dropdown
	And I select equipment purpose "aerator" from the EquipmentPurpose dropdown
	When I press Save
	And I wait for the page to reload
	Then I should be at the Show page for equipment "one"
	And I should see a link to the Show page for facility: "fac" 

Scenario: setting Asset Control Off By sets Asset Control Sign Off Date automatically
	Given an equipment "one" exists with equipment type: "generator", equipment purpose: "generator", requested by: "D:", equipment status: "in service"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Show page for equipment: "one"
	And I follow the edit link for equipment "one"
	Then I should be at the Edit page for equipment: "one"
    When I select employee "D:"'s Description from the AssetControlSignOffBy dropdown
    And I press Save
    And I wait for the page to reload
    Then I should be at the Show page for equipment: "one"
    And I should see a display for AssetControlSignOffDate with today's date

Scenario: user can view an equipment record's facility
	Given an equipment "one" exists with facility: "one", equipment status: "in service", equipment type: "tnk", description: "TNK", s a p equipment id: "220", equipment manufacturer: "tnk"
	And I am logged in as "user"
	When I visit the Show page for equipment: "one"
	And I follow the Show link for facility "one"
	Then I should see a display for facility: "one"'s FacilityId
	When I click the "Equipment" tab
	Then I should see a link to the Show page for equipment: "one"
	When I follow the Show link for equipment "one"
	Then I should see a display for Identifier with "NJ4-1-ETTT-2"	

Scenario: user can add generator details
	Given an equipment "one" exists with facility: "one", equipment purpose: "generator", equipment status: "in service"
	And an emergency power type "one" exists with description: "power"
	And a fuel type "gas" exists with description: "gas"
	And I am logged in as "user"
	When I visit the Show page for equipment: "one"
	And I click the "Generator" tab
	And I follow "Add Generator Details"
	And I enter "1-2-3" into the EngineSerialNumber field
	And I enter "3-2-1" into the GeneratorSerialNumber field
	And I enter "13" into the OutputVoltage field
	And I enter "14" into the OutputKW field
	And I enter "15" into the LoadCapacity field
	And I enter "16" into the HP field
	And I enter "17" into the BTU field
	And I enter "18" into the FuelGPH field
	And I enter "11--2--3" into the AQPermitNumber field
	And I enter "16-17" into the TrailerVIN field
	And I enter "19" into the GVWR field
	And I check the HasParallelElectricOperation field
	And I check the HasAutomaticStart field
	And I check the HasAutomaticPowerTransfer field
	And I check the IsPortable field
	And I check the SCADA field
	And I select equipment manufacturer "generator" from the GeneratorManufacturer dropdown
	And I select equipment manufacturer "engine"'s Description from the EngineManufacturer dropdown
	And I select emergency power type "one"'s Description from the EmergencyPowerType dropdown
	And I select fuel type "gas"'s Description from the FuelType dropdown
	And I press Save
	And I click the "Generator" tab
	Then I should see a display for EngineManufacturer with equipment manufacturer "engine"
	And I should see a display for EngineSerialNumber with "1-2-3"
	And I should see a display for GeneratorManufacturer with equipment manufacturer "generator"
	And I should see a display for GeneratorSerialNumber with "3-2-1" 
	And I should see a display for EmergencyPowerType with "power"
	And I should see a display for OutputVoltage with "13"
	And I should see a display for OutputKW with "14"
	And I should see a display for LoadCapacity with "15"
	And I should see a display for HasParallelElectricOperation with "Yes"
	And I should see a display for HasAutomaticStart with "Yes"
	And I should see a display for HasAutomaticPowerTransfer with "Yes"
	And I should see a display for IsPortable with "Yes"
	And I should see a display for SCADA with "Yes"
	And I should see a display for TrailerVIN with "16-17"
	And I should see a display for GVWR with "19"
	And I should see a display for FuelType with "gas"
	And I should see a display for FuelGPH with "18"
	And I should see a display for BTU with "17"
	And I should see a display for HP with "16"
	And I should see a display for AQPermitNumber with "11--2--3"

Scenario: user can edit a generator
	Given an equipment "one" exists with facility: "one", equipment purpose: "generator", equipment status: "in service", equipment type: "generator", description: "Generator"
	And an emergency power type "one" exists with description: "power"
	And a fuel type "gas" exists with description: "gas"
	And a generator "one" exists with equipment: "one"
	And I am logged in as "user"
	When I visit the Show page for equipment: "one"
	And I click the "Generator" tab
	And I follow the Edit link for generator "one"
	And I enter "1-2-3" into the EngineSerialNumber field
	And I enter "3-2-1" into the GeneratorSerialNumber field
	And I enter "13" into the OutputVoltage field
	And I enter "14" into the OutputKW field
	And I enter "15" into the LoadCapacity field
	And I enter "16" into the HP field
	And I enter "17" into the BTU field
	And I enter "18" into the FuelGPH field
	And I enter "11--2--3" into the AQPermitNumber field
	And I enter "16-17" into the TrailerVIN field
	And I enter "19" into the GVWR field
	And I check the HasParallelElectricOperation field
	And I check the HasAutomaticStart field
	And I check the HasAutomaticPowerTransfer field
	And I check the IsPortable field
	And I check the SCADA field
	And I select equipment manufacturer "generator"'s Description from the GeneratorManufacturer dropdown
	And I select equipment manufacturer "engine"'s Description from the EngineManufacturer dropdown
	And I select emergency power type "one"'s Description from the EmergencyPowerType dropdown
	And I select fuel type "gas"'s Description from the FuelType dropdown
	And I press Save
	And I click the "Generator" tab
	Then I should see a display for EngineManufacturer with equipment manufacturer "engine"
	And I should see a display for EngineSerialNumber with "1-2-3"
	And I should see a display for GeneratorManufacturer with equipment manufacturer "generator"
	And I should see a display for GeneratorSerialNumber with "3-2-1" 
	And I should see a display for EmergencyPowerType with "power"
	And I should see a display for OutputVoltage with "13"
	And I should see a display for OutputKW with "14"
	And I should see a display for LoadCapacity with "15"
	And I should see a display for HasParallelElectricOperation with "Yes"
	And I should see a display for HasAutomaticStart with "Yes"
	And I should see a display for HasAutomaticPowerTransfer with "Yes"
	And I should see a display for IsPortable with "Yes"
	And I should see a display for SCADA with "Yes"
	And I should see a display for TrailerVIN with "16-17"
	And I should see a display for GVWR with "19"
	And I should see a display for FuelType with "gas"
	And I should see a display for FuelGPH with "18"
	And I should see a display for BTU with "17"
	And I should see a display for HP with "16"
	And I should see a display for AQPermitNumber with "11--2--3"

Scenario: user can view an equipment purpose
	Given I am logged in as "user"
	When I visit the Show page for equipment purpose: "generator"
	Then I should see a display for EquipmentCategory with "Cat"
	And I should see a display for EquipmentSubCategory with "SubCat"
	And I should see a display for EquipmentLifespan with "Generator"
	And I should see a display for Description with "eq type one"
	And I should see a display for Abbreviation with "abba"

Scenario: user can edit equipment purpose
	Given I am logged in as "user"
	When I visit the Show page for equipment purpose: "generator"
	And I follow the edit link for equipment purpose "generator"
	And I enter "foo" into the Description field
	And I enter "BAR" into the Abbreviation field
	And I select equipment category "two"'s Description from the EquipmentCategory dropdown
	And I select equipment subcategory "two"'s Description from the EquipmentSubCategory dropdown
	And I select engine equipment lifespan "engine"'s Description from the EquipmentLifespan dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for equipment purpose: "generator"
	And I should see a display for Description with "foo"
	And I should see a display for Abbreviation with "BAR"
	And I should see a display for EquipmentCategory with "cat 2"
	And I should see a display for EquipmentSubCategory with "sub cat 2"
	And I should see a display for EquipmentLifespan with "Engine"

Scenario: user can add equipment purpose
	Given I am logged in as "user"
	When I visit the EquipmentPurpose/New page
	And I enter "foo" into the Description field
	And I enter "BAR" into the Abbreviation field
	And I select equipment category "one"'s Description from the EquipmentCategory dropdown
	And I select equipment subcategory "one"'s Description from the EquipmentSubCategory dropdown
	And I select generator equipment lifespan "generator"'s Description from the EquipmentLifespan dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown equipment purpose shall henceforth be known throughout the land as "new"
	And I should be at the Show page for equipment purpose: "new"
	And I should see a display for EquipmentCategory with "Cat"
	And I should see a display for EquipmentSubCategory with "SubCat"
	And I should see a display for EquipmentLifespan with "Generator"
	And I should see a display for Description with "foo"
	And I should see a display for Abbreviation with "BAR"

Scenario: read-only user cannot modify filter media, notes, or documents
    Given a user "readonly" exists with username: "readonly"
	And a role "roleReadEquipmentReadOnly" exists with action: "Read", module: "ProductionEquipment", user: "readonly", operating center: "opc"
	And a role "roleReadFacilityReadOnly" exists with action: "Read", module: "ProductionFacilities", user: "readonly", operating center: "opc"
	And an equipment "one" exists with facility: "one"
    And I am logged in as "readonly"
    When I visit the Show page for facility: "one"	
    When I click the "Notes" tab
    Then I should not see the toggleNewNote element
    When I click the "Documents" tab
    Then I should not see the toggleLinkDocument element
    And I should not see the toggleNewDocument element
	
Scenario: user cannot change Equipment Type once characteristics have been entered
	Given an equipment "one" exists with equipment type: "generator", equipment purpose: "generator", requested by: "D:", equipment status: "in service"
    And an equipment characteristic field "one" exists with equipment type: "generator", field type: "string", field name: "StringField", required: "true", is active: "true"
	And production prerequisites exist
	And I am logged in as "user"
	When I visit the Show page for equipment: "one"
	And I follow the edit link for equipment "one"
	Then I should be at the Edit page for equipment: "one"
    When I enter "some string value" into the StringField field
    And I press Save
    Then I should be at the Show page for equipment: "one"
	When I follow the edit link for equipment "one"
	#should see the display EquipmentTypeObj instead of the dropdown EquipmentType
	Then I should see a display for EquipmentTypeObj with equipment type "generator"
	And I should not see the field EquipmentType
	
Scenario: User can add a link
	Given an equipment "one" exists with equipment status: "in service"
	And a link type "one" exists with description: "link type one"
	And I am logged in as "user"
	When I visit the Show page for equipment: "one"
	And I click the "Links" tab
	And I press "Add Link"
	And I select link type "one" from the LinkType dropdown
	And I enter "https://foo.foo.com" into the Url field
	And I press "Save Link"
	And I wait for the page to reload
	And I click the "Links" tab
	Then I should see the following values in the linksTable table
	| Link Type     | Url                 |
	| link type one | https://foo.foo.com |

Scenario: User can remove a link
	Given an equipment "one" exists with identifier: "NJSB-1-EQID-1", equipment status: "in service"
	And a link type "one" exists with description: "link type one"
	And an equipment link "one" exists with equipment: "one", link type: "one", url: "https://mapcall.amwater.com"
	And I am logged in as "user"
	When I visit the Show page for equipment: "one"
	And I click the "Links" tab
	And I click ok in the dialog after pressing "Remove Link"
	And I wait for the page to reload
	Then the linksTable table should be empty

Scenario: User can search for equipment with no SAP Equipment ID
    Given an equipment "has" exists with s a p equipment id: "12345678"
    And an equipment "has not" exists
	And I am logged in as "user"
    When I visit the /Equipment/Search page
    And I select "Yes" from the HasNoSAPEquipmentId dropdown
    And I press Search    
    Then I should be at the Equipment page
    And I should see a link to the Show page for equipment "has not"
    And I should not see a link to the Show page for equipment "has"

Scenario: User can search for equipment by state / operating center / facility / facility area / facility sub area
	Given an equipment "one" exists with facility: "one", facility facility area: "one"
	And I am logged in as "user"
	When I visit the /Equipment/Search page
	And I select state "one" from the State dropdown
	And I select operating center "opc" from the OperatingCenter dropdown
	And I select facility "one" from the Facility dropdown
	And I select facility area "one" from the FacilityArea dropdown
	When I visit the /Equipment/Search page
	And I press "Search"
	Then I should be at the Equipment page
	Then I should see a link to the show page for equipment "one"
	When I visit the /Equipment/Search page
	And I select state "one" from the State dropdown
	And I select operating center "opc" from the OperatingCenter dropdown
	And I select facility "one" from the Facility dropdown
	And I select facility area "one" from the FacilityArea dropdown
	And I select facility sub area "one" from the FacilitySubArea dropdown
	When I visit the /Equipment/Search page
	And I press "Search"
	Then I should be at the Equipment page
	Then I should see a link to the show page for equipment "one"

Scenario: User can search for equipment by state and by state / operating center
	 Given an equipment "one" exists with facility: "one"
	 And I am logged in as "user"
	 When I visit the /Equipment/Search page
	 And I select state "one" from the State dropdown
	 And I press "Search"
	 Then I should be at the Equipment page
	 And I should see a link to the show page for equipment "one"
	 When I visit the /Equipment/Search page
	 And I select state "one" from the State dropdown
	 And I select operating center "opc" from the OperatingCenter dropdown
	 And I press "Search"
	 Then I should be at the Equipment page
	 And I should see a link to the show page for equipment "one"
	
Scenario: User can search for equipment by planning plant
	Given a planning plant "one" exists with operating center: "opc", code: "D217", description: "Argh"
	And a facility "facility" exists with operating center: "opc", facility id: "NJSB-1", facility name: "A Facility", planning plant: "one" 
	And an equipment "one" exists with facility: "facility"
	And I am logged in as "user"
	When I visit the /Equipment/Search page
	And I select state "one" from the State dropdown
	And I select operating center "opc" from the OperatingCenter dropdown
	And I select planning plant "one" from the PlanningPlant dropdown
	And I press "Search"
	Then I should see a link to the show page for equipment "one"

Scenario: User can search for equipment that has well tests data
	Given an equipment "equipment-without-well" exists with facility: "one", equipment type: "well", description: "well"
	And an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a production work order "pwo-01" exists with equipment: "equipment-with-well", operating center: "opc2", date completed: "02/10/2020 12:00"
	And a production work order equipment "pwoe-01" exists with production work order: "pwo-01", equipment: "equipment-with-well"
	And a well test "wt-01" exists with equipment: "equipment-with-well", employee: "D:", production work order: "pwo-01", pumping rate: "402", measurement point: "20.50", static water level: "500.50", pumping water level: "600.60", date of test: "3/31/2021"
	And I am logged in as "user"
	When I visit the /Equipment/Search page
	And I select "Yes" from the WellTests dropdown
	And I press "Search"
	Then I should be at the Equipment page
	And I should see a link to the show page for equipment "equipment-with-well"
	And I should not see a link to the show page for equipment "equipment-without-well"
	When I visit the /Equipment/Search page
	And I select "No" from the WellTests dropdown
	And I press "Search"
	Then I should be at the Equipment page
	And I should see a link to the show page for equipment "equipment-without-well"
	And I should not see a link to the show page for equipment "equipment-with-well"

Scenario: User should not see tab for well testing results if an equipment is not of type well
	Given an equipment "equipment-without-well" exists with facility: "one", equipment type: "generator", description: "generator"
	And I am logged in as "user"
	When I visit the Show page for equipment: "equipment-without-well"
	Then I should not see the "Well Test Results" tab

Scenario: User can see a tab for well tests when an equipment is of type well, as well as historical results data
	Given an equipment "equipment-with-well" exists with facility: "one", equipment type: "well", description: "well"
	And a well test grade type "bg" exists with description: "Below Grade"
	And a well test "wt-01" exists with equipment: "equipment-with-well", employee: "D:", production work order: "shoe", pumping rate: "402", measurement point: "20.50", static water level: "500.50", pumping water level: "600.60", date of test: "3/31/2021", grade type: "bg"
	And I am logged in as "user"
	When I visit the Show page for equipment: "equipment-with-well"
	Then I should see the "Well Test Results" tab
	When I click the "Well Test Results" tab
	Then I should see the following values in the well-tests-table table
	| Id                      | Date Of Test | Pumping Rate After 1 Hour (gpm) | Measurement Point (ft-above/below surface grade) | Above or Below Grade | Static Water Level (ft-bmp) | Pumping Water Level After 1 Hour (ft-bmp) | Drawdown (ft) | Specific Capacity (gpm/ft) | Employee       |
	| well test: "wt-01"'s Id | 3/31/2021    | 402                             | 20.50                                            | Below Grade          | 500.50                      | 600.60                                    | 100.10        | 4.02                       | employee: "D:" |

Scenario: User has functional locations cleared out when searching
	Given a planning plant "one" exists with operating center: "opc", code: "D217", description: "Argh"
	And a facility "facility" exists with operating center: "opc", facility id: "NJSB-1", facility name: "A Facility", planning plant: "one"
	And I am logged in as "user"
	When I visit the Equipment/Search page
	And I enter "12345" into the FunctionalLocation field
	Then I should see "12345" in the FunctionalLocation field
	When I select state "one" from the State dropdown
	Then I should not see "12345" in the FunctionalLocation field
	When I select operating center "opc" from the OperatingCenter dropdown
	Then I should not see "12345" in the FunctionalLocation field
	When I enter "12345" into the FunctionalLocation field
	Then I should see "12345" in the FunctionalLocation field
	When I select planning plant "one" from the PlanningPlant dropdown
	Then I should not see "12345" in the FunctionalLocation field
	When I enter "12345" into the FunctionalLocation field
	Then I should see "12345" in the FunctionalLocation field
	When I select facility "facility"'s Description from the Facility dropdown
	Then I should not see "12345" in the FunctionalLocation field

Scenario: user can set and edit characteristics
	Given an equipment "one1" exists with equipment purpose: "generator", equipment type: "generator", requested by: "D:", equipment status: "in service"
    And an equipment characteristic field "one" exists with equipment type: "generator", field type: "string", field name: "StringField", required: "true"
    And an equipment characteristic field "two" exists with equipment type: "generator", field type: "dropdown", field name: "DropDownField", required: "true"
    And an equipment characteristic field "three" exists with equipment type: "generator", field type: "currency", field name: "CurrencyField", required: "false"
    And an equipment characteristic drop down value "one" exists with field: "two", value: "asdf"
    And an equipment characteristic drop down value "two" exists with field: "two", value: "fdsa"
	And production prerequisites exist
	And I am logged in as "user"
	When I visit the Show page for equipment: "one1"
	And I follow the edit link for equipment "one1"
	Then I should be at the Edit page for equipment: "one1"
	When I press Save
    Then I should be at the Edit page for equipment: "one1"
    And I should see the validation message "The field StringField is required."
    And I should see the validation message "The field DropDownField is required."
    When I enter "some string value" into the StringField field
    And I select equipment characteristic drop down value "one"'s Value from the DropDownField dropdown
    And I enter "1234.56" into the CurrencyField field
    And I press Save
    Then I should be at the Show page for equipment: "one1"
	When I click the "Attributes" tab
	Then I should see a display for StringField with "some string value"
    And I should see a display for DropDownField with equipment characteristic drop down value "one"'s Value
	And I should see a display for CurrencyField with "$1,234.56"
    When I follow the edit link for equipment "one1"
    Then I should be at the Edit page for equipment: "one1"
    And I should see "some string value" in the StringField field
    When I enter "some other string value" into the StringField field
    And I press Save
    Then I should be at the Show page for equipment: "one1"
	When I click the "Attributes" tab
    Then I should see a display for StringField with "some other string value"

Scenario: User view measurement points
	Given a equipment "two" exists with equipment purpose: "generator", equipment type: "generator", requested by: "D:", equipment status: "in service", s a p equipment id: "2"
	And a user "testuser" exists with username: "stuff", full name: "the useriest user"
	And a production work order "two" exists with equipment: "two", operating center: "opc2", completed by: "testuser", date completed: "02/10/2020 12:00"
	And a production work order equipment "two" exists with production work order: "two", equipment: "two"
	And a production work order measurement point value exists with production work order: "two", equipment: "two", value: "2", measurement point equipment type: "one", measurement doc id: "123"
	And I am logged in as "user"
	When I visit the Show page for equipment: "two"
	And I click the "Measurement Points" tab
	Then I should see a link to the show page for production work order: "two" and fragment of "#MeasuringPointsTab"
	And I should see "2/10/2020 12:00:00 PM" in the table measurementPointsTable's "Date" column
	And I should see "stuff" in the table measurementPointsTable's "Employee" column
	And I should see "2" in the table measurementPointsTable's "test" column

Scenario: admin can copy equipment several times
	Given an equipment "one" exists with equipment status: "in service", equipment type: "generator"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Show page for equipment: "one"
    Then I should see the button "Copy"
    When I click ok in the dialog after pressing "Copy"
	Then the currently shown equipment shall henceforth be known throughout the land as "thing1"
	And I should be at the edit page for equipment "thing1"
	When I visit the Show page for equipment: "one"
	And I click ok in the dialog after pressing "Copy"
	Then the currently shown equipment shall henceforth be known throughout the land as "thing2"
	And I should be at the edit page for equipment "thing2"
	When I visit the Edit page for equipment "thing1"
	And I select equipment type "engine"'s Description from the EquipmentType dropdown
    And I wait for ajax to finish loading
	And I select equipment purpose "engine"'s ToString from the EquipmentPurpose dropdown
    And I wait for ajax to finish loading
    And I select "ENGINE" from the EquipmentManufacturer dropdown	
	And I select employee "D:"'s Description from the RequestedBy dropdown
	And I select equipment status "in service"'s ToString from the EquipmentStatus dropdown
	And I press Save
	Then I should be at the Show page for equipment "thing1"
	When I visit the Edit page for equipment "thing2"
	And I select equipment type "engine"'s Description from the EquipmentType dropdown
    And I wait for ajax to finish loading
	And I select equipment purpose "engine"'s ToString from the EquipmentPurpose dropdown
    And I wait for ajax to finish loading
    And I select "ENGINE" from the EquipmentManufacturer dropdown	
	And I select employee "D:"'s Description from the RequestedBy dropdown
	And I select equipment status "in service"'s ToString from the EquipmentStatus dropdown
	And I press Save
	Then I should be at the Show page for equipment "thing2"

Scenario: admin can copy equipment and saving with equipment purpose changes the id
	Given an equipment "one" exists with equipment status: "in service", equipment type: "generator"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Show page for equipment: "one"
    Then I should see the button "Copy"
    When I click ok in the dialog after pressing "Copy"
	Then the currently shown equipment shall henceforth be known throughout the land as "thing1"
	And I should be at the edit page for equipment "thing1"
	When I select equipment type "engine"'s Description from the EquipmentType dropdown
    And I wait for ajax to finish loading
	And I select equipment purpose "engine"'s ToString from the EquipmentPurpose dropdown
	And I select equipment type "engine"'s Description from the EquipmentType dropdown
    And I wait for ajax to finish loading
    And I select "ENGINE" from the EquipmentManufacturer dropdown	
	And I select employee "D:"'s Description from the RequestedBy dropdown
	And I select equipment status "in service"'s ToString from the EquipmentStatus dropdown
	And I press Save
	Then I should be at the Show page for equipment "thing1"
	And I should see a display for Identifier with "NJ7-4-ETTT-3"

Scenario: User can search which Equipment Has Open Lockouts
	Given a equipment "two" exists with equipment purpose: "generator", equipment type: "generator", requested by: "D:", equipment status: "in service", s a p equipment id: "2"
	And a user "testuser" exists with username: "stuff", full name: "the useriest user"
	And a production work order "two" exists with equipment: "two", operating center: "opc2", completed by: "testuser", date completed: "02/10/2020 12:00"
	And a production work order equipment "two" exists with production work order: "two", equipment: "two"
	And a lockout form "one" exists with lockout device: "one", production work order: "two", equipment: "two"
	And I am logged in as "admin"
	When I visit the Show page for equipment: "two"
	And I visit the /Equipment/Search page
	And I select "Yes" from the HasOpenLockoutForms dropdown
	And I press "Search"
	Then I should see a link to the show page for equipment "two"

Scenario: User can see Filter Media Tab when a record is associated with Equipment Detail Type	
	Given an equipment "filter" exists with equipment status: "in service", equipment type: "filter", equipment purpose: "filter"
	And an equipment "generator" exists with equipment status: "in service", equipment purpose: "generator"
	And I am logged in as "admin"
	When I visit the /Equipment/Search page	
	And I press "Search"
	Then I should see a link to the Show page for equipment: "filter"
	When I follow the Show link for equipment "filter"
	Then I should see the "Filter Media" tab	

Scenario: User can not see Filter Media Tab when a record is associated with Equipment Detail Type		
	Given a equipment "one" exists with equipment status: "in service", equipment type: "filter", equipment purpose: "filter"
	And an equipment "two" exists with equipment status: "in service", equipment type: "engine", equipment purpose: "engine"	
	And I am logged in as "admin"
	When I visit the /Equipment/Search page
	And I press "Search"	
	Then I should see a link to the Show page for equipment: "two"
	When I follow the Show link for equipment "two"
	Then I should not see the "Filter Media" tab	

Scenario: user cannot add equipment with a compliance selected
	Given I am logged in as "user"
	And production prerequisites exist
	When I visit the Equipment/New page	
	Then the HasProcessSafetyManagement checkbox should be disabled
	And the HasCompanyRequirement checkbox should be disabled
	And the HasRegulatoryRequirement checkbox should be disabled
	And the HasOshaRequirement checkbox should be disabled
	And the OtherCompliance checkbox should be disabled

Scenario: eam user can add equipment with a compliance selected
	Given I am logged in as "other"
	And production prerequisites exist
	When I visit the Equipment/New page	
	Then the HasProcessSafetyManagement checkbox should be enabled
	And the HasCompanyRequirement checkbox should be enabled
	And the HasRegulatoryRequirement checkbox should be enabled
	And the HasOshaRequirement checkbox should be enabled
	And the OtherCompliance checkbox should be enabled

Scenario: admin can add equipment with a compliance selected
	Given I am logged in as "admin"
	And production prerequisites exist
	When I visit the Equipment/New page	
	Then the HasProcessSafetyManagement checkbox should be enabled
	And the HasCompanyRequirement checkbox should be enabled
	And the HasRegulatoryRequirement checkbox should be enabled
	And the HasOshaRequirement checkbox should be enabled
	And the OtherCompliance checkbox should be enabled 

Scenario: user admin can edit equipment purpose
	Given an equipment "one" exists with identifier: "NJSB-1-EQID-1", equipment status: "in service", equipment type: "generator"
	And production prerequisites exist
	And I am logged in as "admin"
	When I visit the Edit page for equipment: "one"
	And I enter "" into the Description field
	And I press Save
	Then I should see the validation message The Description field is required.
	When I enter "description here" into the Description field
	And I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select facility facility area "two"'s Display from the FacilityFacilityArea dropdown
    And I select equipment type "aerator"'s Description from the EquipmentType dropdown
	And I select equipment purpose "aerator"'s ToString from the EquipmentPurpose dropdown
    Then I should see equipment manufacturer "lowry" in the EquipmentManufacturer dropdown
    When I select equipment manufacturer "lowry" from the EquipmentManufacturer dropdown	
    And I select abc indicator "one"'s ToString from the ABCIndicator dropdown	
	And I select equipment status "out of service" from the EquipmentStatus dropdown
	And I enter "serial 1" into the SerialNumber field
	And I enter "1/1/1908" into the DateInstalled field
	And I enter "safety dance" into the SafetyNotes field
	And I enter "take on me" into the MaintenanceNotes field
	And I enter "time after time" into the OperationNotes field
	And I enter "4321" into the SAPEquipmentId field
    And I select employee "D:"'s Description from the RequestedBy dropdown
	Given I can not see the ManufacturerOther field
	When I select equipment manufacturer "other" from the EquipmentManufacturer dropdown
	Then I should see the ManufacturerOther field 
	When I enter "other things" into the ManufacturerOther field
	And I click the "Risk Characteristics" tab
	And I select equipment condition "poor" from the Condition dropdown
	And I select equipment performance rating "poor" from the Performance dropdown
	And I select equipment static dynamic type "static" from the StaticDynamicType dropdown
	And I select equipment consequences of failure rating "low" from the ConsequenceOfFailure dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for equipment "one"
	And I should see a display for Identifier with "NJ4-1-ETTT-2"
	And I should see a display for Description with "description here"
    And I should see a display for Facility_Department with facility "one"'s Department
	And I should see a link to the Show page for facility: "one"
	And I should see a display for EquipmentPurpose with equipment purpose "aerator"'s ToString
    And I should see a display for EquipmentManufacturer with equipment manufacturer "other"
	And I should see a display for ManufacturerOther with "other things"
    And I should see a display for ABCIndicator with abc indicator "one"'s Description	
	And I should see a display for EquipmentStatus with equipment status "out of service"'s Description
	And I should see a display for SerialNumber with "serial 1"
	And I should see a display for Number with "1"
	And I should see a display for DateInstalled with "1/1/1908"
	And I should see a display for SafetyNotes with "safety dance"
	And I should see a display for MaintenanceNotes with "take on me"
	And I should see a display for OperationNotes with "time after time"
	And I should see a display for FunctionalLocation with "Oz"
	And I should see a display for SAPEquipmentId with "4321"
	When I click the "Risk Characteristics" tab
    Then I should see a display for Condition with equipment condition "poor"'s Description
	And I should see a display for Performance with equipment performance rating "poor"'s Description
	And I should see a display for StaticDynamicType with equipment static dynamic type "static"'s Description
	And I should see a display for ConsequenceOfFailure with equipment consequences of failure rating "low"'s Description

Scenario: User can search for equipment that has open red tag permits
	Given an equipment "equipment-with-closed-red-tag-permit" exists with facility: "one", equipment type: "fire suppression", description: "equipment with closed red tag"
	And an equipment "equipment-with-open-red-tag-permit" exists with facility: "one", equipment type: "fire suppression", description: "equipment with open red tag"
	And a production work order "pwo-with-closed-red-tag-permit" exists with equipment: "equipment-with-open-red-tag-permit", operating center: "opc2", date completed: "02/10/2020 12:00"
	And a production work order "pwo-with-open-red-tag-permit" exists with equipment: "equipment-with-closed-red-tag-permit", operating center: "opc2", date completed: "02/10/2020 12:00"
	And a red tag permit "rtp-open" exists with equipment: "equipment-with-open-red-tag-permit", person responsible: "D:", production work order: "pwo-with-open-red-tag-permit"
	And a red tag permit "rtp-closed" exists with equipment: "equipment-with-closed-red-tag-permit", person responsible: "D:", production work order: "pwo-with-closed-red-tag-permit", equipment restored on: "04/28/2021"
	And I am logged in as "user"
	When I visit the /Equipment/Search page
	And I check the HasOpenRedTagPermits field
	And I press "Search"
	Then I should be at the Equipment page
	And I should see a link to the show page for equipment "equipment-with-open-red-tag-permit"
	And I should not see a link to the show page for equipment "equipment-with-closed-red-tag-permit"
	When I visit the /Equipment/Search page
	And I uncheck the HasOpenRedTagPermits field
	And I press "Search"
	Then I should be at the Equipment page
	And I should see a link to the show page for equipment "equipment-with-open-red-tag-permit"
	And I should see a link to the show page for equipment "equipment-with-closed-red-tag-permit"

Scenario: User should not see tab for red tag permits when it is not a fire suppression equipment type
	Given an equipment "equipment-without-red-tag-permit-eligibility" exists with facility: "one", equipment type: "generator", description: "generator"
	And I am logged in as "user"
	When I visit the Show page for equipment: "equipment-without-red-tag-permit-eligibility"
	Then I should not see the "Red Tag Permits" tab

Scenario: User can see a tab for red tag permits when an equipment is of type fire suppression, as well as historical results data
	Given an equipment "e" exists with facility: "one", equipment type: "fire suppression", description: "fire suppression"
	And a production work description "pwd" exists with equipment type: "fire suppression", description: "fix the sprinklers"
	And a production work order "pwo" exists with production work description: "pwd", equipment: "e", operating center: "opc2", date completed: "02/10/2020 12:00"
	And a red tag permit "rtp" exists with equipment: "e", person responsible: "D:", authorized by: "D:", production work order: "pwo", equipment impaired on: "4/21/2021 12:13:48 PM", equipment restored on: "4/28/2021 10:35:00 AM"
	And I am logged in as "user"
	When I visit the Show page for equipment: "e"
	Then I should see the "Red Tag Permits" tab
	When I click the "Red Tag Permits" tab
	Then I should see the following values in the red-tag-permits-table table
	| Production Work Order              | Person Responsible | Authorized By  | Date Equipment Impaired | Date Equipment Restored | Work Description                 | Operating Center                                | Facility                                 |
	| production work order: "pwo"'s Id | employee: "D:"     | employee: "D:" | 4/21/2021 12:13:48 PM           | 4/28/2021 10:35:00 AM           | fix the sprinklers | NJ4 - Lakewood | Facility 0 - NJ7-4 |

Scenario: User can see the tab for maintenance plan and remove maintenance plan 
	Given I am logged in as "admin" 
	And a state "two" exists with abbreviation: "NJ"
	And an operating center "nj7" exists with state: "two", opcode: "NJ7"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
	And an equipment purpose "one" exists with description: "one", equipment type: "well", equipment category: "one", equipment subcategory: "one"
	And a maintenance plan task type "one" exists with description: "A good plan"
	And a task group category "one" exists with description: "This is the description", type: "chemical", abbreviation: "CHEM", is active: "true"
	And a skill set "one" exists with name: "Skill name", abbreviation: "SkAbr", is active: "true", description: "This is the description"    
	And a task group "one" exists with task group id: "Task Id 1", task group name: "This is group 1", task details: "Task details 1", task details summary: "Task summary 1", task group category: "one", resources: "1", estimated hours: "2", contractor cost: "3", equipment types: "well", task group categories: "one", skill set: "one", maintenance plan task type: "one"
	And production work order frequencies exist
	And a production work order priority "one" exists with description: "Routine - Off Scheduled"
	And a maintenance plan "one" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "two", equipment types: "fire suppression", equipment purposes: "one", task group: "one", production work order frequency: "daily", production work order priority: "one", is active: "true", equipment: "one"
	And maintenance plan "one" exists in equipment "shoe"
	When I visit the equipment/Show/1 page
    And I click the "Maintenance Plans" tab
	Then I should see the following values in the equipmentMaintenancePlanTable table
	| Plan Number	| Plan Type		 | Compliance Plan	| Is Active |	Last Work Order Completed |
	| 900000001 	| A good plan    | No				| Yes		|							  |
	When I click the "Remove from Plan" button in the 1st row of equipmentMaintenancePlanTable
	And I wait for the page to reload
	Then I should see "No matching maintenance plans were found."

Scenario: User can see the tab for facility maintenance plans and add plan 
	Given I am logged in as "admin" 
	And a state "two" exists with abbreviation: "NJ"
	And an operating center "nj7" exists with state: "two", opcode: "NJ7"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
	And an equipment purpose "one" exists with description: "one", equipment type: "well", equipment category: "one", equipment subcategory: "one"
	And a maintenance plan task type "one" exists with description: "A good plan"
	And a task group category "one" exists with description: "This is the description", type: "chemical", abbreviation: "CHEM", is active: "true"
	And a skill set "one" exists with name: "Skill name", abbreviation: "SkAbr", is active: "true", description: "This is the description"    
	And a task group "one" exists with task group id: "Task Id 1", task group name: "This is group 1", task details: "Task details 1", task details summary: "Task summary 1", task group category: "one", resources: "1", estimated hours: "2", contractor cost: "3", equipment types: "well", task group categories: "one", skill set: "one", maintenance plan task type: "one"
	And production work order frequencies exist
	And a production work order priority "one" exists with description: "Routine - Off Scheduled"
	And a maintenance plan "one" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "generator", equipment purposes: "one", task group: "one", production work order frequency: "daily", production work order priority: "one", is active: "true", equipment: "one"
	When I visit the equipment/Show/1 page
    And I click the "Facility Maintenance Plans" tab
	Then I should see the following values in the facilityMaintenancePlanTable table
	| Plan Number | Plan Name								| Plan Type		 | Compliance Plan	| Is Active |	Last Work Order Completed |
	| 900000001   | Facility 0 : This is group 1 : DAILY	| A good plan	 | No				| Yes		|							  |
	When I click the "Add to Plan" button in the 1st row of facilityMaintenancePlanTable
	Then I should see the validation message The MaintenancePlan field is required.
	When I select maintenance plan "one"'s Description from the FacilityMaintenancePlan dropdown
	And I click the "Add to Plan" button in the 1st row of facilityMaintenancePlanTable
	Then I should be at the Show page for equipment "shoe"
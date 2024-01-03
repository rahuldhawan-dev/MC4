Feature: FacilityRiskCharacteristicsDataPage

Background: all the things exist
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "ProductionFacilities", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionFacilities", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "ProductionFacilities", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionFacilities", user: "user"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a state "two" exists with name: "New York", abbreviation: "NY"
	And a town "one" exists with stateId: "one"
	And a town "two" exists with stateId: "two"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "one", IsActive = true
	And an operating center "ny1" exists with opcode: "NY1", name: "Rockland, companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "two", IsActive = true
	And a planning plant "one" exists with operating center: "nj4", code: "D217", description: "PPF1"
	And a planning plant "two" exists with operating center: "ny1", code: "P217", description: "PPF2"
	And a facility "one" exists with operating center: "nj4", facility name: "NJ Facility", town: "one", planning plant: "one"
	And a facility "two" exists with operating center: "ny1", facility name: "NY Facility", town: "two", planning plant: "two"
	And a role "roleFacilityReadOpc" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "roleFacilityAddOpc" exists with action: "Add", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "roleFacilityEditOpc" exists with action: "Edit", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "roleFacilityReadOpc1" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "ny1"
	And a role "roleFacilityAddOpc1" exists with action: "Add", module: "ProductionFacilities", user: "user", operating center: "ny1"
	And a role "roleFacilityEditOpc1" exists with action: "Edit", module: "ProductionFacilities", user: "user", operating center: "ny1"

Scenario: User can search and view results
	Given I am logged in as "user"
	When I visit the Reports/FacilityRiskCharacteristicData/Search page 
	And I select state "one" from the TownState dropdown
	And I select state "two" from the TownState dropdown
	And I press Search
	Then I should see the following values in the results table
	| Id | Operating Center | Planning Plant | Facility Name |
	| 1  | NJ4 - Lakewood   | D217 - NJ4 - PPF1 | NJ Facility   |
	| 2  | NY1 - "Rockland  | P217 - NY1 - PPF2 | NY Facility   |
	When I visit the Reports/FacilityRiskCharacteristicData/Search page
	And I select state "one" from the TownState dropdown
	And I select state "two" from the TownState dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select operating center "ny1" from the OperatingCenter dropdown
	And I press Search
	Then I should see the following values in the results table
	| Id | Operating Center | Planning Plant | Facility Name |
	| 1  | NJ4 - Lakewood   | D217 - NJ4 - PPF1 | NJ Facility   |
	| 2  | NY1 - "Rockland  | P217 - NY1 - PPF2 | NY Facility   |
	When I visit the Reports/FacilityRiskCharacteristicData/Search page 
	And I select state "one" from the TownState dropdown
	And I select state "two" from the TownState dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select operating center "ny1" from the OperatingCenter dropdown
	And I select planning plant "one" from the PlanningPlant dropdown
	And I select planning plant "two" from the PlanningPlant dropdown
	And I press Search
	Then I should see the following values in the results table
	| Id | Operating Center | Planning Plant | Facility Name |
	| 1  | NJ4 - Lakewood   | D217 - NJ4 - PPF1 | NJ Facility   |
	| 2  | NY1 - "Rockland  | P217 - NY1 - PPF2 | NY Facility   |
	When I visit the Reports/FacilityRiskCharacteristicData/Search page 
	And I select state "one" from the TownState dropdown
	And I select state "two" from the TownState dropdown
	And I select operating center "nj4" from the OperatingCenter dropdown
	And I select operating center "ny1" from the OperatingCenter dropdown
	And I select planning plant "one" from the PlanningPlant dropdown
	And I select planning plant "two" from the PlanningPlant dropdown
	And I select "NJ Facility - NJ4-1" from the Id dropdown
	And I select "NY Facility - NY1-2" from the Id dropdown
	And I press Search
	Then I should see the following values in the results table
	| Id | Operating Center | Planning Plant | Facility Name |
	| 1  | NJ4 - Lakewood   | D217 - NJ4 - PPF1 | NJ Facility   |
	| 2  | NY1 - "Rockland  | P217 - NY1 - PPF2 | NY Facility   |
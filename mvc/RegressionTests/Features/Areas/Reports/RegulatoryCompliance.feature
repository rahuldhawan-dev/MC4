Feature: RegulatoryCompliance
	To ensure this reports calculations and search features work correctly
	As a developer
	I'm going to write a ton of tests

Background: 
	Given a user "user" exists with username: "user"
	And a state "nj" exists with abbreviation: "NJ"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true", state: "nj"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", sap enabled: "true", sap work orders enabled: "true", state: "nj"
	And production work order priorities exist
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with operating center: "nj7", status: "active", employee id: "12345678", first name: "jay", last name: "bob"
	And a role "productionworkorder-read-nj7" exists with action: "Read", module: "ProductionWorkManagement", user: "user", operating center: "nj7"
	And a role "productionworkorder-read-nj4" exists with action: "Read", module: "ProductionWorkManagement", user: "user", operating center: "nj4"
	And a role "equipment-read-nj7" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "nj7"
	And a role "equipment-read-nj4" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "nj4"
	And a role "facility-read1" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And a role "facility-read2" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "publicwatersupply-read1" exists with action: "Read", module: "EnvironmentalGeneral", user: "user", operating center: "nj7"
	And a role "publicwatersupply-read2" exists with action: "Read", module: "EnvironmentalGeneral", user: "user", operating center: "nj4"
	And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
	And a planning plant "two" exists with operating center: "nj4", code: "D205", description: "Lakewood"
	And public water supply statuses exist
	And a public water supply "one" exists with identifier: "IL0995030", operating area: "EMD", system: "Streator", status: "active"
	And a public water supply "two" exists with identifier: "NJ0712001", operating area: "Passaic Operating Area", system: "Short Hills/Passaic", status: "active"
	And a facility "one" exists with operating center: "nj7", facility id: "NJSB-1", facility name: "A Facility", planning plant: "one", public water supply: "one"
	And a facility "two" exists with operating center: "nj4", facility id: "NJSB-2", facility name: "A Facility", planning plant: "two", public water supply: "two"
	And order types exist
	And a production work description "one" exists with description: "one", order type: "operational activity"
	And a production work description "two" exists with description: "two", order type: "pm work order"
	And a production work description "three" exists with description: "three", order type: "corrective action"
	And a production work description "four" exists with description: "four", order type: "rp capital"
	And an equipment type "generator" exists with description: "Generator"
    And an equipment type "engine" exists with description: "Engine"
	And an equipment type "aerator" exists with description: "Aerator"
	And an equipment purpose "aerator" exists with description: "aerator", equipment type: "aerator"
	And an equipment purpose "generator" exists with description: "generator", equipment type: "generator"
    And an equipment purpose "engine" exists with description: "engine", equipment type: "engine"
	And an equipment "shoe1" exists with equipment type: "generator", HasRegulatoryRequirement: "true"
	And an equipment "shoe2" exists with equipment type: "aerator", HasRegulatoryRequirement: "true", EquipmentPurpose: "aerator"
	And an equipment "shoe3" exists with equipment type: "engine"
	And a production work order "shoe" exists with productionWorkDescription: "two", operating center: "nj7", planning plant: "one", facility: "one"
	And a production work order equipment "pwoe" exists with production work order: "shoe", equipment: "shoe1"
	And a production work order "unscheduled" exists with date received: "10/1/2019", productionWorkDescription: "two", operating center: "nj7", planning plant: "one", facility: "one"
	And a production work order "scheduled" exists with date received: "10/1/2019", productionWorkDescription: "two", operating center: "nj7", planning plant: "one", facility: "one"
	And a production work order "incomplete" exists with date received: "10/1/2019", productionWorkDescription: "two", operating center: "nj7", planning plant: "one", facility: "one"
	And a production work order "canceled" exists with date received: "10/1/2019", productionWorkDescription: "two", operating center: "nj7", planning plant: "one", facility: "one", date cancelled: "10/1/2019"
	And a production work order "completed" exists with date received: "10/1/2019", productionWorkDescription: "two", operating center: "nj4", planning plant: "two", facility: "two", date completed: "10/1/2019"
	And a production work order equipment "unscheduled" exists with production work order: "unscheduled", equipment: "shoe1"
	And a production work order equipment "scheduled" exists with production work order: "scheduled", equipment: "shoe1"
	And a production work order equipment "incomplete" exists with production work order: "incomplete", equipment: "shoe1"	
	And a production work order equipment "completed" exists with production work order: "completed", equipment: "shoe1"
	And a production work order equipment "canceled" exists with production work order: "canceled", equipment: "shoe1"

Scenario: User can search with only by date
	Given I am logged in as "user"
	And I am at the Reports/RegulatoryComplianceReport/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I press Search
	Then I should see the following values in the results table
         | State | Operating Center | Public Water Supply                                       | Facility    | Equipment Description | Compliance Flag Values                                | # of WO's Completed  | Date of Last Completed Order | # of WO's Incomplete | # of WO's Cancelled |
         | NJ    | NJ4 - Lakewood   | NJ0712001 - Passaic Operating Area - Short Hills/Passaic  | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 1                    | 10/1/2019 12:00:00 AM        | 0                    | 0                   |
		 | NJ    | NJ7 - Shrewsbury | IL0995030 - EMD - Streator                                | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 0                    |                              | 3                    | 1                   |
    When I click the "1" link under "# of WO's Completed" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "3" link under "# of WO's Incomplete" in the 2nd row of results 
	Then I should see a link to the Show page for production work order: "unscheduled"
	And I should see a link to the Show page for production work order: "scheduled"
	And I should see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Cancelled" in the 2nd row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"

Scenario: User searching only by state
	Given I am logged in as "user"
	And I am at the Reports/RegulatoryComplianceReport/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select state "nj" from the State dropdown
	And I press Search
	Then I should see the following values in the results table
         | State | Operating Center | Public Water Supply                                       | Facility    | Equipment Description | Compliance Flag Values                                | # of WO's Completed  | Date of Last Completed Order | # of WO's Incomplete | # of WO's Cancelled |
         | NJ    | NJ4 - Lakewood   | NJ0712001 - Passaic Operating Area - Short Hills/Passaic  | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 1                    | 10/1/2019 12:00:00 AM        | 0                    | 0                   |
		 | NJ    | NJ7 - Shrewsbury | IL0995030 - EMD - Streator                                | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 0                    |                              | 3                    | 1                   |
    When I click the "1" link under "# of WO's Completed" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "3" link under "# of WO's Incomplete" in the 2nd row of results 
	Then I should see a link to the Show page for production work order: "unscheduled"
	And I should see a link to the Show page for production work order: "scheduled"
	And I should see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Cancelled" in the 2nd row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"

Scenario: User searching by operating center
	Given I am logged in as "user"
	And I am at the Reports/RegulatoryComplianceReport/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press Search
	Then I should see the following values in the results table
         | State | Operating Center | Public Water Supply           | Facility    | Equipment Description | Compliance Flag Values                                | # of WO's Completed  | Date of Last Completed Order | # of WO's Incomplete | # of WO's Cancelled |
         | NJ    | NJ7 - Shrewsbury | IL0995030 - EMD - Streator    | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 0                    |                              | 3                    | 1                   |
	When I click the "3" link under "# of WO's Incomplete" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "unscheduled"
	And I should see a link to the Show page for production work order: "scheduled"
	And I should see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Cancelled" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"

Scenario: User searching by planning plant
	Given I am logged in as "user"
	And I am at the Reports/RegulatoryComplianceReport/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select planning plant "one" from the PlanningPlant dropdown
	And I press Search
	Then I should see the following values in the results table
         | State | Operating Center | Public Water Supply           | Facility    | Equipment Description | Compliance Flag Values                                | # of WO's Completed  | Date of Last Completed Order | # of WO's Incomplete | # of WO's Cancelled |
         | NJ    | NJ7 - Shrewsbury | IL0995030 - EMD - Streator    | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 0                    |                              | 3                    | 1                   |
	When I click the "3" link under "# of WO's Incomplete" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "unscheduled"
	And I should see a link to the Show page for production work order: "scheduled"
	And I should see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Cancelled" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"

Scenario: User searching by facility
	Given I am logged in as "user"
	And I am at the Reports/RegulatoryComplianceReport/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select state "nj" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select planning plant "one" from the PlanningPlant dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I press Search
	Then I should see the following values in the results table
         | State | Operating Center | Public Water Supply           | Facility    | Equipment Description | Compliance Flag Values                                | # of WO's Completed  | Date of Last Completed Order | # of WO's Incomplete | # of WO's Cancelled |
         | NJ    | NJ7 - Shrewsbury | IL0995030 - EMD - Streator    | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 0                    |                              | 3                    | 1                   |
	When I click the "3" link under "# of WO's Incomplete" in the 1st row of results 
	Then I should see a link to the Show page for production work order: "unscheduled"
	And I should see a link to the Show page for production work order: "scheduled"
	And I should see a link to the Show page for production work order: "incomplete"
	And I should not see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
	When I go back
	And I click the "1" link under "# of WO's Cancelled" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"

Scenario: User searching by equipment type
	Given a production work order equipment "canceled2" exists with production work order: "canceled", equipment: "shoe2"
	And I am logged in as "user"
	And I am at the Reports/RegulatoryComplianceReport/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select equipment type "aerator" from the EquipmentType dropdown
	And I press Search
	Then I should see the following values in the results table
         | State | Operating Center | Public Water Supply           | Facility    | Equipment Description | Compliance Flag Values                                | # of WO's Completed  | Date of Last Completed Order | # of WO's Incomplete | # of WO's Cancelled |
         | NJ    | NJ7 - Shrewsbury | IL0995030 - EMD - Streator    | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 0                    |                              | 0                    | 1                   |
	When I click the "1" link under "# of WO's Cancelled" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"

Scenario: User searching by equipment purpose
	Given a production work order equipment "canceled2" exists with production work order: "canceled", equipment: "shoe2"
	And I am logged in as "user"
	And I am at the Reports/RegulatoryComplianceReport/Search page
	When I select "=" from the DateReceived_Operator dropdown
	And I enter "10/1/2019" into the DateReceived_End field
	And I select equipment type "aerator" from the EquipmentType dropdown
	And I select equipment purpose "aerator" from the EquipmentPurpose dropdown
	And I press Search
	Then I should see the following values in the results table
	     | State | Operating Center | Public Water Supply         | Facility    | Equipment Description | Compliance Flag Values                                | # of WO's Completed  | Date of Last Completed Order | # of WO's Incomplete | # of WO's Cancelled |
		 | NJ    | NJ7 - Shrewsbury | IL0995030 - EMD - Streator  | A Facility  | Equipment             | Environmental / Water Quality Regulatory Requirement  | 0                    |                              | 0                    | 1                   |
	When I click the "1" link under "# of WO's Cancelled" in the 1st row of results 
	Then I should not see a link to the Show page for production work order: "unscheduled"
	And I should not see a link to the Show page for production work order: "scheduled"
	And I should not see a link to the Show page for production work order: "incomplete"
	And I should see a link to the Show page for production work order: "canceled"
	And I should not see a link to the Show page for production work order: "completed"
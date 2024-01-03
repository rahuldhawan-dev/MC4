Feature: NearMissPage

Background: data exists
    Given an admin user "intranetuser" exists with username: "intranetuser"
    And a state "NJ" exists with abbreviation: "NJ"
    And a town "Sometown" exists with state: "NJ"
    And an operating center "one" exists with opcode: "one" 
    And an operating center "two" exists with opcode: "two", town: "Sometown"
    And an employee status "active" exists with description: "Active"
    And an employee "mickey" exists with first name: "Mickey", last name: "Mouse", employee id: "11111117", operating center: "two", status: "active"
    And operating center "two" exists in town "Sometown"
    And a facility "one" exists with operating center: "two", facility id: "NJSB-01"
    And a serious injury or fatality type "one" exists with description: "one"
    And a life saving rule type "one" exists with description: "one" 
    And a stop work usage type "one" exists with description: "one"
    And a near miss type "environmental" exists with description: "Environmental"
    And a near miss type "safety" exists with description: "Safety"
    And a work order type "t&d" exists with description: "T&D"
    And a work order "one" exists with Id: "1"
    And a system type "drinkingwater" exists with description: "drinkingwater"
    And public water supply statuses exist
    And a public water supply "one" exists with identifier: "1111", status: "active", state: "NJ", system: "System"
    And operating center: "two" exists in public water supply: "one"
    And a near miss category "other" exists with description: "Other", type: "safety"
    And a near miss category "stormwater" exists with description: "StormWater", type: "environmental"
    And a near miss sub category "ec" exists with category: "stormwater", description: "Erosion Controls"
    And a near miss category "ergonomics" exists with description: "Ergonomics", type: "safety"
    And a near miss sub category "lifting" exists with category: "ergonomics", description: "lifting"
    And a action taken type "one" exists with description: "Reported_To_My_Supervisor"
    And a severity type "one" exists with description: "Green"
    And a severity type "three" exists with description: "RED"
    And a Stop Work Usage Type "one" exists with description: "Employee"
    And a near miss "one" exists with operating center: "one", incident number: "one" , LocationDetails: "Location" , WorkOrderNumber: "1234"
    And a near miss "two" exists with operating center: "two" 
    And a near miss "three" exists
    And a data type "nearmisses" exists with table name: NearMisses, name: "NearMisses"
    And a document type "nearmisses" exists with data type: "nearmisses", name: "Near Miss Document"

Scenario: Validation for creating a new nearmiss
    Given I am logged in as "intranetuser"
    When I select state "NJ" from the State dropdown
    And I select "No" from the NotCompanyFacility dropdown
	And I check the ReportAnonymously field
	And I select action taken type "one" from the ActionTakenType dropdown
	And I select near miss type "environmental" from the Type dropdown
    And I select near miss category "stormwater" from the Category dropdown
    And I press Save 
    Then I should see the validation message "The Description of Near Miss field is required."
    And I should see the validation message "The OccurredAt field is required."
    And  I should see the validation message "The OperatingCenter field is required." 
    And  I should see the validation message "The Near Miss Completely Resolved field is required."

Scenario: User creates a new nearmiss
	Given I am logged in as "intranetuser"
    And I am at the /?Type=1 page
    And a coordinate "one" exists
    When I select state "NJ" from the State dropdown
	And I select operating center "two" from the OperatingCenter dropdown
	And I select town "Sometown" from the Town dropdown
    And I select "No" from the NotCompanyFacility dropdown
	And I check the ReportAnonymously field
    And I enter coordinate "one"'s Id into the Coordinate field
    And I enter "1/24/2014" into the OccurredAt field
	And I enter "Erosion Controls" into the Description field
	And I select action taken type "one" from the ActionTakenType dropdown
    And I select near miss category "stormwater" from the Category dropdown
    And I select action taken type "one" from the ActionTakenType dropdown
    And I select system type "drinkingwater" from the SystemType dropdown
    And I select public water supply "one"'s NearMissDescription from the PublicWaterSupply dropdown
    And I select "Yes" from the CompletedCorrectiveActions dropdown
    And I enter "Some Action Taken" into the ActionTaken field
    And I select near miss category "stormwater" from the Category dropdown
    And I press Save
	Then I should be at the NearMiss/Show/4 page
    And I should see "Thank you for your Near Miss submission."

Scenario: User creates a new safety nearmiss
	Given I am logged in as "intranetuser"    
    And a coordinate "one" exists
    When I select state "NJ" from the State dropdown
	And I select operating center "two" from the OperatingCenter dropdown
	And I select town "Sometown" from the Town dropdown
    And I select "No" from the NotCompanyFacility dropdown
	And I check the ReportAnonymously field
    And I enter coordinate "one"'s Id into the Coordinate field
    And I select near miss type "safety" from the Type dropdown
    And I select work order type "t&d" from the WorkOrderType dropdown
    And I enter "1" into the WorkOrder field
    And I enter "1/24/2014" into the OccurredAt field
	And I enter "Erosion Controls" into the Description field
	And I select action taken type "one" from the ActionTakenType dropdown
    And I enter "Some Action Taken" into the ActionTaken field
    And I select near miss category "ergonomics" from the Category dropdown
    And I select severity type "one" from the Severity dropdown
    And I multi upload "test1.jpg" to the file field
	And I wait 2 second
    And I press Save 
	Then I should be at the NearMiss/Show/4 page
    And I should see "Thank you for your Near Miss submission."

Scenario: User creates a new safety nearmiss with stop work authority
	Given I am logged in as "intranetuser"
    And a coordinate "one" exists
    When I select state "NJ" from the State dropdown
	And I select operating center "two" from the OperatingCenter dropdown
	And I select town "Sometown" from the Town dropdown
    And I select "Yes" from the NotCompanyFacility dropdown
    And I select facility "one"'s Description from the Facility dropdown
	And I check the ReportAnonymously field
    And I enter coordinate "one"'s Id into the Coordinate field
    And I select near miss type "safety" from the Type dropdown
    And I select work order type "t&d" from the WorkOrderType dropdown
    And I enter "1" into the WorkOrder field
    And I enter "1/24/2014" into the OccurredAt field
	And I enter "Erosion Controls" into the Description field
	And I select action taken type "one" from the ActionTakenType dropdown
    And I enter "Some Action Taken" into the ActionTaken field
    And I select near miss category "ergonomics" from the Category dropdown
    And I select severity type "three" from the Severity dropdown
    And I check the StopWorkAuthorityPerformed field
    And I select Stop Work Usage Type "one" from the StopWorkUsageType dropdown
    And I check the SubmittedOnBehalfOfAnotherEmployee field
    And I select employee "mickey"'s Description from the Employee dropdown
    And I press Save
	Then I should be at the NearMiss/Show/4 page
    And I should see "Thank you for your Near Miss submission."

Scenario: User can logout and redirected to okta login page
	Given I am logged in as "intranetuser"
    And I have pressed "logout-button"
	Then I should be at the / page

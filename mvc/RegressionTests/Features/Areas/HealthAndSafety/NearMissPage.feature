Feature: NearMissPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a state "NJ" exists with abbreviation: "NJ"
    And a town "Sometown" exists with state: "NJ"
    And an operating center "one" exists with opcode: "one"
    And an operating center "two" exists with opcode: "two", town: "Sometown"
    And operating center "two" exists in town "Sometown"
    And a facility "one" exists with operating center: "two", facility id: "NJSB-01"
    And a severity type "green" exists with description: "green"
    And a life saving rule type "one" exists with description: "one"
    And a stop work usage type "one" exists with description: "one"
    And a action taken type "one" exists with description: "submittedaworkorder"
    And a near miss type "environmental" exists with description: "Environmental"
    And a near miss type "safety" exists with description: "Safety"
    And a work order type "t&d" exists with description: "T&D"
    And a work order "one" exists with description: "one"
    And a near miss category "other" exists with description: "Other", type: "safety"
    And a system type "drinkingwater" exists with description: "drinkingwater"
    And public water supply statuses exist
    And a public water supply "one" exists with identifier: "1111", status: "active", state: "NJ", system: "System"
    And operating center: "two" exists in public water supply: "one"
    And a near miss category "stormwater" exists with description: "StormWater", type: "environmental"
    And a near miss sub category "ec" exists with category: "stormwater", description: "Erosion Controls"
    And a near miss "one" exists with operating center: "one", incident number: "one", reported by: "Manoj Patel"
    And a near miss "two" exists with operating center: "two", date completed: "02/23/2022 12:00", reported by: "Reported By 1"
    And a near miss "three" exists
    And I am logged in as "admin"

Scenario: user can search for a near miss
    When I visit the HealthAndSafety/NearMiss/Search page
    And I press Search
    Then I should see a link to the Show page for near miss: "one"
    And I should see a link to the Show page for near miss: "two"
    And I should see a link to the Show page for near miss: "three"
    When I follow the Show link for near miss "one"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
    Then I should be at the Show page for near miss: "one"
    When I visit the HealthAndSafety/NearMiss/Search page
    And I select state "NJ" from the State dropdown
    And I press Search
    Then I should see a link to the Show page for near miss: "one"
    And I should see a link to the Show page for near miss: "two"
    And I should not see a link to the Show page for near miss: "three"
    When I visit the HealthAndSafety/NearMiss/Search page
    And I select operating center "one" from the OperatingCenter dropdown
    And I press Search
    Then I should see a link to the Show page for near miss: "one"
    And I should not see a link to the Show page for near miss: "two"
    And I should not see a link to the Show page for near miss: "three"
    When I visit the HealthAndSafety/NearMiss/Search page
    And I enter "one" into the IncidentNumber field
    And I press Search
    Then I should see a link to the Show page for near miss: "one"
    And I should not see a link to the Show page for near miss: "two"
    And I should not see a link to the Show page for near miss: "three"
    When I visit the HealthAndSafety/NearMiss/Search page
    And I select "Yes" from the Completed dropdown
    And I press Search
    Then I should see a link to the Show page for near miss: "two"
    And I should not see a link to the Show page for near miss: "one"
    And I should not see a link to the Show page for near miss: "three"
    When I visit the HealthAndSafety/NearMiss/Search page
    And I enter "Man" into the ReportedBy field 
    And I press Search
    Then I should see a link to the Show page for near miss: "one"
    And I should not see a link to the Show page for near miss: "two"
    And I should not see a link to the Show page for near miss: "three"

Scenario: user can edit a safety near miss 
    When I visit the Edit page for near miss: "one"
    And I select operating center "two" from the OperatingCenter dropdown
    And I select town "Sometown" from the Town dropdown
    And I select facility "one" from the Facility dropdown
    And I enter "Location" into the LocationDetails field
    And I select near miss type "safety" from the Type dropdown
    And I select life saving rule type "one" from the LifeSavingRuleType dropdown
    And I select near miss category "other" from the Category dropdown
    And I enter "Description 0 - Other" into the DescribeOther field
    And I select action taken type "one" from the ActionTakenType dropdown
    And I select work order type "t&d" from the WorkOrderType dropdown
    And I select severity type "green" from the Severity dropdown
    And I enter "1" into the WorkOrder field
    And I enter "ActionTaken" into the ActionTaken field
    And I enter 10/17/2021 into the DateCompleted field
    And I enter "Description 0 - updated" into the Description field
    And I press Save
    Then I should be at the Show page for near miss: "one"
    And I should see a display for OperatingCenter with operating center "two"'s Description
    And I should see a display for LifeSavingRuleType with life saving rule type "one"'s Description
    And I should see a display for Description with "Description 0 - updated"

Scenario: user can view a near miss
    When I visit the Show page for near miss: "one"
    Then I should see a display for near miss: "one"'s ReportedBy
    Then I should see a display for near miss: "one"'s Facility
    Then I should see a display for near miss: "one"'s Town
    Then I should see a display for near miss: "one"'s LocationDetails

Scenario: Validation for edting nearmiss
    When I visit the Edit page for near miss: "three"
    And I press Save
    Then I should see the validation message "The State field is required."
    Then I should see the validation message "The Near Miss Type field is required."
    When I select state "NJ" from the State dropdown
    And I select near miss type "environmental" from the Type dropdown
    And I enter "" into the Description field
    And I press Save
    Then I should see the validation message "The Description of Near Miss field is required."
    Then I should see the validation message "The OperatingCenter field is required." 
    Then I should see the validation message "The Category field is required." 
    Then I should see the validation message "The SystemType field is required." 
    And  I should see the validation message "The Near Miss Completely Resolved field is required."
    When I select operating center "two" from the OperatingCenter dropdown
    And I select system type "drinkingwater" from the SystemType dropdown
    When I select near miss category "stormwater" from the Category dropdown
    And I select "Yes" from the CompletedCorrectiveActions dropdown
    And I enter "Description 0" into the Description field
    And I press Save

Scenario: user can edit an environmental near miss
    When I visit the Edit page for near miss: "one"
    And I select operating center "two" from the OperatingCenter dropdown
    And I select town "Sometown" from the Town dropdown
    And I select facility "one" from the Facility dropdown
    And I enter "Location" into the LocationDetails field
    And I select near miss type "environmental" from the Type dropdown
    And I select stop work usage type "one" from the StopWorkUsageType dropdown
    And I select near miss category "stormwater" from the Category dropdown
    And I select action taken type "one" from the ActionTakenType dropdown
    And I select system type "drinkingwater" from the SystemType dropdown
    And I select public water supply "one"'s NearMissDescription from the PublicWaterSupply dropdown
    And I enter "Some Action Taken" into the ActionTaken field
    And I select "Yes" from the CompletedCorrectiveActions dropdown
    And I check the ReportedToRegulator field
    And I press Save
    Then I should be at the Show page for near miss: "one"
    And I should see a display for OperatingCenter with operating center "two"'s Description

Scenario: user must select state and operating center on near miss page when editing
    When I visit the Edit page for near miss: "three"
    And I press Save
    Then I should see the validation message The State field is required.
    When I select state "NJ" from the State dropdown
    And I press Save
    Then I should see the validation message The OperatingCenter field is required.
    When I select operating center "two" from the OperatingCenter dropdown
    And I select near miss type "environmental" from the Type dropdown
    And I select town "Sometown" from the Town dropdown
    And I select near miss category "stormwater" from the Category dropdown
    And I select near miss sub category "ec" from the SubCategory dropdown
    And I select action taken type "one" from the ActionTakenType dropdown
    And I select system type "drinkingwater" from the SystemType dropdown
    And I select public water supply "one"'s NearMissDescription from the PublicWaterSupply dropdown
    And I enter "Some Action Taken" into the ActionTaken field
    And I select "Yes" from the CompletedCorrectiveActions dropdown
    And I press Save
    Then I should be at the Show page for near miss: "three"
    And I should see a display for OperatingCenter with operating center "two"'s Description
    And I should see a display for Type with near miss type "environmental"'s Description
    And I should see a display for Category with near miss category "stormwater"'s Description
    And I should see a display for SubCategory with near miss sub category "ec"'s Description
    And I should see a display for ActionTakenType with action taken type "one"'s Description
 
Scenario: user can access all the logs for a specific Near miss from log tab under NearMiss record
    Given the test flag "allow audits" exists
	And I am logged in as "admin"
    When I visit the HealthAndSafety/NearMiss/Search page
    And I press Search
    Then I should see a link to the Show page for near miss: "one"
    When I follow the Show link for near miss "one"
    Then I should be at the Show page for near miss: "one"
	When I click the "Log" tab
	Then I should see "Records found" 

Scenario: user can access all the Documents for a specific Near miss from log tab under NearMiss record
	Given I am logged in as "admin"
    When I visit the HealthAndSafety/NearMiss/Search page
    And I press Search
    Then I should see a link to the Show page for near miss: "one"
    When I follow the Show link for near miss "one"
    Then I should be at the Show page for near miss: "one"
	When I click the "Documents" tab
	Then I should see the "Documents" tab 

Scenario: user can access all the Notes for a specific Near miss from log tab under NearMiss record
	Given I am logged in as "admin"
    When I visit the HealthAndSafety/NearMiss/Search page
    And I press Search
    Then I should see a link to the Show page for near miss: "one"
    When I follow the Show link for near miss "one"
    Then I should be at the Show page for near miss: "one"
	When I click the "Notes" tab
	Then I should see the "Notes" tab

Scenario: user can search nearmiss action item
    Given I am logged in as "admin"
    And a near miss "four" exists with operating center: "one", incident number: "one" , type: "safety"
    And a action item type "type1" exists
    And a data type "data type near miss" exists with table name: "NearMisses"
    And a action item "one" exists with note: "blah", near miss: "four", data type: "data type near miss"
	When I visit the HealthAndSafety/NearMissActionItem/Search page
    And I select near miss type "safety" from the NearMissType dropdown
    And I press "Search"
    Then I should be at the HealthAndSafety/NearMissActionItem page
	And I should see the link "4" ends with "HealthAndSafety/NearMiss/Show/4#ActionItemsTab"

Scenario: user can access add a new NearMiss record from NearMiss search page
	Given I am logged in as "admin"
    When I visit the HealthAndSafety/NearMiss/Search page
    Then I should see a link to "https://mapcallintranetqa.awapps.com"
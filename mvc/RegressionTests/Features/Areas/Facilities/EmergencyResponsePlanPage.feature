Feature: EmergencyResponsePlanPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a state "one" exists with abbreviation: "NJ"
    And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "one"
	And a facility "one" exists with operating center: "nj7"
	And an emergency plan category "one" exists with description: "epc one"
	And a review frequency "one" exists with description: "rf one"
	And a emergency response plan "one" exists
    And a emergency response plan "two" exists
	And an employee "nj7-1" exists with operating center: "nj7"
	And an employee "nj7-2" exists with operating center: "nj7", first name: "Johnny", last name: "Hotdog", employee id: "2000002"
	And I am logged in as "admin"

Scenario: user can search for a emergency response plan
    When I visit the Facilities/EmergencyResponsePlan/Search page
    And I press Search
    Then I should see a link to the Show page for emergency response plan: "one"
    When I follow the Show link for emergency response plan "one"
    Then I should be at the Show page for emergency response plan: "one"

Scenario: user can access all the Documents for a specific Emergency Response Plan from Documents tab under EmergencyResponsePlan record
	Given I am logged in as "admin"
	When I visit the Facilities/EmergencyResponsePlan/Search page
	And I press Search
	Then I should see a link to the Show page for emergency response plan: "one"
	When I follow the Show link for emergency response plan "one"
	Then I should be at the Show page for emergency response plan: "one"
	When I click the "Documents" tab
	Then I should see the "Documents" tab 

Scenario: user can access all the Notes for a specific Emergency Response Plan from Notes tab under EmergencyResponsePlan record
	Given I am logged in as "admin"
    When I visit the Facilities/EmergencyResponsePlan/Search page
    And I press Search
    Then I should see a link to the Show page for emergency response plan: "one"
    When I follow the Show link for emergency response plan "one"
    Then I should be at the Show page for emergency response plan: "one"
	When I click the "Notes" tab
	Then I should see the "Notes" tab
	
Scenario: user can view a emergency response plan
    When I visit the Show page for emergency response plan: "one"
    Then I should see a display for emergency response plan: "one"'s Description

Scenario: user can create a plan review
    When I visit the Show page for emergency response plan: "one"
    Then I should see a display for emergency response plan: "one"'s Description
	When I click the "Review" tab
	And I follow "Add Review"
	And I wait for the page to reload
	And I enter "4/24/2020" into the ReviewDate field
	And I enter "tdog" and select "Hotdog, Johnny - 2000002 - NJ7" from the ReviewedBy autocomplete field
	And I press "Save"
	Then I should see the validation message "The ReviewChangeNotes field is required."
	And I should see the validation message "The NextReviewDate field is required."
	When I enter "The plan was reviewed" into the ReviewChangeNotes field
	And I enter "4/24/2024" into the NextReviewDate field
	And I press "Save"
    Then I should see a link to the Show page for emergency response plan: "one"
	And I should see a display for ReviewChangeNotes with "The plan was reviewed"
	And I should see a display for NextReviewDate with "4/24/2024"
	And I should see a display for ReviewDate with "4/24/2020"
	And I should see a display for ReviewedBy with "Johnny Hotdog"
	And I should see a display for ReviewChangeNotes with "The plan was reviewed"
	And I should see a display for CreatedAt with today's date

Scenario: user can add a emergency response plan
    When I visit the Facilities/EmergencyResponsePlan/New page
    And I press Save
	Then I should see the validation message "The State field is required."
	And I should see the validation message "The Operating Center field is required."
	And I should see the validation message "The Plan Category field is required."
	#And I should see the validation message "The EmergencyPlanSubCategory field is required."
	And I should see the validation message "The Title field is required."
	And I should see the validation message "The Description field is required."
	#And I should see the validation message "The TabletopFrequency field is required."
	#And I should see the validation message "The DrillFrequency field is required."
    When I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select emergency plan category "one"'s Description from the EmergencyPlanCategory dropdown
	And I enter "bar" into the Title field
	And I enter "foo" into the Description field
	And I select review frequency "one"'s Description from the ReviewFrequency dropdown
	And I press Save
    Then the currently shown emergency response plan will now be referred to as "balthazar"
    And I should see a display for Description with "foo"
	And I should see a display for ReviewFrequency with "rf one"
	And I should see "Notes (0)"
	And I should see "Documents (0)"

Scenario: user can edit a emergency response plan
    When I visit the Edit page for emergency response plan: "one"
	And I select "-- Select --" from the State dropdown
	And I select "-- Select --" from the OperatingCenter dropdown
	And I select "-- Select --" from the EmergencyPlanCategory dropdown
	And I enter "" into the Title field
	And I enter "" into the Description field
	And I select "-- Select --" from the ReviewFrequency dropdown
    And I press Save
	Then I should see the validation message "The State field is required."
	And I should see the validation message "The Operating Center field is required."
	And I should see the validation message "The Plan Category field is required."
	#And I should see the validation message "The EmergencyPlanSubCategory field is required."
	And I should see the validation message "The Title field is required."
	And I should see the validation message "The Description field is required."
	#And I should see the validation message "The TabletopFrequency field is required."
	#And I should see the validation message "The DrillFrequency field is required."
    When I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I select emergency plan category "one"'s Description from the EmergencyPlanCategory dropdown
	And I enter "bar" into the Title field
	And I enter "foo" into the Description field
	And I select review frequency "one"'s Description from the ReviewFrequency dropdown
	And I press Save
    Then I should see a display for Title with "bar"
	And I should see a display for Description with "foo"	
	And I should see a display for ReviewFrequency with "rf one"
	And I should see "Notes (0)"
	And I should see "Documents (0)"

Feature: ApcInspectionItemPage

Background: data exists
    Given a state "one" exists with name: "New Jersey", abbreviation: "NJ"	
    And an operating center "one" exists with opcode: "NJ7", name: "Shrewsbury", state: "one"
    And a town "lazytown" exists
	And operating center: "one" exists in town: "lazytown"
    And a facility "one" exists with operating center: "one", town: "lazytown"
    And an employee status "active" exists with description: "Active"
   	And an employee "one" exists with operating center: "one", status: "active", employee id: "32345678", first name: "johnny", last name: "hotdog"
 	And a role "fswm-admin-nj7" exists with action: "UserAdministrator", module: "FieldServicesWorkManagement", operating center: "one"
    And a apc inspection item type "one" exists
    And an admin user "admin" exists with username: "admin", employee: "one", roles: fswm-admin-nj7
	And a apc inspection item "one" exists with type: "one", operating center: "one", facility: "one", assigned to: "one"
    And a facility inspection area type "one" exists with description: "lab"
    And a facility inspection area type "two" exists with description: "grounds"
    And a facility inspection form question category "security" exists with description: "SECURITY"
    And a facility inspection form question category "fire safety" exists with description: "FIRE SAFETY"
	And a facility inspection form question  "one" exists with question: "How do you play that?", category: "security", weightage: 3, display order: 5
    And a facility inspection form question  "two" exists with question: "How do you analyze this?", category: "fire safety", weightage: 3, display order: 5
	And I am logged in as "admin"
   
   Scenario: user can search for a apc inspection item
    When I visit the HealthAndSafety/ApcInspectionItem/Search page
    And I select state "one" from the State dropdown
	And I select operating center "one" from the OperatingCenter dropdown
    And I press Search
    Then I should see a link to the Show page for apc inspection item: "one"
    When I follow the Show link for apc inspection item "one"
    Then I should be at the Show page for apc inspection item: "one"

Scenario: user can view a apc inspection item
    When I visit the Show page for apc inspection item: "one"
    Then I should see a display for apc inspection item: "one"'s Description

Scenario: user can add a apc inspection item
    When I visit the HealthAndSafety/ApcInspectionItem/New page
    And I press Save
    Then I should see the validation message "The OperatingCenter field is required."
    When I select operating center "one" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Facility field is required."
    And I should see the validation message "The Description field is required."
    And I should see the validation message "The Type field is required."
    And I should see the validation message "The DateReported field is required."
    And I should see the validation message "The AssignedTo field is required."
    When I select facility "one"'s Description from the Facility dropdown
    And I enter "blahblahblahblahblahblahblahblahblahblahblahblahblahblah" into the Description field
    And I select apc inspection item type "one" from the Type dropdown
    And I enter today's date into the DateReported field
    And I select employee "one"'s Description from the AssignedTo dropdown
    And I press Save
    Then the currently shown apc inspection item will now be referred to as "new"
    And I should see a display for Description with "blahblahblahblahblahblahblahblahblahblahblahblahblahblah"

Scenario: user can add a apc inspection item with questionniare
    When I visit the HealthAndSafety/ApcInspectionItem/New page
    And I press Save
    Then I should see the validation message "The OperatingCenter field is required."
    When I select operating center "one" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I press Save
    Then I should see the validation message "The Facility field is required."
    And I should see the validation message "The Description field is required."
    And I should see the validation message "The Type field is required."
    And I should see the validation message "The DateReported field is required."
    And I should see the validation message "The AssignedTo field is required."
    When I select facility "one"'s Description from the Facility dropdown
    And I enter "blahblahblahblahblahblahblahblahblahblahblahblahblahblah" into the Description field
    And I select apc inspection item type "one" from the Type dropdown
    And I enter today's date into the DateReported field
    And I select employee "one"'s Description from the AssignedTo dropdown
    And I select "LAB" from the FacilityInspectionAreaTypes multiselect
    And I select "GROUNDS" from the FacilityInspectionAreaTypes multiselect
    And I select "Yes" from the CreateApcFormAnswers_0__IsSafe dropdown
    And I select "No" from the CreateApcFormAnswers_1__IsSafe dropdown
    And I press Save
    Then the currently shown apc inspection item will now be referred to as "new"
    And I should see a display for Description with "blahblahblahblahblahblahblahblahblahblahblahblahblahblah"
    And I should see a display for DisplayFacilityInspectionAreaType with "GROUNDS, LAB"
    And I should see a display for Score with "3"
    And I should see a display for Percentage with "50%"

Scenario: user can edit a apc inspection item
    When I visit the Edit page for apc inspection item: "one"
    And I check the NotApplicableGeneralWorkAreaConditionsSection field
    And I check the NotApplicableEmergencyResponseFirstAidSection field
    And I check the NotApplicableSecuritySection field
    And I check the NotApplicableFireSafetySection field
    And I check the NotApplicablePersonalProtectiveEquipmentSection field
    And I check the NotApplicableChemicalStorageHazComSection field
    And I check the NotApplicableEquipmentToolsSection field
    And I check the NotApplicableConfinedSpaceSection field
    And I check the NotApplicableVehicleMotorizedEquipmentSection field
    And I check the NotApplicableOshaTrainingSection field
    Then I should not see the GeneralWorkAreaConditions element
    And I should not see the EmergencyResponseFirstAid element
    And I should not see the Security element
    And I should not see the FireSafety element
    And I should not see the PersonalProtectiveEquipment element
    And I should not see the ChemicalStorageHazCom element
    And I should not see the EquipmentTools element
    And I should not see the ConfinedSpace element
    And I should not see the VehicleMotorizedEquipment element
    And I should not see the OshaTraining element
    When  I enter "bar" into the Description field
    And I press Save
    Then I should be at the Show page for apc inspection item: "one"
    And I should see a display for Description with "bar"

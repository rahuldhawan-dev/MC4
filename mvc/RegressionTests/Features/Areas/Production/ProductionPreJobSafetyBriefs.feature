Feature: ProductionPreJobSafetyBriefs

Background: 
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
	And production work order priorities exist
	And a state "one" exists with abbreviation: "NJ"
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with operating center: "nj7", status: "active", employee id: "12345678", first name: "this", last name: "person"
	And an employee "two" exists with operating center: "nj7", status: "active", employee id: "22345678", first name: "someother", last name: "person"
	And a role "productionworkorder-all-nj7" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "nj7"
	And a role "healthandsafety-all-nj7" exists with action: "UserAdministrator", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "facilityrole" exists with action: "Read", module: "ProductionFacilities"
	And a role "equipmentrole" exists with action: "Read", module: "ProductionEquipment"
	And a user "user" exists with username: "user", roles: productionworkorder-all-nj7;healthandsafety-all-nj7;facilityrole;equipmentrole
	And a user "employee-one" exists with username: "employee-one", employee: "one", roles: healthandsafety-all-nj7
	And a user "employee-two" exists with username: "employee-two", employee: "two", roles: healthandsafety-all-nj7
	And a production work order "one" exists with operating center: "nj7"
	And an employee assignment "one" exists with production work order: "one", assigned to: "one", assigned for: "today"
	And an employee assignment "two" exists with production work order: "one", assigned to: "two", assigned for: "today"
	And a facility "one" exists with operating center: "nj7"

Scenario: User can create a safety brief for a production work order 
	Given I am logged in as "user"
	And I am at the Show page for production work order "one"
	When I click the "Pre-Job Safety Briefs" tab
	And I follow "Create Pre-Job Safety Brief"
	And I enter "1/11/2021" into the SafetyBriefDateTime field
	And I check employee "one"'s Description in the Employees checkbox list
	And I select "No" from the AnyPotentialWeatherHazards dropdown
	And I select "No" from the HadConversationAboutWeatherHazard dropdown
	And I select "No" from the AnyTimeOfDayConstraints dropdown
	And I select "No" from the AnyTrafficHazards dropdown
	And I select "No" from the InvolveConfinedSpace dropdown
	And I select "No" from the AnyPotentialOverheadHazards dropdown
	And I select "No" from the AnyUndergroundHazards dropdown
	And I select "No" from the AreThereElectricalOrOtherEnergyHazards dropdown
	And I select "No" from the AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel dropdown
	And I select "No" from the DoesJobInvolveUseOfChemicals dropdown
	And I select "Yes" from the HaveEquipmentToDoJobSafely dropdown
	And I select "Yes" from the HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection dropdown
	And I select "Yes" from the ReviewedErgonomicHazards dropdown
	And I select "Yes" from the HasStretchAndFlexBeenPerformed dropdown
	And I select "Yes" from the ReviewedLocationOfSafetyEquipment dropdown
	And I select "No" from the OtherHazardsIdentified dropdown
	And I select "Yes" from the CrewMembersRemindedOfStopWorkAuthority dropdown
	And I select "Yes" from the HaveAllHazardsAndPrecautionsBeenReviewed dropdown
	And I check the HandProtection field
	And I press "Save"
	Then I should see a display for CreatedBy with user "user"
	
Scenario: User can create a safety brief without a production work order
	Given I am logged in as "user"
	And I am at the Production/ProductionPreJobSafetyBrief/New page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I wait for ajax to finish loading
	And I select facility "one" from the Facility dropdown
	And I wait for ajax to finish loading
	And I enter "Neato" into the DescriptionOfWork field
	And I enter "1/11/2021" into the SafetyBriefDateTime field
	And I check employee "one"'s Description in the Employees checkbox list
	And I select "No" from the AnyPotentialWeatherHazards dropdown
	And I select "No" from the HadConversationAboutWeatherHazard dropdown
	And I select "No" from the AnyTimeOfDayConstraints dropdown
	And I select "No" from the AnyTrafficHazards dropdown
	And I select "No" from the InvolveConfinedSpace dropdown
	And I select "No" from the AnyPotentialOverheadHazards dropdown
	And I select "No" from the AnyUndergroundHazards dropdown
	And I select "No" from the AreThereElectricalOrOtherEnergyHazards dropdown
	And I select "No" from the AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel dropdown
	And I select "No" from the DoesJobInvolveUseOfChemicals dropdown
	And I select "Yes" from the HaveEquipmentToDoJobSafely dropdown
	And I select "Yes" from the HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection dropdown
	And I select "Yes" from the ReviewedErgonomicHazards dropdown
	And I select "Yes" from the HasStretchAndFlexBeenPerformed dropdown
	And I select "Yes" from the ReviewedLocationOfSafetyEquipment dropdown
	And I select "No" from the OtherHazardsIdentified dropdown
	And I select "Yes" from the CrewMembersRemindedOfStopWorkAuthority dropdown
	And I select "Yes" from the HaveAllHazardsAndPrecautionsBeenReviewed dropdown
	And I check the HandProtection field
	And I press "Save"
	Then I should see a display for CreatedBy with user "user"
	
Scenario: User can edit a safety brief which has no production work order
	Given a production pre job safety brief "one" exists with operating center: "nj7", facility: "one", production work order: null, description of work: "Neato"
	And I am logged in as "user"
	When I visit the Edit page for production pre job safety brief "one"
	And I enter "8/30/2022" into the SafetyBriefDateTime field
	And I select "Yes" from the HaveAllHazardsAndPrecautionsBeenReviewed dropdown
	And I check the HandProtection field
	And I check employee "one"'s Description in the Employees checkbox list
	And I press Save
	Then I should be at the Show page for production pre job safety brief "one"
	And I should see a display for SafetyBriefDateTime with "8/30/2022 12:00:00 AM (EST)"
	
Scenario: User should see different text fields visibility toggle based on selected answers
	Given I am logged in as "user"
	And I am at the Show page for production work order "one"
	When I click the "Pre-Job Safety Briefs" tab
	And I follow "Create Pre-Job Safety Brief"
	# All four weather fields work together
	When I select "No" from the AnyPotentialWeatherHazards dropdown
	Then I should not see the SafetyBriefWeatherHazardTypes field
	When I select "Yes" from the AnyPotentialWeatherHazards dropdown
	Then I should see the SafetyBriefWeatherHazardTypes field
	When I select "No" from the HadConversationAboutWeatherHazard dropdown
	Then I should not see the HadConversationAboutWeatherHazardNotes field
	When I select "Yes" from the HadConversationAboutWeatherHazard dropdown
	Then I should see the HadConversationAboutWeatherHazardNotes field

	When I select "No" from the AnyTimeOfDayConstraints dropdown
	Then I should not see the SafetyBriefTimeOfDayConstraintTypes field
	When I select "Yes" from the AnyTimeOfDayConstraints dropdown
	Then I should see the SafetyBriefTimeOfDayConstraintTypes field

	When I select "No" from the AnyTrafficHazards dropdown
	Then I should not see the SafetyBriefTrafficHazardTypes field
	When I select "Yes" from the AnyTrafficHazards dropdown
	Then I should see the SafetyBriefTrafficHazardTypes field

	When I select "No" from the AnyPotentialOverheadHazards dropdown
	Then I should not see the SafetyBriefOverheadHazardTypes field
	When I select "Yes" from the AnyPotentialOverheadHazards dropdown
	Then I should see the SafetyBriefOverheadHazardTypes field

	When I select "No" from the AnyUndergroundHazards dropdown
	Then I should not see the SafetyBriefUndergroundHazardTypes field
	When I select "Yes" from the AnyUndergroundHazards dropdown
	Then I should see the SafetyBriefUndergroundHazardTypes field

	When I select "No" from the AreThereElectricalOrOtherEnergyHazards dropdown
	Then I should not see the SafetyBriefElectricalHazardTypes field
	When I select "Yes" from the AreThereElectricalOrOtherEnergyHazards dropdown
	Then I should see the SafetyBriefElectricalHazardTypes field

	When I select "No" from the AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel dropdown
	Then I should not see the TypeOfFallPreventionProtectionSystemBeingUsed field
	When I select "Yes" from the AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel dropdown
	Then I should see the TypeOfFallPreventionProtectionSystemBeingUsed field

	# All three chemical fields work together
	When I select "No" from the DoesJobInvolveUseOfChemicals dropdown
	Then I should not see the IsSafetyDataSheetAvailableForEachChemical field
	When I select "Yes" from the DoesJobInvolveUseOfChemicals dropdown
	Then I should see the IsSafetyDataSheetAvailableForEachChemical field
	When I select "No" from the IsSafetyDataSheetAvailableForEachChemical dropdown
	Then I should see the IsSafetyDataSheetAvailableForEachChemicalNotes field
	When I select "Yes" from the IsSafetyDataSheetAvailableForEachChemical dropdown
	Then I should not see the IsSafetyDataSheetAvailableForEachChemicalNotes field
	
	When I select "Yes" from the HaveEquipmentToDoJobSafely dropdown
	Then I should not see the HaveEquipmentToDoJobSafelyNotes field
	When I select "No" from the HaveEquipmentToDoJobSafely dropdown
	Then I should see the HaveEquipmentToDoJobSafelyNotes field

	When I select "Yes" from the HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection dropdown
	Then I should not see the HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes field
	When I select "No" from the HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection dropdown
	Then I should see the HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspectionNotes field

	When I select "Yes" from the ReviewedErgonomicHazards dropdown
	Then I should not see the ReviewedErgonomicHazardsNotes field
	When I select "No" from the ReviewedErgonomicHazards dropdown
	Then I should see the ReviewedErgonomicHazardsNotes field

	When I select "Yes" from the HasStretchAndFlexBeenPerformed dropdown
	Then I should not see the HasStretchAndFlexBeenPerformedNotes field
	When I select "No" from the HasStretchAndFlexBeenPerformed dropdown
	Then I should see the HasStretchAndFlexBeenPerformedNotes field

	When I select "Yes" from the ReviewedLocationOfSafetyEquipment dropdown
	Then I should not see the ReviewedLocationOfSafetyEquipmentNotes field
	When I select "No" from the ReviewedLocationOfSafetyEquipment dropdown
	Then I should see the ReviewedLocationOfSafetyEquipmentNotes field

	When I select "No" from the OtherHazardsIdentified dropdown
	Then I should not see the OtherHazardNotes field
	When I select "Yes" from the OtherHazardsIdentified dropdown
	Then I should see the OtherHazardNotes field

	When I select "Yes" from the CrewMembersRemindedOfStopWorkAuthority dropdown
	Then I should not see the CrewMembersRemindedOfStopWorkAuthorityNotes field
	When I select "No" from the CrewMembersRemindedOfStopWorkAuthority dropdown
	Then I should see the CrewMembersRemindedOfStopWorkAuthorityNotes field

Scenario: User must check at least one PPE before saving
	Given I am logged in as "user"
	And I am at the Show page for production work order "one"
	When I click the "Pre-Job Safety Briefs" tab
	And I follow "Create Pre-Job Safety Brief"
	When I press "Save"
	# HeadProtection is just the field the client side validation is hooked up to, but it will appear
	# on that field for any of the checkboxes. This could use a nicer UX but this is the easiest way to
	# wire in this validation with our existing validation system.
	Then I should see a validation message for HeadProtection with "At least one PPE type must be checked"
	When I check the HandProtection field
	And I press "Save"
	Then I should not see a validation message for HeadProtection with "At least one PPE type must be checked"

Scenario: User must answer Yes to the HaveAllHazardsAndPrecautionsBeenReviewed question for validation to pass
	Given I am logged in as "user"
	And I am at the Show page for production work order "one"
	When I click the "Pre-Job Safety Briefs" tab
	And I follow "Create Pre-Job Safety Brief"
	When I press "Save"
	Then I should see a validation message for HaveAllHazardsAndPrecautionsBeenReviewed with "Please speak to your supervisor if hazards and precautions have not been discussed before starting work."
	When I select "No" from the HaveAllHazardsAndPrecautionsBeenReviewed dropdown
	And I press "Save"
	Then I should see a validation message for HaveAllHazardsAndPrecautionsBeenReviewed with "Please speak to your supervisor if hazards and precautions have not been discussed before starting work."
	When I select "Yes" from the HaveAllHazardsAndPrecautionsBeenReviewed dropdown
	And I press "Save"
	Then I should not see a validation message for HaveAllHazardsAndPrecautionsBeenReviewed with "Please speak to your supervisor if hazards and precautions have not been discussed before starting work."

Scenario: User should only see specific employees in the employee select list
	# This is a mouthful so it's not going in the title
	# They need to see: 
	# 1. The employees assigned to the work order (This is satisfied by the employee assignments created in the Background section)

	Given a production pre job safety brief "one" exists with production work order: "one"
	
	# 2. Active employees with role for the operating center of the work order
	And an employee "same-op-but-not-assigned" exists with operating center: "nj7", status: "active", first name: "someother", last name: "person"
	And a role "productionworkorder-all-nj7-not-assigned" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "nj7"
	And a user "not-assigned-employee-user" exists with username: "not-assigned-employee-user", roles: productionworkorder-all-nj7-not-assigned, employee: "same-op-but-not-assigned"
		
	# 3. Previously selected employees, who weren't assignments, that may no longer be active
	And an employee status "inactive" exists with description: "Inactive"
	And an employee "inactive" exists with operating center: "nj7", status: "inactive", first name: "this", last name: "person"
	And a role "productionworkorder-all-nj7-inactive" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "nj7"
	And a user "inactive-employee-user" exists with username: "inactive-employee-user", roles: productionworkorder-all-nj7-inactive, employee: "inactive"
	And a production pre job safety brief worker "inactive-employee" exists with production pre job safety brief: "one", employee: "inactive"

	# 4. And they should not see this employee because they're in the wrong operating center entirely and they were never 
	#    previously assigned to the work order or added to the safety brief
	And an operating center "QQ1" exists with opcode: "QQ1", name: "Shrewsbury", sap enabled: "true", sap work orders enabled: "true"
	And an employee "should-not-be-seen" exists with operating center: "QQ1", status: "active", first name: "andanother", last name: "person"
	And a role "productionworkorder-all-nj7-should-not-be-seen" exists with action: "UserAdministrator", module: "ProductionWorkManagement", operating center: "QQ1"
	And a user "should-not-be-seen-employee-user" exists with username: "should-not-be-seen-employee-user", roles: productionworkorder-all-nj7-should-not-be-seen, employee: "should-not-be-seen"

	And I am logged in as "user"
	And I am at the Edit page for production pre job safety brief "one"
	Then I should see employee "one"'s Description in the Employees checkbox list
	And I should see employee "two"'s Description in the Employees checkbox list
	And I should see employee "inactive"'s Description in the Employees checkbox list
	And I should see employee "same-op-but-not-assigned"'s Description in the Employees checkbox list
	And I should not see employee "should-not-be-seen"'s Description in the Employees checkbox list

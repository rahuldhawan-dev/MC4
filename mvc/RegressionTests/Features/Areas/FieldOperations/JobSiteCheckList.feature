Feature: JobSiteCheckList
	
Background: users exist
	Given a user "user" exists with username: "user"
	And an operating center "one" exists with opcode: "QQ1", name: "Wawa"
	And an operating center "sov" exists with opcode: "SOV", name: "SOV"
	And a role "employeerole" exists with action: "Read", module: "HumanResourcesEmployee", user: "user", operating center: "one"
	And a role "hsreadrole" exists with action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "one"
	And a role "hsaddrole" exists with action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "one"
	And a role "hseditrole" exists with action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "one"
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "one", status: "active"
	And an employee "supervisor" exists with operating center: "one", status: "active"
	And a state "one" exists with abbreviation: "QQ", name: "Q State"
	And a county "one" exists with state: "one", name: "Count Countula"
	And a town "one" exists with county: "one", name: "Townie"
	And a job site excavation protection type "one" exists with description: "Some sort of protection type"
	And a job site excavation location type "one" exists with description: "Some Location"
	And a job site excavation soil type "one" exists with description: "Some Soil"
	And a job site check list pressurized risk restrained type "yes" exists with description: "Yes"
	And a job site check list pressurized risk restrained type "no" exists with description: "No"
	And a job site check list no restraint reason type "one" exists with description: "No restraints for this guy!"
	And a job site check list safety brief weather hazard type  "one" exists with description: "weather hazard"
	And a job site check list safety brief weather hazard type  "two" exists with description: "the other weather hazard"
	And a job site check list safety brief time of day constraint type  "one" exists with description: "TOD Constraint"
	And a job site check list safety brief time of day constraint type  "two" exists with description: "the other TOD constraint"
	And a job site check list safety brief traffic hazard type  "one" exists with description: "traffic hazard"
	And a job site check list safety brief traffic hazard type  "two" exists with description: "the other traffic hazard"
	And a job site check list safety brief overhead hazard type  "one" exists with description: "overhead Hazard"
	And a job site check list safety brief overhead hazard type  "two" exists with description: "the other overhead hazard"
	And a job site check list safety brief underground hazard type  "one" exists with description: "underground hazard"
	And a job site check list safety brief underground hazard type  "two" exists with description: "the other underground hazard"
	And a job site check list safety brief electrical hazard type  "one" exists with description: "electrical hazard"
	And a job site check list safety brief electrical hazard type  "two" exists with description: "the other electrical hazard"
	And a work order "one" exists with operating center: "one"

#Background: users exist
	#Given the following objects exist
	#| type                                | name         | values                                                                                                         |
	#| user                                | user         | username: "user"                                                                                               |
	#| operating center                    | one          | opcode: "QQ1", name: "Wawa"                                                                                    |
	#| operating center                    | sov          | opcode: "SOV", name: "SOV"                                                                                     |
	#| role                                | employeerole | action: "Read", module: "HumanResourcesEmployee", user: "user", operating center: "one"                        |
	#| role                                | hsreadrole   | action: "Read", module: "FieldServicesWorkManagement", user: "user", operating center: "one"                   |
	#| role                                | hsaddrole    | action: "Add", module: "FieldServicesWorkManagement", user: "user", operating center: "one"                    |
	#| role                                | hseditrole   | action: "Edit", module: "FieldServicesWorkManagement", user: "user", operating center: "one"                   |
	#| employee status                     | active       | description: "Active"                                                                                          |
	#| employee                            | one          | first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "one", status: "active" |
	#| employee                            | supervisor   | operating center: "one", status: "active"                                                                      |
	#| state                               | one          | abbreviation: "QQ", name: "Q State"                                                                            |
	#| county                              | one          | state: "one", name: "Count Countula"                                                                           |
	#| town                                | one          | county: "one", name: "Townie"                                                                                  |
	#| job site excavation protection type | one          | description: "Some sort of protection type"                                                                    |
	#| job site excavation location type   | one          | description: "Some Location"                                                                                   |
	#| job site excavation soil type       | one          | description: "Some Soil"                                                                                       |


Scenario: User can download pdf
	Given an job site check list "one" exists with operating center: "one"
	And I am logged in as "user"
	When I visit the show page for job site check list: "one"
	Then I should be able to download job site check list "one"'s pdf

Scenario: User without FieldServicesWorkManagement.Add role can not add new checklists
	Given a user "boourns" exists with username: "boourns"
	And I am logged in as "boourns"
	When I visit the HealthAndSafety/JobSiteCheckList/New page 
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "FieldServicesWorkManagement"

Scenario: User should only see operating centers allowed by role in dropdown
	Given an operating center "hellznaw" exists with opcode: "NO", name: "Wrong!"
	And I am logged in as "user"
	When I visit the HealthAndSafety/JobSiteCheckList/Search page
	Then I should see operating center "one" in the OperatingCenter dropdown
	And I should not see operating center "hellznaw" in the OperatingCenter dropdown

Scenario: Supervisor sign off date should automatically be populated with today's date when a supervisor sign off employee is selected
	Given a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", supervisor sign off employee: "supervisor"
	And a job site excavation "one" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "4/24/1984 4:04 AM", width in feet: "1.23", length in feet: "4.56", depth in inches: "12.89", job site excavation location type: "one", job site excavation soil type: "one" 
	And a job site excavation "two" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "5/25/1985 5:05 AM", width in feet: "1", length in feet: "2", depth in inches: "13", job site excavation location type: "one", job site excavation soil type: "one" 
	And a job site excavation location type "two" exists with description: "Some Other Location"
	And a job site excavation soil type "two" exists with description: "Some Other Soil"
	And I am logged in as "user"
	And I am at the edit page for job site check list that is not signed off by a supervisor: "one"
	# Both of these need to be blank to get the javascript to trigger.
	When I select "-- Select --" from the SupervisorSignOffEmployee dropdown
	And I enter "" into the SupervisorSignOffDate field
	And I select employee "one"'s Description from the SupervisorSignOffEmployee dropdown
	Then I should see "today's date" in the SupervisorSignOffDate field

Scenario: Editing a record should display some additional validation
	Given a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", safety brief date time: "08/11/2020", have equipment to do job safely: true, reviewed ergonomic hazards: true
	And a job site excavation "one" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "4/24/1984 4:04 AM", width in feet: "1.23", length in feet: "4.56", depth in inches: "12.89", job site excavation location type: "one", job site excavation soil type: "one" 
	And a job site excavation "two" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "5/25/1985 5:05 AM", width in feet: "1", length in feet: "2", depth in inches: "13", job site excavation location type: "one", job site excavation soil type: "one" 
	And a job site excavation location type "two" exists with description: "Some Other Location"
	And a job site excavation soil type "two" exists with description: "Some Other Soil"
	And I am logged in as "user"
	And I am at the edit page for job site check list that is not signed off by a supervisor: "one"
	When I select "-- Select --" from the SupervisorSignOffEmployee dropdown
	And I enter "" into the SupervisorSignOffDate field
	# This needs to be blanked out so the form doesn't postback, as the Address field's required.
	And I enter "" into the Address field
	And I press Save
	Then I should not see a validation message for SupervisorSignOffEmployee with "The SupervisorSignOffEmployee field is required."
	And I should not see a validation message for SupervisorSignOffDate with "The SupervisorSignOffDate field is required."
	When I enter "1/24/2014" into the SupervisorSignOffDate field
	And I press Save
	Then I should see a validation message for SupervisorSignOffEmployee with "The SupervisorSignOffEmployee field is required."
	When I select employee "one"'s Description from the SupervisorSignOffEmployee dropdown
	And I enter "" into the SupervisorSignOffDate field
	And I press Save
	Then I should see a validation message for SupervisorSignOffDate with "The SupervisorSignOffDate field is required."
	And I should see a validation message for AllEmployeesWearingAppropriatePersonalProtectionEquipment with "Required"
	And I should see a validation message for HasExcavation with "Required"
    When I select "Yes" from the HasExcavation dropdown
	And I press Save 
	Then I should see a validation message for AllStructuresSupportedOrProtected with "Required"
	And I should see a validation message for IsMarkoutValidForSite with "Required"
	And I should see a validation message for AllMaterialsSetBackFromEdgeOfTrenches with "Required"
	And I should see a validation message for WaterControlSystemsInUse with "Required"
	And I should see a validation message for AreExposedUtilitiesProtected with "Required"
	When I enter "N/A" into the MapCallWorkOrder field
	And I press Save
	Then I should see a validation message for MapCallWorkOrder with "The field MapCall Work Order must be a number."
	When I click the "Work Zone Set Up" tab
	And I select "Yes" from the HasTrafficControl dropdown
	And I press Save
	Then I should see a validation message for HasTrafficControl with "At least one traffic control type must be selected."
	When I click the "Work Zone Set Up" tab
	And I check the HasFlagPersonForTrafficControl field
	And I press Save
	Then I should not see a validation message for HasTrafficControl with "At least one traffic control type must be selected."
	When I click the "Excavations" tab
	When I check the HasExcavationOverFourFeetDeep field
	And I press Save
	Then I should see a validation message for IsALadderInPlace with "Required"
	And I should see a validation message for LadderExtendsAboveGrade with "Required"
	And I should see a validation message for IsLadderOnSlope with "Required"
    And I should see a validation message for HasAtmosphereBeenTested with "Required"
	When I click the "Excavations" tab
	When I check the HasExcavationFiveFeetOrDeeper field
	And I press Save
	Then I should see a validation message for IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical with "Required"
	And I should see a validation message for IsShoringSystemUsed with "Required"
	And I should see a validation message for ShoringSystemSidesExtendAboveBaseOfSlope with "Required"
    And I should see a validation message for ShoringSystemInstalledTwoFeetFromBottomOfTrench with "Required"
	And I should see a validation message for SpotterAssigned with "Required"
	And I should see a validation message for IsManufacturerDataOnSiteForShoringOrShieldingEquipment with "Required"
	And I should see a validation message for IsTheExcavationGuardedFromAccidentalEntry with "Required"
	And I should see a validation message for AreThereAnyVisualSignsOfPotentialSoilCollapse with "Required"
	And I should see a validation message for IsTheExcavationSubjectToVibration with "Required"
	And I should see a validation message for ProtectionTypesClientSideValidationHack with "At least one protection type must be selected."
	When I click the "Excavations" tab
	When I select "Yes" from the HasAtmosphereBeenTested dropdown
	And I press Save
	Then I should see a validation message for AtmosphericOxygenLevel with "Required"
	And I should see a validation message for AtmosphericCarbonMonoxideLevel with "Required"
	And I should see a validation message for AtmosphericLowerExplosiveLimit with "Required"
	When I click the "Work Zone Set Up" tab
	When I select "Yes" from the HasTrafficControl dropdown
	And I press Save
	Then I should see a validation message for CompliesWithStandards with "Required"
	When I click the "Excavations" tab
	When I check the HasExcavationFiveFeetOrDeeper field
	And I uncheck the HasExcavationOverFourFeetDeep field
	And I press Save
	Then I should see a validation message for HasExcavationOverFourFeetDeep with "This must be checked when there are excavations five feet or deeper."
	When I click the "Excavations" tab
	When I check the HasExcavationOverFourFeetDeep field
	And I press Save
	Then I should not see a validation message for HasExcavationOverFourFeetDeep with "This must be checked when there are excavations five feet or deeper."
	When I click the "Excavations" tab
	When I press "Add Excavation"
	And I click the "Excavations" tab
	And I press "Add Excavation"
	And I press create-excavation
	Then I should see "The ExcavationDate field is required." in the excavation-details-table element
	And I should see "The WidthInFeet field is required." in the excavation-details-table element
	And I should see "The LengthInFeet field is required." in the excavation-details-table element
	And I should see "The DepthInInches field is required." in the excavation-details-table element
	And I should see "The LocationType field is required." in the excavation-details-table element
	And I should see "The SoilType field is required." in the excavation-details-table element

Scenario: Creating a new record should display a whole lotta validation
	Given I am logged in as "user"
	And I am at the HealthAndSafety/JobSiteCheckList/New page 
	When I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for Address with "The Address field is required."
	And I should see a validation message for Coordinate with "The Coordinate field is required."
	And I should see a validation message for CrewMembers with "The CrewMembers field is required."
	When I select operating center "one" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for CompetentEmployee with "The CompetentEmployee field is required."
	When I click the "Pre Job Safety Brief" tab
	And I select "Yes" from the AnyPotentialWeatherHazards dropdown
	And I press Save 
	Then I should see a validation message for SafetyBriefWeatherHazardTypes with "The If Yes field is required."
	When I select "Yes" from the AnyTimeOfDayConstraints dropdown
	And I press Save 
	Then I should see a validation message for SafetyBriefTimeOfDayConstraintTypes with "The If Yes field is required."
	When I select "Yes" from the AnyTrafficHazards dropdown
	And I press Save 
	Then I should see a validation message for SafetyBriefTrafficHazardTypes with "The If Yes field is required."
	When I select "Yes" from the AnyPotentialOverheadHazards dropdown
	And I press Save 
	Then I should see a validation message for SafetyBriefOverheadHazardTypes with "The If Yes field is required."
	When I select "Yes" from the AnyUndergroundHazards dropdown
	And I press Save 
	Then I should see a validation message for SafetyBriefUndergroundHazardTypes with "The If Yes field is required."
	When I select "Yes" from the AnyUndergroundHazards dropdown
	And I press Save 
	Then I should see a validation message for SafetyBriefUndergroundHazardTypes with "The If Yes field is required."
	When I select "Yes" from the AreThereElectricalHazards dropdown
	And I press Save 
	Then I should see a validation message for SafetyBriefElectricalHazardTypes with "The If Yes field is required."
	When I click the "Pre Job Safety Brief" tab
	And I check the WorkingWithACPipe field
	And I press Save
	Then I should see a validation message for CrewMembersTrainedInACPipe with "The Are the crew members trained in proper AC pipe handling and disposal? field is required."
	And I should see a validation message for HaveEquipmentToDoJobSafely with "The Do you have the equipment you need to do the job safely and are employees trained on the equipment they will be using? field is required."
	And I should see a validation message for ReviewedErgonomicHazards with "The Have you reviewed potential ergonomic hazards with the crew? field is required."
	And I should see a validation message for OtherHazardsIdentified with "The Any other hazards identified? field is required."
	And I should see "At least one PPE type must be checked"

Scenario: User should not see excavations tab if excavations are not required
	Given I am logged in as "user" 
	And a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", safety brief date time: "08/11/2020", have equipment to do job safely: true, reviewed ergonomic hazards: true
	And I am at the edit page for job site check list that is not signed off by a supervisor: "one"
	Then I should not see the "Excavations" tab
	When I select "Yes" from the HasExcavation dropdown
	Then I should see the "Excavations" tab
	When I select "No" from the HasExcavation dropdown
	Then I should not see the "Excavations" tab

Scenario: User is required to enter at least one excavation for job site checklist which has an excavation
	Given a coordinate "one" exists
	And a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", supervisor sign off employee: "supervisor", reviewed ergonomic hazards: true, safety brief date time: "08/11/2020", have equipment to do job safely: true, head protection: true, all employees wearing appropriate personal protection equipment: true, all structures supported or protected: true, reviewed location of safety equipment: true, is markout valid for site: false, has excavation: false
	And I am logged in as "user"
	And I am at the edit page for job site check list that is not signed off by a supervisor: "one"
	When I select operating center "one" from the OperatingCenter dropdown
	And I select employee "one"'s Description from the CompetentEmployee dropdown
    And I select "Yes" from the HasExcavation dropdown
	And I enter "4/24/1984" into the CheckListDate field
	And I enter "That's all I have to say about that" into the Comments field
	And I enter "Some dude's house" into the Address field
    And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "Ron Howard" into the CrewMembers field
	And I enter "1234567890" into the SAPWorkOrderId field
	And I click the "Work Zone Set Up" tab
	And I select "Yes" from the AllEmployeesWearingAppropriatePersonalProtectionEquipment dropdown
	And I select "Yes" from the HasTrafficControl dropdown
	And I check the HasFlagPersonForTrafficControl field
	And I check the HasConesForTrafficControl field
	And I check the HasSignsForTrafficControl field
	And I check the HasPoliceForTrafficControl field
	And I check the HasBarricadesForTrafficControl field
	And I select "Yes" from the CompliesWithStandards dropdown
	And I select "Yes" from the AllStructuresSupportedOrProtected dropdown
	And I select job site check list pressurized risk restrained type "no" from the PressurizedRiskRestrainedType dropdown
	And I select job site check list no restraint reason type "one" from the NoRestraintReason dropdown
	And I click the "Utility Verification" tab
	And I select "Yes" from the IsMarkoutValidForSite dropdown
	And I enter "99999" into the MarkoutNumber field
	And I select "Yes" from the IsEmergencyMarkoutRequest dropdown
	And I select "Yes" from the MarkedSanitarySewer dropdown
	And I select "No" from the MarkedTelephone dropdown
	And I select "N/A" from the MarkedFuelGas dropdown
	And I select "Yes" from the MarkedElectric dropdown
	And I select "No" from the MarkedWater dropdown
	And I select "N/A" from the MarkedOther dropdown
	And I click the "Excavations" tab
	And I select "N/A" from the SpotterAssigned dropdown
	And I select "N/A" from the IsManufacturerDataOnSiteForShoringOrShieldingEquipment dropdown
	And I select "Yes" from the IsTheExcavationGuardedFromAccidentalEntry dropdown
	And I select "No" from the AreThereAnyVisualSignsOfPotentialSoilCollapse dropdown
	And I select "No" from the IsTheExcavationSubjectToVibration dropdown
	And I select "N/A" from the AllMaterialsSetBackFromEdgeOfTrenches dropdown
	And I select "Yes" from the WaterControlSystemsInUse dropdown
	And I select "N/A" from the AreExposedUtilitiesProtected dropdown
	And I check the HasExcavationOverFourFeetDeep field
	And I select "Yes" from the IsALadderInPlace dropdown
	And I select "N/A" from the LadderExtendsAboveGrade dropdown
	And I select "Yes" from the IsLadderOnSlope dropdown
	And I select "Yes" from the HasAtmosphereBeenTested dropdown
	And I enter "1.234" into the AtmosphericOxygenLevel field
	And I enter "4.52" into the AtmosphericCarbonMonoxideLevel field
	And I enter "5" into the AtmosphericLowerExplosiveLimit field
	And I check the HasExcavationFiveFeetOrDeeper field
	And I check job site excavation protection type "one" in the ProtectionTypes checkbox list
	And I select "Yes" from the IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical dropdown
	And I select "N/A" from the IsShoringSystemUsed dropdown
	And I select "Yes" from the ShoringSystemSidesExtendAboveBaseOfSlope dropdown
	And I select "N/A" from the ShoringSystemInstalledTwoFeetFromBottomOfTrench dropdown
	And I click the "Restraint" tab
	And I click the "Pre Job Safety Brief" tab
	And I enter "08/12/2020" into the SafetyBriefDateTime field
	And I select "No" from the AnyPotentialWeatherHazards dropdown
	And I select "No" from the AnyTimeOfDayConstraints dropdown
	And I select "No" from the AnyTrafficHazards dropdown
	And I select "No" from the AnyPotentialOverheadHazards dropdown
	And I select "No" from the InvolveConfinedSpace dropdown
	And I select "No" from the AnyUndergroundHazards dropdown
	And I select "No" from the AreThereElectricalHazards dropdown
	And I select "Yes" from the HaveEquipmentToDoJobSafely dropdown
	And I select "Yes" from the ReviewedErgonomicHazards dropdown
	And I select "No" from the OtherHazardsIdentified dropdown
	And I check the ReviewedLocationOfSafetyEquipment field
	And I check the HeadProtection field
	And I press Save
	And I wait for the page to reload
	Then I should see the error message "At least one excavation must be added to a job site checklist."

Scenario: User is not required or able to enter an excavation for job site checklist which has an excavationn't
	Given a coordinate "one" exists
	And a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", supervisor sign off employee: "supervisor", reviewed ergonomic hazards: true, safety brief date time: "08/11/2020", have equipment to do job safely: true, head protection: true, all employees wearing appropriate personal protection equipment: true, all structures supported or protected: true, reviewed location of safety equipment: true, is markout valid for site: false, has excavation: false
	And I am logged in as "user"
	And I am at the edit page for job site check list that is not signed off by a supervisor: "one"
    When I select "No" from the HasExcavation dropdown
	And I enter "That's all I have to say about that" into the Comments field
    Then the "Excavations" tab should be disabled
	When I enter "4/24/1984" into the CheckListDate field
	And I enter "1234567890" into the SAPWorkOrderId field
	And I click the "Work Zone Set Up" tab
	And I select "Yes" from the AllEmployeesWearingAppropriatePersonalProtectionEquipment dropdown
	And I select "Yes" from the HasTrafficControl dropdown
	And I check the HasFlagPersonForTrafficControl field
	And I check the HasConesForTrafficControl field
	And I check the HasSignsForTrafficControl field
	And I check the HasPoliceForTrafficControl field
	And I check the HasBarricadesForTrafficControl field
	And I select "Yes" from the CompliesWithStandards dropdown
	And I select "Yes" from the AllStructuresSupportedOrProtected dropdown
	And I select job site check list pressurized risk restrained type "no" from the PressurizedRiskRestrainedType dropdown
	And I select job site check list no restraint reason type "one" from the NoRestraintReason dropdown
	And I click the "Utility Verification" tab
	And I select "Yes" from the IsMarkoutValidForSite dropdown
	And I enter "99999" into the MarkoutNumber field
	And I select "Yes" from the IsEmergencyMarkoutRequest dropdown
	And I select "Yes" from the MarkedSanitarySewer dropdown
	And I select "No" from the MarkedTelephone dropdown
	And I select "N/A" from the MarkedFuelGas dropdown
	And I select "Yes" from the MarkedElectric dropdown
	And I select "No" from the MarkedWater dropdown
	And I select "N/A" from the MarkedOther dropdown
	And I click the "Restraint" tab
	And I click the "Pre Job Safety Brief" tab
	And I select operating center "one" from the OperatingCenter dropdown
	And I select employee "one"'s Description from the CompetentEmployee dropdown
	And I enter "Some dude's house" into the Address field
    And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "Ron Howard" into the CrewMembers field
	And I enter "08/12/2020" into the SafetyBriefDateTime field
	And I select "No" from the AnyPotentialWeatherHazards dropdown
	And I select "No" from the AnyTimeOfDayConstraints dropdown
	And I select "No" from the AnyTrafficHazards dropdown
	And I select "No" from the AnyPotentialOverheadHazards dropdown
	And I select "No" from the InvolveConfinedSpace dropdown
	And I select "No" from the AnyUndergroundHazards dropdown
	And I select "No" from the AreThereElectricalHazards dropdown
	And I select "Yes" from the HaveEquipmentToDoJobSafely dropdown
	And I select "Yes" from the ReviewedErgonomicHazards dropdown
	And I select "No" from the OtherHazardsIdentified dropdown
	And I check the ReviewedLocationOfSafetyEquipment field
	And I check the HeadProtection field
	And I press Save
	And I wait for the page to reload
	Then I should not see the error message "At least one excavation must be added to a job site checklist."
    And the currently shown job site check list will now be referred to as "blah"
    And I should be at the show page for job site check list "blah"

Scenario: User can create a checklist
	Given a work order "wo" exists with date completed: "12/8/1980", operating center: "one"
	And a coordinate "one" exists 
	And I am logged in as "user"
	And I am at the HealthAndSafety/JobSiteCheckList/New page 
	When I enter "That's all I have to say about that" into the Comments field
	And I enter "1234567890" into the SAPWorkOrderId field
	And I enter work order "wo"'s Id into the MapCallWorkOrder field
	When I select operating center "one" from the OperatingCenter dropdown
	And I select employee "one"'s Description from the CompetentEmployee dropdown
	And I enter "Ron Howard" into the CrewMembers field
	And I enter "Some dude's house" into the Address field
    And I enter coordinate "one"'s Id into the Coordinate field
	And I enter "08/12/2020" into the SafetyBriefDateTime field
	And I select "Yes" from the AnyPotentialWeatherHazards dropdown
	And I select "weather hazard" from the SafetyBriefWeatherHazardTypes dropdown
	And I enter "A very dangerous situation" into the HadConversationAboutWeatherHazardNotes field
	And I select "Yes" from the AnyTrafficHazards dropdown 
	And I select "traffic hazard" from the SafetyBriefTrafficHazardTypes dropdown
	And I select "Yes" from the HadConversationAboutWeatherHazard dropdown
	And I select "No" from the AnyTimeOfDayConstraints dropdown
	And I select "No" from the InvolveConfinedSpace dropdown
	And I select "No" from the AnyPotentialOverheadHazards dropdown
	And I select "No" from the AnyUndergroundHazards dropdown
	And I select "No" from the AreThereElectricalHazards dropdown
	And I check the WorkingWithACPipe field
	And I check the HaveYouInspectedSlings field
	And I select "Yes" from the CrewMembersTrainedInACPipe dropdown
	And I select "Yes" from the HaveEquipmentToDoJobSafely dropdown
	And I select "Yes" from the ReviewedErgonomicHazards dropdown 
	And I check the ReviewedLocationOfSafetyEquipment field
	And I select "No" from the OtherHazardsIdentified dropdown
	And I check the CrewMembersRemindedOfStopWorkAuthority field
	And I check the HeadProtection field
	And I check the HandProtection field
	And I check the FootProtection field
	And I check the PPEOther field
	And I enter "All the things" into the PPEOtherNotes field
	And I press Save
	Then I should be at the FieldOperations/WorkOrderFinalization/Show page for work order: "wo"

Scenario: User can edit crew members and still see the old crew members
	Given a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", supervisor sign off employee: "supervisor", reviewed ergonomic hazards: true, safety brief date time: "08/11/2020", have equipment to do job safely: true, head protection: true, all employees wearing appropriate personal protection equipment: true, all structures supported or protected: true, reviewed location of safety equipment: true, is markout valid for site: false, has excavation: false
	And a job site excavation "one" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "4/24/1984 4:04 AM", width in feet: "1.23", length in feet: "4.56", depth in inches: "12.89", job site excavation location type: "one", job site excavation soil type: "one" 
	And a job site check list crew members "one" exists with job site check list that is not signed off by a supervisor: "one", crew members: "some dudes on a crew"
	And I am logged in as "user"
	And I am at the edit page for job site check list that is not signed off by a supervisor: "one"
	When I enter "Some other dudes" into the CrewMembers field
	And I press Save
	Then I should be at the show page for job site check list that is not signed off by a supervisor: "one"
	And I should see "some dudes on a crew"
	And I should see "Some other dudes"

Scenario: User can edit comments and still see the old comments
	Given a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", supervisor sign off employee: "supervisor", reviewed ergonomic hazards: true, safety brief date time: "08/11/2020", have equipment to do job safely: true, head protection: true, all employees wearing appropriate personal protection equipment: true, all structures supported or protected: true, reviewed location of safety equipment: true, is markout valid for site: false, has excavation: false
	And a job site excavation "one" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "4/24/1984 4:04 AM", width in feet: "1.23", length in feet: "4.56", depth in inches: "12.89", job site excavation location type: "one", job site excavation soil type: "one" 
	And a job site check list comment "one" exists with job site check list that is not signed off by a supervisor: "one", comments: "some comments about a thing"
	And I am logged in as "user"
	And I am at the edit page for job site check list that is not signed off by a supervisor: "one"
	When I enter "asdgashba vdrdsajgjnad fgggfd" into the Comments field
	And I enter "Placeholder" into the CrewMembers field
	And I press Save
	Then I should be at the show page for job site check list that is not signed off by a supervisor: "one"
	And I should see "some comments about a thing"
	And I should see "asdgashba vdrdsajgjnad fgggfd"

Scenario: User can edit excavations when editing a checklist
	Given a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", has excavation: true, reviewed ergonomic hazards: true, safety brief date time: "08/11/2020", have equipment to do job safely: true, head protection: true, all employees wearing appropriate personal protection equipment: true, all structures supported or protected: true, reviewed location of safety equipment: true, is markout valid for site: false, all materials set back from edge of trenches: true, water control systems in use: true, are exposed utilities protected: true
	And a job site excavation "one" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "4/24/1984 4:04 AM", width in feet: "1.23", length in feet: "4.56", depth in inches: "12.89", job site excavation location type: "one", job site excavation soil type: "one" 
	And a job site excavation "two" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "5/25/1985 5:05 AM", width in feet: "1", length in feet: "2", depth in inches: "13", job site excavation location type: "one", job site excavation soil type: "one" 
	And a job site excavation location type "two" exists with description: "Some Other Location"
	And a job site excavation soil type "two" exists with description: "Some Other Soil"
	And I am logged in as "user"
	When I visit the show page for job site check list that is not signed off by a supervisor: "one"
	And I click the "Excavations" tab
	Then I should see the following values in the excavation-details-table table
	| Date              | Width(ft) | Length(ft) | Depth(in) | Location      | Soil Type | Created By |
	| 4/24/1984 4:04 AM | 1.23      | 4.56       | 12.89     | Some Location | Some Soil | factory    |
	| 5/25/1985 5:05 AM | 1         | 2          | 13        | Some Location | Some Soil | factory    |
	When I visit the edit page for job site check list that is not signed off by a supervisor: "one"
	And I enter "test" into the CrewMembers field
	And I click the "Excavations" tab
	And I click the "Edit" button in the 1st row of excavation-details-table 
	And I enter "6/26/1986 6:06 AM" into the editorModel_ExcavationDate field
	And I enter "7" into the editorModel_WidthInFeet field
	And I enter "8" into the editorModel_LengthInFeet field
	And I enter "4" into the editorModel_DepthInInches field
	And I select job site excavation location type "two" from the editorModel_LocationType dropdown
	And I select job site excavation soil type "two" from the editorModel_SoilType dropdown
	And I select "N/A" from the SpotterAssigned dropdown
	And I select "N/A" from the IsManufacturerDataOnSiteForShoringOrShieldingEquipment dropdown
	And I select "Yes" from the IsTheExcavationGuardedFromAccidentalEntry dropdown
	And I select "No" from the AreThereAnyVisualSignsOfPotentialSoilCollapse dropdown
	And I select "No" from the IsTheExcavationSubjectToVibration dropdown
	And I press create-excavation
	And I press Save
	And I click the "Excavations" tab
	Then I should see the following values in the excavation-details-table table
	| Date              | Width(ft) | Length(ft) | Depth(in) | Location            | Soil Type       | Created By |
	| 5/25/1985 5:05 AM | 1         | 2          | 13        | Some Location       | Some Soil       | factory    |
	| 6/26/1986 6:06 AM | 7         | 8          | 4         | Some Other Location | Some Other Soil | user       |

Scenario: User sees Add Proposed Excavation button when no excavations exist and see Add Excavation button once an excavation is created
	Given a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", supervisor sign off employee: "supervisor", has excavation: "true", safety brief date time: "08/11/2020", have equipment to do job safely: true, head protection: true, all employees wearing appropriate personal protection equipment: true, all structures supported or protected: true, reviewed location of safety equipment: true, is markout valid for site: false, reviewed ergonomic hazards: true, all materials set back from edge of trenches: true, water control systems in use: true, are exposed utilities protected: true  
	And I am logged in as "user"
	When I visit the edit page for job site check list that is not signed off by a supervisor: "one"
	And I click the "Excavations" tab
	And I press "Add Proposed Excavation"
	And I enter "5/25/1985 5:05 AM" into the editorModel_ExcavationDate field
	And I enter "113413413" into the editorModel_WidthInFeet field
	And I enter "2" into the editorModel_LengthInFeet field
	And I enter "13" into the editorModel_DepthInInches field
	And I select job site excavation location type "one" from the editorModel_LocationType dropdown
	And I select job site excavation soil type "one" from the editorModel_SoilType dropdown
	And I select "N/A" from the SpotterAssigned dropdown
	And I select "N/A" from the IsManufacturerDataOnSiteForShoringOrShieldingEquipment dropdown
	And I select "Yes" from the IsTheExcavationGuardedFromAccidentalEntry dropdown
	And I select "No" from the AreThereAnyVisualSignsOfPotentialSoilCollapse dropdown
	And I select "No" from the IsTheExcavationSubjectToVibration dropdown
	And I press create-excavation
	And I press "Add Excavation"
	And I enter "4/24/1984 4:04 AM" into the editorModel_ExcavationDate field
	And I enter "1.23" into the editorModel_WidthInFeet field
	And I enter "4.56" into the editorModel_LengthInFeet field
	And I enter "12.89" into the editorModel_DepthInInches field
	And I select job site excavation location type "one" from the editorModel_LocationType dropdown
	And I select job site excavation soil type "one" from the editorModel_SoilType dropdown
	And I press create-excavation
	And I press Save
	And I click the "Excavations" tab
	Then I should see the following values in the excavation-details-table table
	| Date              | Width(ft) | Length(ft) | Depth(in) | Location      | Soil Type | Created By |
	| 4/24/1984 4:04 AM | 1.23      | 4.56       | 12.89     | Some Location | Some Soil | user       |
	| 5/25/1985 5:05 AM | 113413413 | 2          | 13        | Some Location | Some Soil | user       |

Scenario: User can add excavations to existing checklists
	Given a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", supervisor sign off employee: "supervisor", has excavation: "true", safety brief date time: "08/11/2020", have equipment to do job safely: true, head protection: true, all employees wearing appropriate personal protection equipment: true, all structures supported or protected: true, reviewed location of safety equipment: true, is markout valid for site: false, reviewed ergonomic hazards: true, all materials set back from edge of trenches: true, water control systems in use: true, are exposed utilities protected: true  
	And a job site excavation "one" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "4/24/1984 4:04 AM", width in feet: "1.23", length in feet: "4.56", depth in inches: "12.89", job site excavation location type: "one", job site excavation soil type: "one"
	And I am logged in as "user"
	When I visit the show page for job site check list that is not signed off by a supervisor: "one"
	And I click the "Excavations" tab
	Then I should see the following values in the excavation-details-table table
	| Date              | Width(ft) | Length(ft) | Depth(in) | Location      | Soil Type |
	| 4/24/1984 4:04 AM | 1.23      | 4.56       | 12.89      | Some Location | Some Soil |
	When I visit the edit page for job site check list that is not signed off by a supervisor: "one"
	And I enter "Yes" into the CrewMembers field
	And I click the "Excavations" tab
	And I press "Add Excavation"
	And I enter "5/25/1985 5:05 AM" into the editorModel_ExcavationDate field
	And I enter "113413413" into the editorModel_WidthInFeet field
	And I enter "2" into the editorModel_LengthInFeet field
	And I enter "13" into the editorModel_DepthInInches field
	And I select job site excavation location type "one" from the editorModel_LocationType dropdown
	And I select job site excavation soil type "one" from the editorModel_SoilType dropdown
	And I press create-excavation
	And I select "N/A" from the SpotterAssigned dropdown
	And I select "N/A" from the IsManufacturerDataOnSiteForShoringOrShieldingEquipment dropdown
	And I select "Yes" from the IsTheExcavationGuardedFromAccidentalEntry dropdown
	And I select "No" from the AreThereAnyVisualSignsOfPotentialSoilCollapse dropdown
	And I select "No" from the IsTheExcavationSubjectToVibration dropdown
	And I press Save
	And I click the "Excavations" tab
	Then I should see the following values in the excavation-details-table table
	| Date              | Width(ft) | Length(ft) | Depth(in) | Location      | Soil Type | Created By |
	| 4/24/1984 4:04 AM | 1.23      | 4.56       | 12.89     | Some Location | Some Soil | factory    |
	| 5/25/1985 5:05 AM | 113413413 | 2          | 13        | Some Location | Some Soil | user       |

Scenario: User sees friendly error if they try to prepopulate data from the SAP work order number and it fails
	Given I am logged in as "user"
	And I am at the HealthAndSafety/JobSiteCheckList/New page 
	When I enter "Q" into the SAPWorkOrderId field
	And I press auto-populate-button
	Then I should see "Unable to populate data from the server."

Scenario: User can prepopulate data with the SAP work order number
	Given a work order "wo" exists with sap work order number: "1234567890", operating center: "one"
	And a map icon exists with file name: "pin_black"
	And I am logged in as "user"
	And I am at the HealthAndSafety/JobSiteCheckList/New page 
	When I enter "1234567890" into the SAPWorkOrderId field
	And I press auto-populate-button
	And I wait 2 seconds
	Then I should see work order "wo"'s Id in the MapCallWorkOrder field
	Then operating center "one"'s ToString should be selected in the OperatingCenter dropdown

Scenario: Excavations should be displayed in order by excavation date
	Given a job site check list "one" exists with operating center: "one", has excavation: "true", safety brief date time: "08/11/2020", have equipment to do job safely: true, reviewed ergonomic hazards: true
	# These three excavations need to be created with their dates out of order to test this.
	And a job site excavation "one" exists with excavation date: "4/24/1984 4:04:00 AM", job site check list: "one"
	And a job site excavation "two" exists with excavation date: "3/23/1983 3:03:00 AM", job site check list: "one"
	And a job site excavation "three" exists with excavation date: "5/25/1985 5:05:00 AM", job site check list: "one"
	And I am logged in as "user"
	When I visit the show page for job site check list: "one"
	And I click the "Excavations" tab
	Then I should see the following values in the excavation-details-table table
	| Date              | 
	| 3/23/1983 3:03 AM | 
	| 4/24/1984 4:04 AM | 
	| 5/25/1985 5:05 AM | 

Scenario: A user can NOT edit a checklist that has been approved by a supervisor
	Given a job site check list "one" exists with operating center: "one"
	And I am logged in as "user"
	When I try to visit the edit page for job site check list: "one"
	Then I should be at the show page for job site check list: "one"
	And I should see "Checklists can not be edited after it has been approved by a supervisor."

Scenario: An admin CAN edit a checklist that has been approved by a supervisor
	Given a job site check list "one" exists with operating center: "one", safety brief date time: "08/11/2020", have equipment to do job safely: true, reviewed ergonomic hazards: true
	And an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
	When I try to visit the edit page for job site check list: "one"
	Then I should be at the edit page for job site check list: "one"
	And I should not see "Checklists can not be edited after it has been approved by a supervisor."

Scenario: An user can edit a checklist and select Slings
	Given a job site check list that is not signed off by a supervisor "one" exists with operating center: "one", competent employee: "one", supervisor sign off employee: "supervisor", has excavation: "true", safety brief date time: "08/11/2020", have equipment to do job safely: true, head protection: true, all employees wearing appropriate personal protection equipment: true, all structures supported or protected: true, reviewed location of safety equipment: true, is markout valid for site: false, reviewed ergonomic hazards: true, all materials set back from edge of trenches: true, water control systems in use: true, are exposed utilities protected: true  
	And I am logged in as "user"
	And a job site excavation "one" exists with job site check list that is not signed off by a supervisor: "one", excavation date: "4/24/1984 4:04 AM", width in feet: "1.23", length in feet: "4.56", depth in inches: "12.89", job site excavation location type: "one", job site excavation soil type: "one" 
	When I try to visit the edit page for job site check list that is not signed off by a supervisor: "one"
	Then I should be at the edit page for job site check list that is not signed off by a supervisor: "one"
	When I click the "Pre Job Safety Brief" tab
	And I check the HaveYouInspectedSlings field
	And I select "N/A" from the SpotterAssigned dropdown
	And I select "N/A" from the IsManufacturerDataOnSiteForShoringOrShieldingEquipment dropdown
	And I select "Yes" from the IsTheExcavationGuardedFromAccidentalEntry dropdown
	And I select "No" from the AreThereAnyVisualSignsOfPotentialSoilCollapse dropdown
	And I select "No" from the IsTheExcavationSubjectToVibration dropdown
	And I press Save
	Then I should be at the show page for job site check list that is not signed off by a supervisor: "one"
	When I click the "Pre Job Safety Brief" tab
	Then I should see a display for HaveYouInspectedSlings with "Yes"

Scenario: User can search by operating center
	Given an operating center "two" exists
	And a job site check list "one" exists with operating center: "one", check list date: "4/24/1984", HasExcavation: "true"
	And a job site check list "two" exists with operating center: "one", check list date: "5/25/1985"
	And a job site check list "three" exists with operating center: "two", check list date: "6/26/1986", HasExcavation: "true"
	And I am logged in as "user"
	When I visit the /HealthAndSafety/JobSiteCheckList/Search page
	And I select operating center "one" from the OperatingCenter dropdown
	And I press Search
	Then I should see the following values in the search-results table
	| Operating Center | Check List Date | Is there an OSHA defined excavation or trench that will be entered by the employee? |
	| QQ1 - Wawa       | 4/24/1984       |     Yes                 |
	| QQ1 - Wawa       | 5/25/1985       |    Incomplete           |
	And I should not see "6/26/198"

# TODO: Nothing about this test is right.
# It should be testing that the New action is setting up initial values from the work order in SetDefaults.
# This test isn't doing that, as it's testing if those things were mapped *after* saving.
# Realistically, this test probably doesn't need to exist and a controller/unit test would suffice.
Scenario: User can create a safety brief from work order and it maps all the things correctly
	Given a markout requirement "emergency" exists with description: "emergency"
	And a work order "wo" exists with date completed: "12/8/1980", markout requirement: "emergency", operating center: "one"
	And a markout "one" exists with markout number: "12345", work order: "wo"
	And a coordinate "one" exists 
	And I am logged in as "user"
	# TODO: We need to figure out how to deal with these urls where the parameter is being hardcoded in. 
	# It's a very flimsy way to do this and easily breaks with any changes to factories or background step setups.
	When I visit the /HealthAndSafety/JobSiteCheckList/New?workOrderId=2 page
	And I select employee "one"'s Description from the CompetentEmployee dropdown
	And I enter "Ron Howard" into the CrewMembers field
	And I select "Yes" from the AnyPotentialWeatherHazards dropdown
	And I select "weather hazard" from the SafetyBriefWeatherHazardTypes dropdown
	And I enter "A very dangerous situation" into the HadConversationAboutWeatherHazardNotes field
	And I select "Yes" from the AnyTrafficHazards dropdown 
	And I select "traffic hazard" from the SafetyBriefTrafficHazardTypes dropdown
	And I select "Yes" from the HadConversationAboutWeatherHazard dropdown
	And I select "No" from the AnyTimeOfDayConstraints dropdown
	And I select "No" from the InvolveConfinedSpace dropdown
	And I select "No" from the AnyPotentialOverheadHazards dropdown
	And I select "No" from the AnyUndergroundHazards dropdown
	And I select "No" from the AreThereElectricalHazards dropdown
	And I check the WorkingWithACPipe field
	And I check the HaveYouInspectedSlings field
	And I select "Yes" from the CrewMembersTrainedInACPipe dropdown
	And I select "Yes" from the HaveEquipmentToDoJobSafely dropdown
	And I select "Yes" from the ReviewedErgonomicHazards dropdown 
	And I check the ReviewedLocationOfSafetyEquipment field
	And I select "No" from the OtherHazardsIdentified dropdown
	And I check the CrewMembersRemindedOfStopWorkAuthority field
	And I check the HeadProtection field
	And I check the HandProtection field
	And I check the FootProtection field
	And I check the PPEOther field
	And I enter "All the things" into the PPEOtherNotes field
	And I press Save
	When I visit the /HealthAndSafety/JobSiteCheckList/Show/1 page
	And I click the "Utility Verification" tab
	Then I should see "12345"
	And I should see a display for IsEmergencyMarkoutRequest with "Yes" 
	When I click the "General" tab
	Then I should see "1"
	And I should see a display for OperatingCenter with operating center "one"
	And I should see a display for CompetentEmployee with "Theodore Logan"
Feature: JobObservationPage
	A fridge too far - used appliances
	Annie get your gum
	Simply the Asbestos

Background: users and data for tests exists
	Given a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", state: "one"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", state: "one"
	And a town "lazytown" exists
	And operating center: "nj7" exists in town: "lazytown"
	And a role "workmanagementRoleReadNj7" exists with action: "Read", module: "FieldServicesWorkManagement", operating center: "nj7"
	And a role "workmanagementRoleReadNj4" exists with action: "Read", module: "FieldServicesWorkManagement", operating center: "nj4"
	And a role "roleReadNj7" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "OperationsHealthAndSafety", operating center: "nj7"
	And a role "roleReadNj4" exists with action: "Read", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "OperationsHealthAndSafety", operating center: "nj4"
	And a user "user" exists with username: "user"
	And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
	And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7;workmanagementRoleReadNj7;workmanagementRoleReadNj4
	And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", full name: "user admin nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
	And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And a job category "one" exists
	And an overall safety rating "one" exists
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with status: "active", first name: "Bill", last name: "S. Preston", employee id: "1000001", operating center: "nj7"
	And an employee "two" exists with status: "active", first name: "Johnny", last name: "Hotdog", employee id: "2000002", operating center: "nj4"
	And an overall quality rating "one" exists
	And a data type "data type" exists with table name: "tblJobObservations", name: "Job Observations"

Scenario: user without role cannot access the search/index/new/edit/show pages
	Given a job observation "one" exists
	And I am logged in as "user"
	When I visit the HealthAndSafety/JobObservation/Search page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"
	When I visit the HealthAndSafety/JobObservation/ page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"
	When I visit the HealthAndSafety/JobObservation/New page
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"
	When I visit the Show page for job observation: "one"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"
	When I visit the Edit page for job observation: "one"
	Then I should see "You do not have the roles necessary to access this resource."
	And I should see "OperationsHealthAndSafety"

Scenario: user can create a job observation and validation is functioning 
	Given a coordinate "one" exists
	And a work order "one" exists with operating center: "nj7"
	And a work order "two" exists with operating center: "nj4"
	And I am logged in as "user-admin-nj7"
	When I visit the /HealthAndSafety/JobObservation/New page
	And I enter "" into the ObservationDate field
	And I press Save
	Then I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The Department field is required."
	And I should see the validation message "The Coordinate field is required."
	And I should see the validation message "The OverallSafetyRating field is required."
	And I should see the validation message "The OverallQualityRating field is required."
	And I should see the validation message "The ObservationDate field is required."
	And I should see the validation message "The Address field is required."
	And I should see the validation message "The TaskObserved field is required."
	When I enter "this is my location, longer than 100 characters. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." into the Address field
	And I press Save
	Then I should see the validation message "The field Address must be a string with a maximum length of 100."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select job category "one" from the Department dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I select overall safety rating "one" from the OverallSafetyRating dropdown
	And I select overall quality rating "one" from the OverallQualityRating dropdown
	And I enter today's date into the ObservationDate field
	And I enter "this is my location, upto 100 characters long as the max field length is 100 characters." into the Address field
	And I enter "this is my description. there are many like it." into the TaskObserved field
	And I enter "1234" into the WorkOrder field
	And I enter "watch your step" into the WhyWasTaskSafeOrAtRisk field
	And I enter "not enough coffee and donuts" into the Deficiencies field
	And I enter "could be worse" into the RecommendSolutions field
	And I select "Yes" from the EqTruckForkliftsHoistsLadders dropdown
    And I select "Yes" from the EqFrontEndLoaderOrBackhoe dropdown
    And I select "Yes" from the EqOther dropdown
    And I select "Yes" from the CsPreEntryChecklistOrEntryPermit dropdown
    And I select "Yes" from the CsAtmosphereContinuouslyMonitored dropdown
    And I select "Yes" from the CsRetrievalEquipmentTripodHarnessWinch dropdown
    And I select "Yes" from the CsVentilationEquipment dropdown
    And I select "Yes" from the PpeHardHat dropdown
    And I select "Yes" from the PpeReflectiveVest dropdown
    And I select "Yes" from the PpeEyeProtection dropdown
    And I select "Yes" from the PpeEarProtection dropdown
    And I select "Yes" from the PpeFootProtection dropdown
    And I select "Yes" from the PpeGloves dropdown
    And I select "Yes" from the TcBarricadesConesBarrels dropdown
    And I select "Yes" from the TcAdvancedWarningSigns dropdown
    And I select "Yes" from the TcLightsArrowBoard dropdown
    And I select "Yes" from the TcPoliceFlagman dropdown
    And I select "Yes" from the TcWorkZoneInCompliance dropdown
    And I select "Yes" from the PsWalkwaysClear dropdown
    And I select "Yes" from the PsMaterialStockpile dropdown
    And I select "Yes" from the ExMarkoutRequestedForWorkSite dropdown
    And I select "Yes" from the ExWorkSiteSafetyCheckListUtilized dropdown
    And I select "Yes" from the ExUtilitiesSupportedProtected dropdown
    And I select "Yes" from the ExAtmosphereTestingPerformed dropdown
    And I select "Yes" from the ExSpoilPile2FeetFromEdgeOfExcavation dropdown
    And I select "Yes" from the ExLadderUsedIfGreaterThan4FeetDeep dropdown
    And I select "Yes" from the ExShoringNecessaryOver5FeetDeep dropdown
    And I select "Yes" from the ExProtectiveSystemInUseOver5Feet dropdown
    And I select "Yes" from the ExWaterControlSystemInUse dropdown
    And I select "Yes" from the ErChecklistUtilized dropdown
    And I select "Yes" from the ErErgonomicFactorsProhibitingGoodBodyMechanics dropdown
    And I select "Yes" from the ErToolsEquipmentUsedCorrectly dropdown
	And I press Save
	Then I should see the validation message "T&D WorkOrder's value does not match an existing object."
	When I enter work order "two"'s Id into the WorkOrder field
	And I press Save
	Then I should see the validation message "The Job Observation Operating Center must match the Work Order Operating Center."
	When I enter work order "one"'s Id into the WorkOrder field
	And I press Save
	Then the currently shown job observation shall henceforth be known throughout the land as "one"
	And I should be at the Show page for job observation: "one"
	And I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
	And I should see a display for Department with "JobCategory"
	And I should see a display for OverallSafetyRating with "OverallSafetyRating"
	And I should see a display for OverallQualityRating with "OverallQualityRating"
	And I should see a display for Address with "this is my location, upto 100 characters long as the max field length is 100 characters."
	And I should see a display for TaskObserved with "this is my description. there are many like it."
	And I should see a display for ObservationDate with today's date
	And I should see a link to the Show page for work order "one"
	And I should see a display for WhyWasTaskSafeOrAtRisk with "watch your step"
	And I should see a display for Deficiencies with "not enough coffee and donuts"
	And I should see a display for RecommendSolutions with "could be worse"
	And I should see a display for EqTruckForkliftsHoistsLadders with "Yes"
    And I should see a display for EqFrontEndLoaderOrBackhoe with "Yes"
    And I should see a display for EqOther with "Yes"
    And I should see a display for CsPreEntryChecklistOrEntryPermit with "Yes"
    And I should see a display for CsAtmosphereContinuouslyMonitored with "Yes"
    And I should see a display for CsRetrievalEquipmentTripodHarnessWinch with "Yes"
    And I should see a display for CsVentilationEquipment with "Yes"
    And I should see a display for PpeHardHat with "Yes"
    And I should see a display for PpeReflectiveVest with "Yes"
    And I should see a display for PpeEyeProtection with "Yes"
    And I should see a display for PpeEarProtection with "Yes"
    And I should see a display for PpeFootProtection with "Yes"
    And I should see a display for PpeGloves with "Yes"
    And I should see a display for TcBarricadesConesBarrels with "Yes"
    And I should see a display for TcAdvancedWarningSigns with "Yes"
    And I should see a display for TcLightsArrowBoard with "Yes"
    And I should see a display for TcPoliceFlagman with "Yes"
    And I should see a display for TcWorkZoneInCompliance with "Yes"
    And I should see a display for PsWalkwaysClear with "Yes"
    And I should see a display for PsMaterialStockpile with "Yes"
    And I should see a display for ExMarkoutRequestedForWorkSite with "Yes"
    And I should see a display for ExWorkSiteSafetyCheckListUtilized with "Yes"
    And I should see a display for ExUtilitiesSupportedProtected with "Yes"
    And I should see a display for ExAtmosphereTestingPerformed with "Yes"
    And I should see a display for ExSpoilPile2FeetFromEdgeOfExcavation with "Yes"
    And I should see a display for ExLadderUsedIfGreaterThan4FeetDeep with "Yes"
    And I should see a display for ExShoringNecessaryOver5FeetDeep with "Yes"
    And I should see a display for ExProtectiveSystemInUseOver5Feet with "Yes"
    And I should see a display for ExWaterControlSystemInUse with "Yes"
    And I should see a display for ErChecklistUtilized with "Yes"
    And I should see a display for ErErgonomicFactorsProhibitingGoodBodyMechanics with "Yes"
    And I should see a display for ErToolsEquipmentUsedCorrectly with "Yes"

Scenario: user can update a job observation
	Given a job observation "one" exists
	And I am logged in as "user-admin-nj7"
	When I visit the Show page for job observation: "one"
	And I follow "Edit"
	And I enter "this is not my description" into the TaskObserved field
	And I press Save
	Then I should be at the Show page for job observation: "one"
	And I should see a display for TaskObserved with "this is not my description"

Scenario: user can delete a job observation
	Given a job observation "one" exists
	And I am logged in as "user-admin-nj7"
	When I visit the Show page for job observation: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the HealthAndSafety/JobObservation/Search page
	When I try to access the Show page for job observation: "one" expecting an error
	Then I should see a 404 error message

Scenario: user can search for an apc inspection item
    Given a job observation "one" exists
	And I am logged in as "user-admin-nj7"
	When I visit the HealthAndSafety/JobObservation/Search page
    And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search

Scenario: user can search for an observation based on user operating center
	Given a coordinate "one" exists
	And a work order "one" exists
	And I am logged in as "user-admin-nj7"
	When I visit the /HealthAndSafety/JobObservation/New page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select job category "one" from the Department dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I select overall safety rating "one" from the OverallSafetyRating dropdown
	And I select overall quality rating "one" from the OverallQualityRating dropdown
	And I enter today's date into the ObservationDate field
	And I enter "this is my location" into the Address field
	And I enter "this is my description. there are many like it." into the TaskObserved field
	And I enter work order "one"'s Id into the WorkOrder field
	And I enter "watch your step" into the WhyWasTaskSafeOrAtRisk field
	And I enter "not enough coffee and donuts" into the Deficiencies field
	And I enter "could be worse" into the RecommendSolutions field
	And I select "Yes" from the EqTruckForkliftsHoistsLadders dropdown
    And I select "Yes" from the EqFrontEndLoaderOrBackhoe dropdown
    And I select "Yes" from the EqOther dropdown
    And I select "Yes" from the CsPreEntryChecklistOrEntryPermit dropdown
    And I select "Yes" from the CsAtmosphereContinuouslyMonitored dropdown
    And I select "Yes" from the CsRetrievalEquipmentTripodHarnessWinch dropdown
    And I select "Yes" from the CsVentilationEquipment dropdown
    And I select "Yes" from the PpeHardHat dropdown
    And I select "Yes" from the PpeReflectiveVest dropdown
    And I select "Yes" from the PpeEyeProtection dropdown
    And I select "Yes" from the PpeEarProtection dropdown
    And I select "Yes" from the PpeFootProtection dropdown
    And I select "Yes" from the PpeGloves dropdown
    And I select "Yes" from the TcBarricadesConesBarrels dropdown
    And I select "Yes" from the TcAdvancedWarningSigns dropdown
    And I select "Yes" from the TcLightsArrowBoard dropdown
    And I select "Yes" from the TcPoliceFlagman dropdown
    And I select "Yes" from the TcWorkZoneInCompliance dropdown
    And I select "Yes" from the PsWalkwaysClear dropdown
    And I select "Yes" from the PsMaterialStockpile dropdown
    And I select "Yes" from the ExMarkoutRequestedForWorkSite dropdown
    And I select "Yes" from the ExWorkSiteSafetyCheckListUtilized dropdown
    And I select "Yes" from the ExUtilitiesSupportedProtected dropdown
    And I select "Yes" from the ExAtmosphereTestingPerformed dropdown
    And I select "Yes" from the ExSpoilPile2FeetFromEdgeOfExcavation dropdown
    And I select "Yes" from the ExLadderUsedIfGreaterThan4FeetDeep dropdown
    And I select "Yes" from the ExShoringNecessaryOver5FeetDeep dropdown
    And I select "Yes" from the ExProtectiveSystemInUseOver5Feet dropdown
    And I select "Yes" from the ExWaterControlSystemInUse dropdown
    And I select "Yes" from the ErChecklistUtilized dropdown
    And I select "Yes" from the ErErgonomicFactorsProhibitingGoodBodyMechanics dropdown
    And I select "Yes" from the ErToolsEquipmentUsedCorrectly dropdown
	And I press Save
    Then the currently shown job observation shall henceforth be known throughout the land as "one"
	When I visit the HealthAndSafety/JobObservation/Search page
	And I select operating center "nj7" from the CreatedByOperatingCenter dropdown
    And I press Search
	And I wait for the page to reload
	Then I should see a link to the Show page for job observation: "one"

Scenario: user can select employees from either OpCenter with role access to both
	Given a job observation "one" exists
	And I am logged in as "user-admin-both"
	When I visit the HealthAndSafety/JobObservation/Search page
	And I select state "one" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "Bill" and select "S. Preston, Bill - 1000001 - NJ7" from the JobObservedBy autocomplete field
	And I enter "hotdog" and select "Hotdog, Johnny - 2000002 - NJ4" from the JobObservedBy autocomplete field
	And I press Search
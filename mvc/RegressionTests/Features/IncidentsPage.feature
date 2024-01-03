Feature: IncidentsPage
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers
	
Background: users exist
	Given a state "two" exists with name: "Pennsylvania", abbreviation: "PA"	
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"	
	And a town "lazytown" exists
	And an operating center "one" exists with opcode: "NJ7", name: "Shrewsbury", state: "one", town: "lazytown"
	And a work order "one" exists with id: "1"
	And an operating center "two" exists with opcode: "NJ4", name: "Lakewood", state: "one"	
    And an employee department "one" exists with description: "Haberdashery"
	And an employee status "active" exists with description: "Active"
	And an employee status "inactive" exists with description: "Inactive"
	And an employee "supervisor" exists with first name: "Super", last name: "Dude", employee id: "00000000", operating center: "one", status: "active"
	And a personnel area "one" exists with personnel area id: 9999, description: "Description of sorts", operating center: "one"
	And an employee "one" exists with first name: "Theodore", last name: "Logan", employee id: "11111111", operating center: "one", status: "active", reports to: "supervisor", department: "one", personnel area: "one"
	And a user "user" exists with username: "user", employee: "supervisor"
	And a user "invalid" exists with username: "invalid"
	And a role "employeerole" exists with action: "Read", module: "HumanResourcesEmployee", user: "user"
	And a role "incidentrole" exists with action: "UserAdministrator", module: "OperationsIncidents", user: "user"
	And a role "incidentdrugtestingrole" exists with action: "UserAdministrator", module: "OperationsIncidentsDrugTesting", user: "user"
	And a role "facilityrole" exists with action: "Read", module: "ProductionFacilities", user: "user"
	And a facility "one" exists with operating center: "one", facility id: "NJSB-01", facility name: "Some Facility"
	And a vehicle "one" exists with operating center: "one", vehicle identification number: "12345"
	And an incident classification "one" exists with description: "SomeIncidentClassification"
	And an incident type "one" exists with description: "SomeIncidentType"
	And an incident shift "one" exists with description: "SomeIncidentShift"
	And an incident status "open" exists with description: "Open"
	And an incident status "other" exists with description: "Other"
	And a testing decision "required" exists with description: "TEST - Reasonable Suspicion"
	And a testing decision "notrequired" exists with description: "NO TEST - Other"
	And a testing result "required" exists with description: "Test Positive"
	And a testing result "notrequired" exists with description: "Not Required"
	And a general liability code "one" exists with description: "SomeGeneralLiabilityCode"
	And a motor vehicle code "one" exists with description: "SomeMotorVehicleCode"
	And a county "one" exists with state: "one", name: "Count Countula"
	And a town "one" exists with county: "one", name: "Townie", state: "one"
	And a employee spoke with nurse "one" exists with description: "Not Offered"
	And an at risk behavior section "one" exists with description: "Rick"

	And an at risk behavior sub section "one" exists with description: "morty", section: "one", sub section number: 1
	And an event exposure type "one" exists with description: "one"
	And an body part "arm" exists with description: "arm"
	And an body part "leg" exists with description: "leg"
	And an employee type "employee" exists with description: "Employee", Id: "1"
	And an employee type "contractor" exists with description: "Contractor", Id: "2"
	And workers compensation claim statuses exist

Scenario: a user without the role cannot access the incident page
	Given I am logged in as "invalid"
	When I visit the /Incident/Search page
	Then I should see the missing role error

Scenario: User can download an incident's pdf
	Given an incident "one" exists with employee: "one", operating center: "one", employee type: "employee"
	And I am logged in as "user"
	Then I should be able to download incident "one"'s pdf

# Creating Incidents
Scenario: Employees dropdown should only display active employees for the selected operating center
	Given an employee "two" exists with first name: "Bill", last name: "Preston", employee id: "22222222", operating center: "one", status: "inactive"
	And an employee "three" exists with first name: "Rufus", last name: "McDude", employee id: "33333333", operating center: "two", status: "active"
	And I am logged in as "user"
	When I visit the Incident/New page
	And I select operating center "one" from the OperatingCenter dropdown
	Then I should see employee "one"'s Description in the Employee dropdown
	And I should not see employee "two"'s Description in the Employee dropdown
  	And I should not see employee "three"'s Description in the Employee dropdown

Scenario: Facilities dropdown should filter by operating center
	Given an operating center "no" exists with opcode: "NO1"
	And a facility "two" exists with operating center: "no", facility name: "Building of Blorps"
	And I am logged in as "user"
	When I visit the Incident/New page
	And I select operating center "one" from the OperatingCenter dropdown
	Then I should see "Some Facility - NJ7-1" in the Facility dropdown
	And I should not see "Building of Blorps - BLORP" in the Facility dropdown

Scenario: Vehicles dropdown should filter by operating center
	Given an operating center "no" exists with opcode: "NO1"
	And a vehicle "two" exists with operating center: "no", vehicle identification number: "67890"
	And I am logged in as "user"
	When I visit the Incident/New page
	And I select operating center "one" from the OperatingCenter dropdown
	Then I should see vehicle "one" in the Vehicle dropdown
	And I should not see vehicle "two" in the Vehicle dropdown

Scenario: All the validation for creating a new incident
	Given I am logged in as "user"
	And I am at the Incident/New page 
	When I press Save
	Then I should see the validation message "The OperatingCenter field is required."
	And I should see the validation message "The EmployeeType field is required."
	And I should see the validation message "The IncidentClassification field is required."
	And I should see the validation message "This field is required."
	And I should see the validation message "The Incident State field is required."
	And I should see the validation message "The Incident Coordinates field is required."
	And I should see the validation message "The Event/Exposure Type field is required."
	And I should see the validation message "The Incident Summary field is required."
	And I should see the validation message "The Decision field is required."
	And I should see the validation message "The IsOvertime field is required."
	And I should see the validation message "The IncidentShift field is required."
	When I select state "one" from the AccidentState dropdown
	And I select employee type "employee" from the EmployeeType dropdown
	And I press Save
	Then I should see the validation message "The Incident County field is required."
	When I select county "one" from the AccidentCounty dropdown
	And I press Save
	Then I should see the validation message "The Incident Town field is required."
	When I select operating center "one" from the OperatingCenter dropdown
	And I press Save
	Then I should see the validation message "The Reporting Facility field is required."
	And I should see the validation message "The Employee field is required."
	When I check the SoughtMedicalAttention field
	And I press Save
	Then I should see the validation message "The MedicalProviderName field is required."
	And I should see the validation message "The MedicalProviderPhone field is required."
	And I should see the validation message "The MedicalProviderState field is required."
	When I select state "one" from the MedicalProviderState dropdown
	And I press Save
	Then I should see the validation message "The MedicalProviderCounty field is required."
	When I select county "one" from the MedicalProviderCounty dropdown
	And I press Save
	Then I should see the validation message "The MedicalProviderTown field is required."
	When I select "Open" from the WorkersCompensationClaimStatus dropdown
	And I press Save
	Then I should see the validation message "The Claims Carrier Id field is required."

Scenario: User creates a new incident
	Given a coordinate "one" exists 
	And I am logged in as "user"
	And I am at the Incident/New page 
	When I select operating center "one" from the OperatingCenter dropdown
	And I enter "1" and select "1" from the MapCallWorkOrder autocomplete field
	And I select facility "one"'s Description from the Facility dropdown
	And I select employee type "employee" from the EmployeeType dropdown
	And I select employee "one"'s Description from the Employee dropdown 
	And I select vehicle "one" from the Vehicle dropdown
	And I enter "12/24/2013" into the IncidentDate field
	And I enter "12/25/2013" into the IncidentReportedDate field
	And I enter "12/26/2016" into the DateInvestigationWillBeCompleted field
	And I select incident classification "one" from the IncidentClassification dropdown
	And I select incident type "one" from the IncidentType dropdown
	And I select event exposure type "one" from the EventExposureType dropdown
	And I enter "some notes about the Incident" into the IncidentSummary field
	And I select incident shift "one" from the IncidentShift dropdown
	And I select "Yes" from the IsOvertime dropdown
	And I enter 1 into the NumberOfHoursOvertimeInPastWeek field
	And I check the IsInLitigation field
	And I check the IsOSHARecordable field
	And I check the IsSafetyCodeViolation field
	And I check the IsChargeableMotorVehicleAccident field
	And I enter "Case Number" into the CaseNumber field
	And I enter "TMBG" into the CaseManager field
	And I enter "UNCLAIMED FREIGHT" into the Claimant field
	And I enter "99" into the MarkoutNumber field
	And I enter "1234567890" into the PremiseNumber field
	And I select state "one" from the AccidentState dropdown
	And I select county "one" from the AccidentCounty dropdown
	And I select town "one" from the AccidentTown dropdown
	And I enter "123" into the AccidentStreetNumber field
	And I enter "Fake St." into the AccidentStreetName field
    And I enter coordinate "one"'s Id into the AccidentCoordinate field
	And I check the PoliceReportFiled field
	And I enter "Some guy" into the WitnessName field
	And I enter "555-555-5555" into the WitnessPhone field
	And I enter "Probably something they shouldn't be doing" into the QuestionEmployeeDoingBeforeIncidentOccurred field
	And I enter "Did something they weren't supposed to be doing" into the QuestionWhatHappened field
	And I enter "They got all dinged up" into the QuestionInjuryOrIllness field
	And I select body part "leg" from the BodyParts dropdown
	And I enter "The thing" into the QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee field
	And I check the QuestionHaveHadSimilarInjuryBefore field
	And I enter "1/2/2013" into the PriorInjuryDate field
	And I enter "I fell" into the NatureOfPriorInjury field
	And I enter "Some doctor at some place" into the PriorInjuryMedicalProvider field
	And I check the QuestionParticipatedInAthleticActivitiesInLastTwelveMonths field
	And I enter "I do some things" into the AthleticActivitiesInLastTwelveMonths field
	And I check the QuestionHaveJobOutsideOfAmericanWater field
	And I enter "I lied, I don't really" into the OtherEmployers field
	And I check the SoughtMedicalAttention field
	And I enter "Guy in a shack" into the MedicalProviderName field
	And I enter "444-444-4444" into the MedicalProviderPhone field
	And I select state "one" from the MedicalProviderState dropdown
	And I select county "one" from the MedicalProviderCounty dropdown
	And I select town "one" from the MedicalProviderTown dropdown
	And I select testing decision "required" from the DrugAndAlcoholTestingDecision dropdown
	And I select testing result "required" from the DrugAndAlcoholTestingResult dropdown
	And I enter "Because I said so" into the DrugAndAlcoholTestingNotes field
	And I enter "1/24/2014" into the IncidentCommitteeReportCompletionDate field
	#And I enter "1/25/2014" into the IncidentCommitteeReportTargetCompletionDate field
	And I enter "It's all good" into the IncidentCommitteeReportResults field
	And I enter "some notes about travelers" into the TravelersReport field
	And I select "Not Offered" from the EmployeeSpokeWithNurse dropdown
	And I enter "222-333-4444" into the NursePhone field
	And I select "Open" from the WorkersCompensationClaimStatus dropdown
	And I enter "Testing this box" into the ClaimsCarrierId field
	And I press Save
	Then the currently shown incident will now be referred to as "one"
	And I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
	And I should see a display for Facility with "Some Facility - NJ7-1"
	And I should see a display for Employee with "Theodore Logan"
	And I should see a display for PersonnelArea with "Description of sorts"
	# This is messy because the VehicleID value has to display and that's based off of the primary key
    And I should see a display for "Vehicle" with vehicle: "one"	
	And I should see a display for IncidentDate with "12/24/2013 12:00 AM"
	And I should see a display for IncidentReportedDate with "12/25/2013 12:00 AM"
	# And I should see a display for CreatedOn(or DateReported) with "something akin to DateTime.NowOrAFewSecondsAgo"
	And I should see a display for IncidentClassification with "SomeIncidentClassification"
	And I should see a display for IncidentType with "SomeIncidentType"
	And I should see a display for IncidentShift with "SomeIncidentShift"
	And I should see a display for IsInLitigation with "Yes"
	And I should see a display for IsOSHARecordable with "Yes"
	And I should see a display for IsSafetyCodeViolation with "Yes"
	And I should see a display for IsChargeableMotorVehicleAccident with "Yes"
	And I should see a display for IsOvertime with "Yes"
	And I should see a display for CaseNumber with "Case Number"
	And I should see a display for CaseManager with "TMBG"
	And I should see a display for Claimant with "UNCLAIMED FREIGHT"
	And I should see a display for MarkoutNumber with "99"
	And I should see a display for PremiseNumber with "1234567890"
	And I should see a display for AccidentTown_County_State with "NJ"
	And I should see a display for AccidentTown_County with "Count Countula"
	And I should see a display for AccidentTown with "Townie"
	And I should see a display for AccidentStreetNumber with "123"
	And I should see a display for AccidentStreetName with "Fake St."
	And I should see a display for PoliceReportFiled with "Yes"
	And I should see a display for WitnessName with "Some guy"
	And I should see a display for WitnessPhone with "555-555-5555"
	And I should see a display for QuestionEmployeeDoingBeforeIncidentOccurred with "Probably something they shouldn't be doing" 
	And I should see a display for QuestionWhatHappened with "Did something they weren't supposed to be doing"
	And I should see a display for QuestionInjuryOrIllness with "They got all dinged up"
	And I should only see body part "leg"'s Description in the bodyparts element
	And I should see a display for QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee with "The thing" 
	And I should see a display for QuestionHaveHadSimilarInjuryBefore with "Yes"
	And I should see a display for PriorInjuryDate with "1/2/2013"
	And I should see a display for NatureOfPriorInjury with "I fell"
	And I should see a display for PriorInjuryMedicalProvider with "Some doctor at some place"
	And I should see a display for QuestionParticipatedInAthleticActivitiesInLastTwelveMonths with "Yes"
	And I should see a display for AthleticActivitiesInLastTwelveMonths with "I do some things"
	And I should see a display for QuestionHaveJobOutsideOfAmericanWater with "Yes"
	And I should see a display for OtherEmployers with "I lied, I don't really"
	And I should see a display for NursePhone with "222-333-4444"
	And I should see a display for SoughtMedicalAttention with "Yes"
	And I should see a display for MedicalProviderName with "Guy in a shack"
	And I should see a display for MedicalProviderPhone with "(444) 444-4444"
	And I should see a display for MedicalProviderTown_County_State with "NJ"
	And I should see a display for MedicalProviderTown_County with "Count Countula"
	And I should see a display for MedicalProviderTown with "Townie"
	And I should see a display for DrugAndAlcoholTestingDecision with "TEST - Reasonable Suspicion"
	And I should see a display for DrugAndAlcoholTestingResult with "Test Positive"
	And I should see a display for DrugAndAlcoholTestingNotes with "Because I said so"
	And I should see a display for IncidentCommitteeReportCompletionDate with "1/24/2014"
	And I should see a display for IncidentCommitteeReportTargetCompletionDate with "1/3/2014"
	And I should see a display for IncidentCommitteeReportResults with "It's all good"
	And I should see a display for IncidentStatus with "Open"
	And I should see a display for TravelersReport with "some notes about travelers"
	And I should see a display for IncidentSummary with "some notes about the Incident"
	And I should see a display for WorkersCompensationClaimStatus with "Open"
	And I should see a display for ClaimsCarrierId with "Testing this box"
# Editing Incidents

Scenario: User can view an incident
	Given an incident "one" exists with employee: "one", operating center: "one", vehicle: "one", employee type: "employee"
	And I am logged in as "user"
	When I visit the Show page for incident: "one"
	Then I should see a display for incident: "one"'s Supervisor
	And I should see a display for incident: "one"'s Employee
	And I should see a display for incident: "one"'s OperatingCenter
	And I should see a display for incident: "one"'s Facility
	And I should see a display for incident: "one"'s Vehicle
	# The rest of these values are all tested in the "User creates a new incident" scenario.
	# No reason to repeat them.
	
Scenario: User cannot search or view an incident for someone that does not report to them
	Given an employee "othersupervisor" exists with first name: "Other", last name: "Guy", employee id: "00000001", operating center: "two", status: "active", state: "one"
	And an employee "foo" exists with first name: "Bill", last name: "Preston", employee id: "11111112", operating center: "two", status: "active", reports to: "othersupervisor", department: "one", state: "one"
	And an incident "foo" exists with employee: "foo", operating center: "two"
	And I am logged in as "user"
	When I visit the /Incident/Search page
	And I select state "one" from the State dropdown
	And I select operating center "one" from the OperatingCenter dropdown
	Then I should not see employee "foo"'s Description in the Employee dropdown
	When I try to visit the Show page for incident: "foo" expecting an error
	Then I should see a 404 error message

Scenario: User edits an incident and sees a bunch of validation junk
	Given an incident "one" exists with employee: "one", personnel area: "one", operating center: "one", at risk behavior section: "one", at risk behavior sub section: "one", employee type: "employee"
	And I am logged in as "user"
	When I visit the Edit page for incident: "one"
	Then I should see at risk behavior sub section "one"'s Description in the AtRiskBehaviorSubSection dropdown
	When I select "-- Select --" from the IncidentClassification dropdown
	And I press Save
	Then I should see the validation message "The IncidentClassification field is required."
	When I select incident classification "one" from the IncidentClassification dropdown
	And I enter "" into the QuestionEmployeeDoingBeforeIncidentOccurred field
	And I press Save
	Then I should see the validation message "This field is required."
	When I enter "a long description before" into the QuestionEmployeeDoingBeforeIncidentOccurred field
	And I enter "" into the QuestionWhatHappened field
	And I press Save
	Then I should see the validation message "This field is required."
	When I enter "a long description for what happened" into the QuestionWhatHappened field
	And I enter "" into the QuestionInjuryOrIllness field
	And I press Save
	Then I should see the validation message "This field is required."
	When I enter "a long description for illness" into the QuestionInjuryOrIllness field
	And I enter "" into the QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee field
	And I press Save
	Then I should see the validation message "This field is required."
	When I enter "a longer description for harmed" into the QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee field
	And I enter "some notes about travelers" into the TravelersReport field
	And I press Save
	Then I should see a display for incident: "one"'s Employee
	And I should see a display for PersonnelArea with "Description of sorts"
	And I should see a display for QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee with "a longer description for harmed"
	And I should see a display for QuestionInjuryOrIllness with "a long description for illness"
	And I should see a display for QuestionWhatHappened with "a long description for what happened"
	And I should see a display for QuestionEmployeeDoingBeforeIncidentOccurred with "a long description before"
	And I should see a display for TravelersReport with "some notes about travelers"
	And I should see a display for IncidentSummary with "Notes about Incident"

Scenario: User edits an incident and should see displays for uneditable fields
	Given an incident "one" exists with employee: "one", operating center: "one", vehicle: "one", employee type: "employee"
	And I am logged in as "user"
	When I visit the Edit page for incident: "one"
	Then I should see a display for "CurrentSupervisor" with incident: "one"'s Supervisor
	And I should see a display for "CurrentEmployee" with incident: "one"'s Employee
	And I should see a display for "CurrentOperatingCenter" with incident: "one"'s OperatingCenter
	And I should see a display for "CurrentFacility" with incident: "one"'s Facility

Scenario: User edits an incident and updates fields for contractors
	Given an incident "one" exists with operating center: "one", vehicle: "one", employee type: "contractor"
	And an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
	When I visit the Edit page for incident: "one"
	And I enter "Flip Zipp Sr" into the ContractorName field
	And I enter "Flip Liquid Inc" into the ContractorCompany field
	And I press Save
	Then I should see a display for ContractorName with "Flip Zipp Sr"
	And  I should see a display for ContractorCompany with "Flip Liquid Inc"

Scenario: Admin edits an incident and can edit the fields nonadmins can not edit
    Given an incident "one" exists with employee: "one", operating center: "one", supervisor: "supervisor", facility: "one", department: "one", employee type: "employee"
	And an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
	When I visit the Edit page for incident: "one"
	Then employee "one"'s Description should be selected in the Employee dropdown
	And operating center "one"'s Description should be selected in the OperatingCenter dropdown
	And employee "supervisor"'s Description should be selected in the Supervisor dropdown
	And facility "one"'s Description should be selected in the Facility dropdown
	And employee department "one"'s Description should be selected in the Department dropdown

Scenario: user should see investigations tab when osha recordable is false
	Given an incident "one" exists with employee: "one", operating center: "one", vehicle: "one", is o s h a recordable: "false", employee type: "employee"
	And I am logged in as "user"
	When I visit the Show page for incident: "one"
	Then I should see the "Investigations" tab

Scenario: user should see investigations tab when osha recordable is true or false
	Given an incident "one" exists with employee: "one", operating center: "one", vehicle: "one", is o s h a recordable: "true", employee type: "employee"
	And I am logged in as "user"
	When I visit the Show page for incident: "one"
	Then I should see the "Investigations" tab
	Given an incident "two" exists with employee: "one", operating center: "one", vehicle: "one", is o s h a recordable: "false", employee type: "employee"
	And I am logged in as "user"
	When I visit the Show page for incident: "two"
	Then I should see the "Investigations" tab

Scenario: user should not see five whys tab when five whys is false
	Given an incident "one" exists with employee: "one", operating center: "one", vehicle: "one", employee type: "employee" 
	And I am logged in as "user"
	When I visit the Show page for incident: "one"
	Then I should not see the "Five Whys" tab

Scenario: user should see five whys tab when five whys is true
	Given an incident "one" exists with employee: "one", operating center: "one", vehicle: "one", FiveWhysCompleted: true, employee type: "employee"
	And I am logged in as "user"
	When I visit the Show page for incident: "one"
	Then I should see the "Five Whys" tab

Scenario: User edits an incident and adds the five whys
	Given an incident "one" exists with employee: "one", operating center: "one", at risk behavior section: "one", at risk behavior sub section: "one", employee type: "employee", incident summary: "test"
	And I am logged in as "user"
	When I visit the Edit page for incident: "one"
	And I click the "Five Whys" tab 
	And I select "Yes" from the FiveWhysCompleted dropdown
	When I enter "I fell onto the ground" into the Why1 field
	And I enter "I slipped on my hoagie" into the Why2 field
	And I enter "I tried to punt kick my hoagie but i missed" into the Why3 field
	And I enter "the hoagie had sundried tomatoes" into the Why4 field
	And I enter "I am allergic to sundried tomatoes" into the Why5 field
	When I press "Save"
	Then I should see the validation message "This field is required when 'Five Whys Completed' is Yes."
	When I enter "2/21/2023" into the DateSubmitted field
	And  I press "Save"
	Then I should see the "Five Whys" tab
	When I click the "Five Whys" tab 
	Then I should see a display for Why1 with "I fell onto the ground" 
	And I should see a display for Why2 with "I slipped on my hoagie"
	And I should see a display for Why3 with "I tried to punt kick my hoagie but i missed"
	And I should see a display for Why4 with "the hoagie had sundried tomatoes"
	And I should see a display for Why5 with "I am allergic to sundried tomatoes"
	And I should see a display for DateSubmitted with "2/21/2023"

Scenario: user with edit role can add, edit, and remove an investigation
	Given an incident "one" exists with employee: "one", operating center: "one", vehicle: "one", is o s h a recordable: "true", employee type: "employee"
	And a user "two" exists with default operating center: "one", full name: "Dude McDude"
	And a incident investigation root cause finding type "one" exists with description: "cool root"
	And a incident investigation root cause level1 type "one" exists with description: "level 1"
	And a incident investigation root cause level2 type "one" exists with description: "level 2", incident investigation root cause level1 type: "one"
	And a incident investigation root cause level3 type "one" exists with description: "level 3", incident investigation root cause level2 type: "one"
	And a incident investigation root cause level3 type "two" exists with description: "level 3 but not", incident investigation root cause level2 type: "one"
	And I am logged in as "user"
	When I visit the Show page for incident: "one"
	And I click the "Investigations" tab 
	And I press "Add Investigation"
	And I select incident investigation root cause finding type "one" from the IncidentInvestigationRootCauseFindingType dropdown
	And I select incident investigation root cause level1 type "one" from the IncidentInvestigationRootCauseLevel1Type dropdown
	And I select incident investigation root cause level2 type "one" from the IncidentInvestigationRootCauseLevel2Type dropdown
	And I select incident investigation root cause level3 type "one" from the IncidentInvestigationRootCauseLevel3Type dropdown
	And I select user "two"'s FullName from the RootCauseFindingPerformedByUsers dropdown
	And I press save-investigation
	Then I should see the following values in the investigations-table table
	| Root Cause Finding | Finding Performed By | Root Cause Level 1 | Root Cause Level 2 | Root Cause Level 3 |
	| cool root          | Dude McDude          | level 1            | level 2            | level 3            |
    When I click the "Edit" link in the 1st row of investigations-table
	And I select incident investigation root cause level3 type "two" from the IncidentInvestigationRootCauseLevel3Type dropdown
	And I press "Save" 
	And I click the "Investigations" tab
	Then I should see the following values in the investigations-table table
	| Root Cause Finding | Finding Performed By | Root Cause Level 1 | Root Cause Level 2 | Root Cause Level 3 |
	| cool root          | Dude McDude          | level 1            | level 2            | level 3 but not    |
	When I click the "Remove" button in the 1st row of investigations-table and then click ok in the confirmation dialog
	And I wait for the page to reload
	Then the investigations-table table should be empty
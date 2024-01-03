Feature: Facility Page
	In order to manage and view facilities
	As a user
	I want to be view and manage facilities

Background: users and supporting data exist
    Given a user "user" exists with username: "user"
    And an admin user "admin" exists with username: "admin"
	And a user "other" exists with username: "other"
    And a role "roleRead" exists with action: "Read", module: "ProductionFacilities", user: "user"
    And a role "roleEdit" exists with action: "Edit", module: "ProductionFacilities", user: "user"
    And a role "roleEditOther" exists with action: "Edit", module: "ProductionFacilities", user: "other"
    And a role "roleAdd" exists with action: "Add", module: "ProductionFacilities", user: "user"
    And a role "roleDelete" exists with action: "Delete", module: "ProductionFacilities", user: "user"
    And an operating center "opc" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing     Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
    And a role "roleFacilityReadOpc" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "opc"
    And a role "roleFacilityAddOpc" exists with action: "Add", module: "ProductionFacilities", user: "user", operating center: "opc"
    And a role "roleFacilityEditOpc" exists with action: "Edit", module: "ProductionFacilities", user: "user", operating center: "opc"
    And a role "roleOtherFacilityEditOpc" exists with action: "Edit", module: "ProductionFacilities", user: "other", operating center: "opc"
	And a role "roleEditJ100" exists with action: "Edit", module: "EngineeringJ100AssessmentData", user: "user"
	And a role "roleDeleteJ100" exists with action: "Delete", module: "EngineeringJ100AssessmentData", user: "user"
	And a role "roleEditAsset" exists with action: "Edit", module: "ProductionAssetReliability", user: "other"
	And a role "roleAddAsset" exists with action: "Add", module: "ProductionAssetReliability", user: "other"
	And a role "roleAdminUserAsset" exists with action: "UserAdministrator", module: "ProductionAssetReliability", user: "other"
	And a role "roleDeleteAsset" exists with action: "Delete", module: "ProductionAssetReliability", user: "other"
	And a role "roleEditFacilityAreaManagement" exists with action: "Edit", module: "ProductionFacilityAreaManagement", user: "user"
	And a role "roleEditChemicalStorageLocation" exists with action: "Edit", module: "EnvironmentalChemicalData", user: "user"
    And a town "lazytown" exists
    And operating center: "opc" exists in town: "lazytown"
    And a town section "active" exists with town: "lazytown"
    And a town section "inactive" exists with town: "lazytown", name: "Inactive Section", active: false
    And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
    And a department "production" exists with description: "Production"
    And an asset type "facility" exists with description: "facility"
    And a functional location "one" exists with description: "NJRB-UN-FAC-0003", asset type: "facility", town: "lazytown"
    And facility statuses exist
	And facility risk characteristics exist
	And maintenance risk of failures exist
	And facility asset management maintenance strategy tiers exist
	And facility likelihood of failures exist
	And equipment failure risk ratings exist
    And a process stage "one" exists with description: "Chemical Treatment"
    And a system delivery type "water" exists with description: "Water"
    And a system delivery type "wastewater" exists with description: "Waste water"
    And a facility area "one" exists with description: "facilityLab"
    And a facility sub area "one" exists with description: wet, area: "one"
    And a public water supply "pws-01" exists with identifier: "NJ1345001", system: "Coastal North Monmouth/Lakewood", usage last year: 101
    And a public water supply "pws-02" exists with identifier: "NJ1345002", system: "Coastal North Monmouth/Lakewood Intake", usage last year: 120
    And operating center: "opc" exists in public water supply: "pws-01"
    And public water supply: "pws-01" exists in town: "lazytown"
    And a public water supply pressure zone "pz-01" exists with public water supply: "pws-01", hydraulic model name: "pressure zone - 01"
    And a waste water system "wws-01" exists with id: 1, waste water system name: "waste water system - 01", operating center: "opc"
    And waste water system: "wws-01" exists in town: "lazytown" 
    And a waste water system basin "wwsb-01" exists with WasteWaterSystem: "wws-01", BasinName: "basin - 01"
	And a facility condition "good" exists with description: "Good"
	And a facility performance "good" exists with description: "Good"
	And a insurance score quartile "1" exists with id: 1, description: "1"
	And a state "NJ" exists with abbreviation: "NJ"
	And chemical storage locations exist

Scenario: user can view facilities
	Given a facility "one" exists with facility id: "NJSB-01", entity notes: "this is an entity note", facility status: "inactive"
	And I am logged in as "user"
	When I visit the Show page for facility: "one"
	Then I should see a display for facility: "one"'s FacilityId
	And I should see a display for facility: "one"'s EntityNotes
	And I should see a display for FacilityStatus_Description with facility status "inactive"
	When I go to the Show frag for facility: "one"
	Then I should not see "error"
	And I should see facility: "one"'s FacilityId on the page
	And I should see facility: "one"'s EntityNotes on the page
	
Scenario: user can view a list of facilities
	Given a facility "one" exists with facility id: "NJ7-1", facility status: "active", created by: "user"
	And a facility "two" exists with facility id: "NJ7-2", facility status: "inactive"
	And a facility "three" exists with facility id: "NJ7-3", facility status: "pending"
	And a facility "four" exists with facility id: "NJ7-4", facility status: "pending_retirement"
	And I am logged in as "user"
	When I visit the Facility page
	Then I should see a link to the Show page for facility: "one"
	And I should see a link to the Show page for facility: "two"
	And I should see a link to the Show page for facility: "three"
	And I should see a link to the Show page for facility: "four"
	And the td elements in the 2nd row of the "facility-table" table should have a "background-color" value of "#ffbdbd"						
	
Scenario: User can search for a facility
	Given I am logged in as "user"
	And a facility "one" exists with facility id: "NJ4-1", basic ground water supply: true, raw water pump station: true, insurance id: "abc", insurance score: 1.2, insurance score quartile: "1", insurance visit date: "3/6/2023"
	When I visit the Facility/Search page
	And I select "NJ" from the State dropdown
	And I select operating center "opc" from the OperatingCenter dropdown
	And I select "Yes" from the BasicGroundWaterSupply dropdown
	And I select "Yes" from the RawWaterPumpStation dropdown
	And I enter "1.2" into the InsuranceScore field
	And I select "1" from the InsuranceScoreQuartile dropdown
	When I press Search
	And I press Search
	Then I should see a link to the Show page for facility "one"
	When I follow the Show link for facility "one"
	Then I should be at the Show page for facility "one"

Scenario: User can search for a facility with an arc flash study
	Given I am logged in as "user"
	And a facility "one" exists with facility id: "NJ4-1"
	When I visit the Facility/Search page
	And I select "Yes" from the HasExpiringArcFlashStudy dropdown
	When I press Search
	Then I should not see "Cannot simultaneously fetch multiple bags."
	When I visit the Facility/Search page
	And I select "NJ" from the State dropdown
	And I select operating center "opc" from the OperatingCenter dropdown
	When I press Search
	Then I should not see "Cannot simultaneously fetch multiple bags."

Scenario: User can search for a facility using IsInVamp
	Given I am logged in as "user"
	And a facility "one" exists with facility id: "NJ4-1"
	And a facility "two" exists with is in vamp: "true"
	When I visit the Facility/Search page
	And I check the IsInVamp field
	And I press Search
	Then I should see a link to the Show page for facility "two"
	And I should not see a link to the Show page for facility "one"
	When I follow the Show link for facility "two"
	Then I should be at the Show page for facility "two"
	
Scenario: user can add a facility
	Given I am logged in as "user"
	When I visit the Facility/New page
	Then I should not see the RMPNumber field
	When I press Save
	Then I should see the validation message The Operating Center field is required.
	When I select department "production"'s Description from the Department dropdown
	And I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I select town "lazytown"'s ShortName from the Town dropdown
	And I enter "NJLK-HO-ADDIS" into the FunctionalLocation field
	And I check the IsInVamp field
	And I enter "abcdef" into the VampUrl field
	And I press Save
	Then I should see the validation message The VAMP Url field is not a valid fully-qualified http, https, or ftp URL.
	When I enter "http://vamptest.abc" into the VampUrl field
	And I select "No" from the ArcFlashStudyRequired dropdown
	And I check the RMP field
	Then I should see the RMPNumber field
	When I uncheck the RMP field
	Then I should not see the RMPNumber field
	When I check the RMP field
	And I press Save
	Then I should see the validation message The RMPNumber field is required.
	And I should not see the WaterStress field
	When I check the PointOfEntry field
	And I press Save
	Then I should see the validation message The SystemDeliveryType field is required.
	When I select system delivery type "water" from the SystemDeliveryType dropdown
	And I check the WaterStress field
	When I enter "123456789123" into the RMPNumber field
	And I check the SWMStation field
	And I check the WellProd field
	And I check the WellMonitoring field
	And I check the ClearWell field
	And I check the RawWaterIntake field
	And I check the SampleStation field
	And I check the Radionuclides field
	And I check the CommunityRightToKnow field
	And I check the IgnitionEnterprisePortal field
	And I check the ArcFlashLabelRequired field
	And I check the BasicGroundWaterSupply field
	And I check the RawWaterPumpStation field
	And I select system delivery type "water" from the SystemDeliveryType dropdown
	And I press Save
	Then I should see the validation message Please select either a public water supply or a waste water system
	When I select public water supply "pws-01" from the PublicWaterSupply dropdown
	And I press Save
	Then I should see the validation message Please select a public water supply pressure zone
	When I select "-- Select --" from the PublicWaterSupply dropdown
	And I select waste water system "wws-01" from the WasteWaterSystem dropdown
	And I press Save
	Then I should see the validation message Please select a waste water system basin
	When I select waste water system basin "wwsb-01" from the WasteWaterSystemBasin dropdown
	And I enter "rpa12345" into the RegionalPlanningArea field
	And I enter "test" into the InsuranceId field
	And I enter "1.2" into the InsuranceScore field
	And I select "1" from the InsuranceScoreQuartile dropdown
	And I enter "3/17/2020" into the InsuranceVisitDate field
	And I press Save
	And I wait for the page to reload
	Then I should see "Edit"
	And I should see a display for SystemDeliveryType with system delivery type "water"
	And I should see a display for FacilityId with "NJ4-1"
	And I should see a display for OperatingCenter_OperatingCenterCode with "NJ4"
	And I should see a display for "Department_Description" with department: "production"'s Description
	And I should see a display for FunctionalLocation with "NJLK-HO-ADDIS"
	And I should see a display for RMPNumber with "123456789123"
	And I should see a display for WasteWaterSystem with waste water system "wws-01"
	And I should see a display for WasteWaterSystemBasin with waste water system basin "wwsb-01"
	And I should see a display for RegionalPlanningArea with "rpa12345" 
	And I should see a display for IgnitionEnterprisePortal with "Yes"
	And I should see a display for ArcFlashLabelRequired with "Yes"
	And I should see a display for BasicGroundWaterSupply with "Yes"
	And I should see a display for RawWaterPumpStation with "Yes"
	And I should see a display for InsuranceId with "test"
	And I should see a display for InsuranceScore with "1.2"
	And I should see a display for InsuranceScoreQuartile with "1"
	And I should see a display for InsuranceVisitDate with "3/17/2020 12:00:00 AM"

Scenario: user can add a facility with arc flash study
	Given I am logged in as "user"
	When I visit the Facility/New page
	And I press Save
	Then I should see the validation message The Operating Center field is required.
	When I select department "production"'s Description from the Department dropdown
	And I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I select town "lazytown"'s ShortName from the Town dropdown
	And I enter "NJLK-HO-ADDIS" into the FunctionalLocation field
	And I select "Yes" from the ArcFlashStudyRequired dropdown
	And I select public water supply "pws-01" from the PublicWaterSupply dropdown
	And I select public water supply pressure zone "pz-01" from the PublicWaterSupplyPressureZone dropdown
	And I press Save
	And I wait for the page to reload
	Then I should be at the Engineering/ArcFlashStudy/New page

Scenario: user cannot add a facility for an inactive town section
	Given I am logged in as "user"
	When I visit the Facility/New page
	And I select operating center "opc"'s Name from the OperatingCenter dropdown
    And I wait for ajax to finish loading
	And I select town "lazytown"'s ShortName from the Town dropdown
    And I wait for ajax to finish loading
    Then I should not see town section "inactive" in the TownSection dropdown
	
Scenario: user can edit a facility
	Given a utility transformer k v a rating "one" exists with description: "Neat"
	And a facility "one" exists with facility id: "NJSB-01"
	And I am logged in as "user"
	When I visit the Edit page for facility: "one"
	Then I should not see the RMPNumber field
	When I select "-- Select --" from the OperatingCenter dropdown
	And I select "No" from the ArcFlashStudyRequired dropdown
	And I check the RMP field
	Then I should see the RMPNumber field
	When I uncheck the RMP field
	Then I should not see the RMPNumber field
	When I check the RMP field
	And I press Save
	Then I should see the validation message The RMPNumber field is required.
	When I enter "123456789123" into the RMPNumber field
	And I press Save
	Then I should see the validation message The Operating Center field is required.
	And I should not see the WaterStress field
	When I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I select town "lazytown"'s ShortName from the Town dropdown
	And I check the WaterTreatmentFacility field
	And I enter "NJLK-HO-ADDIS" into the FunctionalLocation field
	And I check the SWMStation field
	And I check the WellProd field
	And I check the WellMonitoring field
	And I check the ClearWell field
	And I check the RawWaterIntake field
	And I check the SampleStation field
	And I check the Radionuclides field
	And I check the CommunityRightToKnow field
	And I check the IgnitionEnterprisePortal field
	And I uncheck the ArcFlashLabelRequired field
	And I select process stage "one" from the Process dropdown
	And I check the PointOfEntry field
	And I select system delivery type "wastewater" from the SystemDeliveryType dropdown	
	And I check the WaterStress field
	And I check the IsInVamp field
	And I enter "abcdef" into the VampUrl field
	And I enter "test" into the InsuranceId field
	And I enter "1.2" into the InsuranceScore field
	And I select "1" from the InsuranceScoreQuartile dropdown
	And I enter "3/17/2020" into the InsuranceVisitDate field
	And I press Save
	Then I should see the validation message The VAMP Url field is not a valid fully-qualified http, https, or ftp URL.
	When I enter "http://vamptest.abc" into the VampUrl field
	And I select system delivery type "wastewater" from the SystemDeliveryType dropdown
	When I enter "rpa12345" into the RegionalPlanningArea field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for FacilityId with "NJ4-1"
	And I should see a display for OperatingCenter_OperatingCenterCode with "NJ4"
	And I should see a display for WaterTreatmentFacility with "Yes"
	And I should see a display for FunctionalLocation with "NJLK-HO-ADDIS"
	And I should see a display for RMPNumber with "123456789123"
	And I should see a display for Process with process stage "one"
	And I should see a display for SystemDeliveryType with system delivery type "wastewater"
	And I should see a display for RegionalPlanningArea with "rpa12345"
	And I should see a display for IgnitionEnterprisePortal with "Yes"
	And I should see a display for ArcFlashLabelRequired with "No"
	And I should see a display for BasicGroundWaterSupply with "No"
	And I should see a display for RawWaterPumpStation with "No"
	And I should see a display for InsuranceId with "test"
	And I should see a display for InsuranceScore with "1.2"
	And I should see a display for InsuranceScoreQuartile with "1"
	And I should see a display for InsuranceVisitDate with "3/17/2020 12:00:00 AM"

Scenario: user can edit Assessment Data section in risk characteristics tab with J100 role
	Given a utility transformer k v a rating "one" exists with description: "Neat"
	And a facility "one" exists with facility id: "NJSB-01"
	And I am logged in as "user"
	When I visit the Edit page for facility: "one"
	Then I should not see the RMPNumber field
	When I select operating center "opc"'s Name from the OperatingCenter dropdown
	And I click the "Risk Characteristics" tab
	And I select "Yes" from the CriticalFacilityIdentified dropdown	
	And I press Save
	Then I should see the validation message The J100 Assessment Completed On Date field is required.
	When I enter "3/17/2020" into the AssessmentCompletedOn field
	And I press Save
	And I wait for the page to reload
	Then I should see a display for FacilityId with "NJ4-1"
	And I should see a display for OperatingCenter_OperatingCenterCode with "NJ4"
	When I click the "Risk Characteristics" tab
	Then I should see a display for CriticalFacilityIdentified with "Yes"
	Then I should see a display for AssessmentCompletedOn with "3/17/2020"
	And I should not see the RiskBasedCompletedDate field

Scenario: user can edit Risk Based Maintenance section in risk characteristics tab with asset reliability
	Given a utility transformer k v a rating "one" exists with description: "Neat"
	And a facility "one" exists with operating center: "opc", town: "lazytown", facility id: "NJSB-01", public water supply: "pws-01"
	And I am logged in as "other"
	When I visit the Edit page for facility: "one"
	Then I should not see the RMPNumber field
	When I click the "Risk Characteristics" tab
	And I select facility consequence of failure "low" from the ConsequenceOfFailure dropdown
	And I press Save
	And I wait for the page to reload
	Then I should see a display for FacilityId with "NJ4-1"
	And I should see a display for OperatingCenter_OperatingCenterCode with "NJ4"
	When I click the "Risk Characteristics" tab
	Then I should see a display for RiskBasedCompletedDate with ""
	And I should not see the CriticalFacilityIdentified field
	And I should not see the AssessmentCompletedOn field
	And I should see a display for LikelihoodOfFailure with ""
	And I should see a display for ConsequenceOfFailure with facility consequence of failure "low"
	And I should see a display for MaintenanceRiskOfFailure with ""
	And I should see a display for StrategyTier with ""
	And I should see a display for ConsequenceOfFailureFactor with "0.5"
	And I should see a display for WeightedRiskOfFailureScore with ""

Scenario: user can edit a facility and set town section to an inactive town section
   # Fun fact, if you don't set the operating center on this facility, it does not end up with the
   # same operating center that the town is associated with in the background step way up top.
   # Then you spend hours trying to figure out what you broke because the TownSection cascade stopped
   # working. Then you find out you didn't break anything. You just uncovered something that 
   # worked when it shouldn't have.
	Given a facility "one" exists with town: "lazytown", operating center: "opc"
	And I am logged in as "user"
	When I visit the Edit page for facility: "one"
    And I wait for ajax to finish loading
    Then I should see town section "inactive" in the TownSection dropdown

Scenario: user can destroy a facility
	Given a facility "demo" exists with facility id: "NJGB-01"
	And I am logged in as "user"
	When I visit the Show page for facility: "demo"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Facility/Search page
	When I try to access the Show page for facility: "demo" expecting an error
    Then I should see a 404 error message

Scenario: read-only user cannot modify filter media, notes, or documents
    Given a user "readonly" exists with username: "readonly", roles: roleRead
	And a facility "one" exists with facility id: "NJSB-01"
    And I am logged in as "readonly"
    When I visit the Show page for facility: "one"
    When I click the "Notes" tab
    Then I should not see the toggleNewNote element
    When I click the "Documents" tab
    Then I should not see the toggleLinkDocument element
    And I should not see the toggleNewDocument element

Scenario: user can view readings chart
    Given a user "readonly" exists with username: "readonly", roles: roleRead
	And a facility "one" exists with facility id: "NJSB-01"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one"
	And a sensor measurement type "kw" exists with description: "kW"
    And a sensor "one" exists with name: "Sensor One", description: "Sensor Description", sensor measurement type: "kw"
    And an equipment sensor exists with equipment: "one", sensor: "one"
    And a reading "one" exists with sensor: "one", date: "6/3/2014 12:15 PM", rawdata: "1", scaleddata: "2", interpolate: "3", checksum: "4"
	And I am logged in as "readonly"
    When I visit the Show page for facility: "one"
    And I click the "Readings" tab
	And I enter "6/3/2014" into the StartDate field
	And I enter "6/4/2014" into the EndDate field
	And I press "Display Readings"
	And I wait for ajax to finish loading
	Then I should see the chart-0 element

Scenario: User can see readings and costs table
    Given a user "readonly" exists with username: "readonly", roles: roleRead
	And a facility "one" exists with facility id: "NJSB-01"
	And a facility kwh cost "one" exists with facility: "one", cost per kwh: "2.00", start date: "6/1/2014", end date: "6/30/2014"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one"
	And a sensor measurement type "kw" exists with description: "kW"
	And a sensor "one" exists with name: "Sensor One", description: "Sensor Description", sensor measurement type: "kw"
	And an equipment sensor exists with equipment: "one", sensor: "one"
	And a reading "one" exists with sensor: "one", date: "6/3/2014 12:00 PM", rawdata: "1", scaleddata: "2", interpolate: "3", checksum: "4"
	And a reading "two" exists with sensor: "one", date: "6/4/2014 12:00 PM", rawdata: "1", scaleddata: "3", interpolate: "3", checksum: "4"
	And a reading "three" exists with sensor: "one", date: "6/5/2014 12:00 PM", rawdata: "1", scaleddata: "4", interpolate: "3", checksum: "4"
	And I am logged in as "readonly"
    When I visit the Show page for facility: "one"
    And I click the "Readings" tab
	And I enter "6/3/2014" into the StartDate field
	And I enter "6/5/2014" into the EndDate field
	And I press "Display Readings"
	And I wait for ajax to finish loading
	And I press "Raw Readings Data and Costs"
	Then I should see the following values in the readings-table table
	| Date     | Reading Value | Kwh Cost | Total |
	| 6/3/2014 | 0.5           | $2.00    | $1.00 |
	| 6/4/2014 | 0.75          | $2.00    | $1.50 |
	| 6/5/2014 | 1             | $2.00    | $2.00 |
	And I should see a display for Total with "$4.50"

# Removed until process tabs are activated in the future
#Scenario: user with edit rights can add and remove a process
#	Given a facility "one" exists with facility id: "NJSB-01"
#	And a process "one" exists with process stage: "one"
#	And I am logged in as "user"
#	When I visit the Show page for facility: "one"
#	And I click the "Processes" tab
#	And I press "Add Process to Facility"
#	And I select process stage "one" from the ProcessStage dropdown
#	And I select process "one" from the Process dropdown
#	And I press "Save Process"
#	# Page reloads here
#	And I click the "Processes" tab
#	And I press "Add Process to Facility"
#	Then I should see the following values in the facility-processes-table table
#         | Process Stage        | Process        |
#         | process stage: "one" | process: "one" |

Scenario: user can see system delivery tab and entry types but cannot add or remove
	Given a facility "one" exists with facility id: "NJSB-01", system delivery type: "water", point of entry: "true"
	And a system delivery entry type "one" exists with description: "Delivered Water", system delivery type: "water"
	And a facility system delivery entry type "one" exists with facility: "one", system delivery entry type: "one", is enabled: true, minimum value: 1.618, maximum value: 3.141
	And I am logged in as "user"
	When I visit the Show page for facility: "one"
	Then I should not see the button "Add System Delivery Entry Type" under the "System Delivery" tab
	And I should see the following values in the system-delivery-entry-types-table table
         | System Delivery Entry Type        | Is Enabled | Minimum Value | Maximum Value |
         | system delivery entry type: "one" | Yes        | 1.618         | 3.141         |
	And I should not see the button "Remove"

Scenario: user can add and remove facility system delivery entry types
	Given a facility "one" exists with facility id: "NJSB-01", system delivery type: "water", point of entry: "true"
	And a system delivery entry type "one" exists with description: "Purchased Water", system delivery type: "water"
	And a system delivery entry type "two" exists with description: "Delivered Water", system delivery type: "water"
	And a role "roleAddSysDel" exists with action: "Add", module: "ProductionSystemDeliveryConfiguration", user: "user"
	And a role "roleDeleteSysDel" exists with action: "Delete", module: "ProductionSystemDeliveryConfiguration", user: "user"
	And I am logged in as "user"
	When I visit the Show page for facility: "one"
	And I click the "System Delivery" tab
	And I press "Add System Delivery Entry Type"
	And I select system delivery entry type "two" from the SystemDeliveryEntryType dropdown
	And I press "Save Entry Type"
	Then I should see a validation message for BusinessUnit with "The BusinessUnit field is required."
	When I enter "123" into the BusinessUnit field
	Then I should see a validation message for BusinessUnit with "Business unit must be 6 digits long."
	When I enter "123456" into the BusinessUnit field
	And I select "Yes" from the IsEnabled dropdown
	Then I should not see a validation message for MinimumValue with "The MinimumValue field is required."
	And I should not see a validation message for MaximumValue with "The MaximumValue field is required."
	When I enter "1.618" into the MinimumValue field
	And I enter "3.141" into the MaximumValue field
	And I select "-- Select --" from the IsInjectionSite dropdown
	And I select "-- Select --" from the IsAutomationEnabled dropdown
	And I press "Save Entry Type"
	Then I should see a validation message for IsInjectionSite with "The IsInjectionSite field is required."
	Then I should see a validation message for IsAutomationEnabled with "The IsAutomationEnabled field is required."
	When I select "Yes" from the IsInjectionSite dropdown
	And I select "Yes" from the IsAutomationEnabled dropdown
	And I press "Save Entry Type"
	And I wait for the page to reload
	# Page reloads here
	And I click the "System Delivery" tab
	Then I should see the following values in the system-delivery-entry-types-table table
         | System Delivery Entry Type        | Business Unit | Is Enabled | Minimum Value | Maximum Value | Is Injection Site | Is Automation Enabled
         | system delivery entry type: "two" | 123456        | Yes        | 1.618         | 3.141         | Yes               | Yes
	When I click the "Remove" button in the 1st row of system-delivery-entry-types-table and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "System Delivery" tab
	Then the system-delivery-entry-types-table table should be empty

Scenario: user can not add multiple of the same entry type
	Given a facility "one" exists with facility id: "NJSB-01", system delivery type: "water", point of entry: "true"
	And a system delivery entry type "one" exists with description: "Purchased Water", system delivery type: "water"
	And a system delivery entry type "two" exists with description: "Delivered Water", system delivery type: "water"
	And a role "roleAddSysDel" exists with action: "Add", module: "ProductionSystemDeliveryConfiguration", user: "user"
	And a role "roleDeleteSysDel" exists with action: "Delete", module: "ProductionSystemDeliveryConfiguration", user: "user"
	And I am logged in as "user"
	When I visit the Show page for facility: "one"
	And I click the "System Delivery" tab
	And I press "Add System Delivery Entry Type"
	And I select system delivery entry type "two" from the SystemDeliveryEntryType dropdown
	And I enter "123456" into the BusinessUnit field
	And I select "Yes" from the IsEnabled dropdown
	Then I should not see a validation message for MinimumValue with "The MinimumValue field is required."
	And I should not see a validation message for MaximumValue with "The MaximumValue field is required."
	When I enter "1.618" into the MinimumValue field
	And I enter "3.141" into the MaximumValue field
	And I press "Save Entry Type" 
	And I wait for the page to reload
	# Page reloads here
	And I click the "System Delivery" tab
	Then I should see the following values in the system-delivery-entry-types-table table
         | System Delivery Entry Type        | Business Unit | Is Enabled | Minimum Value | Maximum Value | Is Injection Site | Is Automation Enabled
         | system delivery entry type: "two" | 123456        | Yes        | 1.618         | 3.141         | No                | No
	When I visit the Show page for facility: "one"
	And I click the "System Delivery" tab
	And I press "Add System Delivery Entry Type"
	And I select system delivery entry type "two" from the SystemDeliveryEntryType dropdown
	And I enter "123456" into the BusinessUnit field
	And I select "Yes" from the IsEnabled dropdown
	Then I should not see a validation message for MinimumValue with "The MinimumValue field is required."
	And I should not see a validation message for MaximumValue with "The MaximumValue field is required."
	When I enter "1.618" into the MinimumValue field
	And I enter "3.141" into the MaximumValue field
	And I select "Yes" from the IsInjectionSite dropdown
	And I press "Save Entry Type"
	And I wait for the page to reload
	Then I should see "Facility can have only one active entry per entry type configured at a time"
	And I should see the following values in the system-delivery-entry-types-table table
         | System Delivery Entry Type        | Is Enabled | Minimum Value | Maximum Value | Is Injection Site | Is Automation Enabled
         | system delivery entry type: "two" | Yes        | 1.618         | 3.141         | No                | No

Scenario: User should see required fields when adding entry type transferred from or transferred to
	Given a facility "one" exists with facility id: "NJSB-01", system delivery type: "water", point of entry: "true"
	And an operating center "nj7" exists
	And a facility "two" exists with facility id: "NJSB-02", point of entry: "true"
	And a system delivery entry type "one" exists with description: "Purchase Point", system delivery type: "water"
	And a system delivery entry type "two" exists with description: "Delivered Water", system delivery type: "water"
	And a system delivery entry type "three" exists with description: "Transferred To", system delivery type: "water"
	And a system delivery entry type "four" exists with description: "Transferred From", system delivery type: "water"
	And a role "roleAddSysDel" exists with action: "Add", module: "ProductionSystemDeliveryConfiguration", user: "user"
	And a role "roleDeleteSysDel" exists with action: "Delete", module: "ProductionSystemDeliveryConfiguration", user: "user"
	And I am logged in as "user"
	When I visit the Show page for facility: "one"
	And I click the "System Delivery" tab
	And I press "Add System Delivery Entry Type" 
	And I select system delivery entry type "one" from the SystemDeliveryEntryType dropdown
	And I enter "123456" into the BusinessUnit field
	Then I should not see the OperatingCenter field 
	And I should not see the SupplierFacility field 
	When I select system delivery entry type "three" from the SystemDeliveryEntryType dropdown
	And I press "Save Entry Type"
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select "NJ7 - Shrewsbury" from the OperatingCenter dropdown
	And I press "Save Entry Type"
	Then I should see a validation message for SupplierFacility with "The SupplierFacility field is required."
	When I select "Facility 0 - NJ7-2" from the SupplierFacility dropdown
	And I select "Yes" from the IsEnabled dropdown
	And I enter "1.618" into the MinimumValue field
	And I enter "3.141" into the MaximumValue field
	And I press "Save Entry Type"
	And I wait for the page to reload
	Then I should see the following values in the system-delivery-entry-types-table table
         | System Delivery Entry Type          | Business Unit | Supplier Facility | Is Enabled | Minimum Value | Maximum Value |
         | system delivery entry type: "three" | 123456        | Facility 0 - NJ7-2 | Yes       | 1.618          | 3.141         |
	When I press "Add System Delivery Entry Type"
	And I select system delivery entry type "four" from the SystemDeliveryEntryType dropdown
	And I enter "123456" into the BusinessUnit field
	And I press "Save Entry Type"
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select "NJ7 - Shrewsbury" from the OperatingCenter dropdown
	And I press "Save Entry Type"
	Then I should see a validation message for SupplierFacility with "The SupplierFacility field is required."
	When I select "Facility 0 - NJ7-2" from the SupplierFacility dropdown
	And I select "Yes" from the IsEnabled dropdown
	And I enter "1.618" into the MinimumValue field
	And I enter "3.141" into the MaximumValue field
	And I press "Save Entry Type"
	And I wait for the page to reload
	Then I should see the following values in the system-delivery-entry-types-table table
         | System Delivery Entry Type          | Business Unit | Supplier Facility  | Is Enabled | Minimum Value | Maximum Value |
         | system delivery entry type: "three" | 123456        | Facility 0 - NJ7-2 | Yes        | 1.618         | 3.141         |
         | system delivery entry type: "four"  | 123456        | Facility 0 - NJ7-2 | Yes        | 1.618         | 3.141         |

Scenario: User should see required fields when adding entry type purchase point
	Given a facility "one" exists with facility id: "NJSB-01", system delivery type: "water", point of entry: "true"
	And an operating center "nj7" exists
	And a facility "two" exists with facility id: "NJSB-02", point of entry: "true"
	And a system delivery entry type "one" exists with description: "Purchase Point", system delivery type: "water"
	And a system delivery entry type "two" exists with description: "Delivered Water", system delivery type: "water"
	And a system delivery entry type "three" exists with description: "Transferred To", system delivery type: "water"
	And a system delivery entry type "four" exists with description: "Transferred From", system delivery type: "water"
	And a role "roleAddSysDel" exists with action: "Add", module: "ProductionSystemDeliveryConfiguration", user: "user"
	And a role "roleDeleteSysDel" exists with action: "Delete", module: "ProductionSystemDeliveryConfiguration", user: "user"
	And I am logged in as "user"
	When I visit the Show page for facility: "one"
	And I click the "System Delivery" tab
	And I press "Add System Delivery Entry Type" 
	And I select system delivery entry type "one" from the SystemDeliveryEntryType dropdown
	And I enter "123456" into the BusinessUnit field
	Then I should not see the OperatingCenter field 
	And I should not see the SupplierFacility field 
	And I should see the PurchaseSupplier field 
	When I press "Save Entry Type"
	Then I should see a validation message for PurchaseSupplier with "The Supplier field is required."
	When I enter "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes" into the PurchaseSupplier field
	And I select "Yes" from the IsEnabled dropdown
	And I enter "1.618" into the MinimumValue field
	And I enter "3.141" into the MaximumValue field
	And I press "Save Entry Type"
	And I wait for the page to reload
	Then I should see the following values in the system-delivery-entry-types-table table
         | System Delivery Entry Type        | Business Unit | Supplier Facility | Purchase Supplier | Is Enabled | Minimum Value | Maximum Value |
         | system delivery entry type: "one" | 123456        |                   | You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes  | Yes       | 1.618          | 3.141         |

Scenario: user with ProductionFacilityAreaManagement edit rights can add an area, cannot delete an area and the coordinates default to those of the facility
	Given a coordinate "one" exists with longitude: -40.25, latitude: 20.75 
	And a facility "two" exists with facility id: "NJSB-01", coordinate: "one"
	And a role "roleEdit2" exists with action: "Edit", module: "ProductionFacilityAreaManagement", user: "user"
	And I am logged in as "user"
	When I visit the Show page for facility: "two"
	And I click the "Areas" tab
	And I press "Add Area to Facility"
	And I select facility area "one" from the FacilityArea dropdown
	And I select facility sub area "one" from the FacilitySubArea dropdown
	And I press "Save Area"
	# Page reloads here
	And I click the "Areas" tab
	And I press "Add Area to Facility"
	Then I should see the following values in the areaFacilitiesTable table
         | Facility Area        | Facility Sub Area        | Coordinate              |
         | facility area: "one" | facility sub area: "one" | Coordinate20.75, -40.25 |
	And I should not see "Remove Area"

Scenario: user with ProductionFacilityAreaManagement edit rights can add and an area and set the coordinates
	Given a coordinate "one" exists with id: 99, longitude: -60.75, latitude: 30.25 
	And a facility "two" exists with facility id: "NJSB-01"
	And a role "roleEdit2" exists with action: "Edit", module: "ProductionFacilityAreaManagement", user: "user"
	And I am logged in as "user"
	When I visit the Show page for facility: "two"
	And I click the "Areas" tab
	And I press "Add Area to Facility"
	And I select facility area "one" from the FacilityArea dropdown
	And I select facility sub area "one" from the FacilitySubArea dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I press "Save Area"
	# Page reloads here
	And I click the "Areas" tab
	And I press "Add Area to Facility"
	Then I should see the following values in the areaFacilitiesTable table
         | Facility Area        | Facility Sub Area        | Coordinate              |
         | facility area: "one" | facility sub area: "one" | Coordinate30.25, -60.75 |

Scenario: user with ProductionFacilityAreaManagement delete rights can remove an area
	Given a coordinate "one" exists with id: 99, longitude: -60.75, latitude: 30.25 
	And a facility "two" exists with facility id: "NJSB-01"
	And a role "roleEdit2" exists with action: "Edit", module: "ProductionFacilityAreaManagement", user: "user"
	And a role "roleDelete3" exists with action: "Delete", module: "ProductionFacilityAreaManagement", user: "user"
	And I am logged in as "user"
	When I visit the Show page for facility: "two"
	And I click the "Areas" tab
	And I press "Add Area to Facility"
	And I select facility area "one" from the FacilityArea dropdown
	And I select facility sub area "one" from the FacilitySubArea dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
	And I press "Save Area"
	# Page reloads here
	And I click the "Areas" tab
	And I press "Add Area to Facility"
	Then I should see the following values in the areaFacilitiesTable table
         | Facility Area        | Facility Sub Area        | Coordinate              |
         | facility area: "one" | facility sub area: "one" | Coordinate30.25, -60.75 |
	And I should see "Remove Area"

Scenario: user with ProductionFacilityAreaManagement delete rights can remove an area only when the area doesn't have an "In Service" equipment 
	Given a coordinate "one" exists with longitude: -40.25, latitude: 20.75 
	And a facility "two" exists with facility id: "NJSB-01", coordinate: "one"
	And a role "roleDelete3" exists with action: "Delete", module: "ProductionFacilityAreaManagement", user: "user"
	And equipment statuses exist
	And a facility facility area "one" exists with facility: "two", facilityArea: "one", facilitySubArea: "one"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "two", equipment status: "in service", facility facility area: "one"
	And I am logged in as "user"
	When I visit the Show page for facility: "two"
	And I click the "Areas" tab
	Then I should not see "Remove Area" in the areaFacilitiesTable element
	And I should see "Cannot delete while the area has an equipment with status of 'In Service'." in the areaFacilitiesTable element

Scenario: User can view system delivery history
	Given a facility "one" exists with facility id: "NJSB-01"
	And a facility "two" exists with facility id: "NJSB-02"
	And a system delivery entry type "one" exists with description: "Purchase Point", system delivery type: "water"
	And a system delivery entry type "two" exists with description: "Delivered Water", system delivery type: "water"
	And a system delivery entry "one" exists with WeekOf: "30 days ago", IsValidated: "true"
	And a system delivery entry "two" exists	
	And a system delivery facility entry "monday1" exists with system delivery entry: "one", system delivery entry type: "one", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday1" exists with system delivery entry: "one", system delivery entry type: "one", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday1" exists with system delivery entry: "one", system delivery entry type: "one", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday1" exists with system delivery entry: "one", system delivery entry type: "one", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday1" exists with system delivery entry: "one", system delivery entry type: "one", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday1" exists with system delivery entry: "one", system delivery entry type: "one", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday1" exists with system delivery entry: "one", system delivery entry type: "one", facility: "one", entry date: 7/17/2022, entry value: 30.121
	And a system delivery facility entry "monday2" exists with system delivery entry: "one", system delivery entry type: "two", facility: "one", entry date: 7/11/2022, entry value: 3.251
	And a system delivery facility entry "tuesday2" exists with system delivery entry: "one", system delivery entry type: "two", facility: "one", entry date: 7/12/2022, entry value: 6.661
	And a system delivery facility entry "wednesday2" exists with system delivery entry: "one", system delivery entry type: "two", facility: "one", entry date: 7/13/2022, entry value: 8.212
	And a system delivery facility entry "thursday2" exists with system delivery entry: "one", system delivery entry type: "two", facility: "one", entry date: 7/14/2022, entry value: 2.122
	And a system delivery facility entry "friday2" exists with system delivery entry: "one", system delivery entry type: "two", facility: "one", entry date: 7/15/2022, entry value: 12.116
	And a system delivery facility entry "saturday2" exists with system delivery entry: "one", system delivery entry type: "two", facility: "one", entry date: 7/16/2022, entry value: 4.726
	And a system delivery facility entry "sunday2" exists with system delivery entry: "one", system delivery entry type: "two", facility: "one", entry date: 7/17/2022, entry value: 30.121
	And I am logged in as "user"
	When I visit the Show page for facility: "one"
	And I click the "System Delivery History" tab 
	Then I should see "No historical data prior to April of 2021 is stored in MapCall."
	And I should see the following values in the system-delivery-history-table table
		 | Entry Type                        | Week Of     | Value (Thousand of Gallons) |
		 | system delivery entry type: "two" | 30 days ago | 67.209                      |
		 | system delivery entry type: "one" | 30 days ago | 67.209                      |
	When I visit the Show page for facility: "two"
	And I click the "System Delivery History" tab 
	Then I should see "No historical data prior to April of 2021 is stored in MapCall."
	And I should see "No Historical Data Found."
	
Scenario: user can view equipment list
    Given a user "readonly" exists with username: "readonly", roles: roleRead
	And a facility "one" exists with facility id: "NJSB-01"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one"
	And I am logged in as "readonly"
    When I visit the Show page for facility: "one"
    And I click the "Equipment" tab
	Then I should see a link to the Show page for equipment: "one"

Scenario: equipment list retired equipment colored
    Given a user "readonly" exists with username: "readonly", roles: roleRead
	And a facility "one" exists with facility id: "NJSB-01"
	And an equipment status "one" exists with description: "Retired"
	And an equipment status "two" exists with description: "Cancelled"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", equipment status: "one"
	And I am logged in as "readonly"
    When I visit the Show page for facility: "one"
    And I click the "Equipment" tab
	Then the td elements in the 1st row of the "equipmentTable" table should have a "background-color" value of "#ffbdbd"

Scenario: equipment list sorted by equipment status
    Given a user "readonly" exists with username: "readonly", roles: roleRead
	And a facility "one" exists with facility id: "NJSB-01"
	And an equipment status "one" exists with description: "Retired"
	And an equipment status "two" exists with description: "Cancelled"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one", equipment status: "one", description: "Equipment one"
	And an equipment "two" exists with identifier: "NJSB-1-EQID-2", facility: "one", equipment status: "two", description: "Equipment two"
	And I am logged in as "readonly"
    When I visit the Show page for facility: "one"
    And I click the "Equipment" tab
	Then I should see the following values in the equipmentTable table
		 | Description   | EquipmentStatus		   |
		 | Equipment two | equipment status: "two" |
		 | Equipment one | equipment status: "one" |

Scenario: user can view maintenance plans associated with a facility
    Given a user "readonly" exists with username: "readonly", roles: roleRead
	And a facility "one" exists with facility id: "NJSB-01"
	And an operating center "nj7" exists with opcode: "NJ7"
	And a planning plant "one" exists with operating center: "nj7", code: "D217", description: "Argh"
	And equipment types exist
    And equipment statuses exist
	And an equipment category "one" exists with description: "Cat"
	And an equipment subcategory "one" exists with description: "SubCat"
	And an equipment purpose "aerator" exists with description: "aerator", equipment type: "rtu", equipment category: "one", equipment subcategory: "one"
	And a maintenance plan task type "one" exists with description: "A good plan"
	And a task group category "one" exists with description: "This is the description", type: "chemical", abbreviation: "CHEM", is active: "true"
	And a skill set "one" exists with name: "Skill name", abbreviation: "SkAbr", is active: "true", description: "This is the description"    
	And a task group "one" exists with task group id: "Task Id 1", task group name: "This is group 1", task details: "Task details 1", task details summary: "Task summary 1", task group category: "one", resources: "1", estimated hours: "2", contractor cost: "3", equipment types: "rtu", task group categories: "one", skill set: "one", maintenance plan task type: "one"
	And production work order frequencies exist
	And a production work order priority "one" exists with description: "Routine - Off Scheduled"
	And a maintenance plan "one" exists with start: "02/24/2020", operating center: "nj7", planning plant: "one", facility: "one", equipment types: "rtu", equipment purposes: "aerator", task group: "one", production work order frequency: "daily", equipment: "one", production work order priority: "one", is active: "true"
	And I am logged in as "readonly"
    When I visit the Show page for facility: "one"
    And I click the "Maintenance Plans" tab
	Then I should see a link to the Show page for maintenance plan "one"
	When I follow the Show link for maintenance plan "one"
	Then I should be at the Show page for maintenance plan "one"
	
Scenario: user can set a facility's chemical feed property by adding a chemical storage
	Given I am logged in as "admin"
	And a chemical "one" exists with part number: "1111", sds hyperlink: "http://chemicals-are-cool.com"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a chemical warehouse number "one" exists with operating center: "nj7"
	And a town "one" exists
	And a facility "one" exists with operating center: "nj7", town: "one"
	When I visit the Environmental/ChemicalStorage/New page
	And I enter "foo" into the DeliveryInstructions field
	And I select chemical "one" from the Chemical dropdown
	And I enter "container" into the ContainerType field
	And I enter "maximum" into the MaximumDailyInventory field
	And I enter "average" into the AverageDailyInventory field
	And I enter "817" into the DaysOnSite field
	And I enter "storage pressure" into the StoragePressure field
	And I enter "storage temp" into the StorageTemperature field
	And I select "NJ" from the State dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown  
	And I select facility "one" from the Facility dropdown  
	And I select chemical warehouse number "one" from the WarehouseNumber dropdown   
	And I press Save  
	Then the currently shown chemical storage will now be referred to as "new" 
	When I visit the Facility/Search page
	And I enter "1" into the EntityId field
	And I press Search
	Then I should see a link to the Show page for facility "one"
	When I follow the Show link for facility "one"
	Then I should be at the Show page for facility "one"
	And I should see a display for ChemicalFeed with "Yes"
	
Scenario: user can set a facility's chemical feed property to true manually without creating a chemical storage record
	Given I am logged in as "admin"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a town "one" exists with state: "nj"
	And a facility "one" exists with operating center: "nj7", town: "one"
	When I visit the Facility/Search page
	And I enter "1" into the EntityId field
	And I press Search
	Then I should see a link to the Show page for facility "one"
	When I follow the Show link for facility "one"
	Then I should be at the Show page for facility "one"
	And I should see a display for ChemicalFeed with "No"
	When I visit the Edit page for facility: "one"
	And I wait for ajax to finish loading
	Then I should be at the Edit page for facility "one"
	And I should not see the ChemicalStorageLocation field
	When I check the ChemicalFeed field
	And I press Save
	Then I should see a validation message for ChemicalStorageLocation with "The ChemicalStorageLocation field is required."
	When I select "Cubs Win" from the ChemicalStorageLocation dropdown
	And I press Save
	Then I should be at the Show page for facility "one"
	And I should see a display for ChemicalFeed with "Yes"

Scenario: user can edit Facility Maintenance Condition and Performance fields on Facility Records in risk characteristics tab with facility area management role
	Given a facility "one" exists with operating center: "opc", town: "lazytown", facility id: "NJSB-01", public water supply: "pws-01"
	And a role "roleEdit2" exists with action: "Edit", module: "ProductionFacilityAreaManagement", user: "user"
	And I am logged in as "user"
	When I visit the Edit page for facility: "one"
	And I click the "Risk Characteristics" tab
	And I select "Good" from the Condition dropdown
	And I select "Good" from the Performance dropdown
	And I press Save
	And I wait for the page to reload
	When I click the "Risk Characteristics" tab
	Then I should see a display for Condition with "Good"
	Then I should see a display for Performance with "Good"

Scenario: user cannot edit Facility Maintenance Condition and Performance fields without Facility Area Management role
	Given a facility "one" exists with operating center: "opc", town: "lazytown", facility id: "NJSB-01", public water supply: "pws-01", condition: "good", performance: "good"
	And I am logged in as "other"
	When I visit the Edit page for facility: "one"
	And I click the "Risk Characteristics" tab
	And I enter "3/17/2020" into the RiskBasedCompletedDate field
	Then I should not see the Condition element
	Then I should not see the Performance element
	When I press Save
	And I wait for the page to reload
	And I click the "Risk Characteristics" tab
	Then I should see a display for Condition with "Good"
	And I should see a display for Performance with "Good"
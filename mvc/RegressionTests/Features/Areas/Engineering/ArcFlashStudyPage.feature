Feature: ArcFlashStudy
	
Background: users and supporting data exist
	Given a user "user" exists with username: "user"
	And an admin user "admin" exists with username: "admin"
	And a role "roleRead" exists with action: "Read", module: "ProductionFacilities", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "ProductionFacilities", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "ProductionFacilities", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "ProductionFacilities", user: "user"
	And a role "arcRoleRead" exists with action: "Read", module: "EngineeringArcFlash", user: "user"
	And a role "arcRoleEdit" exists with action: "Edit", module: "EngineeringArcFlash", user: "user"
	And a role "arcRoleAdd" exists with action: "Add", module: "EngineeringArcFlash", user: "user"
	And a role "arcRoleDelete" exists with action: "Delete", module: "EngineeringArcFlash", user: "user"
	And a state "nj" exists
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true", state: "nj"
	And a town "one" exists with state: "nj"
	And operating center: "nj4" exists in town: "one"
    And a town section "active" exists with town: "one"
    And a town section "inactive" exists with town: "one", name: "Inactive Section", active: false
	And a role "image" exists with action: "Read", module: "FieldServicesImages", user: "user"
	And a department "production" exists with description: "Production"
	And an asset type "facility" exists with description: "facility"
	And a functional location "one" exists with description: "NJRB-UN-FAC-0003", asset type: "facility", town: "one"
	And an arc flash analysis type "one" exists with description: "one"
	And an arc flash label type "Standard Label" exists with description: "Standard Label"
	And an arc flash label type "Custom Label" exists with description: "Custom Label"
	And a utility transformer k v a rating "one" exists with description: "Neat"
	And a voltage "one" exists with description: "one"
	And voltage: "one" exists in utility transformer k v a rating: "one"
	And a power phase "one" exists with description: "one"
	And a facility "one" exists with facility id: "NJSB-01", entity notes: "this is an entity note", operating center: "nj4"
	And an arc flash status "one" exists with description: "Completed"
	And a facility size "one" exists with description: "Medium"
	And a facility transformer wiring type "one" exists with description: "one"
	And a utility company "one" exists with description: "PSEG", state: "nj"
	
Scenario: User can search and view arc flash studies
	Given an arc flash study "one" exists with facility: "one"
	And I am logged in as "user"
	When I visit the Engineering/ArcFlashStudy/Search page
	And I press Search
	And I wait for the page to reload
	And I follow the Show link for arc flash study "one"
	Then I should be at the Show page for arc flash study "one"

Scenario: User can create an arc flash study and gets the proper validation
	Given I am logged in as "user"
	And a role "roleFacilityReadNj4" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And an arc flash study "one" exists with facility: "one"
	When I visit the Engineering/ArcFlashStudy/New page
	And I press Save
	Then I should see a validation message for State with "The State field is required."
	When I select state "nj" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj4" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for Facility with "The Facility field is required."
	When I select facility "one"'s Description from the Facility dropdown
	And I press Save
	Then I should see a validation message for PowerCompanyDataReceived with "The Utility Company Data Received field is required."
	When I select "Yes" from the PowerCompanyDataReceived dropdown
	And I press Save
	Then I should see a validation message for UtilityCompanyDataReceivedDate with "The UtilityCompanyDataReceivedDate field is required."
	When I select "No" from the PowerCompanyDataReceived dropdown
	And I press Save
	Then I should see a validation message for Voltage with "The Secondary (Incoming Service) Voltage (V) field is required."
	And I should see a validation message for PowerPhase with "The Phase field is required."
	When I select voltage "one" from the Voltage dropdown
	And I select power phase "one" from the PowerPhase dropdown
	And I press Save
	Then I should see a validation message for TransformerKVARating with "The TransformerKVARating field is required."
	When I select utility transformer k v a rating "one" from the TransformerKVARating dropdown
	And I select arc flash status "one" from the ArcFlashStatus dropdown
	And I press Save
	Then I should see a validation message for TransformerKVAFieldConfirmed with "The Utility Transformer KVA Field Confirmed? field is required."
	Then I should see a validation message for ArcFlashContractor with "The Arc Flash Site Data Collection Party field is required."
	Then I should see a validation message for CostToComplete with "The Cost to Complete the Study field is required."
	When I select "Yes" from the TransformerKVAFieldConfirmed dropdown
	And I enter "collection party" into the ArcFlashContractor field
	And I enter "foo party" into the ArcFlashHazardAnalysisStudyParty field
	And I enter "808" into the CostToComplete field
	And I enter "08/17/2019" into the UtilityCompanyDataReceivedDate field
	And I enter "1.5" into the Priority field
	And I select facility size "one" from the FacilitySize dropdown
	And I enter "8/18/2018" into the DateLabelsApplied field
	And I enter "arc flash notes" into the ArcFlashNotes field
	And I enter "7.1" into the LineToLineNeutralFaultAmps field
	And I enter "1.1" into the LineToLineFaultAmps field
	And I enter "primary fuse manufacturer" into the PrimaryFuseManufacturer field
	And I enter "primary fuse type" into the PrimaryFuseType field
	And I enter "1.2" into the PrimaryFuseSize field
	And I select facility transformer wiring type "one" from the FacilityTransformerWiringType dropdown
	And I enter "50" into the TransformerReactancePercentage field
	And I enter "10" into the PrimaryVoltageKV field
	And I enter "other" into the UtilityCompanyOther field
	And I enter "uan" into the UtilityAccountNumber field
	And I enter "umn" into the UtilityMeterNumber field
	And I enter "upn" into the UtilityPoleNumber field
	And I select utility company "one" from the UtilityCompany dropdown
	And I select arc flash analysis type "one" from the TypeOfArcFlashAnalysis dropdown
	And I select arc flash label type "Custom Label" from the ArcFlashLabelType dropdown
	And I select "Yes" from the AFHAAnalysisPerformed dropdown
	And I enter "42" into the TransformerResistancePercentage field
	And I press Save
	And I wait for the page to reload
	Then the currently shown arc flash study shall henceforth be known throughout the land as "Tim"
	And I should be at the Show page for arc flash study "Tim"
	And I should see a link to the Show page for facility: "one"
	And I should see a display for ArcFlashStatus with arc flash status "one"
	And I should see a display for Priority with "1.5"
	And I should see a display for FacilitySize with facility size "one"
	And I should see a display for PowerCompanyDataReceived with "No"
	And I should see a display for UtilityCompanyDataReceivedDate with "8/17/2019"
	And I should see a display for AFHAAnalysisPerformed with "Yes"
	And I should see a display for TypeOfArcFlashAnalysis with arc flash analysis type "one"
	And I should see a display for ArcFlashLabelType with arc flash label type "Custom Label"
	And I should see a display for UtilityCompany with utility company "one"
	And I should see a display for UtilityCompanyOther with "other"
	And I should see a display for UtilityAccountNumber with "uan"
	And I should see a display for UtilityMeterNumber with "umn"
	And I should see a display for UtilityPoleNumber with "upn"
	And I should see a display for PrimaryVoltageKV with "10"
	And I should see a display for Voltage with voltage "one"
	And I should see a display for PowerPhase with power phase "one"
	And I should see a display for TransformerKVARating with utility transformer k v a rating "one"
	And I should see a display for TransformerKVAFieldConfirmed with "Yes"
	And I should see a display for TransformerReactancePercentage with "50"
	And I should see a display for FacilityTransformerWiringType with facility transformer wiring type "one"
	And I should see a display for PrimaryFuseSize with "1.2"
	And I should see a display for PrimaryFuseType with "primary fuse type"
	And I should see a display for PrimaryFuseManufacturer with "primary fuse manufacturer"
	And I should see a display for LineToLineFaultAmps with "1.1"
	And I should see a display for LineToLineNeutralFaultAmps with "7.1"
	And I should see a display for ArcFlashNotes with "arc flash notes"
	And I should see a display for DateLabelsApplied with "8/18/2018"
	And I should see a display for ArcFlashContractor with "collection party"
	And I should see a display for ArcFlashHazardAnalysisStudyParty with "foo party"
	And I should see a display for CostToComplete with "$808.00"
	And I should see a display for TransformerResistancePercentage with "42"

Scenario: User can edit arc flash study
	Given an arc flash study "one" exists with facility: "one"
	And I am logged in as "user"
	When I visit the Show page for arc flash study "one"
	And I follow "Edit"
	Then I should be at the Edit page for arc flash study "one"
	When I press Save
	Then I should see a validation message for Voltage with "The Secondary (Incoming Service) Voltage (V) field is required."
	And I should see a validation message for PowerPhase with "The Phase field is required."
	When I select voltage "one" from the Voltage dropdown
	And I select power phase "one" from the PowerPhase dropdown
	And I press Save
	Then I should see a validation message for TransformerKVARating with "The TransformerKVARating field is required."
	When I select utility transformer k v a rating "one" from the TransformerKVARating dropdown
	And I select arc flash status "one" from the ArcFlashStatus dropdown
	And I press Save
	Then I should see a validation message for ArcFlashContractor with "The Arc Flash Site Data Collection Party field is required."
	And I should see a validation message for CostToComplete with "The Cost to Complete the Study field is required."
	When I select "Yes" from the TransformerKVAFieldConfirmed dropdown
	And I enter "collection party" into the ArcFlashContractor field
	And I enter "foo party" into the ArcFlashHazardAnalysisStudyParty field
	And I enter "808" into the CostToComplete field
	And I enter "08/17/2019" into the UtilityCompanyDataReceivedDate field
	And I enter "1.5" into the Priority field
	And I select facility size "one" from the FacilitySize dropdown
	And I enter "8/18/2018" into the DateLabelsApplied field
	And I enter "arc flash notes" into the ArcFlashNotes field
	And I enter "7.1" into the LineToLineNeutralFaultAmps field
	And I enter "1.1" into the LineToLineFaultAmps field
	And I enter "primary fuse manufacturer" into the PrimaryFuseManufacturer field
	And I enter "primary fuse type" into the PrimaryFuseType field
	And I enter "1.2" into the PrimaryFuseSize field
	And I select facility transformer wiring type "one" from the FacilityTransformerWiringType dropdown
	And I enter "50" into the TransformerReactancePercentage field
	And I enter "10" into the PrimaryVoltageKV field
	And I enter "other" into the UtilityCompanyOther field
	And I enter "uan" into the UtilityAccountNumber field
	And I enter "umn" into the UtilityMeterNumber field
	And I enter "upn" into the UtilityPoleNumber field
	And I select utility company "one" from the UtilityCompany dropdown
	And I select arc flash analysis type "one" from the TypeOfArcFlashAnalysis dropdown
	And I select arc flash label type "Custom Label" from the ArcFlashLabelType dropdown
	And I select "Yes" from the AFHAAnalysisPerformed dropdown
	And I enter "42" into the TransformerResistancePercentage field
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for arc flash study "one"
	And I should see a link to the Show page for facility: "one"
	And I should see a display for ArcFlashStatus with arc flash status "one"
	And I should see a display for Priority with "1.5"
	And I should see a display for FacilitySize with facility size "one"
	And I should see a display for PowerCompanyDataReceived with "No"
	And I should see a display for UtilityCompanyDataReceivedDate with "8/17/2019"
	And I should see a display for AFHAAnalysisPerformed with "Yes"
	And I should see a display for TypeOfArcFlashAnalysis with arc flash analysis type "one"
	And I should see a display for ArcFlashLabelType with arc flash label type "Custom Label"
	And I should see a display for UtilityCompany with utility company "one"
	And I should see a display for UtilityCompanyOther with "other"
	And I should see a display for UtilityAccountNumber with "uan"
	And I should see a display for UtilityMeterNumber with "umn"
	And I should see a display for UtilityPoleNumber with "upn"
	And I should see a display for PrimaryVoltageKV with "10"
	And I should see a display for Voltage with voltage "one"
	And I should see a display for PowerPhase with power phase "one"
	And I should see a display for TransformerKVARating with utility transformer k v a rating "one"
	And I should see a display for TransformerKVAFieldConfirmed with "Yes"
	And I should see a display for TransformerReactancePercentage with "50"
	And I should see a display for FacilityTransformerWiringType with facility transformer wiring type "one"
	And I should see a display for PrimaryFuseSize with "1.2"
	And I should see a display for PrimaryFuseType with "primary fuse type"
	And I should see a display for PrimaryFuseManufacturer with "primary fuse manufacturer"
	And I should see a display for LineToLineFaultAmps with "1.1"
	And I should see a display for LineToLineNeutralFaultAmps with "7.1"
	And I should see a display for ArcFlashNotes with "arc flash notes"
	And I should see a display for DateLabelsApplied with "8/18/2018"
	And I should see a display for ArcFlashContractor with "collection party"
	And I should see a display for ArcFlashHazardAnalysisStudyParty with "foo party"
	And I should see a display for CostToComplete with "$808.00"
	And I should see a display for TransformerResistancePercentage with "42"

Scenario: User can delete an arc flash study
	Given an arc flash study "one" exists with facility: "one"
	And I am logged in as "user"
	When I visit the Show page for arc flash study "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Engineering/ArcFlashStudy/Search page
	When I try to access the Show page for arc flash study: "one" expecting an error
	Then I should see a 404 error message

Scenario: User can create an arc flash study and gets the proper validation on selecting standard label arc flash label type
	Given I am logged in as "user"
	And a role "roleFacilityReadNj4" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And an arc flash study "one" exists with facility: "one"
	When I visit the Engineering/ArcFlashStudy/New page
	And I press Save
	Then I should see a validation message for State with "The State field is required."
	Then I should see a validation message for PowerCompanyDataReceived with "The Utility Company Data Received field is required."
	Then I should see a validation message for Voltage with "The Secondary (Incoming Service) Voltage (V) field is required."
	Then I should see a validation message for PowerPhase with "The Phase field is required."
	When I select arc flash label type "Standard Label" from the ArcFlashLabelType dropdown
	Then I should see a validation message for State with "The State field is required."
	When I select state "nj" from the State dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	When I select operating center "nj4" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for Facility with "The Facility field is required."
	When I select facility "one"'s Description from the Facility dropdown
	And I press Save
	And I wait for the page to reload
	Then the currently shown arc flash study shall henceforth be known throughout the land as "Tim"
	And I should be at the Show page for arc flash study "Tim"
	And I should see a link to the Show page for facility: "one"
	And I should see a display for ArcFlashLabelType with arc flash label type "Standard Label"

Scenario: User can edit arc flash study and select standard label arc flash label type
	Given an arc flash study "one" exists with facility: "one", arc flash status: "one"
	And I am logged in as "user"
	When I visit the Show page for arc flash study "one"
	And I follow "Edit"
	Then I should be at the Edit page for arc flash study "one"
	When I press Save
	Then I should see a validation message for Voltage with "The Secondary (Incoming Service) Voltage (V) field is required."
	And I should see a validation message for PowerPhase with "The Phase field is required."
	And I should see a validation message for ArcFlashContractor with "The Arc Flash Site Data Collection Party field is required."
	And I should see a validation message for CostToComplete with "The Cost to Complete the Study field is required."
	When I select arc flash label type "Standard Label" from the ArcFlashLabelType dropdown
	And I enter "other" into the ArcFlashContractor field
	And I enter "10" into the CostToComplete field
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for arc flash study "one"
	And I should see a link to the Show page for facility: "one"
	And I should see a display for ArcFlashLabelType with arc flash label type "Standard Label"

Scenario: User can see arc flash completion report data
	Given arc flash statuses exist
	And I am logged in as "admin"
	And an arc flash study "one" exists with facility: "one", arc flash status: "completed"
	And an arc flash study "two" exists with facility: "one", arc flash status: "completed"
	And an arc flash study "three" exists with facility: "one", arc flash status: "completed"
	And an arc flash study "four" exists with facility: "one", arc flash status: "completed"
	And an arc flash study "five" exists with facility: "one", arc flash status: "pending"
	And an arc flash study "six" exists with facility: "one", arc flash status: "deferred"
	And an arc flash study "seven" exists with facility: "one", arc flash status: "completed"
	And an arc flash study "eight" exists with facility: "one", arc flash status: "deferred"
	And I am at the Engineering/ArcFlashCompletion/ page
	When I wait for the page to reload
	Then I should see the following values in the results table
         | Total Arc Flash Facilities | Number Completed | Number Pending | Number Deferred | Total Completed Not Deferred |
         | 0                          | 0                | 0              | 0               | 0                            |
         | 8                          | 5                | 1              | 2               | 0                            |
         | 0                          | 0                | 0              | 0               | 0                            |

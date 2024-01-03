Feature: Water Sample Compliance Form page

Background: users and supporting data exists
	Given a user "user" exists with username: "user", full name: "Mister User"
	And a role "roleRead" exists with action: "Read", module: "EnvironmentalGeneral", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "EnvironmentalGeneral", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "EnvironmentalGeneral", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "EnvironmentalGeneral", user: "user"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "nj3" exists with opcode: "NJ3", name: "Fire Road", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a state "nj" exists with abbreviation: "NJ"
	And public water supply statuses exist
	And a public water supply "one" exists with operating area: "AA", identifier: "1111", status: "active", aw owned: "true", state: "nj"
	And a not available water sample compliance form answer type "notavailable" exists
	And a no water sample compliance form answer type "no" exists
	And a yes water sample compliance form answer type "yes" exists

Scenario: Creating a new record displays default values on create screen
	Given I am logged in as "user"
	And I am at the Environmental/WaterSampleComplianceForm/New page for public water supply: "one"
	Then I should see a display for PublicWaterSupplyDisplay with public water supply "one"'s ToString
	And I should see a display for PublicWaterSupplyDisplay_State with "NJ"
	# And I should see display for the certified month/year but that's not possible in regression testing

Scenario: User can create a new record 
	Given I am logged in as "user"
	And I am at the Environmental/WaterSampleComplianceForm/New page for public water supply: "one"
	When I select "Yes" from the CentralLabSamplesHaveBeenCollected dropdown
	And I select "No" from the ContractedLabsSamplesHaveBeenCollected dropdown
	And I enter "TestReason" into the ContractedLabsSamplesReason field
	And I select "Yes" from the InternalLabsSamplesHaveBeenCollected dropdown
	And I select "No" from the BactiSamplesHaveBeenCollected dropdown
	And I enter "AnotherTestReason" into the BactiSamplesReason field
	And I select "Yes" from the LeadAndCopperSamplesHaveBeenCollected dropdown
	And I select "No" from the WQPSamplesHaveBeenCollected dropdown
	And I enter "YetAnotherTestReason" into the WQPSamplesReason field
	And I select "Yes" from the SurfaceWaterPlantSamplesHaveBeenCollected dropdown
	And I select "No" from the ChlorineResidualsHaveBeenCollected dropdown
	And I enter "Hopefully the last reason" into the ChlorineResidualsReason field
	And I enter "Here's some notes" into the NoteText field
	And I press "Save" 
	Then I should see a display for CentralLabSamplesHaveBeenCollected with "Yes"
	And I should see a display for ContractedLabsSamplesHaveBeenCollected with "No"
	And I should see a display for ContractedLabsSamplesReason with "TestReason"
	And I should see a display for InternalLabsSamplesHaveBeenCollected with "Yes"
	And I should see a display for BactiSamplesHaveBeenCollected with "No"
	And I should see a display for BactiSamplesReason with "AnotherTestReason"
	And I should see a display for LeadAndCopperSamplesHaveBeenCollected with "Yes"
	And I should see a display for WQPSamplesHaveBeenCollected with "No"
	And I should see a display for WQPSamplesReason with "YetAnotherTestReason"
	And I should see a display for SurfaceWaterPlantSamplesHaveBeenCollected with "Yes"
	And I should see a display for ChlorineResidualsHaveBeenCollected with "No"
	And I should see a display for ChlorineResidualsReason with "Hopefully the last reason"
	And I should see a display for NoteText with "Here's some notes"

Scenario: User can not create a new record when the public water supply is not AW Owned 
	Given a public water supply "two" exists with operating area: "AA", identifier: "2222", status: "active", aw owned: "false", state: "nj"
	And I am logged in as "user"
	And I am at the Environmental/WaterSampleComplianceForm/New page for public water supply: "two"
	When I press "Save"
	Then I should see the error message "Compliance forms can only be entered for American Water owned public water supplies."

Scenario: User should only see notes fields when their respective answer fields are set to "No"
	Given I am logged in as "user"
	And I am at the Environmental/WaterSampleComplianceForm/New page for public water supply: "one"
	When I select "-- Select --" from the CentralLabSamplesHaveBeenCollected dropdown
	Then I should not see the CentralLabSamplesReason field
	When I select "N/A" from the CentralLabSamplesHaveBeenCollected dropdown
	Then I should not see the CentralLabSamplesReason field
	When I select "Yes" from the CentralLabSamplesHaveBeenCollected dropdown
	Then I should not see the CentralLabSamplesReason field
	When I select "No" from the CentralLabSamplesHaveBeenCollected dropdown
	Then I should see the CentralLabSamplesReason field

	When I select "Yes" from the CentralLabSamplesHaveBeenCollected dropdown
	And I select "-- Select --" from the CentralLabSamplesHaveBeenReported dropdown
	Then I should not see the CentralLabSamplesReason field
	When I select "N/A" from the CentralLabSamplesHaveBeenReported dropdown
	Then I should not see the CentralLabSamplesReason field
	When I select "Yes" from the CentralLabSamplesHaveBeenReported dropdown
	Then I should not see the CentralLabSamplesReason field
	When I select "No" from the CentralLabSamplesHaveBeenReported dropdown
	Then I should see the CentralLabSamplesReason field

	When I select "-- Select --" from the ContractedLabsSamplesHaveBeenCollected dropdown
	Then I should not see the ContractedLabsSamplesReason field
	When I select "N/A" from the ContractedLabsSamplesHaveBeenCollected dropdown
	Then I should not see the ContractedLabsSamplesReason field
	When I select "Yes" from the ContractedLabsSamplesHaveBeenCollected dropdown
	Then I should not see the ContractedLabsSamplesReason field
	When I select "No" from the ContractedLabsSamplesHaveBeenCollected dropdown
	Then I should see the ContractedLabsSamplesReason field

	When I select "Yes" from the ContractedLabsSamplesHaveBeenCollected dropdown
	And I select "-- Select --" from the ContractedLabsSamplesHaveBeenReported dropdown
	Then I should not see the ContractedLabsSamplesReason field
	When I select "N/A" from the ContractedLabsSamplesHaveBeenReported dropdown
	Then I should not see the ContractedLabsSamplesReason field
	When I select "Yes" from the ContractedLabsSamplesHaveBeenReported dropdown
	Then I should not see the ContractedLabsSamplesReason field
	When I select "No" from the ContractedLabsSamplesHaveBeenReported dropdown
	Then I should see the ContractedLabsSamplesReason field

	When I select "-- Select --" from the InternalLabsSamplesHaveBeenCollected dropdown
	Then I should not see the InternalLabSamplesReason field
	When I select "N/A" from the InternalLabsSamplesHaveBeenCollected dropdown
	Then I should not see the InternalLabSamplesReason field
	When I select "Yes" from the InternalLabsSamplesHaveBeenCollected dropdown
	Then I should not see the InternalLabSamplesReason field
	When I select "No" from the InternalLabsSamplesHaveBeenCollected dropdown
	Then I should see the InternalLabSamplesReason field

	When I select "Yes" from the InternalLabsSamplesHaveBeenCollected dropdown
	And I select "-- Select --" from the InternalLabsSamplesHaveBeenReported dropdown
	Then I should not see the InternalLabSamplesReason field
	When I select "N/A" from the InternalLabsSamplesHaveBeenReported dropdown
	Then I should not see the InternalLabSamplesReason field
	When I select "Yes" from the InternalLabsSamplesHaveBeenReported dropdown
	Then I should not see the InternalLabSamplesReason field
	When I select "No" from the InternalLabsSamplesHaveBeenReported dropdown
	Then I should see the InternalLabSamplesReason field


	When I select "-- Select --" from the BactiSamplesHaveBeenCollected dropdown
	Then I should not see the BactiSamplesReason field
	When I select "N/A" from the BactiSamplesHaveBeenCollected dropdown
	Then I should not see the BactiSamplesReason field
	When I select "Yes" from the BactiSamplesHaveBeenCollected dropdown
	Then I should not see the BactiSamplesReason field
	When I select "No" from the BactiSamplesHaveBeenCollected dropdown
	Then I should see the BactiSamplesReason field

	When I select "Yes" from the BactiSamplesHaveBeenCollected dropdown
	And I select "-- Select --" from the BactiSamplesHaveBeenReported dropdown
	Then I should not see the BactiSamplesReason field
	When I select "N/A" from the BactiSamplesHaveBeenReported dropdown
	Then I should not see the BactiSamplesReason field
	When I select "Yes" from the BactiSamplesHaveBeenReported dropdown
	Then I should not see the BactiSamplesReason field
	When I select "No" from the BactiSamplesHaveBeenReported dropdown
	Then I should see the BactiSamplesReason field

	When I select "-- Select --" from the LeadAndCopperSamplesHaveBeenCollected dropdown
	Then I should not see the LeadAndCopperSamplesReason field
	When I select "N/A" from the LeadAndCopperSamplesHaveBeenCollected dropdown
	Then I should not see the LeadAndCopperSamplesReason field
	When I select "Yes" from the LeadAndCopperSamplesHaveBeenCollected dropdown
	Then I should not see the LeadAndCopperSamplesReason field
	When I select "No" from the LeadAndCopperSamplesHaveBeenCollected dropdown
	Then I should see the LeadAndCopperSamplesReason field

	When I select "Yes" from the LeadAndCopperSamplesHaveBeenCollected dropdown
	And I select "-- Select --" from the LeadAndCopperSamplesHaveBeenReported dropdown
	Then I should not see the LeadAndCopperSamplesReason field
	When I select "N/A" from the LeadAndCopperSamplesHaveBeenReported dropdown
	Then I should not see the LeadAndCopperSamplesReason field
	When I select "Yes" from the LeadAndCopperSamplesHaveBeenReported dropdown
	Then I should not see the LeadAndCopperSamplesReason field
	When I select "No" from the LeadAndCopperSamplesHaveBeenReported dropdown
	Then I should see the LeadAndCopperSamplesReason field

	When I select "-- Select --" from the WQPSamplesHaveBeenCollected dropdown
	Then I should not see the WQPSamplesReason field
	When I select "N/A" from the WQPSamplesHaveBeenCollected dropdown
	Then I should not see the WQPSamplesReason field
	When I select "Yes" from the WQPSamplesHaveBeenCollected dropdown
	Then I should not see the WQPSamplesReason field
	When I select "No" from the WQPSamplesHaveBeenCollected dropdown
	Then I should see the WQPSamplesReason field

	When I select "Yes" from the WQPSamplesHaveBeenCollected dropdown
	And I select "-- Select --" from the WQPSamplesHaveBeenReported dropdown
	Then I should not see the WQPSamplesReason field
	When I select "N/A" from the WQPSamplesHaveBeenReported dropdown
	Then I should not see the WQPSamplesReason field
	When I select "Yes" from the WQPSamplesHaveBeenReported dropdown
	Then I should not see the WQPSamplesReason field
	When I select "No" from the WQPSamplesHaveBeenReported dropdown
	Then I should see the WQPSamplesReason field

	When I select "-- Select --" from the SurfaceWaterPlantSamplesHaveBeenCollected dropdown
	Then I should not see the SurfaceWaterPlantSamplesReason field
	When I select "N/A" from the SurfaceWaterPlantSamplesHaveBeenCollected dropdown
	Then I should not see the SurfaceWaterPlantSamplesReason field
	When I select "Yes" from the SurfaceWaterPlantSamplesHaveBeenCollected dropdown
	Then I should not see the SurfaceWaterPlantSamplesReason field
	When I select "No" from the SurfaceWaterPlantSamplesHaveBeenCollected dropdown
	Then I should see the SurfaceWaterPlantSamplesReason field

	When I select "Yes" from the SurfaceWaterPlantSamplesHaveBeenCollected dropdown
	And I select "-- Select --" from the SurfaceWaterPlantSamplesHaveBeenReported dropdown
	Then I should not see the SurfaceWaterPlantSamplesReason field
	When I select "N/A" from the SurfaceWaterPlantSamplesHaveBeenReported dropdown
	Then I should not see the SurfaceWaterPlantSamplesReason field
	When I select "Yes" from the SurfaceWaterPlantSamplesHaveBeenReported dropdown
	Then I should not see the SurfaceWaterPlantSamplesReason field
	When I select "No" from the SurfaceWaterPlantSamplesHaveBeenReported dropdown
	Then I should see the SurfaceWaterPlantSamplesReason field

	When I select "-- Select --" from the ChlorineResidualsHaveBeenCollected dropdown
	Then I should not see the ChlorineResidualsReason field
	When I select "N/A" from the ChlorineResidualsHaveBeenCollected dropdown
	Then I should not see the ChlorineResidualsReason field
	When I select "Yes" from the ChlorineResidualsHaveBeenCollected dropdown
	Then I should not see the ChlorineResidualsReason field
	When I select "No" from the ChlorineResidualsHaveBeenCollected dropdown
	Then I should see the ChlorineResidualsReason field

	When I select "Yes" from the ChlorineResidualsHaveBeenCollected dropdown
	And I select "-- Select --" from the ChlorineResidualsHaveBeenReported dropdown
	Then I should not see the ChlorineResidualsReason field
	When I select "N/A" from the ChlorineResidualsHaveBeenReported dropdown
	Then I should not see the ChlorineResidualsReason field
	When I select "Yes" from the ChlorineResidualsHaveBeenReported dropdown
	Then I should not see the ChlorineResidualsReason field
	When I select "No" from the ChlorineResidualsHaveBeenReported dropdown
	Then I should see the ChlorineResidualsReason field

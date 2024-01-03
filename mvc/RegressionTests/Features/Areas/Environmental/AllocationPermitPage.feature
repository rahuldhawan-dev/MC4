Feature: AllocationPermitPage
	Mission A-Corn-Plished - comes with corn salsa
	These collards don't run 
	Fig-eta Bout It	

Background: users and supporting data exists
	Given an admin user "admin" exists with username: "admin"
	And a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "EnvironmentalGeneral", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "EnvironmentalGeneral", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "EnvironmentalGeneral", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "EnvironmentalGeneral", user: "user"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an environmental permit type "PT1" exists
	And an environmental permit type "PT2" exists
	And an operating center "nj3" exists with opcode: "NJ3", name: "Fire Road", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
    And a data type "data type" exists with table name: "AllocationPermits"
    And a document type "document type" exists with data type: "data type", name: "Allocation Document"
	And public water supply statuses exist

Scenario: user can view an allocation permit
	Given an allocation permit "one" exists with source description: "ground well"
	And I am logged in as "user"
	When I visit the Show page for allocation permit: "one"
	Then I should see a display for allocation permit: "one"'s SourceDescription

Scenario: user can add an allocation permit
	Given a public water supply "one" exists with operating area: "AA", identifier: "1111", status: "active"
	And an environmental permit "one" exists with permit number: "123Foo", environmental permit type: "PT2"
	And I am logged in as "user"
	When I visit the Environmental/AllocationPermit page
	And I follow "Add"
	And I select operating center "nj7"'s Name from the OperatingCenter dropdown
	And I select public water supply "one"'s Description from the PublicWaterSupply dropdown
	And I select environmental permit "one"'s PermitNumber from the EnvironmentalPermit dropdown
	And I select "Yes" from the SurfaceSupply dropdown
	And I select "No" from the GroundSupply dropdown
	And I enter "formation" into the GeologicalFormation field
	And I select "Yes" from the ActivePermit dropdown
	And I enter "1/1/2014" into the EffectiveDateOfPermit field
	And I enter "2/1/2014" into the RenewalApplicationDate field
	And I enter "3/1/2014" into the ExpirationDate field
	And I enter "123sub" into the SubAllocationNumber field
	And I enter "1200" into the Gpd field
	And I enter "1300" into the Mgm field
	And I enter "1400" into the Mgy field
	And I enter "1500000.55" into the Gpm field
	And I enter "Allocation" into the PermitType field
	And I enter "2000.10" into the PermitFee field
	And I enter "source description" into the SourceDescription field
	And I enter "source restrictions" into the SourceRestrictions field
	And I enter "permit notes" into the PermitNotes field
	And I press Save
	Then I should see the validation message "Please enter an integer."
	When I enter "2000" into the PermitFee field
	And I press Save
	Then the currently shown allocation permit shall henceforth be known throughout the land as "one"
	And I should see a display for allocation permit: "one"'s Id
	And I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
	And I should see a display for PublicWaterSupply with public water supply "one"'s Description
	And I should see a display for SurfaceSupply with "Yes"
	And I should see a display for GroundSupply with "No"
	And I should see a display for GeologicalFormation with "formation"
	And I should see a display for ActivePermit with "Yes"
	And I should see a display for EffectiveDateOfPermit with "1/1/2014"
	And I should see a display for RenewalApplicationDate with "2/1/2014"
	And I should see a display for ExpirationDate with "3/1/2014"
	And I should see a display for SubAllocationNumber with "123sub"
	And I should see a display for Gpd with "1,200"
	And I should see a display for Mgm with "1,300"
	And I should see a display for Mgy with "1,400"
	And I should see a display for Gpm with "1,500,000.55"
	And I should see a display for PermitType with "Allocation"
	And I should see a display for PermitFee with "$2,000"
	And I should see a display for SourceDescription with "source description"
	And I should see a display for SourceRestrictions with "source restrictions"
	And I should see a display for PermitNotes with "permit notes"
	And I should see "123Foo"
		 
Scenario: user can edit an allocation permit
	Given a public water supply "one" exists with operating area: "AA", identifier: "1111"
	And an environmental permit "one" exists with permit number: "123Foo", environmental permit type: "PT2"
	And an allocation permit "one" exists
	And I am logged in as "user"
	When I visit the Show page for allocation permit: "one"
	And I follow "Edit"
	And I select operating center "nj7"'s Name from the OperatingCenter dropdown
	And I select public water supply "one"'s Description from the PublicWaterSupply dropdown
	And I select environmental permit "one"'s PermitNumber from the EnvironmentalPermit dropdown
	And I select "Yes" from the SurfaceSupply dropdown
	And I select "No" from the GroundSupply dropdown
	And I enter "formation" into the GeologicalFormation field
	And I select "Yes" from the ActivePermit dropdown
	And I enter "1/1/2014" into the EffectiveDateOfPermit field
	And I enter "2/1/2014" into the RenewalApplicationDate field
	And I enter "3/1/2014" into the ExpirationDate field
	And I enter "123sub" into the SubAllocationNumber field
	And I enter "1200" into the Gpd field
	And I enter "1300" into the Mgm field
	And I enter "1400" into the Mgy field
	And I enter "1500" into the Gpm field
	And I enter "Allocation" into the PermitType field
	And I enter "2000.10" into the PermitFee field
	And I enter "source description" into the SourceDescription field
	And I enter "source restrictions" into the SourceRestrictions field
	And I enter "permit notes" into the PermitNotes field
	And I press Save
	Then I should see the validation message "Please enter an integer."
	When I enter "2000" into the PermitFee field
	And I press Save
	Then I should see a display for allocation permit: "one"'s Id
	And I should see a display for OperatingCenter with "NJ7 - Shrewsbury"
	And I should see a display for PublicWaterSupply with public water supply "one"'s Description
	And I should see a display for SurfaceSupply with "Yes"
	And I should see a display for GroundSupply with "No"
	And I should see a display for GeologicalFormation with "formation"
	And I should see a display for ActivePermit with "Yes"
	And I should see a display for EffectiveDateOfPermit with "1/1/2014"
	And I should see a display for RenewalApplicationDate with "2/1/2014"
	And I should see a display for ExpirationDate with "3/1/2014"
	And I should see a display for SubAllocationNumber with "123sub"
	And I should see a display for Gpd with "1,200"
	And I should see a display for Mgm with "1,300"
	And I should see a display for Mgy with "1,400"
	And I should see a display for Gpm with "1,500"
	And I should see a display for PermitType with "Allocation"
	And I should see a display for PermitFee with "$2,000"
	And I should see a display for SourceDescription with "source description"
	And I should see a display for SourceRestrictions with "source restrictions"
	And I should see a display for PermitNotes with "permit notes"
	And I should see "123Foo"

Scenario: user can destroy an allocation permit
	Given an allocation permit "one" exists with source description: "ground well"
	And I am logged in as "user"
	When I visit the Show page for allocation permit: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Environmental/AllocationPermit/Search page
	When I try to access the Show page for allocation permit: "one" expecting an error
	Then I should see a 404 error message

# Notes
Scenario: user adds a note to an allocation permit
    Given an allocation permit "one" exists with source description: "ground well"
	And I am logged in as "admin"
    When I visit the Show page for allocation permit: "one"
    And I click the "Notes" tab
    And I press "New Note"
    And I enter "foo bar baz" into the Text field
    And I press "Add Note"
    And I click the "Notes" tab
    Then I should see "foo bar baz" in the table notesTable's "Note" column
    And I should see admin user "admin"'s UserName in the table notesTable's "Created By" column

Scenario: admin edits an existing note on an allocation permit
    Given an allocation permit "one" exists with source description: "ground well"
	And a note "note" exists with text: "foo bar", allocation permit: "one", data type: "data type"
	And I am logged in as "admin"
    When I visit the Show page for allocation permit: "one"
    And I click the "Notes" tab
    Then I should see "foo bar" in the table notesTable's "Note" column
    When I click the "Edit" button in the 1st row of notesTable
    And I enter "asdf" into the #notesTable #Text field
    And I click the "Update" button in the 1st row of notesTable
    Then I should see "asdf" in the table notesTable's "Note" column

Scenario: admin deletes an existing note from an allocation permit
    Given an allocation permit "one" exists with source description: "ground well"
	And a note "note" exists with text: "foo bar", allocation permit: "one", data type: "data type"
	And I am logged in as "admin"
    When I visit the Show page for allocation permit: "one"
    And I click the "Notes" tab
    Then I should see "foo bar" in the table notesTable's "Note" column
    When I click the "Delete" button in the 1st row of notesTable and then click ok in the confirmation dialog
    And I wait for the page to reload
    Then I should not see "foo bar" in the table notesTable's "Note" column

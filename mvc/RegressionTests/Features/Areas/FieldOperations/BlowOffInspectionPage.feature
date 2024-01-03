Feature: BlowOffInspectionPage
	In order to inspect blowoffs
	As a user or an admin
	I want to be able to add records for blow off inspections
	
Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And a valve billing "public" exists with description: "PUBLIC"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an asset type "valve" exists with description: "valve"
    And an asset type "hydrant" exists with description: "hydrant"
    And an asset type "sewer opening" exists with description: "sewer opening"
	And a town abbreviation type "town" exists
	And a town section abbreviation type "town section" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a street "one" exists with town: "one"
	And a street "two" exists with town: "one", full st name: "Easy St"
	And a valve zone "one" exists
	And a valve zone "two" exists
	And a valve control "flushing" exists with description: "BLOW OFF WITH FLUSHING"
	And a valve control "one" exists with description: "Foo"
	And a valve control "two" exists with description: "Bar"
	And a asset status "active" exists with description: "ACTIVE"
	And a asset status "retired" exists with description: "RETIRED"
    And a valve "one" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-1", valve suffix: 1, turns: 10, valve controls: "flushing", status: "active", valve billing: "public"
    And a valve "three" exists with valve zone: "one", town: "one", street: "one", valve number: "VLA-3", valve suffix: 3, status: "active", valve billing: "public"
	And a coordinate "one" exists 
	And a valve normal position "open" exists with description: "NORMALLY OPEN"
	And a valve normal position "closed" exists with description: "NORMALLY CLOSED"
	And a hydrant billing "public" exists with description: "Public"
	And a hydrant inspection type "flush" exists with description: "FLUSH"
	And a blow off inspection "one" exists with valve: "one"
    And I am logged in as "admin"

Scenario: Blow Off Inspection Add should not be visible in the action bar
	Given I am logged in as "admin"
	And I am at the FieldOperations/BlowOffInspection/Search page
	Then I should not see a link to the FieldOperations/BlowOffInspection/New page

Scenario: can search for a blow off inspection
	When I visit the /FieldOperations/BlowOffInspection page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select town "one" from the Town dropdown
    And I press Search
    Then I should see a link to the Show page for blow off inspection: "one"
	When I follow the Show link for blow off inspection "one"
    Then I should be at the Show page for blow off inspection: "one"

Scenario: can view a blow off inspection
	When I visit the Show page for blow off inspection: "one"
    Then I should be at the Show page for blow off inspection: "one"
	And I should see a display for blow off inspection: "one"'s DateInspected

Scenario: can add a blow off inspection and gets validation
    When I visit the Show page for valve: "one"
	And I click the "Blow Off Inspections" tab
	And I follow "New Blow Off Inspection"
	And I press "Save"
	Then I should see the validation message "The HydrantInspectionType field is required."
	When I select hydrant inspection type "flush" from the HydrantInspectionType dropdown
	And I press "Save"
	Then I should see the validation message "GPM is required for the selected inspection type."
	And I should see the validation message "Minutes flowed is required for the selected inspection type."
	When I enter 1 into the MinutesFlowed field
	And I enter 2 into the StaticPressure field
	And I enter 3 into the GPM field
	And I enter "12/8/1980 11:07 PM" into the DateInspected field
	And I enter 3 into the ResidualChlorine field
	And I enter 3 into the TotalChlorine field
	And I press "Save"
	Then the currently shown blow off inspection shall henceforth be known throughout the land as "Sir Spection"
	And I should see a display for DateInspected with "12/8/1980 11:07 PM"
	And I should see a display for InspectedBy with "admin"
	And I should see a display for HydrantInspectionType with hydrant inspection type "flush"'s Description
	And I should see a display for MinutesFlowed with "1"
	And I should see a display for StaticPressure with "2"
	And I should see a display for GPM with "3"

Scenario: Adding a blowoff inspection displays a popup whenever Chlorine values outside of 0.2-3.2 limits
    When I visit the Show page for valve: "one"
	And I click the "Blow Off Inspections" tab
	And I follow "New Blow Off Inspection"
	And I press "Save" 
	#popup doesn't appear
	 Then I should see a validation message for HydrantInspectionType with "The HydrantInspectionType field is required."
	And I should see a validation message for TotalNoReadReason with "The TotalNoReadReason field is required."
	And I should see a validation message for FreeNoReadReason with "The FreeNoReadReason field is required."
	When I enter "3.3" into the TotalChlorine field
	And I click ok in the dialog after pressing "Save"
	And I wait 1 second
	When I enter "3.1" into the TotalChlorine field
	And I press Save
	#popup doesn't appear
	When I enter "3.3" into the ResidualChlorine field
	And I click ok in the dialog after pressing "Save"
	When I enter "3.1" into the ResidualChlorine field
	And I press Save
	When I enter "0.1" into the TotalChlorine field
	And I click ok in the dialog after pressing "Save"
	When I enter "0.3" into the TotalChlorine field
	And I press Save
	When I enter "0.1" into the ResidualChlorine field
	And I click ok in the dialog after pressing "Save"
	When I enter "0.3" into the ResidualChlorine field
	And I press Save
	#popup doesn't appear
		
Scenario: can edit a blow off inspection
	When I visit the Show page for blow off inspection: "one"
	And I follow "Edit"
	And I press "Save"
	Then I should see the validation message "The HydrantInspectionType field is required."
	When I select hydrant inspection type "flush" from the HydrantInspectionType dropdown
	And I enter -1 into the PreResidualChlorine field
	And I enter 5 into the PreTotalChlorine field
	And I press "Save"
	Then I should see the validation message "GPM is required for the selected inspection type."
	And I should see the validation message "Minutes flowed is required for the selected inspection type."
	And I should see the validation message "Residual chlorine must be between 0 and 9.99"
	And I should see the validation message "Total chlorine must be between 0 and 4."
	When I enter 1 into the MinutesFlowed field
	And I enter 2 into the StaticPressure field
	And I enter 3 into the GPM field
	And I enter "12/8/1980 11:07 PM" into the DateInspected field
	And I enter 3 into the PreResidualChlorine field
	And I enter 4 into the ResidualChlorine field
	And I enter 3 into the PreTotalChlorine field
	And I enter 4 into the TotalChlorine field
	And I click ok in the dialog after pressing "Save"
	Then the currently shown blow off inspection shall henceforth be known throughout the land as "Sir Spection"
	And I should see a display for DateInspected with "12/8/1980 11:07 PM"
	And I should see a display for HydrantInspectionType with hydrant inspection type "flush"'s Description
	And I should see a display for MinutesFlowed with "1"
	And I should see a display for StaticPressure with "2"
	And I should see a display for GPM with "3"
	And I should see a display for PreResidualChlorine with "3"
	And I should see a display for PreTotalChlorine with "3"

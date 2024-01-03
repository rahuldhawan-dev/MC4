Feature: AllocationPermitWithdrawalNodePage
	No More Mr. Mice Guy
	Mouse Arrest
	Bats all folks! Exterminators

Background: users and supporting data exists
	Given a user "user" exists with username: "user"
	And a role "roleRead" exists with action: "Read", module: "EnvironmentalGeneral", user: "user"
	And a role "roleEdit" exists with action: "Edit", module: "EnvironmentalGeneral", user: "user"
	And a role "roleAdd" exists with action: "Add", module: "EnvironmentalGeneral", user: "user"
	And a role "roleDelete" exists with action: "Delete", module: "EnvironmentalGeneral", user: "user"
	And an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "nj3" exists with opcode: "NJ3", name: "Fire Road", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a role "roleFacilityReadNj4" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj4"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And a role "roleEquipmentReadNj4" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "nj4"
	And a role "roleEquipmentReadNj7" exists with action: "Read", module: "ProductionEquipment", user: "user", operating center: "nj7"

Scenario: user can view an allocation permit withdrawal node
	Given an allocation permit "one" exists with source description: "ground well"
	And an allocation permit withdrawal node "one" exists with description: "show this in the view"
	And I am logged in as "user"
	When I visit the Show page for allocation permit withdrawal node: "one"
	Then I should see a display for allocation permit withdrawal node: "one"'s Description

Scenario: user can add an allocation permit withdrawal node and link and remove it from an allocation permit
	Given an allocation permit "one" exists with source description: "ground well", operating center: "nj4"
	And a facility "one" exists with facility name: "Swimming Brook", operating center: "nj4"
	And an allocation category "one" exists with description: "cat 1"
	And an allocation category "two" exists with description: "cat 2"
	And I am logged in as "user"
	When I visit the Environmental/AllocationPermitWithdrawalNode page
	And I follow "Add"
	And I select allocation category "two"'s Description from the AllocationCategory dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I enter "123-A" into the WellPermitNumber field
	And I enter "123abc" into the Description field
	And I enter "123.01" into the AllowableGpm field
	And I enter "124.02" into the AllowableGpd field
	And I enter "125.03" into the AllowableMgm field
	And I enter "126.04" into the CapableGpm field
	And I enter "none" into the WithdrawalConstraint field
	And I select "Yes" from the HasStandByPower dropdown
	And I press Save
	Then the currently shown allocation permit withdrawal node shall henceforth be known throughout the land as "one"
	#And I should see a display for AllocationPermit with "1"
	And I should see a display for Facility with "Swimming Brook - NJ4-1"
	And I should see a display for AllocationCategory with "cat 2"
	And I should see a display for WellPermitNumber with "123-A"
	And I should see a display for Description with "123abc"
	And I should see a display for AllowableGpm with "123.01"
	And I should see a display for AllowableGpd with "124.02"
	And I should see a display for AllowableMgm with "125.03"
	And I should see a display for CapableGpm with "126.04"
	And I should see a display for WithdrawalConstraint with "none"
	And I should see a display for HasStandByPower with "Yes"
	When I click the "Allocation Groupings" tab
	And I press "Add Allocation Grouping"
	And I select allocation permit "one" from the AllocationPermit dropdown
	And I press "Save Allocation Grouping"
	And I wait for the page to reload
	Then I should see a link to the Show page for allocation permit "one"
	When I click the "Allocation Groupings" tab
	And I click ok in the dialog after pressing "Remove Allocation Grouping"
	And I wait for the page to reload
	Then I should not see a link to the Show page for allocation permit "one"
		
Scenario: user can edit an allocation permit withdrawal node
	Given an allocation permit "one" exists with source description: "ground well 1"
	And an allocation permit "two" exists with source description: "ground well 2"
	And a facility "one" exists with facility name: "Swimming", operating center: "nj7"
	And an allocation category "one" exists with description: "cat 1"
	And an allocation category "two" exists with description: "cat 2"
	And an allocation permit withdrawal node "one" exists with description: "show this in the view"
	And I am logged in as "user"
	When I visit the Show page for allocation permit withdrawal node: "one"
	And I follow "Edit"
	And I select allocation category "two"'s Description from the AllocationCategory dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I enter "123-A" into the WellPermitNumber field
	And I enter "123abc" into the Description field
	And I enter "123.01" into the AllowableGpm field
	And I enter "124.02" into the AllowableGpd field
	And I enter "125.03" into the AllowableMgm field
	And I enter "126.04" into the CapableGpm field
	And I enter "none" into the WithdrawalConstraint field
	And I select "Yes" from the HasStandByPower dropdown
	And I press Save
	Then I should see a display for Facility with "Swimming - NJ7-1"
	And I should see a display for AllocationCategory with "cat 2"
	And I should see a display for WellPermitNumber with "123-A"
	And I should see a display for Description with "123abc"
	And I should see a display for AllowableGpm with "123.01"
	And I should see a display for AllowableGpd with "124.02"
	And I should see a display for AllowableMgm with "125.03"
	And I should see a display for CapableGpm with "126.04"
	And I should see a display for WithdrawalConstraint with "none"
	And I should see a display for HasStandByPower with "Yes"

Scenario: user can destroy an allocation permit withdrawal node
	Given an allocation permit withdrawal node "one" exists with description: "show this in the view"
	And I am logged in as "user"
	When I visit the Show page for allocation permit withdrawal node: "one"
	And I click ok in the dialog after pressing "Delete"
	Then I should be at the Environmental/AllocationPermitWithdrawalNode/Search page
	When I try to access the Show page for allocation permit withdrawal node: "one" expecting an error
	Then I should see a 404 error message

Scenario: user can add and remove equipment for an allocation permit withdrawal node
	Given a facility "one" exists with facility id: "NJSB-1", facility name: "Swimming Brook", operating center: "nj7"
	And an equipment "one" exists with identifier: "NJSB-1-EQID-1", facility: "one"
	And an allocation permit "one" exists with source description: "ground well 1"
	And an allocation permit withdrawal node "one" exists with description: "show this in the view", facility: "one"
	And I am logged in as "user"
	When I visit the Show page for allocation permit withdrawal node: "one"
	And I click the "Equipment" tab
	And I press "Add Equipment"
	And I select equipment "one"'s Display from the Equipment dropdown
	And I press "Save Equipment"
	And I wait for the page to reload
	Then I should be at the show page for allocation permit withdrawal node: "one"
	And I should see a link to the Show page for equipment "one"
	When I click the "Equipment" tab
	And I click the "Remove Equipment" button in the 1st row of equipmentTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Equipment" tab
	Then I should not see a link to the Show page for equipment "one"

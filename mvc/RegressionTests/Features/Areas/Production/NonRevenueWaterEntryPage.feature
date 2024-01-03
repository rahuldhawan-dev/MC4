Feature: NonRevenueWaterEntryPage

Background:
	Given an admin user "admin" exists with username: "admin"
	And a state "nj" exists with abbreviation: "NJ"
	And a state "ak" exists with abbreviation: "AK"
	And an operating center "nj7" exists with state: "nj", opcode: "NJ7"
	And an operating center "ak1" exists with state: "ak", opcode: "AK1"
	And a department "dept" exists with description: "shoe department", id: 1
	And a business unit "bizunit" exists with operating center: "nj7", BU: "123456", department: "dept", is active: true
	And a business unit "inactivebizunit" exists with operating center: "nj7", BU: "999990", department: "dept", is active: false
	And a role "nonrevenuewaterread" exists with action: "Read", module: "ProductionNonRevenueWaterUnbilledUsage"
	And a role "nonrevenuewateredit" exists with action: "Edit", module: "ProductionNonRevenueWaterUnbilledUsage"
	And a user "user" exists with username: "user", roles: nonrevenuewaterread;nonrevenuewateredit;
	And a nonrevenue water entry "entryJan" exists with operating center: "nj7", created at: "02/10/2020", year: 2023, month: 1
	And a nonrevenue water entry "entryFeb" exists with operating center: "nj7", created at: "02/10/2020", year: 2023, month: 2
	And a nonrevenue water detail "detailJanOne" exists with nonrevenue water entry: "entryJan", month: "Jan", year: "2023", business unit: "123456", work description: "exterminate daleks", total gallons: 100
	And a nonrevenue water detail "detailJanTwo" exists with nonrevenue water entry: "entryJan", month: "Jan", year: "2023", business unit: "246802", work description: "TARDIS maintenance", total gallons: 250
	And a nonrevenue water detail "detailFebOne" exists with nonrevenue water entry: "entryFeb", month: "Feb", year: "2023", business unit: "135791", work description: "funding for new silly walks", total gallons: 3
	And a nonrevenue water adjustment "adjustmentJanOne" exists with nonrevenue water entry: "entryJan", total gallons: 21, comments: "missed some gallons", business unit: "123456"
	And a nonrevenue water adjustment "adjustmentJanTwo" exists with nonrevenue water entry: "entryJan", total gallons: 34, comments: "missed some more gallons", business unit: "123456"
	And a nonrevenue water adjustment "adjustmentJanThree" exists with nonrevenue water entry: "entryJan", total gallons: 55, comments: "fibonacci sequence", business unit: "246802"
	And a nonrevenue water adjustment "adjustmentFebOne" exists with nonrevenue water entry: "entryFeb", total gallons: 7, comments: "missed a few more gallons", business unit: "135791"
	And a nonrevenue water adjustment "adjustmentFebTwo" exists with nonrevenue water entry: "entryFeb", total gallons: 16, comments: "should be using litres anyway", business unit: "135791"
	
Scenario: user can search and view an entry details and adjustments
	Given I am logged in as "user"
	When I visit the Production/NonRevenueWaterEntry/Search page
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I select "January" from the Month dropdown
	And I enter "2023" into the Year field
	And I press "Search"
	Then I should be at the Production/NonRevenueWaterEntry page
	When I click the "View" link in the 1st row of nonRevenueWaterEntryTable
	And I wait for the page to reload
	Then I should be at the Production/NonRevenueWaterEntry/Show page for nonrevenue water entry "entryJan"
	And I should see the following values in the resultsDetails table
         | Operating Center | Month | Year | Business Unit | Work Description   | Total Gallons |
         | NJ7 - Shrewsbury | Jan   | 2023 | 123456        | exterminate daleks | 100           |
         | NJ7 - Shrewsbury | Jan   | 2023 | 246802        | TARDIS maintenance | 250           |
	And I should see "350" in the NonRevenueWaterDetailSubTotal element
	And I should see the following values in the resultsAdjustments table
		 | Operating Center | Business Unit | Comments                 | Total Gallons |
		 | NJ7 - Shrewsbury | 123456        | missed some gallons      | 21            |
		 | NJ7 - Shrewsbury | 123456        | missed some more gallons | 34            |
		 | NJ7 - Shrewsbury | 246802        | fibonacci sequence       | 55            |
	And I should see "460" in the NonRevenueWaterGrandTotal element

Scenario: user can add adjustments with active business units
	Given I am logged in as "user"
	When I visit the Production/NonRevenueWaterEntry/Show page for nonrevenue water entry "entryJan"
	And I press "Add Adjustments"
	And I enter "42" into the TotalGallons field
	And I enter "It was a tough assignment" into the Comments field
	And I press "Save"
	Then I should see a validation message for BusinessUnit with "The BusinessUnit field is required."
	And I should not see "999990" in the BusinessUnit dropdown
	When I select "123456" from the BusinessUnit dropdown
	And I press "Save"
	And I wait for the page to reload
	Then I should be at the Production/NonRevenueWaterEntry/Show page for nonrevenue water entry "entryJan"
	And I should see the following values in the resultsAdjustments table
		 | Operating Center | Business Unit | Comments                  | Total Gallons |
		 | NJ7 - Shrewsbury | 123456        | missed some gallons       | 21            |
		 | NJ7 - Shrewsbury | 123456        | missed some more gallons  | 34            |
		 | NJ7 - Shrewsbury | 246802        | fibonacci sequence        | 55            |
		 | NJ7 - Shrewsbury | 123456        | It was a tough assignment | 42            |
	And I should see "502" in the NonRevenueWaterGrandTotal element
	
Scenario: user default operating center is initially selected on search screen
	Given I am logged in as "user"
	When I visit the Production/NonRevenueWaterEntry/Search page
	Then operating center "nj7" should be selected in the OperatingCenter dropdown
	
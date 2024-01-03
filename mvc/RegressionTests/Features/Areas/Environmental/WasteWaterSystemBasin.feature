Feature: WasteWaterSystemBasinPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
   	And a waste water system "one" exists with WasteWaterSystemName: "Water System 1"
    And a waste water system basin "one" exists with WasteWaterSystem: "one", BasinName: "basin Name"
	And a role "role-EnvironmentalGeneral" exists with action: "UserAdministrator", module: "EnvironmentalGeneral", user: "user"

Scenario: user with environmental genenal can search waste water system basin
	Given I am logged in as "user"
	And I am at the Environmental/WasteWaterSystemBasin/Search  page
	When I press Search
	Then I should see a link to the Show page for waste water system basin: "one"
	And I should see the following values in the wwsb-results table
	| Id | Basin Name | Firm Capacity |
	| 1  | basin Name | 1.1           |

Scenario: site admin can add a Waste Water system Basin
	Given I am logged in as "admin"
	When I visit the Environmental/WasteWaterSystemBasin/New  page
	And I select waste water system "one"'s Description from the WasteWaterSystem dropdown
	And I enter "some name" into the BasinName field   
	And I enter "12/06/2020" into the FirmCapacityDateUpdated field
	And I press Save
   	Then I should see a display for WasteWaterSystem with waste water system "one"
	And I should see a display for BasinName with "some name"

Scenario: Adding a Waste Water system Basin has all sorts of validation
	Given I am logged in as "admin"
	When I visit the Environmental/WasteWaterSystemBasin/New  page
	And I select waste water system "one"'s Description from the WasteWaterSystem dropdown
	And I enter "eee" into the FirmCapacity field
	And I enter "13455" into the FirmCapacityUnderStandbyPower field
	And I press Save
	Then I should see the validation message "The field FirmCapacity must be a number."
	And I should see the validation message "The BasinName field is required."
	And I should see the validation message "The field FirmCapacityUnderStandbyPower must be between 0 and 999.999."



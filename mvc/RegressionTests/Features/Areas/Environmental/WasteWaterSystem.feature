Feature: Waste Water Systems

Background:
	Given a user "user" exists with username: "user"
	And a user "user-no-edit" exists with username: "user-no-edit"
	And a user "user-edit" exists with username: "user-edit"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And an department "dep1" exists
	And an department "dep2" exists
	And an business unit "bu1" exists with operating center: "nj7", department: "dep2"
	And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "EnvironmentalWasteWaterSystems", user: "user", operating center: "nj7"
	And a role "role-read-user-no-edit" exists with action: "Read", module: "EnvironmentalWasteWaterSystems", user: "user-no-edit"
	And a role "role-edit-user-edit" exists with action: "Edit", module: "EnvironmentalWasteWaterSystems", user: "user-edit"
	And a waste water system "one" exists with WasteWaterSystemName: "Water System 1"
	And an environmental permit status "one" exists with description: "Active"
	And an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit "one" exists with environmental permit type: "one", description: "Give a hoot- dont polute!", waste water system: "one", environmental permit status: "one", permit effective date: 10/2/2020, permit renewal date: 10/2/2020, permit expiration date: 10/2/2020 
	And waste water system statuses exist
	And a waste water system ownership "aw contract" exists with description: "AW Contract"
	And a waste water system type "collection only" exists with description: "Collection Only"
	And a waste water system sub type "fully separated sewer" exists with description: "Fully Separated Sewer"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And an employee status "active" exists with description: "Active"
	And an employee "one" exists with operating center: "nj7", status: "active", employee id: "123", first name: "Bill", last name: "Smith" 
	And a operator license type "one" exists with description: "Operator License Type 1"
	And a operator license "one" exists with employee: "one", operator license type: "one", "operating center: "nj7", expiration date: "3/1/2023", validation date: "3/5/2023", state: "one", license sub level: "15"
	
Scenario: User should see validation messages 
	Given I am logged in as "user"
	And I am at the Environmental/WasteWaterSystem/New page 
	When I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for WasteWaterSystemName with "The Wastewater System field is required."
	And I should see a validation message for PermitNumber with "The PermitNumber field is required."
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for BusinessUnit with "The BusinessUnit field is required."
	And I should see a validation message for Status with "The Status field is required."
	And I should see a validation message for Ownership with "The Ownership field is required."
	And I should see a validation message for Type with "The Type field is required."
	And I should see a validation message for SubType with "The SubType field is required."
	And I should see a validation message for HasConsentOrder with "The HasConsentOrder field is required."
	When I select "Yes" from the HasConsentOrder dropdown
	And I press Save
	Then I should see a validation message for ConsentOrderStartDate with "The ConsentOrderStartDate field is required."
	When I enter "01/01/2022" into the ConsentOrderStartDate field
	And I press Save
	Then I should see a validation message for ConsentOrderEndDate with "The ConsentOrderEndDate field is required."
	And I should not see a validation message for ConsentOrderStartDate with "The ConsentOrderStartDate field is required."
	When I enter "01/02/2022" into the ConsentOrderEndDate field
	And I press Save
	Then I should not see a validation message for ConsentOrderEndDate with "The ConsentOrderEndDate field is required."

Scenario: User can add a new WasteWater system
	Given I am logged in as "user"
	And I am at the Environmental/WasteWaterSystem/New page 
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select business unit "bu1" from the BusinessUnit dropdown
	And I enter "some name" into the WasteWaterSystemName field
	And I enter "some permit" into the PermitNumber field
	And I select waste water system status "active" from the Status dropdown
	And I select waste water system ownership "aw contract" from the Ownership dropdown
	And I select waste water system type "collection only" from the Type dropdown
	And I select waste water system sub type "fully separated sewer" from the SubType dropdown
	And I enter "3/11/2005" into the DateOfOwnership field
	And I enter "3/28/2005" into the DateOfResponsibility field
	And I enter "3333" into the GravityLength field
	And I enter "4444" into the ForceLength field
	And I enter "55" into the NumberOfLiftStations field
	And I enter "some description" into the TreatmentDescription field
	And I enter "66" into the NumberOfCustomers field
	And I enter "77" into the PeakFlowMGD field
	And I select "Yes" from the IsCombinedSewerSystem dropdown
	And I select "No" from the HasConsentOrder dropdown
	And I press Save
	Then I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for BusinessUnit with business unit "bu1"
	And I should see a display for State with "NJ"
	And I should see a display for WasteWaterSystemName with "some name"
	And I should see a display for PermitNumber with "some permit"
	And I should see a display for Status with waste water system status "active"
	And I should see a display for Ownership with waste water system ownership "aw contract"
	And I should see a display for Type with waste water system type "collection only"
	And I should see a display for SubType with waste water system sub type "fully separated sewer"
	And I should see a display for DateOfOwnership with "3/11/2005"
	And I should see a display for DateOfResponsibility with "3/28/2005"
	And I should see a display for GravityLength with "3333"
	And I should see a display for ForceLength with "4444"
	And I should see a display for NumberOfLiftStations with "55"
	And I should see a display for TreatmentDescription with "some description"
	And I should see a display for NumberOfCustomers with "66"
	And I should see a display for PeakFlowMGD with "77"
	And I should see a display for IsCombinedSewerSystem with "Yes"
	And I should see a display for TotalLengthFeet with "7777"
	And I should see a display for TotalLengthMiles with "1.473"
	And I should see a display for Total100Miles with "0.015"
	And I should see a display for HasConsentOrder with "No"

Scenario: User can Search for a waste water system
	Given I am logged in as "user"
	And I am at the Environmental/WasteWaterSystem/Search page
	When I select state "one" from the State dropdown
	And I press Search
    Then I should see a link to the Show page for waste water system: "one"
	And I should see the following values in the ww-results table
	| Id | WWSID  | Operating Center   | 
	| *  | *NJ7WW*  | NJ7 - Shrewsbury |  
	
Scenario: user can view a waste water system
	Given an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
    When I visit the Show page for waste water system: "one"
	Then I should see a display for State with "NJ"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for WasteWaterSystemName with "Water System 1"
	And I should see a display for WasteWaterSystemId with waste water system "one"'s WasteWaterSystemId

Scenario: user can view the environmental permits tab
    Given an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
    When I visit the Show page for waste water system: "one"
    And I click the "Environmental Permits" tab
	Then I should see the following values in the EnvironmentalPermit table
	| Permit Type   | Permit Number | Effective Date | Renewal Date | Expiration Date | Permit Status |
	| Water Quality | P#1123        | 10/2/2020      | 10/2/2020    | 10/2/2020        | Active        |

Scenario: no-edit-access user can see the banner message email address on the search page
	Given I am logged in as "user-no-edit"
	When I visit the Environmental/WasteWaterSystem/Search page
	Then I should see the link "wwid@amwater.com" with the url "mailto:wwid@amwater.com"

Scenario: edit-access user can not see the banner message email address on the search page
	Given I am logged in as "user-edit"
	When I visit the Environmental/WasteWaterSystem/Search page
	Then I should not see the link "wwid@amwater.com"

Scenario: user can view licensed operators when licensed operator has waste water system added
	Given an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
	And I am at the Show page for operator license: "one"
	When I click the "WWSID" tab 
	And I press "Add New Waste Water System"
	And I select waste water system "one" from the WasteWaterSystem dropdown 
	And I press "Add Waste Water System"
	Then I should be at the Show page for operator license: "one"
	When I click the "WWSID" tab 
	Then I should see waste water system "one"'s Description in the table wasteWaterSystemTable's "Waste Water System" column
	When I visit the Show page for waste water system: "one"
	And I click the "Licensed Operators" tab	
	Then I should see the following values in the OperatorLicenseTable table
	| Employee   | Operating Center | Operator License Type   | License Level/Class | License Sub-level/Sub-class | License Number | Validation Date | Expiration Date |
	| Bill Smith | NJ7 - Shrewsbury | Operator License Type 1 | 1234                | 15                          | 1234           | 3/5/2023        | 3/1/2023        |

Scenario: admin user can add a planning plant to a waste water system
	Given an admin user "admin" exists with username: "admin"
	And I am logged in as "admin"
	And a planning plant "planningplantone" exists with operating center: "nj7", code: "D217", description: "My Planning Plant"
	When I visit the Show page for waste water system: "one"
	And I click the "Planning Plants" tab
	And I press "Add New Planning Plant"
	And I select "D217 - NJ7 - My Planning Plant" from the PlanningPlant dropdown
	And I press "Add Planning Plant"
	And I wait for the page to reload
	And I click the "Planning Plants" tab
	Then I should see the following values in the planningPlantTable table
	  | Planning Plant                 | 
	  | D217 - NJ7 - My Planning Plant |

Scenario: not-admin user can not add a planning plant to a waste water system
	Given I am logged in as "user"
	When I visit the Show page for waste water system: "one"
	And I click the "Planning Plants" tab
	Then I should not see "Add New Planning Plant"
	And I should not see the button "Remove Planning Plant"
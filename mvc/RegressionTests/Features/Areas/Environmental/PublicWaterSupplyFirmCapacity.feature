Feature: PublicWaterSupplyFirmCapacity

Background: data exists
    Given a state "one" exists with abbreviation: "NJ"
    And public water supply statuses exist
    And a public water supply "pws-01" exists with identifier: "NJ1345001", system: "Union Beach1", state: "one", status: "active"
    And a public water supply "pws-02" exists with identifier: "NJ1345002", system: "Union Beach2", state: "one", status: "active"
    And a public water supply firm capacity "fc" exists with public water supply: "pws-01", firm capacity multiplier: "0.5"
    And a user "user-unauthorized" exists with username: "user-unauthorized"
    And a user "user-admin" exists with username: "user-admin"
    And a user "user-no-edit" exists with username: "user-no-edit"
    And a role "role-admin" exists with action: "UserAdministrator", module: "EngineeringPWSIDCapacity", user: "user-admin"
    And a role "role-read" exists with action: "Read", module: "EngineeringPWSIDCapacity", user: "user-admin"
	And a role "role-edit" exists with action: "Edit", module: "EngineeringPWSIDCapacity", user: "user-admin"
	And a role "role-add" exists with action: "Add", module: "EngineeringPWSIDCapacity", user: "user-admin"
    And a role "role-read-user-no-edit" exists with action: "Read", module: "EngineeringPWSIDCapacity", user: "user-no-edit"
	And a role "role-edit-user-no-edit" exists with action: "Edit", module: "EngineeringPWSIDCapacity", user: "user-no-edit"
	And a role "role-add-user-no-edit" exists with action: "Add", module: "EngineeringPWSIDCapacity", user: "user-no-edit"
    
Scenario: Unauthorized users cannot access any pages
    Given I am logged in as "user-unauthorized"
    And I am at the Environmental/PublicWaterSupplyFirmCapacity/Search page
    Then I should see "You do not have the roles necessary to access this resource."
    When I visit the Environmental/PublicWaterSupplyFirmCapacity/New page
    Then I should see "You do not have the roles necessary to access this resource."
    When I visit the Show page for public water supply firm capacity: "fc"
    Then I should see "You do not have the roles necessary to access this resource."
    When I visit the Edit page for public water supply firm capacity: "fc"
    Then I should see "You do not have the roles necessary to access this resource."

Scenario: Validation messages display if a public water supply firm capacity form is entered incorrectly
    Given I am logged in as "user-admin"
    When I visit the Environmental/PublicWaterSupplyFirmCapacity/New page

    # Test Required
    And I press Save
    Then I should see a validation message for PublicWaterSupply with "The PublicWaterSupply field is required."
    And I should see a validation message for CurrentSystemPeakDailyDemandMGD with "The CurrentSystemPeakDailyDemandMGD field is required."
    And I should see a validation message for CurrentSystemPeakDailyDemandYearMonth with "The Current System Peak Daily Demand Date field is required."
    And I should see a validation message for FirmCapacityMultiplier with "The Firm Capacity Multiplier (<1.0) field is required."

    # Test Range Max
    When I enter "1.01" into the FirmCapacityMultiplier field
    And I press Save
    Then I should see a validation message for FirmCapacityMultiplier with "The field Firm Capacity Multiplier (<1.0) must be between 0 and 1."

    # Test Range Min
    When I enter "-0.1" into the FirmCapacityMultiplier field
    And I press Save
    Then I should see a validation message for FirmCapacityMultiplier with "The field Firm Capacity Multiplier (<1.0) must be between 0 and 1."

Scenario: An admin user can add and edit a public water supply firm capacity
    Given a facility "one" exists with facility id: "NJ7-1", public water supply: "pws-02", facility total capacity MGD: "2.0"
    And a facility "two" exists with facility id: "NJ7-2", public water supply: "pws-02", facility total capacity MGD: "3.0", used in production capacity calculation: "true"
    Given I am logged in as "user-admin"
    When I visit the Environmental/PublicWaterSupplyFirmCapacity/New page
    And I select public water supply "pws-02" from the PublicWaterSupply dropdown
    And I enter "1.1" into the CurrentSystemPeakDailyDemandMGD field
    And I enter "1/11/2022" into the CurrentSystemPeakDailyDemandYearMonth field
    And I enter "3.3" into the TotalSystemSourceCapacityMGD field
    And I enter "0.5" into the FirmCapacityMultiplier field
    When I press Save
    Then the currently shown public water supply firm capacity will now be referred to as "fc-added"
    And I should see a display for CurrentSystemPeakDailyDemandMGD with "1.1"
    And I should see a display for CurrentSystemPeakDailyDemandYearMonth with "1/11/2022"
    And I should see a display for TotalSystemSourceCapacityMGD with "3.3"
    And I should see a display for TotalCapacityFacilitySumMGD with "3.0000"
    And I should see a display for FirmCapacityMultiplier with "0.5000"
    And I should see a display for FirmSystemSourceCapacityMGD with "1.5000"
    And I should see a display for UpdatedAt with "today's date"
    When I visit the Edit page for public water supply firm capacity: "fc-added"
    And I select public water supply "pws-02" from the PublicWaterSupply dropdown
    And I enter "99.99" into the CurrentSystemPeakDailyDemandMGD field
    And I enter "1/3/2022" into the CurrentSystemPeakDailyDemandYearMonth field
    And I enter "77.77" into the TotalSystemSourceCapacityMGD field
    And I enter "0.6" into the FirmCapacityMultiplier field
    And I press Save
    Then I should see a display for CurrentSystemPeakDailyDemandMGD with "99.99"
    And I should see a display for CurrentSystemPeakDailyDemandYearMonth with "1/3/2022"
    And I should see a display for TotalSystemSourceCapacityMGD with "77.77"
    And I should see a display for TotalCapacityFacilitySumMGD with "3.0000"
    And I should see a display for FirmCapacityMultiplier with "0.6000"
    And I should see a display for FirmSystemSourceCapacityMGD with "1.8000"

Scenario: A non admin user cannot edit firm capacity multiplier
    Given a facility "one" exists with facility id: "NJ7-1", public water supply: "pws-02", facility total capacity MGD: "2.0"
    And a facility "two" exists with facility id: "NJ7-2", public water supply: "pws-02", facility total capacity MGD: "2.0"
    Given I am logged in as "user-no-edit"
    When I visit the Environmental/PublicWaterSupplyFirmCapacity/New page
    And I select public water supply "pws-02" from the PublicWaterSupply dropdown
    And I enter "1.1" into the CurrentSystemPeakDailyDemandMGD field
    And I enter "1/11/2022" into the CurrentSystemPeakDailyDemandYearMonth field
    And I enter "3.3" into the TotalSystemSourceCapacityMGD field
    And I enter "0.5" into the FirmCapacityMultiplier field
    When I press Save
    Then the currently shown public water supply firm capacity will now be referred to as "fc-added"
    When I visit the Edit page for public water supply firm capacity: "fc-added"
    Then I should see a display for FirmCapacityMultiplier with "0.5000"

Scenario: A user should not be able to delete firm capacities
    Given I am logged in as "user-admin"
    When I visit the Edit page for public water supply firm capacity: "fc"
    Then I should not see the "delete" button in the action bar

Scenario: A user can search for a public water supply firm capacity
    Given I am logged in as "user-admin"
    When I visit the Environmental/PublicWaterSupplyFirmCapacity/Search page
    Then I should see the State field
    And I should see the PublicWaterSupply field
    And I should see the TotalCapacityFacilitySumMGD field
    And I should see the UpdatedAt field
    When I select state "one" from the State dropdown
    And I select public water supply "pws-01" from the PublicWaterSupply dropdown
    When I press Search	
    Then I should see a link to the Show page for public water supply firm capacity: "fc"

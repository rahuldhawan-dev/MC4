Feature: PublicWaterSupplyPressureZones

Background: data exists
    Given public water supply statuses exist
    And a public water supply "pws-01" exists with identifier: "NJ1345001", system: "Union Beach", status: "active"
    And a public water supply pressure zone "pz" exists with public water supply: "pws-01", hydraulic model name: "pressure zone - 01", common name: "commoner", hydraulic gradient min: "42", hydraulic gradient max: "52", pressure min: "62", pressure max: "72"
    And a user "user-unauthorized" exists with username: "user-unauthorized"
    And a user "user-admin" exists with username: "user-admin"
    And an admin user "user-site-admin" exists with username: "user-site-admin"
    And a role "role-admin" exists with action: "UserAdministrator", module: "EnvironmentalGeneral", user: "user-admin"

Scenario: Unauthorized users cannot access any pages
    Given I am logged in as "user-unauthorized"
    When I visit the Environmental/PublicWaterSupplyPressureZone/Search page
    Then I should see "You do not have the roles necessary to access this resource."
    When I visit the Show page for public water supply pressure zone: "pz"
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Environmental/PublicWaterSupplyPressureZone/New page
    Then I should be at the Error/Forbidden screen
    When I try to visit the Edit page for public water supply pressure zone: "pz"
    Then I should be at the Error/Forbidden screen

Scenario: Area admins users cannot access mutating pages
    Given I am logged in as "user-admin"
    When I visit the Show page for public water supply pressure zone: "pz"
    Then I should at least see "Pressure Zones" in the bodyHeader element
    And I should at least see "pressure zone - 01" in the bodyHeader element
    And I should not see a link to the Edit page for public water supply pressure zone: "pz"
    And I should not see the "add" button in the action bar
    And I should not see the "delete" button in the action bar
    When I try to visit the Environmental/PublicWaterSupplyPressureZone/New page
    Then I should be at the Error/Forbidden screen
    When I try to visit the Edit page for public water supply pressure zone: "pz"
    Then I should be at the Error/Forbidden screen

Scenario: Validation messages display if a public water supply pressure zone form is entered incorrectly
    Given I am logged in as "user-site-admin"
    When I visit the Environmental/PublicWaterSupplyPressureZone/New page
    And I press Save

    ### Test Required
    Then I should see a validation message for PublicWaterSupply with "The PublicWaterSupply field is required."
    And I should see a validation message for HydraulicModelName with "The HydraulicModelName field is required."
    And I should see a validation message for HydraulicGradientMin with "The Hydraulic Gradient (HGL) Min field is required."
    And I should see a validation message for HydraulicGradientMax with "The Hydraulic Gradient (HGL) Max field is required."

    ### Test Is A Number
    When I enter "invalid data, needs to be numeric only" into the HydraulicGradientMin field
    When I enter "invalid data, needs to be numeric only" into the HydraulicGradientMax field
    When I enter "invalid data, needs to be numeric only" into the PressureMin field
    When I enter "invalid data, needs to be numeric only" into the PressureMax field
    And I press Save    
    Then I should see a validation message for HydraulicGradientMin with "The field Hydraulic Gradient (HGL) Min must be a number."
    Then I should see a validation message for HydraulicGradientMax with "The field Hydraulic Gradient (HGL) Max must be a number."
    Then I should see a validation message for PressureMin with "The field PressureMin must be a number."
    Then I should see a validation message for PressureMax with "The field PressureMax must be a number."

    ### Test Min Value
    When I enter "-1" into the HydraulicGradientMin field
    When I enter "0" into the HydraulicGradientMax field
    When I enter "-1" into the PressureMin field
    When I enter "0" into the PressureMax field
    And I press Save    
    Then I should see a validation message for HydraulicGradientMin with "The field Hydraulic Gradient (HGL) Min must be between 0 and 499."
    Then I should see a validation message for HydraulicGradientMax with "The field Hydraulic Gradient (HGL) Max must be between 1 and 500."
    Then I should see a validation message for PressureMin with "The field PressureMin must be between 0 and 499."
    Then I should see a validation message for PressureMax with "The field PressureMax must be between 1 and 500."

    ### Test Max Value
    When I enter "500" into the HydraulicGradientMin field
    When I enter "501" into the HydraulicGradientMax field
    When I enter "500" into the PressureMin field
    When I enter "501" into the PressureMax field
    And I press Save
    Then I should see a validation message for HydraulicGradientMin with "The field Hydraulic Gradient (HGL) Min must be between 0 and 499."
    Then I should see a validation message for HydraulicGradientMax with "The field Hydraulic Gradient (HGL) Max must be between 1 and 500."
    Then I should see a validation message for PressureMin with "The field PressureMin must be between 0 and 499."
    Then I should see a validation message for PressureMax with "The field PressureMax must be between 1 and 500."

    ### Test Comparison
    When I enter "11" into the HydraulicGradientMin field
    When I enter "10" into the HydraulicGradientMax field
    When I enter "21" into the PressureMin field
    When I enter "20" into the PressureMax field
    And I press Save
    Then I should see a validation message for HydraulicGradientMin with "HydraulicGradientMin must be less than HydraulicGradientMax."
    Then I should see a validation message for HydraulicGradientMax with "HydraulicGradientMax must be greater than HydraulicGradientMin."
    Then I should see a validation message for PressureMin with "PressureMin must be less than PressureMax."
    Then I should see a validation message for PressureMax with "PressureMax must be greater than PressureMin."

    ### Test Null Comparisons
    When I enter "21" into the PressureMin field
    When I enter "" into the PressureMax field
    And I press Save
    Then I should see a validation message for PressureMin with "PressureMin must be less than PressureMax."
    Then I should not see a validation message for PressureMax with "PressureMax must be greater than PressureMin."
    When I enter "" into the PressureMin field
    When I enter "20" into the PressureMax field
    And I press Save
    Then I should not see a validation message for PressureMin with "PressureMin must be less than PressureMax."
    Then I should see a validation message for PressureMax with "PressureMax must be greater than PressureMin."

Scenario: Validation messages do not display when nullable fields are null
    Given I am logged in as "user-site-admin"
    When I visit the Environmental/PublicWaterSupplyPressureZone/New page
    And I press Save
    Then I should not see a validation message for PressureMin with "The field PressureMin must be a number."
    Then I should not see a validation message for PressureMax with "The field PressureMax must be a number."
    Then I should not see a validation message for PressureMin with "The field PressureMin must be between 0 and 499."
    Then I should not see a validation message for PressureMax with "The field PressureMax must be between 1 and 500."
    Then I should not see a validation message for PressureMin with "PressureMin must be less than PressureMax."
    Then I should not see a validation message for PressureMax with "PressureMax must be greater than PressureMin."

Scenario: A user can add a public water supply pressure zone
    Given I am logged in as "user-site-admin"
    When I visit the Environmental/PublicWaterSupplyPressureZone/New page
    And I select public water supply "pws-01" from the PublicWaterSupply dropdown
    And I enter "Pressure Zone - 10" into the HydraulicModelName field
    And I enter "Common Name" into the CommonName field
    And I enter "12" into the HydraulicGradientMin field
    And I enter "22" into the HydraulicGradientMax field
    And I enter "33" into the PressureMin field
    And I enter "44" into the PressureMax field
    And I press Save
    Then the currently shown public water supply pressure zone will now be referred to as "pz-added"
    And I should see a display for HydraulicModelName with "Pressure Zone - 10"
    And I should see a display for CommonName with "Common Name"
    And I should see a display for HydraulicGradientMin with "12"
    And I should see a display for HydraulicGradientMax with "22"
    And I should see a display for PressureMin with "33"
    And I should see a display for PressureMax with "44"

Scenario: A user can edit a public water supply pressure zone
    Given I am logged in as "user-site-admin"
    When I visit the Edit page for public water supply pressure zone: "pz"
    And I select public water supply "pws-01" from the PublicWaterSupply dropdown
    And I enter "Pressure Zone - Updated" into the HydraulicModelName field
    And I enter "Common Name - Updated" into the CommonName field
    And I enter "112" into the HydraulicGradientMin field
    And I enter "122" into the HydraulicGradientMax field
    And I enter "133" into the PressureMin field
    And I enter "144" into the PressureMax field
    And I press Save
    Then I should see a display for HydraulicModelName with "Pressure Zone - Updated"
    And I should see a display for CommonName with "Common Name - Updated"
    And I should see a display for HydraulicGradientMin with "112"
    And I should see a display for HydraulicGradientMax with "122"
    And I should see a display for PressureMin with "133"
    And I should see a display for PressureMax with "144"

Scenario: A user can delete a public water supply pressure zone
    Given I am logged in as "user-site-admin"
    When I visit the Show page for public water supply pressure zone: "pz"
    And I click ok in the dialog after pressing "Delete"
    Then I should be at the Environmental/PublicWaterSupplyPressureZone/Search page
    When I try to access the Show page for public water supply pressure zone: "pz" expecting an error
    Then I should see a 404 error message

Scenario: A user should see no values for firm capacities when a pressure zone is assigned to a pws without a current firm capacity
    Given I am logged in as "user-site-admin"
    When I visit the Show page for public water supply pressure zone: "pz"
    Then I should see a display for HydraulicModelName with "pressure zone - 01"
    And I should see a display for HydraulicGradientMin with "42"
    And I should see a display for HydraulicGradientMax with "52"
    And I should see a display for PressureMin with "62"
    And I should see a display for PressureMax with "72"
    And I should see a display for PublicWaterSupplyFirmCapacity_UpdatedAt with " "
    And I should see a display for PublicWaterSupplyFirmCapacity_FirmSystemSourceCapacityMGD with " "

Scenario: A user can add a firm capacity to a pws and it shows up as the firm capacity for a pressure zone in that pws
    Given a facility "one" exists with facility id: "NJ7-1", public water supply: "pws-01", facility total capacity MGD: "2.0"
    And a facility "two" exists with facility id: "NJ7-2", public water supply: "pws-01", facility total capacity MGD: "3.0", used in production capacity calculation: "true"
    And I am logged in as "user-site-admin"
    When I visit the Environmental/PublicWaterSupplyFirmCapacity/New page
    And I select public water supply "pws-01" from the PublicWaterSupply dropdown
    And I enter "1.1" into the CurrentSystemPeakDailyDemandMGD field
    And I enter "2.2" into the CurrentSystemPeakDailyDemandYearMonth field
    And I enter "3.3" into the TotalSystemSourceCapacityMGD field
    And I enter "0.5" into the FirmCapacityMultiplier field
    And I press Save
    Then the currently shown public water supply firm capacity will now be referred to as "fc-added"
    When I visit the Show page for public water supply pressure zone: "pz"
    Then I should see a display for HydraulicModelName with "pressure zone - 01"
    And I should see a display for CommonName with "commoner"
    And I should see a display for HydraulicGradientMin with "42"
    And I should see a display for HydraulicGradientMax with "52"
    And I should see a display for PressureMin with "62"
    And I should see a display for PressureMax with "72"
    And I should see a display for PublicWaterSupplyFirmCapacity_UpdatedAt with "today's date"
    And I should see a display for PublicWaterSupplyFirmCapacity_FirmSystemSourceCapacityMGD with "1.5000"

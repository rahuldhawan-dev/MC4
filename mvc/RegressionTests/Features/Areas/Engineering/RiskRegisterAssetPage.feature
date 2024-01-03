Feature: RiskRegisterAssets

Background: data exists
    Given a state "NJ" exists with abbreviation: "NJ"
    And a town "Swedesboro" exists with state: "NJ"
    And an operating center "oc-01" exists with opcode: "NJ4", name: "Shrewsbury", state: "NJ"
    And operating center: "oc-01" exists in town: "Swedesboro"
    And a public water supply "pws-01" exists with identifier: "NJ1345001", system: "Coastal North Monmouth/Lakewood"
    And operating center: "oc-01" exists in public water supply: "pws-01"
    And a risk register asset category "category" exists with description: "Service Level"
    And a risk register asset zone "zone" exists with description: "C"
    And a risk register asset group "grouping" exists with description: "System"
    And a facility "f-01" exists with facility id: "NJSB-01", operating center: "oc-01", town: "Swedesboro", facility name: "argh"
    And an employee "e-01" exists with operating center: "oc-01", employee id: "12345678"
    And an equipment "e-01" exists with identifier: "NJSB-1-EQID-1", facility: "f-01"
    And a user "user-unauthorized" exists with username: "user-unauthorized"
    And a user "user-admin" exists with username: "user-admin", employee: "e-01"
    And a role "role-admin" exists with action: "UserAdministrator", module: "EngineeringRiskRegister", user: "user-admin", operating center: "oc-01"
    And a risk register asset "hra" exists with state: "NJ", operating center: "oc-01", risk register asset category: "category", CofMax: "10", LofMax: "20", risk register asset group: "grouping", facility: "f-01", employee: "e-01", RiskDescription: "Testing Risk Description"

Scenario: Unauthorized users cannot access any pages
    Given I am logged in as "user-unauthorized"
    And I am at the Engineering/RiskRegisterAsset/Search page
    Then I should see "You do not have the roles necessary to access this resource."
    When I visit the Engineering/RiskRegisterAsset/New page
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Show page for risk register asset: "hra"
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Edit page for risk register asset: "hra"
    Then I should see "You do not have the roles necessary to access this resource."

Scenario: Default state is set correctly when adding a new risk register asset
    Given I am logged in as "user-admin"
    When I visit the Engineering/RiskRegisterAsset/New page
    Then I should see "today's date" in the IdentifiedAt field

Scenario: Validation messages display if a risk register asset form is entered incorrectly
    Given I am logged in as "user-admin"
    When I visit the Engineering/RiskRegisterAsset/New page
    And I press Save
    Then I should see a validation message for State with "The State field is required."
    And I should see a validation message for RiskRegisterAssetGroup with "The Asset field is required."
    And I should see a validation message for LofMax with "The LOF Max field is required."
    And I should see a validation message for RiskDescription with "The Description of Risk field is required."
    And I should see a validation message for CofMax with "The COF Max field is required."
    And I should see a validation message for ImpactDescription with "The COF Weighted field is required."
    When I enter "" into the IdentifiedAt field
    And I press Save
    Then I should see a validation message for IdentifiedAt with "The Date Risk Identified field is required."
    When I enter "today's date" into the IdentifiedAt field
    And I enter "This is a description of risk" into the RiskDescription field
    And I enter "This is a description of impact" into the ImpactDescription field
    And I select "NJ" from the State dropdown
    And I enter "29" into the RiskQuadrant field
    And I press Save
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
    And I should see a validation message for RiskQuadrant with "The field Risk Max must be between 1 and 25."
    When I select operating center "oc-01" from the OperatingCenter dropdown
    And I enter "5" into the LofMax field
    And I enter "7" into the CofMax field
    And I enter "9" into the RiskQuadrant field
    And I enter "11" into the TotalRiskWeighted field
    And I press Save
    Then I should see a validation message for RiskRegisterAssetGroup with "The Asset field is required."
    And I should see a validation message for Employee with "The Risk Project Owner field is required."
    When I select risk register asset group "grouping" from the RiskRegisterAssetGroup dropdown
    And I select employee "e-01"'s Description from the Employee dropdown
    And I select risk register asset category "category"'s Description from the RiskRegisterAssetCategory dropdown
    And I select risk register asset zone "zone"'s Description from the RiskRegisterAssetZone dropdown
    And I press Save
    And I wait for the page to reload
    Then I should see a display for State with "NJ"
    And I should see a display for RiskRegisterAssetGroup with risk register asset group "grouping"
    And I should see a display for LofMax with "5"
    And I should see a display for CofMax with "7"
    And I should see a display for RiskQuadrant with "9"
    And I should see a display for TotalRiskWeighted with "11"
    And I should see a display for RiskDescription with "This is a description of risk"
    And I should see a display for ImpactDescription with "This is a description of impact"
    And I should see a display for RiskRegisterAssetZone with risk register asset zone "zone"

Scenario: A user can delete a risk register asset
    Given I am logged in as "user-admin"
    When I visit the Show page for risk register asset: "hra"
    And I click ok in the dialog after pressing "Delete"
    Then I should be at the Engineering/RiskRegisterAsset/Search page
    When I try to access the Show page for risk register asset: "hra" expecting an error
    Then I should see a 404 error message

Scenario: User search for results
    Given I am logged in as "user-admin"
    When I visit the Engineering/RiskRegisterAsset/Search page
    And I press Search
    Then I should see the following values in the riskRegisterAssetsTable table
    | Threat/Asset Pair Category | Description of Risk       | LOF Max |
    |   Service Level            | Testing Risk Description  |  20     |
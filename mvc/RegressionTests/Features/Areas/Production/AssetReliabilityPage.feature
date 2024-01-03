Feature: AssetReliabilityPage

Background:
    Given an employee status "active" exists with description: "Active"
    And an employee "one" exists with status: "active", employee id: "12345678", first name: "jay", last name: "bob"
    And an employee "two" exists with status: "active", employee id: "456", first name: "Not", last name: "APerson"
    And a role "AssetReliability-read" exists with action: "Read", module: "ProductionAssetReliability"
    And a role "AssetReliability-add" exists with action: "Add", module: "ProductionAssetReliability"
    And a role "AssetReliability-edit" exists with action: "Edit", module: "ProductionAssetReliability"
    And a role "AssetReliability-delete" exists with action: "Delete", module: "ProductionAssetReliability"
    And a role "productionworkorder-read" exists with action: "Read", module: "ProductionWorkManagement"
    And a role "equipmentrole" exists with action: "Read", module: "ProductionEquipment"
    And a equipment type "generator" exists with description: "Generator"
    And a equipment "one" exists with equipment type: "generator"
    And a equipment "two" exists
    And a production work order "one" exists
    And a production work order equipment "pwoe" exists with production work order: "one", equipment: "one"
    And asset reliability technology used types exist
    And a asset reliability "one" exists with production work order: "one", equipment: "one", employee: "one"
    And a asset reliability "two" exists with equipment: "two", employee: "two"
    And a user "user" exists with username: "user", employee: "one", roles: AssetReliability-read;AssetReliability-add;AssetReliability-edit;AssetReliability-delete;productionworkorder-read;equipmentrole

Scenario: User can create a asset reliability from a production work order
    Given I am logged in as "user"
    And equipment: "one" exists in production work order: "one"
    And equipment: "two" exists in production work order: "one"
    And I am at the Show page for production work order "one"
    When I click the "Asset Reliability" tab
    Then I should see a link to "Production/AssetReliability/New?productionWorkOrderId=1&equipmentId=1"
    When I click the "Create Asset Reliability" link in the 1st row of assetReliabilityTable
    Then I should see production work order "one"'s Id in the ProductionWorkOrder field
    And I should see equipment "one"'s Id in the Equipment field
    And I should see a display for WorkOrder with production work order "one"'s Id
    And I should see a display for WorkDescription with production work order "one"'s ProductionWorkDescription
    And I should see a display for Equipment with equipment "one"'s Id
    And I should see a display for WorkOrderNotes with production work order "one"'s OrderNotes
    And I should see a display for EquipmentDescription with equipment "one"'s Description
    When I press "Save"
    Then I should see a validation message for AssetReliabilityTechnologyUsedType with "The Technology Used field is required."
    Then I should see a validation message for RepairCostNotAllowedToFail with "The Repair Cost If Not Allowed To Fail field is required."
    Then I should see a validation message for RepairCostAllowedToFail with "The Repair Cost If Allowed To Fail field is required."
    Then I should see a validation message for CostAvoidanceNote with "The CostAvoidanceNote field is required."
    When I select "Other" from the AssetReliabilityTechnologyUsedType dropdown
    And I enter "Testing123" into the CostAvoidanceNote field
    And I press "Save"
    Then I should see a validation message for TechnologyUsedNote with "The TechnologyUsedNote field is required."
    When I enter "Now we’re going to do the most human thing of all: attempt something futile with a ton of unearned confidence and fail spectacularly!" into the TechnologyUsedNote field
    And I enter 100 into the RepairCostNotAllowedToFail field 
    And I enter 200 into the RepairCostAllowedToFail field 
    And I press "Save"
    Then the currently shown Asset Reliability shall henceforth be known throughout the land as "Phillipe"
	And I should see a link to the Show page for production work order "one"
    And I should see a link to the Show page for equipment "one"
    And I should see a display for ProductionWorkOrder_OrderNotes with production work order "one"'s OrderNotes
    And I should see a display for ProductionWorkOrder_ProductionWorkDescription with production work order "one"'s ProductionWorkDescription
    And I should see a display for Employee with employee "one"
    And I should see a display for AssetReliabilityTechnologyUsedType with "Other"
    And I should see a display for TechnologyUsedNote with "Now we’re going to do the most human thing of all: attempt something futile with a ton of unearned confidence and fail spectacularly!"
    And I should see a display for RepairCostNotAllowedToFail with "100"
    And I should see a display for RepairCostAllowedToFail with "200"
    And I should see a display for CostAvoidance with "100"
    When I follow the Show link for production work order "one"
    And I click the "Equipment" tab
    Then the equipmentsTable table should have 2 rows
    When I click the "Asset Reliability" tab
    Then the assetReliabilitesTable table should have 2 rows
    And I should see the following values in the assetReliabilitesTable table
	| Employee | Cost Avoidance |
	| jay bob  | 0              |
	| jay bob  | 100            |

Scenario: user can edit a asset reliaility
    Given I am logged in as "user"
    And I am at the Edit page for asset reliability "one"
    Then I should see a display for WorkOrder with production work order "one"'s Id
    And I should see a display for WorkDescription with production work order "one"'s ProductionWorkDescription
    And I should see a display for Equipment with equipment "one"'s Id
    And I should see a display for WorkOrderNotes with production work order "one"'s OrderNotes
    And I should see a display for EquipmentDescription with equipment "one"'s Description
    When I enter "Testing123" into the CostAvoidanceNote field
    And I press "Save" 
    Then I should be at the Show page for asset reliability "one"
    And I should see a display for CostAvoidanceNote with "Testing123"

Scenario: user can search and get correct results
    Given I am logged in as "user"
    And I am at the Production/AssetReliability/Search page
    When I select equipment "one"'s Display from the Equipment dropdown
    And I press "Search"
    Then I should see a link to the Show page for asset reliability "one"
    When I visit the Production/AssetReliability/Search page
    And I select equipment "two"'s Display from the Equipment dropdown
    And I press "Search"
    Then I should see a link to the Show page for asset reliability "two"
    And I should not see a link to the Show page for asset reliability "one"

Scenario: user can delete an asset reliability
    Given I am logged in as "user"
    And I am at the Production/AssetReliability/Search page
    When I select equipment "one"'s Display from the Equipment dropdown
    And I press "Search"
    Then I should see a link to the Show page for asset reliability "one"
    When I visit the Production/AssetReliability/Show page for asset reliability "one"
    Then I should see the button "Delete"
    When I click ok in the dialog after pressing "Delete"
    Then I should be at the Production/AssetReliability/Search page
    When I press "Search"
    Then I should not see a link to the Show page for asset reliability "one"

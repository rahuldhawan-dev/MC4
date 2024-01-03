Feature: Vehicle 

Background:
	Given a user "user" exists with username: "user"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a facility "one" exists with operating center: "nj7"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	And a role "hydrant-useradmin" exists with action: "UserAdministrator", module: "FleetManagementGeneral", user: "user", operating center: "nj7"
	And a vehicle icon "one" exists
	And a vehicle assignment category "one" exists
	And a vehicle assignment justification "one" exists
	And a vehicle assignment status "one" exists
	And a vehicle accounting requirement "one" exists
	And a vehicle status "one" exists
	And a vehicle e z pass "one" exists 
	And an employee status "active" exists with description: "Active"
	And an employee "manager" exists with status: "active"
	And an employee "fleetcontactperson" exists with status: "active"
	And an employee "primarydriver" exists with operating center: "nj7"
	And a vehicle department "one" exists
	And a vehicle primary use "one" exists
	And a vehicle service company "one" exists
	And a vehicle fuel type "one" exists
	And a vehicle ownership type "one" exists
	And a vehicle "replacement" exists

Scenario: User should see validation messages 
	Given I am logged in as "user"
	And I am at the FleetManagement/Vehicle/New page 
	When I press Save
	Then I should see a validation message for Flag with "The Flag field is required."
	And I should see a validation message for PoolUse with "The PoolUse field is required."

Scenario: User can add a new vehicle
	Given I am logged in as "user"
	And I am at the FleetManagement/Vehicle/New page 
	When I select "Yes" from the Flag dropdown
	And I select vehicle icon "one" from the VehicleIcon dropdown
	And I select vehicle assignment category "one" from the AssignmentCategory dropdown
	And I select vehicle assignment justification "one" from the AssignmentJustification dropdown
	And I select vehicle assignment status "one" from the AssignmentStatus dropdown
	And I select vehicle accounting requirement "one" from the AccountingRequirement dropdown
	And I select vehicle status "one" from the Status dropdown
	And I select "Yes" from the PoolUse dropdown
	And I enter "1/1/2016" into the DateRequisitioned field
	And I enter "2/2/2016" into the DateOrdered field
	And I enter "req num" into the RequisitionNumber field
	And I enter "3/3/2016" into the DateInService field
	And I enter "4/4/2016" into the DateRetired field
	And I enter "1234567890" into the VehicleIdentificationNumber field
	And I enter "222" into the ARIVehicleNumber field
	And I enter "333" into the DecalNumber field
	And I enter "444" into the PlateNumber field
	And I enter "555" into the NedapSerialNumber field
	And I check the LogoWaiver field
	And I select "Yes" from the Upbranded dropdown
	And I enter "666" into the VehicleLabel field
	And I select vehicle e z pass "one" from the EZPass dropdown
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I enter "7" into the District field
	And I enter "888" into the WBSNumber field
	And I select facility "one" from the Facility dropdown
	And I select employee "manager"'s Description from the Manager dropdown
	And I select employee "fleetcontactperson"'s Description from the FleetContactPerson dropdown
	And I select vehicle department "one" from the Department dropdown
	And I select vehicle primary use "one" from the PrimaryVehicleUse dropdown
	And I enter "2007" into the ModelYear field
	And I enter "Toyota" into the Make field
	And I enter "Solara" into the ModelType field
	And I enter "3.5" into the GVW field
	And I select vehicle service company "one" from the ServiceCompany dropdown
	And I enter "uh" into the AssetDetails field
	And I select vehicle fuel type "one" from the FuelType dropdown
	And I enter "8/8/2016" into the RegistrationRenewalDate field
	And I enter "1000" into the RegistrationAnnualCost field
	And I select vehicle ownership type "one" from the OwnershipType dropdown
	And I enter "1" into the OriginalAssetValueCapCost field
	And I enter "2017" into the PlannedReplacementYear field
	And I enter "uh" into the AlvId field
	And I enter "4413431" into the ToughbookSerialNumber field
	And I enter "Sure" into the ToughbookMount field
	And I enter "53" into the FuelCardNumber field
	And I select "Yes" from the MileageTracked dropdown
	And I select vehicle "replacement" from the ReplacementVehicle dropdown
	And I enter "some comments" into the Comments field
	And I press Save
	Then I should see a display for Flag with "Yes"
	And I should see a display for VehicleIcon with vehicle icon "one"
	And I should see a display for AssignmentCategory with vehicle assignment category "one"
	And I should see a display for AssignmentJustification with vehicle assignment justification "one"
	And I should see a display for AssignmentStatus with vehicle assignment status "one"
	And I should see a display for AccountingRequirement with vehicle accounting requirement "one"
	And I should see a display for Status with vehicle status "one"
	And I should see a display for PoolUse with "Yes"
	And I should see a display for DateRequisitioned with "1/1/2016"
	And I should see a display for DateOrdered with "2/2/2016" 
	And I should see a display for RequisitionNumber with "req num" 
	And I should see a display for DateInService with "3/3/2016" 
	And I should see a display for DateRetired with "4/4/2016"
	And I should see a display for VehicleIdentificationNumber with "1234567890"
	And I should see a display for ARIVehicleNumber with "222" 
	And I should see a display for DecalNumber with "333"
	And I should see a display for PlateNumber with "444"
	And I should see a display for NedapSerialNumber with "555"
	And I should see a display for LogoWaiver with "Yes"
	And I should see a display for Upbranded with "Yes"
	And I should see a display for VehicleLabel with "666"
	And I should see a display for EZPass with vehicle e z pass "one"
	And I should see a display for OperatingCenter with operating center "nj7"
	And I should see a display for District with "7"
	And I should see a display for WBSNumber with "888"
	And I should see a display for Facility with facility "one"
	And I should see a display for Manager with employee "manager"
	And I should see a display for FleetContactPerson with employee "fleetcontactperson"
	And I should see a display for Department with vehicle department "one"
	And I should see a display for PrimaryVehicleUse with vehicle primary use "one" 
	And I should see a display for ModelYear with "2007"
	And I should see a display for Make with "Toyota" 
	And I should see a display for Model with "Solara" 
	And I should see a display for GVW with "3.5"
	And I should see a display for ServiceCompany with vehicle service company "one"
	And I should see a display for AssetDetails with "uh"
	And I should see a display for FuelType with vehicle fuel type "one"
	And I should see a display for RegistrationRenewalDate with "8/8/2016"
	And I should see a display for RegistrationAnnualCost with "1000"
	And I should see a display for OwnershipType with vehicle ownership type "one"
	And I should see a display for OriginalAssetValueCapCost with "1" 
	And I should see a display for PlannedReplacementYear with "2017"
	And I should see a display for AlvId with "uh"
	And I should see a display for ToughbookSerialNumber with "4413431"
	And I should see a display for ToughbookMount with "Sure"
	And I should see a display for FuelCardNumber with "53" 
	And I should see a display for MileageTracked with "Yes"
	And I should see a display for ReplacementVehicle with vehicle "replacement" 
	And I should see a display for Comments with "some comments"
	And I should see a display for CreatedAt with today's date
Feature: InvestmentProject

Background:
	Given an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsburghy"
	And a user "user" exists with username: "user"
	And a role "project-useradmin" exists with action: "UserAdministrator", module: "FieldServicesProjects", user: "user" 
	And an investment project category "one" exists
	And an investment project asset category "one" exists
	And an investment project approval status "one" exists
	And an investment project status "one" exists
	And an investment project phase "one" exists
	And an employee "one" exists
	And an employee "two" exists
	And an employee "three" exists
	And an employee "four" exists
	And a contractor "one" exists
	And a contractor "two" exists 
	And public water supply statuses exist
	And a public water supply "one" exists
	And a town "one" exists with name: "Loch Arbour"
	And operating center: "nj7" exists in town: "one" with abbreviation: "LA"
	And a facility "one" exists with facility id: "NJSB-01", town: "one", operating center: "nj7"
	And a role "roleFacilityReadNj7" exists with action: "Read", module: "ProductionFacilities", user: "user", operating center: "nj7"
	
Scenario: User can add a new investment project 
	Given I am logged in as "user"
	And I am at the ProjectManagement/InvestmentProject/New page
	When I select operating center "nj7" from the OperatingCenter dropdown
	And I select "2016" from the CPSReferenceYear dropdown
	And I enter Q214 into the CPSPriorityNumber field 
	And I select investment project category "one" from the ProjectCategory dropdown
	And I enter "Neato" into the ProjectNumber field
	And I select investment project approval status "one" from the ApprovalStatus dropdown
	And I select investment project status "one" from the ProjectStatus dropdown
	And I enter "a whole lot of nothing" into the ProjectDescription field
	And I enter "Mark Sumner's Double Dare Obstacle Course" into the ProjectObstacles field
	And I enter "injuries most likely" into the ProjectRisks field
	And I enter "uuh" into the ProjectApproach field
	And I enter 32 into the DurationLandAcquisitionInMonths field
	And I enter 12 into the DurationPermitDesignInMonths field
	And I enter 44 into the DurationConstructionInMonths field
	And I enter 9123 into the ProjectDurationMonths field
	And I enter 1/2/3245 into the TargetStartDate field
	And I enter 2/3/4567 into the TargetEndDate field
	And I enter 1/1/2001 into the ForecastedInServiceDate field
	And I enter 2/2/2002 into the ControlDate field
	And I enter 12345 into the EstimatedProjectCost field
	And I enter 23456 into the FinalProjectCost field
	And I select employee "one"'s Description from the AssetOwner dropdown
	And I select employee "two"'s Description from the ProjectManager dropdown
	And I select employee "three"'s Description from the ConstructionManager dropdown
	And I select employee "four"'s Description from the CompanyInspector dropdown
	And I enter "Bill Nye" into the ContractedInspector field
	And I select contractor "one" from the EngineeringContractor dropdown
	And I select contractor "two" from the ConstructionContractor dropdown
	And I select public water supply "one" from the PublicWaterSupply dropdown
	And I select facility "one"'s Description from the Facility dropdown
	And I enter "Fake Street" into the StreetName field
	And I select town "one" from the Town dropdown
	And I enter "4/4/2004" into the CIMDate field
	And I select investment project phase "one" from the Phase dropdown
	And I select "Yes" from the ProjectFlagged dropdown
	And I select "No" from the CurrentYearActive dropdown
	And I select "Yes" from the BulkSale dropdown
	And I select "No" from the RateCase dropdown
	And I select "Yes" from the MISDates dropdown
	And I select "No" from the COE dropdown
	And I enter "5/5/2005" into the PPDate field
	And I enter "4" into the PPScore field
	And I enter "99999" into the PPWorkOrder field
	And I select investment project asset category "one" from the AssetCategory dropdown
	And I press Save
	Then I should see a display for OperatingCenter with "NJ7 - Shrewsburghy"
	And I should see a display for CPSReferenceYear with "2016"
	And I should see a display for CPSPriorityNumber with "Q214"
	And I should see a display for ProjectCategory with investment project category "one"
	And I should see a display for ProjectNumber with "Neato"
	And I should see a display for ApprovalStatus with investment project approval status "one"
	And I should see a display for ProjectStatus with investment project status "one"
	And I should see a display for ProjectDescription with "a whole lot of nothing"
	And I should see a display for ProjectObstacles with "Mark Sumner's Double Dare Obstacle Course"
	And I should see a display for ProjectRisks with "injuries most likely"
	And I should see a display for ProjectApproach with "uuh"
	And I should see a display for DurationLandAcquisitionInMonths with "32"
	And I should see a display for DurationPermitDesignInMonths with "12"
	And I should see a display for DurationConstructionInMonths with "44"
	And I should see a display for ProjectDurationMonths with "9123"
	And I should see a display for TargetStartDate with "1/2/3245"
	And I should see a display for TargetEndDate with "2/3/4567"
	And I should see a display for ForecastedInServiceDate with "1/1/2001"
	And I should see a display for ControlDate with "2/2/2002"
	And I should see a display for EstimatedProjectCost with "$12,345.00"
	And I should see a display for FinalProjectCost with "$23,456.00"
	And I should see a display for AssetOwner with employee "one"
	And I should see a display for ProjectManager with employee "two"
	And I should see a display for ConstructionManager with employee "three"
	And I should see a display for CompanyInspector with employee "four"
	And I should see a display for ContractedInspector with "Bill Nye"
	And I should see a display for EngineeringContractor with contractor "one"
	And I should see a display for ConstructionContractor with contractor "two"
	And I should see a display for PublicWaterSupply with public water supply "one"
	And I should see a display for Facility with facility "one"
	And I should see a display for StreetName with "Fake Street"
	And I should see a display for Town with town "one"
	And I should see a display for CIMDate with "4/4/2004"
	And I should see a display for Phase with investment project phase "one"
	And I should see a display for ProjectFlagged with "Yes"
	And I should see a display for CurrentYearActive with "No"
	And I should see a display for BulkSale with "Yes"
	And I should see a display for RateCase with "No"
	And I should see a display for MISDates with "Yes"
	And I should see a display for COE with "No"
	And I should see a display for PPDate with "5/5/2005"
	And I should see a display for PPScore with "4"
	And I should see a display for PPWorkOrder with "99999"
	And I should see a display for AssetCategory with investment project asset category "one"
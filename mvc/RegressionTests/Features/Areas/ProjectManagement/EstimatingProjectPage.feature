Feature: Estimating Project Page

Background: data
    Given an operating center "nj7" exists with opcode: "NJ7"
    And an operating center "nj4" exists with opcode: "NJ4"
	And a role "roleReadNj7" exists with action: "Read", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleEditNj7" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleAddNj7" exists with action: "Add", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleDeleteNj7" exists with action: "Delete", module: "FieldServicesEstimatingProjects", operating center: "nj7"
	And a role "roleReadNj4" exists with action: "Read", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleEditNj4" exists with action: "Edit", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleAddNj4" exists with action: "Add", module: "FieldServicesEstimatingProjects", operating center: "nj4"
	And a role "roleDeleteNj4" exists with action: "Delete", module: "FieldServicesEstimatingProjects", operating center: "nj4"
    And a town "nj4ton" exists
    And a town "nj4ford" exists
    And a town "nj7burg" exists
    And operating center: "nj7" exists in town: "nj7burg"
    And operating center: "nj4" exists in town: "nj4ton"
    And operating center: "nj4" exists in town: "nj4ford"
    And an admin user "admin" exists with username: "admin"
    And a user "user" exists with username: "user"
	And a user "invalid" exists with username: "invalid"
    And a user "read-only-nj7" exists with username: "read-only-nj7", roles: roleReadNj7
    And a user "user-admin-nj7" exists with username: "user-admin-nj7", roles: roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
    And a user "read-only-nj4" exists with username: "read-only-nj4", roles: roleReadNj4
    And a user "user-admin-nj4" exists with username: "user-admin-nj4", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4
    And a user "user-admin-both" exists with username: "user-admin-both", roles: roleReadNj4;roleEditNj4;roleAddNj4;roleDeleteNj4;roleReadNj7;roleEditNj7;roleAddNj7;roleDeleteNj7
	And an estimating project type "framework" exists with description: "Framework"
    And an estimating project type "non-framework" exists with description: "Non-Framework"
    And an estimating project "one" exists with project type: "framework", operating center: "nj7", town: "nj7burg"
    And an estimating project "two" exists with project type: "non-framework", operating center: "nj7", town: "nj7burg"
    And an estimating project "three" exists with project type: "non-framework", operating center: "nj4", town: "nj4ton"
    And an estimating project "four" exists with project type: "non-framework", operating center: "nj4", town: "nj4ton"
    And an estimating project "five" exists with project type: "framework", operating center: "nj4", town: "nj4ford"
    And an employee "one" exists with operating center: "nj7"
    And an employee "two" exists with operating center: "nj4"
	And a material "one" exists with description: "Mercury", part number: "Hg"
	And a material "two" exists with description: "Silver", part number: "Ag"
    And a material "three" exists with description: "Gold", part number: "Au"
    And an operating center stocked material "one" exists with operating center: "nj7", material: "one"	
    And an operating center stocked material "two" exists with operating center: "nj7", material: "two"	
    And an operating center stocked material "three" exists with operating center: "nj4", material: "three"	
    And a contractor labor cost "one" exists with operating centers: nj7,nj4
    And an asset type "one" exists
    And a contractor "nj7" exists with framework operating center: "nj7"
    And a contractor "nj4" exists with framework operating center: "nj4"
    And a contractor "nj7-non-framework" exists with operating center: "nj7"

Scenario: a user without the role cannot access the estimating project page
    Given I am logged in as "invalid"
    When I visit the /ProjectManagement/EstimatingProject/Search page
    Then I should see the missing role error
    When I visit the /ProjectManagement/EstimatingProject/Index page
    Then I should see the missing role error
    When I visit the /ProjectManagement/EstimatingProject/New page
    Then I should see the missing role error
    When I visit the Show page for estimating project: "one"
    Then I should see the missing role error
	When I try to visit the Edit page for estimating project: "one" expecting an error
    Then I should see the missing role error

Scenario: admin can search by various search fields
    Given I am logged in as "admin"
    When I visit the /ProjectManagement/EstimatingProject/Search page
    And I select estimating project type "framework"'s Description from the ProjectType dropdown
    And I press Search
    Then I should see a link to the Show page for estimating project: "one"
    And I should not see a link to the Show page for estimating project: "three"
    And I should see a link to the Show page for estimating project: "five"
    And I should not see a link to the Show page for estimating project: "two"
    And I should not see a link to the Show page for estimating project: "four"
    When I visit the /ProjectManagement/EstimatingProject/Search page
    And I enter estimating project "one"'s ProjectNumber into the ProjectNumber field
    And I press Search
    Then I should see a link to the Show page for estimating project: "one"
    And I should not see a link to the Show page for estimating project: "two"
    And I should not see a link to the Show page for estimating project: "three"
    And I should not see a link to the Show page for estimating project: "four"
    And I should not see a link to the Show page for estimating project: "five"
    When I visit the /ProjectManagement/EstimatingProject/Search page
    And I enter estimating project "two"'s ProjectName into the ProjectName field
    And I press Search
    Then I should not see a link to the Show page for estimating project: "one"
    And I should see a link to the Show page for estimating project: "two"
    And I should not see a link to the Show page for estimating project: "three"
    And I should not see a link to the Show page for estimating project: "four"
    And I should not see a link to the Show page for estimating project: "five"
    When I visit the /ProjectManagement/EstimatingProject/Search page
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I press Search
    Then I should see a link to the Show page for estimating project: "one"
    And I should see a link to the Show page for estimating project: "two"
    And I should not see a link to the Show page for estimating project: "three"
    And I should not see a link to the Show page for estimating project: "four"
    And I should not see a link to the Show page for estimating project: "five"
    When I visit the /ProjectManagement/EstimatingProject/Search page
    And I select operating center "nj4" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj4ford" from the Town dropdown
    And I press Search
    Then I should not see a link to the Show page for estimating project: "one"
    And I should not see a link to the Show page for estimating project: "two"
    And I should not see a link to the Show page for estimating project: "three"
    And I should not see a link to the Show page for estimating project: "four"
    And I should see a link to the Show page for estimating project: "five"

Scenario: admin can create new estimating project
    Given I am logged in as "admin"
	When I visit the /ProjectManagement/EstimatingProject/New page
    And I press Save
    Then I should be at the ProjectManagement/EstimatingProject/New page
    And I should see the validation message "The Preliminary File Number field is required."
    And I should see the validation message "The ProjectName field is required."
    And I should see the validation message "The ProjectType field is required."
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "The Street field is required."
    And I should see the validation message "The OverheadPercentage field is required."
    And I should see the validation message "The ContingencyPercentage field is required."
    And I should see the validation message "The Estimator field is required."
    And I should see the validation message "The EstimateDate field is required."
	When I enter "8765309" into the ProjectNumber field
    And I enter "Some Project" into the ProjectName field
    And I select estimating project type "framework"'s Description from the ProjectType dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    Then I should not see contractor "nj4"'s Description in the Contractor dropdown
    And I should not see contractor "nj7-non-framework"'s Description in the Contractor dropdown
    When I select contractor "nj7" from the Contractor dropdown
    And I select town "nj7burg" from the Town dropdown
    And I enter "Some Street" into the Street field
    And I enter "10" into the OverheadPercentage field
    And I enter "20" into the ContingencyPercentage field
    And I enter "100" into the LumpSum field
    And I select employee "one"'s Description from the Estimator dropdown
    And I enter today's date into the EstimateDate field
    And I press Save
    And I wait for the page to reload
    Then the currently shown estimating project will now be referred to as "six"
    And I should be at the Show page for estimating project: "six"

Scenario: admin can edit estimating project
    Given I am logged in as "admin"
    When I visit the Edit page for estimating project: "two"
    And I enter "asdf" into the Description field
    And I press Save
    Then I should be at the Show page for estimating project: "two"
    And I should see a display for Description with "asdf"
    When I visit the Edit page for estimating project: "two"
    And I enter "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789" into the Description field
    And I press Save
    Then I should be at the Show page for estimating project: "two"
    And I should see a display for Description with "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"

Scenario: admin can add edit and remove other costs to and from estimating project
	Given an asset type "two" exists
    And I am logged in as "admin"
    When I visit the Show page for estimating project: "two"
    And I click the "Other Costs" tab
    And I press "Add New Cost"
    And I press "Save Other Cost"
    Then I should see the validation message "The Quantity field is required."
    And I should see the validation message "The Description field is required."
    And I should see the validation message "The Cost field is required."
    And I should see the validation message "The AssetType field is required."
    When I enter 2 into the Quantity field in the otherCostForm form
    And I enter "this is my description, more than 50 charachers in length and less than 250 characters in length." into the Description field in the otherCostForm form
    And I enter 6.66 into the Cost field in the otherCostForm form
    And I select asset type "one" from the AssetType dropdown in the otherCostForm form
    And I press "Save Other Cost"
    Then I should be at the Show page for estimating project: "two"
    And I should see "2" in the table otherCostsTable's "Quantity" column
    And I should see "this is my description, more than 50 charachers in length and less than 250 characters in length." in the table otherCostsTable's "Description" column
    And I should see "6.66" in the table otherCostsTable's "Cost" column
    And I should see "13.32" in the table otherCostsTable's "Total Cost" column
	When I click the "Other Costs" tab
	And I click the "Edit" link in the 1st row of otherCostsTable
    And I enter "this is my description, more than 50 charachers in length and less than 250 characters in length." into the Description field
    And I enter 5.55 into the Cost field
    And I select asset type "two" from the AssetType dropdown
    And I press Save
    Then I should be at the Show page for estimating project: "two"
	When I click the "Other Costs" tab
    Then I should see "2" in the table otherCostsTable's "Quantity" column
    And I should see "this is my description, more than 50 charachers in length and less than 250 characters in length." in the table otherCostsTable's "Description" column
    And I should see "5.55" in the table otherCostsTable's "Cost" column
    And I should see "11.10" in the table otherCostsTable's "Total Cost" column
    When I click ok in the dialog after pressing "Remove"
    Then I should be at the Show page for estimating project: "two"
    And I should not see "2" in the table otherCostsTable's "Quantity" column
    And I should not see "blah blah" in the table otherCostsTable's "Description" column
    And I should not see "6.66" in the table otherCostsTable's "Cost" column
    And I should not see "13.32" in the table otherCostsTable's "Total Cost" column

Scenario: admin can add and remove materials to and from an estimating project
	Given I am logged in as "admin"
	When I visit the Show page for estimating project: "two"
	And I click the "Materials" tab
	And I press "Add New Material"
	And I press "Save Material"
	Then I should see the validation message "The Quantity field is required."
	And I should see the validation message "The Material field is required."
    And I should see the validation message "The AssetType field is required."
	When I enter 2 into the Quantity field in the materialForm form
	And I enter "Hg" and select "Hg - Mercury" from the Material combobox
    And I select "Equipment" from the AssetType dropdown
	And I press "Save Material"
	Then I should be at the Show page for estimating project: "two"
	And I should see "2" in the table materialsTable's "Quantity" column
	And I should see "Mercury" in the table materialsTable's "Material" column
    And I should see "Equipment" in the table materialsTable's "Asset Type" column
	When I click the "Materials" tab
	And I press "Add New Material"
	And I enter 6 into the Quantity field in the materialForm form
	And I enter "Ag" and select "Ag - Silver" from the Material combobox
	And I select "Equipment" from the AssetType dropdown
	And I press "Save Material"
	Then I should be at the Show page for estimating project: "two"
	When I click the "Materials" tab 
	And I click the "Remove Material" button in the 2nd row of materialsTable and then click ok in the confirmation dialog
	Then I should be at the Show page for estimating project: "two"
	And I should not see "6" in the table materialsTable's "Quantity" column
	And I should not see "Silver" in the table materialsTable's "Material" column
	And I should see "2" in the table materialsTable's "Quantity" column
	And I should see "Mercury" in the table materialsTable's "Material" column

Scenario: admin can edit materials
	Given an asset type "facility" exists with description: "facility"
	And I am logged in as "admin"
	When I visit the Show page for estimating project: "two"
	And I click the "Materials" tab
	And I press "Add New Material"
	And I enter 2 into the Quantity field in the materialForm form
	And I enter "Hg" and select "Hg - Mercury" from the Material combobox
    And I select "Equipment" from the AssetType dropdown
	And I press "Save Material"
	Then I should be at the Show page for estimating project: "two"
	And I should see "2" in the table materialsTable's "Quantity" column
	When I click the "Materials" tab
	And I click the "Edit" link in the 1st row of materialsTable
	And I select asset type "facility" from the AssetType dropdown
	And I enter "Ag" and select "Ag - Silver" from the Material combobox
	And I enter 3 into the Quantity field 
	And I press Save
	Then I should be at the Show page for estimating project: "two"
	When I click the "Materials" tab
	Then I should see the following values in the materialsTable table
         | Asset Type | Quantity | Material    |
         | Facility   | 3        | Ag - Silver |

Scenario: admin can add and remove contractor labor costs to and from an estimating project
    Given I am logged in as "admin"
	When I visit the Show page for estimating project: "one"
    And I click the "Contractor Installation Costs" tab
	And I press "Add New Contractor Labor Cost"
    And I press "Save Contractor Labor Cost"
    Then I should see the validation message "The Quantity field is required."
    And I should see the validation message "The ContractorLaborCost field is required."
    And I should see the validation message "The AssetType field is required."
    When I enter 3 into the Quantity field in the contractorLaborCostForm form
    And I enter contractor labor cost "one"'s StockNumber and select contractor labor cost "one"'s Description from the ContractorLaborCost combobox
    And I select asset type "one" from the AssetType dropdown in the contractorLaborCostForm form
    And I press "Save Contractor Labor Cost"
	Then I should be at the Show page for estimating project: "one"
    When I click the "Contractor Installation Costs" tab
    Then I should see "3" in the table contractorLaborCostsTable's "Quantity" column
    And I should see contractor labor cost "one"'s Description in the table contractorLaborCostsTable's "Description" column
    And I should see contractor labor cost "one"'s Cost as currency in the table contractorLaborCostsTable's "Cost" column
	When I click ok in the dialog after pressing "Remove Contractor Labor Cost"
	Then I should be at the Show page for estimating project: "one"
    When I click the "Contractor Installation Costs" tab
    Then I should not see "3" in the table contractorLaborCostsTable's "Quantity" column
    And I should not see contractor labor cost "one"'s Description in the table contractorLaborCostsTable's "Description" column
    And I should not see contractor labor cost "one"'s Cost as currency in the table contractorLaborCostsTable's "Cost" column

	Scenario: admin can edit contractor installation costs
	Given an asset type "facility" exists with description: "facility"
	And a contractor labor cost "two" exists with operating centers: nj7,nj4
	And I am logged in as "admin"
	When I visit the Show page for estimating project: "one"
	And I click the "Contractor Installation Costs" tab
	And I press "Add New Contractor Labor Cost"
    And I enter contractor labor cost "one"'s StockNumber and select contractor labor cost "one"'s Description from the ContractorLaborCost combobox
    And I select asset type "one" from the AssetType dropdown in the contractorLaborCostForm form
	And I enter 2 into the Quantity field in the contractorLaborCostForm form
	And I press "Save Contractor Labor Cost"
	Then I should be at the Show page for estimating project: "one"
	When I click the "Contractor Installation Costs" tab
	And I click the "Edit" link in the 1st row of contractorLaborCostsTable 
	And I select asset type "facility" from the AssetType dropdown
	And I enter contractor labor cost "two"'s StockNumber and select contractor labor cost "two"'s Description from the ContractorLaborCost combobox
	And I enter 3 into the Quantity field
	And I press Save
	Then I should be at the Show page for estimating project: "one"
	And I should see the following values in the contractorLaborCostsTable table
	| Quantity | Description                               | Asset Type |
	| 3        | contractor labor cost: "two"'s Description | Facility   |

Scenario: admin can add edit and remove company labor costs to and from an estimating project
	Given a company labor cost "one" exists with description: "asbuilt drawings", cost: "10.50", unit: "Ea"
	And a company labor cost "two" exists with description: "inuit drawings", cost: "100", unit: "Ea"
	And an asset type "two" exists
	And I am logged in as "admin"
	When I visit the Show page for estimating project: "two"
	And I click the "Company Labor Costs" tab
	And I press "Add New Company Labor Cost"
	And I press "Save Company Labor Cost"
	Then I should see the validation message "The CompanyLaborCost field is required."
	And I should see the validation message "The Quantity field is required."
    And I should see the validation message "The AssetType field is required."	
	When I select company labor cost "one" from the CompanyLaborCost dropdown
	And I enter "1" into the Quantity field in the companyLaborCostForm form
    And I select asset type "one" from the AssetType dropdown in the companyLaborCostForm form
	And I press "Save Company Labor Cost"
	And I wait for the page to reload
	Then I should see "asbuilt drawings" in the table companyLaborCostsTable's "Company Labor Cost" column
	And I should see "1" in the table companyLaborCostsTable's "Quantity" column
	When I click the "Company Labor Costs" tab 
	And I click the "Edit" link in the 1st row of companyLaborCostsTable
    And I select asset type "two" from the AssetType dropdown
	And I select company labor cost "two" from the CompanyLaborCost dropdown
	And I enter "2" into the Quantity field
	And I press Save
	And I wait for the page to reload
	When I click the "Company Labor Costs" tab
	Then I should see "inuit drawings" in the table companyLaborCostsTable's "Company Labor Cost" column
	And I should see "2" in the table companyLaborCostsTable's "Quantity" column
	And I should see asset type "two"'s Description in the table companyLaborCostsTable's "Asset Type" column
	And I should see "$200.00" in the table companyLaborCostsTable's "Total Cost" column
	When I click ok in the dialog after pressing "Remove Company Labor Cost"
	Then I should be at the Show page for estimating project: "two"
	And I should not see "1" in the table companyLaborCostsTable's "Quantity" column
	And I should not see "asbuilt drawings" in the table companyLaborCostsTable's "Company Labor Cost" column

Scenario: admin can add edit and remove permits to and from an estimating project
	Given a permit type "one" exists with description: "State Permit"
	And a permit type "two" exists with description: "County Permit"
	And an asset type "two" exists
	And I am logged in as "admin"
	When I visit the Show page for estimating project: "one"
	And I click the "Permits" tab
	And I press "Add New Permit"
	And I press "Save Permit"
	Then I should see the validation message "The PermitType field is required."
	And I should see the validation message "The Quantity field is required."
	And I should see the validation message "The Cost field is required."
    And I should see the validation message "The AssetType field is required."
	When I select permit type "one" from the PermitType dropdown
	And I enter "2" into the Quantity field in the permitForm form
	And I enter "50" into the Cost field in the permitForm form
    And I select asset type "one" from the AssetType dropdown in the permitForm form
	And I press "Save Permit"
	And I wait for the page to reload
	Then I should see "State Permit" in the table permitsTable's "Permit Type" column
	And I should see "2" in the table permitsTable's "Quantity" column
	And I should see "$50.00" in the table permitsTable's "Cost" column
	And I should see "$100.00" in the table permitsTable's "Total Cost" column
	When I click the "Permits" tab
	And I click the "Edit" link in the 1st row of permitsTable
	And I enter "3" into the Quantity field
	And I enter "55" into the Cost field
    And I select asset type "two" from the AssetType dropdown
	And I select permit type "two" from the PermitType dropdown
	And I press Save
	When I click the "Permits" tab
	Then I should see "County Permit" in the table permitsTable's "Permit Type" column
	And I should see "3" in the table permitsTable's "Quantity" column
	And I should see "$55.00" in the table permitsTable's "Cost" column
	And I should see "$165.00" in the table permitsTable's "Total Cost" column	
	When I click ok in the dialog after pressing "Remove Permit"
	Then I should not see "State Permit" in the table permitsTable's "Permit Type" column
	And I should not see "2" in the table permitsTable's "Quantity" column
	And I should not see "$50.00" in the table permitsTable's "Cost" column
	And I should not see "$100.00" in the table permitsTable's "Total Cost" column

Scenario: admin can delete estimating project
    Given I am logged in as "admin"
    When I visit the Show page for estimating project: "five"
    And I click ok in the dialog after pressing "Delete"
    Then I should be at the ProjectManagement/EstimatingProject/Search page
    When I press Search
    Then I should be at the ProjectManagement/EstimatingProject page
    And I should see a link to the Show page for estimating project: "one"
    And I should see a link to the Show page for estimating project: "two"
    And I should see a link to the Show page for estimating project: "three"
    And I should see a link to the Show page for estimating project: "four"
    And I should not see a link to the Show page for estimating project: "five"

Scenario: user with read rights can search and view but not edit or add
    Given I am logged in as "read-only-nj7"
    When I visit the /ProjectManagement/EstimatingProject/Search page
    And I press Search
    Then I should be at the ProjectManagement/EstimatingProject page
    And I should see a link to the Show page for estimating project: "one"
    And I should see a link to the Show page for estimating project: "two"
    And I should not see a link to the Show page for estimating project: "three"
    And I should not see a link to the Show page for estimating project: "four"
    And I should not see a link to the Show page for estimating project: "five"
	When I follow the Show link for estimating project "two"
    Then I should be at the Show page for estimating project: "two"
    And I should not see a link to the Edit page for estimating project "two"
    When I try to visit the Edit page for estimating project: "two" expecting an error
    Then I should see the missing role error
    When I visit the /ProjectManagement/EstimatingProject/New page
    Then I should see the missing role error

Scenario: user with add rights can add an estimating project for operating centers they have access to
    Given I am logged in as "user-admin-nj7"
	When I visit the /ProjectManagement/EstimatingProject/New page
	Then I should not see operating center "nj4"'s Description in the OperatingCenter dropdown
    And I should not see employee "two"'s Description in the Estimator dropdown
	When I enter "8765309" into the ProjectNumber field
    And I enter "Some Project" into the ProjectName field
    And I select estimating project type "non-framework"'s Description from the ProjectType dropdown
    And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    And I enter "Some Street" into the Street field
    And I enter "10" into the OverheadPercentage field
    And I enter "20" into the ContingencyPercentage field
    And I enter "100" into the LumpSum field
    And I select employee "one"'s Description from the Estimator dropdown
    And I enter today's date into the EstimateDate field
    And I press Save
    And I wait for the page to reload
    Then the currently shown estimating project will now be referred to as "six"
    And I should be at the Show page for estimating project: "six"

Scenario: user with edit rights can edit an estimating project for operating centers they have access to
    Given I am logged in as "user-admin-nj4"
    When I try to visit the Edit page for estimating project: "one" expecting an error
    Then I should see a 404 error message
	When I visit the Edit page for estimating project: "three"
	Then I should not see operating center "nj7"'s Description in the OperatingCenter dropdown
    And I should not see employee "one"'s Description in the Estimator dropdown
    When I enter "asdf" into the Description field
    And I select employee "two"'s Description from the Estimator dropdown
    And I press Save
    Then I should be at the Show page for estimating project: "three"
    And I should see a display for Description with "asdf"

Scenario: non-framework estimating projects should behave differently
    Given I am logged in as "user-admin-nj7"
	When I visit the /ProjectManagement/EstimatingProject/New page
	And I enter "8765309" into the ProjectNumber field
    And I enter "Some Project" into the ProjectName field
    And I select estimating project type "framework"'s Description from the ProjectType dropdown
    Then I should not see the LumpSum field
    When I select estimating project type "non-framework"'s Description from the ProjectType dropdown 
	And I select operating center "nj7" from the OperatingCenter dropdown
    And I wait for ajax to finish loading
    And I select town "nj7burg" from the Town dropdown
    And I enter "Some Street" into the Street field
    And I enter "10" into the OverheadPercentage field
    And I enter "20" into the ContingencyPercentage field
    And I select employee "one"'s Description from the Estimator dropdown
    And I enter today's date into the EstimateDate field
    And I press Save
    And I wait for the page to reload
    Then the currently shown estimating project will now be referred to as "six"
    And I should be at the Show page for estimating project: "six"

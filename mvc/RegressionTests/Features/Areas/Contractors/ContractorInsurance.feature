Feature: ContractorInsurance

Background:
	Given a user "user" exists with username: "user"
	And a role "contractorsgeneral-useradmin" exists with action: "UserAdministrator", module: "ContractorsGeneral", user: "user"
    And a contractor "one" exists with name: "one"
	And a contractor "cool guy" exists with name: "cool guy"
	And a contractor insurance minimum requirement "one" exists with description: "TRANSPORTATION LEVEL 1"
	And a contractor insurance "two" exists with Contractor: "cool guy", ContractorInsuranceMinimumRequirement: "one", InsuranceProvider: "Boynton & Boynton", MeetsCurrentContractualLimits: "true", PolicyNumber: "BKW53671074", EffectiveDate: "08/11/2020", TerminationDate: "10/10/2022"

Scenario: Admin user adds a new contractor insurance
	Given I am logged in as "user"
	And I am at the Contractors/ContractorInsurance/New page
	When I press Save
	Then I should see a validation message for Contractor with "The Contractor field is required."
	And I should see a validation message for ContractorInsuranceMinimumRequirement with "The Coverage field is required."
	When I select contractor "one" from the Contractor dropdown
	And I select contractor insurance minimum requirement "one" from the ContractorInsuranceMinimumRequirement dropdown
	And I enter "Construction Risk Partners, LLC" into the InsuranceProvider field
	And I enter "4029254852" into the PolicyNumber field
	And I check the MeetsCurrentContractualLimits field
	And I enter 3/17/2022 into the EffectiveDate field
	And I enter 3/22/2022 into the TerminationDate field
	And I press Save
	Then I should see a display for Contractor with "one"
	And I should see a display for InsuranceProvider with "Construction Risk Partners, LLC"
	And I should see a display for PolicyNumber with "4029254852"
	And I should see a display for MeetsCurrentContractualLimits with "Yes"
	And I should see a display for EffectiveDate with "3/17/2022" 
	And I should see a display for TerminationDate with "3/22/2022"

Scenario: Admin user can search and view contractor insurances
	Given I am logged in as "user"
	When I visit the Contractors/ContractorInsurance/Search page
	And I press Search
	And I wait for the page to reload
	Then I should see a link to the Show page for contractor insurance "two"
	And I should see the following values in the contractorInsurancesTable table
	| Contractor | Insurance Provider | Policy Number | Coverage                | Meets Current Contractual Limits  | Effective Date | Termination Date |
	| cool guy   | Boynton & Boynton  | BKW53671074   | TRANSPORTATION LEVEL 1  | Yes                               |  8/11/2020     | 10/10/2022       |
	When I follow the Show link for contractor insurance "two"
	Then I should be at the Show page for contractor insurance "two"

Scenario: Admin user can edit a contractor insurance
	Given I am logged in as "user"
	When I visit the Show page for contractor insurance "two"
	And I follow "Edit"
	Then I should be at the Edit page for contractor insurance "two"
	When I enter "BP90094946" into the PolicyNumber field 
	And I enter "Penn National Insurance Co." into the InsuranceProvider field
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for contractor insurance "two"
	And I should see a display for Contractor with contractor "cool guy" 
	And I should see a display for EffectiveDate with "8/11/2020"
	And I should see a display for TerminationDate with "10/10/2022"
	And I should see a display for PolicyNumber with "BP90094946"
	And I should see a display for InsuranceProvider with "Penn National Insurance Co."
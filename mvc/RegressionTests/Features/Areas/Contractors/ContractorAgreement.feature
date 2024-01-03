Feature: ContractorAgreement

Background:
	Given a user "user" exists with username: "user"
	And a role "contractorsagreements-useradmin" exists with action: "UserAdministrator", module: "ContractorsAgreements", user: "user"
    And a contractor "one" exists with name: "one"
	And a contractor company "one" exists with description: "NYAWC"
	And a contractor work category type "one" exists with description: "Landscaping"
	And a contractor agreement status type "one" exists with description: "Active"
	And a contractor agreement "two" exists with Contractor: "one", ContractorCompany: "one", ContractorWorkCategoryType: "one", ContractorAgreementStatusType: "one", Title: "Spoils Management", Description: "Plumbing Agreement", AgreementOwner: "Sunil", AgreementStartDate: "08/11/2020", AgreementEndDate: "10/10/2022", EstimatedContractValue: "400000.00"

Scenario: Admin user adds a new contractor agreement
	Given I am logged in as "user"
	And I am at the Contractors/ContractorAgreement/New page
	When I press Save
	Then I should see a validation message for Contractor with "The Contractor field is required."
	And I should see a validation message for Title with "The Title field is required."
	And I should see a validation message for Description with "The Description field is required."
	And I should see a validation message for ContractorCompany with "The ContractorCompany field is required."
	And I should see a validation message for ContractorWorkCategoryType with "The Agreement Category field is required."
	And I should see a validation message for ContractorAgreementStatusType with "The Agreement Status field is required."
	And I should see a validation message for AgreementOwner with "The AgreementOwner field is required."
	And I should see a validation message for AgreementStartDate with "The AgreementStartDate field is required."
	And I should see a validation message for AgreementEndDate with "The AgreementEndDate field is required."
	And I should see a validation message for EstimatedContractValue with "The EstimatedContractValue field is required."
	When I select contractor "one" from the Contractor dropdown
	And I enter "SMALL METER REPLACEMENT AGREEMENT" into the Title field
	And I enter "SMALL METER REPLACEMENT AGREEMENT" into the Description field
	And I select contractor company "one" from the ContractorCompany dropdown
	And I select contractor work category type "one" from the ContractorWorkCategoryType dropdown
	And I select contractor agreement status type "one" from the ContractorAgreementStatusType dropdown
	And I enter "Shroba" into the AgreementOwner field
	And I enter 3/17/2022 into the AgreementStartDate field
	And I enter 3/22/2022 into the AgreementEndDate field
	And I enter "10.5" into the EstimatedContractValue field
	And I press Save
	Then I should see a display for Contractor with "one"
	And I should see a display for Title with "SMALL METER REPLACEMENT AGREEMENT"
	And I should see a display for Description with "SMALL METER REPLACEMENT AGREEMENT"
	And I should see a display for ContractorCompany with "NYAWC"
	And I should see a display for ContractorWorkCategoryType with "Landscaping"
	And I should see a display for ContractorAgreementStatusType with "Active"
	And I should see a display for AgreementOwner with "Shroba"
	And I should see a display for AgreementStartDate with "3/17/2022" 
	And I should see a display for AgreementEndDate with "3/22/2022"
	And I should see a display for EstimatedContractValue with "10.5"

Scenario: Admin user can search and view contractor agreements
	Given I am logged in as "user"
	When I visit the Contractors/ContractorAgreement/Search page
	And I press Search
	And I wait for the page to reload
	Then I should see a link to the Show page for contractor agreement "two"
	And I should see the following values in the contractorAgreementsTable table
	| Contractor | Contractor Company | Agreement Category | Agreement Status | Title             | Agreement Owner | Agreement Start Date | Agreement End Date |
	| one        | NYAWC              | Landscaping        | Active           | Spoils Management | Sunil           |  8/11/2020           | 10/10/2022         |
	When I follow the Show link for contractor agreement "two"
	Then I should be at the Show page for contractor agreement "two"

Scenario: Admin user can edit a contractor agreement
	Given I am logged in as "user"
	When I visit the Show page for contractor agreement "two"
	And I follow "Edit"
	Then I should be at the Edit page for contractor agreement "two"
	When I enter "Residuals Dewatering Agreement" into the Title field 
	And I enter "Sewer Televising" into the Description field
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for contractor agreement "two"
	And I should see a display for Title with "Residuals Dewatering Agreement"
	And I should see a display for Description with "Sewer Televising"
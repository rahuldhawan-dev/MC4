Feature: Contractor

Background:
	Given a user "user" exists with username: "user"
	And a role "contractorsgeneral-useradmin" exists with action: "UserAdministrator", module: "ContractorsGeneral", user: "user"
	And a role "contractorsagreements-useradmin" exists with action: "UserAdministrator", module: "ContractorsAgreements", user: "user"
	And a state "nj" exists
	And a town "one" exists with state: "nj"
	And a street "one" exists with town: "one", is active: true
    And a contractor "one" exists with name: "Sunil", is active: "true", awr: "true"
	And a contractor company "one" exists with description: "NYAWC"
	And a contractor work category type "one" exists with description: "Landscaping"
	And a contractor agreement status type "one" exists with description: "Active"
	And a contractor agreement "one" exists with Contractor: "one", ContractorCompany: "one", ContractorWorkCategoryType: "one", ContractorAgreementStatusType: "one", Title: "Spoils Management", Description: "Plumbing Agreement", AgreementOwner: "Sunil", AgreementStartDate: "08/11/2020", AgreementEndDate: "10/10/2022", EstimatedContractValue: "400000.00"
	And a contractor insurance minimum requirement "one" exists with description: "TRANSPORTATION LEVEL 1"
	And a contractor insurance "two" exists with Contractor: "one", ContractorInsuranceMinimumRequirement: "one", InsuranceProvider: "Boynton & Boynton", MeetsCurrentContractualLimits: "true", PolicyNumber: "BKW53671074", EffectiveDate: "08/11/2020", TerminationDate: "10/10/2022"
	And an address "addy" exists with town: "one"
	And a contact "contact" exists with first name: "Tina", last name: "Belcher", email: "buns@onburner.com", address: "addy", BusinessPhoneNumber: "1234567890", MobilePhoneNumber: "9876543210", FaxNumber: "1234598760"
	And a contact type "one" exists with description: "Police Department"
	And a contractor contact "one" exists with Contractor: "one", Contact: "contact", ContactType: "one"

Scenario: Admin user adds a new contractor
	Given I am logged in as "user"
	And I am at the Contractors/Contractor/New page
	When I press Save
	Then I should see a validation message for Name with "The Name field is required."	
	When I enter "Sunil" into the Name field
	And I enter "100609" into the VendorId field
	And I select state "nj" from the State dropdown
	And I select town "one" from the Town dropdown
	And I select street "one" from the Street dropdown
	And I enter "15927" into the HouseNumber field
	And I enter "1400" into the ApartmentNumber field
	And I enter "21704" into the Zip field
	And I enter "9848032919" into the Phone field
	And I check the IsUnionShop field
	And I check the IsActive field
	And I check the IsBcpPartner field
	And I check the ContractorsAccess field
	And I check the AWR field
	And I press Save
	Then I should see a display for Name with "Sunil"
	And I should see a display for VendorId with "100609"
	And I should see a display for State with state "nj"
	And I should see a display for Town with town "one"
	And I should see a display for Street with street "one" 
	And I should see a display for HouseNumber with "15927"
	And I should see a display for ApartmentNumber with "1400"
	And I should see a display for Zip with "21704"
	And I should see a display for Phone with "9848032919"
	And I should see a display for IsUnionShop with "Yes" 
	And I should see a display for IsActive with "Yes"
	And I should see a display for IsBcpPartner with "Yes"
	And I should see a display for ContractorsAccess with "Yes"
	And I should see a display for AWR with "Yes"

Scenario: Admin user can search and view contractors
	Given I am logged in as "user"
	When I visit the Contractors/Contractor/Search page
	And I press Search
	And I wait for the page to reload
	Then I should see a link to the Show page for contractor "one"
	And I should see the following values in the contractorsTable table
	| Name  | Is Union Shop | Is BCP Partner | Is Active | Is American Water Resources contractor? | Contractors Access |
	| Sunil | No            | No             | Yes       | Yes								       | No                |
	When I follow the Show link for contractor "one"
	Then I should be at the Show page for contractor "one"

Scenario: Admin user can edit a contractor
	Given I am logged in as "user"
	When I visit the Show page for contractor "one"
	And I follow "Edit"
	Then I should be at the Edit page for contractor "one"
	When I enter "9848032919" into the Phone field 
	And I uncheck the IsUnionShop field
	And I press Save
	And I wait for the page to reload
	Then I should be at the Show page for contractor "one"
	And I should see a display for Phone with "9848032919"
	And I should see a display for IsUnionShop with "No"

Scenario: User can search for a contractor and verify contacts agreements and insurances data
	Given I am logged in as "user"
	When I visit the Contractors/Contractor/Search page
	And I press Search
	Then I should see a link to the Show page for contractor "one"
	When I follow the Show link for contractor "one"
	Then I should be at the Show page for contractor "one"
	When I click the "Contacts" tab
	Then I should see the following values in the contactsTable table
	| Full Name    | Contact Type       | Address          | Business Phone Number | Mobile Phone Number | Fax Number |
	| Tina Belcher | Police Department  | *123 Fake St*    | 1234567890            | 9876543210          | 1234598760 |	
	When I click the "Agreements" tab
	Then I should see the following values in the contractorAgreementsTable table
	| Contractor Company | Agreement Category | Agreement Status | Title             | Agreement Owner | Agreement Start Date | Agreement End Date |
	| NYAWC              | Landscaping        | Active           | Spoils Management | Sunil           |  8/11/2020           | 10/10/2022         |
	And I should see a link to the Show page for contractor agreement "one"
	When I click the "Insurance" tab
	Then I should see the following values in the contractorInsurancesTable table
	| Insurance Provider | Policy Number | Coverage                | Meets Current Contractual Limits  | Effective Date | Termination Date |
	| Boynton & Boynton  | BKW53671074   | TRANSPORTATION LEVEL 1  | Yes                               |  8/11/2020     | 10/10/2022       |
	And I should see a link to the Show page for contractor insurance "two"

Scenario: admin can delete a contractor contact
	Given I am logged in as "user"
	When I visit the Show page for contractor "one"
	And I click the "Contacts" tab
    And I click the "Remove" button in the 1st row of contactsTable and then click ok in the confirmation dialog
	And I wait for the page to reload
	And I click the "Contacts" tab
	Then I should not see "Belcher, Tina"

Scenario: admin can add a contractor contact
	Given a contact "Sunil" exists with first name: "Sai Sunil", last name: "Papineni", email: "SaiSunil.Papineni@amwater.com", address: "addy", BusinessPhoneNumber: "1234567890", MobilePhoneNumber: "9876543210", FaxNumber: "1234598760"
	And a contractor contact type "x" exists with contact type: "one"
	And I am logged in as "user"
	When I visit the Show page for contractor "one"
	And I click the "Contacts" tab
	And I press "Link Contact"
	And I enter "Sunil" and select "Papineni, Sai Sunil" from the Contact autocomplete field
	And I select contact type "one"'s Description from the ContactType dropdown
	And I press "Save Contact"
	And I wait for the page to reload
	Then I should be at the Show page for contractor "one"
	When I click the "Contacts" tab
	Then I should see a link to the Show page for contact "Sunil"
Feature: PublicWaterSupplyPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury"
	And a user "user" exists with username: "user"
	And a user "user-no-edit" exists with username: "user-no-edit"
	And a user "user-edit" exists with username: "user-edit"
	And a role "role-user" exists with action: "UserAdministrator", module: "EnvironmentalWaterSystems", user: "user", operating center: "nj7"
	And a role "role-read-user-no-edit" exists with action: "Read", module: "EnvironmentalWaterSystems", user: "user-no-edit"
	And a role "role-edit-user-edit" exists with action: "Edit", module: "EnvironmentalWaterSystems", user: "user-edit"
	And public water supply statuses exist
	And a public water supply ownership "aw contract" exists with description: "AW Contract"
	And a public water supply type "community water system" exists with description: "Community Water System"
    And a public water supply "one" exists with identifier: "NJ1345001", system: "Coastal North Monmouth/Lakewood", status: "active", ownership: "aw contract", type: "community water system"
	And operating center: "nj7" exists in public water supply: "one" 
	And a public water supply "two" exists with identifier: "NJ1350001", system: "Union Beach", status: "pending merger"
	And a public water supply "three" exists with identifier: "foo", system: "Union Beach1", Purveyor name: "Union Eastific", status: "pending merger"
	And a public water supply "five" exists with identifier: "foo2", system: "Union Beach2", Purveyor name: "Union Eastific"
	And a public water supply "four" exists with identifier: "foo3", system: "Union Beach3", Purveyor name: "Union Eastific", status: "pending merger"
	And an environmental permit status "one" exists with description: "Active"
	And an environmental permit type "one" exists with description: "Water Quality"
	And an environmental permit "one" exists with environmental permit type: "one", description: "Give a hoot- dont polute!", public water supply: "two", environmental permit status: "one", permit effective date: 10/2/2020, permit renewal date: 10/2/2020, permit expiration date: 10/2/2020 
	And operating center: "nj7" exists in public water supply: "two" 
	And operating center: "nj7" exists in public water supply: "three" 
	And operating center: "nj7" exists in public water supply: "four" 
	
Scenario: admin user can search for a public water supply
	Given I am logged in as "admin"
    When I visit the PublicWaterSupply/Search page
	Then I should see the HasConsentOrder field
	When I enter "1345" and select "NJ1345001" from the Identifier autocomplete field 
    And I press Search
	Then I should not see a validation message for HasConsentOrder with "The HasConsentOrder field is required."
	And I should see a link to the Show page for public water supply: "one"
    When I follow the Show link for public water supply "one"
    Then I should be at the Show page for public water supply: "one"

Scenario: admin user can view a public water supply
    Given the test flag "allow audits" exists
	And I am logged in as "admin"
    When I visit the Show page for public water supply: "one"
    Then I should see a display for public water supply: "one"'s Identifier
    And I should see a display for public water supply: "one"'s Ownership
    And I should see a display for public water supply: "one"'s Type
    When I click the "Log" tab
    Then I should see the following values in the LogTab table
    | User | Entity Name         | Entity | Audit Entry Type | Time Stamp (EST)| Field Name | Old Value | New Value |
    |      | PublicWaterSupply   | 1      | Show             | **              |            |           |           |

Scenario: admin user can view the environmental permits tab
	Given I am logged in as "admin"
    When I visit the Show page for public water supply: "two"
    And I click the "Environmental Permits" tab
	Then I should see the following values in the EnvironmentalPermit table
	| Permit Type   | Permit Number | Effective Date | Renewal Date | Expiration Date | Permit Status |
	| Water Quality | P#1123        | 10/2/2020      | 10/2/2020    | 10/2/2020       | Active        |

Scenario: admin user can add a public water supply
	Given I am logged in as "admin"
    When I visit the PublicWaterSupply/New page
    And I enter "foo62" into the Identifier field
	And I select public water supply status "active" from the Status dropdown
	And I enter "ABCDE12345" into the LocalCertifiedStateId field
	And I enter "3/11/2005" into the DateOfOwnership field
	And I select "Yes" from the HasConsentOrder dropdown
	When I enter "01/01/2022" into the ConsentOrderStartDate field
	And I press Save
	Then I should see the validation message The Ownership field is required.
	And I should see the validation message The Type field is required.
	And I should see a validation message for ConsentOrderEndDate with "The ConsentOrderEndDate field is required."
	When I enter "01/02/2022" into the ConsentOrderEndDate field
	And I press Save
	Then I should not see a validation message for ConsentOrderEndDate with "The ConsentOrderEndDate field is required."
	When I select public water supply ownership "aw contract" from the Ownership dropdown
	And I select public water supply type "community water system" from the Type dropdown
	And I press Save
    Then the currently shown public water supply will now be referred to as "michonne"
    And I should see a display for Identifier with "foo62"
	And I should see a display for Status with public water supply status "active"
	And I should see a display for LocalCertifiedStateId with "ABCDE12345"

Scenario: admin user can edit a public water supply
	Given I am logged in as "admin"
    When I visit the Edit page for public water supply: "one"
    And I enter "bar" into the Identifier field
	And I select public water supply status "inactive" from the Status dropdown
	And I select public water supply ownership "aw contract" from the Ownership dropdown
	And I select public water supply type "community water system" from the Type dropdown
	And I select "Yes" from the HasConsentOrder dropdown
	And I enter "01/01/2022" into the ConsentOrderStartDate field
    And I press Save
	Then I should see a validation message for ConsentOrderEndDate with "The ConsentOrderEndDate field is required."
	When I enter "01/02/2022" into the ConsentOrderEndDate field
	And I press Save
	Then I should not see a validation message for ConsentOrderEndDate with "The ConsentOrderEndDate field is required."
    And I should be at the Show page for public water supply: "one"
    And I should see a display for Identifier with "bar"
	And I should see a display for Status with public water supply status "inactive"

Scenario: admin user can add an operating center to a public water supply
	Given I am logged in as "admin"
	When I visit the Show page for public water supply: "one"
	And I click the "Operating Centers" tab
	And I press "Add New Operating Center"
	And I select operating center "nj7" from the OperatingCenter dropdown
	And I press "Add Operating Center"
	And I wait for the page to reload
	And I click the "Operating Centers" tab
	Then I should see the following values in the operatingCenterTable table
	| Operating Center | Abbreviation |
	| NJ7 - Shrewsbury |              |

Scenario: not-admin user can not add an operating center to a public water supply
	Given I am logged in as "user"
	When I visit the Show page for public water supply: "one"
	And I click the "Operating Centers" tab
	Then I should not see "Add New Operating Center"
	And I should not see the button "Remove Operating Center"

Scenario: admin user can view licensed operators on the licensed operator tab
	Given I am logged in as "admin"
	And an employee status "active" exists with description: "Active"
	And an employee "D:" exists with operating center: "nj7", status: "active"
	And a operator license type "one" exists with description: "The best license type"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a operator license "one" exists with employee: "D:", operator license type: "one", "operating center: "nj7", expiration date: "3/9/2020", validation date: "3/5/2020", license level: "1234", license sub level: "15"
	And a public water supply licensed operator "one" exists with public water supply: "one", licensed operator: "one"
	When I visit the Show page for public water supply: "one"
	And I click the "Licensed Operators" tab
	Then I should see a link to the show page for operator license "one"
	Then I should see "NJ7 - Shrewsbury" in the table OperatorLicenseTable's "Operating Center" column
	Then I should see "The best license type" in the table OperatorLicenseTable's "Operator License Type" column
	Then I should see "1234" in the table OperatorLicenseTable's "License Level/Class" column
	Then I should see "15" in the table OperatorLicenseTable's "License Sub-level/Sub-class" column
	Then I should see "1234" in the table OperatorLicenseTable's "License Number" column
	Then I should see "3/5/2020" in the table OperatorLicenseTable's "Validation Date" column
	Then I should see "3/9/2020" in the table OperatorLicenseTable's "Expiration Date" column

Scenario: admin user can add a public water supply with anticipated active date
    Given I am logged in as "admin"
	When I visit the PublicWaterSupply/New page
	Then I should not see the AnticipatedActiveDate field
	And I should see the HasConsentOrder field
    When I enter "foo8" into the Identifier field
	And I select public water supply status "active" from the Status dropdown
	And I enter "ABCDE12345" into the LocalCertifiedStateId field
	And I select public water supply ownership "aw contract" from the Ownership dropdown
	And I select public water supply type "community water system" from the Type dropdown
	Then I should not see the AnticipatedActiveDate field
	When I select public water supply status "pending" from the Status dropdown
	Then I should see the AnticipatedActiveDate field
	When I select public water supply status "inactive" from the Status dropdown
	Then I should not see the AnticipatedActiveDate field
	When I select public water supply status "inactive -see note" from the Status dropdown
	Then I should not see the AnticipatedActiveDate field
	When I select public water supply status "pending" from the Status dropdown
	Then I should see the AnticipatedActiveDate field
	When I press Save
	Then I should see the validation message The AnticipatedActiveDate field is required.
	When I enter "2/20/2020" into the AnticipatedActiveDate field
	And I select "No" from the HasConsentOrder dropdown
	And I press Save
	Then I should see a display for AnticipatedActiveDate with "2/20/2020"

Scenario: admin user can edit a public water supply with anticipated active date
    Given I am logged in as "admin"
	When I visit the Edit page for public water supply: "one"
	Then I should not see the AnticipatedActiveDate field
	And I should see the HasConsentOrder field
    When I enter "foo11" into the Identifier field
	And I select public water supply status "active" from the Status dropdown
	And I select public water supply ownership "aw contract" from the Ownership dropdown
	And I select public water supply type "community water system" from the Type dropdown
	And I enter "ABCDE12345" into the LocalCertifiedStateId field
	And I select "No" from the HasConsentOrder dropdown
	Then I should not see the AnticipatedActiveDate field
	When I select public water supply status "pending" from the Status dropdown
	Then I should see the AnticipatedActiveDate field
	When I select public water supply status "inactive" from the Status dropdown
	Then I should not see the AnticipatedActiveDate field
	When I select public water supply status "inactive -see note" from the Status dropdown
	Then I should not see the AnticipatedActiveDate field
	When I select public water supply status "pending" from the Status dropdown
	Then I should see the AnticipatedActiveDate field
	When I press Save
	Then I should see the validation message The AnticipatedActiveDate field is required.
	When I enter "2/20/2020" into the AnticipatedActiveDate field
	And I press Save
	Then I should see a display for AnticipatedActiveDate with "2/20/2020"
	#show AnticipatedActiveDate if already has value no matter what the status is 
	When I visit the Edit page for public water supply: "one"
	Then I should see the AnticipatedActiveDate field
	When I select public water supply status "inactive" from the Status dropdown
	Then I should see the AnticipatedActiveDate field

Scenario: admin user can edit a public water supply and Add PWSID for merger
	Given I am logged in as "admin"
    When I visit the Edit page for public water supply: "one"
	# Fields should be hidden by default on page load
	Then I should not see the ValidTo field
	And I should not see the ValidFrom field
	And I should not see the AnticipatedMergerDate field
	And I should not see the AnticipatedMergePublicWaterSupply field
	# And they should continue to stay hidden until "Pending Merger" is selected
	When I select public water supply status "active" from the Status dropdown
	Then I should not see the ValidTo field
	And I should not see the ValidFrom field
	And I should not see the AnticipatedMergerDate field
	And I should not see the AnticipatedMergePublicWaterSupply field
	When I select public water supply status "pending merger" from the Status dropdown
	And I select public water supply ownership "aw contract" from the Ownership dropdown
	And I select public water supply type "community water system" from the Type dropdown
	Then I should see the ValidTo field
	And I should see the ValidFrom field
	And I should see the AnticipatedMergerDate field
	And I should see the AnticipatedMergePublicWaterSupply field
	When I press Save
	Then I should see the validation message The AnticipatedMergePublicWaterSupply field is required.
	And I should see the validation message The AnticipatedMergerDate field is required.
	When I enter "2/20/2020" into the ValidFrom field
	And I enter "3/30/2020" into the ValidTo field
	And I enter "4/10/2020" into the AnticipatedMergerDate field
    And I select public water supply "three" from the AnticipatedMergePublicWaterSupply dropdown
	And I select "No" from the HasConsentOrder dropdown
	And I press Save
	Then I should see a display for ValidFrom with "2/20/2020"
	And I should see a display for ValidTo with "3/30/2020"
	And I should see a display for AnticipatedMergerDate with "4/10/2020"
	And I should see a link to the Show page for public water supply: "three"

Scenario: admin user can search for a public water supply by Ownership and Type
	Given I am logged in as "admin"
    When I visit the PublicWaterSupply/Search page
	And I select public water supply ownership "aw contract" from the Ownership dropdown
	And I select public water supply type "community water system" from the Type dropdown
    When I press Search	
    Then I should see a link to the Show page for public water supply: "one"
    And I should see the following values in the siteContent table
    |  Ownership  |           Type         |
    | AW Contract | Community Water System |
    When I follow the Show link for public water supply "one"
    Then I should be at the Show page for public water supply: "one"

Scenario: no-edit-access user can see the banner message email address on the search page
	Given I am logged in as "user-no-edit"
	When I visit the PublicWaterSupply/Search page
	Then I should see the link "pwsid@amwater.com" with the url "mailto:pwsid@amwater.com"

Scenario: edit-access user can not see the banner message email address on the search page
	Given I am logged in as "user-edit"
	When I visit the PublicWaterSupply/Search page
	Then I should not see the link "pwsid@amwater.com"

Scenario: admin user can add a planning plant to a public water supply
	Given I am logged in as "admin"
	And a planning plant "planningplantone" exists with operating center: "nj7", code: "D217", description: "My Planning Plant"
	When I visit the Show page for public water supply: "one"
	And I click the "Planning Plants" tab
	And I press "Add New Planning Plant"
	And I select "D217 - NJ7 - My Planning Plant" from the PlanningPlant dropdown
	And I press "Add Planning Plant"
	And I wait for the page to reload
	And I click the "Planning Plants" tab
	Then I should see the following values in the planningPlantTable table
	  | Planning Plant	 | 
	  | D217 - NJ7 - My Planning Plant |
   
Scenario: not-admin user can not add a planning plant to a public water supply
	Given I am logged in as "user"
	When I visit the Show page for public water supply: "one"
	And I click the "Planning Plants" tab
	Then I should not see "Add New Planning Plant"
	And I should not see the button "Remove Planning Plant" 
Feature: ServiceRestorationContractorPage

Background: there was stuff
	Given an operating center "nj4" exists with opcode: "NJ4", name: "Lakewood", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And an operating center "nj7" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", hydrantInspFreq: "1", hydrantInspFreqUnit: "Y", largeValveInspFreq: "2", largeValveInspFreqUnit: "M", smallValveInspFreq: "3", smallValveInspFreqUnit: "W", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a service restoration contractor "one" exists with contractor: "one", operating center: "nj7"
	And a service restoration contractor "two" exists with contractor: "two", operating center: "nj4"
	And an admin user "admin" exists with username: "admin"

Scenario: user can search for things
    Given I am logged in as "admin"
    And I am at the ServiceRestorationContractor/Search page
    When I enter "tw" into the Contractor_Value field
    And I press "Search"
    Then I should be at the ServiceRestorationContractor page
    And I should not see service restoration contractor "one"'s Contractor in the table serviceRestorationContractors's "Contractor" column	
    And I should see service restoration contractor "two"'s Contractor in the table serviceRestorationContractors's "Contractor" column	
    When I visit the ServiceRestorationContractor/Search page
    And I enter "" into the Contractor_Value field
    And I select operating center "nj7"'s Description from the OperatingCenter dropdown
    And I press "Search"
    Then I should be at the ServiceRestorationContractor page
    And I should see service restoration contractor "one"'s Contractor in the table serviceRestorationContractors's "Contractor" column	
    And I should not see service restoration contractor "two"'s Contractor in the table serviceRestorationContractors's "Contractor" column	

Scenario: user can create new record
    Given I am logged in as "admin"
    And I am at the ServiceRestorationContractor/New page
	When I press Save
    Then I should be at the ServiceRestorationContractor/New page
    And I should see the validation message "The Contractor field is required."
    And I should see the validation message "The OperatingCenter field is required."
    And I should see the validation message "The FinalRestoration field is required."
    And I should see the validation message "The PartialRestoration field is required."
	When I enter "foo" into the Contractor field
    And I select operating center "nj7"'s Description from the OperatingCenter dropdown
    And I select "Yes" from the FinalRestoration dropdown
    And I select "No" from the PartialRestoration dropdown
    And I press Save
    Then I should be at the ServiceRestorationContractor page

Scenario: user can edit and delete records that have no linked restorations
    Given I am logged in as "admin"
    And I am at the ServiceRestorationContractor/Search page
    When I press "Search"
    Then I should see a link to the Edit page for service restoration contractor "one"
    When I follow the Edit link for service restoration contractor "one"
    Then I should be at the Edit page for service restoration contractor "one"
    When I enter "meh" into the Contractor field
    And I press Save
    Then I should be at the ServiceRestorationContractor page
    And I should see "meh" in the table serviceRestorationContractors's "Contractor" column
    When I visit the ServiceRestorationContractor/Search page
    And I enter "meh" into the Contractor_Value field
    And I press Search
    And I click ok in the dialog after pressing "Delete"
    Then I should be at the ServiceRestorationContractor page
    And I should not see "meh" in the table serviceRestorationContractors's "Contractor" column	

Scenario: user cannot edit or delete records that have linked restorations
    Given a service restoration "one" exists with final restoration completion by: "one"
    And I am logged in as "admin"
    And I am at the ServiceRestorationContractor/Search page
	When I enter "one" into the Contractor_Value field
    And I press Search
    Then I should be at the ServiceRestorationContractor page
    And I should not see the button "Delete"
    And I should not see a link to the Edit page for service restoration contractor "one"
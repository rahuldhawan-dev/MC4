Feature: AwiaCompliances

Background: data exists
    Given a state "NJ" exists with abbreviation: "NJ"
    And an operating center "oc-01" exists with opcode: "NJ4", name: "Shrewsbury", state: "NJ"
    And a public water supply "pws-01" exists with identifier: "NJ1345001", system: "Coastal North Monmouth/Lakewood"
    And operating center: "oc-01" exists in public water supply: "pws-01"
    And a awia compliance certification type "awia-01" exists
    And an employee "e-01" exists with operating center: "oc-01", employee id: "12345678"    
    And a user "user-unauthorized" exists with username: "user-unauthorized"
    And a user "user-admin" exists with username: "user-admin", employee: "e-01", full name: "Boaty McBoatface"
    And a user "testuser" exists with username: "stuff", default operating center: "oc-01", full name: "Smith"	
    And a role "role-read" exists with action: "Read", module: "EngineeringRiskRegister", user: "testuser", operating center: "oc-01"
    And a role "role-admin" exists with action: "UserAdministrator", module: "EngineeringRiskRegister", user: "user-admin", operating center: "oc-01"
    And a awia compliance "aca" exists with State: "NJ", OperatingCenter: "oc-01", CertificationType: "awia-01", CreatedBy: "testuser", CertifiedBy: "testuser", DateSubmitted: "07/11/2022", DateAccepted: "07/19/2022", RecertificationDue: "07/29/2022"

Scenario: Validation messages display if a awia compliance form is entered incorrectly
    Given I am logged in as "user-admin"
    When I visit the Engineering/AwiaCompliance/New page
    And I press Save
    Then I should see a validation message for State with "The State field is required."    
    And I should see a validation message for CertificationType with "The CertificationType field is required."    
    And I should see a validation message for DateSubmitted with "The DateSubmitted field is required."
    And I should see a validation message for DateAccepted with "The Certification Due field is required."
    And I should see a validation message for RecertificationDue with "The RecertificationDue field is required."
    When I select "NJ" from the State dropdown
    And I press Save
    Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."    
    When I select operating center "oc-01" from the OperatingCenter dropdown
    And I press Save
    Then I should see a validation message for CertifiedBy with "The CertifiedBy field is required."
    When I select "Smith" from the CertifiedBy dropdown
    And I press Save
    Then I should see a validation message for PublicWaterSupplies with "The PWSID field is required."
    When I enter "today's date" into the DateSubmitted field
    And I enter "today's date" into the DateAccepted field
    And I enter "today's date" into the RecertificationDue field
    And I select awia compliance certification type "awia-01" from the CertificationType dropdown
    And I select public water supply "pws-01" from the PublicWaterSupplies dropdown
    And I press Save
    And I wait for the page to reload
    Then I should see a display for State with "NJ"
    And I should see a display for OperatingCenter with operating center "oc-01"
    And I should see a display for CertificationType with awia compliance certification type "awia-01"
    And I should see a display for CertifiedBy_FullName with "Smith"
    And I should see a display for DateSubmitted with "today's date"
    And I should see a display for DateAccepted with "today's date"
    And I should see a display for RecertificationDue with "today's date"
    And I should see a display for CreatedBy_FullName with "Boaty McBoatface"

Scenario: Unauthorized users cannot access any pages
    Given I am logged in as "user-unauthorized"
    And I am at the Engineering/AwiaCompliance/Search page
    Then I should see "You do not have the roles necessary to access this resource."
    When I visit the Engineering/AwiaCompliance/New page
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Show page for awia compliance: "aca"
    Then I should see "You do not have the roles necessary to access this resource."
    When I try to visit the Edit page for awia compliance: "aca"
    Then I should see "You do not have the roles necessary to access this resource."

Scenario: can search for a awia compliance
    Given I am logged in as "user-admin"
	When I visit the /Engineering/AwiaCompliance/Search page
	And I select "NJ" from the State dropdown
    And I press Search
    Then I should see a link to the Show page for awia compliance: "aca"
	When I follow the Show link for awia compliance "aca"
    Then I should be at the Show page for awia compliance: "aca"
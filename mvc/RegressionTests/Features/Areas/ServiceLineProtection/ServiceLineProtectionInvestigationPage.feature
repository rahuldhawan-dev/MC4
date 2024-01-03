Feature: ServiceLineProtectionInvestigationPage

Background: data exists
    Given an admin user "admin" exists with username: "admin"
	And an operating center "one" exists with opcode: "NJ7", name: "Shrewsbury", companyInfo: "Some Info", phone: "123-234-5678", fax: "987-654-3210", mailingName: "The Mailing Address Name", mailingStreet: "The Mailing Street", mailingCSZ: "The Mailing Town ST 12345", servicePhone: "000-000-0000", permitsOmUserName: "john", permitsCapitalUserName: "not john", workOrdersEnabled: "true"
	And a state "one" exists with name: "New Jersey", abbreviation: "NJ"
	And a county "one" exists with name: "Monmouth", state: "one"
	And a town "one" exists with name: "Loch Arbour", county: "one"
	And a street "one" exists with town: "one", full st name: "Easy St", is active: true
	And operating center: "one" exists in town: "one"
	And a contractor "one" exists with operating center: "one", awr: true
	And a service material "one" exists with description: "copper"
	And a service size "one" exists with service size description: "1/2", size: "0.5"
	And a service line protection work type "one" exists with description: "Replace"
	And a service line protection investigation status "one" exists 
	And a service line protection investigation status "two" exists 
	And a service line protection investigation status "three" exists 
	And a service line protection investigation status "four" exists 
	And a coordinate "one" exists 
    And I am logged in as "admin"

Scenario: user can search for a service line protection investigation
	Given a service line protection investigation "one" exists with operating center: "one", customer city: "one", work type: "one"
    When I visit the ServiceLineProtection/ServiceLineProtectionInvestigation/Search page
    And I press Search
    Then I should see a link to the Show page for service line protection investigation: "one"
    When I follow the Show link for service line protection investigation "one"
    Then I should be at the Show page for service line protection investigation: "one"

Scenario: user can view a service line protection investigation
    Given a service line protection investigation "one" exists with operating center: "one", customer city: "one", work type: "one"
	When I visit the Show page for service line protection investigation: "one"
    Then I should see a display for service line protection investigation: "one"'s CustomerName

Scenario: user can add a service line protection investigation
    When I visit the ServiceLineProtection/ServiceLineProtectionInvestigation/New page
	And I press Save
	Then I should see a validation message for CustomerName with "The CustomerName field is required."
	And I should see a validation message for StreetNumber with "The StreetNumber field is required."
	And I should see a validation message for State with "The State field is required."
	And I should see a validation message for PremiseNumber with "The PremiseNumber field is required."
	And I should see a validation message for CustomerServiceSize with "The CustomerServiceSize field is required."
	When I select state "one" from the State dropdown
	And I press Save
	Then I should see a validation message for CustomerCity with "The CustomerCity field is required."
	When I select town "one"'s NameWithCounty from the CustomerCity dropdown
	And I select "-- Select --" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for OperatingCenter with "The OperatingCenter field is required."
	And I should see a validation message for Street with "The Street field is required."
	When I select operating center "one" from the OperatingCenter dropdown
	And I press Save
	Then I should see a validation message for Contractor with "The Contractor field is required."
	When I enter "foo" into the CustomerName field
	And I enter "123" into the StreetNumber field
	And I select street "one" from the Street dropdown
	And I enter "Suite 2A" into the CustomerAddress2 field
	And I enter "12356-1234" into the CustomerZip field
	And I enter "7328675309" into the CustomerPhone field
	And I enter "1234567890" into the PremiseNumber field
	And I enter "867" into the AccountNumber field
	And I select service material "one" from the CustomerServiceMaterial dropdown
	And I select service size "one" from the CustomerServiceSize dropdown
	And I select service line protection work type "one" from the WorkType dropdown
	And I select contractor "one" from the Contractor dropdown
	And I enter coordinate "one"'s Id into the Coordinate field
    And I press Save
    Then the currently shown service line protection investigation will now be referred to as "lime"
    And I should see a display for CustomerName with "foo"
	And I should see a display for StreetNumber with "123"
	And I should see a display for Street with street "one"
	And I should see a display for CustomerAddress2 with "Suite 2A"
	And I should see a display for CustomerCity with town "one"
	And I should see a display for CustomerPhone with "7328675309"
	And I should see a display for CustomerZip with "12356-1234" 
	And I should see a display for PremiseNumber with "1234567890" 
	And I should see a display for AccountNumber with "867" 
	And I should see a display for OperatingCenter with operating center "one"
	And I should see a display for CustomerServiceMaterial with service material "one"
	And I should see a display for CustomerServiceSize with service size "one"
	And I should see a display for WorkType with service line protection work type "one"
	
Scenario: user can edit a service line protection investigation
	Given a service line protection investigation "edit" exists with operating center: "one", customer city: "one", work type: "one", premise number: "1234567890", customer service size: "one", contractor: "one", street: "one"
    When I visit the Edit page for service line protection investigation: "edit"
    And I press Save
	Then I should see a validation message for DateInstalled with "The DateInstalled field is required."
	And I should see a validation message for CompanyServiceMaterial with "The CompanyServiceMaterial field is required."
    When I enter "12/8/1980" into the DateInstalled field
	And I select service material "one" from the CompanyServiceMaterial dropdown
	And I select service size "one" from the CompanyServiceSize dropdown
    And I press Save
	Then I should be at the Show page for service line protection investigation: "edit"
	And I should see a display for DateInstalled with "12/8/1980"
	And I should see a display for CompanyServiceMaterial with service material "one"
	And I should see a display for CompanyServiceSize with service size "one"

Scenario: user can edit a service line protection investigation and selects a service
	Given a service line protection investigation "edit" exists with operating center: "one", customer city: "one", work type: "one", premise number: "1234567890", customer service size: "one", contractor: "one", street: "one"
	And a service "one" exists with service number: "123", operating center: "one", town: "one", street: "one", service material: "one", premise number: "456", length of service: "4", service size: "one", date installed: "12/8/1980"
    When I visit the Edit page for service line protection investigation: "edit"
    And I press Save
	Then I should see a validation message for DateInstalled with "The DateInstalled field is required."
	And I should see a validation message for CompanyServiceMaterial with "The CompanyServiceMaterial field is required."
    When I select street "one" from the Street dropdown
	And I wait for ajax to finish loading
	And I select service "one" from the Service dropdown
	And I wait for ajax to finish loading
    And I press Save
	Then I should be at the Show page for service line protection investigation: "edit"
	And I should see a display for DateInstalled with "12/8/1980"
	And I should see a display for CompanyServiceMaterial with service material "one"
	And I should see a display for CompanyServiceSize with service size "one"